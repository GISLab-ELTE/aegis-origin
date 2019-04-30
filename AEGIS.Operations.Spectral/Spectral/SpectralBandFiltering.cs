/// <copyright file="SpectralBandFiltering.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
    /// Represents an operation which filters the specified band indices from a raster image.
    /// </summary>
    public class SpectralBandFiltering : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The array of band indices.
        /// </summary>
        private Int32[] _bandIndices;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralBandFiltering" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The target is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public SpectralBandFiltering(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralBandFiltering" /> class.
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
        public SpectralBandFiltering(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.SpectralBandFiltering, parameters)
        {
            Int32[] bandsFromIndices = null, bandsFromNames = null;

            if (IsProvidedParameter(SpectralOperationParameters.BandIndex))
            {
                bandsFromIndices = new Int32[] { Convert.ToInt32(ResolveParameter(SpectralOperationParameters.BandIndex)) };

                if (bandsFromIndices[0] < 0 || bandsFromIndices[0] >= Source.Raster.NumberOfBands)
                    throw new ArgumentException("A parameter value does not satisfy the conditions of the parameter.", nameof(parameters), new ArgumentOutOfRangeException("BandIndices", "BandIndex is not within the range 0.." + (Source.Raster.NumberOfBands - 1) + "."));

            }
            else if (IsProvidedParameter(SpectralOperationParameters.BandIndices))
            {
                bandsFromIndices = ResolveParameter<IEnumerable<Int32>>(SpectralOperationParameters.BandIndices).ToArray();

                if (bandsFromIndices.Length == 0)
                    throw new ArgumentException("A parameter value does not satisfy the conditions of the parameter.", nameof(parameters), new ArgumentOutOfRangeException("BandIndices", "BandIndices has 0 values."));
                if (bandsFromIndices.Any(index => index < 0 || index >= Source.Raster.NumberOfBands))
                    throw new ArgumentException("A parameter value does not satisfy the conditions of the parameter.", nameof(parameters), new ArgumentOutOfRangeException("BandIndices", "One or more values within BandIndices is not within the range 0.." + (Source.Raster.NumberOfBands - 1) + "."));
            }

            if (IsProvidedParameter(SpectralOperationParameters.BandName) && Source.Imaging != null)
            {
                SpectralDomain domain;
                if (Enum.TryParse<SpectralDomain>(ResolveParameter<String>(SpectralOperationParameters.BandName), out domain) && source.Imaging.Bands.Any(band => band.SpectralDomain == domain))
                {
                    bandsFromNames = new Int32[] { source.Imaging.Bands.IndexOf(band => band.SpectralDomain == domain) };
                }
                if (bandsFromNames.Length == 0)
                    throw new ArgumentException("A parameter value does not satisfy the conditions of the parameter.", nameof(parameters), new ArgumentOutOfRangeException("BandName", "No matching band names found."));
            }
            else if (IsProvidedParameter(SpectralOperationParameters.BandNames) && Source.Imaging != null)
            {
                SpectralDomain domain;
                bandsFromNames = ResolveParameter<IEnumerable<String>>(SpectralOperationParameters.BandNames).Select(name =>
                {
                    Enum.TryParse<SpectralDomain>(name, true, out domain);
                    return source.Imaging.Bands.IndexOf(band => band.SpectralDomain == domain);
                }).Where(index => index >= 0).ToArray();

                if (bandsFromNames.Length == 0)
                    throw new ArgumentException("A parameter value does not satisfy the conditions of the parameter.", nameof(parameters), new ArgumentOutOfRangeException("BandNames", "No matching band names found."));
            }

            if (bandsFromIndices != null && bandsFromNames != null)
            {
                _bandIndices = bandsFromIndices.Union(bandsFromNames).OrderBy(bandIndex => bandIndex).Distinct().ToArray();
            }
            else if (bandsFromIndices != null)
            {
                _bandIndices = bandsFromIndices;
            }
            else if (bandsFromNames != null)
            {
                _bandIndices = bandsFromNames;
            }
            else
            {
                _bandIndices = Enumerable.Range(0, Source.Raster.NumberOfBands).ToArray();
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            RasterPresentation presentation = null;
            if (_bandIndices.SequenceEqual(Enumerable.Range(0, Source.Raster.NumberOfBands)))
                presentation = Source.Presentation;
            else if (_bandIndices.Length == 3)
                presentation = RasterPresentation.CreateFalseColorPresentation(0, 1, 2);
            else
                presentation = RasterPresentation.CreateGrayscalePresentation();

            RasterImaging imaging = null;
            if (Source.Imaging != null)
            {
                imaging = RasterImaging.Filter(Source.Imaging, _bandIndices);
            }

            SetResultProperties(Source.Raster.Format, _bandIndices.Length, Source.Raster.NumberOfRows, Source.Raster.NumberOfColumns, Source.Raster.RadiometricResolution, Source.Raster.Mapper, presentation, imaging);

            return base.PrepareResult();
        }

        #endregion;

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return Source.Raster.GetValue(rowIndex, columnIndex, _bandIndices[bandIndex]);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return Source.Raster.GetFloatValue(rowIndex, columnIndex, _bandIndices[bandIndex]);
        }

        #endregion
    }
}
