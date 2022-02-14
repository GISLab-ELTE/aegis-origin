/// <copyright file="VerticalPerspectiveProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>András Fábián</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Vertical Perspective projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9838", "Vertical Perspective")]
    public class VerticalPerspectiveProjection : CoordinateProjection
    {
        #region Private fields

        private readonly Double _latitudeOfTopocentricOrigin; // operation parameters
        private readonly Double _longitudeOfTopocentricOrigin;
        private readonly Double _ellipsoidalHeightOfTopocentricOrigin;
        private readonly Double _viewPointHeight;
        private readonly Double _originRadiousOfPrimeVerticalCurvature; // operation constants

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalPerspectiveProjection" /> class.
        /// </summary>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameters which are not specified.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public VerticalPerspectiveProjection(IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : base(CoordinateOperationMethods.VerticalPerspectiveProjection.Identifier, CoordinateOperationMethods.VerticalPerspectiveProjection.Name, CoordinateOperationMethods.VerticalPerspectiveProjection, parameters, ellipsoid, AreasOfUse.World)
        {
            _latitudeOfTopocentricOrigin = ((Angle)parameters[CoordinateOperationParameters.LatitudeOfTopocentricOrigin]).BaseValue;
            _longitudeOfTopocentricOrigin = ((Angle)parameters[CoordinateOperationParameters.LongitudeOfTopocentricOrigin]).BaseValue;
            _ellipsoidalHeightOfTopocentricOrigin = ((Length)parameters[CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin]).BaseValue;
            _viewPointHeight = ((Length)parameters[CoordinateOperationParameters.ViewPointHeight]).BaseValue;

            _originRadiousOfPrimeVerticalCurvature = _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfTopocentricOrigin);
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
            // source: EPSG Guidance Note number 7, part 2, page 85
            Double primeVerticalCurvature = _ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude).BaseValue;

            Double height = coordinate.Height.BaseValue;
            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;

            Double u = (height + primeVerticalCurvature) * Math.Cos(latitude) * Math.Sin(longitude - _longitudeOfTopocentricOrigin);
            Double v = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Cos(_latitudeOfTopocentricOrigin) - Math.Cos(latitude) * Math.Sin(_latitudeOfTopocentricOrigin) * Math.Cos(longitude - _longitudeOfTopocentricOrigin)) + _ellipsoid.EccentricitySquare * (_originRadiousOfPrimeVerticalCurvature * Math.Sin(_latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Cos(_latitudeOfTopocentricOrigin);
            Double w = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Sin(_latitudeOfTopocentricOrigin) + Math.Cos(latitude) * Math.Cos(_latitudeOfTopocentricOrigin) * Math.Cos(longitude - _longitudeOfTopocentricOrigin)) + _ellipsoid.EccentricitySquare * (_originRadiousOfPrimeVerticalCurvature * Math.Sin(_latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Sin(_latitudeOfTopocentricOrigin) - (_ellipsoidalHeightOfTopocentricOrigin + _originRadiousOfPrimeVerticalCurvature);

            Double easting = u * _viewPointHeight / (_viewPointHeight - w);
            Double northing = v * _viewPointHeight / (_viewPointHeight - w);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            throw new NotSupportedException("Coordinate operation is not reversible.");
        }


        #endregion
    }
}

