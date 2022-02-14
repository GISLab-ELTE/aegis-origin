/// <copyright file="DBaseHeader.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Nagy</author>

using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represents a header of a dBase file.
    /// </summary>
    internal class DBaseHeader
    {
        #region Private fields

        /// <summary>
        /// The dBase III PLUS file type identifier. This field is constant.
        /// </summary>
        private const Byte DBaseIIIPlusIdentifier = (Byte)0x03;

        /// <summary>
        /// The field terminator of the header. This field is constant.
        /// </summary>
        private const Byte FieldTerminator = (Byte)0x0D;

        /// <summary>
        /// The table file type identifier (byte 0).
        /// </summary>
        private Byte _identifier;

        /// <summary>
        /// The timestamp of the last modification (bytes 1-3).
        /// </summary>
        private DateTime _lastUpdate;

        /// <summary>
        /// The number of records in the file (bytes 4-7).
        /// </summary>
        private Int32 _numOfRecords;

        /// <summary>
        /// The number of bytes in the file header (bytes 8-9).
        /// </summary>
        private Int16 _bytesInHeader;

        /// <summary>
        /// The number of bytes in the record (bytes 10-11).
        /// </summary>
        private Int16 _bytesInRecord;

        /// <summary>
        /// The array of field descriptors.
        /// </summary>
        private List<DBaseField> _fields;

        #endregion

        #region Public fields

        /// <summary>
        /// Gets the time of the last modification.
        /// </summary>
        /// <value>The time of the last modification.</value>
        public DateTime LastUpdate { get { return _lastUpdate; } }

        /// <summary>
        /// Gets the number of records.
        /// </summary>
        /// <value>The number of records in the file.</value>
        public Int32 NumOfRecords { get { return _numOfRecords; } }

        /// <summary>
        /// Gets the number of bytes in one record.
        /// </summary>
        /// <value>The number of bytes in one record.</value>
        public Int32 BytesInRecord { get { return _bytesInRecord; } }

        /// <summary>
        /// Gets The field description of 
        /// </summary>
        public IList<DBaseField> Fields { get { return _fields.AsReadOnly(); } }

        /// <summary>
        /// Gets the number of fields in the file.
        /// </summary>
        /// <value>The number of fields in the file.</value>
        public Int32 FieldCount { get { return _fields.Count; } }

        #endregion
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DBaseHeader"/> class.
        /// </summary>
        public DBaseHeader()
        {
            _identifier = DBaseIIIPlusIdentifier;
            _lastUpdate = DateTime.Now;
            _fields = new List<DBaseField>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBaseHeader"/> class.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.IO.InvalidDataException">The specified dBase format is not supported.</exception>
        public DBaseHeader(Stream stream) : this()
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "The stream is null.");

            Byte[] headerBuffer = new Byte[32];
            stream.Read(headerBuffer, 0, 32);

            _identifier = headerBuffer[0];

            if (_identifier != DBaseIIIPlusIdentifier && _identifier != (Byte)0x04)
                throw new InvalidDataException("The specified dBase format is not supported.");

            _lastUpdate = new DateTime(1900 + headerBuffer[1], headerBuffer[2], headerBuffer[3]);

            _numOfRecords = EndianBitConverter.ToInt32(headerBuffer, 4, ByteOrder.LittleEndian);
            _bytesInHeader = EndianBitConverter.ToInt16(headerBuffer, 8, ByteOrder.LittleEndian);
            _bytesInRecord = EndianBitConverter.ToInt16(headerBuffer, 10, ByteOrder.LittleEndian);

            Byte b = headerBuffer[29];

            DBaseField field = DBaseField.FromStream(stream);
            while (field != null)
            {
                _fields.Add(field);
                field = DBaseField.FromStream(stream);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes the header to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="sampleRecord">The sample record.</param>
        public void Write(Stream stream, IDictionary<String, Object> sampleRecord)
        {
            _bytesInRecord = 1;
            _fields = new List<DBaseField>();
            foreach (String key in sampleRecord.Keys)
            {
                DBaseField field = DBaseField.FromRecord(key, sampleRecord[key]);
                _fields.Add(field);
                _bytesInRecord += (Int16)field.FieldLength;
            }

            _numOfRecords = 0;
            _bytesInHeader = (Int16)(32 + (sampleRecord.Count * 32) + 1);

            Byte[] headerBytes = new Byte[_bytesInHeader];
            headerBytes[0] = _identifier; // write dBase ID
            headerBytes[1] = (Byte)(_lastUpdate.Year - 1900); // write last update time
            headerBytes[2] = (Byte)_lastUpdate.Month;
            headerBytes[3] = (Byte)_lastUpdate.Day;
            headerBytes[headerBytes.Length - 1] = FieldTerminator;

            EndianBitConverter.CopyBytes(_numOfRecords, headerBytes, 4, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_bytesInHeader, headerBytes, 8, ByteOrder.LittleEndian);
            EndianBitConverter.CopyBytes(_bytesInRecord, headerBytes, 10, ByteOrder.LittleEndian);

            Int32 headerIndex = 32;
            foreach (DBaseField field in _fields)
            {
                Array.Copy(field.ToByteArray(), 0, headerBytes, headerIndex, 32);
                headerIndex += 32;
            }

            stream.Write(headerBytes, 0, headerBytes.Length);
        }

        /// <summary>
        /// Writes the record count.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="recordCount">The number of the records.</param>
        public void WriteRecordCount(Stream stream, Int32 recordCount)
        {
            stream.Seek(4, SeekOrigin.Begin);
            stream.Write(EndianBitConverter.GetBytes(recordCount, ByteOrder.LittleEndian), 0, 4);
            stream.Seek(0, SeekOrigin.End);
        }

        #endregion
    }
}
