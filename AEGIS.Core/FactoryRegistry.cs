/// <copyright file="FactoryRegistry.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Management.InversionOfControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a global registry of factories.
    /// </summary>
    /// <remarks>
    /// This type realizes a service locator pattern, and represents a storage of factory behaviors based on contract.
    /// The registry is by default configured for automatic location of factories in loaded assemblies. This behavior can be modified by the <see cref="AutoLocation"/> property.
    /// </remarks>
    public static class FactoryRegistry
    {
        #region Private types

        /// <summary>
        /// Represents the implementation of factory registry operations.
        /// </summary>
        /// <remarks>
        /// This type realizes the services which <see cref="FactoryRegistry" /> provides.
        /// </remarks>
        private class FactoryRegistryImplementation : IDisposable
        {
            #region Private fields

            /// <summary>
            /// The singleton instance.
            /// </summary>
            private static readonly Lazy<FactoryRegistryImplementation> _instance;

            /// <summary>
            /// The IoC container.
            /// </summary>
            private readonly ConcurrentContainer _container;

            /// <summary>
            /// A value indicating whether this instance is disposed.
            /// </summary>
            private volatile Boolean _disposed;

            /// <summary>
            /// A value indicating whether factories are automatically located.
            /// </summary>
            private volatile Boolean _autoLocation;

            #endregion

            #region Constructors and finalizer

            /// <summary>
            /// Initializes the <see cref="FactoryRegistryImplementation" /> class.
            /// </summary>
            static FactoryRegistryImplementation()
            {
                _instance = new Lazy<FactoryRegistryImplementation>(() => new FactoryRegistryImplementation());
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FactoryRegistryImplementation" /> class.
            /// </summary>
            private FactoryRegistryImplementation()
            {
                _container = new ConcurrentContainer();
                _autoLocation = false;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="FactoryRegistryImplementation" /> class.
            /// </summary>
            ~FactoryRegistryImplementation()
            {
                Dispose(false);
            }

            #endregion

            #region Public properties

            /// <summary>
            /// Gets the instance of the factory registry implementation.
            /// </summary>
            /// <value>The instance of the factory registry implementation.</value>
            public static FactoryRegistryImplementation Instance { get { return _instance.Value; } }

            /// <summary>
            /// Gets a value indicating whether factories are automatically located.
            /// </summary>
            /// <value><c>true</c> if factories are automatically located; otherwise <c>false</c>.</value>
            public Boolean AutoLocation { get { return _autoLocation; } }

            #endregion

            #region Public methods

            /// <summary>
            /// Returns the default instance for a specified factory contract.
            /// </summary>
            /// <param name="factoryContract">The factory contract.</param>
            /// <returns>The default instance for the factory or <c>null</c> if the factory is unresolvable.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            public IFactory GetFactory(Type factoryContract)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (_container.IsRegistered(factoryContract))
                {
                    return _container.Resolve(factoryContract) as IFactory;
                }

                // in case it is not registered try to register
                RegisterFactoryContract(factoryContract, false);

                if (_container.IsRegistered(factoryContract))
                {
                    return _container.Resolve(factoryContract) as IFactory;
                }

                // it cannot be registered
                return null;
            }

            /// <summary>
            /// Returns the instance of the factory with the specified parameters.
            /// </summary>
            /// <param name="factoryContract">The factory contract.</param>
            /// <param name="parameters">The parameters.</param>
            /// <returns>The factory instance with the specified parameters or <c>null</c> if the factory is unresolvable.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            public IFactory GetFactory(Type factoryContract, params Object[] parameters)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                try
                {
                    return _container.Resolve(factoryContract, parameters) as IFactory;
                }
                catch (ArgumentException) // the type has no instance with the specified parameters
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the default factory instance for a specified product.
            /// </summary>
            /// <param name="productType">The type of the product.</param>
            /// <returns>The default factory instance for the factory or <c>null</c> if the product is unresolvable.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            public IFactory GetFactoryFor(Type productType)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (!_container.IsRegistered(productType.FullName))
                    return null;

                try
                {
                    return _container.Resolve(productType.FullName) as IFactory;
                }
                catch (ArgumentException) // the type has no default instance
                {
                    return null;
                }
            }

            /// <summary>
            /// Returns the instance of the factory with the specified parameters.
            /// </summary>
            /// <param name="productType">The type of the product.</param>
            /// <param name="parameters">The parameters.</param>
            /// <returns>The factory instance with the specified parameters or <c>null</c> if the product is unresolvable.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            public IFactory GetFactoryFor(Type productType, params Object[] parameters)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (!_container.IsRegistered(productType.FullName))
                    return null;

                try
                {
                    return _container.Resolve(productType.FullName, parameters) as IFactory;
                }
                catch (ArgumentException) // the type has no instance with the specified parameters
                {
                    return null;
                }
            }

            /// <summary>
            /// Registers the factories located in all loaded assemblies.
            /// </summary>
            public void RegisterFactories()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                // register all loaded assemblies
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    RegisterFactories(assembly);
            }

            /// <summary>
            /// Registers the factories located in the specified assembly.
            /// </summary>
            /// <param name="assembly">The assembly.</param>
            public void RegisterFactories(Assembly assembly)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                Type[] types = assembly.GetTypes();

                // query the factory contracts
                foreach (Type factoryContract in types.Where(type => type.IsInterface && type.GetInterfaces().Contains(typeof(IFactory))))
                {
                    RegisterFactoryContract(factoryContract, false);
                }

                // query the factory behaviors
                foreach (Type factoryBehavior in types.Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof(IFactory))))
                {
                    RegisterFactoryBehavior(factoryBehavior);
                }
            }

            /// <summary>
            /// Sets the default behavior of a factory.
            /// </summary>
            /// <param name="factoryContract">The factory contract.</param>
            /// <param name="factoryBehavior">The factory behavior.</param>
            public void SetFactory(Type factoryContract, Type factoryBehavior)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                RegisterFactoryContract(factoryContract, true);
            }

            /// <summary>
            /// Starts the automatic location of factories.
            /// </summary>
            public void StartAutoLocation()
            {
                if (_autoLocation)
                    return;

                _autoLocation = true;

                try
                {

                    RegisterFactories();

                    AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
                }
                catch (SecurityException)
                {
                    _autoLocation = false;
                }
            }

            /// <summary>
            /// Stops the automatic location of factories.
            /// </summary>
            public void StopAutoLocation()
            {
                if (!_autoLocation)
                    return;

                _autoLocation = false;

                AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Registers the factory contract.
            /// </summary>
            /// <param name="factoryContract">The factory contract.</param>
            private void RegisterFactoryContract(Type factoryContract, Boolean overwrite)
            {
                if (!overwrite && _container.IsRegistered(factoryContract))
                    return;

                FactoryContractAttribute factoryContractAttribute = factoryContract.GetCustomAttribute(typeof(FactoryContractAttribute), false) as FactoryContractAttribute;

                // the contract can only be registered if the attribute exists
                if (factoryContractAttribute != null)
                {
                    // if a previously registered instance exists, it should be removed
                    if (_container.IsRegisteredInstance(factoryContract))
                    {
                        _container.UnregisterInstance(factoryContract);
                    }

                    _container.Register(factoryContract, factoryContractAttribute.DefaultBehavior, true);
                    _container.Register(factoryContractAttribute.Product.FullName,
                                        factoryContract, 
                                        factoryContractAttribute.DefaultBehavior, true);
                }
            }

            /// <summary>
            /// Registers the factory behavior.
            /// </summary>
            /// <param name="factoryBehavior">The factory behavior.</param>
            private void RegisterFactoryBehavior(Type factoryBehavior)
            {
                foreach (Type factoryContract in factoryBehavior.GetInterfaces().Where(inter => inter.GetInterfaces().Contains(typeof(IFactory))))
                {
                    RegisterFactoryContract(factoryContract, false); // register the contract first

                    // the registration should be overwritten if it exists (for example, the assembly was refreshed)
                    _container.Register(factoryContract.FullName + ";" + factoryBehavior.FullName, factoryContract, factoryBehavior, true);
                }
            }

            /// <summary>
            /// Handles the AssemblyLoad event of the CurrentDomain control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="args">The <see cref="AssemblyLoadEventArgs"/> instance containing the event data.</param>
            private void CurrentDomain_AssemblyLoad(Object sender, AssemblyLoadEventArgs args)
            {
                RegisterFactories(args.LoadedAssembly); // register the factories from the newly loaded assembly
            }

            #endregion

            #region Protected methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            protected void Dispose(Boolean disposing)
            {
                _disposed = true;

                if (disposing)
                {
                    // free managed resources
                    StopAutoLocation();
                    _container.Dispose();
                }
            }

            #endregion
        }

        #endregion

        #region Private constant fields

        /// <summary>
        /// Exception message in case the factory behavior is null. This field is constant.
        /// </summary>
        private const String FactoryBehaviorIsNull = "The factory behavior is null.";

        /// <summary>
        /// Exception message in case the factory contract is null. This field is constant.
        /// </summary>
        private const String FactoryContractIsNull = "The factory contract is null.";

        /// <summary>
        /// Exception message in case the factory is null. This field is constant.
        /// </summary>
        private const String FactoryIsNull = "The factory is null.";

        /// <summary>
        /// Exception message in case the specified factory has no default behavior. This field is constant.
        /// </summary>
        private const String NoDefaultBehavior = "The specified factory has no default behavior.";

        /// <summary>
        /// Exception message in case the specified product has no default factory instance. This field is constant.
        /// </summary>
        private const String NoDefaultBehaviorForProduct = "The specified product has no default factory behavior.";

        /// <summary>
        /// Exception message in case the specified factory has no behavior with the specified parameters. This field is constant.
        /// </summary>
        private const String NoParameterizedBehavior = "The specified factory has no behavior with the specified parameters.";

        /// <summary>
        /// Exception message in case the specified product has no factory instance with the specified parameters. This field is constant.
        /// </summary>
        private const String NoParameterizedBehaviorForProduct = "The specified product has no factory with the specified parameters.";

        /// <summary>
        /// Exception message in case the product type is null. This field is constant.
        /// </summary>
        private const String ProductTypeIsNull = "The product type is null.";

        #endregion

        #region Private static fields

        /// <summary>
        /// A value indicating whether the registry is automatically registering factories.
        /// </summary>
        private static volatile Boolean _autoLocation = true;

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets or sets a value indicating whether the registry is automatically locating factories.
        /// </summary>
        /// <value><c>true</c> if factories are automatically located and registered; otherwise, <c>false</c>.</value>
        public static Boolean AutoLocation
        {
            get { return _autoLocation; }
            set
            {
                if (_autoLocation != value)
                {
                    _autoLocation = value;

                    if (!_autoLocation)
                        FactoryRegistryImplementation.Instance.StopAutoLocation();
                }
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Returns the contract of the specified factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>The contract of the specified factory.</returns>
        /// <exception cref="System.ArgumentNullException">The factory is null.</exception>
        public static Type GetContract(IFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory", FactoryIsNull);

            return GetContract(factory.GetType());
        }

        /// <summary>
        /// Returns the contract of the specified factory behavior.
        /// </summary>
        /// <param name="factoryBehavior">The factory behavior.</param>
        /// <returns>The contract of the specified factory behavior.</returns>
        /// <exception cref="System.ArgumentNullException">The factory behavior is null.</exception>
        public static Type GetContract(Type factoryBehavior)
        {
            if (factoryBehavior == null)
                throw new ArgumentNullException("factoryBehavior", FactoryBehaviorIsNull);

            return factoryBehavior.GetInterfaces().FirstOrDefault(inter => inter.GetInterfaces().Contains(typeof(IFactory)));
        }

        /// <summary>
        /// Returns the default factory behavior for the specified factory contract.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <returns>The default behavior instance for the specified factory contract.</returns>
        /// <exception cref="System.InvalidOperationException">The specified factory has no default behavior.</exception>
        public static FactoryContract GetFactory<FactoryContract>() where FactoryContract : IFactory
        {
            return (FactoryContract)GetFactory(typeof(FactoryContract));
        }

        /// <summary>
        /// Returns the factory behavior of the specified contract based on the provided parameters.
        /// </summary>
        /// <typeparam name="FactoryContract">The factory contract.</typeparam>
        /// <param name="parameters">The parameters used for factory behavior instantiation.</param>
        /// <returns>The behavior instance for the specified factory contract with the provided parameters.</returns>
        /// <exception cref="System.InvalidOperationException">The specified factory has no behavior with the specified parameters.</exception>
        public static FactoryContract GetFactory<FactoryContract>(params Object[] parameters) where FactoryContract : IFactory
        {
            return (FactoryContract)GetFactory(typeof(FactoryContract), parameters);
        }

        /// <summary>
        /// Returns the default factory behavior for the specified factory contract.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</typeparam>
        /// <returns>The default behavior instance for the specified factory contract.</returns>
        /// <exception cref="System.ArgumentNullException">The factory type is null.</exception>
        /// <exception cref="System.InvalidOperationException">The specified factory has no default behavior.</exception>
        public static IFactory GetFactory(Type factoryContract)
        {
            if (factoryContract == null)
                throw new ArgumentNullException("factoryContract", FactoryContractIsNull);

            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();

            IFactory factory = FactoryRegistryImplementation.Instance.GetFactory(factoryContract);

            if (factory == null)
                throw new InvalidOperationException(NoDefaultBehavior);

            return factory;
        }

        /// <summary>
        /// Returns the factory behavior of the specified contract based on the provided parameters.
        /// </summary>
        /// <param name="factoryContract">The factory contract.</typeparam>
        /// <param name="parameters">The parameters used for factory behavior instantiation.</param>
        /// <returns>The behavior instance for the specified factory contract with the provided parameters.</returns>
        /// <exception cref="System.ArgumentNullException">The factory type is null.</exception>
        /// <exception cref="System.InvalidOperationException">The specified factory has no behavior with the specified parameters.</exception>
        public static IFactory GetFactory(Type factoryContract, params Object[] parameters)
        {
            if (factoryContract == null)
                throw new ArgumentNullException("factoryType", FactoryContractIsNull);

            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();

            IFactory factory = FactoryRegistryImplementation.Instance.GetFactory(factoryContract, parameters);

            if (factory == null)
                throw new InvalidOperationException(NoParameterizedBehavior);

            return factory;
        }

        /// <summary>
        /// Returns the default factory behavior for the specified product.
        /// </summary>
        /// <typeparam name="ProductType">The type of the product.</param>
        /// <returns>The default factory behavior instance for the specified product.</returns>
        /// <exception cref="System.InvalidOperationException">The specified product has no default factory behavior.</exception>
        public static IFactory GetFactoryFor<ProductType>()
        {
            return GetFactoryFor(typeof(ProductType));
        }

        /// <summary>
        /// Returns the factory instance for the product with the specified parameters.
        /// </summary>
        /// <typeparam name="ProductType">The type of the product.</typeparam>
        /// <param name="parameters">The parameters used for factory instantiation.</param>
        /// <returns>The behavior instance for the specified factory contract with the provided parameters.</returns>
        /// <exception cref="System.InvalidOperationException">The specified product has no factory with the specified parameters.</exception>
        public static IFactory GetFactoryFor<ProductType>(params Object[] parameters)
        {
            return GetFactoryFor(typeof(ProductType), parameters);
        }

        /// <summary>
        /// Returns the default factory behavior for the specified product.
        /// </summary>
        /// <param name="productType">The type of the product.</param>
        /// <returns>The default factory behavior instance for the specified product.</returns>
        /// <exception cref="System.ArgumentNullException">The product type is null.</exception>
        /// <exception cref="System.InvalidOperationException">The specified product has no default factory behavior.</exception>
        public static IFactory GetFactoryFor(Type productType)
        {
            if (productType == null)
                throw new ArgumentNullException("productType", ProductTypeIsNull);

            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();

            IFactory factory = FactoryRegistryImplementation.Instance.GetFactoryFor(productType);

            if (factory == null)
                throw new InvalidOperationException(NoDefaultBehaviorForProduct);

            return factory;
        }

        /// <summary>
        /// Returns the factory instance for the product with the specified parameters.
        /// </summary>
        /// <param name="productType">The type of the product.</typeparam>
        /// <param name="parameters">The parameters used for factory instantiation.</param>
        /// <returns>The instance of <paramref name="factoryType"/> with the specified parameters.</returns>
        /// <exception cref="System.ArgumentNullException">The product type is null.</exception>
        /// <exception cref="System.InvalidOperationException">The specified product has no factory with the specified parameters.</exception>
        public static IFactory GetFactoryFor(Type productType, params Object[] parameters)
        {
            if (productType == null)
                throw new ArgumentNullException("productType", ProductTypeIsNull);

            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();

            IFactory factory = FactoryRegistryImplementation.Instance.GetFactoryFor(productType, parameters);

            if (factory == null)
                throw new InvalidOperationException(NoParameterizedBehaviorForProduct);

            return factory;
        }

        /// <summary>
        /// Registers the specified factory behavior for the contract.
        /// </summary>
        public static void Register<FactoryContract, FactoryBehavior>()
            where FactoryBehavior : FactoryContract
            where FactoryContract : IFactory
        {
            Register(typeof(FactoryContract), typeof(FactoryBehavior));
        }


        /// <summary>
        /// Registers the factories located in all loaded assemblies.
        /// </summary>
        public static void Register()
        {
            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();
        }

        /// <summary>
        /// Registers the factories located in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public static void Register(Assembly assembly)
        {
            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();

            FactoryRegistryImplementation.Instance.RegisterFactories(assembly);
        }

        /// <summary>
        /// Registers the specified factory behavior for the contract.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// The factory contract is null.
        /// or
        /// The factory behavior is null.
        /// </exception>
        public static void Register(Type factoryContract, Type factoryBehavior)
        {
            if (factoryContract == null)
                throw new ArgumentNullException("factoryContract", FactoryContractIsNull);
            if (factoryBehavior == null)
                throw new ArgumentNullException("factoryBehavior", FactoryBehaviorIsNull);

            if (_autoLocation && !FactoryRegistryImplementation.Instance.AutoLocation)
                FactoryRegistryImplementation.Instance.StartAutoLocation();

            FactoryRegistryImplementation.Instance.SetFactory(factoryContract, factoryBehavior);
        }

        #endregion
    }
}
