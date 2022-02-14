/// <copyright file="ClassifiedImageChangeDetection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Operations.Management;

namespace ELTE.AEGIS.Operations.Spectral.ChangeDetection
{
    /// <summary>
    /// Represents an operation that computes the change image of a classified image compared to a reference image.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253502", "Classified image change detection")]
    public sealed class ClassifiedImageChangeDetection : ClassificationChangeDetection<ISpectralGeometry>
    {
        #region Private fields

        private readonly Boolean _differencialChangeDetection;
        private readonly Boolean _lossDetection;
        private readonly List<Int32> _categoryIndices;
        private readonly Dictionary<Int32, Int32> _categoryBandAssociation;
        private readonly Int32 _numberOfCategories;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassifiedImageChangeDetection" /> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// A parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public ClassifiedImageChangeDetection(ISpectralGeometry input, IDictionary<OperationParameter, Object> parameters)
            : base(input, SpectralOperationMethods.ClassifiedImageChangeDetection, parameters)
        {
            _differencialChangeDetection = ResolveParameter<Boolean>(SpectralOperationParameters.DifferentialChangeDetection);
            _lossDetection = ResolveParameter<Boolean>(SpectralOperationParameters.LossDetection);

            _categoryIndices = new List<Int32>();
            if (IsProvidedParameter(SpectralOperationParameters.ChangeDetectionCategoryIndex))
            {
                Int32 categoryIndex = ResolveParameter<Int32>(SpectralOperationParameters.ChangeDetectionCategoryIndex);
                _categoryIndices.Add(categoryIndex);
            }
            else if (IsProvidedParameter(SpectralOperationParameters.ChangeDetectionCategoryIndices))
            {
                Int32[] indices = ResolveParameter<Int32[]>(SpectralOperationParameters.ChangeDetectionCategoryIndices);
                _categoryIndices.AddRange(indices);
            }
            else
            {
                _numberOfCategories = GetNumberOfCategories(Source.Raster);
                for (Int32 i = 0; i < _numberOfCategories; i++)
                    _categoryIndices.Add(i);
            }

            Int32 bandIndex = 0;
            _categoryBandAssociation = _categoryIndices.ToDictionary(x => x, x => bandIndex++);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override ISpectralGeometry PrepareResult()
        {
            IRasterFactory factory = Source.Factory.GetFactory<ISpectralGeometryFactory>()
                                                    .GetFactory<IRasterFactory>();

            Int32 radiometricResolution = _numberOfCategories > 65535 ? 32 : (_numberOfCategories > 255 ? 16 : 8);

            IRaster raster = factory.CreateRaster(_categoryIndices.Count, Source.Raster.NumberOfRows,
                                                    Source.Raster.NumberOfColumns, radiometricResolution,
                                                    Source.Raster.Mapper);

            return Source.Factory.CreateSpectralGeometry(Source, raster, Source.Presentation, Source.Imaging);
        }

        #endregion

        #region Protected ClassificationChangeDetection methods

        /// <summary>
        /// Computes the change for the specified values at the specified location.
        /// </summary>
        protected override void ComputeChange(Int32 rowIndex, Int32 columnIndex, UInt32 value, UInt32 referenceValue)
        {
            if (value == referenceValue)
            {
                if (!_differencialChangeDetection && _categoryBandAssociation.ContainsKey((Int32) value))
                    Result.Raster.SetValue(rowIndex, columnIndex, _categoryBandAssociation[(Int32) value], value);

                return;
            }

            if (!_lossDetection)
            {
                if (_categoryBandAssociation.ContainsKey((Int32)value))
                    Result.Raster.SetValue(rowIndex, columnIndex, _categoryBandAssociation[(Int32)value], referenceValue);

                if(!_differencialChangeDetection && _categoryBandAssociation.ContainsKey((Int32)referenceValue))
                    Result.Raster.SetValue(rowIndex, columnIndex, _categoryBandAssociation[(Int32)referenceValue], referenceValue);
            }
            else
            {
                if(_categoryBandAssociation.ContainsKey((Int32)referenceValue))
                    Result.Raster.SetValue(rowIndex, columnIndex, _categoryBandAssociation[(Int32)referenceValue], value);
                
                if(!_differencialChangeDetection && _categoryBandAssociation.ContainsKey((Int32)value))
                    Result.Raster.SetValue(rowIndex, columnIndex, _categoryBandAssociation[(Int32)value], value);

            }
        }

        #endregion
    }
}
