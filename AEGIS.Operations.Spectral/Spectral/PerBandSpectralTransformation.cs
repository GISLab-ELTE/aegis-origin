/// <copyright file="PerBandSpectralTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a transformation that is applied on the individual bands in the spectral data of the geometry.
    /// </summary>
    public abstract class PerBandSpectralTransformation : SpectralTransformation
    {
        #region Protected fields

        protected Int32[] _sourceBandIndices;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PerBandRasterTransformation" /> class.
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
        /// The source is invalid.
        /// or
        /// The target is invalid.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        protected PerBandSpectralTransformation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (IsProvidedParameter(SpectralOperationParameters.BandIndex))
            {
                _sourceBandIndices = new Int32[] { Convert.ToInt32(ResolveParameter(SpectralOperationParameters.BandIndex)) };

                if (_sourceBandIndices[0] < 0 || _sourceBandIndices[0] >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("parameters", "A parameter value does not satisfy the conditions of the parameter.", new ArgumentOutOfRangeException("BandIndex", "BandIndex is not within the range 0.." + (Source.Raster.NumberOfBands - 1) + "."));

            }
            else if (IsProvidedParameter(SpectralOperationParameters.BandIndices))
            {
                _sourceBandIndices = ResolveParameter<Int32[]>(SpectralOperationParameters.BandIndices);

                if (_sourceBandIndices.Any(index => index < 0 || index >= Source.Raster.NumberOfBands))
                    throw new ArgumentException("parameters", "A parameter value does not satisfy the conditions of the parameter.", new ArgumentOutOfRangeException("BandIndices", "One or more values within BandIndices is not within the range 0.." + (Source.Raster.NumberOfBands - 1) + "."));
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_sourceBandIndices != null)
            {
                Int32[] radiometricResolutions = new Int32[_sourceBandIndices.Length];

                for (Int32 k = 0; k < _sourceBandIndices.Length; k++)
                    radiometricResolutions[k] = _source.Raster.RadiometricResolutions[_sourceBandIndices[k]];

                _result = _source.Factory.CreateSpectralGeometry(_source, 
                                                                 PrepareRasterResult(_source.Raster.Format,
                                                                                     _sourceBandIndices.Length,
                                                                                     _source.Raster.NumberOfRows,
                                                                                     _source.Raster.NumberOfColumns, 
                                                                                     _sourceBandIndices.Select(index => _source.Raster.RadiometricResolutions[index]).ToArray(),
                                                                                     _source.Raster.Mapper),
                                                                 _source.Presentation, 
                                                                 _source.Imaging);
            }
            else
            {
                base.PrepareResult();
            }
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override sealed void ComputeResult()
        {
            if (_sourceBandIndices != null)
            {
                if (_result.Raster.Format == RasterFormat.Floating)
                {
                    for (Int32 k = 0; k < _sourceBandIndices.Length; k++)
                        for (Int32 i = 0; i < _result.Raster.NumberOfRows; i++)
                            for (Int32 j = 0; j < _result.Raster.NumberOfColumns; j++)
                                _result.Raster.SetFloatValue(i, j, 0, ComputeFloat(i, j, _sourceBandIndices[k]));
                }
                else
                {
                    for (Int32 k = 0; k < _sourceBandIndices.Length; k++)
                        for (Int32 i = 0; i < _result.Raster.NumberOfRows; i++)
                            for (Int32 j = 0; j < _result.Raster.NumberOfColumns; j++)
                                _result.Raster.SetValue(i, j, 0, Compute(i, j, _sourceBandIndices[k]));
                }
            }
            else
            {
                if (_result.Raster.Format == RasterFormat.Floating)
                {
                    for (Int32 k = 0; k < _result.Raster.NumberOfBands; k++)
                        for (Int32 i = 0; i < _result.Raster.NumberOfRows; i++)
                            for (Int32 j = 0; j < _result.Raster.NumberOfColumns; j++)
                                _result.Raster.SetFloatValue(i, j, k, ComputeFloat(i, j, k));
                }
                else
                {
                    for (Int32 k = 0; k < _result.Raster.NumberOfBands; k++)
                        for (Int32 i = 0; i < _result.Raster.NumberOfRows; i++)
                            for (Int32 j = 0; j < _result.Raster.NumberOfColumns; j++)
                                _result.Raster.SetValue(i, j, k, Compute(i, j, k));
                }
            }
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32[] values = new UInt32[_result.Raster.NumberOfBands];
            for (Int32 k = 0; k < _result.Raster.NumberOfBands; k++)
                values[k] = Compute(rowIndex, columnIndex, k);

            return values;
        }

        /// <summary>
        /// Computes the specified floating spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Double[] values = new Double[_result.Raster.NumberOfBands];
            for (Int32 k = 0; k < _result.Raster.NumberOfBands; k++)
                values[k] = ComputeFloat(rowIndex, columnIndex, k);

            return values;
        }

        #endregion
    }
}
