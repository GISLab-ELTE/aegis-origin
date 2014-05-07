﻿/// <copyright file="ThresholdClassification.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents a threshold transformation.
    /// </summary>
    public abstract class ThresholdClassification : PerBandSpectralTransformation
    {
        #region protected fields

        protected Double[] _lowerThresholdValues;
        protected Double[] _upperThresholdValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThresholdClassification" /> class.
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
        /// The type of a parameter value does not match the type specified by the method.
        /// </exception>
        protected ThresholdClassification(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
        }

        #endregion

        #region Protected RasterTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (_source.Raster.Representation == RasterRepresentation.Floating)
            {
                Double value = (_source.Raster as IFloatRaster).GetValue(rowIndex, columnIndex, bandIndex);
                return (value >= _lowerThresholdValues[bandIndex] && value <= _upperThresholdValues[bandIndex]) ? Byte.MaxValue : Byte.MinValue;
            }
            else
            {
                UInt32 value = _source.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                return (value >= _lowerThresholdValues[bandIndex] && value <= _upperThresholdValues[bandIndex]) ? Byte.MaxValue : Byte.MinValue;
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            // the result will have integer representation in all cases
            if (_sourceBandIndex >= 0)
            {
                _result = _source.Factory.CreateSpectralGeometry(PrepareRasterResult(1,
                                                                                     _source.Raster.NumberOfRows,
                                                                                     _source.Raster.NumberOfColumns,
                                                                                     new Int32[] { 8 },
                                                                                     new SpectralRange[] { _source.Raster.SpectralRanges[_sourceBandIndex] },
                                                                                     _source.Raster.Mapper,
                                                                                     RasterRepresentation.Integer),
                                                                 _source);
            }
            else
            {
                _result = _source.Factory.CreateSpectralGeometry(PrepareRasterResult(_source.Raster.SpectralResolution,
                                                                                     _source.Raster.NumberOfRows,
                                                                                     _source.Raster.NumberOfColumns,
                                                                                     Enumerable.Repeat(8, _source.Raster.SpectralResolution).ToArray(),
                                                                                     _source.Raster.SpectralRanges,
                                                                                     _source.Raster.Mapper,
                                                                                     RasterRepresentation.Integer),
                                                                 _source);
            }
        }

        #endregion
    }
}
