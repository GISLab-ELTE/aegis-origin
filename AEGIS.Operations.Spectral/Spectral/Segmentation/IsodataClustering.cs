/// <copyright file="IsodataClustering.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Numerics.Statistics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing clustering of spectral geometries using the ISODATA method.
    /// </summary>
    [OperationMethodImplementation("AEGIS::254210", "ISODATA clustering")]
    public class IsodataClustering : SpectralClustering
    {
        #region Private fields

        /// <summary>
        /// The upper threshold for cluster distance.
        /// </summary>
        private Double _clusterDistanceThreshold;

        /// <summary>
        /// The lower threshold for cluster size.
        /// </summary>
        private Int32 _clusterSizeThreshold;

        /// <summary>
        /// The number of initial clusters.
        /// </summary>
        private Int32 _numberOfClusters;

        /// <summary>
        /// The initial cluster centers.
        /// </summary>
        private Double[][] _clusterCenters;
    
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IsodataClustering"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public IsodataClustering(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsodataClustering"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
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
        public IsodataClustering(ISpectralGeometry source, SegmentCollection target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.IsodataClustering, parameters)  
        {
            _numberOfClusters = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.NumberOfClusters));
            _clusterDistanceThreshold = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.ClusterDistanceThreshold));
            _clusterSizeThreshold = Convert.ToInt32(ResolveParameter(SpectralOperationParameters.ClusterSizeThreshold));

            if (_numberOfClusters < 10)
                _numberOfClusters = Math.Min(Math.Max(10, Convert.ToInt32(Math.Sqrt(Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns))), Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns);
        }

        #endregion

        #region Protected Operation methods
        
        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            CreateInitialClusters();
            
            if (_initialSegmentsProvided)
                MergeSegmentsToClusters();
            else
                MergeValuesToClusters();
            
            EliminateSmallClusters();

            MergeClusters();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the initial clusters.
        /// </summary>
        private void CreateInitialClusters()
        {            
            // compute median and std. deviation
            Double[] mean = new Double[Source.Raster.NumberOfBands];
            Double[] standardDeviation = new Double[Source.Raster.NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
            {
                mean[bandIndex] = 0;
                standardDeviation[bandIndex] = 0;

                for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                        mean[bandIndex] += Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);

                mean[bandIndex] /= (Source.Raster.NumberOfColumns * Source.Raster.NumberOfRows);

                for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
                    for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                        standardDeviation[bandIndex] += Math.Pow(Source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) - mean[bandIndex], 2);

                standardDeviation[bandIndex] = Math.Sqrt(standardDeviation[bandIndex] / (Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns - 1));
            }

            // generate the initial clusters
            _clusterCenters = new Double[_numberOfClusters][];
            GaussianRandomGenerator randomGenerator = new GaussianRandomGenerator();

            for (Int32 clusterIndex = 0; clusterIndex < _numberOfClusters; clusterIndex++)
            {
                Double[] randomNumbers = new Double[Source.Raster.NumberOfBands];

                for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                    randomNumbers[bandIndex] = randomGenerator.NextDouble(mean[bandIndex], standardDeviation[bandIndex]);

                _clusterCenters[clusterIndex] = randomNumbers;
            }
        }

        /// <summary>
        /// Merges spectral values to clusters.
        /// </summary>
        private void MergeValuesToClusters()
        {
            Segment[] clusters = new Segment[_numberOfClusters];
            
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

                    if (clusters[minimalIndex] == null)
                        clusters[minimalIndex] = Result.GetSegment(rowIndex, columnIndex);
                    else
                        Result.MergeSegments(clusters[minimalIndex], rowIndex, columnIndex);
                }
            }
        }

        /// <summary>
        /// Merges segments to clusters.
        /// </summary>
        private void MergeSegmentsToClusters()
        {
            Segment[] clusters = new Segment[_numberOfClusters];
            
            Int32 minimalIndex = 0;

            Segment[] segments = Result.GetSegments().ToArray();

            foreach (Segment segment in segments)
            {
                if (!Result.Contains(segment))
                    continue;

                minimalIndex = _clusterCenters.MinIndex(center => _distance.Distance(segment, center));
                
                if (clusters[minimalIndex] == null)
                    clusters[minimalIndex] = segment;
                else
                    Result.MergeSegments(clusters[minimalIndex], segment);
            }
        }

        /// <summary>
        /// Eliminates small clusters.
        /// </summary>
        private void EliminateSmallClusters()
        {
            _clusterCenters = null;

            Segment[] segments = Result.GetSegments().ToArray();

            foreach (Segment segment in segments)
            {
                if (segment.Count < _clusterSizeThreshold)
                    Result.SplitSegment(segment);
            }
        }

        /// <summary>
        /// Merges the clusters.
        /// </summary>
        private void MergeClusters()
        {
            Boolean clusterMerged = true;

            do
            {
                clusterMerged = false;

                List<Segment> segments = Result.GetSegments().ToList();

                for (Int32 firstIndex = 0; firstIndex < segments.Count - 1; firstIndex++)
                {
                    for (Int32 secondIndex = segments.Count - 1; secondIndex > firstIndex; secondIndex--)
                    {
                        Double distance = _clusterDistance.Distance(segments[firstIndex], segments[secondIndex]);

                        if (distance < _clusterDistanceThreshold)
                        {
                            Segment mergedSegment = Result.MergeSegments(segments[firstIndex], segments[secondIndex]);
                            segments.RemoveAt(mergedSegment == segments[firstIndex] ? secondIndex : firstIndex);
                            clusterMerged = true;
                        }
                    }
                }
            } while (clusterMerged);
        }
 
        #endregion
    }
}
