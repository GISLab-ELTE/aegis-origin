/// <copyright file="TopoGeometryObject.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS.IO.TopoJSON
{
    /// <summary>
    /// Represents a Geometry read from TopoJSON file.
    /// </summary>
    [Serializable]
    public class TopoGeometryObject : TopoJsonObject
    {
        /// <summary>
        /// The coordinates, if it is a Point or MultiPoint
        /// </summary>
        public JArray Coordinates { get; set; }
        /// <summary>
        /// The geometries, if it is a GeometryCollection
        /// </summary>
        public TopoGeometryObject[] Geometries { get; set; }
        /// <summary>
        /// The arcs, if it is a LineString, MultiLineString, Polygon or MultiPolygon
        /// </summary>
        public JArray Arcs { get; set; }
        /// <summary>
        /// The metadata of the geometry.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }
        /// <summary>
        /// The id of the geometry.
        /// </summary>
        public string Id { get; set; }
    }
}
