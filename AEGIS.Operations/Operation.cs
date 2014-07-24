/// <copyright file="Operation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Defines general behavior of operations.
    /// </summary>
    /// <typeparam name="ResultType">The type of the source.</typeparam>
    /// <typeparam name="SourceType">The type of the result.</typeparam>
    public abstract class Operation<SourceType, ResultType> : IOperation<SourceType, ResultType>
    {
        #region Private fields

        /// <summary>
        /// The operation method. This field is read-only.
        /// </summary>
        private readonly OperationMethod _method;

        /// <summary>
        /// The current operation state.
        /// </summary>
        private OperationState _state; 

        #endregion

        #region Protected fields

        /// <summary>
        /// The parameters of the operation. This field is read-only.
        /// </summary>
        protected readonly IDictionary<OperationParameter, Object> _parameters;

        /// <summary>
        /// The source object. This field is read-only.
        /// </summary>
        protected readonly SourceType _source;

        /// <summary>
        /// The result object.
        /// </summary>
        protected ResultType _result;
        
        #endregion

        #region IOperation properties

        /// <summary>
        /// Gets the method associated with the operation.
        /// </summary>
        /// <value>The associated coordinate operation method.</value>
        public OperationMethod Method { get { return _method; } }

        /// <summary>
        /// Gets the parameters of the operation.
        /// </summary>
        /// <value>The parameters of the operation stored as key/value pairs.</value>
        public IDictionary<OperationParameter, Object> Parameters { get { return _parameters; } }

        /// <summary>
        /// Gets a value indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get { return Method.IsReversible; } }

        /// <summary>
        /// Gets the operation state.
        /// </summary>
        /// <value>The current state of the operation.</value>
        public OperationState State { get { return _state; } }

        /// <summary>
        /// Gets the source of the operation.
        /// </summary>
        /// <value>The source of the operation.</value>
        public SourceType Source { get { return _source; } }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result of the operation.</value>
        public ResultType Result
        {
            get { if (_result == null) PrepareResult(); return _result; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{SourceType, ResultType}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        protected Operation(SourceType source, ResultType target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
        {
            _state = OperationState.Initializing;

            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");
            if (target != null && ReferenceEquals(source, target) && method.SupportedModes.Contains(ExecutionMode.InPlace))
                throw new ArgumentException("The specified source and result are the same objects, but the method does not support in-place operations.", "target");

            if (parameters == null && method.Parameters != null && method.Parameters.Any(parameter => !parameter.IsOptional))
                throw new ArgumentNullException("parameters", "The method requires parameters which are not specified.");

            if (parameters != null && method.Parameters != null)
            {
                foreach (OperationParameter parameter in method.Parameters)
                {
                    // check parameter existence
                    if (!parameter.IsOptional && (!parameters.ContainsKey(parameter) || parameters[parameter] == null))
                        throw new ArgumentException("The parameters do not contain a required parameter value (" + parameter.Name + ").", "parameters");
                    
                    if (parameters.ContainsKey(parameter) && parameters[parameter] != null)
                    {
                        // check parameter type
                        if (!(parameter.Type.GetInterfaces().Contains(typeof(IConvertible)) && parameters[parameter] is IConvertible) &&
                            !parameter.Type.Equals(parameters[parameter].GetType()) && 
                            !parameters[parameter].GetType().IsSubclassOf(parameter.Type) &&
                            !parameters[parameter].GetType().GetInterfaces().Contains(parameter.Type))
                            throw new ArgumentException("The type of a parameter value (" + parameter.Name + ") does not match the type specified by the method.", "parameters");

                        // check parameter value
                        if (!parameter.IsValid(parameters[parameter]))
                            throw new ArgumentException("The parameter value (" + parameter.Name + ") does not satisfy the conditions of the parameter.", "parameters");
                    }
                }
            }

            _source = source;
            _result = target;
            _method = method;

            if (parameters != null)
            {
                _parameters = new Dictionary<OperationParameter, Object>(method.Parameters.Count);
                // only keep the parameters which apply according to the method
                foreach (OperationParameter parameter in parameters.Keys)
                {
                    if (method.Parameters.Contains(parameter))
                        _parameters.Add(parameter, parameters[parameter]);
                }
            }

            _state = OperationState.Initialized;
        }

        #endregion

        #region IOperation methods

        /// <summary>
        /// Executes the operation.
        /// </summary>
        public void Execute()
        {
            _state = OperationState.Preparing;
            PrepareResult();
            _state = OperationState.Executing;
            ComputeResult();
            _state = OperationState.Finalizing;
            FinalizeResult();
            _state = OperationState.Finished;
        }

        /// <summary>
        /// Gets the reverse operation.
        /// </summary>
        /// <returns>The reverse operation.</returns>
        public IOperation<ResultType, SourceType> GetReverseOperation()
        {
            if (!Method.IsReversible)
                throw new NotSupportedException("Operation is not reversible.");

            return ComputeReverseOperation();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected virtual void PrepareResult() { }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected abstract void ComputeResult();

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected virtual void FinalizeResult() { }

        /// <summary>
        /// Computes the reverse operation.
        /// </summary>
        /// <returns>The reverse operation.</returns>
        protected virtual IOperation<ResultType, SourceType> ComputeReverseOperation() { return null; }

        /// <summary>
        /// Resolves the specified parameter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The specified parameter value or the default value if none specified.</returns>
        protected Object ResolveParameter(OperationParameter parameter)
        {
            if (_parameters != null && _parameters.ContainsKey(parameter))
                return _parameters[parameter];

            return parameter.DefaultValue;
        }

        /// <summary>
        /// Resolves the specified parameter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The specified parameter value or the default value if none specified.</returns>
        protected T ResolveParameter<T>(OperationParameter parameter)
        {
            if (_parameters != null && _parameters.ContainsKey(parameter) && _parameters[parameter] is T)
                return (T)_parameters[parameter];

            return (T)parameter.DefaultValue;
        }

        /// <summary>
        /// Resolves the specified parameter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter value.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The parameter value or the specified default value if not found.</returns>
        protected T ResolveParameter<T>(OperationParameter parameter, T defaultValue)
        {
            if (_parameters == null || !_parameters.ContainsKey(parameter) || !(_parameters[parameter] is T))
                return defaultValue;

            return (T)_parameters[parameter];
        }


        /// <summary>
        /// Determines whether the specified parameter is porovided.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns><c>true</c> if the parameter is provided; otherwise, <c>false</c>.</returns>
        protected Boolean IsProvidedParameter(OperationParameter parameter)
        {
            return _parameters != null && _parameters.ContainsKey(parameter);
        }

        #endregion
    }
}
