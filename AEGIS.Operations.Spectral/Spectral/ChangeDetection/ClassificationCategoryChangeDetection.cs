// <copyright file="ClassificationCategoryChangeDetection.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.ChangeDetection
{
    /// <summary>
    /// Represents an operation that computes the change matrix of the classification categories compared to a reference image.
    /// </summary>
    /// <author>Tamas Nagy</author>
    public class ClassificationCategoryChangeDetection : ClassificationChangeDetection<Double[,]>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationCategoryChangeDetection" /> class.
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
        public ClassificationCategoryChangeDetection(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters) 
            : base(source, SpectralOperationMethods.ClassificationCategoryChangeDetection, parameters)
        {
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override Double[,] PrepareResult()
        {
            Int32 sourceCategories = GetNumberOfCategories(Source.Raster);
            Int32 referenceCategories = GetNumberOfCategories(Reference.Raster);

            return new Double[sourceCategories, referenceCategories];
        }

        #endregion

        #region Protected ClassificationChangeDetection methods

        /// <summary>
        /// Computes the change for the specified values at the specified location.
        /// </summary>
        protected override void ComputeChange(Int32 rowIndex, Int32 columnIndex, UInt32 value, UInt32 referenceValue)
        {
            if (value == referenceValue)
                Result[value, referenceValue]++;
            else
            {
                if(referenceValue <= Result.GetLength(0) - 1 && value <= Result.GetLength(1) - 1)
                    Result[referenceValue, value]++;
                if(value <= Result.GetLength(0) - 1 && value <= Result.GetLength(1) - 1)
                    Result[value, referenceValue]--;
            }
        }

        #endregion
    }
}
