/// <copyright file="ClassificationAccuracyValidation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents an operation performing the validation of classification based on a reference geometry that results in the percentage of correctly classified pixels.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253921", "Classification validation by accuracy")]
    public class ClassificationAccuracyValidation : Operation<ISpectralGeometry, Double>
    {
        #region Private fields

        /// <summary>
        /// The validation geometry.
        /// </summary>
        private ISpectralGeometry _validationGeometry;

        /// <summary>
        /// The accuracy.
        /// </summary>
        private Double _accuracy;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationAccuracyValidation"/> class.
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
        public ClassificationAccuracyValidation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, 0, SpectralOperationMethods.ClassificationAccuracyValidation, parameters)
        {
            _validationGeometry = ResolveParameter<ISpectralGeometry>(SpectralOperationParameters.ClassificationValidationGeometry);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            if (RasterAlgorithms.IsMatching(Source.Raster, _validationGeometry.Raster))
            {
                MatchByIndices();
            }
            else if (_validationGeometry.Raster.IsMapped && Source.Raster.IsMapped)
            {
                MatchByLocation();
            }
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override Double FinalizeResult()
        {
            return _accuracy;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Matches the reference image with different dimensions or location.
        /// </summary>
        private void MatchByLocation()
        {
            Int32 sourceRowIndex, sourceColumnIndex;
            Double numberOfMatchingValues = 0;

            for (Int32 rowIndex = 0; rowIndex < _validationGeometry.Raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < _validationGeometry.Raster.NumberOfColumns; columnIndex++)
                {
                    Coordinate coordinate = _validationGeometry.Raster.Mapper.MapCoordinate(rowIndex, columnIndex);
                    Source.Raster.Mapper.MapRaster(coordinate, out sourceRowIndex, out sourceColumnIndex);

                    if (sourceRowIndex < 0 || sourceRowIndex >= Source.Raster.NumberOfRows || sourceColumnIndex < 0 || sourceColumnIndex >= Source.Raster.NumberOfColumns)
                        continue;

                    for (Int32 bandIndex = 0; bandIndex < _validationGeometry.Raster.NumberOfBands; bandIndex++)
                    {
                        UInt32 referenceValue = _validationGeometry.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                        UInt32 sourceValue = Source.Raster.GetValue(sourceRowIndex, sourceColumnIndex, bandIndex);
                        if (sourceValue == referenceValue)
                            numberOfMatchingValues++;
                    }
                }
            }

            _accuracy = numberOfMatchingValues / (Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns * Source.Raster.NumberOfBands);
        }

        /// <summary>
        /// Matches the reference image with the same image dimension and location.
        /// </summary>
        private void MatchByIndices()
        {
            Double numberOfMatchingValues = 0;

            for (Int32 bandIndex = 0; bandIndex < _validationGeometry.Raster.NumberOfBands; bandIndex++)
            {
                for (Int32 rowIndex = 0; rowIndex < _validationGeometry.Raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnInex = 0; columnInex < _validationGeometry.Raster.NumberOfColumns; columnInex++)
                    {
                        UInt32 referenceValue = _validationGeometry.Raster.GetValue(rowIndex, columnInex, bandIndex);
                        UInt32 sourceValue = Source.Raster.GetValue(rowIndex, columnInex, bandIndex);

                        if (sourceValue == referenceValue)
                            numberOfMatchingValues++;
                    }
                }
            }

            _accuracy = numberOfMatchingValues / (Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns * Source.Raster.NumberOfBands);
        }

        #endregion
    }
}
