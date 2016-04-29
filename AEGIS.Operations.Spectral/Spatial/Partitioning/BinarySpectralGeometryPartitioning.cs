/// <copyright file="BinarySpectralGeometryPartitioning.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spatial.Partitioning
{
    /// <summary>
    /// Represents an operation performing binary partitioning of spectral geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::211501", "Binary geometry partitioning", "1.0.0", typeof(BinarySpectralGeometryPartitioningCredential))]
    public class BinarySpectralGeometryPartitioning : Operation<IGeometry, IGeometryCollection<IGeometry>>
    {
        #region Private types

        /// <summary>
        /// Represents the dimensions of a raster part.
        /// </summary>
        private class PartDimensions
        {
            #region Public properties

            /// <summary>
            /// Gets the index of the row.
            /// </summary>
            /// <value>
            /// The index of the row.
            /// </value>
            public Int32 RowIndex { get; set; }

            /// <summary>
            /// Gets the index of the column.
            /// </summary>
            /// <value>
            /// The index of the column.
            /// </value>
            public Int32 ColumnIndex { get; set; }

            /// <summary>
            /// Gets the number of rows.
            /// </summary>
            /// <value>
            /// The number of rows.
            /// </value>
            public Int32 NumberOfRows { get; set; }

            /// <summary>
            /// Gets the number of columns.
            /// </summary>
            /// <value>
            /// The number of columns.
            /// </value>
            public Int32 NumberOfColumns { get; set; }

            /// <summary>
            /// Gets the area of the raster.
            /// </summary>
            /// <value>The number of spectral values within the raster.</value>
            public Int32 Area { get { return NumberOfRows * NumberOfColumns; } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="PartDimensions" /> class.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="numberOfRows">The number of rows.</param>
            /// <param name="numberOfColumns">The number of columns.</param>
            public PartDimensions(Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns)
            {
                RowIndex = rowIndex;
                ColumnIndex = columnIndex;
                NumberOfRows = numberOfRows;
                NumberOfColumns = numberOfColumns;
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The source raster.
        /// </summary>
        private IRaster SourceRaster;

        /// <summary>
        /// A value indicating whether to preserve the metadata.
        /// </summary>
        private Boolean _metadataPreservation;

        /// <summary>
        /// The number of parts.
        /// </summary>
        private Int32 _numberOfParts;

        /// <summary>
        /// The number of values which should overlap for columns.
        /// </summary>
        private Int32 _overlapColumnMargin;

        /// <summary>
        /// The number of values which should overlap for rows.
        /// </summary>
        private Int32 _overlapRowMargin;

        /// <summary>
        /// The array of raster masks.
        /// </summary>
        private ISpectralGeometry[] _rasterMasks;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySpectralGeometryPartitioning"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentException">source;The specified source is not supported.</exception>
        public BinarySpectralGeometryPartitioning(IGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySpectralGeometryPartitioning"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentException">source;The specified source is not supported.</exception>
        public BinarySpectralGeometryPartitioning(IGeometry source, IGeometryCollection<IGeometry> target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, CommonOperationMethods.BinaryGeometryPartitioning, parameters)
        {
            if (!(Source is ISpectralGeometry))
                throw new ArgumentException("source", "The specified source is not supported.");

            SourceRaster = (source as ISpectralGeometry).Raster;
            _metadataPreservation = ResolveParameter<Boolean>(CommonOperationParameters.MetadataPreservation);
            _numberOfParts = Convert.ToInt32(ResolveParameter(CommonOperationParameters.NumberOfParts));

            // compute overlap of raster
            Double overlapMargin = Convert.ToDouble(ResolveParameter(CommonOperationParameters.OverlapMargin));

            if (overlapMargin == 0)
            {
                _overlapColumnMargin = _overlapRowMargin = 0;
            }
            else if ((Source as ISpectralGeometry).Raster.IsMapped)
            {
                _overlapColumnMargin = Convert.ToInt32(overlapMargin / (Source as ISpectralGeometry).Raster.Mapper.ColumnSize);
                _overlapRowMargin = Convert.ToInt32(overlapMargin / (Source as ISpectralGeometry).Raster.Mapper.RowSize);
            }
            else
            {
                _overlapColumnMargin = _overlapRowMargin = Convert.ToInt32(overlapMargin);
            }
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            // perform the partitioning using a queue containing the dimensions
            Queue<PartDimensions> mappingQueue = new Queue<PartDimensions>();
            mappingQueue.Enqueue(new PartDimensions(0, 0, SourceRaster.NumberOfRows, SourceRaster.NumberOfColumns));

            Int32 currentLevel = 0;

            while (mappingQueue.Count < _numberOfParts)
            {
                // tile the first element in the queue
                PartDimensions currentDimensions = mappingQueue.Dequeue();

                // check in which direction the tiling should be performed
                if (currentLevel % 2 == 0)
                {
                    mappingQueue.Enqueue(new PartDimensions(currentDimensions.RowIndex, currentDimensions.ColumnIndex, currentDimensions.NumberOfRows / 2, currentDimensions.NumberOfColumns));
                    mappingQueue.Enqueue(new PartDimensions(currentDimensions.RowIndex + currentDimensions.NumberOfRows / 2, currentDimensions.ColumnIndex, currentDimensions.NumberOfRows - currentDimensions.NumberOfRows / 2, currentDimensions.NumberOfColumns));
                }
                else
                {
                    mappingQueue.Enqueue(new PartDimensions(currentDimensions.RowIndex, currentDimensions.ColumnIndex, currentDimensions.NumberOfRows, currentDimensions.NumberOfColumns / 2));
                    mappingQueue.Enqueue(new PartDimensions(currentDimensions.RowIndex, currentDimensions.ColumnIndex + currentDimensions.NumberOfColumns / 2, currentDimensions.NumberOfRows, currentDimensions.NumberOfColumns - currentDimensions.NumberOfColumns / 2));
                }

                // if the number of parts in the kD-tree has reached the next level
                if (mappingQueue.Count == Calculator.Pow(2, currentLevel + 1))
                    currentLevel++;
            }

            // the partitioning only creates a mask for the specified raster

            _rasterMasks = new ISpectralGeometry[mappingQueue.Count];

            Int32 partIndex = 0;
            while (mappingQueue.Count > 0)
            {
                PartDimensions dimensions = mappingQueue.Dequeue();

                // compute overlap
                dimensions.ColumnIndex -= Math.Min(_overlapColumnMargin, dimensions.ColumnIndex);
                dimensions.NumberOfColumns += Math.Min(_overlapColumnMargin, dimensions.ColumnIndex);
                dimensions.NumberOfRows += Math.Min(_overlapRowMargin, dimensions.RowIndex);
                dimensions.RowIndex -= Math.Min(_overlapRowMargin, dimensions.RowIndex);
                dimensions.NumberOfColumns = Math.Min(dimensions.NumberOfColumns + _overlapColumnMargin, (Source as ISpectralGeometry).Raster.NumberOfColumns - dimensions.ColumnIndex);
                dimensions.NumberOfRows = Math.Min(dimensions.NumberOfRows + _overlapRowMargin, (Source as ISpectralGeometry).Raster.NumberOfRows - dimensions.RowIndex);

                // create mask
                _rasterMasks[partIndex] = Source.Factory.CreateSpectralPolygon(Source.Factory.GetFactory<ISpectralGeometryFactory>().GetFactory<IRasterFactory>().CreateMask(SourceRaster, dimensions.RowIndex, dimensions.ColumnIndex, dimensions.NumberOfRows, dimensions.NumberOfColumns), 
                                                                        (Source as ISpectralGeometry).Presentation, 
                                                                        _metadataPreservation ? (Source as ISpectralGeometry).Imaging : null, 
                                                                        _metadataPreservation ? Source.Metadata : null);
                partIndex++;
            }
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override IGeometryCollection<IGeometry> FinalizeResult()
        {
            return Source.Factory.CreateGeometryCollection<ISpectralGeometry>(_rasterMasks);
        }

        #endregion
    }
}
