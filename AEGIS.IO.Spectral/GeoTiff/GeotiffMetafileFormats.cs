/// <copyright file="GeotiffMetafileFormats.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Represents a collection of metafile formats for GeoTiff.
    /// </summary>
    public static class GeotiffMetafileFormats
    {
        #region Query fields

        private static MetafileFormat[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="MetafileFormat" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="MetafileFormat" /> instances within the collection.</value>
        public static IList<MetafileFormat> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GeotiffMetafileFormats).GetProperties().
                                                         Where(property => property.Name != "All").
                                                         Select(property => property.GetValue(null, null) as MetafileFormat).
                                                         ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeometryStreamFormat" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="GeometryStreamFormat" /> instances that match the specified identifier.</returns>
        public static IList<MetafileFormat> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }
        /// <summary>
        /// Returns all <see cref="GeometryStreamFormat" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="GeometryStreamFormat" /> instances that match the specified name.</returns>
        public static IList<MetafileFormat> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => System.Text.RegularExpressions.Regex.IsMatch(alias, name, System.Text.RegularExpressions.RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static MetafileFormat _dimap;
        private static MetafileFormat _landsat;

        #endregion

        #region Public static fields

        /// <summary>
        /// Dimap format.
        /// </summary>
        public static MetafileFormat Dimap
        {
            get
            {
                return _dimap ?? (_dimap = new MetafileFormat("AEGIS:000000", "DIMAP", "1.0", String.Empty, "metadata.dim", "dim"));
            }
        }

        /// <summary>
        /// Landsat format.
        /// </summary>
        public static MetafileFormat Landsat
        {
            get
            {
                return _landsat ?? (_landsat = new MetafileFormat("AEGIS:000000", "Landsat", "1.0", "*_MTL.txt", String.Empty, "txt"));
            }
        }

        #endregion
    }
}
