/// <copyright file="XElementExtensions.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using System.Xml.Linq;

namespace ELTE.AEGIS.IO.GeoTiff.Metafile
{
    /// <summary>
    /// Defines extensions to <see cref="XElement"/>.
    /// </summary>
    public static class XElementExtensions
    {
        #region Extension methods

        /// <summary>
        /// Gets the first child element (in document order) with the specified name by ignoring culture and case.
        /// </summary>
        /// <param name="element">The base element.</param>
        /// <param name="name">The name of the child element.</param>
        /// <returns>A <see cref="XElement"/> that matches the specified name, or <c>null</c>.</returns>
        public static XElement Element(this XElement element, String name)
        {
            return Element(element, name, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the first child element (in document order) with the specified name.
        /// </summary>
        /// <param name="element">The base element.</param>
        /// <param name="name">The name of the child element.</param>
        /// <param name="comparison">The string comparison mode.</param>
        /// <returns>A <see cref="XElement"/> that matches the specified name, or <c>null</c>.</returns>
        public static XElement Element(this XElement element, String name, StringComparison comparison)
        {
            return element.Elements().FirstOrDefault(child => child.Name.LocalName.Equals(name, comparison));
        }

        #endregion
    }
}
