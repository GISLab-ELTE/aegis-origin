/// <copyright file="ForwardProjectionStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using System;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Represents a forward projection strategy.
    /// </summary>
    public class ForwardProjectionStrategy : ITransformationStrategy<GeoCoordinate, Coordinate>
    {
        #region Private fields

        private readonly ProjectedCoordinateReferenceSystem _target;
        private readonly CoordinateProjection _projection;
        
        #endregion

        #region ITransformationStrategy properties

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem SourceReferenceSystem { get { return _target.Base; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem TargetReferenceSystem { get { return _target; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ForwardProjectionStrategy" /> class.
        /// </summary>
        /// <param name="target">The target reference system.</param>
        /// <exception cref="System.ArgumentNullException">target;The target coordinate reference system is null.</exception>
        public ForwardProjectionStrategy(ProjectedCoordinateReferenceSystem target)
        {
            if (target == null)
                throw new ArgumentNullException("target", "The target reference system is null.");

            _target = target;
            _projection = target.Projection;
        }

        #endregion

        #region ICoordinateTransformationStrategy methods

        /// <summary>
        /// Transforms the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public Coordinate Transform(GeoCoordinate coordinate)
        {
            return _projection.Forward(coordinate);
        }

        #endregion
    }
}
