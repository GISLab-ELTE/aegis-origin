/// <copyright file="ClassificationConfusionValidation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents an operation performing the validation of classification based on a reference geometry that results in a confusion matrix.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253922", "Classification validation using confusion matrix")]
    public class ClassificationConfusionValidation : Operation<ISpectralGeometry, Matrix>
    {
        #region Private fields
        
        /// <summary>
        /// The validation geometry.
        /// </summary>
        private ISpectralGeometry _validationGeometry;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationConfusionValidation" /> class.
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
        public ClassificationConfusionValidation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, SpectralOperationMethods.ClassificationConfusionValidation, parameters)
        {
            _validationGeometry = ResolveParameter<ISpectralGeometry>(SpectralOperationParameters.ClassificationValidationGeometry);
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            if (_source.Envelope.Disjoint(_validationGeometry.Envelope))
                return;

            if (RasterAlgorithms.IsMatching(Source.Raster, _validationGeometry.Raster))
            {
                MatchByIndices();
            }
            else if (_validationGeometry.Raster.IsMapped && _source.Raster.IsMapped)
            {
                MatchByLocation();
            }
        }

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            Int32 numberOfCategories = GetNumberOfCategories();
            _result = new Matrix(numberOfCategories, numberOfCategories);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Matches the reference image with different dimensions or location.
        /// </summary>
        private void MatchByLocation()
        {
            Int32 sourceRowIndex, sourceColumnIndex;

            for (Int32 rowIndex = 0; rowIndex < _validationGeometry.Raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < _validationGeometry.Raster.NumberOfColumns; columnIndex++)
                {
                    Coordinate coordinate = _validationGeometry.Raster.Mapper.MapCoordinate(rowIndex, columnIndex);
                    _source.Raster.Mapper.MapRaster(coordinate, out sourceRowIndex, out sourceColumnIndex);

                    if (sourceRowIndex < 0 || sourceRowIndex >= _source.Raster.NumberOfRows || sourceColumnIndex < 0 || sourceColumnIndex >= _source.Raster.NumberOfColumns)
                        continue;

                    for (Int32 bandIndex = 0; bandIndex < _validationGeometry.Raster.NumberOfBands; bandIndex++)
                    {
                        Int32 referenceValue = (Int32)_validationGeometry.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                        Int32 sourceValue = (Int32)_source.Raster.GetValue(sourceRowIndex, sourceColumnIndex, bandIndex);

                        _result[sourceValue, referenceValue]++;
                    }
                }
            }
        }

        /// <summary>
        /// Matches the reference image with the same image dimension and location.
        /// </summary>
        private void MatchByIndices()
        {
            for (Int32 bandIndex = 0; bandIndex < _validationGeometry.Raster.NumberOfBands; bandIndex++)
            {
                for (Int32 rowIndex = 0; rowIndex < _validationGeometry.Raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < _validationGeometry.Raster.NumberOfColumns; columnIndex++)
                    {
                        Int32 referenceValue = (Int32)_validationGeometry.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                        Int32 sourceValue = (Int32)_source.Raster.GetValue(rowIndex, columnIndex, bandIndex);

                        _result[sourceValue, referenceValue]++;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of categories from the reference image.
        /// </summary>
        /// <returns>The number of categories in the reference image.</returns>
        private Int32 GetNumberOfCategories()
        {
            Int32 numberOfCategories = 0;
            for (Int32 bandIndex = 0; bandIndex < _validationGeometry.Raster.NumberOfBands; bandIndex++)
                for (Int32 rowIndex = 0; rowIndex < _validationGeometry.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < _validationGeometry.Raster.NumberOfColumns; columnIndex++)
                    {
                        if (_validationGeometry.Raster.GetValue(rowIndex, columnIndex, bandIndex) > numberOfCategories)
                            numberOfCategories = (Int32)_validationGeometry.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                    }

            return numberOfCategories + 1;
        }

        #endregion
    }
}
