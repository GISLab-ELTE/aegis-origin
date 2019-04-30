/// <copyright file="CassiniSoldnerProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
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
    /// Represents a Cassini-Soldner projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9806", "Cassini-Soldner")]
    public class CassiniSoldnerProjection : CoordinateProjection
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
        /// Initializes a new instance of the <see cref="CassiniSoldnerProjection" /> class.
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
        public CassiniSoldnerProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.CassiniSoldnerProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 40

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _e2 = Calculator.Pow(_ellipsoid.Eccentricity, 2);
            _e4 = Calculator.Pow(_ellipsoid.Eccentricity, 4);
            _e6 = Calculator.Pow(_ellipsoid.Eccentricity, 6);
            _e8 = Calculator.Pow(_ellipsoid.Eccentricity, 8);
            _M0 = _ellipsoid.SemiMajorAxis.Value * ((1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256) * _latitudeOfNaturalOrigin -
                                                       (3 * _e2 / 8 + 3 * _e4 / 32 + 45 * _e6 / 1024) * Math.Sin(2 * _latitudeOfNaturalOrigin) +
                                                       (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Sin(4 * _latitudeOfNaturalOrigin) -
                                                       35 * _e6 / 3072 * Math.Sin(6 * _latitudeOfNaturalOrigin));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CassiniSoldnerProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
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
        protected CassiniSoldnerProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 40

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _e2 = Calculator.Pow(_ellipsoid.Eccentricity, 2);
            _e4 = Calculator.Pow(_ellipsoid.Eccentricity, 4);
            _e6 = Calculator.Pow(_ellipsoid.Eccentricity, 6);
            _e8 = Calculator.Pow(_ellipsoid.Eccentricity, 8);
            _M0 = _ellipsoid.SemiMajorAxis.Value * ((1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256) * _latitudeOfNaturalOrigin -
                                                       (3 * _e2 / 8 + 3 * _e4 / 32 + 45 * _e6 / 1024) * Math.Sin(2 * _latitudeOfNaturalOrigin) +
                                                       (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Sin(4 * _latitudeOfNaturalOrigin) -
                                                       35 * _e6 / 3072 * Math.Sin(6 * _latitudeOfNaturalOrigin));
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
            // source: EPSG Guidance Note number 7, part 2, page 40

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double M = _ellipsoid.SemiMajorAxis.Value * ((1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256) * phi -
                                                         (3 * _e2 / 8 + 3 * _e4 / 32 + 45 * _e6 / 1024) * Math.Sin(2 * phi) +
                                                         (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Sin(4 * phi) -
                                                         35 * _e6 / 3072 * Math.Sin(6 * phi));

            Double C = _e2 * Calculator.Cos2(phi) / (1 - _e2);
            Double T = Calculator.Tan2(phi);
            Double A = (lambda - _longitudeOfNaturalOrigin) * Math.Cos(phi);
            Double X = M - _M0 + _ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Math.Tan(phi) * (Calculator.Square(A) / 2 + (5 - T + 6 * C) * Calculator.Pow(A, 4) / 24);

            Double easting = _falseEasting + _ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * (A - T * Calculator.Pow(A, 3) / 6 - (8 - T + 8 * C) * T * Calculator.Pow(A, 5) / 120);
            Double northing = ComputeNorthing(coordinate, X);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 40

            Double M1 = ComputeM1(coordinate);
            Double nu1 = M1 / (_ellipsoid.SemiMajorAxis.Value * (1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256));
            Double e1 = (1 - Math.Sqrt(1 - _e2)) / (1 + Math.Sqrt(1 - _e2));
            Double phi1 = nu1 + (3 * e1 / 2 - 27 * Calculator.Pow(e1, 3) / 32) * Math.Sin(2 * nu1) +
                                (21 * Calculator.Pow(e1, 2) / 16 - 55 * Calculator.Pow(e1, 4) / 32) * Math.Sin(4 * nu1) + 
                                (151 * Calculator.Pow(e1, 3) / 96) * Math.Sin(6 * nu1) + (1097 * Calculator.Pow(e1, 4) / 512) * Math.Sin(8 * nu1);
            Double D = (coordinate.X - _falseEasting) / _ellipsoid.RadiusOfPrimeVerticalCurvature(phi1);
            Double T1 = Calculator.Tan2(phi1);

            Double phi = phi1 - (_ellipsoid.RadiusOfPrimeVerticalCurvature(phi1) * Math.Tan(phi1) / _ellipsoid.RadiusOfMeridianCurvature(phi1)) * (Calculator.Square(D) / 2 - (1 + 3 * T1) * Calculator.Pow(D, 4) / 24);
            Double lambda = _longitudeOfNaturalOrigin + (D - T1 * Calculator.Pow(D, 3) / 3 + (1 + 3 * T1) * T1 * Calculator.Pow(D, 5) / 15) / Math.Cos(phi1);

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the northing.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="x">The X value.</param>
        /// <returns>The northing.</returns>
        protected virtual Double ComputeNorthing(GeoCoordinate coordinate, Double x)
        {
            return _falseNorthing + x;
        }

        /// <summary>
        /// Computes the M1 parameter.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The M1 value.</returns>
        protected virtual Double ComputeM1(Coordinate coordinate)
        {
            return _M0 + (coordinate.Y - _falseNorthing);
        }

        #endregion
    }
}
