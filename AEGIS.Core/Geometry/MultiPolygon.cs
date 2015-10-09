/// <copyright file="MultiPolygon.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a set of polygons in spatial coordinate space.
    /// </summary>
    public class MultiPolygon : GeometryList<IPolygon>, IMultiPolygon
    {
        #region IMultiSurface properties

        /// <summary>
        /// Gets the area of the multi polygon.
        /// </summary>
        /// <value>The sum of polygon areas within the multi polygon.</value>
        public Double Area
        {
            get { return _geometries.Sum(geometry => geometry.Area); }
        }

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the general name of the geometry list.
        /// </summary>
        /// <value>The general name of the specific geometry.</value>
        public override String Name { get { return "MultiPolygon"; } }

        /// <summary>
        /// Gets the inherent dimension of the multi polygon.
        /// </summary>
        /// <value><c>2</c>, which is the defined dimension of a multi polygon.</value>
        public override sealed Int32 Dimension { get { return 2; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public MultiPolygon(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon" /> class.
        /// </summary>
        /// <param name="source">The source of polygons.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public MultiPolygon(IEnumerable<IPolygon> source, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(source, precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon" /> class.
        /// </summary>
        /// <param name="factory">The factory of the multi polygon.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public MultiPolygon(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPolygon" /> class.
        /// </summary>
        /// <param name="source">The source of polygons.</param>
        /// <param name="factory">The factory of the multi polygon.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The source is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public MultiPolygon(IEnumerable<IPolygon> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(source, factory, metadata)
        {
        }

        #endregion
    }
}
