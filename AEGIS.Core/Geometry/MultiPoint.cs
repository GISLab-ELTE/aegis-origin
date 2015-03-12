/// <copyright file="MultiPoint.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a set of points in spatial coordinate space.
    /// </summary>
    public class MultiPoint : GeometryList<IPoint>, IMultiPoint
    {
        #region IGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the multi point.
        /// </summary>
        /// <value><c>0</c>, which is the defined dimension of a multi point.</value>
        public override sealed Int32 Dimension { get { return 0; } }

        /// <summary>
        /// Determines whether the multi point is simple.
        /// </summary>
        /// <value><c>true</c> if all points are distinct; otherwise, <c>false</c>.</value>
        public override Boolean IsSimple 
        { 
            get 
            {
                HashSet<Coordinate> hashSet = new HashSet<Coordinate>();
                for (Int32 i = 0; i < _geometries.Length; i++)
                {
                    if (hashSet.Contains(_geometries[i].Coordinate))
                        return false;
                    hashSet.Add(_geometries[i].Coordinate);
                }
                return true;
            } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public MultiPoint(PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="source">The source of points.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public MultiPoint(IEnumerable<IPoint> source, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(source, precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="factory">The factory of the multi point.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public MultiPoint(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiPoint" /> class.
        /// </summary>
        /// <param name="source">The source of points.</param>
        /// <param name="factory">The factory of the multi point.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory is null.
        /// or
        /// The source is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public MultiPoint(IEnumerable<IPoint> source, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(source, factory, metadata)
        {
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the multi point instance.
        /// </summary>
        /// <returns>The deep copy of the multi point instance.</returns>
        public override Object Clone()
        {
            return new MultiPoint(_geometries, Factory, Metadata);
        }

        #endregion
    }
}