/// <copyright file="GeocentricToTopocentricConversion.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a geocentric to topocentric conversion.
    /// </summary>
    [CoordinateOperationMethodImplementation("EPSG::9836", "Geocentric/topocentric conversion")]
    public class GeocentricToTopocentricConversion : CoordinateConversion<Coordinate, Coordinate>
    {
        #region Private fields

        private readonly Double _geocentricXOfTopocentricOrigin;
        private readonly Double _geocentricYOfTopocentricOrigin;
        private readonly Double _geocentricZOfTopocentricOrigin;
        private readonly Double _sinLamda0, _cosLamda0, _sinFi0, _cosFi0;
 
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocentricToTopocentricConversion" /> class.
        /// </summary>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The ellipsoid is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not a length value as required by the method.
        /// </exception>
        public GeocentricToTopocentricConversion(IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : base(CoordinateOperationMethods.GeocentricToTopocentricConversion.Identifier, CoordinateOperationMethods.GeocentricToTopocentricConversion.Name, CoordinateOperationMethods.GeocentricToTopocentricConversion, parameters)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");

            _geocentricXOfTopocentricOrigin = ((Length)parameters[CoordinateOperationParameters.GeocenticXOfTopocentricOrigin]).BaseValue;
            _geocentricYOfTopocentricOrigin = ((Length)parameters[CoordinateOperationParameters.GeocenticYOfTopocentricOrigin]).BaseValue;
            _geocentricZOfTopocentricOrigin = ((Length)parameters[CoordinateOperationParameters.GeocenticZOfTopocentricOrigin]).BaseValue;

            GeographicToGeocentricConversion _conversion = new GeographicToGeocentricConversion(ellipsoid);
            GeoCoordinate geographicCoordinate = _conversion.Reverse(new Coordinate(_geocentricXOfTopocentricOrigin, _geocentricYOfTopocentricOrigin, _geocentricZOfTopocentricOrigin));

            _sinLamda0 = Math.Sin(geographicCoordinate.Longitude.BaseValue);
            _cosLamda0 = Math.Cos(geographicCoordinate.Longitude.BaseValue);

            _sinFi0 = Math.Sin(geographicCoordinate.Latitude.BaseValue);
            _cosFi0 = Math.Cos(geographicCoordinate.Latitude.BaseValue);
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 91
            Double dx = coordinate.X - _geocentricXOfTopocentricOrigin;
            Double dy = coordinate.Y - _geocentricYOfTopocentricOrigin;
            Double dz = coordinate.Z - _geocentricZOfTopocentricOrigin;

            Double u = -1 * dx * _sinLamda0 + dy * _cosLamda0;
            Double v = -1 * dx * _sinFi0 * _cosLamda0 - dy * _sinFi0 * _sinLamda0 + dz * _cosFi0;
            Double w = dx * _cosFi0 * _cosLamda0 + dy * _cosFi0 * _sinLamda0 + dz * _sinFi0;
            return new Coordinate(u, v, w);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 91
            Double x = _geocentricXOfTopocentricOrigin - coordinate.X * _sinLamda0 - coordinate.Y * _sinFi0 * _cosLamda0 + coordinate.Z * _cosFi0 * _cosLamda0;
            Double y = _geocentricYOfTopocentricOrigin + coordinate.X * _cosLamda0 - coordinate.Y * _sinFi0 * _sinLamda0 + coordinate.Z * _cosFi0 * _sinLamda0;
            Double z = _geocentricZOfTopocentricOrigin + coordinate.Y * _cosFi0 + coordinate.Z * _sinFi0;

            return new Coordinate(x, y, z);
        }

        #endregion
    }
}
