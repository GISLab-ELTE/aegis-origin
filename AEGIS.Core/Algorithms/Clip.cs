/// <copyright file="Clip.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a clip of a polygon.
    /// </summary>
    public class Clip
    {
        #region Public properties

        /// <summary>
        /// Gets the shell of the clip.
        /// </summary>
        /// <value>The read-only list of shell coordinates.</value>
        public IList<Coordinate> Shell { get; private set; }

        /// <summary>
        /// Gets the holes of the clip.
        /// </summary>
        /// <value>The list of read-only holes.</value>
        public IList<IList<Coordinate>> Holes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Clip"/> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public Clip(IList<Coordinate> shell)
            : this(shell, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Clip" /> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        /// <param name="holes">The holes.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public Clip(IList<Coordinate> shell, IList<IList<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            Shell = shell.AsReadOnly();

            Holes = new List<IList<Coordinate>>();
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                    Holes.Add(hole.AsReadOnly());
            }
        }

        #endregion
    }
}
