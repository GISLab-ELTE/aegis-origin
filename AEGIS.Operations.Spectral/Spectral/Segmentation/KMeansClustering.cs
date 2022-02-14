/// <copyright file="KMeansClustering.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Greta Bereczki</author>

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing clustering of spectral geometries using the k-means method.
    /// </summary>
    /// <remarks>
    /// K-means clustering aims to partition n observations into k clusters in which each observation belongs to the cluster with the nearest mean, serving as a prototype of the cluster.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::254220", "K-means clustering")]
    public class KMeansClustering : SpectralClustering
    {
        #region Private fields

        /// <summary>
        /// The number of clusters.
        /// </summary>
        private Int32 _numberOfClusters;

        /// <summary>
        /// The cluster centers for each band.
        /// </summary>
        private List<Double[]> _clusterCenters;

        /// <summary>
        /// The array of clusters.
        /// </summary>
        private Segment[] _clusters;

        /// <summary>
        /// The maximum number of iterations.
        /// </summary>
        private Double _maximumIterations;

        /// <summary>
        /// The maximum difference with the cluster mean.
        /// </summary>
        private Double _maximumDifference;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KMeansClustering"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
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
        public KMeansClustering(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KMeansClustering"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
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
        public KMeansClustering(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.KMeansClustering, parameters)
        {
            _numberOfClusters = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.NumberOfClusters));
            _maximumIterations = Convert.ToInt32(ResolveParameter(CommonOperationParameters.NumberOfIterations));

            if (_numberOfClusters == 0)
                _numberOfClusters = (Int32)(Math.Sqrt(Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns) * Source.Raster.RadiometricResolution / 8);

            if (_maximumIterations == 0)
                _maximumIterations = _numberOfClusters;

            _maximumDifference = RasterAlgorithms.RadiometricResolutionMax(Source.Raster) / Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            InitializeClusterCenters();
            
            Int32 iterationIndex = 0;

            while (iterationIndex < _maximumIterations)
            {
                if (Source.Raster is ISegmentedRaster)
                    MergeSegmentsToClusters();
                else
                    MergeValuesToClusters();

                iterationIndex++;
                if (iterationIndex == _maximumIterations)
                    break;

                if (AreClustersCentersValid())
                    break;

                // create new clusters with new centers
                for (Int32 clusterIndex = 0; clusterIndex < _clusters.Length; clusterIndex++)
                {
                    _clusterCenters[clusterIndex] = _clusters[clusterIndex].Mean.ToArray();
                    _clusters[clusterIndex] = null;
                }

                ResultSegments.Clear();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the cluster centers.
        /// </summary>
        private void InitializeClusterCenters()
        {
            Random randomGenerator = new Random();
            Int32 centroidRow, centroidColumn;
            _clusterCenters = new List<Double[]>();

            for (Int32 clusterIndex = 0; clusterIndex < _numberOfClusters; clusterIndex++)
            {
                centroidRow = randomGenerator.Next(0, Source.Raster.NumberOfRows);
                centroidColumn = randomGenerator.Next(0, Source.Raster.NumberOfColumns);

                Double[] values = Source.Raster.GetFloatValues(centroidRow, centroidColumn);

                // find a cluster center different from the former ones
                Boolean hasMoreValues = true;
                for (Int32 i = 0; i < 1000; i++)
                {
                    if (!_clusterCenters.Any(center => center.SequenceEqual(values)))
                        break;

                    centroidRow = randomGenerator.Next(0, Source.Raster.NumberOfRows);
                    centroidColumn = randomGenerator.Next(0, Source.Raster.NumberOfColumns);
                    hasMoreValues = false;

                    values = Source.Raster.GetFloatValues(centroidRow, centroidColumn);
                }

                // probably reached the maximum number of different values
                if (!hasMoreValues)
                    _numberOfClusters = clusterIndex;

                _clusterCenters.Add(new Double[Source.Raster.NumberOfBands]);
                _clusterCenters[clusterIndex] = values;
            }

            _clusters = new Segment[_numberOfClusters];
        }

        /// <summary>
        /// Merges segments to clusters.
        /// </summary>
        private void MergeSegmentsToClusters()
        {
            Int32 minimalIndex = 0;

            List<Segment> segments = ResultSegments.GetSegments().ToList();
            foreach (Segment segment in segments)
            {
                if (!ResultSegments.Contains(segment))
                    continue;

                minimalIndex = _clusterCenters.MinIndex(center => _distance.Distance(segment, center));

                if (_clusters[minimalIndex] == null)
                    _clusters[minimalIndex] = segment;
                else
                    _clusters[minimalIndex] = ResultSegments.MergeSegments(_clusters[minimalIndex], segment);
            }
        }

        /// <summary>
        /// Merges spectral values to clusters.
        /// </summary>
        private void MergeValuesToClusters()
        {
            Int32 minimalIndex = 0;
            for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                {
                    switch (Source.Raster.Format)
                    {
                        case RasterFormat.Integer:
                            UInt32[] values = Source.Raster.GetValues(rowIndex, columnIndex);
                            minimalIndex = _clusterCenters.MinIndex(center => _distance.Distance(center, values));
                            break;
                        case RasterFormat.Floating:
                            Double[] floatValues = Source.Raster.GetFloatValues(rowIndex, columnIndex);
                            minimalIndex = _clusterCenters.MinIndex(center => _distance.Distance(center, floatValues));
                            break;
                    }

                    if (_clusters[minimalIndex] == null)
                        _clusters[minimalIndex] = ResultSegments.GetSegment(rowIndex, columnIndex);
                    else
                        ResultSegments.MergeSegments(_clusters[minimalIndex], rowIndex, columnIndex);
                }
            }
        }

        /// <summary>
        /// Determines whether the clusters centers are valid.
        /// </summary>
        /// <returns><c>true</c> if the euclidean difference between the cluster centers and cluster means is smaller than the maximum allowed; otherwise <c>false</c>.</returns>
        private Boolean AreClustersCentersValid()
        {
            Double difference = 0;
            for (Int32 clusterIndex = 0; clusterIndex < _clusters.Length; clusterIndex++)
            {
                if (_clusters[clusterIndex] == null)
                    continue;

                for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                {
                    difference += Calculator.Square(_clusterCenters[clusterIndex][bandIndex] - _clusters[clusterIndex].Mean[bandIndex]);
                }
            }

            return Math.Sqrt(difference) <= _maximumDifference;
        }

        #endregion
    }
}
