// <copyright file="SequentialKeyFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Versioning.Keys
{
    /// <summary>
    /// Key factory class for versioning based on sequentially incrementing numbers keys.
    /// </summary>
    /// <remarks>
    /// This key type does not support distribution.
    /// </remarks>
    /// <author>Máté Cserép</author>
    public class SequentialKeyFactory : IKeyFactory<Int32>
    {
        #region IKeyFactory properties

        /// <summary>
        /// Gets whether the key factory supports distributed usage.
        /// </summary>
        public Boolean SupportsDistribution
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the initially created key.
        /// </summary>
        public Int32 FirstKey { get; protected set; }

        /// <summary>
        /// Gets the last created key.
        /// </summary>
        public Int32 LastKey { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an instance of the <see cref="Factory"/> class.
        /// </summary>
        /// <param name="seed">The seed value for the sequential key generation.</param>
        /// <exception cref="System.ArgumentException">The seed should be positive, since 0 is used as an extremal value.</exception>
        public SequentialKeyFactory(Int32 seed = 1)
        {
            if(seed < 1)
                throw new ArgumentException("The seed should be positive.", "seed");
            FirstKey = LastKey = seed;
        }

        #endregion

        #region IKeyFactory methods

        /// <summary>
        /// Creates a new key based on a context.
        /// </summary>
        /// <returns>The key generated based on the context.</returns>
        public Int32 CreateKey()
        {
            return LastKey = LastKey + 1;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates a new key based on a context.
        /// </summary>
        /// <remarks>
        /// The key creation process is independent from the given object context.
        /// </remarks>
        /// <param name="context">The optional object context to create the key from.</param>
        /// <returns>The key generated based on the context.</returns>
        Int32 IKeyFactory<Int32>.CreateKey(Object context)
        {
            return CreateKey();
        }

        #endregion
    }
}
