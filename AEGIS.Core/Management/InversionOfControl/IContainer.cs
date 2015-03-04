/// <copyright file="IContainer.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Management.InversionOfControl
{
    /// <summary>
    /// Defines behavior of service containers.
    /// </summary>
    /// <remarks>
    /// The inversion of control (IoC) container is a manager object to maintain the instantiation of services during runtime.
    /// The control can register different services, and query these services based on the contract they implement. 
    /// The contract defines the interface of the service and is provided as interface, thus the behavior providing the service must implement the interface.
    /// </remarks>
    public interface IContainer : IDisposable
    {
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
        void Register<ContractType, BehaviorType>() where BehaviorType : ContractType;

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
        void Register<ContractType, BehaviorType>(Boolean overwrite) where BehaviorType : ContractType;

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="BehaviorType">The behavior of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        void Register<ContractType, BehaviorType>(String name) where BehaviorType : ContractType;

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
        void Register<ContractType, BehaviorType>(String name, Boolean overwrite) where BehaviorType : ContractType;

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
        void Register(Type contractType, Type behaviorType);

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
        void Register(Type contractType, Type behaviorType, Boolean overwrite);

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
        void Register(String name, Type contractType, Type behaviorType);

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
        void Register(String name, Type contractType, Type behaviorType, Boolean overwrite);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        void RegisterInstance<ContractType>(ContractType instance);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is already registered.</exception>
        void RegisterInstance<ContractType>(ContractType instance, Boolean overwrite);

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
        void RegisterInstance<ContractType>(String name, ContractType instance);

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
        void RegisterInstance<ContractType>(String name, ContractType instance, Boolean overwrite);

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
        void RegisterInstance(Type contractType, Object instance);

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
        void RegisterInstance(Type contractType, Object instance, Boolean overwrite);

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
        void RegisterInstance(String name, Type contractType, Object instance);

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
        void RegisterInstance(String name, Type contractType, Object instance, Boolean overwrite);

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean Unregister<ContractType>();

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean Unregister(Type contractType);

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean Unregister(String name);

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service instance.</typeparam>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean UnregisterInstance<ContractType>();

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="contractType">The contract of the service instance.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean UnregisterInstance(Type contractType);

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean UnregisterInstance(String name);

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        Boolean UnregisterInstance(Object instance);

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean IsRegistered<ContractType>();

        /// <summary>
        /// Determines whether the specified behavior is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="behaviorType">The behavior of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean IsRegistered<ContractType, BehaviorType>() where BehaviorType : ContractType;

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean IsRegistered(Type contractType);

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean IsRegistered(String name);

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
        Boolean IsRegistered(Type contractType, Type behaviorType);

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean IsRegisteredInstance<ContractType>();

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean IsRegisteredInstance(Type contractType);

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean IsRegisteredInstance(String name);

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        Boolean IsRegisteredInstance(Object instance);

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
        ContractType Resolve<ContractType>();

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
        ContractType Resolve<ContractType>(String name);

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
        ContractType Resolve<ContractType>(String name, Object[] parameters);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the service created using the specified parameters.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        ContractType Resolve<ContractType>(Object[] parameters);

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
        Object Resolve(Type contractType);

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
        Object Resolve(Type contractType, String name);

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
        Object Resolve(Type contractType, String name, params Object[] parameters);

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
        Object Resolve(Type contractType, params Object[] parameters);

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
        Object Resolve(String name);

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
        Object Resolve(String name, params Object[] parameters);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>The registered instance for the specified contract.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        ContractType ResolveInstance<ContractType>();

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance for the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The service is not registered.
        /// or
        /// The instance does not implement the contract.
        /// </exception>
        ContractType ResolveInstance<ContractType>(String name);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <returns>The registered instance for the specified contract.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">The service is not registered.</exception>
        Object ResolveInstance(Type contractType);

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
        Object ResolveInstance(Type contractType, String name);
        
        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance for the specified name.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.ArgumentException">The service is not registered.</exception>
        Object ResolveInstance(String name);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">An instance of the registered implementation of <typeparamref name="ContractType" />.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean TryResolve<ContractType>(out ContractType resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean TryResolve<ContractType>(Object[] parameters, out ContractType resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean TryResolve(Type contractType, out Object resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean TryResolve(Type contractType, Object[] parameters, out Object resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean TryResolve(String name, out Object resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean TryResolve(String name, Object[] parameters, out Object resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        Boolean TryResolveInstance<ContractType>(out ContractType resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean TryResolveInstance<ContractType>(String name, out ContractType resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean TryResolveInstance(Type contractType, out Object resolvedInstance);

        /// <summary>
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
        Boolean TryResolveInstance(Type contractType, String name, out Object resolvedInstance);

        /// <summary>
        /// Tries to resolve the specified service instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance can be resolved; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean TryResolveInstance(String name, out Object resolvedInstance);
    }
}
