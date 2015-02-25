/// <copyright file="GeometryCompatibility.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.WellKnown.Compatility
{
    /// <summary>
    /// Provides compatibility methods for <see cref="IGeometry" /> instances.
    /// </summary>
    /// <remarks>
    /// These extensions provides additional methods for <see cref="IGeometry" /> and descendant types 
    /// to enable full compliance with the OGC Simple Feature Access standard.
    /// </remarks>
    public static class GeometryCompatibility
    {
        /// <summary>
        /// Exports the geometry to Well-known Text (WKT) Representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The <see cref="System.String" /> representing the Well-known Text (WKT) form of the geometry.</returns>
        public static String AsText(this IGeometry geometry) { return geometry.ToWellKnownText(); }

        /// <summary>
        /// Exports the geometry to Well-known Binary (WKB) Representation.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The <see cref="System.String" /> representing the Well-known Binary (WKB) form of the geometry.</returns>
        public static Byte[] AsBinary(this IGeometry geometry) { return geometry.ToWellKnownBinary(); }
    }
}
