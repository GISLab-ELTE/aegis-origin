/// <copyright file="RandomColorClassification.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents an operation performing spectral classification with random colors.
    /// </summary>
    public class RandomColorClassification : SpectralTransformation
    {
        #region Protected fields

        /// <summary>
        /// The maximum spectral value.
        /// </summary>
        private Int32 _maxSpectralValue;

        /// <summary>
        /// The array of generated random colors.
        /// </summary>
        protected Int32[] _colors;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomColorClassification"/> class.
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
        /// </exception>
        public RandomColorClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomColorClassification"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
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
        public RandomColorClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.RandomColorClassification, parameters)
        {
            if (source.Raster.NumberOfBands != 1)
                throw new ArgumentException("The number of bands in the source geometry is not equal to 1.", "source");
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _maxSpectralValue = 0;
            for (Int32 bandIndex = 0; bandIndex < _source.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < _source.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < _source.Raster.NumberOfColumns; columnIndex++)
                        _maxSpectralValue = Math.Max(_maxSpectralValue, (Int32)_source.Raster.GetValue(rowIndex, columnIndex, bandIndex));

            _result = _source.Factory.CreateSpectralGeometry(_source,
                                                             PrepareRasterResult(RasterFormat.Integer,
                                                                                 3,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 _maxSpectralValue > Calculator.Pow(256, 3) ? 16 : 8,
                                                                                 _source.Raster.Mapper),
                                                             RasterPresentation.CreateTrueColorPresentation(),
                                                             null);

            // selection of colors with equal range from minimum to maximum
            _colors = Enumerable.Range(1, _maxSpectralValue + 1).Select(number => (Int32)(number * ((1 << 24) - 1.0) / (_maxSpectralValue + 1))).ToArray();

            // randomization
            Random random = new Random();
            for (Int32 colorIndex = 0; colorIndex < _colors.Length; colorIndex++)
            {
                Int32 replaceIndex = random.Next(0, _colors.Length);
                Int32 temp = _colors[colorIndex];
                _colors[colorIndex] = _colors[replaceIndex];
                _colors[replaceIndex] = temp;
            }

        }

        #endregion

        #region Protected SpectralOperation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            UInt32 colorNumber = _source.Raster.GetValue(rowIndex, columnIndex, 0);

            switch (bandIndex)
            { 
                case 0:
                    return (UInt32)_colors[colorNumber] >> 16;
                case 1:
                    return (UInt32)(_colors[colorNumber] % (1 << 16)) >> 8;
                case 2:
                    return (UInt32)(_colors[colorNumber] % (1 << 8));
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Computes the specified spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32 colorNumber = _source.Raster.GetValue(rowIndex, columnIndex, 0);

            UInt32[] value = new UInt32[3];
            value[0] = (UInt32)_colors[colorNumber] >> 16;
            value[1] = (UInt32)(_colors[colorNumber] % (1 << 16)) >> 8;
            value[2] = (UInt32)(_colors[colorNumber] % (1 << 8));

            return value;
        }

        #endregion
    }
}
