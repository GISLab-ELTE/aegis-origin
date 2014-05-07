/// <copyright file="Container.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using System.Reflection;

namespace ELTE.AEGIS.Management.InversionOfControl
{
    /// <summary>
    /// Represents a container of services.
    /// </summary>    
    public class Container : IContainer
    {
        #region Private types

        /// <summary>
        /// Represents a type registration.
        /// </summary>
        private class TypeRegistration
        {
            #region Public properties

            /// <summary>
            /// Gets the name associated with the registration.
            /// </summary>
            /// <value>The name associated with the registration.</value>
            public String Name { get; private set; }

            /// <summary>
            /// Gets the contract of the registration.
            /// </summary>
            /// <value>The contract type of the registration.</value>
            public Type Contract { get; private set; }

            /// <summary>
            /// Gets the implementation of the registration.
            /// </summary>
            /// <value>The implementation type of the registration.</value>
            public Type Implementation { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="TypeRegistration" /> class.
            /// </summary>
            /// <param name="name">The name of the registration.</param>
            /// <param name="contractType">The contract type.</param>
            /// <param name="implementationType">The implementation type of the contract.</param>
            public TypeRegistration(String name, Type contractType, Type implementationType)
            {
                Name = name ?? contractType.FullName;
                Contract = contractType;
                Implementation = implementationType;
            }

            #endregion
        }

        /// <summary>
        /// Represents an instance registration.
        /// </summary>
        private class InstanceRegistration
        {
            #region Public properties

            /// <summary>
            /// Gets the name associated with the registration.
            /// </summary>
            /// <value>The name associated with the registration.</value>
            public String Name { get; private set; }

            /// <summary>
            /// Gets the contract of the registration.
            /// </summary>
            /// <value>The contract type of the registration.</value>
            public Type Contract { get; private set; }

            /// <summary>
            /// Gets the instance of the registration.
            /// </summary>
            /// <value>The instance of the registration that implements the contract.</value>
            public Object Instance { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="InstanceRegistration" /> class.
            /// </summary>
            /// <param name="name">The name of the registration.</param>
            /// <param name="contractType">The contract type.</param>
            /// <param name="instance">The instance that implements the contract.</param>
            public InstanceRegistration(String name, Type contractType, Object instance)
            {
                Name = name ?? contractType.FullName;
                Contract = contractType;
                Instance = instance;
            }

            #endregion
        }

        #endregion

        #region Private fields

