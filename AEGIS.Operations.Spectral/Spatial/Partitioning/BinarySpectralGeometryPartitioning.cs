/// <copyright file="BinarySpectralGeometryPartitioning.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial.Partitioning
{
    /// <summary>
    /// Represents an operation performing binary partitioning of spectral geometries.
    /// </summary>
    [OperationMethodImplementation("AEGIS::211501", "Binary geometry partitioning", "1.0.0", typeof(BinarySpectralGeometryPartitioningCredential))]
    public class BinarySpectralGeometryPartitioning : Operation<IGeometry, IGeometryCollection<IGeometry>>
    {
        #region Private types

        #endregion

        #region Private fields

        /// <summary>
        /// The source raster.
        /// </summary>
        private IRaster _sourceRaster;

        /// <summary>
        /// The array of resulting geometries.
        /// </summary>
        private ISpectralGeometry[] _resultGeometries;

        /// <summary>
        /// A value indicating whether to preserve the metadata.
        /// </summary>
        private Boolean _metadataPreservation;

        /// <summary>
        /// The number of parts.
        /// </summary>
        private Int32 _numberOfParts;

        /// <summary>
        /// The size of the buffer area with respect to columns.
        /// </summary>
        private Int32 _bufferColumnCount;

        /// <summary>
        /// The size of the buffer area with respect to rows.
        /// </summary>
        private Int32 _bufferRowCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySpectralGeometryPartitioning" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentException">source;The specified source is not supported.</exception>
        public BinarySpectralGeometryPartitioning(IGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySpectralGeometryPartitioning" /> class.
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

            _sourceRaster = (source as ISpectralGeometry).Raster;
            _metadataPreservation = ResolveParameter<Boolean>(CommonOperationParameters.MetadataPreservation);
            _numberOfParts = Convert.ToInt32(ResolveParameter(CommonOperationParameters.NumberOfParts));

            // compute overlap of raster
            Double overlapAreaSize = Convert.ToDouble(ResolveParameter(CommonOperationParameters.BufferAreaSize));

            if ((Source as ISpectralGeometry).Raster.IsMapped)
            {
                _bufferColumnCount = Convert.ToInt32(Math.Ceiling(overlapAreaSize / (Source as ISpectralGeometry).Raster.Mapper.ColumnSize));
                _bufferRowCount = Convert.ToInt32(Math.Ceiling(overlapAreaSize / (Source as ISpectralGeometry).Raster.Mapper.RowSize));
            }

            Int32 overlapPixelCount = Convert.ToInt32(ResolveParameter(CommonOperationParameters.BufferValueCount));

            _bufferColumnCount = Math.Max(_bufferColumnCount, overlapPixelCount);
            _bufferRowCount = Math.Max(_bufferRowCount, overlapPixelCount);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            // perform the partitioning using a queue containing the sections
            Queue<RasterSection> sectionQueue = new Queue<RasterSection>();
            sectionQueue.Enqueue(new RasterSection(0, 0, _sourceRaster.NumberOfRows, _sourceRaster.NumberOfColumns));

            Int32 currentLevel = 0;

            while (sectionQueue.Count < _numberOfParts)
            {
                // tile the first element in the queue
                RasterSection currentDimensions = sectionQueue.Dequeue();

                // check in which direction the tiling should be performed
                if (currentLevel % 2 == 0)
                {
                    sectionQueue.Enqueue(new RasterSection(currentDimensions.RowIndex, currentDimensions.ColumnIndex, currentDimensions.NumberOfRows / 2, currentDimensions.NumberOfColumns));
                    sectionQueue.Enqueue(new RasterSection(currentDimensions.RowIndex + currentDimensions.NumberOfRows / 2, currentDimensions.ColumnIndex, currentDimensions.NumberOfRows - currentDimensions.NumberOfRows / 2, currentDimensions.NumberOfColumns));
                }
                else
                {
                    sectionQueue.Enqueue(new RasterSection(currentDimensions.RowIndex, currentDimensions.ColumnIndex, currentDimensions.NumberOfRows, currentDimensions.NumberOfColumns / 2));
                    sectionQueue.Enqueue(new RasterSection(currentDimensions.RowIndex, currentDimensions.ColumnIndex + currentDimensions.NumberOfColumns / 2, currentDimensions.NumberOfRows, currentDimensions.NumberOfColumns - currentDimensions.NumberOfColumns / 2));
                }

                // if the number of parts in the kD-tree has reached the next level
                if (sectionQueue.Count == Calculator.Pow(2, currentLevel + 1))
                    currentLevel++;
            }

            // update buffer size based on part size
            _bufferColumnCount = Math.Min(_bufferColumnCount, sectionQueue.Max(section => section.NumberOfColumns) / 2);
            _bufferRowCount = Math.Min(_bufferRowCount, sectionQueue.Max(section => section.NumberOfRows) / 2);

            // the partitioning only creates a mask for the specified raster

            RasterSection[] sectionsOverlappingParts = new RasterSection[sectionQueue.Count];
            RasterSection[][] sectionsWithinParts = new RasterSection[sectionQueue.Count][];
            _resultGeometries = new ISpectralGeometry[sectionQueue.Count];

            Int32 partIndex = 0;
            while (sectionQueue.Count > 0)
            {
                RasterSection section = sectionQueue.Dequeue();

                // compute overlap
                Int32 lowColumnDelta = 0, highColumnDelta = 0, lowRowDelta = 0, highRowDelta = 0;
                if (section.ColumnIndex > 0)
                {
                    lowColumnDelta = Math.Min(_bufferColumnCount / 2, section.ColumnIndex);
                    section.ColumnIndex -= lowColumnDelta;
                    section.NumberOfColumns += lowColumnDelta;
                }
                if (section.RowIndex > 0)
                {
                    lowRowDelta = Math.Min(_bufferRowCount / 2, section.RowIndex);
                    section.RowIndex -= lowRowDelta;
                    section.NumberOfRows += lowRowDelta;
                }
                if (section.ColumnEndIndex < (Source as ISpectralGeometry).Raster.NumberOfColumns)
                {
                    highColumnDelta = _bufferColumnCount / 2;
                    section.NumberOfColumns += highColumnDelta;
                }
                if (section.RowEndIndex < (Source as ISpectralGeometry).Raster.NumberOfRows)
                {
                    highRowDelta = _bufferRowCount / 2;
                    section.NumberOfRows += highRowDelta;
                }

                // create mask
                _resultGeometries[partIndex] = Source.Factory.CreateSpectralPolygon(Source.Factory.GetFactory<ISpectralGeometryFactory>().GetFactory<IRasterFactory>().CreateMask(_sourceRaster, section.RowIndex, section.ColumnIndex, section.NumberOfRows, section.NumberOfColumns), 
                                                                        (Source as ISpectralGeometry).Presentation, 
                                                                        _metadataPreservation ? (Source as ISpectralGeometry).Imaging : null, 
                                                                        _metadataPreservation ? Source.Metadata : null);

                sectionsOverlappingParts[partIndex] = section;

                // recompute sections (sections are in row-major order within the part)
                sectionsWithinParts[partIndex] = new RasterSection[]
                {
                    new RasterSection(section.RowIndex, section.ColumnIndex, lowRowDelta * 2, lowColumnDelta * 2),
                    new RasterSection(section.RowIndex, section.ColumnIndex + lowColumnDelta * 2, lowRowDelta * 2, section.NumberOfColumns - lowColumnDelta * 2 - highColumnDelta * 2),
                    new RasterSection(section.RowIndex, section.ColumnEndIndex - highColumnDelta * 2, lowRowDelta * 2, highColumnDelta * 2),
                    new RasterSection(section.RowIndex + lowRowDelta * 2, section.ColumnIndex, section.NumberOfRows - lowRowDelta * 2 - highRowDelta * 2, lowColumnDelta * 2),
                    new RasterSection(section.RowIndex + lowRowDelta * 2, section.ColumnIndex + lowColumnDelta * 2, section.NumberOfRows - lowRowDelta * 2 - highRowDelta * 2, section.NumberOfColumns - lowColumnDelta * 2 - highColumnDelta * 2),
                    new RasterSection(section.RowIndex + lowRowDelta * 2, section.ColumnEndIndex - highColumnDelta * 2, section.NumberOfRows - lowRowDelta * 2 - highRowDelta * 2, highColumnDelta * 2),
                    new RasterSection(section.RowEndIndex - highRowDelta * 2, section.ColumnIndex, highRowDelta * 2, lowColumnDelta * 2),
                    new RasterSection(section.RowEndIndex - highRowDelta * 2, section.ColumnIndex + lowColumnDelta * 2, highRowDelta * 2, section.NumberOfColumns - lowColumnDelta * 2 - highColumnDelta * 2),
                    new RasterSection(section.RowEndIndex - highRowDelta * 2, section.ColumnEndIndex - highColumnDelta * 2, highRowDelta * 2, highColumnDelta * 2)
                };

                partIndex++;
            }

            // create raster section map
            RasterSectionMap rasterSectionMap = new RasterSectionMap();
            foreach (RasterSection[] partSections in sectionsWithinParts)
                foreach (RasterSection section in partSections)
                {
                    rasterSectionMap.AddSection(section);
                }

            // create part section maps
            for (partIndex = 0; partIndex < sectionsWithinParts.Length; partIndex++)
            {
                RasterSectionMap tileSectionMap = new RasterSectionMap(rasterSectionMap);

                foreach (RasterSection[] partSections in sectionsWithinParts)
                    foreach (RasterSection section in partSections)
                    {
                        if (sectionsOverlappingParts[partIndex].Contains(section))
                            tileSectionMap.AddTileSection(section);
                    }

                // set the section map
                _resultGeometries[partIndex]["AEGIS.RasterSectionMap"] = tileSectionMap.ToString();
            }
        }

        /// <summary>
        /// Finalizes the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override IGeometryCollection<IGeometry> FinalizeResult()
        {
            return Source.Factory.CreateGeometryCollection<ISpectralGeometry>(_resultGeometries);
        }

        #endregion
    }
}
