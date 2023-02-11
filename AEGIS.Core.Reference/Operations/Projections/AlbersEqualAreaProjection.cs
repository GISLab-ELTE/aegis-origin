// <copyright file="AlbersEqualAreaProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Management;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents an Albers Equal Area projection.
    /// </summary>
    /// <author>Krisztián Fodor</author>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9822", "Albers Equal Area")]
    public class AlbersEqualAreaProjection : CoordinateProjection
    {
        #region Private fields

        private Double _eastingAtFalseOrigin; // projection params
        private Double _northingAtFalseOrigin;
        private Double _latitudeOfFalseOrigin;
        private Double _latitudeOf1stStandardParallel;
        private Double _latitudeOf2ndStandardParallel;
        private Double _longitudeOfFalseOrigin;

        private Double _e; // ellipsoid eccentricities
        private Double _e2;
        private Double _e4;
        private Double _e6;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbersEqualAreaProjection"/> class.
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
        public AlbersEqualAreaProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.AlbersEqualAreaProjection, parameters, ellipsoid, areaOfUse)
        {
            _eastingAtFalseOrigin = ((Length)_parameters[CoordinateOperationParameters.EastingAtFalseOrigin]).BaseValue;
            _northingAtFalseOrigin = ((Length)_parameters[CoordinateOperationParameters.NorthingAtFalseOrigin]).BaseValue;
            _latitudeOfFalseOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfFalseOrigin]).BaseValue;
            _latitudeOf1stStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
            _latitudeOf2ndStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf2ndStandardParallel]).BaseValue;
            _longitudeOfFalseOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfFalseOrigin]).BaseValue;

            _e = _ellipsoid.Eccentricity;
            _e2 = _ellipsoid.EccentricitySquare;
            _e4 = Calculator.Pow(_ellipsoid.Eccentricity, 4);
            _e6 = Calculator.Pow(_ellipsoid.Eccentricity, 6);
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
            // source: EPSG Guidance Note number 7, part 2, page 75

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double m1 = ComputeM(_latitudeOf1stStandardParallel);
            Double m2 = ComputeM(_latitudeOf2ndStandardParallel);
            Double alpha = ComputeAlpha(phi);
            Double alpha0 = ComputeAlpha(_latitudeOfFalseOrigin);
            Double alpha1 = ComputeAlpha(_latitudeOf1stStandardParallel);
            Double alpha2 = ComputeAlpha(_latitudeOf2ndStandardParallel);
            Double n = ComputeN(m1, m2, alpha1, alpha2);
            Double C = ComputeC(m1, n, alpha1);

            Double theta = n * (lambda - _longitudeOfFalseOrigin);
            Double rho = (_ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(C - n * alpha, 0.5)) / n;
            Double rho0 = (_ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(C - n * alpha0, 0.5)) / n;

            Double easting = _eastingAtFalseOrigin + (rho * Math.Sin(theta));
            Double northing = _northingAtFalseOrigin + rho0 - (rho * Math.Cos(theta));

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 75

            Double m1 = ComputeM(_latitudeOf1stStandardParallel);
            Double m2 = ComputeM(_latitudeOf2ndStandardParallel);
            //Double alpha = ComputeAlpha(phi);
            Double alpha0 = ComputeAlpha(_latitudeOfFalseOrigin);
            Double alpha1 = ComputeAlpha(_latitudeOf1stStandardParallel);
            Double alpha2 = ComputeAlpha(_latitudeOf2ndStandardParallel);
            Double n = ComputeN(m1, m2, alpha1, alpha2);
            Double C = ComputeC(m1, n, alpha1);

            Double rho0 = (_ellipsoid.SemiMajorAxis.BaseValue * Math.Pow(C - n * alpha0, 0.5)) / n;

            // coordinate.Y == N , coordinate.X == E
            Double rho = Math.Pow(Calculator.Pow(coordinate.X - _eastingAtFalseOrigin, 2) + Calculator.Pow(rho0 - (coordinate.Y - _northingAtFalseOrigin), 2), 0.5);
            Double theta = Math.Atan((coordinate.X - _eastingAtFalseOrigin) / (rho0 - (coordinate.Y - _northingAtFalseOrigin)));
            Double alpha_ = (C - (Calculator.Pow(rho, 2) * Calculator.Pow(n, 2) / Calculator.Pow(_ellipsoid.SemiMajorAxis.BaseValue, 2))) / n;
            Double beta_ = Math.Asin(alpha_ / (1 - ((1 - _e2) / (2 * _e))) * Math.Log((1 - _e) / (1 + _e))); // possible incorrect formula
            // beta’ = asin(alpha' / {1 – [(1 – e2) / (2 e)]} ln [(1 – e) / (1 + e)])


            Double lambda = _longitudeOfFalseOrigin + (theta / n);
            Double phi = beta_ + (((_e2 / 3) + (31 * _e4 / 180) + (517 * _e6 / 5040)) * Math.Sin(2 * beta_)) +
                                 (((23 * _e4 / 360) + (251 * _e6 / 3780)) * Math.Sin(4 * beta_)) +
                                 ((761 * _e6 / 45360) * Math.Sin(6 * beta_));
            // phi = ß' + [(e2/3 + 31e4/180 + 517e6/5040) sin 2ß'] + [(23e4/360 + 251e6/3780) sin 4ß'] + [(761e6/45360) sin 6ß']

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the Alpha value.
        /// </summary>
        /// <param name="phi">The latitude (expressed in radian).</param>
        /// <returns>The Alpha value.</returns>
        protected Double ComputeAlpha(Double phi)
        {
            return (1 - _e2) * ((Math.Sin(phi) / (1 - _e2 * Calculator.Sin2(phi))) - (1 / (2 * _e)) * Math.Log((1 - _e * Math.Sin(phi)) / (1 + _e * Math.Sin(phi))));
        }

        /// <summary>
        /// Computes the m value.
        /// </summary>
        /// <param name="phi">The latitude (expressed in radian).</param>
        /// <returns>The m value.</returns>
        protected Double ComputeM(Double phi)
        {
            return Math.Cos(phi) / Math.Pow(1 - _e2 * Calculator.Sin2(phi), 0.5);
        }

        /// <summary>
        /// Computes the C value.
        /// </summary>
        /// <param name="m1">The m1.</param>
        /// <param name="n">The n.</param>
        /// <param name="alpha1">The alpha1.</param>
        /// <returns>The Alpha value.</returns>
        protected Double ComputeC(Double m1, Double n, Double alpha1)
        {
            return Calculator.Pow(m1, 2) + (n * alpha1);
        }

        /// <summary>
        /// Computes the Alpha value.
        /// </summary>
        /// <param name="m1">The latitude (expressed in radian).</param>
        /// <param name="m2">The latitude (expressed in radian).</param>
        /// <param name="alpha1">The latitude (expressed in radian).</param>
        /// <param name="alpha2">The latitude (expressed in radian).</param>
        /// <returns>The Alpha value.</returns>
        protected Double ComputeN(Double m1, Double m2, Double alpha1, Double alpha2)
        {
            return (Calculator.Pow(m1, 2) - Calculator.Pow(m2, 2)) / (alpha2 - alpha1);
        }

        #endregion
    }
}
