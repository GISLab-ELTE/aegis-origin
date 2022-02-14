using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
/// <copyright file="EhfaObject.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using ELTE.AEGIS.IO.ErdasImagine.Types;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Represents a primitive object in the HFA file.
    /// </summary>
    public class EhfaObject : IEhfaObject
    {
        #region Private fields

        /// <summary>
        /// The UNIX zero time. This field is read-only.
        /// </summary>
        private static readonly DateTime UnixTime = new DateTime(1970, 1, 1);

        /// <summary>
        /// The primitive object type. This field is read-only.
        /// </summary>
        private readonly EhfaPrimitiveType _type;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EhfaObject" /> class.
        /// </summary>
        /// <param name="type">The primitive object type.</param>
        /// <exception cref="ArgumentNullException">The type is null.</exception>
        public EhfaObject(EhfaPrimitiveType type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "The type is null.");

            _type = type;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the value of the <see cref="EhfaObject" />.
        /// </summary>
        /// <value>
        /// The value of the object.
        /// </value>
        public Object Value { get; private set; }

        /// <summary>
        /// Gets the type of the <see cref="EhfaObject" />.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public IEhfaObjectType Type { get { return _type; } }

        #endregion

        #region IEhfaObject methods

        /// <summary>
        /// Reads the contents of the <see cref="EhfaObject" /> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public virtual void Read(Stream stream)
        {
            Int32 size = ItemTypeInfo.TypeByteCount(Type.ItemType);

            if (Type.DataPlacement == DataPlacement.Internal)
            {
                if (Type.ItemType == ItemType.UChar || Type.ItemType == ItemType.Char)
                    size = _type.ItemNumber;

                Value = ReadValue(stream, size, Type.ItemType);
            }
            else
            {
                Byte[] temp = new Byte[8];
                stream.Read(temp, 0, temp.Length);
                Int32 repeatCount = (Int32)EndianBitConverter.ToUInt32(temp, ByteOrder.LittleEndian);

                if (repeatCount <= 0)
                    return;

                if (Type.ItemType == ItemType.UChar || Type.ItemType == ItemType.Char)
                {
                    size = repeatCount;
                    repeatCount = 1;
                }

                if (repeatCount > 1)
                    Value = ReadValues(stream, size, repeatCount, Type.ItemType);
                else
                    Value = ReadValue(stream, size, Type.ItemType);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the value of the object in the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>The value of the object.</returns>
        public T GetValue<T>()
        {
            return (T)Value;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads multiple values when location is specified.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objectSize">Object size.</param>
        /// <param name="repeatCount">The number of times the object repeats.</param>
        /// <param name="itemType">The item type.</param>
        /// <returns>The collection of values.</returns>
        private List<Object> ReadValues(Stream stream, Int32 objectSize, Int32 repeatCount, ItemType itemType)
        {
            List<Object> result = new List<Object>();
            while(repeatCount-- > 0)
            {
                Object value = ReadValue(stream, objectSize, itemType);
                // can return multiple elements (e.g. in case of U1 type).

                if (value is IEnumerable)
                {
                    result.AddRange((value as IEnumerable).Cast<Object>());
                }
                else
                    result.Add(value); 
            }
            return result;
        }

        /// <summary>
        /// Reads the value of the object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objectSize">Size of the object.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <returns>The value of the item.</returns>
        private Object ReadValue(Stream stream, Int32 objectSize, ItemType itemType)
        {
            Object value = null;
            Byte[] content = new Byte[objectSize];
            stream.Read(content, 0, objectSize);

            switch (itemType)
            {
                case ItemType.U1:
                    value = GetU1Values(content[0]);
                    break;
                case ItemType.U2:
                    value = GetU2Values(content[0]);
                    break;
                case ItemType.U4:
                    value = GetU4Values(content[0]);
                    break;
                case ItemType.UChar:
                    IEnumerable<Char> characters = content.Select(x => Convert.ToChar(Convert.ToSByte(x)));
                    value = new String(characters.ToArray().TakeWhile(x => x != '\0').ToArray());
                    break;
                case ItemType.Char:
                    value = new String(Encoding.UTF8.GetString(content).TakeWhile(x => x != '\0').ToArray());
                    break;
                case ItemType.Enum:
                    Int32 enumIndex = EndianBitConverter.ToUInt16(content, ByteOrder.LittleEndian);
                    value = _type.EnumItems[enumIndex];
                    break;
                case ItemType.UShort:
                    value = EndianBitConverter.ToUInt16(content, ByteOrder.LittleEndian);
                    break;
                case ItemType.Short:
                    value = EndianBitConverter.ToInt16(content, ByteOrder.LittleEndian);
                    break;
                case ItemType.Time:
                    UInt32 seconds = EndianBitConverter.ToUInt32(content, ByteOrder.LittleEndian);
                    value = UnixTime + TimeSpan.FromSeconds(seconds);
                    break;
                case ItemType.ULong:
                    value = EndianBitConverter.ToUInt32(content, ByteOrder.LittleEndian);
                    break;
                case ItemType.Long:
                    value = EndianBitConverter.ToInt32(content, ByteOrder.LittleEndian);
                    break;
                case ItemType.Float:
                    value = EndianBitConverter.ToSingle(content, ByteOrder.LittleEndian);
                    break;
                case ItemType.Double:
                    value = EndianBitConverter.ToDouble(content, ByteOrder.LittleEndian);
                    break;
                case ItemType.Complex:
                    Single real = EndianBitConverter.ToSingle(content, ByteOrder.LittleEndian);
                    Single imaginary = EndianBitConverter.ToSingle(content, 4, ByteOrder.LittleEndian);
                    value = new Complex(real, imaginary);
                    break;
                case ItemType.DComplex:
                    Double real2 = EndianBitConverter.ToDouble(content, ByteOrder.LittleEndian);
                    Double imaginary2 = EndianBitConverter.ToDouble(content, 8, ByteOrder.LittleEndian);
                    value = new Complex(real2, imaginary2);
                    break;
                case ItemType.BaseData:
                    value = GetBaseDataValue(stream);
                    break;
            }

            return value;
        }

        /// <summary>
        /// Returns the base-data value.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The base-data value.</returns>
        private BaseData GetBaseDataValue(Stream stream)
        {
            Byte[] buffer = new Byte[12];
            stream.Read(buffer, 0, 12);

            Int32 numberOfRows = EndianBitConverter.ToInt32(buffer, ByteOrder.LittleEndian);
            Int32 numberOfColumns = EndianBitConverter.ToInt32(buffer, 4, ByteOrder.LittleEndian);
            Int32 itemTypeIndex = EndianBitConverter.ToUInt16(buffer, 8, ByteOrder.LittleEndian);
            Int32 objectType = EndianBitConverter.ToUInt16(buffer, 10, ByteOrder.LittleEndian);

            ItemType itemType = ItemTypeInfo.ItemTypeFromIndex(itemTypeIndex);

            Object[,] values = new Object[numberOfRows, numberOfColumns];
            Int32 rowIndex = 0, columnIndex = 0;

            while (rowIndex < numberOfRows)
            {
                Object value = ReadValue(stream, ItemTypeInfo.TypeByteCount(itemType), itemType);
                IEnumerable<Object> enumerableValue = value as IEnumerable<Object>;

                if (enumerableValue != null)
                {
                    foreach (Object item in enumerableValue)
                    {
                        values[rowIndex, columnIndex] = item;
                        columnIndex++;

                        if (columnIndex == numberOfColumns)
                        {
                            columnIndex = 0;
                            rowIndex++;
                        }
                    }
                }
                else
                {
                    values[rowIndex, columnIndex] = value;
                    columnIndex++;

                    if (columnIndex == numberOfColumns)
                    {
                        columnIndex = 0;
                        rowIndex++;
                    }
                }
            }
            
            return new BaseData(itemType, values);
        }

        /// <summary>
        /// Returns the 1 bit unsigned integer values within the byte.
        /// </summary>
        /// <param name="content">The byte content.</param>
        /// <returns>The array containing 8 values.</returns>
        private Int32[] GetU1Values(Byte content)
        {
            Int32[] value = new Int32[8];

            value[0] = content & 0x1;
            value[1] = content & 0x1 << 1;
            value[2] = content & 0x1 << 2;
            value[3] = content & 0x1 << 3;
            value[4] = content & 0x1 << 4;
            value[5] = content & 0x1 << 5;
            value[6] = content & 0x1 << 6;
            value[7] = content & 0x1 << 7;

            return value;
        }

        /// <summary>
        /// Returns the 2 bit unsigned integer values within the byte.
        /// </summary>
        /// <param name="content">The byte content.</param>
        /// <returns>The array containing 4 values.</returns>
        private Int32[] GetU2Values(Byte content)
        {
            Int32[] value = new Int32[4];

            value[0] = content & 0x3;
            value[1] = content & 0x3 << 2;
            value[2] = content & 0x3 << 4;
            value[3] = content & 0x3 << 6;

            return value;
        }

        /// <summary>
        /// Returns the 4 bit unsigned integer values within the byte.
        /// </summary>
        /// <param name="content">The byte content.</param>
        /// <returns>The array containing 2 values.</returns>
        private Int32[] GetU4Values(Byte content)
        {
            Int32[] value = new Int32[2];

            value[0] = content & 0xf;
            value[1] = content & 0xf << 4;

            return value;
        }

        #endregion
    }
}
