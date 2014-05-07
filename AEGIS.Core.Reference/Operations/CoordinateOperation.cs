/// <copyright file="CoordinateOperation.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.ObjectModel;
using System.Linq;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a generic coordinate operation between different coordinate types.
    /// </summary>
    /// <typeparam name="SourceType">The type of the source.</typeparam>
    /// <typeparam name="ResultType">The type of the result.</typeparam>
    /// <remarks>
    /// A mathematical operation on coordinates that transforms or converts coordinates to another coordinate reference system. 
    /// Many but not all coordinate operations (from CRS A to CRS B) also uniquely define the inverse coordinate operation (from CRS B to CRS A). 
    /// In some cases, the coordinate operation method algorithm for the inverse coordinate operation is the same as for the forward algorithm, 
    /// but the signs of some coordinate operation parameter values have to be reversed. In other cases, different algorithms are required for 
    /// the forward and inverse coordinate operations, but the same coordinate operation parameter values are used. 
    /// If (some) entirely different parameter values are needed, a different coordinate operation shall be defined.
    /// </remarks>
    public abstract class CoordinateOperation<SourceType, ResultType> : IdentifiedObject
    {
        #region Protected fields

        protected readonly CoordinateOperationMethod _method;
        protected readonly IDictionary<CoordinateOperationParameter, Object> _parameters;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the method associated with the operation.
        /// </summary>
        /// <value>The associated coordinate operation method.</value>
        public CoordinateOperationMethod Method { get { return _method; } }

        /// <summary>
        /// Gets the parameters of the operation.
        /// </summary>
        /// <value>The parameters of the operation stored as key/value pairs.</value>
        public IDictionary<CoordinateOperationParameter, Object> Parameters { get { return new ReadOnlyDictionary<CoordinateOperationParameter, Object>(_parameters); } }

        /// <summary>
        /// Gets a value indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get { return _method.IsReversible; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperation{SourceType, DestinationType}" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameteres which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a scalar value as required by the method.
        /// </exception>
        protected CoordinateOperation(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters) : base(identifier, name)
        {
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");

            if (parameters == null && method.Parameters != null && method.Parameters.Count > 0)
                throw new ArgumentNullException("parameters", "The method requires parameteres which are not specified.");

            if (parameters != null && method.Parameters != null)
            {
                foreach (CoordinateOperationParameter parameter in method.Parameters)
                {
                    // check for parameter existance
                    if (!parameters.ContainsKey(parameter) || parameters[parameter] == null) 
                        throw new ArgumentException("The parameters do not contain a required parameter value (" + parameter.Name + ").", "parameters");

                    // check for parameter type
                    switch (parameter.UnitType)
                    {
                        case UnitQuantityType.Angle:
                            if (!(parameters[parameter] is Angle))
                                throw new ArgumentException("The parameter '" + parameter.Name + "' is not an angular value as required by the method.", "parameters");
                            break;
                        case UnitQuantityType.Length:
                            if (!(parameters[parameter] is Length))
                                throw new ArgumentException("The parameter '" + parameter.Name + "' is not a length value as required by the method.", "parameters");
                            break;
                        case UnitQuantityType.Scale:
                            if (!(parameters[parameter] is IConvertible))
                                throw new ArgumentException("The parameter '" + parameter.Name + "' is not a scalar value as required by the method.", "parameters");
                            break;
                    }
                }
            }

            _method = method;

            if (parameters != null)
            {
                _parameters = new Dictionary<CoordinateOperationParameter, Object>(method.Parameters.Count);
                // only keep the parameters which apply according to the method
                foreach (CoordinateOperationParameter parameter in parameters.Keys)
                {
                    if (method.Parameters.Contains(parameter))
                        _parameters.Add(parameter, parameters[parameter]);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public ResultType Forward(SourceType coordinate)
        {
            return ComputeForward(coordinate);
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinates are null.</exception>
        public ResultType[] Forward(params SourceType[] coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinates are null.");

            ResultType[] result = new ResultType[coordinates.Length];
            for (Int32 i = 0; i < result.Length; i++)
            {
                result[i] = ComputeForward(coordinates[i]);
            }
            return result;
        }

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinates are null.</exception>
        public ResultType[] Forward(IEnumerable<SourceType> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinates are null.");

            ResultType[] result = new ResultType[coordinates.Count()];
            Int32 i = 0;
            foreach (SourceType location in coordinates)
            {
                result[i] = ComputeForward(location);
                i++;
            }
            return result;
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        /// <exception cref="System.NotSupportedException">Coordinate operation is not reversible.</exception>
        public SourceType Reverse(ResultType coordinate)
        {
            if (!_method.IsReversible)
                throw new NotSupportedException("Coordinate operation is not reversible.");

            return ComputeReverse(coordinate);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.NotSupportedException">Coordinate operation is not reversible.</exception>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinates are null.</exception>
        public SourceType[] Reverse(params ResultType[] coordinates)
        {
            if (!_method.IsReversible)
                throw new NotSupportedException("Coordinate operation is not reversible.");

            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinates are null.");

            SourceType[] result = new SourceType[coordinates.Length];
            for (Int32 i = 0; i < result.Length; i++)
            {
                result[i] = ComputeReverse(coordinates[i]);
            }
            return result;
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <exception cref="System.NotSupportedException">Coordinate operation is not reversible.</exception>
        /// <exception cref="System.ArgumentNullException">coordinates;The coordinates are null.</exception>
        public SourceType[] Reverse(IEnumerable<ResultType> coordinates)
        {
            if (!_method.IsReversible)
                throw new NotSupportedException("Coordinate operation is not reversible.");

            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The coordinates are null.");

            SourceType[] result = new SourceType[coordinates.Count()];
            Int32 i = 0;
            foreach (ResultType coordinate in coordinates)
            {
                result[i] = ComputeReverse(coordinate);
                i++;
            }
            return result;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected abstract ResultType ComputeForward(SourceType coordinate);
        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected abstract SourceType ComputeReverse(ResultType coordinate);

        #endregion
    }
}
