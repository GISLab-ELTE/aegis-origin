/// <copyright file="DBaseField.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
using System.IO;
using System.Text;

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Represents a field of a dBase file.
    /// </summary>
    /// <remarks>
    /// The field represents one column of the dBase file, and describes the column name, type, length and decimal precision. 
    /// </remarks>
    internal class DBaseField
    {
        #region Private fields

        /// <summary>
        /// The field terminator. This field is constant.
        /// </summary>
        private const Byte FieldTerminator = (Byte)0x0D;

        /// <summary>
        /// The name of field (byte 0-10).
        /// </summary>
        private String _name;

        /// <summary>
        /// The type of the field (byte 11).
        /// </summary>
        private Char _type;

        /// <summary>
        /// The field address (byte 12-15).
        /// </summary>
        private Int32 _dataAddress;

        /// <summary>
        /// The length of the field (byte 16).
        /// </summary>
        private Int32 _length;

        /// <summary>
        /// The number of decimal places in the field (byte 17).
        /// </summary>
        private Int32 _decimalCount;

        /// <summary>
        /// The length of the name.
        /// </summary>
        private Int32 _nameLength;

        #endregion

        #region Public properties 

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public String FieldName { get { return _name; } }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <value>The type of the field.</value>
        public Char FieldType { get { return _type; } }

        /// <summary>
        /// Gets the length of the field.
        /// </summary>
        /// <value>The number of bytes in the field.</value>
        public Int32 FieldLength { get { return _length; } }

        /// <summary>
        /// Gets the decimal count of the field.
        /// </summary>
        /// <value>The number of decimal places in the field.</value>
        public Int32 DecimalCount { get { return _decimalCount; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts the dBase field to its binary representation.
        /// </summary>
        /// <returns>The byte array contained the field.</returns>
        public Byte[] ToByteArray()
        {
            Byte[] result = new Byte[32];

            Array.Copy(Encoding.ASCII.GetBytes(_name), result, _nameLength);

            result[11] = Convert.ToByte(_type);

            result[16] = (Byte)_length;
            result[17] = (Byte)_decimalCount;

            return result;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates the field from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The produced dBase field.</returns>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        public static DBaseField FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "The stream is null.");

            DBaseField result = new DBaseField();

            Byte[] buffer = new Byte[14];
            buffer[0] = (Byte)stream.ReadByte();

            if (buffer[0] == FieldTerminator) // we reached the end of the field descriptor
                return null;

            stream.Read(buffer, 1, 10);

            for (Int32 i = 0; i < 11; i++)
            {
                if (buffer[i] == (Byte)0)
                {
                    result._nameLength = i;
                    break;
                }
            }

            if (result._nameLength == 0)
                result._nameLength = 11;

            result._name = Encoding.ASCII.GetString(buffer, 0, result._nameLength);
            result._type = Convert.ToChar(stream.ReadByte());

            stream.Read(buffer, 0, 4);
            result._dataAddress = EndianBitConverter.ToInt32(buffer, 0, ByteOrder.LittleEndian);
            result._length = stream.ReadByte();
            result._decimalCount = stream.ReadByte();

            stream.Read(buffer, 0, 14);

            return result;
        }

        /// <summary>
        /// Creates the field from a metadata record name and value.
        /// </summary>
        /// <param name="name">The record name.</param>
        /// <param name="value">The record value.</param>
        /// <returns>The produced dDbase field.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The value is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The name is empty or consists only of white-space characters.
        /// or
        /// The length of the name is more than the maximum supported by the field (10).
        /// or
        /// The value is null.
        /// </exception>
        public static DBaseField FromRecord(String name, Object value)
        {
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The name is empty or consists only of white-space characters.", "name");
            if (name.Length > 10)
                throw new ArgumentException("The length of the name is more than the maximum supported by the field (10).", "name");
            if (value == null)
                throw new ArgumentNullException("value", "The value is null.");


            DBaseField result = new DBaseField();
            result._name = name;
            result._nameLength = name.Length;

            switch(Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Char:
                case TypeCode.String:
                    result._type = 'C';
                    result._decimalCount = 0;
                    result._length = 255;

                    break;
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    result._type = 'N';
                    result._decimalCount = 0;
                    result._length = 18;

                    break;
                case TypeCode.Single:
                case TypeCode.Double:
                    result._type = 'N';
                    result._decimalCount = 8;
                    result._length = 18;
 
                    break;
                case TypeCode.Boolean:
                    result._type = 'L';
                    result._decimalCount = 0;
                    result._length = 1;

                    break;
                case TypeCode.DateTime:
                    result._type = 'D';
                    result._decimalCount = 0;
                    result._length = 8;

                    break;
            }

            return result;
        }

        #endregion
    }
}
