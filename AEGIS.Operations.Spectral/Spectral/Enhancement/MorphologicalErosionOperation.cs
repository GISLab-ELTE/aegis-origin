/// <copyright file="ErosionMathematicalMorphology.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Enhancement
{
    /// <summary>
    /// Represents the erosion morphological operation.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213291", "Morphological erosion operation")]
    public class MorphologicalErosionOperation: PerBandSpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The structuring element used to apply erosion.
        /// </summary>
        private Matrix _structuringElement;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MorphologicalErosionOperation"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        public MorphologicalErosionOperation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.MorphologicalErosionOperation, parameters)
        {
            _structuringElement = ResolveParameter<Matrix>(SpectralOperationParameters.MorphologicalStructuringElement);
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>
        /// The spectral value at the specified index.
        /// </returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Int32 rowCenter = (_structuringElement.NumberOfRows - 1) / 2;
            Int32 columnCenter = (_structuringElement.NumberOfColumns - 1) / 2;

            Int32 firstRowIndex = rowIndex - rowCenter;
            Int32 firstColumnIndex = columnIndex - columnCenter;
            UInt32 result = RasterAlgorithms.RadiometricResolutionMax(_source.Raster.RadiometricResolutions[bandIndex]);

            for (Int32 row = 0; row < _structuringElement.NumberOfRows; row++)
            {
                for (Int32 column = 0; column < _structuringElement.NumberOfColumns; column++)
                {
                    UInt32 value = (UInt32)_source.Raster.GetNearestValue(firstRowIndex + row, firstColumnIndex + column, bandIndex) - (UInt32)_structuringElement[row, column];
                    if (value < 0)
                        value = 0;

                    if (value < result)
                        result = value;
                }
            }

            return result;
        }

        #endregion

    }
}
