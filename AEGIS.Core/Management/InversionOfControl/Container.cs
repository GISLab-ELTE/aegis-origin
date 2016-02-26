/// <copyright file="Container.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ELTE.AEGIS.Management.InversionOfControl
{
    /// <summary>
    /// Represents a container of services.
    /// </summary>    
    /// <remarks>
    /// This implementation of the <see cref="IContainer" /> interface is not thread safe. 
    /// To enable thread safety, the <see cref="ConcurrentContainer" /> implementation should be used.
    /// </remarks>
    public class Container : IContainer
    {
        #region Private types

        /// <summary>
        /// Represents a type registration.
        /// </summary>
        protected class TypeRegistration
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
            /// Gets the behavior of the registration.
            /// </summary>
            /// <value>The behavior type of the registration.</value>
            public Type Behavior { get; private set; }

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
                Name = name;
                Contract = contractType;
                Behavior = implementationType;
            }

            #endregion
        }

        /// <summary>
        /// Represents an instance registration.
        /// </summary>
        protected class InstanceRegistration
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
                Name = name;
                Contract = contractType;
                Instance = instance;
            }

            #endregion
        }

        #endregion

        #region Private constant fields

        /// <summary>
        /// Exception message in case the contract is null. This field is constant.
        /// </summary>
        private const String MessageContractIsNull = "The contract is null.";

        /// <summary>
        /// Exception message in case the behavior is null. This field is constant.
        /// </summary>
        private const String MessageBehaviorIsNull = "The behavior is null.";

        /// <summary>
        /// Exception message in case the behavior does not implement the contract. This field is constant.
        /// </summary>
        private const String MessageBehaviorNotImplementing = "The behavior does not implement contract.";

        /// <summary>
        /// Exception message in case the behavior is not instantiable. This field is constant.
        /// </summary>
        private const String MessageBehaviorNotInstantiable = "The behavior is not instantiable.";

        /// <summary>
        /// Exception message in case the instance is null. This field is constant.
        /// </summary>
        private const String MessageInstanceIsNull = "The instance is null.";

        /// <summary>
        /// Exception message in case the instance does not implement the contract. This field is constant.
        /// </summary>
        private const String MessageInstanceNotImplementing = "The instance does not implement the contract.";

        /// <summary>
        /// Exception message in case no instance is registered. This field is constant.
        /// </summary>
        private const String MessageInstanceNotRegistered = "No instance is not registered.";

        /// <summary>
        /// The name is null.
        /// </summary>
        private const String MessageNameIsNull = "The name is null.";

        /// <summary>
        /// Exception message in case the service is not registered. This field is constant.
        /// </summary>
        private const String MessageServiceIsNotRegistered = "The service is not registered.";

        /// <summary>
        /// Exception message in case the service is already registered. This field is constant.
        /// </summary>
        private const String MessageServiceIsRegistered = "The service is already registered.";

        /// <summary>
        /// Exception message in case the service does not implement the contract. This field is constant.
        /// </summary>
        private const String MessageServiceNotImplementing = "The service does not implement the contract.";

        /// <summary>
        /// Exception message in case the service parameters are missing. This field is constant.
        /// </summary>
        private const String MessageServiceParametersMissing = "The service cannot be instantiated without parameters.";

        /// <summary>
        /// Exception message in case the service parameters are invalid. This field is constant.
        /// </summary>
        private const String MessageServiceParametersInvalid = "The service cannot be instantiated based on the specified parameters.";


        #endregion

        #region Private fields
        
        /// <summary>
        /// The dictionary of instance mappings.
        /// </summary>
        private IDictionary<String, InstanceRegistration> _instanceMapping;

        /// <summary>
        /// The dictionary of type mappings.
        /// </summary>
        private IDictionary<String, TypeRegistration> _typeMapping;

        /// <summary>
        /// A value indicating whether the instance is disposed.
        /// </summary>
        private Boolean _disposed;

        #endregion

        #region Constructors and finalizer
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        public Container()
        {
            _instanceMapping = new Dictionary<String, InstanceRegistration>();
            _typeMapping = new Dictionary<String, TypeRegistration>();
            _disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Container" /> class.
        /// </summary>
        /// <param name="typeMapping">The type mapping.</param>
        /// <param name="instanceMapping">The instance mapping.</param>
        protected Container(IDictionary<String, TypeRegistration> typeMapping, IDictionary<String, InstanceRegistration> instanceMapping)
        {
            _instanceMapping = instanceMapping ?? new Dictionary<String, InstanceRegistration>();
            _typeMapping = typeMapping ?? new Dictionary<String, TypeRegistration>();
            _disposed = false;
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
        /// <typeparam name="BehaviorType">The behavior of the service.</typeparam>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register<ContractType, BehaviorType>() where BehaviorType : ContractType
        {
            Register(typeof(ContractType).FullName, typeof(ContractType), typeof(BehaviorType), false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="BehaviorType">The behavior of the service.</typeparam>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register<ContractType, BehaviorType>(Boolean overwrite) where BehaviorType : ContractType
        {
            Register(typeof(ContractType).FullName, typeof(ContractType), typeof(BehaviorType), overwrite);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="BehaviorType">The behavior of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        public void Register<ContractType, BehaviorType>(String name) where BehaviorType : ContractType
        {
            Register(name, typeof(ContractType), typeof(BehaviorType), false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="BehaviorType">The behavior of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register<ContractType, BehaviorType>(String name, Boolean overwrite) where BehaviorType : ContractType
        {
            Register(name, typeof(ContractType), typeof(BehaviorType), overwrite);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The behavior is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior does not implement the contract.
        /// or
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register(Type contractType, Type behaviorType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            Register(contractType.FullName, contractType, behaviorType, false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The behavior is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior does not implement the contract.
        /// or
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register(Type contractType, Type behaviorType, Boolean overwrite)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            Register(contractType.FullName, contractType, behaviorType, overwrite);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The contract is null.
        /// or
        /// The behavior is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior does not implement the contract.
        /// or
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register(String name, Type contractType, Type behaviorType)
        {
            Register(name, contractType, behaviorType, false);
        }

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The contract is null.
        /// or
        /// The behavior is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The behavior does not implement the contract.
        /// or
        /// The behavior is not instantiable.
        /// or
        /// The service is already registered.
        /// </exception>
        public void Register(String name, Type contractType, Type behaviorType, Boolean overwrite)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (behaviorType == null)
                throw new ArgumentNullException("behaviorType", MessageBehaviorIsNull);
            if (_typeMapping.ContainsKey(name) && !overwrite)
                throw new InvalidOperationException(MessageServiceIsRegistered);
            if (!IsDescendant(contractType, behaviorType))
                throw new InvalidOperationException(MessageBehaviorNotImplementing);
            if (behaviorType.IsAbstract || behaviorType.IsInterface || behaviorType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
                throw new InvalidOperationException(MessageBehaviorNotInstantiable);

            TypeRegistration mapping = new TypeRegistration(name, contractType, behaviorType);

            _typeMapping[mapping.Name] = mapping;
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        public void RegisterInstance<ContractType>(ContractType instance)
        {
            RegisterInstance(typeof(ContractType).FullName, typeof(ContractType), instance, false);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        public void RegisterInstance<ContractType>(ContractType instance, Boolean overwrite)
        {
            RegisterInstance(typeof(ContractType).FullName, typeof(ContractType), instance, overwrite);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
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
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
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
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        public void RegisterInstance(Type contractType, Object instance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            RegisterInstance(contractType.FullName, contractType, instance, false);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        public void RegisterInstance(Type contractType, Object instance, Boolean overwrite)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            RegisterInstance(contractType.FullName, contractType, instance, overwrite);
        }

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
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
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        public void RegisterInstance(String name, Type contractType, Object instance, Boolean overwrite)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (instance == null)
                throw new ArgumentNullException("instance", MessageInstanceIsNull);
            if (_instanceMapping.ContainsKey(name) && !overwrite)
                throw new InvalidOperationException(MessageServiceIsRegistered);
            if (!IsDescendant(contractType, instance.GetType()))
                throw new ArgumentException(MessageInstanceNotImplementing, "instance");

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
                throw new ArgumentNullException("contractType", MessageContractIsNull);

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
                throw new ArgumentNullException("name", MessageNameIsNull);

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
                throw new ArgumentNullException("contractType", MessageContractIsNull);

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
                throw new ArgumentNullException("name", MessageNameIsNull);

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
                throw new ArgumentNullException("instance", MessageInstanceIsNull);

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
        /// Determines whether the specified behavior is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean IsRegistered<ContractType, BehaviorType>() where BehaviorType : ContractType
        {
            return IsRegistered(typeof(ContractType), typeof(BehaviorType));
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
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return _typeMapping.ContainsKey(contractType.FullName);
        }

        /// <summary>
        /// Determines whether the specified service is registered.
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
                throw new ArgumentNullException("name", MessageNameIsNull);

            return _typeMapping.ContainsKey(name);
        }

        /// <summary>
        /// Determines whether the specified behavior is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The behavior is null.
        /// </exception>
        public Boolean IsRegistered(Type contractType, Type behaviorType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (behaviorType == null)
                throw new ArgumentNullException("behaviorType", MessageBehaviorIsNull);

            TypeRegistration mapping;
            return _typeMapping.TryGetValue(contractType.FullName, out mapping) && mapping.Behavior.Equals(behaviorType);
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
                throw new ArgumentNullException("contractType", MessageContractIsNull);

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
                throw new ArgumentNullException("name", MessageNameIsNull);

            return _instanceMapping.ContainsKey(name);
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
                throw new ArgumentNullException("instance", MessageInstanceIsNull);

            InstanceRegistration mapping = _instanceMapping.Values.FirstOrDefault(item => item.Instance.Equals(instance));

            return (mapping != null);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service cannot be instantiated without parameters.
        /// </exception>
        public ContractType Resolve<ContractType>()
        {
            return (ContractType)Resolve(typeof(ContractType).FullName);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the service.</param>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service does not implement the contract.
        /// or
        /// The service cannot be instantiated without parameters.
        /// </exception>
        public ContractType Resolve<ContractType>(String name)
        {
            return (ContractType)Resolve(typeof(ContractType), name);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the service created using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service does not implement the contract.
        /// </exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public ContractType Resolve<ContractType>(String name, params Object[] parameters)
        {
            return (ContractType)Resolve(typeof(ContractType), name, parameters);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the service created using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public ContractType Resolve<ContractType>(params Object[] parameters)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            return (ContractType)Resolve(typeof(ContractType), typeof(ContractType).FullName, parameters);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service cannot be instantiated without parameters.
        /// </exception>
        public Object Resolve(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return Resolve(contractType.FullName);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="name">The name of the service.</param>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The name is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service does not implement the contract.
        /// or
        /// The service cannot be instantiated without parameters.
        /// </exception>
        public Object Resolve(Type contractType, String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (!_typeMapping.ContainsKey(name))
                throw new InvalidOperationException(MessageServiceIsNotRegistered);
            if (!contractType.Equals(_typeMapping[name].Contract))
                throw new InvalidOperationException(MessageServiceNotImplementing);

            return Resolve(name);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="name">The name of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the service created using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The name is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service does not implement the contract.
        /// </exception>
        public Object Resolve(Type contractType, String name, params Object[] parameters)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (!_typeMapping.ContainsKey(name))
                throw new InvalidOperationException(MessageServiceIsNotRegistered);
            if (!contractType.Equals(_typeMapping[name].Contract))
                throw new InvalidOperationException(MessageServiceNotImplementing);

            return Resolve(name, parameters);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the service created using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        public Object Resolve(Type contractType, params Object[] parameters)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return Resolve(contractType.FullName, parameters);
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The service cannot be instantiated without parameters.
        /// </exception>
        public Object Resolve(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (!_typeMapping.ContainsKey(name))
                throw new InvalidOperationException(MessageServiceIsNotRegistered);

            Object resolvedInstance;

            if (!TryResolve(name, out resolvedInstance))
                throw new InvalidOperationException(MessageServiceParametersMissing);

            return resolvedInstance;
        }

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the service created using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        public Object Resolve(String name, params Object[] parameters)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (!_typeMapping.ContainsKey(name))
                throw new InvalidOperationException(MessageServiceIsNotRegistered);

            Object resolvedInstance;

            if (!TryResolve(name, parameters, out resolvedInstance))
                throw new ArgumentException(MessageServiceParametersInvalid, "parameters");

            return resolvedInstance;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>The registered instance for the specified contract.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        public ContractType ResolveInstance<ContractType>()
        {
            return (ContractType)ResolveInstance(typeof(ContractType).FullName);
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance for the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The instance does not implement the contract.
        /// </exception>
        public ContractType ResolveInstance<ContractType>(String name)
        {
            return (ContractType)ResolveInstance(typeof(ContractType), name);
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <returns>The registered instance for the specified contract.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        public Object ResolveInstance(Type contractType)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return ResolveInstance(contractType, contractType.FullName);
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance for the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The name is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The instance does not implement the contract.
        /// </exception>
        public Object ResolveInstance(Type contractType, String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (!_instanceMapping.ContainsKey(name))
                throw new InvalidOperationException(MessageServiceIsNotRegistered);
            if (!contractType.Equals(_instanceMapping[name].Contract))
                throw new InvalidOperationException(MessageInstanceNotImplementing);

            return _instanceMapping[name].Instance;
        }

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance for the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        public Object ResolveInstance(String name)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);
            if (!_instanceMapping.ContainsKey(name))
                throw new InvalidOperationException(MessageServiceIsNotRegistered);

            return _instanceMapping[name].Instance;
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">An instance of the registered implementation of <typeparamref name="ContractType" />.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean TryResolve<ContractType>(out ContractType resolvedInstance)
        {
            Object instance;
            Boolean result = TryResolve(typeof(ContractType).FullName, out instance);
            resolvedInstance = (ContractType)instance;

            return result;
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean TryResolve<ContractType>(Object[] parameters, out ContractType resolvedInstance)
        {
            Object instance;
            Boolean result = TryResolve(typeof(ContractType).FullName, parameters, out instance);
            resolvedInstance = (ContractType)instance;

            return result;
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean TryResolve(Type contractType, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return TryResolve(contractType.FullName, out resolvedInstance);
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean TryResolve(Type contractType, Object[] parameters, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return TryResolve(contractType.FullName, parameters, out resolvedInstance);
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean TryResolve(String name, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);

            resolvedInstance = null;

            if (!_typeMapping.ContainsKey(name))
                return false;

            // invoke the specified service using the default constructor
            ConstructorInfo constructor = _typeMapping[name].Behavior.GetConstructors()[0];

            try
            {
                // resolve all internal services
                ParameterInfo[] parameters = constructor.GetParameters();
                Object[] resolvedParameters = new Object[parameters.Length];
                for (Int32 paramIndex = 0; paramIndex < parameters.Length; paramIndex++)
                {
                    if (!TryResolve(parameters[paramIndex].ParameterType, out resolvedParameters[paramIndex]))
                        return false;
                }

                resolvedInstance = constructor.Invoke(resolvedParameters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean TryResolve(String name, Object[] parameters, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            resolvedInstance = null;

            if (!_typeMapping.ContainsKey(name))
                return false;

            // check all constructors for matching parameters
            foreach (ConstructorInfo constructor in _typeMapping[name].Behavior.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                ParameterInfo[] constructorParamInfos = constructor.GetParameters();
                Object[] constructorParams = new Object[constructorParamInfos.Length];
                Int32 providedParamIndex = 0;
                Boolean allResolved = true;

                // check whether the parameter is specified, or can be resolved
                for (Int32 requiredParamIndex = 0; requiredParamIndex < constructorParamInfos.Length && allResolved; requiredParamIndex++)
                {
                    if (parameters != null && 
                        providedParamIndex < parameters.Length && (parameters[providedParamIndex] == null || 
                        IsMatching(parameters[providedParamIndex].GetType(), constructorParamInfos[requiredParamIndex].ParameterType)))
                    {
                        constructorParams[requiredParamIndex] = parameters[providedParamIndex];
                        providedParamIndex++;
                    }
                    else
                    {
                        if (!TryResolve(constructorParamInfos[requiredParamIndex].ParameterType, out constructorParams[requiredParamIndex]))
                        {
                            allResolved = false;
                        }
                    }
                }

                // check whether all parameters which required resolving were resolved
                if (!allResolved)
                    continue;

                // check whether all parameters were used
                if (providedParamIndex < parameters.Length)
                    continue;
                
                // try to instantiate the service
                try
                {
                    if (allResolved)
                    {
                        resolvedInstance = constructor.Invoke(constructorParams);
                        return true;
                    }
                }
                catch { }
            }

            return false;
        }

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        public Boolean TryResolveInstance<ContractType>(out ContractType resolvedInstance)
        {
            Object instance;
            Boolean result = TryResolveInstance(typeof(ContractType), typeof(ContractType).FullName, out instance);
            resolvedInstance = (ContractType)instance;

            return result;
        }

        /// <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean TryResolveInstance<ContractType>(String name, out ContractType resolvedInstance)
        {
            Object instance;
            Boolean result = TryResolveInstance(typeof(ContractType), name, out instance);
            resolvedInstance = (ContractType)instance;

            return result;
        }

        /// <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        public Boolean TryResolveInstance(Type contractType, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (contractType == null)
                throw new ArgumentNullException("contractType", MessageContractIsNull);

            return TryResolveInstance(contractType, contractType.FullName, out resolvedInstance);
        }

        // <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
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
                throw new ArgumentNullException("contractType", MessageContractIsNull);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);

            if (!_instanceMapping.ContainsKey(name) || !contractType.Equals(_instanceMapping[name].Contract))
            {
                resolvedInstance = null;
                return false;
            }

            resolvedInstance = _instanceMapping[name].Instance;
            return true;
        }

        /// <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        public Boolean TryResolveInstance(String name, out Object resolvedInstance)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (name == null)
                throw new ArgumentNullException("name", MessageNameIsNull);

            if (!_instanceMapping.ContainsKey(name))
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
        /// Determines whether a type matches another type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="otherType">The other type.</param>
        /// <returns><c>true</c> if the type matches the other type; otherwise, <c>false</c>.</returns>
        protected static Boolean IsMatching(Type type, Type otherType)
        {
            return type.Equals(otherType) || // same types
                   type.IsSubclassOf(otherType) || type.GetInterfaces().Contains(otherType) || // descendant types
                   type.GetInterfaces().Contains(typeof(IConvertible)) && otherType.GetInterfaces().Contains(typeof(IConvertible)); // both are convertible
        }

        /// <summary>
        /// Determines whether the a type a descendant of another type.
        /// </summary>
        /// <param name="baseType">The base type.</param>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the base type is a parent of the other type; otherwise, <c>false</c>.</returns>
        protected static Boolean IsDescendant(Type baseType, Type type)
        {
            return type.IsSubclassOf(baseType) || type.GetInterface(baseType.Name) != null;
        }


        #endregion
    }
}
