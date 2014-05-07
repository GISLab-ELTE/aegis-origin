/// <copyright file="ISearchTreeEnumerator.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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

namespace ELTE.AEGIS.Collections.SearchTree
{
    /// <summary>
    /// Supports a multidirection iteration over a <see cref="ISearchTree" /> collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface ISearchTreeEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Advances the enumerator to the previous element of the collection.
        /// </summary>
        /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
        Boolean MovePrev();

        /// <summary>
        /// Advances the enumerator to the minimal element of the collection.
        /// </summary>
        /// <returns><c>true</c> if the enumerator was successfully advanced to the minimal element; <c>false</c> if the collection is empty.</returns>
        Boolean MoveMin();

        /// <summary>
        /// Advances the enumerator to the maximal element of the collection.
        /// </summary>
        /// <returns><c>true</c> if the enumerator was successfully advanced to the maximal element; <c>false</c> if the collection is empty.</returns>
        Boolean MoveMax();

        /// <summary>
        /// Advances the enumerator to the root element of the collection.
        /// </summary>
        /// <returns><c>true</c> if the enumerator was successfully advanced to the root element; <c>false</c> if the collection is empty.</returns>
        Boolean MoveRoot();
    }
}
