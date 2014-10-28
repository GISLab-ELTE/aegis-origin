/// <copyright file="GeographicToTopocentricConversion.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>András Fábián</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a geographic to topocentric conversion.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9837", "Geographic/topocentric conversion")]
    public class GeographicToTopocentricConversion : CoordinateConversion<GeoCoordinate, Coordinate>
    {
        #region Private fields

        private readonly Ellipsoid _ellipsoid;
        private readonly Double _latitudeOfTopocentricOrigin; // operation parameters
        private readonly Double _longitudeOfTopocentricOrigin;
        private readonly Double _ellipsoidalHeightOfTopocentricOrigin;
        private readonly Double _originRadiousOfPrimeVerticalCurvature; // operation constants
        private readonly Coordinate _originGeocentricCoordinate;
        private readonly Double _sinLamda0;
        private readonly Double _cosLamda0;
        private readonly Double _sinFi0;
        private readonly Double _cosFi0;
        private readonly GeographicToGeocentricConversion _conversion;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid used by the operation.</value>
        public Ellipsoid Ellipsoid { get { return _ellipsoid; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicToTopocentricConversion" /> class.
        /// </summary>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The method requires parameteres which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// </exception>
        public GeographicToTopocentricConversion(IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : base(CoordinateOperationMethods.GeographicToTopocentricConversion.Identifier, CoordinateOperationMethods.GeographicToTopocentricConversion.Name, CoordinateOperationMethods.GeographicToTopocentricConversion, parameters)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");

            _ellipsoid = ellipsoid;
            _latitudeOfTopocentricOrigin = ((Angle)parameters[CoordinateOperationParameters.LatitudeOfTopocentricOrigin]).BaseValue;
            _longitudeOfTopocentricOrigin = ((Angle)parameters[CoordinateOperationParameters.LongitudeOfTopocentricOrigin]).BaseValue;
            _ellipsoidalHeightOfTopocentricOrigin = ((Length)parameters[CoordinateOperationParameters.EllipsoidalHeightOfTopocentricOrigin]).BaseValue;

            _originRadiousOfPrimeVerticalCurvature = _ellipsoid.RadiusOfPrimeVerticalCurvature(_latitudeOfTopocentricOrigin);

            _conversion = new GeographicToGeocentricConversion(_ellipsoid);

            _originGeocentricCoordinate = _conversion.Forward(new GeoCoordinate(_latitudeOfTopocentricOrigin, _longitudeOfTopocentricOrigin, _ellipsoidalHeightOfTopocentricOrigin));

            _sinLamda0 = Math.Sin(_longitudeOfTopocentricOrigin);
            _cosLamda0 = Math.Cos(_longitudeOfTopocentricOrigin);

            _sinFi0 = Math.Sin(_latitudeOfTopocentricOrigin);
            _cosFi0 = Math.Cos(_latitudeOfTopocentricOrigin);
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
            // source: EPSG Guidance Note number 7, part 2, page 94
            Double primeVerticalCurvature = _ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude).BaseValue;

            Double height = coordinate.Height.BaseValue;
            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;

            Double u = (height + primeVerticalCurvature) * Math.Cos(latitude) * Math.Sin(longitude - _longitudeOfTopocentricOrigin);
            Double v = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Cos(_latitudeOfTopocentricOrigin) - Math.Cos(latitude) * Math.Sin(_latitudeOfTopocentricOrigin) * Math.Cos(longitude - _longitudeOfTopocentricOrigin)) + _ellipsoid.EccentricitySquare * (_originRadiousOfPrimeVerticalCurvature * Math.Sin(_latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Cos(_latitudeOfTopocentricOrigin);
            Double w = (height + primeVerticalCurvature) * (Math.Sin(latitude) * Math.Sin(_latitudeOfTopocentricOrigin) + Math.Cos(latitude) * Math.Cos(_latitudeOfTopocentricOrigin) * Math.Cos(longitude - _longitudeOfTopocentricOrigin)) + _ellipsoid.EccentricitySquare * (_originRadiousOfPrimeVerticalCurvature * Math.Sin(_latitudeOfTopocentricOrigin) - primeVerticalCurvature * Math.Sin(latitude)) * Math.Sin(_latitudeOfTopocentricOrigin) - (_ellipsoidalHeightOfTopocentricOrigin + _originRadiousOfPrimeVerticalCurvature);

            return new Coordinate(u, v, w);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 94

            Double x = _originGeocentricCoordinate.X - coordinate.X * _sinLamda0 - coordinate.Y * _sinFi0 * _cosLamda0 + coordinate.Z * _cosFi0 * _cosLamda0;
            Double y = _originGeocentricCoordinate.Y + coordinate.X * _cosLamda0 - coordinate.Y * _sinFi0 * _sinLamda0 + coordinate.Z * _cosFi0 * _sinLamda0;
            Double z = _originGeocentricCoordinate.Z + coordinate.Y * _cosFi0 + coordinate.Z * _sinFi0;

            return _conversion.Reverse(new Coordinate(x, y, z));
        }

        #endregion
    }
}

