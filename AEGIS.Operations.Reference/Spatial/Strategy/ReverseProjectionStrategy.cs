/// <copyright file="ReverseProjectionStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Represents a reverse projection strategy.
    /// </summary>
    public class ReverseProjectionStrategy : ITransformationStrategy<Coordinate, GeoCoordinate>
    {
        #region Private fields

        private readonly ProjectedCoordinateReferenceSystem _source;
        private readonly CoordinateProjection _projection;
        
        #endregion

        #region ITransformationStrategy properties

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem SourceReferenceSystem { get { return _source; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem TargetReferenceSystem { get { return _source.Base; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionReverseStrategy" /> class.
        /// </summary>
        /// <param name="source">The source reference system.</param>
        /// <exception cref="System.ArgumentNullException">The source coordinate reference system is null.</exception>
        public ReverseProjectionStrategy(ProjectedCoordinateReferenceSystem source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source reference system is null.");

            _source = source;
            _projection = source.Projection;
        }

        #endregion

        #region ICoordinateTransformationStrategy methods

        /// <summary>
        /// Transforms the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public GeoCoordinate Transform(Coordinate coordinate)
        {
            return _projection.Reverse(coordinate);
        }

        #endregion
    }
}
