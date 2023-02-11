// <copyright file="GuidKeyFactory.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Key factory class for versioning based on GUID keys.
    /// </summary>
    /// <remarks>
    /// This key type supports distribution.
    /// </remarks>
    /// <seealso cref="System.Guid"/>
    /// <author>Máté Cserép</author>
    public class GuidKeyFactory : IKeyFactory<Guid>
    {
        #region IKeyFactory properties

        /// <summary>
        /// Gets whether the key factory supports distributed usage.
        /// </summary>
        public Boolean SupportsDistribution
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the initially created key.
        /// </summary>
        public Guid FirstKey { get; protected set; }

        /// <summary>
        /// Gets the last created key.
        /// </summary>
        public Guid LastKey { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an instance of the <see cref="Factory"/> class.
        /// </summary>
        public GuidKeyFactory()
        {
            FirstKey = LastKey = new Guid();
        }

        #endregion

        #region IKeyFactory methods

        /// <summary>
        /// Creates a new key based on a context.
        /// </summary>
        /// <returns>The key generated based on the context.</returns>
        public Guid CreateKey()
        {
            return LastKey = Guid.NewGuid();
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
        Guid IKeyFactory<Guid>.CreateKey(Object context)
        {
            return CreateKey();
        }

        #endregion
    }
}
