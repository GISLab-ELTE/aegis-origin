// <copyright file="SpectralSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation performing segmentation on spectral geometries.
    /// </summary>
    public abstract class SpectralSegmentation : Operation<ISpectralGeometry, ISpectralGeometry>
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
        protected SpectralSegmentation(ISpectralGeometry source, ISpectralGeometry target, SpectralOperationMethod method, IDictionary<OperationParameter, Object> parameters)
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

            Factory = ResolveParameter<IGeometryFactory>(CommonOperationParameters.GeometryFactory, Source.Factory);

            if (Factory.GetFactory<IRasterFactory>() == null)
                Factory = Source.Factory;
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// The geometry factory.
        /// </summary>
        protected IGeometryFactory Factory { get; private set; }

        /// <summary>
        /// Gets the source segment collection.
        /// </summary>
        /// <value>The source segment collection.</value>
        protected SegmentCollection SourceSegments { get { return (Source?.Raster as ISegmentedRaster)?.Segments; } }

        /// <summary>
        /// Gets the resulting segment collection.
        /// </summary>
        /// <value>The resulting segment collection.</value>
        protected SegmentCollection ResultSegments { get { return (Result?.Raster as ISegmentedRaster)?.Segments; } }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override sealed ISpectralGeometry PrepareResult()
        {
            return Factory.CreateSpectralGeometry(Source, Factory.GetFactory<IRasterFactory>().CreateSegmentedRaster(PrepareSegmentCollection()));
        }

        #endregion

        /// <summary>
        /// Prepares the segment collection.
        /// </summary>
        /// <returns>The resulting segment collection.</returns>
        protected virtual SegmentCollection PrepareSegmentCollection()
        {
            return new SegmentCollection(Source.Raster, _distance.Statistics);
        }
    }
}
