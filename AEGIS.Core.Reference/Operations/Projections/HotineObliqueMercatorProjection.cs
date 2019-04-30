/// <copyright file="HotineObliqueMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Hotine Oblique Mercator projection.
    /// </summary>
    public abstract class HotineObliqueMercatorProjection : ObliqueMercatorProjection
    {
        #region Protected fields

        protected readonly Double _angleFromRectifiedToSkewGrid; // projection params
        protected readonly Double _a; // projection constants
        protected readonly Double _h;
        protected readonly Double _gammaO;
        protected readonly Double _lambdaO;
        protected readonly Double _uC;
        protected readonly Double[] _inverseParams;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
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
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        protected HotineObliqueMercatorProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 56
            _angleFromRectifiedToSkewGrid = ((Angle)_parameters[CoordinateOperationParameters.AngleFromRectifiedToSkewGrid]).BaseValue;

            _a = _ellipsoid.SemiMajorAxis.Value * _b * _scaleFactorOnInitialLine * Math.Sqrt(1 - _ellipsoid.EccentricitySquare) / (1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfProjectionCentre));
            Double tO = Math.Tan(Constants.PI / 4 - _latitudeOfProjectionCentre / 2) / Math.Pow((1 - _ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)) / (1 + _ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)), _ellipsoid.Eccentricity / 2);
            Double d = _b * Math.Sqrt(1 - _ellipsoid.EccentricitySquare) / Math.Cos(_latitudeOfProjectionCentre) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfProjectionCentre));
            Double f = d + Math.Sqrt(Math.Max(d * d, 1) - 1) * Math.Sign(_latitudeOfProjectionCentre) * Math.Sign(_latitudeOfProjectionCentre);
            _h = f * Math.Pow(tO, _b);
            Double g = (f - 1 / f) / 2;

            _gammaO = Math.Asin(Math.Sin(_azimuthOfInitialLine) / d);
            _lambdaO = _longitudeOfProjectionCentre - Math.Asin(g * Math.Tan(_gammaO)) / _b;

            if (_azimuthOfInitialLine == Math.PI)
            {
                _uC = _ellipsoid.SemiMajorAxis.Value * (_longitudeOfProjectionCentre - _lambdaO);
            }
            else
            {
                _uC = (Math.Abs(_azimuthOfInitialLine - Constants.PI / 2) <= Calculator.Tolerance) ?
                      _a * (_longitudeOfProjectionCentre - _lambdaO) :
                      (_a / _b) * Math.Atan(Math.Sqrt(Math.Max(d * d, 1) - 1) / Math.Cos(_azimuthOfInitialLine)) * Math.Sign(_latitudeOfProjectionCentre);
            }

            _inverseParams = new Double[] 
            {
                _ellipsoid.EccentricitySquare / 2 + 5 * Calculator.Pow(_ellipsoid.Eccentricity, 4) / 24 + Calculator.Pow(_ellipsoid.Eccentricity, 6) / 12 + 13 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 360,
                7 * Calculator.Pow(_ellipsoid.Eccentricity, 4) / 48 + 29 * Calculator.Pow(_ellipsoid.Eccentricity, 6) / 240 + 811 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 11520,
                7 * Calculator.Pow(_ellipsoid.Eccentricity, 6) / 120 + 81 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 1120,
                4279 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 161280
            };
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the u and v values.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="u">The u value.</param>
        /// <param name="v">The v value.</param>
        protected void ComputeUV(Double latitude, Double longitude, out Double u, out Double v)
        {
            // source: EPSG Guidance Note number 7, part 2, page 56

            Double t = Math.Tan(Constants.PI / 4 - latitude / 2) / Math.Pow((1 - _ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 + _ellipsoid.Eccentricity * Math.Sin(latitude)), _ellipsoid.Eccentricity / 2);
            Double q = _h / Math.Pow(t, _b);
            Double S = (q - 1 / q) / 2;
            Double T = (q + 1 / q) / 2;
            Double V = Math.Sin(_b * (longitude - _lambdaO));
            Double U = (-V * Math.Cos(_gammaO) + S * Math.Sin(_gammaO)) / T;

            u = ComputeU(S, V, longitude);
            v = _a * Math.Log((1 - U) / (1 + U)) / (2 * _b);
        }

        /// <summary>
        /// Computes the u value.
        /// </summary>
        /// <param name="s">The S value.</param>
        /// <param name="v">The V value.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The u value.</returns>
        protected abstract Double ComputeU(Double s, Double v, Double longitude);

        /// <summary>
        /// Computes the latitude and longitude.
        /// </summary>
        /// <param name="u">The u value.</param>
        /// <param name="v">The v value.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        protected void ComputeLatitudeLongitude(Double u, Double v, out Double latitude, out Double longitude)
        {
            // source: EPSG Guidance Note number 7, part 2, page 56

            Double q = Math.Exp(-_b * v / _a);
            Double S = (q - 1 / q) / 2;
            Double T = (q + 1 / q) / 2;
            Double V = Math.Sin(_b * u / _a);
            Double U = (V * Math.Cos(_gammaO) + S * Math.Sin(_gammaO)) / T;
            Double t = Math.Pow(_h / Math.Sqrt((1 + U) / (1 - U)), 1 / _b);
            Double chi = Constants.PI / 2 - 2 * Math.Atan(t);

            latitude = chi + _inverseParams[0] * Math.Sin(2 * chi) + _inverseParams[1] * Math.Sin(4 * chi) + _inverseParams[2] * Math.Sin(6 * chi) + _inverseParams[3] * Math.Sin(8 * chi);
            longitude = _lambdaO - Math.Atan((S * Math.Cos(_gammaO) - V * Math.Sin(_gammaO)) / Math.Cos(_b * u / _a)) / _b;
        }

        #endregion
    }
}
