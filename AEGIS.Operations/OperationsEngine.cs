// <copyright file="OperationsEngine.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ELTE.AEGIS.Operations
{
    using IOperation = IOperation<Object, Object>;

    /// <summary>
    /// Represents an engine performing the management and execution of operations.
    /// </summary>
    public class OperationsEngine
    {
        #region Private fields

        private readonly List<OperationMethod> _methods;
        private readonly Dictionary<OperationMethod, Type> _operations;

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the currently loaded operations.
        /// </summary>
        public IList<OperationMethod> Operations { get { return _methods.AsReadOnly(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsEngine" /> class.
        /// </summary>
        public OperationsEngine()
        {
            _methods = new List<OperationMethod>();
            _operations = new Dictionary<OperationMethod, Type>();

            LoadOperations(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load the operations from the specified assembly.
        /// </summary>
        /// <param name="path">The path of the assembly.</param>
        public void LoadOperations(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            try
            {
                LoadOperations(Assembly.LoadFrom(path));
            }
            catch
            {
                throw new ArgumentException("The path is invalid.", "path");
            }
        }

        /// <summary>
        /// Returns the specified operation.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source.</param>
        /// <returns>The operation of the specified method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public IOperation GetOperation(OperationMethod method, IDictionary<OperationParameter, Object> parameters, Object source)
        {
            return GetOperation(method, parameters, source, null);
        }

        /// <summary>
        /// Returns the specified operation.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The operation of the specified method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public IOperation GetOperation(OperationMethod method, IDictionary<OperationParameter, Object> parameters, Object source, Object target)
        {
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.GetType().Equals(method.SourceType) && !source.GetType().IsSubclassOf(method.SourceType) && !source.GetType().GetInterfaces().Contains(method.SourceType))
                throw new ArgumentException("The specified source type is not supported by the method.", "source");

            Type operationType;
            if (!_operations.TryGetValue(method, out operationType))
                throw new ArgumentException("The specified method is not available.", "method");

            return Activator.CreateInstance(operationType, source, target, parameters) as IOperation;
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="source">The source.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public Object ExecuteOperation(OperationConfiguration operation, Object source)
        {
            return ExecuteOperation(operation.Method, operation.Parameters, source);
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public void ExecuteOperation(OperationConfiguration operation, Object source, Object target)
        {
            ExecuteOperation(operation.Method, operation.Parameters, source, target);
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="method">The operation method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public Object ExecuteOperation(OperationMethod method, IDictionary<OperationParameter, Object> parameters, Object source)
        {
            IOperation operation = GetOperation(method, parameters, source);
            operation.Execute();
            return operation.Result;
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="method">The operation method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// </exception>
        public void ExecuteOperation(OperationMethod method, IDictionary<OperationParameter, Object> parameters, Object source, Object target)
        {
            IOperation operation = GetOperation(method, parameters, source, target);
            operation.Execute();
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="source">The source.</param>
        /// <returns>
        /// The result of the operation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified source type is not supported by the method.
        /// or
        /// The specified method is not available.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public Object ExecuteOperation(IList<OperationConfiguration> operations, Object source)
        {
            if (operations == null)
                throw new ArgumentNullException("operations", "The array of operations is null.");
            if (operations.Count == 0)
                throw new ArgumentException("The array of operations is empty.", "operations");
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            // check source types for all methods
            if (source.GetType().Equals(operations[0].Method.SourceType) && !source.GetType().IsSubclassOf(operations[0].Method.SourceType) && !source.GetType().GetInterfaces().Contains(operations[0].Method.SourceType))
                throw new ArgumentException("The specified source type is not supported by the method.", "source");

            for (Int32 i = 1; i < operations.Count; i++)
            {
                if (!operations[i].Method.SourceType.Equals(operations[i - 1].Method.ResultType) && !operations[i - 1].Method.ResultType.GetType().IsSubclassOf(operations[i].Method.SourceType) && !operations[i - 1].Method.ResultType.GetInterfaces().Contains(operations[i - 1].Method.ResultType))
                    throw new ArgumentException("The specified source type is not supported by the method (" + operations[i].Method.Name + ").", "source");
            }

            // load methods
            IOperation[] operationsArray = new IOperation[operations.Count];
            
            Type operationType;
            if (!_operations.TryGetValue(operations[0].Method, out operationType))
                throw new ArgumentException("The specified method (" + operations[0].Method.Name + ") is not available.", "operations");
            operationsArray[0] = Activator.CreateInstance(operationType, source, operations[0].Parameters) as IOperation;

            for (Int32 i = 1; i < operations.Count; i++)
            {
                if (!_operations.TryGetValue(operations[i].Method, out operationType))
                    throw new ArgumentException("The specified method (" + operations[i].Method.Name + ") is not available.", "operations");

                // the source of an operation is the result of the previous operation
                operationsArray[i] = Activator.CreateInstance(operationType, source, operationsArray[i - 1].Result, operations[i].Parameters) as IOperation;
            }

            // execute the final operation
            operationsArray[operationsArray.Length - 1].Execute();

            // get the result of the final operation
            return operationsArray[operationsArray.Length - 1].Result;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the operations from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private void LoadOperations(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            List<OperationMethod> methods = new List<OperationMethod>();

            // get operation methods
            foreach (Type collectionType in types.Where(type => type.GetCustomAttributes(typeof(IdentifiedObjectCollectionAttribute), false).Any(attribute => (attribute as IdentifiedObjectCollectionAttribute).Type == typeof(OperationMethod) || (attribute as IdentifiedObjectCollectionAttribute).Type.IsSubclassOf(typeof(OperationMethod)))))
            {
                methods.AddRange(collectionType.InvokeMember("All", BindingFlags.GetProperty, null, null, null) as IList<OperationMethod>);
            }

            // get operations
            foreach (Type operationType in types.Where(type => !type.IsAbstract && type.GetCustomAttributes(typeof(IdentifiedObjectInstanceAttribute), false).Length > 0))
            {
                IdentifiedObjectInstanceAttribute attribute = operationType.GetCustomAttributes(typeof(IdentifiedObjectInstanceAttribute), false)[0] as IdentifiedObjectInstanceAttribute;
                OperationMethod method = methods.FirstOrDefault(m => m.Identifier.Equals(attribute.Identifier));

                if (method != null) // add methods and operations to the collection
                {
                    if (_methods.Contains(method))
                    {
                        _operations[method] = operationType;
                    }
                    else
                    {
                        _methods.Add(method);
                        _operations.Add(method, operationType);
                    }
                }
            }
        }

        #endregion
    }
}
