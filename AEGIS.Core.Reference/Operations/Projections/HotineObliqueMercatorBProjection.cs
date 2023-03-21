// <copyright file="HotineObliqueMercatorBProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Hotine Oblique Mercator (Variant B) projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9815", "Hotine Oblique Mercator (variant B)")]
    public class HotineObliqueMercatorBProjection : HotineObliqueMercatorProjection
    {
        #region Private fields

        private readonly Double _eastingAtProjectionCentre; // projection params
        private readonly Double _northingAtProjectionCentre;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HotineObliqueMercatorBProjection" /> class.
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
        public HotineObliqueMercatorBProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.HotineObliqueMercatorBProjection, parameters, ellipsoid, areaOfUse)
        {
            _eastingAtProjectionCentre = ((Length)_parameters[CoordinateOperationParameters.EastingAtProjectionCentre]).BaseValue;
            _northingAtProjectionCentre = ((Length)_parameters[CoordinateOperationParameters.NorthingAtProjectionCentre]).BaseValue;
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate location)
        {
            // source: EPSG Guidance Note number 7, part 2, page 56

            Double u, v;
            ComputeUV(location.Latitude.BaseValue, location.Longitude.BaseValue, out u, out v);

            Double easting = v * Math.Cos(_angleFromRectifiedToSkewGrid) + u * Math.Sin(_angleFromRectifiedToSkewGrid) + _eastingAtProjectionCentre;
            Double northing = u * Math.Cos(_angleFromRectifiedToSkewGrid) - v * Math.Sin(_angleFromRectifiedToSkewGrid) + _northingAtProjectionCentre;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 57

            Double v = (coordinate.X - _eastingAtProjectionCentre) * Math.Cos(_angleFromRectifiedToSkewGrid) - (coordinate.Y - _northingAtProjectionCentre) * Math.Sin(_angleFromRectifiedToSkewGrid);
            Double u = (coordinate.Y - _northingAtProjectionCentre) * Math.Cos(_angleFromRectifiedToSkewGrid) + (coordinate.X - _eastingAtProjectionCentre) * Math.Sin(_angleFromRectifiedToSkewGrid) + Math.Abs(_uC) * Math.Sign(_latitudeOfProjectionCentre);
            Double latitude, longitude;

            ComputeLatitudeLongitude(u, v, out latitude, out longitude);

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the u value.
        /// </summary>
        /// <param name="s">The S value.</param>
        /// <param name="v">The V value.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>
        /// The u value.
        /// </returns>
        protected override Double ComputeU(Double s, Double v, Double longitude)
        {
            return (Math.Abs(_azimuthOfInitialLine - Constants.PI / 2) <= Calculator.Tolerance) ?
                       ((longitude == _longitudeOfProjectionCentre) ? 0 : _a * Math.Atan((s * Math.Cos(_gammaO) + v * Math.Sin(_gammaO)) / Math.Cos(_b * (longitude - _lambdaO))) / _b - Math.Abs(_uC) * Math.Sign(_latitudeOfProjectionCentre) * Math.Sign(_longitudeOfProjectionCentre - longitude)) :
                       _a * Math.Atan((s * Math.Cos(_gammaO) + v * Math.Sin(_gammaO)) / Math.Cos(_b * (longitude - _lambdaO))) / _b - Math.Abs(_uC) * Math.Sign(_latitudeOfProjectionCentre);
        }

        #endregion
    }
}
