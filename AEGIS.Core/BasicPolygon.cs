/// <copyright file="BasicPolygon.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a basic polygon in spatial coordinate space.
    /// </summary>
    public class BasicPolygon : IBasicPolygon
    {
        #region Private fields

        /// <summary>
        /// The list of holes.
        /// </summary>
        private List<IBasicLineString> _holes;

        #endregion

        #region IBasicGeometry properties

        /// <summary>
        /// Gets the inherent dimension of the surface.
        /// </summary>
        /// <value><c>2</c>, which is the defined dimension of a surface.</value>
        public Int32 Dimension { get { return 2; } }

        /// <summary>
        /// Gets a value indicating whether the geometry is valid.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be valid; otherwise, <c>false</c>.</value>
        public Boolean IsValid { get { return PolygonAlgorithms.IsValid(this); } }

        #endregion

        #region IBasicPolygon methods

        /// <summary>
        /// Gets the shell of the clip.
        /// </summary>
        /// <value>The read-only list of shell coordinates.</value>
        public IBasicLineString Shell { get; private set; }

        /// <summary>
        /// Gets the number of holes of the polygon.
        /// </summary>
        /// <value>The number of holes in the polygon.</value>
        public Int32 HoleCount { get { return Holes.Count; } }

        /// <summary>
        /// Gets the holes of the clip.
        /// </summary>
        /// <value>The list of read-only holes.</value>
        public IList<IBasicLineString> Holes { get { return _holes.AsReadOnly(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicPolygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates representing the polygon shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public BasicPolygon(IList<Coordinate> shell)
            : this(shell, null)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicPolygon" /> class.
        /// </summary>
        /// <param name="shell">The coordinates representing the polygon shell.</param>
        /// <param name="holes">The collection of coordinates representing the polygon holes.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The shell contains less than 3 coordinates.
        /// or
        /// A hole is null.
        /// or
        /// A hole contains less than 3 coordinates.
        /// </exception>
        public BasicPolygon(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (shell.Count < 3)
                throw new ArgumentException("The shell contains less than 3 coordinates.", "shell");

            if (shell[0].Equals(shell[shell.Count - 1]))
                Shell = new BasicLineString(shell);
            else
                Shell = new BasicLineString(shell.Concat(new Coordinate[] { shell[0] }));

            _holes = new List<IBasicLineString>();
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    if (hole != null)
                    {
                        if (shell == null)
                            throw new ArgumentException("A hole is null.", "holes");
                        if (shell.Count < 3)
                            throw new ArgumentException("A hole contains less than 3 coordinates.", "holes");

                        if (hole[0].Equals(hole[hole.Count - 1]))
                            _holes.Add(new BasicLineString(hole));
                        else
                            _holes.Add(new BasicLineString(hole.Concat(new Coordinate[] { hole[0] })));
                    }
                }
            }
        }

        #endregion

        #region IBasicPolygon methods

        /// <summary>
        /// Gets a hole at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the hole to get.</param>
        /// <returns>The hole at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">There are no holes in the polygon.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// Index is equal to or greater than the number of holes.
        /// </exception>
        public IBasicLineString GetHole(Int32 index)
        {
            if (_holes.Count == 0)
                throw new InvalidOperationException("There are no holes in the polygon.");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _holes.Count)
                throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than the number of holes.");

            return _holes[index];
        }

        #endregion
    }
}
