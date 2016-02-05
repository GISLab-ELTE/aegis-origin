/// <copyright file="SpectralSegmentation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Algorithms.Distances;
using ELTE.AEGIS.Collections.Segmentation;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing segmentation os spectral geometries.
    /// </summary>
    public abstract class SpectralSegmentation : Operation<ISpectralGeometry, SegmentCollection>
    {
        #region Protected fields

        /// <summary>
        /// The spectral distance algorithm.
        /// </summary>
        protected readonly SpectralDistance _distance;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralSegmentation" /> class.
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
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// or
        /// The source geometry does not contain raster data.
        /// or
        /// The raster format of the source is not supported by the method.
        /// </exception>
        protected SpectralSegmentation(ISpectralGeometry source, SegmentCollection target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (source.Raster == null)
                throw new ArgumentException("The source geometry does not contain raster data.", "source");
            if (!method.SupportedFormats.Contains(source.Raster.Format))
                throw new ArgumentException("The raster format of the source is not supported by the method.", "source");

            if (IsProvidedParameter(SpectralOperationParameters.SpectralDistanceAlgorithm))
            {
                _distance = ResolveParameter<SpectralDistance>(SpectralOperationParameters.SpectralDistanceAlgorithm);
            }
            else if (IsProvidedParameter(SpectralOperationParameters.SpectralDistanceType))
            {
                _distance = (SpectralDistance)Activator.CreateInstance(ResolveParameter<Type>(SpectralOperationParameters.SpectralDistanceType));
            }
            else
            {
                _distance = new EuclideanDistance();
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            if (_result == null)
                _result = new SegmentCollection(_source.Raster, SpectralStatistics.All);
        }
      
        #endregion
    }
}
