/// <copyright file="TransverseMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Management;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Transverse Mercator Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9807", "Transverse Mercator")]
    public class TransverseMercatorProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // projection params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _scaleFactorAtNaturalOrigin;
        protected Double _B; // projection constants
        protected Double[] _h;
        protected Double[] _hInverse;
        protected Double _MO;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorProjection" /> class.
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
        public TransverseMercatorProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.TransverseMercatorProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 47

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;
            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);

            Double n = _ellipsoid.Flattening / (2 - _ellipsoid.Flattening);
            _B = _ellipsoid.SemiMajorAxis.BaseValue / (1 + n) * (1 + Calculator.Square(n) / 4 + Calculator.Pow(n, 4) / 64);
            _h = new Double[]
            {
                n / 2 - 2 * Calculator.Square(n) / 3 + 5 * Calculator.Pow(n, 3) / 16 + 41 * Calculator.Pow(n, 4) / 180,
                13 * Calculator.Square(n) / 48 - 3 * Calculator.Pow(n, 3) / 5 + 437 * Calculator.Pow(n, 4) / 1440,
                17 * Calculator.Pow(n, 3) / 480 - 37 * Calculator.Pow(n, 4) / 840,
                4397 * Calculator.Pow(n, 4) / 161280
            };

            _hInverse = new Double[]
            {
                n / 2 - 2 * Calculator.Square(n) / 3 + 37 * Calculator.Pow(n, 3) / 96 + 1 * Calculator.Pow(n, 4) / 360,
                1 * Calculator.Square(n) / 48 - 1 * Calculator.Pow(n, 3) / 15 + 557 * Calculator.Pow(n, 4) / 1440,
                61 * Calculator.Pow(n, 3) / 240 - 103 * Calculator.Pow(n, 4) / 140,
                49561 * Calculator.Pow(n, 4) / 161280
            };

            if (_latitudeOfNaturalOrigin == 0)
                _MO = 0;
            else if (Math.Abs(_latitudeOfNaturalOrigin - Constants.PI / 2) <= Calculator.Tolerance)
                _MO = _B * Constants.PI / 2;
            else if (Math.Abs(_latitudeOfNaturalOrigin + Constants.PI / 2) <= Calculator.Tolerance)
                _MO = -_B * Constants.PI / 2;
            else
            {
                Double qO = Calculator.Asinh(Math.Tan(_latitudeOfNaturalOrigin)) - _ellipsoid.Eccentricity * Calculator.Atanh(_ellipsoid.Eccentricity * Math.Sin(_latitudeOfNaturalOrigin));
                Double xiO0 = Math.Atan(Math.Sinh(qO));
                _MO = _B * (xiO0 + _h[0] * Math.Sin(2 * xiO0) + _h[1] * Math.Sin(4 * xiO0) + _h[2] * Math.Sin(6 * xiO0) + _h[3] * Math.Sin(8 * xiO0));
            }
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
            // source: EPSG Guidance Note number 7, part 2, page 47

            Double q = Calculator.Asinh(Math.Tan(coordinate.Latitude.BaseValue)) - _ellipsoid.Eccentricity * Calculator.Atanh(_ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue));
            Double beta = Math.Atan(Math.Sinh(q));
            Double eta0 = Calculator.Atanh(Math.Cos(beta) * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin));
            Double xi0 = Math.Asin(Math.Sin(beta) * Math.Cosh(eta0));

            Double eta = eta0 + _h[0] * Math.Cos(2 * xi0) * Math.Sinh(2 * eta0) + _h[1] * Math.Cos(4 * xi0) * Math.Sinh(4 * eta0) + _h[2] * Math.Cos(6 * xi0) * Math.Sinh(6 * eta0) + _h[3] * Math.Cos(8 * xi0) * Math.Sinh(8 * eta0);
            Double xi = xi0 + _h[0] * Math.Sin(2 * xi0) * Math.Cosh(2 * eta0) + _h[1] * Math.Sin(4 * xi0) * Math.Cosh(4 * eta0) + _h[2] * Math.Sin(6 * xi0) * Math.Cosh(6 * eta0) + _h[3] * Math.Sin(8 * xi0) * Math.Cosh(8 * eta0);

            return new Coordinate(_falseEasting + _scaleFactorAtNaturalOrigin * _B * eta, _falseNorthing + _scaleFactorAtNaturalOrigin * (_B * xi - _MO));
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 47

            Double eta = (coordinate.X - _falseEasting) / (_B * _scaleFactorAtNaturalOrigin);
            Double xi = (coordinate.Y - _falseNorthing + _scaleFactorAtNaturalOrigin * _MO) / (_B * _scaleFactorAtNaturalOrigin);

            Double xi0 = xi - _hInverse[0] * Math.Sin(2 * xi) * Math.Cosh(2 * eta) - _hInverse[1] * Math.Sin(4 * xi) * Math.Cosh(4 * eta) - _hInverse[2] * Math.Sin(6 * xi) * Math.Cosh(6 * eta) - _hInverse[3] * Math.Sin(8 * xi) * Math.Cosh(8 * eta);
            Double eta0 = eta - _hInverse[0] * Math.Cos(2 * xi) * Math.Sinh(2 * eta) - _hInverse[1] * Math.Cos(4 * xi) * Math.Sinh(4 * eta) - _hInverse[2] * Math.Cos(6 * xi) * Math.Sinh(6 * eta) - _hInverse[3] * Math.Sinh(8 * xi) * Math.Sinh(8 * eta);

            Double beta = Math.Asin(Math.Sin(xi0) / Math.Cosh(eta0));

            Double q1 = Calculator.Asinh(Math.Tan(beta));
            Double q2 = q1 + (_ellipsoid.Eccentricity * Calculator.Atanh(_ellipsoid.Eccentricity * Math.Tanh(q1)));

            for (Int32 i = 0; i < 1E4 && q2 < q1 && Math.Abs(q1 - q2) > 1E-4; i++)
            {
                q2 = q1 + (_ellipsoid.Eccentricity * Calculator.Atanh(_ellipsoid.Eccentricity * Math.Tanh(q2)));
            }
            return new GeoCoordinate(Math.Atan(Math.Sinh(q2)), _longitudeOfNaturalOrigin + Math.Asin(Math.Tanh(eta0) / Math.Cos(beta)));
        }

        #endregion
    }
}
