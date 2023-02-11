// <copyright file="SegmentCollection.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections.Segmentation
{
    /// <summary>
    /// Represents a collections of segments within a raster.
    /// </summary>
    public class SegmentCollection
    {
        #region Private fields

        /// <summary>
        /// The dictionary mapping indices to segments.
        /// </summary>
        protected Dictionary<Int32, Segment> _indexToSegmentDictionary;

        /// <summary>
        /// The dictionary mapping segments to indices.
        /// </summary>
        protected Dictionary<Segment, List<Int32>> _segmentToIndexDictionary;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentCollection" /> class.
        /// </summary>
        public SegmentCollection()
            : this(SpectralStatistics.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentCollection" /> class.
        /// </summary>
        /// <param name="statistics">The statistics computed for the segments.</param>
        public SegmentCollection(SpectralStatistics statistics)
        {
            Statistics = statistics;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentCollection" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public SegmentCollection(IRaster raster)
            : this(raster, SpectralStatistics.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentCollection" /> class.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="statistics">The statistics computed for the segments.</param>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        public SegmentCollection(IRaster raster, SpectralStatistics statistics)
            : this(statistics)
        {
            if (raster == null)
                throw new ArgumentNullException("raster", "The raster is null.");

            Raster = raster;
            Count = raster.NumberOfRows * raster.NumberOfColumns;
            _indexToSegmentDictionary = new Dictionary<Int32, Segment>(raster.NumberOfRows * raster.NumberOfColumns);
            _segmentToIndexDictionary = new Dictionary<Segment, List<Int32>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentCollection" /> class.
        /// </summary>
        /// <param name="other">The other segment collection.</param>
        /// <exception cref="System.ArgumentNullException">The other segment collection is null.</exception>
        public SegmentCollection(SegmentCollection other)
            : this(other, SpectralStatistics.None)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentCollection" /> class.
        /// </summary>
        /// <param name="other">The other segment collection.</param>
        /// <param name="statistics">The spectral statistics.</param>
        /// <exception cref="System.ArgumentNullException">The other segment collection is null.</exception>
        public SegmentCollection(SegmentCollection other, SpectralStatistics statistics)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other segment collection is null.");

            Raster = other.Raster;
            Count = other.Count;
            Statistics = other.Statistics | statistics;
            _indexToSegmentDictionary = new Dictionary<Int32, Segment>();
            _segmentToIndexDictionary = new Dictionary<Segment, List<Int32>>();

            foreach (Segment otherSegment in other._segmentToIndexDictionary.Keys)
            {
                List<Int32> otherIndices = other._segmentToIndexDictionary[otherSegment];
                
                Segment segment;
                if (Statistics == other.Statistics)
                {
                    segment = new Segment(otherSegment);
                    otherIndices.ForEach(x => _indexToSegmentDictionary.Add(x, segment));
                }
                else
                {
                    Int32 rowIndex = otherIndices[0] / Raster.NumberOfColumns;
                    Int32 columnIndex = otherIndices[0] % Raster.NumberOfColumns;

                    if (Raster.Format == RasterFormat.Integer)
                    {
                        segment = new Segment(Raster.GetValues(rowIndex, columnIndex), Statistics);

                        for (Int32 i = 0; i < other._segmentToIndexDictionary[otherSegment].Count; i++)
                        {
                            _indexToSegmentDictionary.Add(otherIndices[i], segment);
                            rowIndex = otherIndices[i] / Raster.NumberOfColumns;
                            columnIndex = otherIndices[i] % Raster.NumberOfColumns;

                            segment.AddValues(Raster.GetValues(rowIndex, columnIndex));
                        }
                    }
                    else
                    {
                        segment = new Segment(Raster.GetFloatValues(rowIndex, columnIndex), Statistics);

                        for (Int32 i = 0; i < other._segmentToIndexDictionary[otherSegment].Count; i++)
                        {
                            _indexToSegmentDictionary.Add(otherIndices[i], segment);
                            rowIndex = otherIndices[i] / Raster.NumberOfColumns;
                            columnIndex = otherIndices[i] % Raster.NumberOfColumns;

                            segment.AddFloatValues(Raster.GetFloatValues(rowIndex, columnIndex));
                        }
                    }
                }

                _segmentToIndexDictionary.Add(segment, new List<Int32>(otherIndices));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The number of segments within the collection.
        /// </summary>
        /// <value>The number of segments within the collection.</value>
        public virtual Int32 Count { get; set; }

        /// <summary>
        /// The raster of the collection.
        /// </summary>
        public IRaster Raster { get; set; }

        /// <summary>
        /// Gets the statistics computed for the segments.
        /// </summary>
        /// <value>The statistics computed for the segments.</value>
        public SpectralStatistics Statistics { get; private set; }

        /// <summary>
        /// Gets the segment at the specified row and column indices.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The segment at the specified row and column indices.</returns>
        public Segment this[Int32 rowIndex, Int32 columnIndex]
        {
            get { return GetSegment(rowIndex, columnIndex); }
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the collection contains the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns><c>true</c> if the collection contains the segment; otherwise <c>false</c>.</returns>
        public virtual Boolean Contains(Segment segment)
        {
            return _segmentToIndexDictionary.ContainsKey(segment);
        }

        /// <summary>
        /// Returns the segment at the specified row and column indices.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The segment at the specified row and column indices.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        public virtual Segment GetSegment(Int32 rowIndex, Int32 columnIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The row index is less than 0.");
            if (rowIndex >= Raster.NumberOfRows)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The column index is less than 0.");
            if (columnIndex >= Raster.NumberOfColumns)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The column index is equal to or greater than the number of columns.");

            Int32 index = rowIndex * Raster.NumberOfColumns + columnIndex;

            if (!_indexToSegmentDictionary.ContainsKey(index))
            {
                Segment segment = new Segment(Raster.GetFloatValues(rowIndex, columnIndex), Statistics);
                _indexToSegmentDictionary.Add(index, segment);
                _segmentToIndexDictionary.Add(segment, new List<Int32> { index });
            }

            return _indexToSegmentDictionary[index];
        }

        /// <summary>
        /// Returns the segments within the collection.
        /// </summary>
        /// <returns>The collection containing the segments.</returns>
        public virtual IEnumerable<Segment> GetSegments()
        {
            // if some segments are not constructed, they must be constructed
            if (_indexToSegmentDictionary.Count < Raster.NumberOfColumns * Raster.NumberOfRows)
            {
                for (Int32 rowIndex = 0; rowIndex < Raster.NumberOfRows; rowIndex++)
                {
                    for (Int32 columnIndex = 0; columnIndex < Raster.NumberOfColumns; columnIndex++)
                    {
                        Int32 index = rowIndex * Raster.NumberOfColumns + columnIndex;
                        if (!_indexToSegmentDictionary.ContainsKey(index))
                        {
                            Segment segment = new Segment(Raster.GetFloatValues(rowIndex, columnIndex), Statistics);
                            _indexToSegmentDictionary.Add(index, segment);
                            _segmentToIndexDictionary.Add(segment, new List<Int32> { index });
                        }
                    }
                }
            }

            foreach (Segment segment in _segmentToIndexDictionary.Keys)
                yield return segment;
        }

        /// <summary>
        /// Returns the floating point values of the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>The collection of spectral values.</returns>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        public virtual IEnumerable<Double[]> GetFloatValues(Segment segment)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                throw new ArgumentException("The segment is not within the collection.", "segment");

            foreach (Int32 index in _segmentToIndexDictionary[segment])
            {
                Int32 rowIndex = index / Raster.NumberOfColumns;
                Int32 columnIndex = index % Raster.NumberOfColumns;

                yield return Raster.GetFloatValues(rowIndex, columnIndex);
            }
        }

        /// <summary>
        /// Returns the floating point values of the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The collection of spectral values of the specified band.</returns>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public virtual IEnumerable<Double> GetFloatValues(Segment segment, Int32 bandIndex)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                throw new ArgumentException("The segment is not within the collection.", "segment");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bandIndex), "The band index is less than 0.");
            if (bandIndex >= Raster.NumberOfBands)
                throw new ArgumentOutOfRangeException(nameof(bandIndex), "The band index is equal to or greater than the number of bands.");

            foreach (Int32 index in _segmentToIndexDictionary[segment])
            {
                Int32 rowIndex = index / Raster.NumberOfColumns;
                Int32 columnIndex = index % Raster.NumberOfColumns;

                yield return Raster.GetFloatValue(rowIndex, columnIndex, bandIndex);
            }
        }

        /// <summary>
        /// Returns the values of the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>The collection of spectral values.</returns>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        public virtual IEnumerable<UInt32[]> GetValues(Segment segment)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                throw new ArgumentException("The segment is not within the collection.", "segment");

            foreach (Int32 index in _segmentToIndexDictionary[segment])
            {
                Int32 rowIndex = index / Raster.NumberOfColumns;
                Int32 columnIndex = index % Raster.NumberOfColumns;

                yield return Raster.GetValues(rowIndex, columnIndex);
            }
        }

        /// <summary>
        /// Returns the floating point values of the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <returns>The collection of spectral values of the specified band.</returns>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The band index is less than 0.
        /// or
        /// The band index is equal to or greater than the number of bands.
        /// </exception>
        public virtual IEnumerable<UInt32> GetValues(Segment segment, Int32 bandIndex)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                throw new ArgumentException("The segment is not within the collection.", "segment");
            if (bandIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(bandIndex), "The band index is less than 0.");
            if (bandIndex >= Raster.NumberOfBands)
                throw new ArgumentOutOfRangeException(nameof(bandIndex), "The band index is equal to or greater than the number of bands.");

            foreach (Int32 index in _segmentToIndexDictionary[segment])
            {
                Int32 rowIndex = index / Raster.NumberOfColumns;
                Int32 columnIndex = index % Raster.NumberOfColumns;

                yield return Raster.GetValue(rowIndex, columnIndex, bandIndex);
            }
        }

        /// <summary>
        /// Gets the starting row and column index of the segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        public virtual void GetIndex(Segment segment, out Int32 rowIndex, out Int32 columnIndex)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");

            List<Int32> indexList;
            if (!_segmentToIndexDictionary.TryGetValue(segment, out indexList))
                throw new ArgumentException("The segment is not within the collection.", "segment");

            rowIndex = indexList[0] / Raster.NumberOfColumns;
            columnIndex = indexList[0] % Raster.NumberOfColumns;
        }

        /// <summary>
        /// Merges the specified segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        public virtual void MergeSegments(Segment segment, Int32 rowIndex, Int32 columnIndex)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                throw new ArgumentException("The segment is not within the collection.", "segment");
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The row index is less than 0.");
            if (rowIndex >= Raster.NumberOfRows)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The column index is less than 0.");
            if (columnIndex >= Raster.NumberOfColumns)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The column index is equal to or greater than the number of columns.");


            Int32 index = rowIndex * Raster.NumberOfColumns + columnIndex;

            if (_indexToSegmentDictionary.ContainsKey(index))
            {
                Segment otherSegment = _indexToSegmentDictionary[index];

                if (segment == otherSegment)
                    return;

                if (segment.Count > otherSegment.Count)
                    ApplyMergeSegments(segment, otherSegment);
                else
                    ApplyMergeSegments(otherSegment, segment);
            }
            else
                ApplyMergeSegments(segment, index);
        }

        /// <summary>
        /// Merges the specified segments.
        /// </summary>
        /// <param name="first">The first segment.</param>
        /// <param name="second">The second segment.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first segment is null.
        /// or
        /// The second segment is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The first segment is not within the collection.
        /// or
        /// The second segment is not within the collection.
        /// </exception>
        public virtual Segment MergeSegments(Segment first, Segment second)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first segment is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(first))
                throw new ArgumentException("The first segment is not within the collection.", "first");
            if (!_segmentToIndexDictionary.ContainsKey(second))
                throw new ArgumentException("The second segment is not within the collection.", "second");

            if (first == second)
                return first;

            if (first.Count > second.Count)
            {
                ApplyMergeSegments(first, second);
                return first;
            }
            else
            {
                ApplyMergeSegments(second, first);
                return second;
            }
        }

        /// <summary>
        /// Merges the segments at the specified indices.
        /// </summary>
        /// <param name="firstRowIndex">The first row index.</param>
        /// <param name="firstColumnIndex">The first column index.</param>
        /// <param name="secondRowIndex">The second row index.</param>
        /// <param name="secondColumnIndex">The second column index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The first row index is less than 0.
        /// or
        /// The first row index is equal to or greater than the number of rows.
        /// or
        /// The first column index is less than 0.
        /// or
        /// The first column index is equal to or greater than the number of columns.
        /// or
        /// The second row index is less than 0.
        /// or
        /// The second row index is equal to or greater than the number of rows.
        /// or
        /// The second column index is less than 0.
        /// or
        /// The second column index is equal to or greater than the number of columns.
        /// </exception>
        public virtual void MergeSegments(Int32 firstRowIndex, Int32 firstColumnIndex, Int32 secondRowIndex, Int32 secondColumnIndex)
        {
            if (firstRowIndex < 0)
                throw new ArgumentOutOfRangeException("firstRowIndex", "The first row index is less than 0.");
            if (firstRowIndex >= Raster.NumberOfRows)
                throw new ArgumentOutOfRangeException("firstRowIndex", "The first row index is equal to or greater than the number of rows.");
            if (firstColumnIndex < 0)
                throw new ArgumentOutOfRangeException("firstColumnIndex", "The first column index is less than 0.");
            if (firstColumnIndex >= Raster.NumberOfColumns)
                throw new ArgumentOutOfRangeException("firstColumnIndex", "The first column index is equal to or greater than the number of columns.");
            if (secondRowIndex < 0)
                throw new ArgumentOutOfRangeException("secondRowIndex", "The second row index is less than 0.");
            if (secondRowIndex >= Raster.NumberOfRows)
                throw new ArgumentOutOfRangeException("secondRowIndex", "The second row index is equal to or greater than the number of rows.");
            if (secondColumnIndex < 0)
                throw new ArgumentOutOfRangeException("secondColumnIndex", "The second column index is less than 0.");
            if (secondColumnIndex >= Raster.NumberOfColumns)
                throw new ArgumentOutOfRangeException("secondColumnIndex", "The second column index is equal to or greater than the number of columns.");

            // query the sets
            Int32 firstIndex = firstRowIndex * Raster.NumberOfColumns + firstColumnIndex;
            Int32 secondIndex = secondRowIndex * Raster.NumberOfColumns + secondColumnIndex;

            if (firstIndex == secondIndex)
                return;

            // apply merge on the available set
            if (!_indexToSegmentDictionary.ContainsKey(firstIndex))
            {
                if (!_indexToSegmentDictionary.ContainsKey(secondIndex))
                {
                    Segment segment = new Segment(Raster.GetFloatValues(firstRowIndex, firstColumnIndex), Statistics);
                    segment.AddFloatValues(Raster.GetFloatValues(secondRowIndex, secondColumnIndex));

                    _indexToSegmentDictionary.Add(firstIndex, segment);
                    _indexToSegmentDictionary.Add(secondIndex, segment);
                    _segmentToIndexDictionary.Add(segment, new List<Int32> { firstIndex, secondIndex });

                    Count--;
                }
                else
                    ApplyMergeSegments(_indexToSegmentDictionary[secondIndex], firstIndex);
            }
            else if (!_indexToSegmentDictionary.ContainsKey(secondIndex))
            {
                ApplyMergeSegments(_indexToSegmentDictionary[firstIndex], secondIndex);
            }
            else // or merge the sets
            {
                Segment firstSegment = _indexToSegmentDictionary[firstIndex];
                Segment secondSegment = _indexToSegmentDictionary[secondIndex];
                if (firstSegment == secondSegment)
                    return;

                if (firstSegment.Count > secondSegment.Count)
                    ApplyMergeSegments(firstSegment, secondSegment);
                else
                    ApplyMergeSegments(secondSegment, firstSegment);
            }
        }

        /// <summary>
        /// Splits the specified segment.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The row index is less than 0.
        /// or
        /// The row index is equal to or greater than the number of rows.
        /// or
        /// The column index is less than 0.
        /// or
        /// The column index is equal to or greater than the number of columns.
        /// </exception>
        public virtual void SplitSegment(Int32 rowIndex, Int32 columnIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The row index is less than 0.");
            if (rowIndex >= Raster.NumberOfRows)
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "The row index is equal to or greater than the number of rows.");
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The column index is less than 0.");
            if (columnIndex >= Raster.NumberOfColumns)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "The column index is equal to or greater than the number of columns.");

            ApplySplitSegment(_indexToSegmentDictionary[rowIndex * Raster.NumberOfColumns + columnIndex]);
        }

        /// <summary>
        /// Splits the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        /// <exception cref="System.ArgumentException">The segment is not within the collection.</exception>
        public virtual void SplitSegment(Segment segment)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");
            if (!_segmentToIndexDictionary.ContainsKey(segment))
                throw new ArgumentException("The segment is not within the collection.", "segment");

            ApplySplitSegment(segment);
        }

        /// <summary>
        /// Clears all segments from the collection.
        /// </summary>
        public void Clear()
        {
            _indexToSegmentDictionary.Clear();
            _segmentToIndexDictionary.Clear();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Merges the specified segments.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        private void ApplyMergeSegments(Segment segment, Int32 index)
        {
            segment.AddFloatValues(Raster.GetFloatValues(index / Raster.NumberOfColumns, index % Raster.NumberOfColumns));
            _indexToSegmentDictionary[index] = segment;

            _segmentToIndexDictionary[segment].Add(index);

            Count--;
        }

        /// <summary>
        /// Merges the specified segments.
        /// </summary>
        /// <param name="first">The first segment.</param>
        /// <param name="second">The second segment.</param>
        private void ApplyMergeSegments(Segment first, Segment second)
        {
            // create union set
            first.Merge(second);

            // update indices
            List<Int32> indices = _segmentToIndexDictionary[second];
            _segmentToIndexDictionary.Remove(second);
            _segmentToIndexDictionary[first].AddRange(indices);

            foreach (Int32 index in indices)
                _indexToSegmentDictionary[index] = first;

            Count--;
        }

        /// <summary>
        /// Splits the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        private void ApplySplitSegment(Segment segment)
        {
            List<Int32> indices = _segmentToIndexDictionary[segment];
            _segmentToIndexDictionary.Remove(segment);

            foreach (Int32 index in indices)
                _indexToSegmentDictionary.Remove(index);

            Count += segment.Count - 1;
        }

        #endregion
    }
}
