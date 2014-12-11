/// <copyright file="Curve.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a curve geometry in spatial coordinate space.
    /// </summary>
    public abstract class Curve : Geometry, ICurve
    {
        #region IGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the curve.
        /// </summary>
        /// <value><c>1</c>, which is the defined dimension of a curve.</value>
        public override sealed Int32 Dimension { get { return 1; } }

        #endregion

        #region ICurve properties

        /// <summary>
        /// Gets a value indicating whether the curve is closed.
        /// </summary>
        /// <value><c>true</c> if the starting and ending coordinates are equal; otherwise, <c>false</c>.</value>
        public abstract Boolean IsClosed { get; }

        /// <summary>
        /// Gets a value indicating whether the curve is a ring.
        /// </summary>
        /// <value><c>true</c> if the curve is simple and closed; otherwise, <c>false</c>.</value>
        public Boolean IsRing { get { return IsClosed && IsSimple; } }

        /// <summary>
        /// Gets the length of the curve.
        /// </summary>
        /// <value>The length of the curve.</value>
        public abstract Double Length { get; }

        /// <summary>
        /// Gets the staring point.
        /// </summary>
        /// <value>The first point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        public abstract IPoint StartPoint { get; }

        /// <summary>
        /// Gets the ending point.
        /// </summary>
        /// <value>The last point of the curve if the curve has at least one point; otherwise, <c>null</c>.</value>
        public abstract IPoint EndPoint { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        protected Curve(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Curve" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="factory">The factory of the curve.</param>
        /// <param name="metadata">The metadata.</param>
        protected Curve(IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(factory, metadata)
        {
        }

        #endregion
    }
}
