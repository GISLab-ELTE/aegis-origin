/// <copyright file="IMetadataFactory.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Metadata;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for metadata factories.
    /// </summary>
    [FactoryContract(Product = typeof(IMetadataCollection), DefaultBehavior = typeof(MetadataFactory))]
    public interface IMetadataFactory : IFactory
    {
        /// <summary>
        /// Creates a metadata collection.
        /// </summary>
        /// <returns>The produced metadata collection.</returns>
        IMetadataCollection CreateCollection();

        /// <summary>
        /// Creates a metadata collection.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <returns>The produced metadata collection.</returns>
        IMetadataCollection CreateCollection(IDictionary<String, Object> source);
    }
}
