// <copyright file="LambertAzimuthalEqualAreaProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Azimuthal Equal Area Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::9820", "Lambert Azimuthal Equal Area Projection")]
    public class LambertAzimuthalEqualAreaProjection : CoordinateProjection
    {
        #region Protected fields

        // projection params
        protected readonly double _latO;
        protected readonly double _lonO;
        protected readonly double _FE;
        protected readonly double _FN;
        protected readonly double _e;
        protected readonly double _e2;
        protected readonly double _a;
        protected readonly double _ras;

        // projection constants
        protected readonly double _D;
        protected readonly double _Rq;
        protected readonly double _betaO;
        protected readonly double _qP;
        protected readonly double _qO;

        protected OperationAspect _operationAspect;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertAzimuthalEqualAreaProjection" /> class.
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
        public LambertAzimuthalEqualAreaProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertAzimuthalEqualAreaProjection" /> class.
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
        protected LambertAzimuthalEqualAreaProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // EPSG Guidance Note number 7, part 2, page 72
            // Also here: https://epsg.io/9820-method

            _latO = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _lonO = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _FE = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).BaseValue;
            _FN = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).BaseValue;

            // Short variable names to improve readability
            _e = ellipsoid.Eccentricity;
            _e2 = ellipsoid.EccentricitySquare;
            _a = _ellipsoid.SemiMajorAxis.BaseValue;
            _ras = _ellipsoid.RadiusOfAuthalicSphere;

            _qP = (1 - _e2) * ((1 / (1 - _e2)) - (1 / (2 * _e) * Math.Log((1 - _e) / (1 + _e), Math.E)));
            _qO = (1 - _e2) * ((Math.Sin(_latO) / (1 - _e2 * Calculator.Sin2(_latO))) - (1 / (2 * _e) * Math.Log((1 - _e * Math.Sin(_latO)) / (1 + _e * Math.Sin(_latO)), Math.E)));
            _betaO = Math.Asin(_qO / _qP);
            _Rq = _a * Math.Sqrt(_qP / 2);
            _D = _a * (Math.Cos(_latO) / Math.Sqrt(1 - _e2 * Calculator.Sin2(_latO))) / (_Rq * Math.Cos(_betaO));

            if (Math.Abs(_latO - Angles.NorthPole.BaseValue) <= Calculator.Tolerance)
            {
                _operationAspect = OperationAspect.NorthPolar;
            }
            else if (Math.Abs(_latO - Angles.SouthPole.BaseValue) <= Calculator.Tolerance)
            {
                _operationAspect = OperationAspect.SouthPolar;
            }
            else if (_ellipsoid.IsSphere && Math.Abs(_latO - Angles.Equator.BaseValue) <= Calculator.Tolerance)
            {
                _operationAspect = OperationAspect.Equatorial;
            }
            else
            {
                _operationAspect = OperationAspect.Oblique;
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
            double easting = 0;
            double northing = 0;
            double lat = coordinate.Latitude.BaseValue;
            double lon = coordinate.Longitude.BaseValue;

            if (_ellipsoid.IsSphere)
            {
                // source: Snyder, J. P.: Map Projections - A Working Manual, page 185
                double k;

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        easting = 2 * _ras * Math.Sin(Math.PI / 4 - lat / 2) * Math.Sin(lon - _lonO);
                        northing = -2 * _ras * Math.Sin(Math.PI / 4 - lat / 2) * Math.Cos(lon - _lonO);
                        break;
                    case OperationAspect.SouthPolar:
                        easting = 2 * _ras * Math.Cos(Math.PI / 4 - lat / 2) * Math.Sin(lon - _lonO);
                        northing = 2 * _ras * Math.Cos(Math.PI / 4 - lat / 2) * Math.Cos(lon - _lonO);
                        break;
                    case OperationAspect.Equatorial:
                        k = Math.Sqrt(2 / (1 + Math.Cos(lat) * Math.Cos(lon - _lonO)));
                        easting = _ras * k * Math.Cos(lat) * Math.Sin(lon - _lonO);
                        northing = _ras * k * Math.Sin(lat);
                        break;
                    case OperationAspect.Oblique:
                        k = Math.Sqrt(2 / (1 + Math.Sin(_latO) * Math.Sin(lat) + Math.Cos(_latO) * Math.Cos(lat) * Math.Cos(lon - _lonO)));
                        easting = _ras * k * Math.Cos(lat) * Math.Sin(lon - _lonO);
                        northing = _ras * k * (Math.Cos(_latO) * Math.Sin(lat) - Math.Sin(_latO) * Math.Cos(lat) * Math.Cos(lon - _lonO));
                        break;
                }
            }
            else
            {
                // source: EPSG Guidance Note number 7, part 2, page 72

                double q = (1 - _e2) * ((Math.Sin(lat) / (1 - _e2 * Calculator.Sin2(lat))) - ((1 / (2 * _e)) * Math.Log((1 - _e * Math.Sin(lat)) / (1 + _e * Math.Sin(lat)), Math.E)));

                double rho;
                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        rho = _a * Math.Sqrt(_qP - q);
                        easting = _FE + (rho * Math.Sin(lon - _lonO));
                        northing = _FN - (rho * Math.Cos(lon - _lonO));
                        break;

                    case OperationAspect.SouthPolar:
                        rho = _a * Math.Sqrt(_qP + q);
                        easting = _FE + (rho * Math.Sin(lon - _lonO));
                        northing = _FN + (rho * Math.Cos(lon - _lonO));
                        break;

                    case OperationAspect.Oblique:
                        double beta = Math.Asin(q / _qP);
                        double b = _Rq * Math.Sqrt((2 / (1 + Math.Sin(_betaO) * Math.Sin(beta) + (Math.Cos(_betaO) * Math.Cos(beta) * Math.Cos(lon - _lonO)))));
                        easting = _FE + ((b * _D) * (Math.Cos(beta) * Math.Sin(lon - _lonO)));
                        northing = _FN + ((b / _D) * ((Math.Cos(_betaO) * Math.Sin(beta)) - (Math.Sin(_betaO) * Math.Cos(beta) * Math.Cos(lon - _lonO))));
                        break;
                }
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
            double phi = 0;
            double lambda = 0;
            double x = coordinate.X;
            double y = coordinate.Y;

            if (_ellipsoid.IsSphere)
            {
                // source: Snyder, J. P.: Map Projections - A Working Manual, page 185

                double rho = Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 1 / 2);
                double c = 2 * Math.Asin(rho / (2 * _ras));
                phi = Math.Asin(Math.Cos(c) * Math.Sin(_latO) + (y * Math.Sin(c) * Math.Cos(_latO / rho)));

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        lambda = _lonO + Math.Atan(x / (-y));
                        break;
                    case OperationAspect.SouthPolar:
                        lambda = _lonO + Math.Atan(x / y);
                        break;
                    case OperationAspect.Oblique:
                        lambda = _lonO + Math.Atan(x * Math.Sin(c) / (rho * Math.Cos(_latO) * Math.Cos(c) - y * Math.Sin(_latO) * Math.Sin(c)));
                        break;
                }
            }
            else
            {
                // source: EPSG Guidance Note number 7, part 2, page 72

                double rho = Math.Pow((Math.Pow((x - _FE) / _D, 2) + Math.Pow(_D * (y - _FN), 2)), 0.5);
                double c = 2 * Math.Asin(rho / (2 * _Rq));
                double beta = 0;

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        beta = Math.Asin(1 - Math.Pow(rho, 2) / (Math.Pow(_a, 2) * (1 - ((1 - _e2) / 2 * _e) * Math.Log((1 - _e) / (1 + _e), Math.E))));
                        lambda = _lonO + Math.Atan((x - _FE) / (_FN - y));
                        break;
                    case OperationAspect.SouthPolar:
                        beta = -Math.Asin(1 - Math.Pow(rho, 2) / (Math.Pow(_a, 2) * (1 - ((1 - _e2) / 2 * _e) * Math.Log((1 - _e) / (1 + _e), Math.E))));
                        lambda = _lonO + Math.Atan((x - _FE) / (y - _FN));
                        break;
                    case OperationAspect.Oblique:
                        beta = Math.Asin((Math.Cos(c) * Math.Sin(_betaO)) + ((_D * (y - _FN) * Math.Sin(c) * Math.Cos(_betaO)) / rho));
                        lambda = _lonO + Math.Atan((x - _FE) * Math.Sin(c) / (_D * rho * Math.Cos(_betaO) * Math.Cos(c) - Math.Pow(_D, 2) * (y - _FN) * Math.Sin(_betaO) * Math.Sin(c)));
                        break;
                }

                phi = beta + ((_e2 / 3 + 31 * Math.Pow(_e2, 2) / 180 + 517 * Math.Pow(_e2, 3) / 5040) * Math.Sin(2 * beta)) + ((23 * Math.Pow(_e2, 2) / 360 + 251 * Math.Pow(_e2, 3) / 3780) * Math.Sin(4 * beta)) + ((761 * Math.Pow(_e2, 3) / 45360) * Math.Sin(6 * beta));
            }

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}
