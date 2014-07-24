/// <copyright file="DBaseStreamReader.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Globalization;
using System.IO;
using System.Text;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represents a dBase format reader.
    /// </summary>
    public class DBaseStreamReader : IDisposable
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

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DBaseStreamReader"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream opening.</exception>
        public DBaseStreamReader(String path) : this(path, Encoding.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBaseStreamReader" /> class.
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
        /// <exception cref="System.IO.IOException">Exception occured during stream opening.</exception>
        public DBaseStreamReader(String path, Encoding encoding)
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
                _stream = FileSystem.GetFileSystemForPath(_path).OpenFile(_path.LocalPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream opening.", ex);
            }

            _encoding = encoding;
            _header = new DBaseHeader(_stream);
            _disposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DBaseStreamReader"/> class.
        /// </summary>
        ~DBaseStreamReader()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reads the next record from the stream.
        /// </summary>
        /// <returns>The dictionary of key/value pairs representing the record read from the stream.</returns>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream reading.</exception>
        /// <exception cref="System.IO.InvalidDataException">Stream content is invalid.</exception>
        public IDictionary<String, Object> Read() 
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            Byte[] recordBytes = new Byte[_header.BytesInRecord];

            try
            {
                Int32 readBytes = _stream.Read(recordBytes, 0, _header.BytesInRecord);

                if (recordBytes[0] == (Byte)0x1A || readBytes == 0) // end of file
                    return null;
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream reading.", ex);
            }

            return ParseRecord(recordBytes);
        }

        /// <summary>
        /// Closes the reader and the underlying stream, and releases any system resources associated with the reader.
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

            if (disposing)
            {
                _stream.Dispose();
                _stream = null;

                _header = null;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// PArses the byte array into a record.
        /// </summary>
        /// <param name="recordBytes">The byte array.</param>
        /// <returns>The dictionary of key/value pairs representing the record read from the byte array.</returns>
        /// <exception cref="System.IO.InvalidDataException">Stream content is invalid.</exception>
        private IDictionary<String, Object> ParseRecord(Byte[] recordBytes)
        {
            Dictionary<String, Object> result = new Dictionary<String, Object>();

            // data records are preceded by one byte, that is a space (0x20) if the record is not deleted, 
            // or an asterisk (0x2A) if the record is deleted
            if (recordBytes[0] == (Byte)0x2A)
                return result;

            if (recordBytes[0] != (Byte)0x20)
                throw new InvalidDataException("Stream content is invalid.");

            Int32 index = 1;

            try
            {
                foreach (DBaseField field in _header.Fields)
                {
                    switch (field.FieldType)
                    {
                        case 'C':
                            String str = _encoding.GetString(recordBytes, index, field.FieldLength).Trim();
                            result.Add(field.FieldName, str);
                            index += field.FieldLength;
                            break;

                        case 'D':
                            String s = Encoding.ASCII.GetString(recordBytes, index, 8);

                            Int32 year = Convert.ToInt32(s.Substring(0, 4));
                            Int16 month = Convert.ToInt16(s.Substring(4, 2));
                            Int16 day = Convert.ToInt16(s.Substring(6, 2));

                            DateTime date = new DateTime(year, month, day);

                            index += 8;
                            result.Add(field.FieldName, date);
                            break;

                        case 'N':
                            Byte[] numericEncoded = new Byte[field.FieldLength];
                            Array.Copy(recordBytes, index, numericEncoded, 0, field.FieldLength);
                            String numericInString = Encoding.ASCII.GetString(numericEncoded);

                            Boolean intParseFailed = false;
                            if (field.DecimalCount == 0)
                            {
                                Int32 decodedInt;
                                if (Int32.TryParse(numericInString, out decodedInt))
                                    result.Add(field.FieldName, decodedInt);
                                else
                                    intParseFailed = true;
                            }

                            if (field.DecimalCount > 0 || intParseFailed)
                            {
                                Double decodedDouble;
                                Double.TryParse(numericInString, NumberStyles.Any,
                                    CultureInfo.InvariantCulture, out decodedDouble);

                                result.Add(field.FieldName, decodedDouble);
                            }

                            index += field.FieldLength;
                            break;

                        case 'F':
                            Byte[] encodedFloat = new Byte[field.FieldLength];
                            Array.Copy(recordBytes, index, encodedFloat, 0, field.FieldLength);
                            String floatInString = Encoding.ASCII.GetString(encodedFloat);

                            Double decodedFloat;
                            Double.TryParse(floatInString, NumberStyles.Any, CultureInfo.InvariantCulture, out decodedFloat);

                            result.Add(field.FieldName, decodedFloat);

                            index += field.FieldLength;
                            break;

                        case 'L':
                            Byte logicalEncoded = recordBytes[index];

                            Boolean decoded = (logicalEncoded == 'Y' || logicalEncoded == 'y'
                                                || logicalEncoded == 'T' || logicalEncoded == 't');

                            result.Add(field.FieldName, decoded);

                            index++;

                            break;

                        default:
                            throw new InvalidDataException("Stream content is invalid.");
                    }
                }
            }
            catch (InvalidDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Stream content is invalid.", ex);
            }

            return result;
        }

        #endregion
    }
}
