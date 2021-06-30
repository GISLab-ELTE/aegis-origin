/// <copyright file="GeometryObject.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
/// <author>Norbert Vass</author>

using Newtonsoft.Json.Linq;
using System;

namespace ELTE.AEGIS.IO.GeoJSON
{
    /// <summary>
    /// Represents a Geometry from GeoJSON file.
    /// </summary>
    [Serializable]
    public class GeometryObject : GeoJsonObject
    {
        /// <summary>
        /// The coordinates of the Geometry, if it is not GeometryCollection.
        /// </summary>
        public JArray Coordinates { get; set; }
        /// <summary>
        /// The geometries of the Geometry, if it is a GeometryCollection.
        /// </summary>
        public GeometryObject[] Geometries { get; set; }
    }
}
