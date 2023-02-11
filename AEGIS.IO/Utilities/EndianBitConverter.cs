// <copyright file="EndianBitConverter.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS.IO
{
    /// <summary>
    /// Converts base data types to an array of bytes, and an array of bytes to base data types with respect to byte-order.
    /// </summary>
    public static class EndianBitConverter
    {
        #region Public static fields

        /// <summary>
        /// Gets the system default byte order.
        /// </summary>
        /// <value>The system default byte order.</value>
        public static readonly ByteOrder DefaultByteOrder = GetDefaultByteOrder();

        #endregion

        #region Public static methods for converting from bytes

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit signed integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Int16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt16Unchecked(array);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two byte at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt16(array, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit signed integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Int16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt16Unchecked(array, order);
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int16 ToInt16(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt16Unchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit signed integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt32Unchecked(array);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt32Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit signed integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt32Unchecked(array, order);
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int32 ToInt32(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToInt32Unchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Int64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt64Unchecked(array);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt64Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Int64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToInt64Unchecked(array, order);
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Int64 ToInt64(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToInt64Unchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(UInt16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt16Unchecked(array);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt16Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(UInt16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt16Unchecked(array, order);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt16 ToUInt16(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToUInt16Unchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(UInt32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt32Unchecked(array);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt32Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(UInt32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt32Unchecked(array, order);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt32 ToUInt32(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToUInt32Unchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(UInt64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt64Unchecked(array);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt64Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(UInt64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToUInt64Unchecked(array, order);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static UInt64 ToUInt64(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToUInt64Unchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Single) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToSingleUnchecked(array);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A single-precision floating point number converted from four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToSingleUnchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Single) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToSingleUnchecked(array, order);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Single ToSingle(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToSingleUnchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToDoubleUnchecked(array);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A double-precision floating point number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToDoubleUnchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A double-precision floating point number converted from eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToDoubleUnchecked(array, order);
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A double-precision floating point number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Double ToDouble(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToDoubleUnchecked(array, startIndex, order);
        }



        /// <summary>
        /// Returns a rational number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A rational number converted from eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (2 * sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToRationalUnchecked(array);
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToRationalUnchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (2 * sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToRationalUnchecked(array, order);
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static Rational ToRational(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");
            
            return ToRationalUnchecked(array, startIndex, order);
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The spatial dimension of the coordinate must be 2 or 3.;spatialDimension
        /// or
        /// The number of bytes in the array is less than required.
        /// </exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 spatialDimension)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (spatialDimension * sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToCoordinateUnchecked(array, spatialDimension);
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The spatial dimension of the coordinate must be 2 or 3.;spatialDimension
        /// or
        /// The number of bytes in the array is less than required.
        /// </exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 startIndex, Int32 spatialDimension)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (spatialDimension < 2 || spatialDimension > 3)
                throw new ArgumentException("The spatial dimension of the coordinate must be 2 or 3.", "spatialDimension");
            if (startIndex + spatialDimension * sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToCoordinateUnchecked(array, startIndex, spatialDimension);
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The spatial dimension of the coordinate must be 2 or 3.;spatialDimension
        /// or
        /// The number of bytes in the array is less than required.
        /// </exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 spatialDimension, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (spatialDimension * sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToCoordinateUnchecked(array, spatialDimension, order);
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The spatial dimension of the coordinate must be 2 or 3.;spatialDimension
        /// or
        /// The number of bytes in the array is less than required.
        /// </exception>
        public static Coordinate ToCoordinate(Byte[] array, Int32 startIndex, Int32 spatialDimension, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (spatialDimension < 2 || spatialDimension > 3)
                throw new ArgumentException("The spatial dimension of the coordinate must be 2 or 3.", "spatialDimension");
            if (startIndex + spatialDimension * sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            return ToCoordinateUnchecked(array, startIndex, spatialDimension, order);
        }

        #endregion

        #region Public static methods for converting from bytes (unchecked)

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit signed integer formed by two bytes.</returns>
        public static unsafe Int16 ToInt16Unchecked(Byte[] array)
        {
            fixed (Byte* pbyte = &array[0])
            {
                return *((Int16*)pbyte);
            }
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two byte at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Int16 ToInt16Unchecked(Byte[] array, Int32 startIndex)
        {
            fixed (Byte* pbyte = &array[startIndex])
            {
                if (startIndex % 2 == 0)
                { // data is aligned 
                    return *((Int16*)pbyte);
                }
                else
                {
                    return (Int16)((*pbyte) | (*(pbyte + 1) << 8));
                }
            }
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit signed integer formed by two bytes.</returns>
        public static unsafe Int16 ToInt16Unchecked(Byte[] array, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToInt16Unchecked(array);

            fixed (Byte* pbyte = &array[0])
            {
                return (Int16)((*pbyte << 8) | (*(pbyte + 1)));
            }
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Int16 ToInt16Unchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToInt16Unchecked(array, startIndex);

            fixed (Byte* pbyte = &array[startIndex])
            {
                return (Int16)((*pbyte << 8) | (*(pbyte + 1)));
            }
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit signed integer formed by four bytes.</returns>
        public static unsafe Int32 ToInt32Unchecked(Byte[] array)
        {
            fixed (Byte* pbyte = &array[0])
            {
                return *((Int32*)pbyte);
            }
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Int32 ToInt32Unchecked(Byte[] array, Int32 startIndex)
        {
            fixed (Byte* pbyte = &array[startIndex])
            {
                if (startIndex % 4 == 0)
                {
                    return *((Int32*)pbyte);
                }
                else
                {
                    return (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
                }
            }
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit signed integer formed by four bytes.</returns>
        public static unsafe Int32 ToInt32Unchecked(Byte[] array, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToInt32Unchecked(array);

            fixed (Byte* pbyte = &array[0])
            {
                return (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
            }
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Int32 ToInt32Unchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToInt32Unchecked(array, startIndex);

            fixed (Byte* pbyte = &array[startIndex])
            {
                return (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
            }
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static unsafe Int64 ToInt64Unchecked(Byte[] array)
        {
            fixed (Byte* pbyte = &array[0])
            {
                return *((Int64*)pbyte);
            }
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Int64 ToInt64Unchecked(Byte[] array, Int32 startIndex)
        {
            fixed (Byte* pbyte = &array[startIndex])
            {
                if (startIndex % 8 == 0)
                {
                    return *((Int64*)pbyte);
                }
                else
                {
                    Int32 i1 = (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
                    Int32 i2 = (*(pbyte + 4)) | (*(pbyte + 5) << 8) | (*(pbyte + 6) << 16) | (*(pbyte + 7) << 24);
                    return (UInt32)i1 | ((Int64)i2 << 32);
                }
            }
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes.</returns>
        public static unsafe Int64 ToInt64Unchecked(Byte[] array, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToInt64Unchecked(array);

            fixed (Byte* pbyte = &array[0])
            {
                Int32 i1 = (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
                Int32 i2 = (*(pbyte + 4) << 24) | (*(pbyte + 5) << 16) | (*(pbyte + 6) << 8) | (*(pbyte + 7));
                return (UInt32)i2 | ((Int64)i1 << 32);
            }
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Int64 ToInt64Unchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToInt64Unchecked(array, startIndex);

            fixed (Byte* pbyte = &array[startIndex])
            {
                Int32 i1 = (*pbyte << 24) | (*(pbyte + 1) << 16) | (*(pbyte + 2) << 8) | (*(pbyte + 3));
                Int32 i2 = (*(pbyte + 4) << 24) | (*(pbyte + 5) << 16) | (*(pbyte + 6) << 8) | (*(pbyte + 7));
                return (UInt32)i2 | ((Int64)i1 << 32);
            }
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes.</returns>
        public static UInt16 ToUInt16Unchecked(Byte[] array)
        {
            return (UInt16)ToInt16Unchecked(array, 0);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        public static UInt16 ToUInt16Unchecked(Byte[] array, Int32 startIndex)
        {
            return (UInt16)ToInt16Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes.</returns>
        public static UInt16 ToUInt16Unchecked(Byte[] array, ByteOrder order)
        {
            return (UInt16)ToInt16Unchecked(array, 0, order);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
        public static UInt16 ToUInt16Unchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            return (UInt16)ToInt16Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes.</returns>
        public static UInt32 ToUInt32Unchecked(Byte[] array)
        {
            return (UInt32)ToInt32Unchecked(array, 0);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        public static UInt32 ToUInt32Unchecked(Byte[] array, Int32 startIndex)
        {
            return (UInt32)ToInt32Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes.</returns>
        public static UInt32 ToUInt32Unchecked(Byte[] array, ByteOrder order)
        {
            return (UInt32)ToInt32Unchecked(array, 0, order);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        public static UInt32 ToUInt32Unchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            return (UInt32)ToInt32Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes.</returns>
        public static UInt64 ToUInt64Unchecked(Byte[] array)
        {
            return (UInt64)ToInt64Unchecked(array, 0);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static UInt64 ToUInt64Unchecked(Byte[] array, Int32 startIndex)
        {
            return (UInt64)ToInt64Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes.</returns>
        public static UInt64 ToUInt64Unchecked(Byte[] array, ByteOrder order)
        {
            return (UInt64)ToInt64Unchecked(array, 0, order);
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static UInt64 ToUInt64Unchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            return (UInt64)ToInt64Unchecked(array, startIndex);
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static unsafe Single ToSingleUnchecked(Byte[] array)
        {
            Int32 value = ToInt32Unchecked(array);
            return *(Single*)&value;
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A single-precision floating point number converted from four bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Single ToSingleUnchecked(Byte[] array, Int32 startIndex)
        {
            Int32 value = ToInt32Unchecked(array, startIndex);
            return *(Single*)&value;
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        public static unsafe Single ToSingleUnchecked(Byte[] array, ByteOrder order)
        {
            Int32 value = ToInt32Unchecked(array, order);
            return *(Single*)&value;
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Single ToSingleUnchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            Int32 value = ToInt32Unchecked(array, startIndex, order);
            return *(Single*)&value;
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A single-precision floating point number converted from four bytes.</returns>
        public static unsafe Double ToDoubleUnchecked(Byte[] array)
        {
            Int64 value = ToInt64Unchecked(array);
            return *(Double*)&value;
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A double-precision floating point number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Double ToDoubleUnchecked(Byte[] array, Int32 startIndex)
        {
            Int64 value = ToInt64Unchecked(array, startIndex);
            return *(Double*)&value;
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A double-precision floating point number converted from eight bytes.</returns>
        public static unsafe Double ToDoubleUnchecked(Byte[] array, ByteOrder order)
        {
            Int64 value = ToInt64Unchecked(array, order);
            return *(Double*)&value;
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A double-precision floating point number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static unsafe Double ToDoubleUnchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            Int64 value = ToInt64Unchecked(array, startIndex, order);
            return *(Double*)&value;
        }
        
        /// <summary>
        /// Returns a rational number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A rational number converted from eight bytes.</returns>
        public static Rational ToRationalUnchecked(Byte[] array)
        {
            return new Rational(ToInt32Unchecked(array), ToInt32Unchecked(array, sizeof(Int32)));
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static Rational ToRationalUnchecked(Byte[] array, Int32 startIndex)
        {
            return new Rational(ToInt32Unchecked(array, startIndex), ToInt32Unchecked(array, startIndex + sizeof(Int32)));
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static Rational ToRationalUnchecked(Byte[] array, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToRationalUnchecked(array);

            return new Rational(ToInt32Unchecked(array, sizeof(Int32), order), ToInt32Unchecked(array, order));
        }

        /// <summary>
        /// Returns a rational number converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>A rational number converted from eight bytes beginning at <paramref name="startIndex" />.</returns>
        public static Rational ToRationalUnchecked(Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToRationalUnchecked(array, startIndex);

            return new Rational(ToInt32Unchecked(array, startIndex + sizeof(Int32), order), ToInt32Unchecked(array, startIndex, order));
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        public static Coordinate ToCoordinateUnchecked(Byte[] array, Int32 spatialDimension)
        {
            if (spatialDimension == 3)
            {
                return new Coordinate(ToDoubleUnchecked(array), ToDoubleUnchecked(array, sizeof(Double)), ToDoubleUnchecked(array, 2 * sizeof(Double)));
            }
            else
            {
                return new Coordinate(ToDoubleUnchecked(array), ToDoubleUnchecked(array, sizeof(Double)));
            }
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        public static Coordinate ToCoordinateUnchecked(Byte[] array, Int32 startIndex, Int32 spatialDimension)
        {
            if (spatialDimension == 3)
            {
                return new Coordinate(ToDoubleUnchecked(array, startIndex), ToDoubleUnchecked(array, startIndex + sizeof(Double)), ToDoubleUnchecked(array, startIndex + 2 * sizeof(Double)));
            }
            else
            {
                return new Coordinate(ToDoubleUnchecked(array, startIndex), ToDoubleUnchecked(array, startIndex + sizeof(Double)));
            }
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at the beginning of a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes.</returns>
        public static Coordinate ToCoordinateUnchecked(Byte[] array, Int32 spatialDimension, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToCoordinateUnchecked(array, spatialDimension);

            if (spatialDimension == 3)
            {
                return new Coordinate(ToDoubleUnchecked(array, order), ToDoubleUnchecked(array, sizeof(Double), order), ToDoubleUnchecked(array, 2 * sizeof(Double), order));
            }
            else
            {
                return new Coordinate(ToDoubleUnchecked(array, order), ToDoubleUnchecked(array, sizeof(Double), order));
            }
        }

        /// <summary>
        /// Returns a coordinate converted from 16 or 24 bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="array">An array of bytes.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="spatialDimension">The spatial dimension of the coordinate.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>The coordinate formed by 16 or 24 bytes beginning at <paramref name="startIndex" />.</returns>
        public static Coordinate ToCoordinateUnchecked(Byte[] array, Int32 startIndex, Int32 spatialDimension, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return ToCoordinateUnchecked(array, startIndex, spatialDimension);

            if (spatialDimension == 3)
            {
                return new Coordinate(ToDoubleUnchecked(array, startIndex, order), ToDoubleUnchecked(array, startIndex + sizeof(Double), order), ToDoubleUnchecked(array, startIndex + 2 * sizeof(Double), order));
            }
            else
            {
                return new Coordinate(ToDoubleUnchecked(array, startIndex, order), ToDoubleUnchecked(array, startIndex + sizeof(Double), order));
            }
        }

        #endregion

        #region Public static methods for converting to bytes

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 2.</returns>
        public static Byte[] GetBytes(Int16 value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 2.</returns>
        public static Byte[] GetBytes(Int16 value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Int32 value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Int32 value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 8.</returns>
        public static Byte[] GetBytes(Int64 value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 8.</returns>
        public static Byte[] GetBytes(Int64 value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 2.</returns>
        public static Byte[] GetBytes(UInt16 value)
        {
            return GetBytesUnchecked(value);
        }
        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 2.</returns>
        public static Byte[] GetBytes(UInt16 value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(UInt32 value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(UInt32 value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 8.</returns>
        public static Byte[] GetBytes(UInt64 value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 8.</returns>
        public static Byte[] GetBytes(UInt64 value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Single value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Single value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Double value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Double value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified rational value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytes(Rational value)
        {
            return GetBytesUnchecked(value);
        }

        /// <summary>
        /// Returns the specified rational value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytes(Rational value, ByteOrder order)
        {
            return GetBytesUnchecked(value, order);
        }

        /// <summary>
        /// Returns the specified coordinate as an array of bytes.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <param name="spatialDimension">The spatial dimension of the conversion.</param>
        /// <returns>The array of bytes in the default byte-order with length 16 or 24.</returns>
        /// <exception cref="System.ArgumentException">The spatial dimension of the coordinate must be 2 or 3.;spatialDimension</exception>
        public static Byte[] GetBytes(Coordinate value, Int32 spatialDimension)
        {
            if (spatialDimension < 2 || spatialDimension > 3)
                throw new ArgumentException("The spatial dimension of the coordinate must be 2 or 3.", "spatialDimension");

            return GetBytesUnchecked(value, spatialDimension);
        }

        /// <summary>
        /// Returns the specified coordinate as an array of bytes.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <param name="spatialDimension">The spatial dimension of the conversion.</param>
        /// <param name="order">The byte order of the resulting array.</param>
        /// <returns>The array of bytes in the specified byte-order with length 16 or 24.</returns>
        /// <exception cref="System.ArgumentException">The spatial dimension of the coordinate must be 2 or 3.;spatialDimension</exception>
        public static Byte[] GetBytes(Coordinate value, Int32 spatialDimension, ByteOrder order)
        {
            if (spatialDimension < 2 || spatialDimension > 3)
                throw new ArgumentException("The spatial dimension of the coordinate must be 2 or 3.", "spatialDimension");

            return GetBytesUnchecked(value, spatialDimension, order);
        }

        #endregion

        #region Public static methods for converting to bytes (unchecked)

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 2.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Int16 value)
        {
            Byte[] bytes = new Byte[sizeof(Int16)];
            fixed (Byte* b = bytes)
                *((Int16*)b) = value;
            return bytes;
        }

        /// <summary>
        /// Returns the specified 16-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 2.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Int16 value, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return GetBytesUnchecked(value);

            Byte[] bytes = new Byte[sizeof(Int16)];
            fixed (Byte* b = bytes)
            {
                Byte* v = (Byte*)&value;

                *b = *(v + 1);
                *(b + 1) = *v;
            }
            return bytes;
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Int32 value)
        {
            Byte[] bytes = new Byte[sizeof(Int32)];
            fixed (Byte* b = bytes)
                *((Int32*)b) = value;
            return bytes;
        }

        /// <summary>
        /// Returns the specified 32-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Int32 value, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return GetBytesUnchecked(value);

            Byte[] bytes = new Byte[sizeof(Int32)];
            fixed (Byte* b = bytes)
            {
                Byte* v = (Byte*)&value;

                for (Int32 i = 0; i < bytes.Length; i++)
                    *(b + i) = *(v + bytes.Length - 1 - i);
            }
            return bytes;
        }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 8.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Int64 value)
        {
            Byte[] bytes = new Byte[sizeof(Int64)];
            fixed (Byte* b = bytes)
                *((Int64*)b) = value;
            return bytes;
        }

        /// <summary>
        /// Returns the specified 64-bit signed integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 8.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Int64 value, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return GetBytesUnchecked(value);

            Byte[] bytes = new Byte[sizeof(Int64)];
            fixed (Byte* b = bytes)
            {
                Byte* v = (Byte*)&value;

                for (Int32 i = 0; i < bytes.Length; i++)
                    *(b + i) = *(v + bytes.Length - 1 - i);
            }
            return bytes;
        }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 2.</returns>
        public static Byte[] GetBytesUnchecked(UInt16 value)
        {
            return GetBytesUnchecked((Int16)value);
        }

        /// <summary>
        /// Returns the specified 16-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 2.</returns>
        public static Byte[] GetBytesUnchecked(UInt16 value, ByteOrder order)
        {
            return GetBytesUnchecked((Int16)value, order);
        }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytesUnchecked(UInt32 value)
        {
            return GetBytesUnchecked((Int32)value);
        }

        /// <summary>
        /// Returns the specified 32-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytesUnchecked(UInt32 value, ByteOrder order)
        {
            return GetBytesUnchecked((Int32)value, order);
        }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 8.</returns>
        public static Byte[] GetBytesUnchecked(UInt64 value)
        {
            return GetBytesUnchecked((Int64)value);
        }

        /// <summary>
        /// Returns the specified 64-bit unsigned integer value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert. </param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 8.</returns>
        public static Byte[] GetBytesUnchecked(UInt64 value, ByteOrder order)
        {
            return GetBytesUnchecked((Int64)value, order);
        }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Single value)
        {
            return GetBytesUnchecked(*(Int32*)&value);
        }

        /// <summary>
        /// Returns the specified single-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Single value, ByteOrder order)
        {
            return GetBytesUnchecked(*(Int32*)&value, order);
        }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Double value)
        {
            return GetBytesUnchecked(*(Int64*)&value);
        }

        /// <summary>
        /// Returns the specified double-precision floating point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static unsafe Byte[] GetBytesUnchecked(Double value, ByteOrder order)
        {
            return GetBytesUnchecked(*(Int64*)&value, order);
        }

        /// <summary>
        /// Returns the specified rational value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes in the default byte-order with length 4.</returns>
        public static Byte[] GetBytesUnchecked(Rational value)
        {
            Byte[] rationalBytes = new Byte[2 * sizeof(Int32)];

            Byte[] array = GetBytesUnchecked(value.Numerator);
            array.CopyTo(rationalBytes, 0);
            array = GetBytesUnchecked(value.Denominator);
            array.CopyTo(rationalBytes, sizeof(Int32));

            return rationalBytes;
        }

        /// <summary>
        /// Returns the specified rational value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <returns>An array of bytes in the specified byte-order with length 4.</returns>
        public static Byte[] GetBytesUnchecked(Rational value, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return GetBytesUnchecked(value);

            Byte[] rationalBytes = new Byte[2 * sizeof(Int32)];
            
            Byte[] array = GetBytesUnchecked(value.Numerator, order);
            array.CopyTo(rationalBytes, sizeof(Int32));
            array = GetBytesUnchecked(value.Denominator, order);
            array.CopyTo(rationalBytes, 0);
            
            return rationalBytes;
        }

        /// <summary>
        /// Returns the specified coordinate as an array of bytes.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <param name="spatialDimension">The spatial dimension of the conversion.</param>
        /// <returns>The array of bytes in the default byte-order with length 16 or 24.</returns>
        public static Byte[] GetBytesUnchecked(Coordinate value, Int32 spatialDimension)
        {
            Byte[] coordinateBytes = null;
            if (spatialDimension == 3)
            {
                coordinateBytes = new Byte[3 * sizeof(Double)];

                Byte[] array = GetBytesUnchecked(value.X);
                array.CopyTo(coordinateBytes, 0);

                array = GetBytesUnchecked(value.Y);
                array.CopyTo(coordinateBytes, sizeof(Double));

                array = GetBytesUnchecked(value.Z);
                array.CopyTo(coordinateBytes, 2 * sizeof(Double));

                return coordinateBytes;
            }
            else
            {
                coordinateBytes = new Byte[2 * sizeof(Double)];

                Byte[] array = GetBytesUnchecked(value.X);
                array.CopyTo(coordinateBytes, 0);

                array = GetBytesUnchecked(value.Y);
                array.CopyTo(coordinateBytes, sizeof(Double));
            }
            return coordinateBytes;
        }

        /// <summary>
        /// Returns the specified coordinate as an array of bytes.
        /// </summary>
        /// <param name="value">The coordinate.</param>
        /// <param name="spatialDimension">The spatial dimension of the conversion.</param>
        /// <param name="order">The byte order of the resulting array.</param>
        /// <returns>The array of bytes in the specified byte-order with length 16 or 24.</returns>
        public static Byte[] GetBytesUnchecked(Coordinate value, Int32 spatialDimension, ByteOrder order)
        {
            if (order == DefaultByteOrder)
                return GetBytesUnchecked(value, spatialDimension);

            Byte[] coordinateBytes = null;
            if (spatialDimension == 3)
            {
                coordinateBytes = new Byte[3 * sizeof(Double)];

                Byte[] array = GetBytesUnchecked(value.Z, order);
                Array.Reverse(array, 0, array.Length);
                array.CopyTo(coordinateBytes, 0);

                array = GetBytesUnchecked(value.Y, order);
                Array.Reverse(array, 0, array.Length);
                array.CopyTo(coordinateBytes, sizeof(Double));

                array = GetBytesUnchecked(value.X, order);
                Array.Reverse(array, 0, array.Length);
                array.CopyTo(coordinateBytes, 2 * sizeof(Double));
            }
            else
            {
                coordinateBytes = new Byte[2 * sizeof(Double)];

                Byte[] array = GetBytesUnchecked(value.Y, order);
                Array.Reverse(array, 0, array.Length);
                array.CopyTo(coordinateBytes, 0);

                array = GetBytesUnchecked(value.X, order);
                Array.Reverse(array, 0, array.Length);
                array.CopyTo(coordinateBytes, sizeof(Double));
            }

            return coordinateBytes;
        }

        #endregion

        #region Public static methods for converting to existing byte array

        /// <summary>
        /// Copies the specified 16-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int16 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 16-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int16 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 32-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int32 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 32-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int32 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 64-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int64 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 64-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Int64 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Int64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt16 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt16 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt16) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt32 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt32 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt64 value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(UInt64 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(UInt64) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified single-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Single value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified single-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Single value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Single) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified double-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Double value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified double-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Double value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + sizeof(Double) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified rational value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Rational value, Byte[] array, Int32 startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified rational value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static void CopyBytes(Rational value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (array == null)
                throw new ArgumentNullException("array", "The array is null.");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is less than 0.");
            if (startIndex >= array.Length)
                throw new ArgumentOutOfRangeException("startIndex", "The starting index is equal to or greater than the length of the array.");
            if (startIndex + 2 * sizeof(Int32) > array.Length)
                throw new ArgumentException("The number of bytes in the array is less than required.", "array");

            CopyBytesUnchecked(value, array, startIndex, order);
        }
        
        #endregion

        #region Public static methods for converting to existing byte array (unchecked)

        /// <summary>
        /// Copies the specified 16-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static unsafe void CopyBytesUnchecked(Int16 value, Byte[] array, Int32 startIndex)
        {
            fixed (Byte* b = array)
                *((Int16*)(b + startIndex)) = value;
        }

        /// <summary>
        /// Copies the specified 16-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        /// <exception cref="System.ArgumentNullException">The array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting index is less than 0.
        /// or
        /// The starting index is equal to or greater than the length of the array.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of bytes in the array is less than required.</exception>
        public static unsafe void CopyBytesUnchecked(Int16 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
            {
                CopyBytesUnchecked(value, array, startIndex);
                return;
            }

            fixed (Byte* b = array)
            {
                Byte* v = (Byte*)&value;

                *(b + startIndex) = *(v + 1);
                *(b + startIndex + 1) = *v;
            }
        }

        /// <summary>
        /// Copies the specified 32-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static unsafe void CopyBytesUnchecked(Int32 value, Byte[] array, Int32 startIndex)
        {
            fixed (Byte* b = array)
                *((Int32*)(b + startIndex)) = value;
        }

        /// <summary>
        /// Copies the specified 32-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static unsafe void CopyBytesUnchecked(Int32 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
            {
                CopyBytesUnchecked(value, array, startIndex);
                return;
            }

            Byte[] bytes = new Byte[sizeof(Int32)];
            fixed (Byte* b = bytes)
            {
                Byte* v = (Byte*)&value;

                for (Int32 i = 0; i < bytes.Length; i++)
                    *(b + startIndex + i) = *(v + bytes.Length - 1 - i);
            }
        }

        /// <summary>
        /// Copies the specified 64-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static unsafe void CopyBytesUnchecked(Int64 value, Byte[] array, Int32 startIndex)
        {
            fixed (Byte* b = array)
                *((Int64*)(b + startIndex)) = value;
        }

        /// <summary>
        /// Copies the specified 64-bit signed integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static unsafe void CopyBytesUnchecked(Int64 value, Byte[] array, Int32 startIndex, ByteOrder order = ByteOrder.LittleEndian)
        {
            if (order == DefaultByteOrder)
            {
                CopyBytesUnchecked(value, array, startIndex);
                return;
            }

            Byte[] bytes = new Byte[sizeof(Int64)];
            fixed (Byte* b = bytes)
            {
                Byte* v = (Byte*)&value;

                for (Int32 i = 0; i < bytes.Length; i++)
                    *(b + startIndex + i) = *(v + bytes.Length - 1 - i);
            }
        }

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static void CopyBytesUnchecked(UInt16 value, Byte[] array, Int32 startIndex)
        {
            CopyBytesUnchecked((Int16)value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 16-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static void CopyBytesUnchecked(UInt16 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            CopyBytesUnchecked((Int16)value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static void CopyBytesUnchecked(UInt32 value, Byte[] array, Int32 startIndex)
        {
            CopyBytesUnchecked((Int32)value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 32-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static void CopyBytesUnchecked(UInt32 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            CopyBytesUnchecked((Int32)value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static void CopyBytesUnchecked(UInt64 value, Byte[] array, Int32 startIndex)
        {
            CopyBytesUnchecked((Int64)value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified 64-bit unsigned integer value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static void CopyBytesUnchecked(UInt64 value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            CopyBytesUnchecked((Int64)value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified single-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static unsafe void CopyBytesUnchecked(Single value, Byte[] array, Int32 startIndex)
        {
            CopyBytesUnchecked(*(Int32*)&value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified single-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static unsafe void CopyBytesUnchecked(Single value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            CopyBytesUnchecked(*(Int32*)&value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified double-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        public static unsafe void CopyBytesUnchecked(Double value, Byte[] array, Int32 startIndex)
        {
            CopyBytesUnchecked(*(Int64*)&value, array, startIndex);
        }

        /// <summary>
        /// Copies the specified double-precision floating point value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static unsafe void CopyBytesUnchecked(Double value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            CopyBytesUnchecked(*(Int64*)&value, array, startIndex, order);
        }

        /// <summary>
        /// Copies the specified rational value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static void CopyBytesUnchecked(Rational value, Byte[] array, Int32 startIndex)
        {
            CopyBytesUnchecked(value.Numerator, array, startIndex);
            CopyBytesUnchecked(value.Denominator, array, startIndex + sizeof(Int32));
        }

        /// <summary>
        /// Copies the specified rational value to the specified array.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <param name="array">The destination array.</param>
        /// <param name="startIndex">The starting position within the array.</param>
        /// <param name="order">The byte-order of the array.</param>
        public static void CopyBytesUnchecked(Rational value, Byte[] array, Int32 startIndex, ByteOrder order)
        {
            if (order == DefaultByteOrder)
            {
                CopyBytesUnchecked(value, array, startIndex);
                return;
            }

            CopyBytesUnchecked(value.Numerator, array, startIndex + sizeof(Int32));
            CopyBytesUnchecked(value.Denominator, array, startIndex);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Gets the system default byte order.
        /// </summary>
        /// <returns>The system default byte order.</returns>
        private static unsafe ByteOrder GetDefaultByteOrder()
        {
            Int32 i = 1;
            return (*((Byte*)&i) == 1) ? ByteOrder.LittleEndian : ByteOrder.BigEndian;
        }

        #endregion
    }
}
