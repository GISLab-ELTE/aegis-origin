/// <copyright file="GraphBasedMergeSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing graph-based merge segmentation on spectral geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::254104", "Graph-based merge segmentation")]
    public class GraphBasedMergeSegmentation : SpectralSegmentation
    {
        #region Private types

        /// <summary>
        /// Simple graph edge representation. The source and destination vertexes are just indexes
        /// </summary>
        public class SimpleGraphEdge
        {
            /// <summary>
            /// The index of the source.
            /// </summary>
            public Int32 Source { get; private set; }

            /// <summary>
            /// The index of the destination.
            /// </summary>
            public Int32 Destination { get; private set; }

            /// <summary>
            /// The weight of the edge.
            /// </summary>
            public Single Weight { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SimpleGraphEdge"/> class.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="destination">The destination.</param>
            /// <param name="weight">The weight.</param>
            public SimpleGraphEdge(Int32 source, Int32 destination, Single weight)
            {
                Source = source;
                Destination = destination;
                Weight = weight;
            }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The segment merge threshold.
        /// </summary>
        private Double _mergeThreshold;

        /// <summary>
        /// The dictionary of inner difference values.
        /// </summary>
        private Dictionary<Segment, Single> _innerDiffences; 

        #endregion  

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBasedMergeSegmentation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
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
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public GraphBasedMergeSegmentation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBasedMergeSegmentation" /> class.
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
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public GraphBasedMergeSegmentation(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.GraphBasedMergeSegmentation, parameters)
        {
            _mergeThreshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SegmentMergeThreshold));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            // source: Felzenszwalb, Huttenlocher: Efficient Graph-Based Image Segmentation

            List<SimpleGraphEdge> edges = GenerateEdgeList();
            _innerDiffences = new Dictionary<Segment, Single>();
            
            // remove the edges and merge segments
            while (edges.Count > 0)
            {
                SimpleGraphEdge edge = edges[edges.Count - 1];
                edges.RemoveAt(edges.Count - 1);

                Segment firstSegment = ResultSegments[edge.Source / Source.Raster.NumberOfColumns, edge.Source % Source.Raster.NumberOfColumns];
                Segment secondSegment = ResultSegments[edge.Destination / Source.Raster.NumberOfColumns, edge.Destination % Source.Raster.NumberOfColumns];

                // if the two indices are already within the same segment
                if (firstSegment == secondSegment)
                    continue;

                Double internalDifference = ComputeMaximumInternalDifference(firstSegment, secondSegment);

                // if the weight of the edge does not influence the internal difference
                if (internalDifference > edge.Weight)
                {
                    // the segments should be merged
                    Segment mergedSegment = ResultSegments.MergeSegments(firstSegment, secondSegment);
                    Segment otherSegment = mergedSegment == firstSegment ? firstSegment : secondSegment;

                    // modify internal difference
                    Single weight = edge.Weight;
                    if (_innerDiffences.ContainsKey(otherSegment))
                    {
                        weight = Math.Max(_innerDiffences[otherSegment], edge.Weight);
                        _innerDiffences.Remove(otherSegment);
                    }

                    if (!_innerDiffences.ContainsKey(mergedSegment))
                        _innerDiffences.Add(mergedSegment, (Single)edge.Weight);
                    else
                        _innerDiffences[mergedSegment] = Math.Max(_innerDiffences[mergedSegment], weight);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generates the initial edge list.
        /// </summary>
        /// <returns>The sorted edge list.</returns>
        private List<SimpleGraphEdge> GenerateEdgeList()
        {
            List<SimpleGraphEdge> edges = new List<SimpleGraphEdge>();

            // compute edge weight
            for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                {
                    Int32 index = rowIndex * Source.Raster.NumberOfColumns + columnIndex;
                    Single weight;

                    switch (Source.Raster.Format)
                    { 
                        case RasterFormat.Integer:                            
                            if (columnIndex < Source.Raster.NumberOfColumns - 1)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetValues(rowIndex, columnIndex), Source.Raster.GetValues(rowIndex, columnIndex + 1));
                                edges.Add(new SimpleGraphEdge(index, index + 1, weight));
                            }

                            if (columnIndex > 0)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetValues(rowIndex, columnIndex), Source.Raster.GetValues(rowIndex, columnIndex - 1));
                                edges.Add(new SimpleGraphEdge(index, index - 1, weight));
                            }

                            if (rowIndex > 0)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetValues(rowIndex, columnIndex), Source.Raster.GetValues(rowIndex - 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index - Source.Raster.NumberOfColumns, weight));
                            }

                            if (rowIndex < Source.Raster.NumberOfRows - 1)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetValues(rowIndex, columnIndex), Source.Raster.GetValues(rowIndex + 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index + Source.Raster.NumberOfColumns, weight));
                            }
                            break;
                        case RasterFormat.Floating:
                            if (columnIndex < Source.Raster.NumberOfColumns - 1)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetFloatValues(rowIndex, columnIndex), Source.Raster.GetFloatValues(rowIndex, columnIndex + 1));
                                edges.Add(new SimpleGraphEdge(index, index + 1, weight));
                            }

                            if (columnIndex > 0)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetFloatValues(rowIndex, columnIndex), Source.Raster.GetFloatValues(rowIndex, columnIndex - 1));
                                edges.Add(new SimpleGraphEdge(index, index - 1, weight));
                            }

                            if (rowIndex > 0)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetFloatValues(rowIndex, columnIndex), Source.Raster.GetFloatValues(rowIndex - 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index - Source.Raster.NumberOfColumns, weight));
                            }

                            if (rowIndex < Source.Raster.NumberOfRows - 1)
                            {
                                weight = (Single)_distance.Distance(Source.Raster.GetFloatValues(rowIndex, columnIndex), Source.Raster.GetFloatValues(rowIndex + 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index + Source.Raster.NumberOfColumns, weight));
                            }
                            break;
                    }

                }
            }

            // the edges are sorted by weight in descending order
            edges.Sort((x, y) => -x.Weight.CompareTo(y.Weight));

            return edges;
        }

        /// <summary>
        /// Computes the maximum internal difference of two segments.
        /// </summary>
        /// <param name="firstSegment">The first segment.</param>
        /// <param name="secondSegment">The second segment.</param>
        /// <returns>The maximum internal difference of the two segments.</returns>
        private Double ComputeMaximumInternalDifference(Segment firstSegment, Segment secondSegment)
        {
            return Math.Min(InternalDifference(firstSegment) + _mergeThreshold / firstSegment.Count, InternalDifference(secondSegment) + _mergeThreshold / secondSegment.Count);
        }
        
        /// <summary>
        /// Returns the internal difference of the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>The internal difference of <paramref name="segment" />.</returns>
        private Single InternalDifference(Segment segment)
        {
            Single value;
            if (!_innerDiffences.TryGetValue(segment, out value))
                return 0;

            return value;
        }

        #endregion
    }
}
