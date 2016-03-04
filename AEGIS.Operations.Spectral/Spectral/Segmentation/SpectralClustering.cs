/// <copyright file="SpectralClustering.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation performing clustering of spectral geometries.
    /// </summary>
    public abstract class SpectralClustering : SpectralSegmentation
    {
        #region Protected fields

        /// <summary>
        /// The cluster distance algorithm.
        /// </summary>
        protected readonly SpectralDistance _clusterDistance;

        /// <summary>
        /// A value inidcating whether the initial segments were provided.
        /// </summary>
        protected Boolean _initialSegmentsProvided;

        #endregion

        #region Private fields

        /// <summary>
        /// The collection of segments.
        /// </summary>
        private SegmentCollection _initialSegments;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralClustering" /> class.
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
        protected SpectralClustering(ISpectralGeometry source, SegmentCollection target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            _initialSegments = ResolveParameter<SegmentCollection>(SpectralOperationParameters.SegmentCollection);

            if (IsProvidedParameter(SpectralOperationParameters.ClusterDistanceAlgorithm))
            {
                _clusterDistance = ResolveParameter<SpectralDistance>(SpectralOperationParameters.ClusterDistanceAlgorithm);
            }
            else if (IsProvidedParameter(SpectralOperationParameters.ClusterDistanceType))
            {
                _clusterDistance = (SpectralDistance)Activator.CreateInstance(ResolveParameter<Type>(SpectralOperationParameters.ClusterDistanceType));
            }
            else
            {
                _clusterDistance = _distance;
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            // if the initial segments are specified, they should be copied to the result
            if (_initialSegments != null)
            {
                _result = new SegmentCollection(_initialSegments);
                _initialSegments = null;
                _initialSegmentsProvided = true;
            }
            else
            {
                _result = new SegmentCollection(Source.Raster, _distance.Statistics);
            }
        }

        #endregion
    }
}
