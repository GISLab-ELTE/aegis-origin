/// <copyright file="LambertConicNearConformalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Near-Conformal projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9817", "Lambert Conic Near-Conformal")]
    public class LambertConicNearConformalProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin;
        protected Double _longitudeOfNaturalOrigin;
        protected Double _scaleFactorAtNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _A;
        protected Double _a;
        protected Double _b;
        protected Double _c;
        protected Double _d;
        protected Double _e;
        protected Double _s0;
        protected Double _r0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicNearConformalProjection" /> class.
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
        public LambertConicNearConformalProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.LambertConicNearConformalProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 22

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _scaleFactorAtNaturalOrigin = ((Double)_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).BaseValue;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).BaseValue;

            Double n = _ellipsoid.Flattening / (2 - _ellipsoid.Flattening);
            _A = 1 / (6 * _ellipsoid.RadiusOfMeridianCurvature(_latitudeOfNaturalOrigin) * _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfNaturalOrigin));
            _a = _ellipsoid.SemiMajorAxis.BaseValue * (1 - n + 5 * (Math.Pow(n, 2) - Math.Pow(n, 3)) / 4 + 81 * (Math.Pow(n, 4) - Math.Pow(n, 5)) / 64) * Constants.DegreeToRadian;
            _b = 3 * _ellipsoid.SemiMajorAxis.BaseValue * (n - Math.Pow(n, 2) + 7 * (Math.Pow(n, 3) - Math.Pow(n, 4)) / 8 + 55 * Math.Pow(n, 5) / 64) / 2;
            _c = 15 * _ellipsoid.SemiMajorAxis.BaseValue * (Math.Pow(n, 2) - Math.Pow(n, 3) + 3 * (Math.Pow(n, 4) - Math.Pow(n, 5)) / 4) / 16;
            _d = 35 * _ellipsoid.SemiMajorAxis.BaseValue * (Math.Pow(n, 3) - Math.Pow(n, 4) + 11 * Math.Pow(n, 5) / 16) / 48;
            _e = 315 * _ellipsoid.SemiMajorAxis.BaseValue * (Math.Pow(n, 4) - Math.Pow(n, 5)) / 512;
            _r0 = _scaleFactorAtNaturalOrigin * _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfNaturalOrigin) / Math.Tan(_latitudeOfNaturalOrigin);
            _s0 = _a * (_latitudeOfNaturalOrigin * Constants.RadianToDegree) - _b * Math.Sin(2 * _latitudeOfNaturalOrigin) + _c * Math.Sin(4 * _latitudeOfNaturalOrigin) - _d * Math.Sin(6 * _latitudeOfNaturalOrigin) + _e * Math.Sin(8 * _latitudeOfNaturalOrigin);
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
            // source: EPSG Guidance Note number 7, part 2, page 22

            Double s = _a * (coordinate.Latitude.BaseValue * Constants.RadianToDegree) - _b * Math.Sin(2 * coordinate.Latitude.BaseValue) + _c * Math.Sin(4 * coordinate.Latitude.BaseValue) - _d * Math.Sin(6 * coordinate.Latitude.BaseValue) + _e * Math.Sin(8 * coordinate.Latitude.BaseValue);
            Double m = s - _s0;
            Double M = _scaleFactorAtNaturalOrigin * (m + _A * Math.Pow(m, 3));
            Double r = _r0 - M;
            Double theta = (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin) * Math.Sin(_latitudeOfNaturalOrigin);

            Double easting = _falseEasting + r * Math.Sin(theta);
            Double northing = _falseNorthing + M + r * Math.Sin(theta) * Math.Tan(theta/2);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 23

            Double theta_ = Math.Atan((coordinate.X - _falseEasting) / (_r0 - (coordinate.Y - _falseNorthing)));
            Double r_ = Math.Sign(_latitudeOfNaturalOrigin) * Math.Sqrt(Calculator.Square(coordinate.X - _falseEasting) + Calculator.Square(_r0 - (coordinate.Y - _falseNorthing)));
            Double M_ = _r0 - r_;

            Double m_ = M_ - (M_ - _scaleFactorAtNaturalOrigin * M_ - _scaleFactorAtNaturalOrigin * _A * Math.Pow(M_,3)) / (-1 * _scaleFactorAtNaturalOrigin - 3 * _scaleFactorAtNaturalOrigin * _A * Math.Pow(M_,2));
            Double phi_ = _latitudeOfNaturalOrigin + m_ / _a * Constants.DegreeToRadian;
            Double s_ = _a * (phi_ * Constants.RadianToDegree) - _b * Math.Sin(2 * phi_) + _c * Math.Sin(4 * phi_) - _d * Math.Sin(6 * phi_) + _e * Math.Sin(8 * phi_);
            Double ds_ = _a * Constants.RadianToDegree - 2 * _b * Math.Cos(2 * phi_) + 4 * _c * Math.Cos(4 * phi_) - 6 * _d * Math.Cos(6 * phi_) + 8 * _e * Math.Cos(8 * phi_);

            Double phi = phi_ - ((m_ + _s0 - s_) / (-1 * ds_));
            Double lambda = _longitudeOfNaturalOrigin + theta_ / Math.Sin(_latitudeOfNaturalOrigin);

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}
