/// <copyright file="ModifiedAzimuthalEquidistantProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Modified Azimuthal Equidistant projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::9832", "Modified Azimuthal Equidistant Projection")]
    public class ModifiedAzimuthalEquidistantProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // projection params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _g; // projection constant

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedAzimuthalEquidistantProjection" /> class.
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
        public ModifiedAzimuthalEquidistantProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.ModifiedAzimuthalEquidistantProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 80

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).BaseValue;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).BaseValue;

            _g = _ellipsoid.Eccentricity * Math.Sin(_latitudeOfNaturalOrigin) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare);
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
            // source: EPSG Guidance Note number 7, part 2, page 80

            Double psi = Math.Atan((1 - _ellipsoid.EccentricitySquare) * Math.Tan(coordinate.Latitude.BaseValue) + _ellipsoid.EccentricitySquare * _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfNaturalOrigin) *
                          Math.Sin(_latitudeOfNaturalOrigin) / (_ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Latitude.BaseValue)));

            Double alpha = Math.Atan(Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin) / (Math.Cos(_latitudeOfNaturalOrigin) * Math.Tan(psi) - Math.Sin(_latitudeOfNaturalOrigin) * Math.Cos(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin)));

            Double h = _ellipsoid.Eccentricity * Math.Cos(_latitudeOfNaturalOrigin) * Math.Cos(alpha) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare);

            Double s;
            if (Math.Sin(alpha) == 0)
                s = Math.Asin(Math.Cos(_latitudeOfNaturalOrigin) * Math.Sin(psi) - Math.Sin(_latitudeOfNaturalOrigin) * Math.Cos(psi)) * Math.Sign(Math.Cos(alpha));
            else
                s = Math.Asin(Math.Sin(coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin) * Math.Cos(psi) / Math.Sin(alpha));

            Double c = _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfNaturalOrigin) * s * ((1 - Calculator.Pow(s,2) * Calculator.Pow(h,2) * (1 - Calculator.Pow(h,2)) / 6) +
                       ((Calculator.Pow(s, 3) / 8) * _g * h * (1 - 2 * Calculator.Pow(h, 2))) + (Calculator.Pow(s, 4) / 120) * (Calculator.Pow(h, 2) * (4 - 7 * Calculator.Pow(h, 2)) - 3 * Calculator.Pow(_g, 2) *
                       (1 - 7 * Calculator.Pow(h, 2))) - ((Calculator.Pow(s, 5) / 48) * _g * h));

            Double easting = _falseEasting + c * Math.Sin(alpha);
            Double northing = _falseNorthing + c * Math.Cos(alpha);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 80

            Double c = Math.Sqrt(Calculator.Pow((coordinate.X - _falseEasting),2) + Calculator.Pow((coordinate.Y - _falseNorthing),2));
            Double alpha = Math.Atan((coordinate.X - _falseEasting) / (coordinate.Y - _falseNorthing));
            Double a = -1 * _ellipsoid.EccentricitySquare * Calculator.Cos2(_latitudeOfNaturalOrigin) * Calculator.Cos2(alpha) / (1 - _ellipsoid.EccentricitySquare);
            Double b = 3 * _ellipsoid.EccentricitySquare * (1 - a) * Math.Sin(_latitudeOfNaturalOrigin) * Math.Cos(_latitudeOfNaturalOrigin) * Math.Cos(alpha) / (1 - _ellipsoid.EccentricitySquare);
            Double d = c / _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfNaturalOrigin);
            Double j = d - (a * (1 + a) * Calculator.Pow(d, 3) / 6) - (b * (1 + 3 * a) * Calculator.Pow(d, 4) / 24);
            Double k = 1 - (a * Calculator.Pow(j, 2) / 2) - (b * Calculator.Pow(j, 3) / 6);
            Double psi = Math.Asin(Math.Sin(_latitudeOfNaturalOrigin) * Math.Cos(j) + Math.Cos(_latitudeOfNaturalOrigin) * Math.Sin(j) * Math.Cos(alpha));

            Double latitude = Math.Atan((1-_ellipsoid.EccentricitySquare * k * Math.Sin(_latitudeOfNaturalOrigin) / Math.Sin(psi)) * Math.Tan(psi) / (1 - _ellipsoid.EccentricitySquare));
            Double longitude = _longitudeOfNaturalOrigin + Math.Asin(Math.Sin(alpha) * Math.Sin(j) / Math.Cos(psi));

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion
    }
}
