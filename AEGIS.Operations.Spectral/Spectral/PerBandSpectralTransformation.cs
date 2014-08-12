/// <copyright file="PerBandSpectralTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a transformation that is applied on the individual bands in the spectral data of the geometry.
    /// </summary>
    public abstract class PerBandSpectralTransformation : SpectralTransformation
    {
        #region Protected fields

        protected Int32 _sourceBandIndex;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Operations.Spectral.PerBandRasterTransformation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The specified band index parameter or greater or equal to the spectral resoultion of the source.</exception>
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
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        protected PerBandSpectralTransformation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.BandIndex))
            {
                _sourceBandIndex = Convert.ToInt32(parameters[SpectralOperationParameters.BandIndex]);

                if (_sourceBandIndex >= source.Raster.NumberOfBands)
                    throw new ArgumentOutOfRangeException("The specified band index parameter or greater or equal to the spectral resoultion of the source.", "parameters");
            }
            else
                _sourceBandIndex = Int32.MinValue;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_sourceBandIndex >= 0)
            {
                _result = _source.Factory.CreateSpectralGeometry(_source, 
                                                                 PrepareRasterResult(_source.Raster.Representation,
                                                                                     1,
                                                                                     _source.Raster.NumberOfRows,
                                                                                     _source.Raster.NumberOfColumns,
                                                                                     new Int32[] { _source.Raster.RadiometricResolutions[_sourceBandIndex] },
                                                                                     _source.Raster.Mapper));
            }
            else
            {
                base.PrepareResult();
            }
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            if (_sourceBandIndex >= 0)
            {
                if (_result.Raster.Representation == RasterRepresentation.Floating)
                {
                    for (Int32 i = 0; i < _source.Raster.NumberOfRows; i++)
                        for (Int32 j = 0; j < _source.Raster.NumberOfColumns; j++)
                            _result.Raster.SetFloatValue(i, j, 0, ComputeFloat(i, j, _sourceBandIndex));
                }
                else
                {
                    for (Int32 i = 0; i < _source.Raster.NumberOfRows; i++)
                        for (Int32 j = 0; j < _source.Raster.NumberOfColumns; j++)
                            _result.Raster.SetValue(i, j, 0, Compute(i, j, _sourceBandIndex));
                }
            }
            else
            {
                if (_result.Raster.Representation == RasterRepresentation.Floating)
                {
                    for (Int32 k = 0; k < _source.Raster.NumberOfBands; k++)
                        for (Int32 i = 0; i < _source.Raster.NumberOfRows; i++)
                            for (Int32 j = 0; j < _source.Raster.NumberOfColumns; j++)
                                _result.Raster.SetFloatValue(i, j, k, ComputeFloat(i, j, k));
                }
                else
                {
                    for (Int32 k = 0; k < _source.Raster.NumberOfBands; k++)
                        for (Int32 i = 0; i < _source.Raster.NumberOfRows; i++)
                            for (Int32 j = 0; j < _source.Raster.NumberOfColumns; j++)
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
            UInt32[] values = new UInt32[_source.Raster.NumberOfBands];
            for (Int32 k = 0; k < _source.Raster.NumberOfBands; k++)
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
            Double[] values = new Double[_source.Raster.NumberOfBands];
            for (Int32 k = 0; k < _source.Raster.NumberOfBands; k++)
                values[k] = ComputeFloat(rowIndex, columnIndex, k);

            return values;
        }

        #endregion
    }
}
