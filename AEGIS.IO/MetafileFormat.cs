/// <copyright file="MetafileFormat.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO
{
    /// <summary>
    /// Represents a format of metafile.
    /// </summary>
    public class MetafileFormat : IdentifiedObject
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the version of the format.
        /// </summary>
        /// <value>The version of the format.</value>
        public String Version { get; private set; }

        /// <summary>
        /// Gets the default extension of the format.
        /// </summary>
        /// <value>The default extension of the format.</value>
        public String DefaultExtension { get; private set; }

        /// <summary>
        /// Gets the default file name of the format.
        /// </summary>
        /// <value>The default file name of the format.</value>
        public String DefaultFileName { get; private set; }

        /// <summary>
        /// Gets the default naming pattern of the format.
        /// </summary>
        /// <value>The default naming pattern of the format.</value>
        public String DefaultNamingPattern { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the format has a default file name.
        /// </summary>
        /// <value><c>true</c> if the format has a default file name; otherwise, <c>false</c>.</value>
        public Boolean HasDefaultFileName { get { return !String.IsNullOrEmpty(DefaultFileName); } }

        /// <summary>
        /// Gets a value indicating whether the format has a default naming pattern.
        /// </summary>
        /// <value><c>true</c> if the format has a default naming pattern; otherwise, <c>false</c>.</value>
        public Boolean HasDefaultNamingPattern { get { return !String.IsNullOrEmpty(DefaultNamingPattern); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetafileFormat" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="defaultNamingPattern">The default naming pattern.</param>
        /// <param name="defaultFileName">The default file name.</param>
        /// <param name="defaultExtension">The default extension.</param>
        public MetafileFormat(String identifier, String name, String version, String defaultNamingPattern, String defaultFileName, String defaultExtension)
            : this(identifier, name, null, null, version, defaultNamingPattern, defaultFileName, defaultExtension)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetafileFormat" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="defaultNamingPattern">The default naming pattern.</param>
        /// <param name="defaultFileName">The default file name.</param>
        /// <param name="defaultExtension">The default extension.</param>
        public MetafileFormat(String identifier, String name, String remarks, String[] aliases, String version, String defaultNamingPattern, String defaultFileName, String defaultExtension)
            : base(identifier, name, remarks, aliases)
        {
            Version = version;
            DefaultFileName = defaultFileName;
            DefaultNamingPattern = defaultNamingPattern;
            DefaultExtension = defaultExtension;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns whether the specified file name matches the format.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns><c>true</c> if the file name matches the format; otherwise, <c>false</c>.</returns>
        public Boolean IsMatchingName(String fileName)
        {
            if (HasDefaultFileName)
                return fileName.Contains(DefaultFileName);
            if (HasDefaultNamingPattern)
                return Regex.IsMatch(fileName, DefaultNamingPattern);

            return fileName.EndsWith(DefaultExtension);
        }

        #endregion
    }
}
