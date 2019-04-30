/// <copyright file="MetadataFactory.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Geometry;

namespace ELTE.AEGIS.Metadata
{
    /// <summary>
    /// Represents a factory for metadata containers.
    /// </summary>
    public class MetadataFactory : Factory, IMetadataFactory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataFactory" /> class.
        /// </summary>
        public MetadataFactory() { }

        #endregion

        #region Factory methods for metadata collections

        /// <summary>
        /// Creates a metadata collection.
        /// </summary>
        /// <returns>The produced metadata collection.</returns>
        public virtual IMetadataCollection CreateCollection() { return new MetadataCollection(this); }

        /// <summary>
        /// Creates a metadata collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced metadata collection.</returns>
        public virtual IMetadataCollection CreateCollection(IDictionary<String, Object> source)
        {
            if (source == null)
                return null;
            return new MetadataCollection(source, this);
        }

        #endregion
    }
}
