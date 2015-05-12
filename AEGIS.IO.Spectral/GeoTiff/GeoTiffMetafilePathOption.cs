/// <copyright file="GeoTiffMetafilePathOption.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Defines path options when opening metafiles.
    /// </summary>
    public enum GeoTiffMetafilePathOption
    {
        /// <summary>
        /// Indicates that the specified path is a directory.
        /// </summary>
        IsDirectoryPath,

        /// <summary>
        /// Indicates that the specified path is the path of the metafile.
        /// </summary>
        IsMetafilePath,

        /// <summary>
        /// Indicates that the specified path is the path of the GeoTIFF.
        /// </summary>
        IsGeoTiffFilePath,

        /// <summary>
        /// Indicates that the specified path is a search pattern.
        /// </summary>
        IsSearchPattern
    }
}
