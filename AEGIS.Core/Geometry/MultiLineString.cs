/// <copyright file="MultiLineString.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a set of line strings in spatial coordinate space.
    /// </summary>
    public class MultiLineString : GeometryList<ILineString>, IMultiLineString
    {
        #region IMultiCurve properties

        /// <summary>
        /// Gets a value indicating whether the multi line string is closed.
        /// </summary>
        /// <value><c>true</c> if all curves within the multi line string are closed; otherwise, <c>false</c>.</value>
        public virtual Boolean IsClosed { get { return _geometries.All(geometry => geometry.IsClosed); } }

        /// <summary>
        /// Gets the length of the multi line string.
        /// </summary>
        /// <value>The length of the multi line string.</value>
        public virtual Double Length { get { return _geometries.Sum(geometry => geometry.Length); } }

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the multi line string.
        /// </summary>
        /// <value><c>1</c>, which is the defined dimension of a multi line string.</value>
        public override sealed Int32 Dimension { get { return 1; } }

        /// <summary>
        /// Gets a value indicating whether the multi line string is simple.
        /// </summary>
        /// <value><c>true</c> if all line string within the multi line string are simple; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple { get { return _geometries.All(geometry => geometry.IsSimple); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineString" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public MultiLineString(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineString" /> class.
        /// </summary>
        /// <param name="source">The source of line strings.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">source;Source is null.</exception>
        /// <exception cref="System.ArgumentException">Reference system of source geometries does not match.;source</exception>
        public MultiLineString(IEnumerable<ILineString> source, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(source, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineString" /> class.
        /// </summary>
        /// <param name="factory">The factory of the multi line string.</param>
        /// <param name="metadata">The metadata.</param>
        public MultiLineString(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiLineString" /> class.
        /// </summary>
        /// <param name="source">The source of line strings.</param>
        /// <param name="factory">The factory of the multi line string.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">source;Source is null.</exception>
        /// <exception cref="System.ArgumentException">Reference system of source geometries does not match.;source</exception>
        public MultiLineString(IEnumerable<ILineString> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(source, factory, metadata)
        {
        }

        #endregion
    }
}
