/// <copyright file="DBaseStreamWriter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Nagy</author>

using ELTE.AEGIS.IO.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represents a dBase format writer.
    /// </summary>
    public class DBaseStreamWriter : IDisposable
    {
        #region Private fields

        /// <summary>
        /// The encoding of the file. This field is read-only.
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// The path of the dBase file. This field is read-only.
        /// </summary>
        private readonly Uri _path;

        /// <summary>
        /// The stream of the dBase file.
        /// </summary>
        private Stream _stream;

        /// <summary>
        /// The file header.
        /// </summary>
        private DBaseHeader _header;

        /// <summary>
        /// A value indicating whether this instance is diposed.
        /// </summary>
        private Boolean _disposed;

        /// <summary>
        /// A value indicating whether the the file header is written to stream.
        /// </summary>
        private Boolean _headerWritten;

        /// <summary>
        /// The number of records in the stream.
        /// </summary>
        private Int32 _numberOfRecords;

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DBaseStreamWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public DBaseStreamWriter(String path) : this(path, Encoding.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBaseStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding of the file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The encoding is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public DBaseStreamWriter(String path, Encoding encoding)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (encoding == null)
                throw new ArgumentNullException("encoding", "The encoding is null.");


            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("The path is empty, or consists only of whitespace characters.", "path");
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out _path))
                throw new ArgumentException("The path is in an invalid format.", "path");

            try
            {
                if (_path.IsAbsoluteUri)
                    _stream = FileSystem.GetFileSystemForPath(_path).CreateFile(_path.AbsolutePath, true);
                else
                    _stream = FileSystem.GetFileSystemForPath(_path).CreateFile(_path.OriginalString, true);
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during stream opening.", ex);
            }

            _encoding = encoding;
            _header = new DBaseHeader();
            _headerWritten = false;
            _numberOfRecords = 0;
            _disposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DBaseStreamWriter"/> class.
        /// </summary>
        ~DBaseStreamWriter()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Writes the next record to the stream.
        /// </summary>
        /// <param name="record">The record to be written.</param>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream writing.</exception>
        public void Write(IDictionary<String,Object> record) 
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            try
            {
                if (!_headerWritten)
                {
                    _header.Write(_stream, record);
                    _headerWritten = true;
                }

                WriteRecord(record);
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during stream writing.", ex);
            }

            _numberOfRecords++;
        }

        /// <summary>
        /// Closes the writer and the underlying stream, and releases any system resources associated with the writer.
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            // there is only one case when at this point the header has not written, that is, when no records have been written in the file
            if (!_headerWritten)
            {
                // therefore an empty dictionary will be the sample record for the write method
                _header.Write(_stream, new Dictionary<String, Object>());
                _headerWritten = true;
            }

            _stream.Flush();

            if (disposing)
            {
                _stream.Close();
                _stream.Dispose();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Writes a record to the stream.
        /// </summary>
        /// <param name="record">The record to be written.</param>
        /// <exception cref="System.ArgumentNullException">The record is null.</exception>
        /// <exception cref="System.ArgumentException">The record contains incorrect number of fields.</exception>
        private void WriteRecord(IDictionary<String, Object> record)
        {
            if (record == null)
                throw new ArgumentNullException("record", "The record is null.");
            if (record.Count != _header.FieldCount)
                throw new ArgumentException("The record contains incorrect number of fields.", "record");

            _stream.WriteByte((Byte)0x20);
            
            foreach (DBaseField field in _header.Fields)
            {
                Byte[] fieldInBytes = new Byte[field.FieldLength];
                for (int i = 0; i < field.FieldLength; i++)
                    fieldInBytes[i] = (Byte)' ';

                switch(field.FieldType)
                {
                    case 'C':
                        String stringField = record[field.FieldName].ToString();
                        if (stringField == null)
                            stringField = " ";
                        if (stringField.Length > field.FieldLength)
                            stringField = stringField.Substring(0, field.FieldLength);

                        Array.Copy(_encoding.GetBytes(stringField), fieldInBytes, stringField.Length);
                        break;

                    case 'N':
                        String numericInString;
                        if (field.DecimalCount > 0)
                        {
                            Double numericRounded = Math.Round(Convert.ToDouble(record[field.FieldName]), field.DecimalCount);
                            numericInString = numericRounded.ToString(/*"R"*/);

                            String[] splitted = numericInString.Split('.');

                            if (splitted[0].Length > field.FieldLength - field.DecimalCount - 1)
                                splitted[0] = splitted[0].Substring(0, field.FieldLength - field.DecimalCount);

                            if (splitted.Count() > 1)
                            {
                                if (splitted[1].Length > field.DecimalCount)
                                    splitted[1] = splitted[1].Substring(0, field.DecimalCount);
                                numericInString = splitted[0] + '.' + splitted[1];
                                
                            }
                        }
                        else
                        {
                            numericInString = (Convert.ToInt64(record[field.FieldName])).ToString();
                            if (numericInString.Length > field.FieldLength)
                                numericInString = numericInString.Substring(0, field.FieldLength);
                        }

                        Byte[] numericInBytes = _encoding.GetBytes(numericInString);
                        Array.Copy(numericInBytes, fieldInBytes, numericInBytes.Length);
                        break;

                    case 'L':
                        
                        if ((Boolean)record[field.FieldName])
                            fieldInBytes[0] = (Byte)'T';
                        else
                            fieldInBytes[0] = (Byte)'F';
                        break;

                    case 'D':
                        DateTime date = (DateTime)record[field.FieldName];

                        if (date == null)
                            date = new DateTime();

                        Byte[] dateInBytes = _encoding.GetBytes(date.ToString("yyyyMMdd"));

                        Array.Copy(dateInBytes, fieldInBytes, dateInBytes.Length);
                        break;
                }

                _stream.Write(fieldInBytes, 0, field.FieldLength);
            }
        }

        #endregion
    }
}
