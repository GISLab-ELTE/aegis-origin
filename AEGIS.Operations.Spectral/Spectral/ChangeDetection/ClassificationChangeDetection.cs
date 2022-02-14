/// <copyright file="ClassificationChangeDetection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Tamas Nagy</author>

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Algorithms;

namespace ELTE.AEGIS.Operations.Spectral.ChangeDetection
{
    /// <summary>
    /// Represents a type for performing change detection with respect to image classification.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class ClassificationChangeDetection<TResult> : Operation<ISpectralGeometry, TResult>
    {
        #region Protected properties 
            
        /// <summary>
        /// Gets the reference geometry.
        /// </summary>
        /// <value>
        /// The reference geometry.
        /// </value>
        protected ISpectralGeometry Reference { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the the source and reference rasters match.
        /// </summary>
        /// <value>
        /// <c>true</c> if the source and the reference rasters have the same dimensions and are mapped to the same extent; otherwise, <c>false</c>.
        /// </value>
        protected Boolean AreMatchingRasters { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassificationChangeDetection{TResult}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="operationMethod">The method.</param>
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
        protected ClassificationChangeDetection(ISpectralGeometry source, OperationMethod operationMethod, IDictionary<OperationParameter, Object> parameters)
            : base(source, default(TResult), operationMethod, parameters)
        {
            ISpectralGeometry reference = ResolveParameter<ISpectralGeometry>(SpectralOperationParameters.ClassificationReferenceGeometry);

            if (source.Raster == null)
                throw new ArgumentException("The source geometry does not contain a raster image.", nameof(source));
            if (source.Raster.NumberOfBands != 1)
                throw new ArgumentException("The source geometry does not contain a single band.", nameof(source));
            if (reference.Raster == null)
                throw new ArgumentException("The reference geometry does not contain a raster image.", nameof(parameters));
            if (reference.Raster.NumberOfBands != 1)
                throw new ArgumentException("The reference geometry does not contain a single band.", nameof(parameters));

            AreMatchingRasters = RasterAlgorithms.IsMatching(source.Raster, reference.Raster);
            Reference = reference;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                {
                    Int32 sourceRowIndex, sourceColumnIndex;
                    if (AreMatchingRasters)
                    {
                        sourceRowIndex = rowIndex;
                        sourceColumnIndex = columnIndex;
                    }
                    else
                    {
                        Coordinate coordinate = Reference.Raster.Mapper.MapCoordinate(rowIndex, columnIndex);
                        Source.Raster.Mapper.MapRaster(coordinate, out sourceRowIndex, out sourceColumnIndex);

                        if (sourceRowIndex < 0 || sourceColumnIndex < 0  || sourceRowIndex >= Source.Raster.NumberOfRows || sourceColumnIndex >= Source.Raster.NumberOfColumns)
                            continue;
                    }

                    UInt32 value = Source.Raster.GetValue(sourceRowIndex, sourceColumnIndex, 0);
                    UInt32 referenceValue = Reference.Raster.GetValue(rowIndex, columnIndex, 0);

                    ComputeChange(sourceRowIndex, sourceColumnIndex, value, referenceValue);
                }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Gets the number of categories the raster contains.
        /// </summary>
        /// <returns>The number of categories the raster contains.</returns>
        protected virtual Int32 GetNumberOfCategories(IRaster raster)
        {
            Int32 numberOfCategories = 0;
            for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                {
                    if (raster.GetValue(rowIndex, columnIndex, 0) > numberOfCategories)
                        numberOfCategories = (Int32) raster.GetValue(rowIndex, columnIndex, 0);
                }

            return numberOfCategories + 1;
        }

        /// <summary>
        /// Computes the change for the specified values at the specified location.
        /// </summary>
        protected abstract void ComputeChange(Int32 rowIndex, Int32 columnIndex, UInt32 value, UInt32 referenceValue);

        #endregion
    }
}





