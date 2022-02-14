/// <copyright file="BasicLineString.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a basic line string geometry in spatial coordinate space.
    /// </summary>
    public class BasicLineString : IBasicLineString
    {
        #region Private fields

        /// <summary>
        /// The list of coordinates.
        /// </summary>
        private readonly List<Coordinate> _coordinates;

        #endregion

        #region IBasicGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        /// <value>
        /// The inherent dimension of the geometry. The inherent dimension is always less than or equal to the coordinate dimension.
        /// </value>
        public Int32 Dimension { get { return 1; } }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value>
        /// <c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsValid
        {
            get { return _coordinates.All(coordinate => coordinate.IsValid); }
        }

        #endregion

        #region IBasicLineString properties

        /// <summary>
        /// Gets the number of coordinates in the line string.
        /// </summary>
        /// <value>The number of coordinates in the line string.</value>
        public Int32 Count { get { return _coordinates.Count; } }

        /// <summary>
        /// Gets the coordinates in the line string.
        /// </summary>
        /// <value>The read-only list of coordinates of the line string.</value>
        public IList<Coordinate> Coordinates { get { return _coordinates.AsReadOnly(); } }

        /// <summary>
        /// Gets the staring coordinate.
        /// </summary>
        /// <value>The first coordinate of the line string.</value>
        public Coordinate StartCoordinate
        {
            get
            {
                if (_coordinates.Count == 0)
                    return Coordinate.Undefined;
                return _coordinates[0];
            }
        }

        /// <summary>
        /// Gets the ending coordinate.
        /// </summary>
        /// <value>The last coordinate of the line string.</value>
        public Coordinate EndCoordinate
        {
            get
            {
                if (_coordinates.Count == 0)
                    return Coordinate.Undefined;
                return _coordinates[_coordinates.Count - 1];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicLineString" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BasicLineString(IEnumerable<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _coordinates = new List<Coordinate>(source);
        }

        #endregion

        #region IBasicLineString methods

        /// <summary>
        /// Determines whether the line string contains the specified coordinate within its coordinates.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line string contains the specified coordinate within its coordinates; otherwise, <c>false</c>.</returns>
        public virtual Boolean Contains(Coordinate coordinate)
        {
            return _coordinates.Contains(coordinate);
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the coordinate to get.</param>
        /// <returns>The coordinate located at the specified <paramref name="index" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of coordinates.
        /// </exception>
        public virtual Coordinate GetCoordinate(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _coordinates.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of coordinates.");

            return _coordinates[index];
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{Coordinate}" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<Coordinate> GetEnumerator()
        {
            foreach (Coordinate coordinate in _coordinates)
                yield return coordinate;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
