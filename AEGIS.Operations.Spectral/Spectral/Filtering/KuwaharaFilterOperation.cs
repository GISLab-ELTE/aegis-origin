// <copyright file="KuwaharaFilterOperation.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Algorithms;

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a raster image filter using Kuwahara's algorithm.
    /// </summary>
    /// <remarks>
    /// The Kuwahara filter is a non-linear smoothing filter used in image processing for adaptive noise reduction.
    /// <author>Gábor Balázs Butkay</author>
    /// </remarks>
    [OperationMethodImplementation("AEGIS::251168", "Kuwahara filter")]
    public class KuwaharaFilterOperation : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The radius of the filter.
        /// </summary>
        private Int32 _filterRadius;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of Kuwahara filter transformation.
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
        public KuwaharaFilterOperation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initialize a new instance of Kuwahara filter transformation.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
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
        public KuwaharaFilterOperation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.KuwaharaFilter, parameters)
        {
            _filterRadius = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.FilterRadius));
        }

        #endregion

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
            return RasterAlgorithms.Restrict(ComputeFloat(rowIndex, columnIndex, bandIndex), Source.Raster.RadiometricResolution);
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
            List<Double> topLeftElements = GetTopLeftElements(rowIndex, columnIndex, bandIndex);
            List<Double> topRightElements = GetTopRightElements(rowIndex, columnIndex, bandIndex);
            List<Double> bottomLeftElements = GetBottomLeftElements(rowIndex, columnIndex, bandIndex);
            List<Double> bottomRightElements = GetBottomRightElements(rowIndex, columnIndex, bandIndex);

            Double[] standardDeviations = new Double[4];
            standardDeviations[0] = StandardDeviation(topLeftElements);
            standardDeviations[1] = StandardDeviation(topRightElements);
            standardDeviations[2] = StandardDeviation(bottomLeftElements);
            standardDeviations[3] = StandardDeviation(bottomRightElements);

            Int32 min = 0;
            for (Int32 i = 1; i < standardDeviations.Length; i++)
                if (standardDeviations[i] < standardDeviations[min])
                    min = i;

            switch (min)
            {
                case 0:
                    return topLeftElements.Average();
                case 1:
                    return topRightElements.Average();
                case 2:
                    return bottomLeftElements.Average();
                default:
                    return bottomRightElements.Average();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the standard deviation of a concrete area.
        /// </summary>
        /// <param name="values">The population of the area.</param>
        /// <returns>The standard deviation of the area.</returns>
        private Double StandardDeviation(List<Double> values)
        {
            Double mean = values.Average();
            List<Double> deviationsFromMean = new List<Double>(values.Count);

            foreach (Double elem in values)
                deviationsFromMean.Add(Math.Pow(elem - mean, 2));

            Double variance = deviationsFromMean.Average();
            return Math.Sqrt(variance);
        }

        /// <summary>
        /// Returns with the list of the elements of the top left area.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The list of the elements of the top left area.</returns>
        private List<Double> GetTopLeftElements(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            List<Double> result = new List<Double>();

            for (Int32 i = Math.Max(rowIndex - _filterRadius, 0); i <= rowIndex; i++)
                for (Int32 j = Math.Max(columnIndex - _filterRadius, 0); j <= columnIndex; j++)
                    result.Add(Source.Raster.GetFloatValue(i, j, bandIndex));

            return result;
        }

        /// <summary>
        /// Returns with the list of the elements of the top right area.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The list of the elements of the top right area.</returns>
        private List<Double> GetTopRightElements(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            List<Double> result = new List<Double>();

            for (Int32 i = Math.Max(rowIndex - _filterRadius, 0); i <= rowIndex; i++)
                for (Int32 j = columnIndex; j <= Math.Min(columnIndex + _filterRadius, Source.Raster.NumberOfColumns - 1); j++)
                    result.Add(Source.Raster.GetFloatValue(i, j, bandIndex));

            return result;
        }

        /// <summary>
        /// Returns with the list of the elements of the bottom left area.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The list of the elements of the bottom left area.</returns>
        private List<Double> GetBottomLeftElements(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            List<Double> result = new List<Double>();

            for (Int32 i = rowIndex; i <= Math.Min(rowIndex + _filterRadius, Source.Raster.NumberOfRows - 1); i++)
                for (Int32 j = Math.Max(columnIndex - _filterRadius, 0); j <= columnIndex; j++)
                    result.Add(Source.Raster.GetFloatValue(i, j, bandIndex));

            return result;
        }

        /// <summary>
        /// Returns with the list of the elements of the bottom right area.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The list of the elements of the bottom right area.</returns>
        private List<Double> GetBottomRightElements(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            List<Double> result = new List<Double>();

            for (Int32 i = rowIndex; i <= Math.Min(rowIndex + _filterRadius, Source.Raster.NumberOfRows - 1); i++)
                for (Int32 j = columnIndex; j <= Math.Min(columnIndex + _filterRadius, Source.Raster.NumberOfColumns - 1); j++)
                    result.Add(Source.Raster.GetFloatValue(i, j, bandIndex));

            return result;
        }

        #endregion
    }
}
