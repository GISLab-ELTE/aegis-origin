/// <copyright file="KMeansClustering.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Greta Bereczki</author>

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

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
        #region Constants

        /// <summary>
        /// The number of steps.
        /// </summary>
        private const Int32 NumberOfSteps = 10;

        #endregion

        #region Private fields

        /// <summary>
        /// The number of clusters.
        /// </summary>
        private Int32 _numberOfClusters;

        /// <summary>
        /// The centroids of the clusters.
        /// </summary>
        private List<Double[]> _centroids;

        /// <summary>
        /// The clusters.
        /// </summary>
        private Segment[] _clusters;

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
        public KMeansClustering(ISpectralGeometry source, SegmentCollection target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.KMeansClustering, parameters)
        {
            Int32 numberOfClusters = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.NumberOfClusters));
            _numberOfClusters = (numberOfClusters < 1) ? 1 : numberOfClusters;
        }

        #endregion

        #region Protected Operation methods
        
        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            CreateRandomCentroids();

            if (_initialSegmentsProvided)
                MergeSegmentsToClusters();
            else
                MergeValuesToClusters();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generates the random centroids.
        /// </summary>
        private void CreateRandomCentroids()
        {
            Random randomGenerator = new Random();
            Int32 centroidRow, centroidColumn;
            _centroids = new List<Double[]>();
            _clusters = new Segment[_numberOfClusters];

            for (Int32 clusterIndex = 0; clusterIndex < _numberOfClusters; clusterIndex++)
            {
                centroidRow = randomGenerator.Next(0, Source.Raster.NumberOfRows);
                centroidColumn = randomGenerator.Next(0, Source.Raster.NumberOfColumns);

                // find a cluster center different from the former ones
                Boolean hasMoreColors = true;
                for (Int32 i = 0; i < 1000; i++)
                {
                    hasMoreColors = true;
                    if ((ContainsCentroid(clusterIndex, Source.Raster.GetFloatValues(centroidRow, centroidColumn))))
                    {
                        centroidRow = randomGenerator.Next(0, Source.Raster.NumberOfRows);
                        centroidColumn = randomGenerator.Next(0, Source.Raster.NumberOfColumns);
                        hasMoreColors = false;
                    }
                    else
                    {
                        break;
                    }
                }

                if (!hasMoreColors)
                    _numberOfClusters = clusterIndex;

                // set the centroid
                _centroids.Add(new Double[Source.Raster.NumberOfBands]);
                _centroids[clusterIndex] = Source.Raster.GetFloatValues(centroidRow, centroidColumn);
            }

            Array.Resize(ref _clusters, _numberOfClusters);
        }

        /// <summary>
        /// Merges segments to clusters.
        /// </summary>
        private void MergeSegmentsToClusters()
        {
            Boolean isConvergent = false;
            Int32 steps = 0;

            while (!isConvergent && steps < NumberOfSteps)
            {
                Int32 segmentIndex = 0;
                Segment[] segments = Result.GetSegments().ToArray();

                foreach (Segment segment in segments)
                {
                    if (!Result.Contains(segment))
                        continue;

                    // find the cluster with the minimum distance
                    Double minimumDistance = _clusterDistance.Distance(segment, _centroids[0]);
                    segmentIndex = 0;
                    for (Int32 clusterIndex = 1; clusterIndex < _numberOfClusters; clusterIndex++)
                    {
                        Double distance = _clusterDistance.Distance(segment, _centroids[clusterIndex]);
                        if (distance < minimumDistance)
                        {
                            minimumDistance = distance;
                            segmentIndex = clusterIndex;
                        }
                    }

                    if (_clusters[segmentIndex] == null)
                        _clusters[segmentIndex] = segment;
                    else
                        Result.MergeSegments(_clusters[segmentIndex], segment);
                }

                isConvergent = IsConvergent();
                if (!isConvergent)
                {
                    steps++;
                    if (steps == NumberOfSteps)
                        break;

                    // create new clusters with new centroids
                    for (Int32 i = 0; i < _clusters.Length; i++)
                    {
                        _centroids[i] = _clusters[i].Mean.ToArray();
                        _clusters[i] = null;
                    }

                    Result.Clear();
                }
            }
        }

        /// <summary>
        /// Merges spectral values to clusters.
        /// </summary>
        private void MergeValuesToClusters()
        {
            Boolean isConvergent = false;
            Int32 steps = 0;
            while (!isConvergent && steps < NumberOfSteps)
            {
                Int32 row = 0;
                Int32 column = 0;
                Int32 segmentIndex = 0;
                for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                    {
                        // find the cluster with the minimum distance
                        Double minimumDistance = _clusterDistance.Distance(_centroids[0], Source.Raster.GetFloatValues(rowIndex, columnIndex));
                        row = rowIndex;
                        column = columnIndex;
                        segmentIndex = 0;
                        for (Int32 clusterIndex = 1; clusterIndex < _numberOfClusters; clusterIndex++)
                        {
                            Double distance = _clusterDistance.Distance(_centroids[clusterIndex], Source.Raster.GetFloatValues(rowIndex, columnIndex));
                            if (distance < minimumDistance)
                            {
                                minimumDistance = distance;
                                segmentIndex = clusterIndex;
                                row = rowIndex;
                                column = columnIndex;
                            }
                        }

                        if (_clusters[segmentIndex] == null)
                            _clusters[segmentIndex] = Result.GetSegment(row, column);
                        else
                            Result.MergeSegments(_clusters[segmentIndex], row, column);
                    }
                }

                isConvergent = IsConvergent();
                if (!isConvergent)
                {
                    steps++;
                    if (steps == NumberOfSteps)
                        break;

                    // create new clusters with new centroids
                    for (Int32 i = 0; i < _clusters.Length; i++)
                    {
                        _centroids[i] = _clusters[i].Mean.ToArray();
                        _clusters[i] = null;
                    }

                    Result.Clear();
                }
            }
        }

        /// <summary>
        /// Determines whether one of the clusters already has the given centroid.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="centroid">The centroid.</param>
        /// <returns><c>true</c> if one of the clusters already has the same centroid; otherwise <c>false</c>.</returns>
        private Boolean ContainsCentroid(Int32 index, Double[] centroid)
        {
            for (Int32 i = 0; i < index; i++)
                if (AreEqual(_centroids[i], centroid))
                    return true;

            return false;
        }

        /// <summary>
        /// Determines whether the two values in the two arrays are equal.
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns><c>true</c> if the values in the two arrays are equal; otherwise <c>false</c>.</returns>
        private Boolean AreEqual(Double[] first, Double[] second)
        {
            for (Int32 index = 0; index < first.Length; index++)
                if (first[index] != second[index])
                    return false;

            return true;
        }

        /// <summary>
        /// Determines whether new centroid values can be assigned.
        /// </summary>
        /// <returns><c>true</c> if the method converges; otherwise <c>false</c></returns>
        private Boolean IsConvergent()
        {
            Boolean areAllEqual = true;
            for (Int32 i = 0; i < _clusters.Length; i++)
            {
                for (Int32 j = 0; j < Source.Raster.NumberOfBands; j++)
                {
                    if (_clusters[i] == null)
                        return true;

                    if (_centroids[i][j] != _clusters[i].Mean[j])
                        areAllEqual = false;
                }
            }

            return areAllEqual;
        }

        #endregion
    }
}