        private Dictionary<String, TypeRegistration> _typeMapping;
        private Dictionary<String, InstanceRegistration> _instanceMapping;
        private Boolean _disposed;

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container()
        {
            _instanceMapping = new Dictionary<String, InstanceRegistration>();
            _typeMapping = new Dictionary<String, TypeRegistration>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Container" /> class.
        /// </summary>
        ~Container()
        {
            Dispose(false);
        }

        #endregion

        #region IContainer methods

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register<ContractType, ImplementationType>()
        {
            Register(null, typeof(ContractType), typeof(ImplementationType), false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register<ContractType, ImplementationType>(Boolean overwrite)
        {
            Register(null, typeof(ContractType), typeof(ImplementationType), overwrite);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register<ContractType, ImplementationType>(String name)
        {
            Register(name, typeof(ContractType), typeof(ImplementationType), false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register<ContractType, ImplementationType>(String name, Boolean overwrite)
        {
            Register(name, typeof(ContractType), typeof(ImplementationType), overwrite);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The implementation is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register(Type contractType, Type implementationType)
        {
            Register(null, contractType, implementationType, false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The implementation is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register(Type contractType, Type implementationType, Boolean overwrite)
        {
            Register(null, contractType, implementationType, overwrite);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The implementation is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register(String name, Type contractType, Type implementationType)
        {
            Register(name, contractType, implementationType, false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The implementation is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        public void Register(String name, Type contractType, Type implementationType, Boolean overwrite)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (implementationType == null)
                throw new ArgumentNullException("implementationType", "The implementation is null.");
            if (!IsEqualOrDescendant(contractType, implementationType))
                throw new InvalidOperationException("Type " + implementationType.FullName + " does not implement contract " + contractType.FullName + ".");
            if (implementationType.IsAbstract || implementationType.IsInterface || implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
                throw new InvalidOperationException("Type " + implementationType.FullName + " is not instantiable.");
            if ((name != null && _typeMapping.ContainsKey(name) || _typeMapping.ContainsKey(contractType.FullName)) && !overwrite)
                throw new InvalidOperationException("Contract " + contractType.FullName + " is already registered.");


            TypeRegistration mapping = new TypeRegistration(name, contractType, implementationType);

            _typeMapping[mapping.Name] = mapping;
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public void RegisterInstance<ContractType>(ContractType instance)
        {
            RegisterInstance(null, typeof(ContractType), instance, false);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public void RegisterInstance<ContractType>(ContractType instance, Boolean overwrite)
        {
            RegisterInstance(null, typeof(ContractType), instance, overwrite);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public void RegisterInstance<ContractType>(String name, ContractType instance)
        {
            RegisterInstance(name, typeof(ContractType), instance, false);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public void RegisterInstance<ContractType>(String name, ContractType instance, Boolean overwrite)
        {
            RegisterInstance(name, typeof(ContractType), instance, overwrite);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        public void RegisterInstance(Type contractType, Object instance)
        {
            RegisterInstance(null, contractType, instance, false);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        public void RegisterInstance(Type contractType, Object instance, Boolean overwrite)
        {
            RegisterInstance(null, contractType, instance, overwrite);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        public void RegisterInstance(String name, Type contractType, Object instance)
        {
            RegisterInstance(name, contractType, instance, false);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        public void RegisterInstance(String name, Type contractType, Object instance, Boolean overwrite)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (instance != null && !IsEqualOrDescendant(contractType, instance.GetType()))
                throw new ArgumentException("The instance does not implement the contract.", "instance");

            InstanceRegistration mapping = new InstanceRegistration(name, contractType, instance);

            _instanceMapping[mapping.Name] = mapping;
        }

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean Unregister<ContractType>()
        {
            return Unregister(typeof(ContractType));
        }

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean Unregister(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            return _typeMapping.Remove(contractType.FullName);
        }

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean Unregister(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            return _typeMapping.Remove(name);
        }

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service instance.</typeparam>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean UnregisterInstance<ContractType>()
        {
            return UnregisterInstance(typeof(ContractType));
        }

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="contractType">The contract of the service instance.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean UnregisterInstance(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            return _instanceMapping.Remove(contractType.FullName);
        }

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean UnregisterInstance(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            return _instanceMapping.Remove(name);
        }

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        public Boolean UnregisterInstance(Object instance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (instance == null)
                throw new ArgumentNullException("instance", "The instance is null.");

            InstanceRegistration mapping = _instanceMapping.Values.FirstOrDefault(item => item.Instance == instance);

            if (mapping == null)
                return false;

            return _instanceMapping.Remove(mapping.Name);
        }

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean IsRegistered<ContractType>()
        {
            return IsRegistered(typeof(ContractType));
        }

        /// <summary>
        /// Determines whether the specified implementation is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean IsRegistered<ContractType, ImplementationType>()
        {
            return IsRegistered(typeof(ContractType), typeof(ImplementationType));
        }

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean IsRegistered(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            return _typeMapping.ContainsKey(contractType.FullName);
        }

        /// <summary>
        /// Determines whether the specified name is registered.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean IsRegistered(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            return _typeMapping.ContainsKey(name);
        }

        /// <summary>
        /// Determines whether the specified implementation is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean IsRegistered(Type contractType, Type implementationType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            TypeRegistration mapping;
            return _typeMapping.TryGetValue(contractType.FullName, out mapping) && mapping.Contract.Equals(implementationType);
        }

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean IsRegisteredInstance<ContractType>()
        {
            return IsRegisteredInstance(typeof(ContractType));
        }

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean IsRegisteredInstance(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            return _instanceMapping.ContainsKey(contractType.FullName);
        }

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean IsRegisteredInstance(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            return _typeMapping.ContainsKey(name);
        }

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        public Boolean IsRegisteredInstance(Object instance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (instance == null)
                throw new ArgumentNullException("instance", "The instance is null.");

            InstanceRegistration mapping = _instanceMapping.Values.FirstOrDefault(item => item.Instance.Equals(instance));

            return (mapping == null);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>An instance of the registered implementation of <typeparamref name="ContractType" />.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">Service is not registered.</exception>
        public ContractType Resolve<ContractType>()
        {
            return (ContractType)Resolve(typeof(ContractType));
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the registered implementation of <typeparamref name="ContractType" /> crated using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">Service is not registered.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public ContractType Resolve<ContractType>(params Object[] parameters)
        {
            return (ContractType)Resolve(typeof(ContractType), parameters);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns>The registered implementation of <paramref name="contractType" />.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The contract is null.
        /// or
        /// Service is not registered.
        /// </exception>
        public Object Resolve(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (!_typeMapping.ContainsKey(contractType.FullName))
                throw new InvalidOperationException("Contract " + contractType.FullName + " is not registered.");

            // invoke the specified service using the default contructor
            ConstructorInfo constructor = _typeMapping[contractType.FullName].Implementation.GetConstructors()[0];

            // resolve all internal services
            ParameterInfo[] parameters = constructor.GetParameters();
            Object[] resolvedParameters = new Object[parameters.Length];
            for (Int32 i = 0; i < parameters.Length; i++)
            {
                resolvedParameters[i] = Resolve(parameters[i].ParameterType);
            }

            return constructor.Invoke(resolvedParameters);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>The registered implementation of <paramref name="contractType" />.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The contract is null.
        /// or
        /// Service is not registered.
        /// </exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public Object Resolve(Type contractType, params Object[] parameters)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (!_typeMapping.ContainsKey(contractType.FullName))
                throw new InvalidOperationException("Contract " + contractType.FullName + " is not registered.");
           
            // check all constructors for matching parameters
            foreach (ConstructorInfo constructor in _typeMapping[contractType.FullName].Implementation.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterInfo[] constructorParameterInfos = constructor.GetParameters();
                Object[] constructorParameters = new Object[constructorParameterInfos.Length];
                Int32 parameterIndex = 0;

                // check whether the parameter is specified, or can be resolved
                for (Int32 i = 0; i < constructorParameterInfos.Length; i++)
                {
                    if (parameterIndex < parameters.Length &&
                        (parameters[parameterIndex] == null ||
                         IsEqualOrDescendant(constructorParameterInfos[i].ParameterType, parameters[parameterIndex].GetType())))
                    {
                        constructorParameters[i] = parameters[parameterIndex];
                        parameterIndex++;
                    }
                    else if (!TryResolve(constructorParameterInfos[i].ParameterType, out constructorParameters[i]))
                    {
                        // if not specitied, or it cannot be resolved, try the default value
                        constructorParameters[i] = constructorParameterInfos[i].ParameterType.IsValueType ? Activator.CreateInstance(constructorParameterInfos[i].ParameterType) : null;
                    }
                }

                // try to instantiate the service
                try
                {
                    return constructor.Invoke(constructorParameters);
                }
                catch { }
            }

            // if no service has been constructed, the parameters are not valid
            throw new ArgumentException("The service cannot be instantiated based on the specified parameters.", "parameters");
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>The registered instance of <typeparamref name="contractType" />.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public ContractType ResolveInstance<ContractType>()
        {
            return (ContractType)ResolveInstance(typeof(ContractType));
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance of <typeparamref name="contractType" /> under the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public ContractType ResolveInstance<ContractType>(String name)
        {
            return (ContractType)ResolveInstance(name);
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <returns>The registered instance of <paramref name="contractType" />.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">No instance is registered under the contract.</exception>
        public Object ResolveInstance(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (!_instanceMapping.ContainsKey(contractType.FullName))
                throw new InvalidOperationException("No instance is registered under the " + contractType.FullName + " contract.");

            return _instanceMapping[contractType.FullName].Instance;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance of under the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The name is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// No instance is registered under the name.
        /// or
        /// The instance registered under the name does not implement the contract.
        /// </exception>
        public Object ResolveInstance(Type contractType, String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");
            if (_instanceMapping.ContainsKey(name))
                throw new ArgumentException("No instance is registered under the " + name + " name.");
            if (!IsEqualOrDescendant(contractType, _instanceMapping[name].Contract))
                throw new ArgumentException("The instance registered under the " + name + " name does not implement the contract.");

            return _instanceMapping[name].Instance;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance of under the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.ArgumentException">No instance is registered under the name.</exception>
        public Object ResolveInstance(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            if (_instanceMapping.ContainsKey(name))
                throw new ArgumentException("No instance is registered under the " + name + " name.");

            return _instanceMapping[name].Instance;
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">An instance of the registered implementation of <typeparamref name="ContractType" />.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean TryResolve<ContractType>(out ContractType resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            Object instanceObject = null;

            if (!TryResolve(typeof(ContractType), out instanceObject))
            {
                resolvedInstance = default(ContractType);
                return false;
            }

            resolvedInstance = (ContractType)instanceObject;
            return true;
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public Boolean TryResolve<ContractType>(Object[] parameters, out ContractType resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            Object instanceObject = null;

            if (!TryResolve(typeof(ContractType), parameters, out instanceObject))
            {
                resolvedInstance = default(ContractType);
                return false;
            }

            resolvedInstance = (ContractType)instanceObject;
            return true;
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Contract is not an interface type.
        /// or
        /// Service is not registered.
        /// </exception>
        public Boolean TryResolve(Type contractType, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            resolvedInstance = null;

            if (!_typeMapping.ContainsKey(contractType.FullName))
                return false;

            // invoke the specified service using the default contructor
            ConstructorInfo constructor = _typeMapping[contractType.FullName].Implementation.GetConstructors()[0];

            // resolve all internal services
            ParameterInfo[] parameters = constructor.GetParameters();
            Object[] resolvedParameters = new Object[parameters.Length];
            for (Int32 i = 0; i < parameters.Length; i++)
            {
                if (!TryResolve(parameters[i].ParameterType, out resolvedParameters[i]))
                    return false;
            }

            resolvedInstance = constructor.Invoke(resolvedParameters);
            return true;
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Contract is not an interface type.
        /// or
        /// Service is not registered.
        /// </exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public Boolean TryResolve(Type contractType, Object[] parameters, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            resolvedInstance = null;

            if (!_typeMapping.ContainsKey(contractType.FullName))
                return false;

            Boolean parameterResolveSuccess = true;

            // check all constructors for matching parameters
            foreach (ConstructorInfo constructor in _typeMapping[contractType.FullName].Implementation.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterInfo[] constructorParameterInfos = constructor.GetParameters();
                Object[] constructorParameters = new Object[constructorParameterInfos.Length];
                Int32 parameterIndex = 0;

                // check whether the parameter is specified, or can be resolved
                for (Int32 i = 0; i < constructorParameterInfos.Length && parameterResolveSuccess; i++)
                {
                    if (parameterIndex < parameters.Length && (parameters[parameterIndex] == null || IsEqualOrDescendant(constructorParameterInfos[i].ParameterType, parameters[parameterIndex].GetType())))
                    {
                        constructorParameters[i] = parameters[parameterIndex];
                        parameterIndex++;
                    }
                    else
                    {
                        if (!TryResolve(constructorParameterInfos[i].ParameterType, out constructorParameters[i]))
                        {
                            parameterResolveSuccess = false;
                        }
                    }
                }

                // try to instantiate the service
                try
                {
                    if (parameterResolveSuccess)
                    {
                        resolvedInstance = constructor.Invoke(constructorParameters);
                        return true;
                    }
                }
                catch { }
            }

            return false;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean TryResolveInstance<ContractType>(out ContractType resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            Object instanceObject = null;

            if (!TryResolveInstance(typeof(ContractType), out instanceObject))
            {
                resolvedInstance = default(ContractType);
                return false;
            }

            resolvedInstance = (ContractType)instanceObject;
            return true;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean TryResolveInstance<ContractType>(String name, out ContractType resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            Object instanceObject = null;

            if (!TryResolveInstance(typeof(ContractType), name, out instanceObject))
            {
                resolvedInstance = default(ContractType);
                return false;
            }

            resolvedInstance = (ContractType)instanceObject;
            return true;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean TryResolveInstance(Type contractType, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");

            if (!_instanceMapping.ContainsKey(contractType.FullName))
            {
                resolvedInstance = null;
                return false;
            }

            resolvedInstance = _instanceMapping[contractType.FullName].Instance;
            return true;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The name is null.
        /// </exception>
        public Boolean TryResolveInstance(Type contractType, String name, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", "The contract is null.");
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            if (!_instanceMapping.ContainsKey(name) || !IsEqualOrDescendant(contractType, _instanceMapping[name].Contract))
            {
                resolvedInstance = null;
                return false;
            }

            resolvedInstance = _instanceMapping[name].Instance;
            return true;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean TryResolveInstance(String name, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            if (_instanceMapping.ContainsKey(name))
            {
                resolvedInstance = null;
                return false;
            }

            resolvedInstance = _instanceMapping[name].Instance;
            return true;
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
        protected virtual void Dispose(Boolean disposing)
        {
            _disposed = true;

            if (disposing)
            {
                _instanceMapping = null;
                _typeMapping = null;
            }
        }

        #endregion

        #region Protected static methods

        /// <summary>
        /// Determines whether the a type is equal to or a descendant of another type.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the base type is equal to or a parent of the other type.</returns>
        protected static Boolean IsEqualOrDescendant(Type baseType, Type type)
        {
            return type.Equals(baseType) ||
                   type.IsSubclassOf(baseType) ||
                   type.GetInterface(baseType.Name) != null;
        }


        #endregion
    }
}
