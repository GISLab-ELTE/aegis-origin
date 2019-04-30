/// <copyright file="AssemblyOperationProvider.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace ELTE.AEGIS.Operations.Providers
{
    /// <summary>
    /// Representing an operation provider working with an assembly.
    /// </summary>
    public class AssemblyOperationProvider : IOperationProvider
    {
        #region Private fields

        /// <summary>
        /// The assembly.
        /// </summary>
        private Assembly _assembly;

        #endregion

        #region IOperationProvider properties

        /// <summary>
        /// Gets the uniform resource identifier (URI) of the provider.
        /// </summary>
        /// <value>The uniform resource identifier of the provider.</value>
        public Uri Uri { get { return new Uri(_assembly.Location); } }

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyOperationProvider" /> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="System.ArgumentNullException">The assembly is null.</exception>
        public AssemblyOperationProvider(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly", "The assembly is null.");

            _assembly = assembly;
        }

        #endregion

        #region IOperationProvider methods

        /// <summary>
        /// Returns the operation methods provided by the location.
        /// </summary>
        /// <returns>The read-only list of operation methods provided by the location.</returns>
        public IList<OperationMethod> GetMethods()
        {
            Type[] types = _assembly.GetTypes();
            List<OperationMethod> listOfMethods = new List<OperationMethod>();

            // get methods
            foreach (Type collectionType in types.Where(type => type.GetCustomAttributes(typeof(OperationMethodCollectionAttribute), false).Length > 0))
            {
                listOfMethods.AddRange(collectionType.InvokeMember("All", BindingFlags.GetProperty, null, null, null) as IList<OperationMethod>);
            }

            return listOfMethods.AsReadOnly();
        }

        /// <summary>
        /// Returns all operations provided by the provider.
        /// </summary>
        /// <returns>The read-only dictionary of provided operations.</returns>
        public IDictionary<OperationMethod, IList<Type>> GetOperations()
        {
            Dictionary<OperationMethod, IList<Type>> operations = new Dictionary<OperationMethod, IList<Type>>();

            Type[] types = _assembly.GetTypes();
            List<OperationMethod> listOfMethods = new List<OperationMethod>();

            // get methods
            foreach (Type collectionType in types.Where(type => type.GetCustomAttributes(typeof(OperationMethodCollectionAttribute), false).Length > 0))
            {
                listOfMethods.AddRange(collectionType.InvokeMember("All", BindingFlags.GetProperty, null, null, null) as IList<OperationMethod>);
            }

            // get operations
            foreach (Type operationType in types.Where(type => !type.IsAbstract && type.GetCustomAttributes(typeof(OperationMethodImplementationAttribute), false).Length > 0))
            {
                OperationMethodImplementationAttribute attribute = operationType.GetCustomAttributes(typeof(OperationMethodImplementationAttribute), false)[0] as OperationMethodImplementationAttribute;
                OperationMethod method = listOfMethods.FirstOrDefault(m => m.Identifier.Equals(attribute.Identifier));

                if (method != null) // add methods and operations to the collection
                {
                    if (!operations.ContainsKey(method))
                        operations.Add(method, new List<Type>());

                    operations[method].Add(operationType);
                }
            }

            return new ReadOnlyDictionary<OperationMethod, IList<Type>>(operations);
        }

        /// <summary>
        /// Returns the operations for the specified method.
        /// </summary>
        /// <param name="method">The operation method.</param>
        /// <returns>The read-only list of provided operations for the specified method.</returns>
        /// <exception cref="System.ArgumentNullException">The method is null.</exception>
        public IList<Type> GetOperations(OperationMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");

            List<Type> operation = new List<Type>();

            Type[] types = _assembly.GetTypes();

            // get operations
            foreach (Type operationType in types.Where(type => !type.IsAbstract && type.GetCustomAttributes(typeof(OperationMethodImplementationAttribute), false).Length > 0))
            {
                OperationMethodImplementationAttribute attribute = operationType.GetCustomAttributes(typeof(OperationMethodImplementationAttribute), false)[0] as OperationMethodImplementationAttribute;

                // the operation matches the specified method
                if (attribute.Identifier.Equals(method.Identifier) && attribute.Version.Equals(method.Version))
                {
                    operation.Add(operationType);
                }
            }

            return operation.AsReadOnly();
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        #endregion
    }
}
