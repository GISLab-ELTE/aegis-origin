/// <copyright file="ConversionMethods.cs" company="Eötvös Loránd University (ELTE)">
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Conversion
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationMethod" /> instances for conversion operations.
    /// </summary>
    [IdentifiedObjectCollection(typeof(OperationMethod))]
    public static class ConversionMethods
    {
        #region Query fields

        private static OperationMethod[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationMethod" /> instances in the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationMethod" /> instances in the collection.</value>
        public static IList<OperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(ConversionMethods).GetProperties().
                                                     Where(property => property.Name != "All").
                                                     Select(property => property.GetValue(null, null) as OperationMethod).
                                                     ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified identifier.</returns>
        public static IList<OperationMethod> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified name.</returns>
        public static IList<OperationMethod> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static fields

        private static OperationMethod _geometryPolygonization;
        private static OperationMethod _geometryToGraphConversion;
        private static OperationMethod _geometryToNetworkConversion;
        private static OperationMethod _graphToGeometryConversion;

        #endregion

        #region Public static properties

        /// <summary>
        /// Geometry polygonization.
        /// </summary>
        public static OperationMethod GeometryPolygonization
        {
            get
            {
                return _geometryPolygonization ?? (_geometryPolygonization =
                    OperationMethod.CreateMethod<IGeometry, IGeometryGraph>(
                        "AEGIS::212185", "Geometry polygonization",
                        "Converts geometries of any kind to polygonial representation.", null, "1.0.0",
                        false,
                        GeometryModel.Spatial2D | GeometryModel.Spatial3D | GeometryModel.SpatioTemporal2D | GeometryModel.SpatioTemporal3D,
                        ExecutionMode.OutPlace,
                        ExecutionDomain.Local | ExecutionDomain.Remote,
                        OperationParameters.GeometryFactory,
                        OperationParameters.MetadataPreservation
                    ));
            }
        }

        /// <summary>
        /// Geometry to graph conversion.
        /// </summary>
        public static OperationMethod GeometryToGraphConversion
        {
            get
            {
                return _geometryToGraphConversion ?? (_geometryToGraphConversion =
                    OperationMethod.CreateMethod<IGeometry, IGeometryGraph>(
                        "AEGIS::212185", "Geometry to graph conversion",
                        "Converts geometries to a single graph representation. The resulting graph can match the original directions of the geometry lines, os can be bidirectional.", null, "1.0.0",
                        false,
                        GeometryModel.Spatial2D | GeometryModel.Spatial3D | GeometryModel.SpatioTemporal2D | GeometryModel.SpatioTemporal3D,
                        ExecutionMode.OutPlace,
                        ExecutionDomain.Local | ExecutionDomain.Remote,
                        ConversionParameters.BidirectionalConversion,
                        OperationParameters.MetadataPreservation
                    ));
            }
        }

        /// <summary>
        /// Geometry to network conversion.
        /// </summary>
        public static OperationMethod GeometryToNetworkConversion
        {
            get
            {
                return _geometryToNetworkConversion ?? (_geometryToNetworkConversion =
                     OperationMethod.CreateMethod<IGeometry, IGeometryGraph>(
                        "AEGIS::212101", "Geometry to network conversion",
                        "Converts geometries to a single network representation. The resulting network can match the original directions of the geometry lines, os can be bidirectional.", null, "1.0.0",
                        false,
                        GeometryModel.Spatial2D | GeometryModel.Spatial3D | GeometryModel.SpatioTemporal2D | GeometryModel.SpatioTemporal3D,
                        ExecutionMode.OutPlace,
                        ExecutionDomain.Local | ExecutionDomain.Remote,
                        ConversionParameters.BidirectionalConversion,
                        OperationParameters.MetadataPreservation
                    ));
            }
        }

        /// <summary>
        /// Graph to geometry conversion.
        /// </summary>
        public static OperationMethod GraphToGeometryConversion
        {
            get
            {
                return _graphToGeometryConversion ?? (_graphToGeometryConversion =
                     OperationMethod.CreateMethod<IGeometryGraph, IGeometry>(
                        "AEGIS::212110", "Graph to geometry conversion",
                        "Converts a graph to geometry representation. The dimension of the extracted geometries can be limited to points (0), curves (1) and surfaces (2). The factory of the resulting geometries can also be specified.", null, "1.0.0",
                        false,
                        GeometryModel.Spatial2D | GeometryModel.Spatial3D | GeometryModel.SpatioTemporal2D | GeometryModel.SpatioTemporal3D,
                        ExecutionMode.OutPlace,
                        ExecutionDomain.Local | ExecutionDomain.Remote,
                        ConversionParameters.GeometryDimension,
                        OperationParameters.GeometryFactory,
                        OperationParameters.MetadataPreservation
                    ));
            }
        }

        #endregion
    }
}
