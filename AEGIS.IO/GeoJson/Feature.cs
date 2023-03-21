// <copyright file="Feature.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.IO.GeoJSON
{
    /// <summary>
    /// Represents a Feature from GeoJSON file.
    /// </summary>
    /// <author>Norbert Vass</author>
    [Serializable]
    public class Feature : GeoJsonObject
    {
        /// <summary>
        /// The feature's geometry.
        /// </summary>
        public GeometryObject Geometry { get; set; }
        /// <summary>
        /// The feature's data.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }
        /// <summary>
        /// The id of the feature.
        /// </summary>
        public string Id { get; set; }
    }
}
