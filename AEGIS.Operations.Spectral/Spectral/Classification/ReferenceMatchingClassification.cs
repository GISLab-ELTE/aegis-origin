/// <copyright file="ReferenceMatchingClassification.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an operation performing classification by matching a reference image.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213850", "Reference matching classification")]
    public class ReferenceMatchingClassification : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The segment collection.
        /// </summary>
        private SegmentCollection _segmentCollection;

        /// <summary>
        /// The reference geometry.
        /// </summary>
        private ISpectralGeometry _referenceGeometry;

        /// <summary>
        /// The dictionary mapping segments to indices.
        /// </summary>
        private Dictionary<Segment, Int32> _segmentToIndexDictionary;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceMatchingClassification" /> class.
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
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public ReferenceMatchingClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceMatchingClassification" /> class.
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
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public ReferenceMatchingClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.ReferenceMatchingClassification, parameters)
        {
            _segmentCollection = ResolveParameter<SegmentCollection>(SpectralOperationParameters.SegmentCollection);
            _referenceGeometry = ResolveParameter<ISpectralGeometry>(SpectralOperationParameters.ClassificationReferenceGeometry);

            if (_segmentCollection.Raster != _source.Raster)
                throw new ArgumentException(String.Format("The parameter value ({0}) does not satisfy the conditions of the parameter.", SpectralOperationParameters.SegmentCollection.Name));
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
            Segment segment = _segmentCollection.GetSegment(rowIndex, columnIndex);

            if (!_segmentToIndexDictionary.ContainsKey(segment))
                return 0;

            Int32 referenceIndex = _segmentToIndexDictionary[segment];
            return _referenceGeometry.Raster.GetValue(referenceIndex / _referenceGeometry.Raster.NumberOfColumns, referenceIndex % _referenceGeometry.Raster.NumberOfColumns, bandIndex);
        }

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            Segment segment = _segmentCollection.GetSegment(rowIndex, columnIndex);
            
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                return new UInt32[_referenceGeometry.Raster.NumberOfBands];

            Int32 referenceIndex = _segmentToIndexDictionary[segment];
            return _referenceGeometry.Raster.GetValues(referenceIndex / _referenceGeometry.Raster.NumberOfColumns, referenceIndex % _referenceGeometry.Raster.NumberOfColumns);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Segment segment = _segmentCollection.GetSegment(rowIndex, columnIndex);

            if (!_segmentToIndexDictionary.ContainsKey(segment))
                return 0;

            Int32 referenceIndex = _segmentToIndexDictionary[segment];
            return _referenceGeometry.Raster.GetFloatValue(referenceIndex / _referenceGeometry.Raster.NumberOfColumns, referenceIndex % _referenceGeometry.Raster.NumberOfColumns, bandIndex);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Segment segment = _segmentCollection.GetSegment(rowIndex, columnIndex);

            if (!_segmentToIndexDictionary.ContainsKey(segment))
                return new Double[_referenceGeometry.Raster.NumberOfBands];

            Int32 referenceIndex = _segmentToIndexDictionary[segment];
            return _referenceGeometry.Raster.GetFloatValues(referenceIndex / _referenceGeometry.Raster.NumberOfColumns, referenceIndex % _referenceGeometry.Raster.NumberOfColumns);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            Dictionary<Segment, Dictionary<Int32, Int32>> segmentToClassDictionary = new Dictionary<Segment, Dictionary<Int32, Int32>>();
            Dictionary<Int32, Int32> classToIndexDictionary = new Dictionary<Int32, Int32>(); 

            IRaster referenceRaster = _referenceGeometry.Raster;
            Int32 sourceRowIndex, sourceColumnIndex;

            // map all values (and indices) to segments
            for (Int32 rowIndex = 0; rowIndex < referenceRaster.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < referenceRaster.NumberOfColumns; columnIndex++)
                {
                    Int32 referenceHashCode = 0;

                    switch (referenceRaster.Format)
                    {
                        case RasterFormat.Floating:
                            Double[] referenceFloatValues = referenceRaster.GetFloatValues(rowIndex, columnIndex);
                            if (referenceFloatValues.All(value => value == 0))
                                continue;

                            referenceHashCode = referenceFloatValues.Select(value => value.GetHashCode()).Aggregate((x, y) => (x << 1) ^ y);
                            break;
                        case RasterFormat.Integer:

                            UInt32[] referenceValues = referenceRaster.GetValues(rowIndex, columnIndex);
                            if (referenceValues.All(value => value == 0))
                                continue;

                            referenceHashCode = referenceValues.Select(value => value.GetHashCode()).Aggregate((x, y) => (x << 1) ^ y);
                            break;
                    }

                    
                    Coordinate coordinate = referenceRaster.Mapper.MapCoordinate(rowIndex, columnIndex);
                    _source.Raster.Mapper.MapRaster(coordinate, out sourceRowIndex, out sourceColumnIndex);

                    // check if the reference location is available in the source
                    if (sourceRowIndex < 0 || sourceRowIndex >= Source.Raster.NumberOfRows || sourceColumnIndex < 0 || sourceColumnIndex >= Source.Raster.NumberOfColumns)
                        continue;

                    Segment segment = _segmentCollection.GetSegment(sourceRowIndex, sourceColumnIndex);

                    // check if the segment was already mapped
                    if (!segmentToClassDictionary.ContainsKey(segment))
                        segmentToClassDictionary.Add(segment, new Dictionary<Int32, Int32>());

                    if (!segmentToClassDictionary[segment].ContainsKey(referenceHashCode))
                        segmentToClassDictionary[segment].Add(referenceHashCode, 0);

                    segmentToClassDictionary[segment][referenceHashCode]++;

                    if (!classToIndexDictionary.ContainsKey(referenceHashCode))
                        classToIndexDictionary.Add(referenceHashCode, rowIndex * referenceRaster.NumberOfColumns + columnIndex);
                }

            _segmentToIndexDictionary = new Dictionary<Segment, Int32>();

            // filter indices for most frequent value
            foreach (Segment segment in segmentToClassDictionary.Keys)
            {
                Int32 maxValueHashCode = 0;

                foreach (Int32 referenceHashCode in segmentToClassDictionary[segment].Keys)
                {
                    if (maxValueHashCode == 0 || segmentToClassDictionary[segment][referenceHashCode] > segmentToClassDictionary[segment][maxValueHashCode])
                        maxValueHashCode = referenceHashCode;
                }

                _segmentToIndexDictionary.Add(segment, classToIndexDictionary[maxValueHashCode]);
            }

            // create result
            _result = Source.Factory.CreateSpectralGeometry(_source,
                                                            PrepareRasterResult(_referenceGeometry.Raster.Format,
                                                                                _referenceGeometry.Raster.NumberOfBands,
                                                                                _source.Raster.NumberOfRows,
                                                                                _source.Raster.NumberOfColumns,
                                                                                _referenceGeometry.Raster.RadiometricResolutions,
                                                                                _source.Raster.Mapper),
                                                            _referenceGeometry.Presentation,
                                                            _source.Imaging);
        }

        #endregion
    }
}
