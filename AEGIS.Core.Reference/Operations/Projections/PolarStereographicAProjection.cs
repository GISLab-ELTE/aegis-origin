/// <copyright file="PolarStereographicAProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Polar Stereographic (Variant A) Projection.
    /// </summary>
    [IdentifiedObjectInstance("EPSG::9810", "Polar Stereographic (variant A)")]
    public class PolarStereographicAProjection : PolarStereographicProjection
    {
        #region Private fields

        private readonly Double _latitudeOfNaturalOrigin; // projection params
        private readonly Double _longitudeOfNaturalOrigin;
        private readonly Double _scaleFactorAtNaturalOrigin;
        private readonly Double _falseEasting;
        private readonly Double _falseNorthing;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicAProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
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
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double percision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public PolarStereographicAProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.PolarStereographicAProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 65

            if (Math.Abs(((Angle)parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue) - Angles.NorthPole.BaseValue > Calculator.Tolerance)
            {
                throw new ArgumentException("The latitude of natural origin is not a pole.", "parameters");
            }
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _operationAspect = (_latitudeOfNaturalOrigin >= 0) ? OperationAspect.NorthPolar : OperationAspect.SouthPolar;
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
            // source: EPSG Guidance Note number 7, part 2, page 66
            Double easting = 0, northing = 0;
            Double t = ComputeT(coordinate.Latitude.BaseValue);
            Double phi = 2 * _ellipsoid.SemiMajorAxis.BaseValue * _scaleFactorAtNaturalOrigin * t / Math.Sqrt(Math.Pow(1 + _ellipsoid.Eccentricity, 1 + _ellipsoid.Eccentricity) * Math.Pow(1 - _ellipsoid.Eccentricity, 1 - _ellipsoid.Eccentricity));

            switch (_operationAspect)
            {
                case OperationAspect.NorthPolar:
                    easting = _falseEasting + phi * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                    northing = _falseNorthing - phi * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                    break;
                case OperationAspect.SouthPolar:
                    easting = _falseEasting + phi * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                    northing = _falseNorthing + phi * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                    break;
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
            // source: EPSG Guidance Note number 7, part 2, page 66
            Double phi = Math.Sqrt(Calculator.Square(coordinate.X - _falseEasting) + Calculator.Square(coordinate.Y - _falseNorthing));
            Double t = phi * Math.Sqrt(Math.Pow(1 + _ellipsoid.Eccentricity, 1 + _ellipsoid.Eccentricity) * Math.Pow(1 - _ellipsoid.Eccentricity, 1 - _ellipsoid.Eccentricity)) / (2 * _ellipsoid.SemiMajorAxis.BaseValue * _scaleFactorAtNaturalOrigin);

            Double ksi = 0, longitude = 0, latitude;

            switch (_operationAspect)
            {
                case OperationAspect.NorthPolar:
                    ksi = Constants.PI / 2 - 2 * Math.Atan(t);

                    if (coordinate.X == _falseEasting)
                        longitude = _longitudeOfNaturalOrigin;
                    else
                        longitude = _longitudeOfNaturalOrigin + Math.Atan2(coordinate.X - _falseEasting, _falseNorthing - coordinate.Y);
                    break;
                case OperationAspect.SouthPolar:
                    ksi = 2 * Math.Atan(t) - Constants.PI / 2;

                    if (coordinate.X == _falseEasting)
                        longitude = _longitudeOfNaturalOrigin;
                    else
                        longitude = _longitudeOfNaturalOrigin + Math.Atan2(coordinate.X - _falseEasting, coordinate.Y - _falseNorthing);
                    break;
            }

            latitude = ksi +
                       (_ellipsoid.EccentricitySquare / 2 + 5 * Calculator.Pow(_ellipsoid.Eccentricity, 4) / 24 + Calculator.Pow(_ellipsoid.Eccentricity, 6) / 12 + 13 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 360) * Math.Sin(2 * ksi) +
                       (7 * Calculator.Pow(_ellipsoid.Eccentricity, 4) / 48 + 29 * Calculator.Pow(_ellipsoid.Eccentricity, 6) / 240 + 811 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 11520) * Math.Sin(4 * ksi) +
                       (7 * Calculator.Pow(_ellipsoid.Eccentricity, 6) / 120 + 81 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 1120) * Math.Sin(6 * ksi) +
                       (4279 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 161280) * Math.Sin(8 * ksi);

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion
    }
}
