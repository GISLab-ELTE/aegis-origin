/// <copyright file="ClassificationMapClassification.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents an operation performing the validation of classification based on a reference geometry that results in an image map of correctly classified pixels.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253925", "Classification validation using image map")]
    public class ClassificationMapValidation : PerBandSpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The validation geometry.
        /// </summary>
        private ISpectralGeometry _validationGeometry;

        /// <summary>
        /// A value indicating whether the validation geometry matches the source image.
        /// </summary>
        private Boolean _isValidationMatching;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationMapValidation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public ClassificationMapValidation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.ClassificationMapValidation, parameters)
        {
            _validationGeometry = ResolveParameter<ISpectralGeometry>(SpectralOperationParameters.ClassificationValidationGeometry);
            _isValidationMatching = RasterAlgorithms.IsMatching(Source.Raster, _validationGeometry.Raster);
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
            if (_source.Envelope.Disjoint(_validationGeometry.Envelope))
                return 0;

            Int32 sourceRowIndex, sourceColumnIndex;

            // match the source coordinates
            if (_isValidationMatching)
            {
                sourceColumnIndex = columnIndex;
                sourceRowIndex = rowIndex;
            }
            else
            {
                Coordinate coordinate = _validationGeometry.Raster.Mapper.MapCoordinate(rowIndex, columnIndex);
                _source.Raster.Mapper.MapRaster(coordinate, out sourceRowIndex, out sourceColumnIndex);

                if (sourceRowIndex < 0 || sourceRowIndex >= _source.Raster.NumberOfRows || sourceColumnIndex < 0 || sourceColumnIndex >= _source.Raster.NumberOfColumns)
                    return 0;
            }

            if (_validationGeometry.Raster.GetValue(rowIndex, columnIndex, bandIndex) == _source.Raster.GetValue(sourceRowIndex, sourceColumnIndex, bandIndex))
                return 255;
            else
                return 0;
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>
        /// The spectral value at the specified index.
        /// </returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return (Double)Compute(rowIndex, columnIndex, bandIndex);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _result = _source.Factory.CreateSpectralGeometry(_validationGeometry,
                                                             PrepareRasterResult(RasterFormat.Integer,
                                                                                 1,
                                                                                 _validationGeometry.Raster.NumberOfRows,
                                                                                 _validationGeometry.Raster.NumberOfColumns,
                                                                                 8,
                                                                                 _validationGeometry.Raster.Mapper),
                                                             RasterPresentation.CreateGrayscalePresentation(), 
                                                             null);
        }

        #endregion
    }
}
