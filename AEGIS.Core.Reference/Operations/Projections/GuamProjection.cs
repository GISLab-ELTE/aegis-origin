/// <copyright file="GuamProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Guam projection.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::9831", "Guam Projection")]
    public class GuamProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // projection params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _M0; // projection constants
        protected Double _E1;
        protected Double _e2; // ellipsoid eccentricities
        protected Double _e4;
        protected Double _e6;
        protected Double _e8;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GuamProjection" /> class.
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
        /// The parameter is not a double percision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public GuamProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.GuamProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 81

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _e2 = Calculator.Pow(_ellipsoid.Eccentricity, 2);
            _e4 = Calculator.Pow(_ellipsoid.Eccentricity, 4);
            _e6 = Calculator.Pow(_ellipsoid.Eccentricity, 6);
            _e8 = Calculator.Pow(_ellipsoid.Eccentricity, 8);

            _M0 = ComputeM(_latitudeOfNaturalOrigin);
            _E1 = (1 - Math.Sqrt(1 - _e2)) / (1 + Math.Sqrt(1 - _e2));
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 81

            Double x = _ellipsoid.SemiMajorAxis.BaseValue * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin) * Math.Cos(coordinate.Latitude.BaseValue) /
                       Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(coordinate.Latitude.BaseValue));

            Double M = ComputeM(coordinate.Latitude.BaseValue);

            Double easting = _falseEasting + x;
            Double northing = _falseNorthing + M - _M0 + (Calculator.Pow(x, 2) * Math.Tan(coordinate.Latitude.BaseValue) * Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(coordinate.Latitude.BaseValue)) / (2 * _ellipsoid.SemiMajorAxis.BaseValue));
            
            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 81

            Double M, mu, phi = _latitudeOfNaturalOrigin;
   
            for (Int32 i = 0; i < 3; i++)
            {
                M = _M0 + (coordinate.Y - _falseNorthing) - (Calculator.Pow((coordinate.X - _falseEasting), 2) * Math.Tan(phi) *
                           Math.Sqrt(1 - _e2 * Calculator.Sin2(phi)) / (2 * _ellipsoid.SemiMajorAxis.BaseValue));

                mu = M / _ellipsoid.SemiMajorAxis.BaseValue * (1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256);

                phi = mu + (3 * _E1 / 2 - 27 * Calculator.Pow(_E1, 3) / 32) * Math.Sin(2 * mu) + (21 * Calculator.Pow(_E1, 2) / 16 - 55 * Calculator.Pow(_E1, 4) / 32) *
                           Math.Sin(4 * mu) + (151 * Calculator.Pow(_E1, 3) / 96) * Math.Sin(6 * mu) + (1097 * Calculator.Pow(_E1, 4) / 512) * Math.Sin(8 * mu);
            }

            Double lambda = _longitudeOfNaturalOrigin + ((coordinate.X - _falseEasting) * Math.Sqrt(1 - _e2 * Calculator.Sin2(phi)) / (_ellipsoid.SemiMajorAxis.BaseValue * Math.Cos(phi)));

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
