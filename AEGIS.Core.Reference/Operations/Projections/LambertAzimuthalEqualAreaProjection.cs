/// <copyright file="LambertAzimuthalEqualAreaProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

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
        protected Double _latitudeOfNaturalOrigin;
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;

        // projection constants
        protected Double _D;
        protected Double _RQ;
        protected Double _betaO;
        protected Double _qP;
        protected Double _qO;

        // aspect
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
            :this(identifier, name, CoordinateOperationMethods.LambertAzimuthalEqualAreaProjection, parameters, ellipsoid, areaOfUse)
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

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).BaseValue;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).BaseValue;

            _qP = (1 - _ellipsoid.EccentricitySquare) * ((1 / (1 - _ellipsoid.EccentricitySquare)) - ((1 / (2 * _ellipsoid.Eccentricity)) * Math.Log((1 - _ellipsoid.Eccentricity) / (1 + _ellipsoid.Eccentricity), Math.E)));
            _qO = (1 - _ellipsoid.EccentricitySquare) * ((Math.Sin(_latitudeOfNaturalOrigin) / (1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfNaturalOrigin))) - ((1 / (2 * _ellipsoid.Eccentricity)) * Math.Log((1 - _ellipsoid.Eccentricity * Math.Sin(_latitudeOfNaturalOrigin)) / (1 + _ellipsoid.Eccentricity * Math.Sin(_latitudeOfNaturalOrigin)), Math.E)));
            _betaO = Math.Asin(_qO / _qP);
            _RQ = _ellipsoid.SemiMajorAxis.BaseValue * Math.Sqrt(_qP / 2);
            _D = _ellipsoid.SemiMajorAxis.BaseValue * (Math.Cos(_latitudeOfNaturalOrigin) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfNaturalOrigin))) / (_RQ * Math.Cos(_betaO));

            if (Math.Abs(_latitudeOfNaturalOrigin - Angles.NorthPole.BaseValue) <= Calculator.Tolerance)
                _operationAspect = OperationAspect.NorthPolar;
            else if (Math.Abs(_latitudeOfNaturalOrigin - Angles.SouthPole.BaseValue) <= Calculator.Tolerance)
                _operationAspect = OperationAspect.SouthPolar;
            else if (_ellipsoid.IsSphere && Math.Abs(_latitudeOfNaturalOrigin - Angles.Equator.BaseValue) <= Calculator.Tolerance)
                _operationAspect = OperationAspect.Equatorial;
            else
                _operationAspect = OperationAspect.Oblique;
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
            Double easting = 0, northing = 0;

            if (_ellipsoid.IsSphere)
            {
                // source: Snyder, J. P.: Map Projections - A Working Manual, page 185
                Double k;

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        easting = 2 * _ellipsoid.RadiusOfAuthalicSphere * Math.Sin(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                        northing = -2 * _ellipsoid.RadiusOfAuthalicSphere * Math.Sin(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                        break;
                    case OperationAspect.SouthPolar:
                        easting = 2 * _ellipsoid.RadiusOfAuthalicSphere * Math.Cos(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                        northing = 2 * _ellipsoid.RadiusOfAuthalicSphere * Math.Cos(Math.PI / 4 - coordinate.Latitude.BaseValue / 2) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                        break;
                    case OperationAspect.Equatorial:
                        k = Math.Sqrt(2 / (1 + Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin)));
                        easting = _ellipsoid.RadiusOfAuthalicSphere * k * Math.Cos(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                        northing = _ellipsoid.RadiusOfAuthalicSphere * k * Math.Sin(coordinate.Latitude.BaseValue);
                        break;
                    case OperationAspect.Oblique:
                        k = Math.Sqrt(2 / (1 + Math.Sin(_latitudeOfNaturalOrigin) * Math.Sin(coordinate.Latitude.BaseValue) + Math.Cos(_latitudeOfNaturalOrigin) * Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin)));
                        easting = _ellipsoid.RadiusOfAuthalicSphere * k * Math.Cos(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                        northing = _ellipsoid.RadiusOfAuthalicSphere * k * (Math.Cos(_latitudeOfNaturalOrigin) * Math.Sin(coordinate.Latitude.BaseValue) - Math.Sin(_latitudeOfNaturalOrigin) * Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin));
                        break;
                }
            }
            else
            {
                // source: EPSG Guidance Note number 7, part 2, page 72

                Double q = (1 - _ellipsoid.EccentricitySquare) * ((Math.Sin(coordinate.Latitude.BaseValue) / (1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(coordinate.Latitude.BaseValue))) - ((1 / (2 * _ellipsoid.Eccentricity)) * Math.Log((1 - _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)), Math.E)));

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        Double rho = _ellipsoid.SemiMajorAxis.BaseValue * Math.Sqrt(_qP - q);

                        easting = _falseEasting + (rho * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin));
                        northing = _falseNorthing - (rho * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin));
                        break;
                    case OperationAspect.SouthPolar:
                        Double ro = _ellipsoid.SemiMajorAxis.BaseValue * Math.Sqrt(_qP + q);

                        easting = _falseEasting + (ro * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin));
                        northing = _falseNorthing + (ro * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin));
                        break;
                    case OperationAspect.Oblique:
                        Double beta = Math.Asin(q / _qP);
                        Double b = _RQ * Math.Sqrt((2 / (1 + Math.Sin(_betaO) * Math.Sin(beta) + (Math.Cos(_betaO) * Math.Cos(beta) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin)))));

                        easting = _falseEasting + ((b * _D) * (Math.Cos(beta) * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin)));
                        northing = _falseNorthing + ((b / _D) * ((Math.Cos(_betaO) * Math.Sin(beta)) - (Math.Sin(_betaO) * Math.Cos(beta) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin))));
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
            Double phi = 0, lambda = 0;

            if (_ellipsoid.IsSphere)
            {
                // source: Snyder, J. P.: Map Projections - A Working Manual, page 185

                Double rho = Math.Pow(Math.Pow(coordinate.X, 2) + Math.Pow(coordinate.Y, 2), 1 / 2);
                Double c = 2 * Math.Asin(rho / (2 * _ellipsoid.RadiusOfAuthalicSphere));
                phi = Math.Asin(Math.Cos(c) * Math.Sin(_latitudeOfNaturalOrigin) + (coordinate.Y * Math.Sin(c) * Math.Cos(_latitudeOfNaturalOrigin / rho)));

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        lambda = _longitudeOfNaturalOrigin + Math.Atan(coordinate.X / (-coordinate.Y));
                        break;
                    case OperationAspect.SouthPolar:
                        lambda = _longitudeOfNaturalOrigin + Math.Atan(coordinate.X / coordinate.Y);
                        break;
                    case OperationAspect.Oblique:
                        lambda = _longitudeOfNaturalOrigin + Math.Atan(coordinate.X * Math.Sin(c) / (rho * Math.Cos(_latitudeOfNaturalOrigin) * Math.Cos(c) - coordinate.Y * Math.Sin(_latitudeOfNaturalOrigin) * Math.Sin(c)));
                        break;
                }
            }
            else
            {
                // source: EPSG Guidance Note number 7, part 2, page 72

                Double rho = Math.Pow((Math.Pow((coordinate.X - _falseEasting) / _D, 2) + Math.Pow(_D * (coordinate.Y - _falseNorthing), 2)), 0.5);
                Double c = 2 * Math.Asin(rho / (2 * _RQ));
                Double beta = 0;

                switch (_operationAspect)
                {
                    case OperationAspect.NorthPolar:
                        beta = Math.Asin(1 - Math.Pow(rho, 2) / (Math.Pow(_ellipsoid.SemiMajorAxis.BaseValue, 2) * (1 - ((1 - _ellipsoid.EccentricitySquare) / 2 * _ellipsoid.Eccentricity) * Math.Log((1 - _ellipsoid.Eccentricity) / (1 + _ellipsoid.Eccentricity), Math.E))));
                        lambda = _longitudeOfNaturalOrigin + Math.Atan((coordinate.X - _falseEasting) / (_falseNorthing - coordinate.Y));
                        break;
                    case OperationAspect.SouthPolar:
                        beta = -Math.Asin(1 - Math.Pow(rho, 2) / (Math.Pow(_ellipsoid.SemiMajorAxis.BaseValue, 2) * (1 - ((1 - _ellipsoid.EccentricitySquare) / 2 * _ellipsoid.Eccentricity) * Math.Log((1 - _ellipsoid.Eccentricity) / (1 + _ellipsoid.Eccentricity), Math.E))));
                        lambda = _longitudeOfNaturalOrigin + Math.Atan((coordinate.X - _falseEasting) / (coordinate.Y - _falseNorthing));
                        break;
                    case OperationAspect.Oblique:
                        beta = Math.Asin((Math.Cos(c) * Math.Sin(_betaO)) + ((_D * (coordinate.Y - _falseNorthing) * Math.Sin(c) * Math.Cos(_betaO)) / rho));
                        lambda = _longitudeOfNaturalOrigin + Math.Atan((coordinate.X - _falseEasting) * Math.Sin(c) / (_D * rho * Math.Cos(_betaO) * Math.Cos(c) - Math.Pow(_D, 2) * (coordinate.Y - _falseNorthing) * Math.Sin(_betaO) * Math.Sin(c)));
                        break;
                }

                phi = beta + ((_ellipsoid.EccentricitySquare / 3 + 31 * Math.Pow(_ellipsoid.EccentricitySquare, 2) / 180 + 517 * Math.Pow(_ellipsoid.EccentricitySquare, 3) / 5040) * Math.Sin(2 * beta)) + ((23 * Math.Pow(_ellipsoid.EccentricitySquare, 2) / 360 + 251 * Math.Pow(_ellipsoid.EccentricitySquare, 3) / 3780) * Math.Sin(4 * beta)) + ((761 * Math.Pow(_ellipsoid.EccentricitySquare, 3) / 45360) * Math.Sin(6 * beta));
            }

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}
