/// <copyright file="Triangle.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using ELTE.AEGIS.Algorithms;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a triangle geometry in spatial coordinate space.
    /// </summary>
    /// <remarks>
    /// A Triangle is a polygon with 3 distinct, non-collinear vertices and no interior boundary.
    /// </remarks>
    public class Triangle : Polygon, ITriangle
    {
        #region IGeometry properties

        /// <summary>
        /// Gets a value indicating whether the triangle is valid.
        /// </summary>
        /// <value><c>true</c> if the coordinates form a legitimate triangle; otherwise, <c>false</c>.</value>
        public override Boolean IsValid
        {
            get
            {
                Double zValue = _shell.StartCoordinate.Z;
                if (_shell.Coordinates.Any(coordinate => coordinate.Z != zValue))
                    return false;
                
                if (Coordinate.Distance(_shell.Coordinates[0], _shell.Coordinates[1]) >= Coordinate.Distance(_shell.Coordinates[0], _shell.Coordinates[2]) + Coordinate.Distance(_shell.Coordinates[1], _shell.Coordinates[2]) &&
                    Coordinate.Distance(_shell.Coordinates[0], _shell.Coordinates[2]) < Coordinate.Distance(_shell.Coordinates[0], _shell.Coordinates[1]) + Coordinate.Distance(_shell.Coordinates[1], _shell.Coordinates[2]) &&
                    Coordinate.Distance(_shell.Coordinates[1], _shell.Coordinates[2]) < Coordinate.Distance(_shell.Coordinates[0], _shell.Coordinates[1]) + Coordinate.Distance(_shell.Coordinates[0], _shell.Coordinates[2]))
                    return false;

                if (PolygonAlgorithms.Orientation(_shell.Coordinates) != Orientation.CounterClockwise)
                    return false;

                return true;
            }
        }
        
        #endregion

        #region ISurface properties

        /// <summary>
        /// Gets a value indicating whether the triangle is convex.
        /// </summary>
        /// <value><c>true</c>, as a triangle is always convex.</value>
        public override sealed Boolean IsConvex { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether the triangle is whole.
        /// </summary>
        /// <value><c>true</c>, as a triangle is always whole.</value>
        public override sealed Boolean IsWhole { get { return true; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle" /> class.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public Triangle(Coordinate first, Coordinate second, Coordinate third, PrecisionModel precisionModel, IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : base(new Coordinate[] { first, second, third }, null, precisionModel, referenceSystem, metadata)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Triangle" /> class.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="second">The second coordinate.</param>
        /// <param name="third">The third coordinate.</param>
        /// <param name="factory">The factory of the triangle.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        /// <exception cref="System.ArgumentException">The specified factory is invalid.</exception>
        public Triangle(Coordinate first, Coordinate second, Coordinate third, IGeometryFactory factory, IDictionary<String, Object> metadata)
            : base(new Coordinate[] { first, second, third }, null, factory, metadata)
        {
        }

        #endregion
        
        #region IPolygon methods

        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <exception cref="System.NotSupportedException">Cannot add holes to triangle.</exception>
        public override void AddHole(ILinearRing hole)
        {
            throw new NotSupportedException("Cannot add holes to triangle.");
        }

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.NotSupportedException">The triangle does not contain holes.</exception>
        public override ILinearRing GetHole(Int32 index)
        {
            throw new NotSupportedException("The triangle does not contain holes.");
        }

        /// <summary>
        /// Removes a hole from the triangle.
        /// </summary>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the triangle contains the <paramref name="hole" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">The triangle does not contain holes.</exception>
        public override Boolean RemoveHole(ILinearRing hole)
        {
            throw new NotSupportedException("The triangle does not contain holes.");
        }

        /// <summary>
        /// Removes the hole at the specified index of the triangle.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to remove.</param>
        /// <exception cref="System.NotSupportedException">The triangle does not contain holes.</exception>
        public override void RemoveHoleAt(Int32 index)
        {
            throw new NotSupportedException("The triangle does not contain holes.");
        }

        /// <summary>
        /// Removes all holes from the triangle.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The triangle does not contain holes.</exception>
        public override void ClearHoles()
        {
            throw new NotSupportedException("The triangle does not contain holes.");
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the triangle instance.
        /// </summary>
        /// <returns>The deep copy of the triangle instance.</returns>
        public override Object Clone()
        {
            return new Triangle(_shell.Coordinates[0], _shell.Coordinates[1], _shell.Coordinates[2], Factory, Metadata);
        }

        #endregion
    }
}
