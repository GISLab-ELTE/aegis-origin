/// <copyright file="GraphOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for graph operations.
    /// </summary>
    [OperationParameterCollection]
    public class GraphOperationParameters
    {
        #region Query fields

        private static OperationParameter[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationParameter" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationParameter" /> instances within the collection.</value>
        public static IList<OperationParameter> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GraphOperationParameters).GetProperties().
                                                            Where(property => property.Name != "All").
                                                            Select(property => property.GetValue(null, null) as OperationParameter).
                                                            ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationParameter" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationParameter" /> instances that match the specified identifier.</returns>
        public static IList<OperationParameter> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="OperationParameter" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationParameter" /> instances that match the specified name.</returns>
        public static IList<OperationParameter> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static fields

        private static OperationParameter _capacityMetric;
        private static OperationParameter _heuristicMetric;
        private static OperationParameter _heuristicLimitMultiplier;
        private static OperationParameter _sourceVertex;
        private static OperationParameter _targetVertex;
        private static OperationParameter _weightMetric;

        #endregion

        #region Public static methods

        /// <summary>
        /// Capacity metric.
        /// </summary>
        public static OperationParameter CapacityMetric
        {
            get
            {
                return _capacityMetric ?? (_capacityMetric =
                    OperationParameter.CreateOptionalParameter<Func<IGraphEdge, Int32>>(
                        "AEGIS::222302", "Capacity metric",
                        "Capacity metric for edges.", null,
                        (edge => 1)
                    ));
            }
        }

        /// <summary>
        /// Heuristic metric.
        /// </summary>
        public static OperationParameter HeuristicMetric
        {
            get
            {
                return _heuristicMetric ?? (_heuristicMetric =
                    OperationParameter.CreateOptionalParameter<Func<IGraphVertex, IGraphVertex, Double>>(
                        "AEGIS::222305", "Heuristic metric", 
                        "Heuristic metric for traversal methods used for computing the distance between the current vertex and the specified target vertex.", null, 
                        ((source, target) => Coordinate.Distance(source.Coordinate, target.Coordinate))
                    ));
            }
        }

        /// <summary>
        /// Heuristic limit modifier.
        /// </summary>
        public static OperationParameter HeuristicLimitMultiplier
        {
            get
            {
                return _heuristicLimitMultiplier ?? (_heuristicLimitMultiplier =
                    OperationParameter.CreateOptionalParameter<Double>(
                        "AEGIS::222312", "Heuristic limit modifier", 
                        "Heuristic limit modifier for traversal methods for determining the maximum distance factor from the specified target vertex. The value must be at least 2.", null, 
                        5,
                        Conditions.IsGreaterThan(2)
                    ));
            }
        }

        /// <summary>
        /// Source vertex.
        /// </summary>
        public static OperationParameter SourceVertex
        {
            get
            {
                return _sourceVertex ?? (_sourceVertex =
                    OperationParameter.CreateRequiredParameter<IGraphVertex>(
                        "AEGIS::222361", "Source vertex", 
                        "Source graph vertex for traversal methods.", null
                    ));
            }
        }

        /// <summary>
        /// Target vertex.
        /// </summary>
        public static OperationParameter TargetVertex
        {
            get
            {
                return _targetVertex ?? (_targetVertex =
                    OperationParameter.CreateRequiredParameter<IGraphVertex>(
                        "AEGIS::222362", "Target vertex", 
                        "Target graph vertex for traversal methods.", null
                    ));
            }
        }

        /// <summary>
        /// Weight metric.
        /// </summary>
        public static OperationParameter WeightMetric
        {
            get
            {
                return _weightMetric ?? (_weightMetric =
                    OperationParameter.CreateOptionalParameter<Func<IGraphEdge, Double>>(
                        "AEGIS::222301", "Weight metric",
                        "Weight metric for edges used by traversal methods.", null,
                        (edge => Coordinate.Distance(edge.Source.Coordinate, edge.Target.Coordinate))
                    ));
            }
        }

        #endregion
    }
}
