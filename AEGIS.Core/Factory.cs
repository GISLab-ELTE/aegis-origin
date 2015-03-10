/// <copyright file="Factory.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Management.InversionOfControl;
using ELTE.AEGIS.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a manager of all factories.
    /// </summary>
    /// <remarks>
    /// This type realizes a service locator pattern, and represents a storage of factories including the default implementation of factory types, and the default factory instances.
    /// The type can also be used as base class for all factories which may use internal factories.
    /// </remarks>
    public abstract class Factory : IFactory
    {
        #region Private constant fields

        /// <summary>
        /// Exception message in case the factory behavior is null. This field is constant.
        /// </summary>
        private const String MessageFactoryBehaviorIsNull = "The factory behavior is null.";

        /// <summary>
        /// Exception message in case the factory contract is null. This field is constant.
        /// </summary>
        private const String MessageFactoryContractIsNull = "The factory contract is null.";

        /// <summary>
        /// Exception message in case the factory contract is alread ensured. This field is constant.
        /// </summary>
        private const String MessageFactoryIsEnsured = "The factory contract is already ensured.";

        /// <summary>
        /// Exception message in case the factory behavior does not implement the contract. This field is constant.
        /// </summary>
        private const String MessageFactoryNotImplementContract = "The factory behavior does not implement the contract.";

        /// <summary>
        /// Exception message in case the product type is null. This field is constant.
        /// </summary>
        private const String MessageProductTypeIsNull = "The product type is null.";

        #endregion

        #region Private fields

        /// <summary>
        /// The dictionary of factories based on factory contract.
        /// </summary>
        private Dictionary<Type, IFactory> _contractDictionary;
        
        /// <summary>
        /// The dictionary of factories based on product type.
        /// </summary>
        private Dictionary<Type, IFactory> _productDictionary;

        /// <summary>
        /// The list of factories.
        /// </summary>
        private List<IFactory> _factories;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        protected Factory()
        {
            _contractDictionary = new Dictionary<Type, IFactory>();
            _productDictionary = new Dictionary<Type,IFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        /// <param name="factories">The underlying factories based on factory contract.</param>
        protected Factory(params IFactory[]  factories)
        {
            _contractDictionary = new Dictionary<Type, IFactory>();
            _productDictionary = new Dictionary<Type, IFactory>();

            foreach (IFactory factory in factories)
            { 
                foreach(Type factoryContract in factory.GetType().GetInterfaces().Where(inter => inter.GetInterfaces().Contains(typeof(IFactory))))
                {
                    _contractDictionary.Add(factoryContract, factory);

                    FactoryContractAttribute attribute = factoryContract.GetCustomAttribute(typeof(FactoryContractAttribute), false) as FactoryContractAttribute;

                    if (attribute != null)
                        _productDictionary.Add(attribute.Product, factory);
                }
            }
        }

        #endregion

        #region IFactory properties

        /// <summary>
        /// Gets the directly underlying factories.
        /// </summary>
        /// <value>The read-only list of direcly underlying factories.</value>
        public IList<IFactory> Factories { get { return _factories ?? (_factories = _contractDictionary.Values.ToList()); } }

        #endregion

        #region IFactory methods

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        public Boolean ContainsFactory<FactoryContract>()
        {
            return ContainsFactory(typeof(FactoryContract));
        }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</param>
        /// <returns><c>true</c> if an underlying factory exists for the specified contract; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        public Boolean ContainsFactory(Type factoryContract)
        {
            if (factoryContract == null)
                throw new ArgumentNullException("factoryContract", MessageFactoryBehaviorIsNull);

            return (_contractDictionary.ContainsKey(factoryContract)) || _contractDictionary.Values.Any(factory => factory.ContainsFactory(factoryContract));
        }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified product.
        /// </summary>
        /// <typeparam name="ProductType">The product type.</typeparam>
        /// <returns><c>true</c> if an underlying factory exists for the specified product; otherwise, <c>false</c>.</returns>
        public Boolean ContainsFactoryFor<ProductType>()
        {
            return ContainsFactoryFor(typeof(ProductType));
        }

        /// <summary>
        /// Determines whether an underlying factory behavior exists for the specified product.
        /// </summary>
        /// <param name="productType">The product type.</param>
        /// <returns><c>true</c> if an underlying factory exists for the specified product; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The product type is null.</exception>
        public Boolean ContainsFactoryFor(Type productType)
        {
            if (productType == null)
                throw new ArgumentNullException("productType", MessageProductTypeIsNull);

            return (_productDictionary.ContainsKey(productType)) || _productDictionary.Values.Any(factory => factory.ContainsFactoryFor(productType));
        }

        /// <summary>
        /// Ensures the specified underlying factory.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <param name="factory">The factory behavior.</param>
        /// <exception cref="System.ArgumentNullException">The factory realization is null.</exception>
        public void EnsureFactory<FactoryContract>(FactoryContract factory) where FactoryContract : IFactory
        {
            EnsureFactory(typeof(FactoryContract), factory);
        }

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
        public void EnsureFactory(Type factoryContract, IFactory factory)
        {
            if (factoryContract == null)
                throw new ArgumentNullException("factoryContract", MessageFactoryBehaviorIsNull);
            if (factory == null)
                throw new ArgumentNullException("factory", MessageFactoryBehaviorIsNull);
            if (!factory.GetType().GetInterfaces().Contains(factoryContract))
                throw new ArgumentException("factory", MessageFactoryNotImplementContract);

            if (_contractDictionary.ContainsKey(factoryContract))
                throw new InvalidOperationException(MessageFactoryIsEnsured);

            _contractDictionary.Add(factoryContract, factory);


            FactoryContractAttribute attribute = factoryContract.GetCustomAttribute(typeof(FactoryContractAttribute), false) as FactoryContractAttribute;

            if (attribute != null)
                _productDictionary.Add(attribute.Product, factory);
        }

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        public FactoryContract GetFactory<FactoryContract>() where FactoryContract : IFactory
        {
            return (FactoryContract)GetFactory(typeof(FactoryContract));
        }

        /// <summary>
        /// Returns the underlying factory behavior of the specified contract.
        /// </summary>
        /// <param name="FactoryContract">The factory contract.</typeparam>
        /// <returns>The factory behavior for the specified contract if any; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The factory contract is null.</exception>
        public IFactory GetFactory(Type factoryContract)
        {
            if (factoryContract == null)
                throw new ArgumentNullException("factoryContract", MessageFactoryBehaviorIsNull);

            if (_contractDictionary.ContainsKey(factoryContract))
                return _contractDictionary[factoryContract];

            // recursively process all undelying factories
            foreach (IFactory factory in _contractDictionary.Values)
            {
                if (factory.ContainsFactory(factoryContract))
                    return factory.GetFactory(factoryContract);
            }

            // no factory found
            return null;
        }


        /// <summary>
        /// Returns the underlying factory behavior of the specified product.
        /// </summary>
        /// <typeparam name="ProductType">The product type.</typeparam>
        /// <returns>The factory behavior for the specified product if any; otherwise, <c>null</c>.</returns>
        public IFactory GetFactoryFor<ProductType>()
        {
            return GetFactoryFor(typeof(ProductType));
        }

        /// <summary>
        /// Returns the underlying factory behavior of the specified product.
        /// </summary>
        /// <param name="productType">The product type.</param>
        /// <returns>The factory behavior for the specified product if any; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The product type is null.</exception>
        public IFactory GetFactoryFor(Type productType)
        {
            if (productType == null)
                throw new ArgumentNullException("productType", MessageProductTypeIsNull);

            if (_productDictionary.ContainsKey(productType))
                return _productDictionary[productType];

            // recursively process all undelying factories
            foreach (IFactory factory in _contractDictionary.Values)
            {
                if (factory.ContainsFactoryFor(productType))
                    return factory.GetFactoryFor(productType);
            }

            // no factory found
            return null;
        }

        #endregion
    }
}
