/// <copyright file="PerBandSpectralTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

        /// <summary>
        /// The band indices of the source which should be processed.
        /// </summary>
        protected Int32[] SourceBandIndices { get; private set; }

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
                SourceBandIndices = new Int32[] { Convert.ToInt32(ResolveParameter(SpectralOperationParameters.BandIndex)) };

                if (SourceBandIndices[0] < 0 || SourceBandIndices[0] >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("parameters", "A parameter value does not satisfy the conditions of the parameter.", new ArgumentOutOfRangeException("BandIndex", "BandIndex is not within the range 0.." + (Source.Raster.NumberOfBands - 1) + "."));

            }
            else if (IsProvidedParameter(SpectralOperationParameters.BandIndices))
            {
                SourceBandIndices = ResolveParameter<IEnumerable<Int32>>(SpectralOperationParameters.BandIndices).ToArray();

                if (SourceBandIndices.Any(index => index < 0 || index >= Source.Raster.NumberOfBands))
                    throw new ArgumentException("parameters", "A parameter value does not satisfy the conditions of the parameter.", new ArgumentOutOfRangeException("BandIndices", "One or more values within BandIndices is not within the range 0.." + (Source.Raster.NumberOfBands - 1) + "."));
            }
            else if (IsProvidedParameter(SpectralOperationParameters.BandName) && Source.Imaging != null)
            {
                SpectralDomain domain;
                if (Enum.TryParse<SpectralDomain>(ResolveParameter<String>(SpectralOperationParameters.BandName), out domain) && source.Imaging.Bands.Any(band => band.SpectralDomain == domain))
                    SourceBandIndices = new Int32[] { source.Imaging.Bands.FindIndex(band => band.SpectralDomain == domain) };
            }
            else if (IsProvidedParameter(SpectralOperationParameters.BandNames) && Source.Imaging != null)
            {
                SpectralDomain domain;
                SourceBandIndices = ResolveParameter<IEnumerable<String>>(SpectralOperationParameters.BandNames).Select(name =>
                {
                    Enum.TryParse<SpectralDomain>(name, true, out domain);
                    return source.Imaging.Bands.FindIndex(band => band.SpectralDomain == domain);
                }).Where(index => index >= 0).ToArray();
            }

            if (SourceBandIndices == null)
                SourceBandIndices = Enumerable.Range(0, Source.Raster.NumberOfBands).ToArray();
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            RasterPresentation presentation = null;
            if (SourceBandIndices.SequenceEqual(Enumerable.Range(0, _source.Raster.NumberOfBands)))
                presentation = _source.Presentation;
            else if (SourceBandIndices.Length == 3)
                presentation = RasterPresentation.CreateTrueColorPresentation();
            else
                presentation = RasterPresentation.CreateGrayscalePresentation();

            _result = _source.Factory.CreateSpectralGeometry(_source,
                                                             PrepareRasterResult(_source.Raster.Format,
                                                                                 SourceBandIndices.Length,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 SourceBandIndices.Select(index => _source.Raster.RadiometricResolutions[index]).ToArray(),
                                                                                 _source.Raster.Mapper),
                                                             presentation,
                                                             _source.Imaging);
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override sealed void ComputeResult()
        {
            if (_result.Raster.Format == RasterFormat.Floating)
            {
                for (Int32 bandIndex = 0; bandIndex < SourceBandIndices.Length; bandIndex++)
                    for (Int32 rowIndex = 0; rowIndex < _result.Raster.NumberOfRows; rowIndex++)
                        for (Int32 columnIndex = 0; columnIndex < _result.Raster.NumberOfColumns; columnIndex++)
                            _result.Raster.SetFloatValue(rowIndex, columnIndex, bandIndex, ComputeFloat(rowIndex, columnIndex, SourceBandIndices[bandIndex]));
            }
            else
            {
                for (Int32 bandIndex = 0; bandIndex < SourceBandIndices.Length; bandIndex++)
                    for (Int32 rowIndex = 0; rowIndex < _result.Raster.NumberOfRows; rowIndex++)
                        for (Int32 columnIndex = 0; columnIndex < _result.Raster.NumberOfColumns; columnIndex++)
                            _result.Raster.SetValue(rowIndex, columnIndex, bandIndex, Compute(rowIndex, columnIndex, SourceBandIndices[bandIndex]));
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
            for (Int32 bandIndex = 0; bandIndex < _result.Raster.NumberOfBands; bandIndex++)
                values[bandIndex] = Compute(rowIndex, columnIndex, bandIndex);

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
            for (Int32 bandIndex = 0; bandIndex < _result.Raster.NumberOfBands; bandIndex++)
                values[bandIndex] = ComputeFloat(rowIndex, columnIndex, bandIndex);

            return values;
        }

        #endregion
    }
}
