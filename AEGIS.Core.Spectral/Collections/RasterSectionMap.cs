/// <copyright file="RasterSectionMap.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Text;

namespace ELTE.AEGIS.Collections
{
    /// <summary>
    /// Represents a type for mapping raster section to raster tiles.
    /// </summary>
    public class RasterSectionMap
    {
        #region Private fields

        /// <summary>
        /// The list of sections.
        /// </summary>
        private List<RasterSection> _sections;

        /// <summary>
        /// The list of tile sections.
        /// </summary>
        private List<RasterSection> _tileSections;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSectionMap" /> class.
        /// </summary>
        public RasterSectionMap()
        {
            _sections = new List<RasterSection>();
            _tileSections = new List<RasterSection>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSectionMap" /> class.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <exception cref="System.ArgumentNullException">The other section map is null.</exception>
        public RasterSectionMap(RasterSectionMap other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other section map is null.");

            _sections = new List<RasterSection>(other.Sections);
            _tileSections = new List<RasterSection>(other.TileSections);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterSectionMap" /> class.
        /// </summary>
        /// <param name="sections">The collection of raster sections.</param>
        /// <param name="tileSections">The collection of tile sections.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The section collection is null.
        /// or
        /// The tile section collection is null.
        /// </exception>
        public RasterSectionMap(IEnumerable<RasterSection> sections, IEnumerable<RasterSection> tileSections)
        {
            if (sections == null)
                throw new ArgumentNullException("sections", "The section collection is null.");
            if (tileSections == null)
                throw new ArgumentNullException("tileSections", "The tile section collection is null.");

            _sections = new List<RasterSection>(sections);
            _tileSections = new List<RasterSection>(tileSections);
        }

        #endregion

        #region Public properties
        
        /// <summary>
        /// Gets the sections of the raster.
        /// </summary>
        /// <value>The list of sections.</value>
        public IReadOnlyList<RasterSection> Sections { get { return _sections; } }

        /// <summary>
        /// Gets the sections of the tile.
        /// </summary>
        /// <value>The list of tile sections.</value>
        public IReadOnlyList<RasterSection> TileSections { get { return _tileSections; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified section to the map.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The section is null.</exception>
        public Int32 AddSection(RasterSection section)
        {
            if (section == null)
                throw new ArgumentNullException("section", "The section is null.");

            if (section.RowIndex < 0 ||section.ColumnIndex < 0 || section.Area == 0)
                return 0;

            foreach (RasterSection existingSection in _sections)
                if (existingSection.IsMatching(section))
                {
                    section.Id = existingSection.Id;
                    return section.Id;
                }

            section.Id = _sections.Count + 1;
            _sections.Add(section);
            return section.Id;
        }

        /// <summary>
        /// Adds the specified tile section to the map.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <exception cref="System.ArgumentNullException">The section is null.</exception>
        public void AddTileSection(RasterSection section)
        {
            if (section == null)
                throw new ArgumentNullException("section", "The section is null.");



            if (section.Id == 0 || section.RowIndex < 0 || section.ColumnIndex < 0 || section.Area == 0)
                return;

            foreach (RasterSection existingSection in _tileSections)
                if (existingSection.IsMatching(section))
                    return;

            _tileSections.Add(section);
        }

        #endregion

        #region Object methods
        
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override String ToString()
        {
            StringBuilder builder = new StringBuilder("[");
            for (Int32 index = 0; index < _sections.Count; index++)
            {
                if (index > 0)
                    builder.Append('|');

                builder.Append(_sections[index].ToString());
            }
            builder.Append("][");
            for (Int32 index = 0; index < _sections.Count; index++)
            {
                if (index > 0)
                    builder.Append('|');

                builder.Append(_sections[index].ToString());
            }
            builder.Append("]");

            return builder.ToString();
        }

        #endregion        
    }
}
