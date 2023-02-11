// <copyright file="AmericanPolyconicProjection.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents an American Polyconic projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9818", "American Polyconic")]
    public class AmericanPolyconicProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // projection params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _M0; // projection constant
        protected Double _e2; // ellipsoid eccentricities
        protected Double _e4;
        protected Double _e6;
        protected Double _e8;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmericanPolyconicProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public AmericanPolyconicProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.AmericanPolyconicProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 71

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _e2 = Calculator.Pow(_ellipsoid.Eccentricity, 2);
            _e4 = Calculator.Pow(_ellipsoid.Eccentricity, 4);
            _e6 = Calculator.Pow(_ellipsoid.Eccentricity, 6);
            _e8 = Calculator.Pow(_ellipsoid.Eccentricity, 8);
            _M0 = ComputeM(_latitudeOfNaturalOrigin);
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        /// <exception cref="System.ArgumentException">The specified coordinate is not within range of the projection.;coordinate</exception>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            if (Math.Abs(coordinate.Latitude.BaseValue - _latitudeOfNaturalOrigin) > Math.PI / 2)
                throw new ArgumentException("The coordinate is not within the range of the transformation.", "coordinate");

            // source: EPSG Guidance Note number 7, part 2, page 71

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double L = (lambda - _longitudeOfNaturalOrigin) * Math.Sin(phi);
            Double M = ComputeM(phi);

            Double easting, northing;
            if (coordinate.Latitude.BaseValue == 0)
            {
                easting = _falseEasting + _ellipsoid.SemiMajorAxis.BaseValue * (lambda - _longitudeOfNaturalOrigin);
                northing = _falseNorthing - _M0;
            }
            else
            {
                easting = _falseEasting + _ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Calculator.Cot(phi) * Math.Sin(L);
                northing = _falseNorthing + M - _M0 + _ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Calculator.Cot(phi) * (1 - Math.Cos(L));
            }

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 72

            Double phi, lambda;

            if (_M0 == (coordinate.Y - _falseNorthing))
            {
                phi = 0;
                lambda = _longitudeOfNaturalOrigin + (coordinate.X - _falseEasting) / _ellipsoid.SemiMajorAxis.BaseValue;
            }
            else
            {
                Double A = (_M0 + coordinate.Y - _falseNorthing) / _ellipsoid.SemiMajorAxis.BaseValue;
                Double B = Math.Pow(A, 2) + Calculator.Pow(coordinate.X - _falseEasting, 2) / Calculator.Pow(_ellipsoid.SemiMajorAxis.BaseValue, 2);

                Double C, H, M, J, phi1, phi2 = A;

                Int32 n = 1;
                do {
                    phi1 = phi2;
                    C = Math.Sqrt(1 - _e2 * Calculator.Sin2(phi1)) * Math.Tan(phi1);
                    M = ComputeM(phi1);

                    // source for H: Snyder, J.P.: Map Projections - A Working Manual, page 130
                    
                    H = 1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256
                           - 2 * (3 * _e2 / 8 + 3 *_e4 / 32 + 45 * _e6 / 1024) * Math.Cos(2 * phi1)
                           + 4 * (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Cos(4 * phi1)
                           - 6 * (35 * _e6 / 3072) * Math.Cos(6 * phi1);
                    J = M / _ellipsoid.SemiMajorAxis.BaseValue;

                    phi2 =  phi1 - (A * (C * J + 1) - J - C * (Calculator.Pow(J, 2) + B) / 2) /
                                   (_e2 * Math.Sin(2 * phi1) * (Calculator.Pow(J, 2) + B - 2 * A * J) / (4 * C) + (A - J) * (C * H - 2 / Math.Sin(2 * phi1)) - H);
                    n++;
                } while (n < Calculator.IterationLimit && Math.Abs(phi2 - phi1) > Calculator.Tolerance);

                // check for convergence
                if (n == Calculator.IterationLimit)
                    throw new ArgumentException("The coordinate is not within the range of the transformation.", "coordinate");

                phi = phi2;
                lambda = _longitudeOfNaturalOrigin + (Math.Asin((coordinate.X-_falseEasting) * C / _ellipsoid.SemiMajorAxis.BaseValue)) / Math.Sin(phi);
            }

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M Value.</returns>
        protected Double ComputeM(Double latitude)
        {
            return _ellipsoid.SemiMajorAxis.BaseValue * ((1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256) * latitude -
                                                         (3 * _e2 / 8 + 3 * _e4 / 32 + 45 * _e6 / 1024) * Math.Sin(2 * latitude) +
                                                         (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Sin(4 * latitude) -
                                                         35 * _e6 / 3072 * Math.Sin(6 * latitude));
        }

        #endregion
    }
}
