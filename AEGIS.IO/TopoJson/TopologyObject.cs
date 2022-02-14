/// <copyright file="TopoJsonObject.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Norbert Vass</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.IO.TopoJSON
{
    /// <summary>
    /// Represents a Topology object from TopoJSON file.
    /// </summary>
    [Serializable]
    public class TopologyObject : TopoJsonObject
    {
        /// <summary>
        /// Contains all objects of the topology in (name, object) pairs.
        /// </summary>
        public Dictionary<string, TopoGeometryObject> Objects { get; set; }
        /// <summary>
        /// Contains all arcs the topology has.
        /// </summary>
        public List<List<List<double>>> Arcs { get; set; }
        /// <summary>
        /// The transfrom object of the topology.
        /// </summary>
        public TransformObject Transform { get; set; }
    }
}
