/// <copyright file="IGeometry.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines general behavior for geometry in coordinate space.
    /// </summary>
    public interface IGeometry : IMetadataProvider, ICloneable
    {
        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        IGeometryFactory Factory { get; }

        /// <summary>
        /// Gets the precision model of the geometry.
        /// </summary>
        /// <value>The precision model of the geometry.</value>
        PrecisionModel PrecisionModel { get; }

        /// <summary>
        /// Gets the general name of the geometry.
        /// </summary>
        /// <value>The general name of the specific geometry.</value>
        String Name { get; }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>The inherent dimension of the geometry. The inherent dimension is always less than or equal to the coordinate dimension.</value>
        Int32 Dimension { get; }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        Int32 CoordinateDimension { get; }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        Int32 SpatialDimension { get; }

        /// <summary>
        /// Gets the model of the geometry.
        /// </summary>
        /// <value>The model of the geometry.</value>
        GeometryModel GeometryModel { get; }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        IReferenceSystem ReferenceSystem { get; }

        /// <summary>
        /// Gets the minimum bounding <see cref="Envelope" /> of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        Envelope Envelope { get; }

        /// <summary>
        /// Gets the bounding <see cref="IGeometry" />.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        IGeometry Boundary { get; }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        /// <value>The centroid of the geometry.</value>
        Coordinate Centroid { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        Boolean IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        Boolean IsSimple { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        Boolean IsValid { get; }

        /// <summary>
        /// Occurs when the geometry is changed.
        /// </summary>
        event EventHandler GeometryChanged;
    }
}
