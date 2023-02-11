// <copyright file="Line.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a line geometry in spatial coordinate space.
    /// </summary>
    public class Line : LineString, ILine
    {
        #region IGeometry properties

        /// <summary>
        /// Gets a value indicating whether the current geometry is simple.
        /// </summary>
        /// <value><c>true</c>, as a line is always considered to be simple.</value>
        public override Boolean IsSimple { get { return true; } }

        #endregion

        #region ICurve properties

        /// <summary>
        /// Gets the length of the line.
        /// </summary>
        /// <value>The length of the line.</value>
        public override Double Length { get { return Coordinate.Distance(_coordinates[0], _coordinates[1]); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Line" /> class.
        /// </summary>
        /// <param name="startCoordinate">The starting coordinate.</param>
        /// <param name="endCoordinate">The ending coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public Line(Coordinate startCoordinate, Coordinate endCoordinate, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(new List<Coordinate> { startCoordinate, endCoordinate }, precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line" /> class.
        /// </summary>
        /// <param name="startCoordinate">The starting coordinate.</param>
        /// <param name="endCoordinate">The ending coordinate.</param>
        /// <param name="factory">The factory of the line.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public Line(Coordinate startCoordinate, Coordinate endCoordinate, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(new List<Coordinate> { startCoordinate, endCoordinate }, factory, metadata)
        {
        }

        #endregion

        #region LineString methods

        /// <summary>
        /// Adds a coordinate to the end of the line.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.NotSupportedException">Cannot add coordinates to lines.</exception>
        public override void Add(Coordinate coordinate)
        {
            throw new NotSupportedException("Extension of lines is not supported.");        
        }

        /// <summary>
        /// Inserts a coordinate into the line at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the coordinate should be inserted.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <exception cref="System.NotSupportedException">Cannot insert coordinates to lines.</exception>
        public override void Insert(Int32 index, Coordinate coordinate)
        {
            throw new NotSupportedException("Extension of lines is not supported.");
        }

        /// <summary>
        /// Removes the first occurrence of the specified coordinate from the line.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate was removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">Cannot remove coordinates from lines.</exception>
        public override Boolean Remove(Coordinate coordinate)
        {
            throw new NotSupportedException("Reduction of lines is not supported.");
        }

        /// <summary>
        /// Removes the coordinate at the specified index from the line.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to remove.</param>
        /// <exception cref="System.NotSupportedException">Cannot remove coordinates from lines.</exception>
        public override void RemoveAt(Int32 index)
        {
            throw new NotSupportedException("Reduction of lines is not supported.");
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the line instance.
        /// </summary>
        /// <returns>The deep copy of the line instance.</returns>
        public override Object Clone()
        {
            return new Line(_coordinates[0], _coordinates[1], Factory, Metadata);
        }

        #endregion
    }
}
