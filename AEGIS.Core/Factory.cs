/// <copyright file="Factory.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Geometry;
using ELTE.AEGIS.Management.InversionOfControl;
using ELTE.AEGIS.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a manager of all factories.
    /// </summary>
    /// <remarks>
    /// This type represents a storage of used factories including the default implementation of factory types, and the default factory instances.
    /// </remarks>
    public abstract class Factory : IFactory
    {
        #region Private types

        /// <summary>
        /// Represents the implementation of factory management operations.
        /// </summary>
        /// <remarks>
        /// This type realizes the services which <see cref="Factory" /> provides.
        /// </remarks>
        private class FactoryImplementation : IDisposable
        {
            #region Singleton pattern

            private static readonly Lazy<FactoryImplementation> _instance;

            /// <summary>
            /// Gets the instance of the factory management implementation.
            /// </summary>
            /// <value>The instance of the factory management implementation.</value>
            public static FactoryImplementation Instance { get { return _instance.Value; } }

            #endregion

            #region Private fields

            private readonly Container _defaultContainer;
            private Int32 _version;
            private Boolean _disposed;

            #endregion

            #region Constructors and destructor

            /// <summary>
            /// Initializes the <see cref="FactoryImplementation" /> class.
            /// </summary>
            static FactoryImplementation()
            {
                _instance = new Lazy<FactoryImplementation>(() => new FactoryImplementation());
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FactoryImplementation" /> class.
            /// </summary>
            private FactoryImplementation()
            {
                MetadataFactory metadataFactory = new MetadataFactory();
                GeometryFactory geometryFactory = new GeometryFactory(null, metadataFactory);

                _defaultContainer = new Container();

                _defaultContainer.Register<IMetadataFactory, MetadataFactory>();
                _defaultContainer.Register<MetadataFactory, MetadataFactory>();
                _defaultContainer.Register<IGeometryFactory, GeometryFactory>();
                _defaultContainer.Register<GeometryFactory, GeometryFactory>();

                _defaultContainer.RegisterInstance<IMetadataFactory>(metadataFactory);
                _defaultContainer.RegisterInstance<MetadataFactory>(metadataFactory);
                _defaultContainer.RegisterInstance<IGeometryFactory>(geometryFactory);
                _defaultContainer.RegisterInstance<GeometryFactory>(geometryFactory);

                _version = 0;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="FactoryImplementation" /> class.
            /// </summary>
            ~FactoryImplementation()
            {
                Dispose(false);
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Returns the default instance for a specified factory.
            /// </summary>
            /// <param name="factoryType">The type of the factory.</param>
            /// <returns>The default registered instance for the factory or the default type value if the factory is unresolvable.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            public Object DefaultInstance(Type factoryType)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                try
                {
                    Object defaultInstance;
                    Int32 version, nextVersion;

                    do
                    {
                        version = _version;
                        nextVersion = version;

                        if (!_defaultContainer.IsRegisteredInstance(factoryType))
                        {
                            // if the instance is not registered
                            if (!_defaultContainer.IsRegistered(factoryType))
                            {
                                // if the contract is not registered
                                if (factoryType.IsInterface || factoryType.IsAbstract)
                                {
                                    // if the contract cannot be resolved
                                    defaultInstance = factoryType.IsValueType ? Activator.CreateInstance(factoryType) : null;
                                    continue;
                                }

                                // if the contract is not registered, and can be registered, it should be
                                _defaultContainer.Register(factoryType, factoryType);

                                // also, all interfaces that are not registered, should be
                                foreach(Type interfaceType in factoryType.GetInterfaces())
                                {
                                    if (!_defaultContainer.IsRegistered(interfaceType))
                                        _defaultContainer.Register(interfaceType, factoryType);
                                }

                                Interlocked.Increment(ref _version);
                                nextVersion++;
                            }

                            _defaultContainer.RegisterInstance(factoryType, _defaultContainer.Resolve(factoryType));
                            // the instance is registered
                        }

                        defaultInstance = _defaultContainer.ResolveInstance(factoryType);

                    } while (nextVersion != Interlocked.CompareExchange(ref _version, _version, nextVersion));

                    // the registered instance is resolved
                    return defaultInstance;
                }
                catch
                {
                    // either the type of the instance cannot be registered
                    return factoryType.IsValueType ? Activator.CreateInstance(factoryType) : null;
                }
            }

            /// <summary>
            /// Returns the instance of the factory with the specified parameters.
            /// </summary>
            /// <param name="factoryType">The type of the factory.</param>
            /// <param name="parameters">The parameters.</param>
            /// <returns>The factory instance with the specified parameters or the default type value if the factory is unresolvable.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            public Object GetInstance(Type factoryType, params Object[] parameters)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                try
                {
                    Object defaultInstance;
                    Int32 version, nextVersion;

                    do
                    {
                        version = _version;
                        nextVersion = version;

                        if (!_defaultContainer.IsRegistered(factoryType))
                        {
                            if (factoryType.IsInterface || factoryType.IsAbstract)
                            {
                                // if the contract cannot be resolved
                                defaultInstance = factoryType.IsValueType ? Activator.CreateInstance(factoryType) : null;
                                continue;
                            }

                            _defaultContainer.Register(factoryType, factoryType);
                            // if the contract is not registered, and can be registered, it should be

                            Interlocked.Increment(ref _version);
                            nextVersion++;
                        }

                        defaultInstance = _defaultContainer.Resolve(factoryType, parameters);

                    } while (nextVersion != Interlocked.CompareExchange(ref _version, _version, nextVersion));

                    return defaultInstance;
                }
                catch
                {
                    // either the type of the instance cannot be registered
                    return factoryType.IsValueType ? Activator.CreateInstance(factoryType) : null;
                }
            }

            /// <summary>
            /// Determines whether the specified factory has a default instance.
            /// </summary>
            /// <param name="factoryType">The type of the factory.</param>
            /// <returns><c>true</c> if the factory has a registered default instance; otherwise, <c>false</c>.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            public Boolean HasDefaultInstance(Type factoryType)
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                Boolean isRegistered;
                Int32 version;

                do
                {
                    version = _version;
                    isRegistered = _defaultContainer.IsRegistered(factoryType);

                } while (version != Interlocked.CompareExchange(ref _version, version, version));

                return isRegistered;
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
                    if (_defaultContainer != null)
                    {
                        _defaultContainer.Dispose();
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Private fields

        private Dictionary<Type, IFactory> _factories;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory" /> class.
        /// </summary>
        protected Factory()
        {
            _factories = new Dictionary<Type, IFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        /// <param name="internalFactories">The internal factories.</param>
        protected Factory(params IFactory[] internalFactories)
        {
            _factories = new Dictionary<Type, IFactory>();

            foreach (IFactory factory in internalFactories)
            {
                if (factory == null)
                    continue;

                _factories.Add(factory.ProductType, factory);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        /// <param name="internalFactories">The internal factories.</param>
        /// <exception cref="System.ArgumentNullException">The collection of internal factories is null.</exception>
        protected Factory(IEnumerable<IFactory> internalFactories)
        {
            _factories = new Dictionary<Type, IFactory>();

            if (internalFactories != null)
            {
                foreach (IFactory factory in internalFactories)
                {
                    if (factory == null)
                        continue;

                    _factories.Add(factory.ProductType, factory);
                }
            }
        }

        #endregion

        #region IFactory properties

        /// <summary>
        /// Return the product type of the factory.
        /// </summary>
        /// <value>The type of the product.</value>
        public Type ProductType { get { return GetProductType(); } }

        /// <summary>
        /// Gets the internal factories.
        /// </summary>
        /// <value>The list of internal factories.</value>
        public IList<IFactory> Factories { get { return _factories.Values.ToList(); } }

        #endregion

        #region IFactory methods

        /// <summary>
        /// Returns the specified internal factory.
        /// </summary>
        /// <typeparam name="T">The type of the factory.</typeparam>
        /// <returns>The internal factory instance for the specified type if any; otherwise, <c>null</c>.</returns>
        public T GetFactory<T>() where T : IFactory
        {
            return (T)_factories.Values.FirstOrDefault(value => value.GetType().IsSubclassOf(typeof(T)) || value.GetType().GetInterfaces().Contains(typeof(T)));
        }

        /// <summary>
        /// Returns the specified internal factory.
        /// </summary>
        /// <param name="factoryType">The type of the factory.</param>
        /// <returns>The internal factory instance for the specified type if any; otherwise, <c>null</c>.</returns>
        public IFactory GetFactory(Type factoryType)
        {
            return _factories.Values.FirstOrDefault(value => value.GetType().IsSubclassOf(factoryType) || value.GetType().GetInterfaces().Contains(factoryType));
        }

        /// <summary>
        /// Returns the factory of the specified product.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <returns>
        /// The internal factory for the specified product if any; otherwise, <c>null</c>.
        /// </returns>
        public IFactory GetFactoryFor<T>()
        {
            if (!_factories.ContainsKey(typeof(T)))
                return null;

            return _factories[typeof(T)];
        }

        /// <summary>
        /// Returns the factory of the specified product.
        /// </summary>
        /// <param name="productType">The product type.</param>
        /// <returns>The internal factory for the specified product if any; otherwise, <c>null</c>.</returns>
        public IFactory GetFactoryFor(Type productType)
        {
            if (!_factories.ContainsKey(productType))
                return null;

            return _factories[productType];
        }

        /// <summary>
        /// Defines the factory of the specified product.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <param name="factory">The factory.</param>
        public void SetFactoryFor<T>(IFactory factory)
        {
            _factories[typeof(T)] = factory;
        }

        /// <summary>
        /// Determines whether the factory contains an internal factory for the specified product type.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <returns><c>true</c> if the factory contains an internal factory for the specified product; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Boolean ContainsFactoryFor<T>()
        {
            return _factories.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Determines whether the factory contains an internal factory for the specified product type.
        /// </summary>
        /// <typeparam name="T">The product type.</typeparam>
        /// <returns><c>true</c> if the factory contains an internal factory for the specified product; otherwise, <c>false</c>.</returns>
        public Boolean ContainsFactoryFor(Type productType)
        {
            return _factories.ContainsKey(productType);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Returns the default instance for a specified factory.
        /// </summary>
        /// <typeparam name="FactoryType">The type of the factory.</typeparam>
        /// <returns>The default registered instance for the factory or the default type value if the factory is unresolvable.</returns>
        public static FactoryType DefaultInstance<FactoryType>()
        {
            return (FactoryType)FactoryImplementation.Instance.DefaultInstance(typeof(FactoryType));
        }

        /// <summary>
        /// Returns the default instance for a specified factory.
        /// </summary>
        /// <param name="factoryType">The type of the factory.</typeparam>
        /// <returns>The default registered instance for the factory or the default type value if the factory is unresolvable.</returns>
        public static Object DefaultInstance(Type factoryType)
        {
            return FactoryImplementation.Instance.DefaultInstance(factoryType);
        }

        /// <summary>
        /// Returns the instance of the factory with the specified parameters.
        /// </summary>
        /// <typeparam name="FactoryType">The type of the factory.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The factory instance with the specified parameters or the default type value if the factory is unresolvable.</returns>
        public static FactoryType GetInstance<FactoryType>(params Object[] parameters) where FactoryType : IFactory
        {
            return (FactoryType)FactoryImplementation.Instance.GetInstance(typeof(FactoryType), parameters);
        }

        /// <summary>
        /// Returns the instance of the factory with the specified parameters.
        /// </summary>
        /// <param name="FactoryType">The type of the factory.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The factory instance with the specified parameters or the default type value if the factory is unresolvable.</returns>
        public static Object GetInstance(Type factoryType, params Object[] parameters)
        {
            return FactoryImplementation.Instance.GetInstance(factoryType, parameters);
        }

        /// <summary>
        /// Returns the instance of the factory with the specified parameters.
        /// </summary>
        /// <param name="prototype">The prototype factory.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The factory instance with the specified parameters or the default type value if the factory is unresolvable.</returns>
        public static Object GetInstance(Object prototype, params Object[] parameters)
        {
            return FactoryImplementation.Instance.GetInstance(prototype.GetType(), parameters);
        }

        /// <summary>
        /// Determines whether the specified factory has a default instance.
        /// </summary>
        /// <typeparam name="FactoryType">The type of the factory.</typeparam>
        /// <returns><c>true</c> if the factory has a registered default instance; otherwise, <c>false</c>.</returns>
        public static Boolean HasDefaultInstance<FactoryType>()
        { 
            return FactoryImplementation.Instance.HasDefaultInstance(typeof(FactoryType));
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Gets the product type of the factory.
        /// </summary>
        /// <returns>The product type of the factory.</returns>
        protected abstract Type GetProductType();

        #endregion
    }
}
