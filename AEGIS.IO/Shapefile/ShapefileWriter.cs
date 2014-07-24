/// <copyright file="ShapefileWriter.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Storage;
using ELTE.AEGIS.IO.WellKnown;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represens a shapefile format writer.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::610101", "Esri shapefile")]
    public class ShapefileWriter : GeometryStreamWriter
    {
        #region Private types

        /// <summary>
        /// Contains information abount a shape record.
        /// </summary>
        private struct ShapeRecordInfo
        {
            /// <summary>
            /// The offset of the record.
            /// </summary>
            public Int32 Offset;

            /// <summary>
            /// The length of the record.
            /// </summary>
            public Int32 Length;
        }

        #endregion

        #region Private fields

        private readonly String _basePath;
        private readonly String _baseFileName;
        private readonly FileSystem _fileSystem;

        private ShapeType _shapeType;
        private List<ShapeRecordInfo> _shapeIndex;
        private IReferenceSystem _referenceSystem;
        private Envelope _envelope;
        private GeometryModel _geometryModel;

        private DBaseStreamWriter _metadataWriter;

        #endregion

        #region Private properties

        /// <summary>
        /// Gets the shape index file path.
        /// </summary>
        /// <value>The path of the shape index file.</value>
        private String ShapeIndexFilePath { get { return _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".shx"; } }

        /// <summary>
        /// Gets the spatial index file path.
        /// </summary>
        /// <value>The path of the spatial index file.</value>
        private String SpatialIndexFilePath { get { return _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".sbn"; } }

        /// <summary>
        /// Gets the metadata file path.
        /// </summary>
        /// <value>The path of the metadata file.</value>
        private String MetadataFilePath { get { return _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".dbf"; } }

        /// <summary>
        /// Gets the reference system file path.
        /// </summary>
        /// <value>The path of the reference system file.</value>
        private String ReferenceSystemFilePath { get { return _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".prj"; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream writing.
        /// </exception>
        public ShapefileWriter(String path) : base(path, GeometryStreamFormats.Shapefile, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path);

            _shapeType = ShapeType.Null;
            _shapeIndex = new List<ShapeRecordInfo>();

            _geometryModel = GeometryModel.None;
            _envelope = Envelope.Undefined;

            try
            {
                _metadataWriter = new DBaseStreamWriter(MetadataFilePath);
                _baseStream.Write(new Byte[100], 0, 100); // write empty header 
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream writing.", ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream writing.
        /// </exception>
        public ShapefileWriter(Uri path) : base(path, GeometryStreamFormats.Shapefile, null)
        {
            _baseStream.Seek(100, SeekOrigin.Begin);

            _basePath = _fileSystem.GetDirectory(path.AbsolutePath);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path.AbsolutePath);
            _shapeType = ShapeType.Null;

            try
            {
                _metadataWriter = new DBaseStreamWriter(MetadataFilePath);
                _baseStream.Write(new Byte[100], 0, 100); // write empty header 
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream writing.", ex);
            }
        }

        #endregion

        #region GeometryStreamWriter protected methods

        /// <summary>
        /// Apply the write operation for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentException">
        /// The model of the specified geometry is not supported.;geometry
        /// or
        /// The specified geometry does not match the the type of the shapefile.;geometry
        /// </exception>
        protected override void ApplyWriteGeometry(IGeometry geometry)
        {
            // check the model
            if (_geometryModel == GeometryModel.None)
            {
                switch (geometry.GeometryModel)
                { 
                    case GeometryModel.Spatial2D:
                    case GeometryModel.SpatioTemporal2D:
                        _geometryModel = GeometryModel.Spatial2D;
                        break;
                    case GeometryModel.Spatial3D:
                    case GeometryModel.SpatioTemporal3D:
                        _geometryModel = GeometryModel.Spatial3D;
                        break;
                    default:
                        throw new ArgumentException("The model of the specified geometry is not supported.", "geometry");
                }
            }

            Shape shape = new Shape(geometry);

            // check the shape type
            if (_shapeType == ShapeType.Null)
            {
                _shapeType = shape.Type;
            }
            else if (_shapeType != shape.Type)
            {
                throw new ArgumentException("The specified geometry does not match the the type of the shapefile.", "geometry");
            }

            // check the reference system
            if (_referenceSystem == null)
            {
                _referenceSystem = geometry.ReferenceSystem;
            }

            // compute the envelope
            if (_envelope == null)
                _envelope = geometry.Envelope;
            else if (!_envelope.Contains(geometry.Envelope))
                _envelope = Envelope.FromEnvelopes(_envelope, geometry.Envelope);

            Byte[] byteArray = shape.ToRecord(_shapeIndex.Count);

            _shapeIndex.Add(new ShapeRecordInfo { Offset = (Int32)_baseStream.Position, Length = byteArray.Length });

            IDictionary<String, Object> metadata = geometry.Metadata;
            _metadataWriter.Write(metadata ?? new Dictionary<String, Object>());

            _baseStream.Write(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            // write information before closing the file
            WriteHeader();
            WriteShapeIndex();
            WriteReferenceSystem();

            if (disposing)
            {
                _metadataWriter.Dispose();
                _shapeIndex = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes the file header.
        /// </summary>
        private void WriteHeader()
        {
            Byte[] headerBytes = new Byte[100];

            // header identifiers
            EndianBitConverter.CopyBytes(9994, headerBytes, 0, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes((100 + _shapeIndex.Sum(recordInfo => recordInfo.Length)) / 2, headerBytes, 24, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes(1000, headerBytes, 28, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes((Int32)_shapeType, headerBytes, 32, ByteOrder.LittleEndian);

            // envelope
            EndianBitConverter.CopyBytes(_envelope.MinX, headerBytes, 36, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_envelope.MinY, headerBytes, 44, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_envelope.MaxX, headerBytes, 52, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_envelope.MaxY, headerBytes, 60, ByteOrder.LittleEndian);
            if (_geometryModel == GeometryModel.Spatial3D || _geometryModel == GeometryModel.SpatioTemporal3D)
            {
                EndianBitConverter.CopyBytes(_envelope.MinZ, headerBytes, 68, ByteOrder.LittleEndian);
                EndianBitConverter.CopyBytes(_envelope.MaxZ, headerBytes, 76, ByteOrder.LittleEndian);
            }

            _baseStream.Seek(0, SeekOrigin.Begin);
            _baseStream.Write(headerBytes, 0, headerBytes.Length);
        }

        /// <summary>
        /// Writes the shape index.
        /// </summary>
        private void WriteShapeIndex()
        {
            Byte[] byteArray = new Byte[100 + _shapeIndex.Count * 8];

            // header identifiers
            EndianBitConverter.CopyBytes(9994, byteArray, 0, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes(byteArray.Length / 2, byteArray, 24, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes(1000, byteArray, 28, ByteOrder.BigEndian);
            EndianBitConverter.CopyBytes((Int32)_shapeType, byteArray, 32, ByteOrder.LittleEndian);

            // envelope
            EndianBitConverter.CopyBytes(_envelope.MinX, byteArray, 36, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_envelope.MinY, byteArray, 44, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_envelope.MaxX, byteArray, 52, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_envelope.MaxY, byteArray, 60, ByteOrder.LittleEndian);
            if (_geometryModel == GeometryModel.Spatial3D || _geometryModel == GeometryModel.SpatioTemporal3D)
            {
                EndianBitConverter.CopyBytes(_envelope.MinZ, byteArray, 68, ByteOrder.LittleEndian);
                EndianBitConverter.CopyBytes(_envelope.MaxZ, byteArray, 76, ByteOrder.LittleEndian);
            }

            // indices
            for (Int32 i = 0; i < _shapeIndex.Count; i++)
            {
                EndianBitConverter.CopyBytes(_shapeIndex[i].Offset / 2, byteArray, 100 + 8 * i, ByteOrder.BigEndian);
                EndianBitConverter.CopyBytes(_shapeIndex[i].Length / 2, byteArray, 104 + 8 * i, ByteOrder.BigEndian);
            }

            // write bytes to stream
            using (Stream stream = _fileSystem.CreateFile(ShapeIndexFilePath))
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }
        }

        /// <summary>
        /// Writes the reference system.
        /// </summary>
        private void WriteReferenceSystem()
        {
            // writes the reference system as WKT text
            using (StreamWriter writer = new StreamWriter(ReferenceSystemFilePath))
            {
                String text = IdentifiedObjectConverter.ToWellKnownText(_referenceSystem);
                writer.WriteLine(text);
            }
        }

        #endregion
    }
}
