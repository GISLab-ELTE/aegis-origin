// <copyright file="GraphReferenceTransformationCredential.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial
{
    /// <summary>
    /// Represents a credential for the <see cref="ReferenceTransformation" /> class.
    /// </summary>
    public class GraphReferenceTransformationCredential : OperationCredential
    {
        /// <summary>
        /// Gets the priority of the operation.
        /// </summary>
        /// <value>The priority of the operation.</value>
        public override Double Priority { get { return 2; } }

        /// <summary>
        /// Validates the source object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><c>true</c> if the source object is valid for the operation; otherwise <c>false</c>.</returns>
        public override Boolean ValidateSource(Object source)
        {
            if (source == null)
                return false;

            if (source is IGeometryGraph)
                return true;

            if (source is IGeometryCollection<IGeometryGraph>)
                return (source as IGeometryCollection<IGeometryGraph>).All(geometry => ValidateSource(geometry));

            return false;
        }
    }
}
