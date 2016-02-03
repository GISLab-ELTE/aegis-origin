/// <copyright file="QuadSegmentCollection.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.Segmentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a collections of quadtree segments within a raster.
    /// </summary>
    public class QuadSegmentCollection : SegmentCollection
    {
        #region Private fields

        /// <summary>
        /// The Morton code of the parent segment.
        /// </summary>
        private Double _parentMortonCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadSegmentCollection"/> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">raster;The raster is null.</exception>
        public QuadSegmentCollection(IRaster raster)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            Raster = raster;
            Count = 0;

            _indexToSegmentDictionary = new Dictionary<Int32, Segment>();
            Segment segment = new Segment(raster.NumberOfBands);
            Double[] spectralValues;
            for (Int32 rowNumber = 0; rowNumber < Raster.NumberOfRows; rowNumber++)
            {
                for (Int32 columnNumber = 0; columnNumber < raster.NumberOfColumns; columnNumber++)
                {
                    spectralValues = new Double[Raster.NumberOfBands];
                    spectralValues = Raster.GetFloatValues(rowNumber, columnNumber);
                    segment.AddFloatValues(spectralValues);
                }
            }

            _segmentToIndexDictionary = new Dictionary<Segment, List<Int32>>();
            for (Int32 index = 0; index < raster.NumberOfRows * raster.NumberOfColumns; index++)
                _indexToSegmentDictionary.Add(index, segment);
            _segmentToIndexDictionary.Add(_indexToSegmentDictionary[0], new List<Int32>(Enumerable.Range(0, raster.NumberOfRows * raster.NumberOfColumns)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadSegmentCollection"/> class.
        /// </summary>
        /// <param name="other">The other rectangular segment collection.</param>
        public QuadSegmentCollection(QuadSegmentCollection other)
            : base(other)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the segments within the collection.
        /// </summary>
        /// <returns>
        /// The collection containing the segments.
        /// </returns>
        public new IEnumerable<Segment> GetSegments()
        {
            foreach (Segment segment in _segmentToIndexDictionary.Keys)
                yield return segment;
        }

        /// <summary>
        /// Splits a segment into four equal segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        public void SplitIntoFour(Segment segment)
        {
            Int32 startIndex = _segmentToIndexDictionary[segment].First();
            Int32 endIndex = _segmentToIndexDictionary[segment].Last();

            Int32 startRowIndex = startIndex / Raster.NumberOfColumns;
            Int32 startColumnIndex = startIndex % Raster.NumberOfColumns;

            Int32 endRowIndex = endIndex / Raster.NumberOfColumns;
            Int32 endColumnIndex = endIndex % Raster.NumberOfColumns;

            _segmentToIndexDictionary.Remove(segment);
            _parentMortonCode = segment.MortonCode;

            GenerateIndices(startRowIndex, (endRowIndex + startRowIndex) / 2, startColumnIndex, (endColumnIndex + startColumnIndex) / 2, 1);
            GenerateIndices(startRowIndex, (endRowIndex + startRowIndex) / 2, (endColumnIndex + startColumnIndex) / 2 + 1, endColumnIndex, 2);
            GenerateIndices((endRowIndex + startRowIndex) / 2 + 1, endRowIndex, startColumnIndex, (endColumnIndex + startColumnIndex) / 2, 3);
            GenerateIndices((endRowIndex + startRowIndex) / 2 + 1, endRowIndex, (endColumnIndex + startColumnIndex) / 2 + 1, endColumnIndex, 4);

        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generates the indices of the segment.
        /// </summary>
        /// <param name="startRowIndex">Start row index.</param>
        /// <param name="endRowIndex">End row index.</param>
        /// <param name="startColumnIndex">Start column index.</param>
        /// <param name="endColumnIndex">End column index.</param>
        private void GenerateIndices(Int32 startRowIndex, Int32 endRowIndex, Int32 startColumnIndex, Int32 endColumnIndex, Int32 step)
        {
            Double[] spectralValues;
            List<Int32> indices = new List<Int32>();
            Segment segment = new Segment(Raster.NumberOfBands);
            for (Int32 rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
            {
                for (Int32 columnIndex = startColumnIndex; columnIndex <= endColumnIndex; columnIndex++)
                {
                    indices.Add(rowIndex * Raster.NumberOfColumns + columnIndex);
                    spectralValues = new Double[Raster.NumberOfBands];
                    spectralValues = Raster.GetFloatValues(rowIndex, columnIndex);
                    segment.AddFloatValues(spectralValues);
                }
            }

            if (indices.Count != 0)
            {
                segment.MortonCode = _parentMortonCode * 10 + step;
                _segmentToIndexDictionary.Add(segment, indices);
                foreach (Int32 index in indices)
                    _indexToSegmentDictionary[index] = segment;
            }
            Count = _segmentToIndexDictionary.Count;
        }

        #endregion

    }
}
