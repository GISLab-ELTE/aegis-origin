/// <copyright file="PolarStereographicBProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>András Fábián</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Polar Stereographic (Variant B) Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9829", "Polar Stereographic (variant B)")]
    public class PolarStereographicBProjection : PolarStereographicProjection
    {
        #region Private fields

        private readonly Double _latitudeOfStandardParallel; // projection parameters
        private readonly Double _longitudeOfOrigin;
        private readonly Double _falseEasting;
        private readonly Double _falseNorthing;
        private readonly Double _scaleFactorAtNaturalOrigin; // projection constants

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
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public PolarStereographicBProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.PolarStereographicBProjection, parameters, ellipsoid, areaOfUse)
        {
            _latitudeOfStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfStandardParallel]).BaseValue;
            _longitudeOfOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _operationAspect = (_latitudeOfStandardParallel >= 0) ? OperationAspect.NorthPolar : OperationAspect.SouthPolar;

            // source: EPSG Guidance Note number 7, part 2, page 65
            Double mF, tF;

            mF = Math.Cos(_latitudeOfStandardParallel) / Math.Sqrt(1 - Ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfStandardParallel));

            if (_operationAspect == OperationAspect.NorthPolar)
            {
                tF = Math.Tan(Numerics.Constants.PI / 4 - _latitudeOfStandardParallel / 2) / Math.Pow((1 + Ellipsoid.Eccentricity * Math.Sin(_latitudeOfStandardParallel)) / (1 - Ellipsoid.Eccentricity * Math.Sin(_latitudeOfStandardParallel)), Ellipsoid.Eccentricity / 2);
            }
            else
            {
                tF = Math.Tan(Numerics.Constants.PI / 4 + _latitudeOfStandardParallel / 2) / Math.Pow((1 + Ellipsoid.Eccentricity * Math.Sin(_latitudeOfStandardParallel)) / (1 - Ellipsoid.Eccentricity * Math.Sin(_latitudeOfStandardParallel)), Ellipsoid.Eccentricity / 2);            
            }

            _scaleFactorAtNaturalOrigin = mF * (Math.Sqrt(Math.Pow(1 + Ellipsoid.Eccentricity, 1 + Ellipsoid.Eccentricity) * Math.Pow(1 - Ellipsoid.Eccentricity, 1 - Ellipsoid.Eccentricity))) / (2 * tF);
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
                    easting = _falseEasting + phi * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfOrigin);
                    northing = _falseNorthing - phi * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfOrigin);
                    break;
                case OperationAspect.SouthPolar:
                    easting = _falseEasting + phi * Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfOrigin);
                    northing = _falseNorthing + phi * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfOrigin);
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
            // source: EPSG Guidance Note number 7, part 2, page 67

            Double phi = Math.Sqrt(Calculator.Square(coordinate.X - _falseEasting) + Calculator.Square(coordinate.Y - _falseNorthing));
            Double t = phi * Math.Sqrt(Math.Pow(1 + _ellipsoid.Eccentricity, 1 + _ellipsoid.Eccentricity) * Math.Pow(1 - _ellipsoid.Eccentricity, 1 - _ellipsoid.Eccentricity)) / (2 * _ellipsoid.SemiMajorAxis.BaseValue * _scaleFactorAtNaturalOrigin);

            Double ksi = 0, longitude = 0, latitude;

            switch (_operationAspect)
            {
                case OperationAspect.NorthPolar:
                    ksi = Constants.PI / 2 - 2 * Math.Atan(t);

                    if (coordinate.X == _falseEasting)
                        longitude = _longitudeOfOrigin;
                    else
                        longitude = _longitudeOfOrigin + Math.Atan2(coordinate.X - _falseEasting, _falseNorthing - coordinate.Y);
                    break;
                case OperationAspect.SouthPolar:
                    ksi = 2 * Math.Atan(t) - Constants.PI / 2;

                    if (coordinate.X == _falseEasting)
                        longitude = _longitudeOfOrigin;
                    else
                        longitude = _longitudeOfOrigin + Math.Atan2(coordinate.X - _falseEasting, coordinate.Y - _falseNorthing);
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
