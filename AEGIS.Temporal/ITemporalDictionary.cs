/// <copyright file="Instant.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Represents a temporal collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictinary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictinary.</typeparam>
    interface ITemporalDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Gets or sets the element with the specified key and temporal position.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <param name="position">The temporal position of the element.</param>
        /// <returns>The element with the specified key and temporal position.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found at the specified temporal position.</exception>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="ITemporalDictionary<TKey, TValue>" /> is read-only.</exception>
        TValue this[TKey key, Instant position] { get; set; }

        /// <summary>
        /// Sets the element with the specified key and temporal period.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <param name="period">The temporal period of the element.</param>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="ITemporalDictionary<TKey, TValue>" /> is read-only.</exception>
        TValue this[TKey key, Period period] { set; }

        /// <summary>
        /// Adds an element with the provided key, time and value to the <see cref="ITemporalDictionary<TKey, TValue>" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <param name="position">The temporal position at which the key/value pair is considered to be valid.</param>
        void Add(TKey key, TValue value, Instant position);

        /// <summary>
        /// Adds an element with the provided key, time and value to the <see cref="ITemporalDictionary<TKey, TValue>" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <param name="period">The temporal period at which the key/value pair is considered to be valid.</param>
        void Add(TKey key, TValue value, Period period);

        Boolean ContainsKey(TKey key, Instant position);
        Boolean ContainsKey(TKey key, Period period);
        Boolean Remove(TKey key, Instant position);
        Boolean Remove(TKey key, Period period);
        Boolean TryGetValue(TKey key, Instant position, out TValue value);
    }
}
