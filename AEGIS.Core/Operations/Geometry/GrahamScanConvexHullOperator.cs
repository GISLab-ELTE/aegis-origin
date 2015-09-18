///<copyright file="GrahamScanConvexHullOperator.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Algorithms;
using System;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents an operator computing the convex hull using the Graham scan algorithm.
    /// </summary>
    public sealed class GrahamScanConvexHullOperator : IGeometryConvexHullOperator
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private readonly IGeometryFactory _geometryFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GrahamScanConvexHullOperator" /> class.
        /// </summary>
        public GrahamScanConvexHullOperator()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrahamScanConvexHullOperator" /> class.
        /// </summary>
        /// <param name="factory">The geometry factory.</param>
        public GrahamScanConvexHullOperator(IGeometryFactory factory)
        {
            _geometryFactory = factory;
        }

        #endregion

        #region IGeometryConvexHullOperator methods

        /// <summary>
        /// Computes the convex hull of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The convex hull of the <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry ConvexHull(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            IGeometryFactory factory = _geometryFactory ?? geometry.Factory;

            if (geometry is IPoint)
                return factory.CreatePoint((geometry as IPoint).Coordinate);
            if (geometry is ICurve)
                return factory.CreatePolygon(GrahamScanAlgorithm.ComputeConvexHull((geometry as ILineString).Coordinates, geometry.PrecisionModel));
            if (geometry is IPolygon)
                return factory.CreatePolygon(GrahamScanAlgorithm.ComputeConvexHull((geometry as IPolygon).Shell.Coordinates, geometry.PrecisionModel));

            throw new ArgumentException("The operation is not supported with the specified geometry type.");
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        #endregion
    }
}
