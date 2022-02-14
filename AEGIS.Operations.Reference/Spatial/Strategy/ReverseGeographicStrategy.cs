/// <copyright file="ReverseGeographicStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Represents a reverse geographic transformation strategy.
    /// </summary>
    public class ReverseGeographicStrategy : ITransformationStrategy<GeoCoordinate, GeoCoordinate>
    {
        #region Private fields

        private readonly GeographicCoordinateReferenceSystem _source;
        private readonly GeographicCoordinateReferenceSystem _target;
        private readonly GeographicTransformation _transformation;

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
        public ReferenceSystem TargetReferenceSystem { get { return _target; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseGeographicStrategy" /> class.
        /// </summary>
        /// <param name="source">The source reference system.</param>
        /// <param name="target">The target reference system.</param>
        /// <param name="transformation">The geographic transformation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source coordinate reference system is null.
        /// or
        /// target;The target coordinate reference system is null.
        /// or
        /// transformation;The transformation is null.
        /// </exception>
        public ReverseGeographicStrategy(GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, GeographicTransformation transformation)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source coordinate reference system is null.");
            if (target == null)
                throw new ArgumentNullException("target", "The target coordinate reference system is null.");
            if (transformation == null)
                throw new ArgumentNullException("transformation", "The transformation is null.");

            _source = source;
            _target = target;
            _transformation = transformation;
        }

        #endregion

        #region ICoordinateTransformationStrategy methods

        /// <summary>
        /// Transforms the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public GeoCoordinate Transform(GeoCoordinate coordinate)
        {
            return _transformation.Reverse(coordinate);
        }

        #endregion
    }
}
