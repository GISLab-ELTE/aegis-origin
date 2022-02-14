/// <copyright file="GeometryOperatorFactory.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents a factory producing geometry operators
    /// </summary>
    public class GeometryOperatorFactory : Factory, IGeometryOperatorFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryOperatorFactory" /> class.
        /// </summary>
        /// <param name="geometryFactory">The geometry factory.</param>
        public GeometryOperatorFactory(IGeometryFactory geometryFactory)
            : base(geometryFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryOperatorFactory" /> class.
        /// </summary>
        /// <param name="factories">The underlying factories based on factory contract.</param>
        protected GeometryOperatorFactory(params IFactory[] factories)
            : base(factories)
        {
        }

        #endregion

        #region IGeometryOperationFactory methods

        /// <summary>
        /// Gets the buffer operator.
        /// </summary>
        /// <value>The buffer operator.</value>
        /// <exception cref="System.NotSupportedException">Buffer operations are not supported.</exception>
        public IGeometryBufferOperator Buffer
        {
            get { throw new NotSupportedException("Buffer operations are not supported."); }
        }

        /// <summary>
        /// Gets the convex hull operator.
        /// </summary>
        /// <value>The Graham scan convex hull operator.</value>
        public IGeometryConvexHullOperator ConvexHull
        {
            get { return new GrahamScanConvexHullOperator(GetFactory<IGeometryFactory>()); }
        }

        /// <summary>
        /// Gets the measure operator.
        /// </summary>
        /// <value>The measure operator.</value>
        /// <exception cref="System.NotSupportedException">Measure operations are not supported.</exception>
        public IGeometryMeasureOperator Measure
        {
            get { throw new NotSupportedException("Measure operations are not supported."); }
        }

        /// <summary>
        /// Gets the overlay operator.
        /// </summary>
        /// <value>The halfedge topology model based overlay operator.</value>
        public IGeometryOverlayOperator Overlay
        {
            get { return new HalfedgeGeometryOverlayOperator(GetFactory<IGeometryFactory>()); }
        }

        /// <summary>
        /// Gets the relate operator.
        /// </summary>
        /// <value>The halfedge topology model based relate operator.</value>
        public IGeometryRelateOperator Relate
        {
            get { return new HalfedgeGeometryRelateOperator(); }
        }

        #endregion
    }
}
