/// <copyright file="BonneProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Krisztián Fodor</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Bonne projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9827", "Bonne")]
    public class BonneProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _falseEasting; // projection params
        protected Double _falseNorthing;
        protected Double _latitudeOfNaturalOrigin;
        protected Double _longitudeOfNaturalOrigin;

        protected Double _e4; // ellipsoid eccentricities
        protected Double _e6;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        public BonneProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, CoordinateOperationMethods.Bonne, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        protected BonneProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;
            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;

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
            // source: EPSG Guidance Note number 7, part 2, page 79

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double m = Computem(phi);
            Double m0 = Computem(_latitudeOfNaturalOrigin);
            Double M = ComputeM(phi);
            Double M0 = ComputeM(_latitudeOfNaturalOrigin);

            Double rho = _ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) + M0 - M;
            Double T = _ellipsoid.SemiMajorAxis.Value * m * (lambda - _longitudeOfNaturalOrigin) / rho;

            Double easting = (rho * Math.Sin(T)) + _falseEasting;
            Double northing = (_ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) - rho * Math.Cos(T)) + _falseNorthing;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 79

            Double x = coordinate.X - _falseEasting;
            Double y = coordinate.Y - _falseNorthing;

            return ComputeReverseInternal(x, y);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the m value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The m value.</returns>
        protected Double Computem(Double latitude)
        {
            return Math.Cos(latitude) / Math.Pow(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(latitude), 0.5);
        }

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M value.</returns>
        protected Double ComputeM(Double latitude)
        {
            return _ellipsoid.SemiMajorAxis.BaseValue * ((1 - _ellipsoid.EccentricitySquare / 4 - 3 * _e4 / 64 - 5 * _e6 / 256) * latitude -
                                                        (3 * _ellipsoid.EccentricitySquare / 8 + 3 * _e4 / 32 + 45 * _e6 / 1024) * Math.Sin(2 * latitude) +
                                                        (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Sin(4 * latitude) -
                                                        35 * _e6 / 3072 * Math.Sin(6 * latitude));
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="X">The X coordinate.</param>
        /// <param name="Y">The Y coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected GeoCoordinate ComputeReverseInternal(Double X, Double Y)
        {
            Double m0 = Computem(_latitudeOfNaturalOrigin);
            Double M0 = ComputeM(_latitudeOfNaturalOrigin);

            Double rho = Math.Sign(_latitudeOfNaturalOrigin) *
                Math.Pow((Calculator.Pow(X, 2) + Calculator.Pow(_ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) - Y, 2)), 0.5);
            Double M = _ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) + M0 - rho;
            Double mu = M / (_ellipsoid.SemiMajorAxis.Value * (1 - _ellipsoid.EccentricitySquare / 4 - 3 * _e4 / 64 - 5 * _e6 / 256));
            Double e1 = (1 - Math.Pow(1 - _ellipsoid.EccentricitySquare, 0.5)) / (1 + Math.Pow(1 - _ellipsoid.EccentricitySquare, 0.5));
            Double phi = mu +
                         (3 * e1 / 2 - 27 * Math.Pow(e1, 3) / 32) * Math.Sin(2 * mu) +
                         (21 * Calculator.Pow(e1, 2) / 16 - 55 * Calculator.Pow(e1, 4) / 32) * Math.Sin(4 * mu) +
                         (151 * Calculator.Pow(e1, 3) / 96) * Math.Sin(6 * mu) +
                         (1097 * Calculator.Pow(e1, 4) / 512) * Math.Sin(8 * mu);
            Double m = Computem(phi);

            if (Math.Abs(Math.Abs(phi) - Math.PI / 2) < Calculator.Tolerance) // m == 0 case
            {
                return new GeoCoordinate(phi, _longitudeOfNaturalOrigin);
            }

            Double lambda;
            if (_latitudeOfNaturalOrigin >= 0)
            {
                lambda = _longitudeOfNaturalOrigin + rho *
                         Math.Atan(X / _ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) - Y) /
                         _ellipsoid.SemiMajorAxis.Value * m;
            }
            else
            {
                lambda = _longitudeOfNaturalOrigin + rho *
                         Math.Atan(-X / (Y - _ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin))) /
                         _ellipsoid.SemiMajorAxis.Value * m;
            }

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}
