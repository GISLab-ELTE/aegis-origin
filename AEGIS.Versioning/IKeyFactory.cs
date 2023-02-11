// <copyright file="IKeyFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Versioning
{
    /// <summary>
    /// Defines the factory interface for creating keys.
    /// </summary>
    /// <typeparam name="TKey">The key type the factory produces.</typeparam>
    /// <author>Máté Cserép</author>
    public interface IKeyFactory<out TKey>
    {
        /// <summary>
        /// Gets whether the key factory supports distributed usage.
        /// </summary>
        Boolean SupportsDistribution { get; }

        /// <summary>
        /// Gets the initially created key.
        /// </summary>
        TKey FirstKey { get; }

        /// <summary>
        /// Gets the last created key.
        /// </summary>
        TKey LastKey { get; }

        /// <summary>
        /// Creates a new key based on a context.
        /// </summary>
        /// <remarks>
        /// The factory should never produce the <c>default(TKey)</c> as a new key, 
        /// since it is used as an extremal value.
        /// </remarks>
        /// <param name="context">The optional object context to create the key from.</param>
        /// <returns>The key generated based on the context.</returns>
        TKey CreateKey(Object context = null);
    }
}
