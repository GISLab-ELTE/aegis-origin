/// <copyright file="IFactory.cs" company="Eötvös Loránd University (ELTE)">
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
        /// Gets the directly underlying factories.
        /// </summary>
        /// <value>The read-only list of directly underlying factories.</value>
        IReadOnlyList<IFactory> Factories { get; }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        Boolean ContainsFactory<FactoryContract>();

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        Boolean ContainsFactory(Type factoryContract);

        /// <summary>
        /// Ensures the specified underlying factory.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</typeparam>
        /// <param name="factory">The factory behavior.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory contract is null.
        /// or
        /// The factory behavior is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The factory behavior does not implement the contract.</exception>
        /// <exception cref="System.InvalidOperationException">The factory contract is already ensured.</exception>
        void EnsureFactory<FactoryContract>(FactoryContract factory) where FactoryContract : IFactory;

        /// <summary>
        /// Ensures the specified underlying factory.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</typeparam>
        /// <param name="factory">The factory behavior.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The factory contract is null.
        /// or
        /// The factory behavior is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The factory behavior does not implement the contract.</exception>
        void EnsureFactory(Type factoryContract, IFactory factory);

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        FactoryContract GetFactory<FactoryContract>() where FactoryContract : IFactory;

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <param name="FactoryContract">The factory contract.</typeparam>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        IFactory GetFactory(Type factoryContract);
    }
}
