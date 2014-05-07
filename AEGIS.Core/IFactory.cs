/// <copyright file="IFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines general behavior of factories.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Return the product type of the factory.
        /// </summary>
        /// <value>The type of the product.</value>
        Type ProductType { get; }

        /// <summary>
        /// Gets the internal factories.
        /// </summary>
        /// <value>The list of internal factories.</value>
        IList<IFactory> Factories { get; }

        /// <summary>
        /// Returns the specified internal factory.
        /// </summary>
        /// <typeparam name="T">TThe type of the factory.</typeparam>
        /// <returns>The internal factory instance for the specified type if any; otherwise, <c>null</c>.</returns>
        T GetFactory<T>() where T : IFactory; 

        /// <summary>
        /// Returns the specified internal factory.
        /// </summary>
        /// <param name="factoryType">The type of the factory.</param>
        /// <returns>The internal factory instance for the specified type if any; otherwise, <c>null</c>.</returns>
        IFactory GetFactory(Type factoryType);

        /// <summary>
        /// Returns the factory of the specified product.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <returns>The internal factory for the specified product if any; otherwise, <c>null</c>.</returns>
        IFactory GetFactoryFor<T>();

        /// <summary>
        /// Returns the factory of the specified product.
        /// </summary>
        /// <param name="productType">The product type.</param>
        /// <returns>The internal factory for the specified product if any; otherwise, <c>null</c>.</returns>
        IFactory GetFactoryFor(Type productType);

        /// <summary>
        /// Defines the factory of the specified product.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <param name="factory">The factory.</param>
        void SetFactoryFor<T>(IFactory factory);

        /// <summary>
        /// Determines whether the factory contains an internal factory for the specified product type.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <returns><c>true</c> if the factory contains an internal factory for the specified product; otherwise, <c>false</c>.</returns>
        Boolean ContainsFactoryFor<T>();

        /// <summary>
        /// Determines whether the factory contains an internal factory for the specified product type.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <returns><c>true</c> if the factory contains an internal factory for the specified product; otherwise, <c>false</c>.</returns>
        Boolean ContainsFactoryFor(Type productType);
    }
}
