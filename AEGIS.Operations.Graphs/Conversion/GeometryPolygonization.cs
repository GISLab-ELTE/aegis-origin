/// <copyright file="GeometryPolygonization.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Conversion
{
    /// <summary>
    /// Represents an operation converting any kind of <see cref="IGeometry" /> to polygons.
    /// </summary>
    [OperationClass("AEGIS::212185", "Geometry polygonization")]
    public class GeometryPolygonization : Operation<IGeometry, IGeometry>
    {
        #region Private fields

        private readonly IGeometryFactory _factory;
        private readonly Boolean _metadataPreservation;
        private GeometryToNetworkConversion _networkConversion;
        private GraphToGeometryConversion _geometryConversion;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryPolygonization" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">The type of a parameter does not match the type specified by the method.</exception>
        public GeometryPolygonization(IGeometry source, IDictionary<OperationParameter, Object> parameters)
            : base(source, null, ConversionMethods.GeometryPolygonization, parameters)
        {
            _factory = GetParameter<IGeometryFactory>(OperationParameters.GeometryFactory, _source.Factory);
            _metadataPreservation = Convert.ToBoolean(GetParameter(OperationParameters.MetadataPreservation));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            Dictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ConversionParameters.BidirectionalConversion, true);
            parameters.Add(OperationParameters.MetadataPreservation, _metadataPreservation);

            _networkConversion = new GeometryToNetworkConversion(_source, parameters);
            _networkConversion.Execute();

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ConversionParameters.GeometryDimension, 2);
            parameters.Add(OperationParameters.GeometryFactory, _factory);
            parameters.Add(OperationParameters.MetadataPreservation, _metadataPreservation);

            _geometryConversion = new GraphToGeometryConversion(_networkConversion.Result, parameters);
            _geometryConversion.Execute();

            _result = _geometryConversion.Result;
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        protected override void FinalizeResult()
        {
            if (_result == null)
                return;

            // the result contains only polygons
            if (_result is IPolygon || _result is IEnumerable<IPolygon>)
                return;

            // the result contains different geometry types
            if (_result is IGeometryCollection<IGeometry>)
            {
                IGeometryCollection<IGeometry> collection = _result as IGeometryCollection<IGeometry>;

                List<IPolygon> polygonList = new List<IPolygon>();

                for (Int32 i = 0; i < collection.Count; i++)
                    if (collection[i] is IPolygon)
                        polygonList.Add(collection[i] as IPolygon);

                _result = _factory.CreateMultiPolygon(polygonList);
                return;
            }

            // the result does not contain any polygons
            _result = null;
        }

        #endregion
    }
}
