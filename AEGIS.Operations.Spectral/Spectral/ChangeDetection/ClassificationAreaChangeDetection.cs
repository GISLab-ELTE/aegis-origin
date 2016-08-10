/// <copyright file="ClassificationAreaChangeDetection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Operations.Management;

namespace ELTE.AEGIS.Operations.Spectral.ChangeDetection
{
    /// <summary>
    /// Represents an operation that computes the area change of each classification category compared to a reference image.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253503", "Classification area change detection")]
    public class ClassificationAreaChangeDetection : ClassificationChangeDetection<Double[]>
    {
        #region Private fields

        private readonly Boolean _differencialChangeDetection;
        private readonly Boolean _differentialAreaComputation;
        private readonly SortedDictionary<UInt32, Int32> _inputCounts;
        private readonly SortedDictionary<UInt32, Int32> _referenceCounts;
        private Int32 _numberOfValues;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationAreaChangeDetection" /> class.
        /// </summary>
        /// <param name="source">The input.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public ClassificationAreaChangeDetection(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, SpectralOperationMethods.ClassificationAreaChangeDetection, parameters)
        {
            _differencialChangeDetection = ResolveParameter<Boolean>(SpectralOperationParameters.DifferentialChangeDetection);
            _differentialAreaComputation = ResolveParameter<Boolean>(SpectralOperationParameters.DifferentialAreaComputation);

            _inputCounts = new SortedDictionary<UInt32, Int32>();
            _referenceCounts = new SortedDictionary<UInt32, Int32>();
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override Double[] FinalizeResult()
        {
            Double[] result = new Double[_inputCounts.Keys.Max() + 1];

            foreach (UInt32 category in _inputCounts.Keys)
            {
                Int32 referenceCount = _referenceCounts.ContainsKey(category) ? _referenceCounts[category] : 0;

                if (referenceCount == 0)
                {
                    result[category] = Double.PositiveInfinity;
                    continue;
                }

                if (_differencialChangeDetection)
                {
                    Int32 diffCount = _inputCounts[category] - referenceCount;

                    if (_differentialAreaComputation)
                        result[category] = (Double)diffCount / referenceCount;
                    else
                        result[category] = (Double)diffCount / _numberOfValues;
                }
                else
                {
                    if (_differentialAreaComputation)
                        result[category] = (Double)_inputCounts[category] / referenceCount;
                    else
                        result[category] = (Double)_inputCounts[category] / _numberOfValues;
                }
            }

            return result;
        }

        #endregion

        #region Protected ClassificationChangeDetection methods        

        /// <summary>
        /// Computes the change for the specified values at the specified location.
        /// </summary>
        protected override void ComputeChange(Int32 rowIndex, Int32 columnIndex, UInt32 value, UInt32 referenceValue)
        {
            _numberOfValues++;

            if (_inputCounts.ContainsKey(value))
                _inputCounts[value]++;
            else
                _inputCounts.Add(value, 1);

            if (_referenceCounts.ContainsKey(referenceValue))
                _referenceCounts[referenceValue]++;
            else
                _referenceCounts.Add(referenceValue, 1);
        }

        #endregion
    }
}
