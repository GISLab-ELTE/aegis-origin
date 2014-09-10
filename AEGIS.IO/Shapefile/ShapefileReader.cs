/// <copyright file="ShapefileReader.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Storage;
using ELTE.AEGIS.IO.WellKnown;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represents a shapefile format reader.
    /// </summary>    
    /// <remarks>
    /// The shapefile format is an interchange format for simple vector data.
    /// The format consinst of multiple files within the same directory. The main file are the vector file (shp), sptial index file (shx) and attribute dBase table (dbf).
    /// </remarks>
    [IdentifiedObjectInstance("AEGIS::610101", "Esri shapefile")]
    public class ShapefileReader : GeometryStreamReader
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
            public Int32 Offset {get; set; }

            /// <summary>
            /// The length of the record.
            /// </summary>
            public Int32 Length { get; set; }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The base path.
        /// </summary>
        private readonly String _basePath;

        /// <summary>
        /// The base file name.
        /// </summary>
        private readonly String _baseFileName;

        /// <summary>
        /// The file system of the path.
        /// </summary>
        private readonly FileSystem _fileSystem;

        /// <summary>
        /// The type of shapes in the file.
        /// </summary>
        private ShapeType _shapeType;

        /// <summary>
        /// The index of the shapes in the file.
        /// </summary>
        private ShapeRecordInfo[] _shapeIndex;

        /// <summary>
        /// The reference system of the file.
        /// </summary>
        private IReferenceSystem _referenceSystem;

        /// <summary>
        /// The envelope of the file.
        /// </summary>
        private Envelope _envelope;

        /// <summary>
        /// The metadata reader.
        /// </summary>
        private DBaseStreamReader _metadataReader;

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
        /// Initializes a new instance of the <see cref="ShapefileReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public ShapefileReader(String path) : base(path, GeometryStreamFormats.Shapefile, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path);

            try
            {
                ReadHeader();
                ReadReferenceSystem();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentReadError, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream reading.
        /// </exception>
        public ShapefileReader(Uri path)
            : base(path, GeometryStreamFormats.Shapefile, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path.AbsolutePath);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path.AbsolutePath);

            try
            {
                ReadHeader();
                ReadReferenceSystem();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentReadError, ex);
            }
        }

        #endregion

        #region GeometryStreamReader protected methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise, <c>false</c>.</returns>
        protected override Boolean GetEndOfStream()
        {
            return _baseStream.Position == _baseStream.Length;
        }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        protected override IGeometry ApplyReadGeometry()
        {
            Int32 recordNumber, recordLength;
            Byte[] recordHeaderBytes = new Byte[8];
            Byte[] recordBytes;
            Dictionary<String, Object> metadata = null;

            // read record header
            _baseStream.Read(recordHeaderBytes, 0, recordHeaderBytes.Length);

            recordNumber = EndianBitConverter.ToInt32(recordHeaderBytes, 0, ByteOrder.BigEndian);
            recordLength = EndianBitConverter.ToInt32(recordHeaderBytes, 4, ByteOrder.BigEndian) * 2;

            recordBytes = new Byte[recordLength];
            // read record content
            _baseStream.Read(recordBytes, 0, recordBytes.Length);

            // read metadata
            if (_fileSystem.Exists(MetadataFilePath))
                metadata = new Dictionary<String, Object>(_metadataReader.Read()) ?? new Dictionary<String, Object>();

            // convert shape

            return Shape.FromRecord(recordNumber, recordBytes, ResolveFactory(_referenceSystem), metadata).ToGeometry();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_metadataReader != null)
                {
                    _metadataReader.Dispose();
                    _metadataReader = null;
                }

                _shapeIndex = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Readsthe file header.
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException">Stream content is in an invalid format.</exception>
        private void ReadHeader()
        {
            _baseStream.Seek(0, SeekOrigin.Begin);
            Byte[] headerBytes = new Byte[100];
            _baseStream.Read(headerBytes, 0, headerBytes.Length);

            // perform checks for header
            if (EndianBitConverter.ToInt32(headerBytes, 0, ByteOrder.BigEndian) != 9994 || // identifier 
                EndianBitConverter.ToInt32(headerBytes, 24, ByteOrder.BigEndian) != _baseStream.Length / 2 || // file length
                EndianBitConverter.ToInt32(headerBytes, 28, ByteOrder.LittleEndian) != 1000) // version
            {
                throw new InvalidDataException(MessageHeaderInvalid);
            }

            // shape type
            _shapeType = (ShapeType)EndianBitConverter.ToInt32(headerBytes, 32, ByteOrder.LittleEndian);

            // envelope
            Double xMin = EndianBitConverter.ToDouble(headerBytes, 36);
            Double yMin = EndianBitConverter.ToDouble(headerBytes, 44);
            Double xMax = EndianBitConverter.ToDouble(headerBytes, 52);
            Double yMax = EndianBitConverter.ToDouble(headerBytes, 60);
            Double zMin = EndianBitConverter.ToDouble(headerBytes, 68);
            Double zMax = EndianBitConverter.ToDouble(headerBytes, 76);

            _envelope = new Envelope(xMin, xMax, yMin, yMax, zMin, zMax);

            // load metadata for shapes
            if (_fileSystem.Exists(MetadataFilePath))
            {
                _metadataReader = new DBaseStreamReader(MetadataFilePath);
            }
        }

        /// <summary>
        /// Reads the reference system.
        /// </summary>
        private void ReadReferenceSystem()
        {
            // read the reference system from the WKT text
            using (StreamReader reader = new StreamReader(ReferenceSystemFilePath))
            {
                StringBuilder builder = new StringBuilder();
                while (!reader.EndOfStream)
                    builder.Append(reader.ReadLine().Trim());

                _referenceSystem = IdentifiedObjectConverter.ToReferenceSystem(builder.ToString());
            }
        }

        /// <summary>
        /// Reads the shape index.
        /// </summary>
        private void ReadShapeIndex()
        {
            try
            {
                using (Stream stream = _fileSystem.OpenFile(ShapeIndexFilePath, FileMode.Open))
                {
                    Byte[] headerBytes = new Byte[100];
                    stream.Read(headerBytes, 0, headerBytes.Length); // read header

                    // check header content
                    if (EndianBitConverter.ToInt32(headerBytes, 0, ByteOrder.BigEndian) != 9994 || // identifier 
                        EndianBitConverter.ToInt32(headerBytes, 24, ByteOrder.BigEndian) != stream.Length / 2 || // file length
                        EndianBitConverter.ToInt32(headerBytes, 28, ByteOrder.LittleEndian) != 1000) // version
                    {
                        return;
                    }

                    Byte[] shapeIndexBytes = new Byte[stream.Length - 100];
                    stream.Read(shapeIndexBytes, 0, (Int32)stream.Length - 100); // read indices

                    _shapeIndex = new ShapeRecordInfo[(stream.Length - 100) / 8];
                    for (Int32 i = 0; i < _shapeIndex.Length; i++)
                    {
                        _shapeIndex[i].Offset = EndianBitConverter.ToInt32(shapeIndexBytes, 0, ByteOrder.BigEndian) * 2;
                        _shapeIndex[i].Length = EndianBitConverter.ToInt32(shapeIndexBytes, 4, ByteOrder.BigEndian) * 2;
                    }
                }
            }
            catch
            {
                _shapeIndex = null;
            }
        }

        #endregion
    }
}
