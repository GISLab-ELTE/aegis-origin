/// <copyright file="SegmentClassification.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents an operation performing spectral classification of segments.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213818", "Segment classification")]
    public class SegmentClassification : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The segment collection.
        /// </summary>
        private SegmentCollection _segmentCollection;

        /// <summary>
        /// The dictionary mapping segments to numbers.
        /// </summary>
        private Dictionary<Segment, UInt32> _segmentNumbers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentClassification" /> class.
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
        /// The value of a parameter is not within the expected range.
        /// or
        /// The source geometry does not contain raster data.
        /// or
        /// The raster format of the source is not supported by the method.
        /// </exception>
        public SegmentClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentClassification" /> class.
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
        public SegmentClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.SegmentClassification, parameters)
        {
            _segmentCollection = ResolveParameter<SegmentCollection>(SpectralOperationParameters.SegmentCollection);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            Int32 radiometricResolution = _segmentCollection.Count > 65536 ? 32 : (_segmentCollection.Count > 256 ? 16 : 8);

            _result = Source.Factory.CreateSpectralGeometry(_source,
                                                      PrepareRasterResult(RasterFormat.Integer,
                                                                          1,
                                                                          _source.Raster.NumberOfRows,
                                                                          _source.Raster.NumberOfColumns,
                                                                          radiometricResolution,
                                                                          _source.Raster.Mapper),
                                                      RasterPresentation.CreateGrayscalePresentation(),
                                                      null);

            _segmentNumbers = new Dictionary<Segment, UInt32>();

            UInt32 number = 0;
            foreach (Segment segment in _segmentCollection.Segments)
            {
                _segmentNumbers.Add(segment, number);
                number++;
            }
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _segmentNumbers[_segmentCollection[rowIndex, columnIndex]];
        }

        /// <summary>
        /// Computes the specified spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            return new UInt32[] { _segmentNumbers[_segmentCollection[rowIndex, columnIndex]] };
        }

        #endregion
    }
}
