/// <copyright file="Container.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Management.InversionOfControl
{
    /// <summary>
    /// Defines behavior of service containers.
    /// </summary>
    /// <remarks>
    /// The inversion of control (IoC) container is a manager object to maintain the instantiation of services during runtime.
    /// The control can register different services, and query these services based on the contract they implement. 
    /// The contract defines the interface of the service and is provided as interface, thus the class providing the service must implement the interface. 
    /// All containers and all services are also queriable through the static <see cref="ServiceLocator" /> class.
    /// </remarks>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        void Register<ContractType, ImplementationType>();

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        void Register<ContractType, ImplementationType>(Boolean overwrite);

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Contract is already registered.
        /// </exception>
        void Register<ContractType, ImplementationType>(String name);

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <typeparam name="ImplementationType">The implementation of the service.</typeparam>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Type does not implement contract.
        /// or
        /// Type is not instantiable.
        /// or
        /// Contract is already registered.
        /// </exception>
        void Register<ContractType, ImplementationType>(String name, Boolean overwrite);

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
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
        void Register(Type contractType, Type implementationType);

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
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
        void Register(Type contractType, Type implementationType, Boolean overwrite);

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
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
        void Register(String name, Type contractType, Type implementationType);

        /// <summary>
        /// Registers the specified service type in the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
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
        void Register(String name, Type contractType, Type implementationType, Boolean overwrite);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        void RegisterInstance<ContractType>(ContractType instance);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        void RegisterInstance<ContractType>(ContractType instance, Boolean overwrite);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        void RegisterInstance<ContractType>(String name, ContractType instance);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        void RegisterInstance<ContractType>(String name, ContractType instance, Boolean overwrite);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        void RegisterInstance(Type contractType, Object instance);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        void RegisterInstance(Type contractType, Object instance, Boolean overwrite);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The instance is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The instance does not implement the contract.</exception>
        void RegisterInstance(String name, Type contractType, Object instance);

        /// <summary>
        /// Registers the specified service instance in the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="instance">The instance of the service.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the registration if already present.</param>
        /// <exception cref="System.ArgumentNullException">
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
        Boolean Unregister<ContractType>();

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean Unregister(Type contractType);

        /// <summary>
        /// Unregisters the specified service from the container.
        /// </summary>
        /// <param name="name">The name under which the service is registered.</param>
        /// <returns><c>true</c> if the service was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean Unregister(String name);

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service instance.</typeparam>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</return
        Boolean UnregisterInstance<ContractType>();

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="contractType">The contract of the service instance.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean UnregisterInstance(Type contractType);

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="name">The name under which the service instance is registered.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean UnregisterInstance(String name);

        /// <summary>
        /// Unregisters the specified service instance from the container.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the service instance was registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        Boolean UnregisterInstance(Object instance);

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        Boolean IsRegistered<ContractType>();

        /// <summary>
        /// Determines whether the specified implementation is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        Boolean IsRegistered<ContractType, ImplementationType>();

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean IsRegistered(Type contractType);

        /// <summary>
        /// Determines whether the specified service is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean IsRegistered(String name);

        /// <summary>
        /// Determines whether the specified implementation is registered for the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="implementationType">The implementation of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean IsRegistered(Type contractType, Type implementationType);

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        Boolean IsRegisteredInstance<ContractType>();

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean IsRegisteredInstance(Type contractType);

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean IsRegisteredInstance(String name);

        /// <summary>
        /// Determines whether the specified service instance is registered.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The instance is null.</exception>
        Boolean IsRegisteredInstance(Object instance);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>The registered implementation of <typeparamref name="ContractType" />.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Contract is not an interface type.
        /// or
        /// Service is not registered.
        /// </exception>
        ContractType Resolve<ContractType>();

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>An instance of the registered implementation of <typeparamref name="ContractType" /> crated using the specified parameters.</returns>
        /// <exception cref="System.InvalidOperationException">Service is not registered.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        ContractType Resolve<ContractType>(Object[] parameters);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns>The registered implementation of <paramref name="contractType" />.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Contract is not an interface type.
        /// or
        /// Service is not registered.
        /// </exception>
        Object Resolve(Type contractType);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns>The registered implementation of <paramref name="contractType" />.</returns>
        /// <exception cref="System.ArgumentNullException">contractType;The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// The contract is null.
        /// or
        /// Service is not registered.
        /// </exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        Object Resolve(Type contractType, params Object[] parameters);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <returns>The registered instance of <typeparamref name="contractType" />.</returns>
        ContractType ResolveInstance<ContractType>();

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance of <typeparamref name="contractType" /> under the specified name.</returns>
        ContractType ResolveInstance<ContractType>(String name);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <returns>The registered instance of <paramref name="contractType" />.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">No instance is registered under the contract.</exception>
        Object ResolveInstance(Type contractType);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance of under the specified name.</returns>
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
        Object ResolveInstance(Type contractType, String name);
        
        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <returns>The registered instance of under the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        /// <exception cref="System.ArgumentException">No instance is registered under the name.</exception>
        Object ResolveInstance(String name);

        /// <summary>
        /// Tries to resolve the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">An instance of the registered implementation of <typeparamref name="ContractType" />.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        Boolean TryResolve<ContractType>(out ContractType resolvedInstance);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        Boolean TryResolve<ContractType>(Object[] parameters, out ContractType resolvedInstance);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Contract is not an interface type.
        /// or
        /// Service is not registered.
        /// </exception>
        Boolean TryResolve(Type contractType, out Object resolvedInstance);

        /// <summary>
        /// Resolves the specified service.
        /// </summary>
        /// <param name="contractType">The contract of the service.</param>
        /// <param name="parameters">The parameters of the service.</param>
        /// <returns><c>true</c> if the specified service is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">contractType;The contract is null.</exception>
        /// <exception cref="System.InvalidOperationException">Contract is not an interface type.
        /// or
        /// Service is not registered.</exception>
        /// <exception cref="System.ArgumentException">The service cannot be instantiated based on the specified parameters.</exception>
        Boolean TryResolve(Type contractType, Object[] parameters, out Object resolvedInstance);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        Boolean TryResolveInstance<ContractType>(out ContractType resolvedInstance);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <typeparam name="ContractType">The contract of the service.</typeparam>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <typeparamref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        Boolean TryResolveInstance<ContractType>(String name, out ContractType resolvedInstance);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The contract is null.</exception>
        Boolean TryResolveInstance(Type contractType, out Object resolvedInstance);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="contractType">Type of the contract.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The contract is null.
        /// or
        /// The name is null.
        /// </exception>
        Boolean TryResolveInstance(Type contractType, String name, out Object resolvedInstance);

        /// <summary>
        /// Resolves the specified instance.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="resolvedInstance">The registered instance of <paramref name="contractType" /> under the specified name.</param>
        /// <returns><c>true</c> if the specified service instance is registered; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The name is null.</exception>
        Boolean TryResolveInstance(String name, out Object resolvedInstance);
    }
}
