// <copyright file="SpectralClustering.cs" company="Eötvös Loránd University (ELTE)">
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
        protected readonly SpectralDistance _clusterCenterDistance;

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
        protected SpectralClustering(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, method, parameters)
        {
            if (IsProvidedParameter(SpectralOperationParameters.ClusterCenterDistanceAlgorithm))
            {
                _clusterCenterDistance = ResolveParameter<SpectralDistance>(SpectralOperationParameters.ClusterCenterDistanceAlgorithm);
            }
            else if (IsProvidedParameter(SpectralOperationParameters.ClusterCenterDistanceType))
            {
                _clusterCenterDistance = (SpectralDistance)Activator.CreateInstance(ResolveParameter<Type>(SpectralOperationParameters.ClusterCenterDistanceType));
            }
            else
            {
                _clusterCenterDistance = _distance;
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Prepares the segment collection.
        /// </summary>
        /// <param name="source">The source raster.</param>
        /// <returns>The resulting segment collection.</returns>
        protected override SegmentCollection PrepareSegmentCollection()
        {
            if (SourceSegments != null)
            {
                return new SegmentCollection(SourceSegments, _distance.Statistics | _clusterCenterDistance.Statistics);
            }
            else
            {
                return new SegmentCollection(Source.Raster, _distance.Statistics | _clusterCenterDistance.Statistics);
            }
        }

        #endregion
    }
}
