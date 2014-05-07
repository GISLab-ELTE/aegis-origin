﻿/// <copyright file="GeometryToGraphConversion.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Conversion
{
    /// <summary>
    /// Represents an operation for converting geometry to graph represetation.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::212100", "Geometry to graph conversion")]
    public class GeometryToGraphConversion : Operation<IGeometry, IGeometryGraph>
    {
        #region Protected fields

        protected readonly Boolean _bidirectionalConversion;
        protected readonly Boolean _metadataPreservation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryToGraphConversion" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
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
        public GeometryToGraphConversion(IGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryToGraphConversion" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The result.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
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
        public GeometryToGraphConversion(IGeometry source, IGeometryGraph target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, ConversionMethods.GeometryToGraphConversion, parameters)
        {
            _bidirectionalConversion = Convert.ToBoolean(GetParameter(ConversionParameters.BidirectionalConversion));
            _metadataPreservation = Convert.ToBoolean(GetParameter(OperationParameters.MetadataPreservation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryToGraphConversion" /> class.
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
        protected GeometryToGraphConversion(IGeometry source, IGeometryGraph target, OperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _bidirectionalConversion = Convert.ToBoolean(GetParameter(ConversionParameters.BidirectionalConversion));
            _metadataPreservation = Convert.ToBoolean(GetParameter(OperationParameters.MetadataPreservation));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_state == OperationState.Preparing && _result == null)
                _result = _source.Factory.CreateGraph(_source.Metadata);
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            _source.ToGraph(_result, _bidirectionalConversion, _metadataPreservation);
        }

        #endregion
    }
}
