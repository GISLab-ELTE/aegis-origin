/// <copyright file="TiffTag.cs" company="Eötvös Loránd University (ELTE)">
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
using System;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Defines methods and values of TIFF tags.
    /// </summary>
    public static class TiffTag
    {
        /// <summary>
        /// The image width tag.
        /// </summary>
        public const UInt16 ImageWidth = 256;

        /// <summary>
        /// The image length tag.
        /// </summary>
        public const UInt16 ImageLength = 257;

        /// <summary>
        /// The bits per sample tag.
        /// </summary>
        public const UInt16 BitsPerSample = 258;

        /// <summary>
        /// The compression tag.
        /// </summary>
        public const UInt16 Compression = 259;

        /// <summary>
        /// The photometric interpretation tag.
        /// </summary>
        public const UInt16 PhotometricInterpretation = 262;

        /// <summary>
        /// The document name tag.
        /// </summary>
        public const UInt16 DocumentName = 269;

        /// <summary>
        /// The image description tag.
        /// </summary>
        public const UInt16 ImageDescription = 270;

        /// <summary>
        /// The make tag.
        /// </summary>
        public const UInt16 Make = 271;

        /// <summary>
        /// The model tag.
        /// </summary>
        public const UInt16 Model = 272;

        /// <summary>
        /// The strip offsets tag.
        /// </summary>
        public const UInt16 StripOffsets = 273;

        /// <summary>
        /// The samples per pixel tag.
        /// </summary>
        public const UInt16 SamplesPerPixel = 277;

        /// <summary>
        /// The rows per strip tag.
        /// </summary>
        public const UInt16 RowsPerStrip = 278;

        /// <summary>
        /// The strip byte counts tag.
        /// </summary>
        public const UInt16 StripByteCounts = 279;

        /// <summary>
        /// The X resolution tag.
        /// </summary>
        public const UInt16 XResolution = 282;

        /// <summary>
        /// The Y resolution tag.
        /// </summary>
        public const UInt16 YResolution = 283;

        /// <summary>
        /// The page name tag.
        /// </summary>
        public const UInt16 PageName = 285;

        /// <summary>
        /// The resolution unit tag.
        /// </summary>
        public const UInt16 ResolutionUnit = 296;

        /// <summary>
        /// The software tag.
        /// </summary>
        public const UInt16 Software = 305;

        /// <summary>
        /// The date/time tag.
        /// </summary>
        public const UInt16 DateTime = 306;

        /// <summary>
        /// The artist tag.
        /// </summary>
        public const UInt16 Artist = 307;

        /// <summary>
        /// The host computer tag.
        /// </summary>
        public const UInt16 HostComputer = 308;

        /// <summary>
        /// The horizontal differencing tag.
        /// </summary>
        public const UInt16 HorizontalDifferencing = 317;

        /// <summary>
        /// The color palette tag.
        /// </summary>
        public const UInt16 ColorPalette = 320;

        /// <summary>
        /// The tile width tag.
        /// </summary>
        public const UInt16 TileWidth = 322;

        /// <summary>
        /// The tile length tag.
        /// </summary>
        public const UInt16 TileLength = 323;

        /// <summary>
        /// The tile offsets tag.
        /// </summary>
        public const UInt16 TileOffsets = 324;

        /// <summary>
        /// The tile byte counts tag.
        /// </summary>
        public const UInt16 TileByteCounts = 325;

        /// <summary>
        /// The sample format tag.
        /// </summary>
        public const UInt16 SampleFormat = 339;

        /// <summary>
        /// The copyright tag.
        /// </summary>
        public const UInt16 Copyright = 33432;

        /// <summary>
        /// The model pixel scale tag.
        /// </summary>
        public const UInt16 ModelPixelScaleTag = 33550;

        /// <summary>
        /// The model transformation tag.
        /// </summary>
        public const UInt16 ModelTransformationTag = 34264;

        /// <summary>
        /// The model transformation tag for GeoTIFF 0.2 (Intergraph).
        /// </summary>
        public const UInt16 IntergraphMatrixTag = 33920;

        /// <summary>
        /// The model tie-point tag.
        /// </summary>
        public const UInt16 ModelTiepointTag = 33922;

        /// <summary>
        /// The geo-key directory tag.
        /// </summary>
        public const UInt16 GeoKeyDirectoryTag = 34735;

        /// <summary>
        /// The geo-parameters tag for double precision floating-point values.
        /// </summary>
        public const UInt16 GeoDoubleParamsTag = 34736;

        /// <summary>
        /// The geo-parameters tag for ASCII values.
        /// </summary>
        public const UInt16 GeoAsciiParamsTag = 34737;

        /// <summary>
        /// The AEGIS imaging tag.
        /// </summary>
        public const UInt16 AegisImaging = 56101;

        /// <summary>
        /// The AEGIS interpretation tag.
        /// </summary>
        public const UInt16 AegisInterpretation = 56102;

        /// <summary>
        /// The AEGIS attributes tag.
        /// </summary>
        public const UInt16 AegisAttributes = 56165;

        /// <summary>
        /// Returns the value of a TIFF tag.
        /// </summary>
        /// <param name="type">The type of the tag.</param>
        /// <param name="array">The byte array.</param>
        /// <param name="startIndex">The starting index of the value in the array.</param>
        /// <param name="byteOrder">The byte order.</param>
        /// <returns>The value of the tag.</returns>
        public static Object GetValue(TiffTagType type, Byte[] array, Int32 startIndex, ByteOrder byteOrder)
        {
            switch (type)
            {
                case TiffTagType.Byte:
                    return array[startIndex];
                case TiffTagType.ASCII:
                    return System.Text.Encoding.ASCII.GetChars(array, startIndex, 1)[0];
                case TiffTagType.Short:
                    return EndianBitConverter.ToUInt16(array, startIndex, byteOrder);
                case TiffTagType.Long:
                    return EndianBitConverter.ToUInt32(array, startIndex, byteOrder);
                case TiffTagType.Rational:
                    return EndianBitConverter.ToRational(array, startIndex, byteOrder);
                case TiffTagType.SByte:
                    return Convert.ToSByte(array[startIndex]);
                case TiffTagType.Undefined:
                    return array[startIndex];
                case TiffTagType.SShort:
                    return EndianBitConverter.ToUInt16(array, startIndex, byteOrder);
                case TiffTagType.SLong:
                    return EndianBitConverter.ToUInt32(array, startIndex, byteOrder);
                case TiffTagType.SRational:
                    return EndianBitConverter.ToRational(array, startIndex, byteOrder);
                case TiffTagType.Float:
                    return EndianBitConverter.ToSingle(array, startIndex, byteOrder);
                case TiffTagType.Double:
                    return EndianBitConverter.ToDouble(array, startIndex, byteOrder);
                case TiffTagType.Long8:
                case TiffTagType.LongOffset:
                    return EndianBitConverter.ToUInt64(array, startIndex, byteOrder);
                case TiffTagType.SLong8:
                    return EndianBitConverter.ToInt64(array, startIndex, byteOrder);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the TIFF tag type of a value.
        /// </summary>
        /// <param name="value">The value of the tag.</param>
        public static TiffTagType GetType(Object value)
        {
            if (value is Byte)
                return TiffTagType.Byte;
            if (value is Char || value is String)
                return TiffTagType.ASCII;
            if (value is UInt16)
                return TiffTagType.Short;
            if (value is UInt32)
                return TiffTagType.Long;
            if (value is SByte)
                return TiffTagType.SByte;
            if (value is Int16)
                return TiffTagType.SShort;
            if (value is Int32)
                return TiffTagType.SLong;
            if (value is Rational)
                return TiffTagType.SRational; // missing comparison for Rational
            if (value is Single)
                return TiffTagType.Float;
            if (value is Double)
                return TiffTagType.Double;
            if (value is Int64)
                return TiffTagType.SLong8;
            if (value is UInt64)
                return TiffTagType.Long8;

            return TiffTagType.Undefined;
        }

        /// <summary>
        /// Returns the value size for a specified tag type.
        /// </summary>
        /// <param name="type">The type of the tag.</param>
        /// <returns>The size of the value in bytes.</returns>
        public static UInt16 GetSize(TiffTagType type)
        {
            switch (type)
            {
                case TiffTagType.Byte:
                case TiffTagType.SByte:
                case TiffTagType.ASCII:
                case TiffTagType.Undefined:
                    return 1;
                case TiffTagType.Short:
                case TiffTagType.SShort:
                    return 2;
                case TiffTagType.Long:
                case TiffTagType.SLong:
                case TiffTagType.Float:
                    return 4;
                case TiffTagType.Double:
                case TiffTagType.Rational:
                case TiffTagType.SRational:
                case TiffTagType.Long8:
                case TiffTagType.SLong8:
                case TiffTagType.LongOffset:
                    return 8;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Sets the value of a TIFF tag.
        /// </summary>
        /// <param name="type">The type of the tag.</param>
        /// <param name="value">The value.</param>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The zero-based start index.</param>
        /// <returns>The index of the array after the operation.</returns>
        public static Int32 SetValue(TiffTagType type, Object value, Byte[] array, Int32 startIndex)
        {
            Int32 dataSize = GetSize(type);

            switch (type)
            {
                case TiffTagType.Byte:
                    EndianBitConverter.CopyBytes(Convert.ToByte(value), array, startIndex);
                    break;
                case TiffTagType.ASCII:
                    Byte[] asciiValues = System.Text.Encoding.ASCII.GetBytes(value as String);
                    Array.Copy(asciiValues, 0, array, startIndex, asciiValues.Length);
                    return startIndex + asciiValues.Length;
                case TiffTagType.Short:
                    EndianBitConverter.CopyBytes(Convert.ToUInt16(value), array, startIndex);
                    break;
                case TiffTagType.Long:
                    EndianBitConverter.CopyBytes(Convert.ToUInt32(value), array, startIndex);
                    break;
                case TiffTagType.SByte:
                    EndianBitConverter.CopyBytes(Convert.ToSByte(value), array, startIndex);
                    break;
                case TiffTagType.SShort:
                    EndianBitConverter.CopyBytes(Convert.ToInt16(value), array, startIndex);
                    break;
                case TiffTagType.Rational:
                    EndianBitConverter.CopyBytes((Rational)value, array, startIndex);
                    break;
                case TiffTagType.SRational:
                    EndianBitConverter.CopyBytes((Rational)value, array, startIndex);
                    break;
                case TiffTagType.SLong:
                    EndianBitConverter.CopyBytes(Convert.ToInt32(value), array, startIndex);
                    break;
                case TiffTagType.Float:
                    EndianBitConverter.CopyBytes(Convert.ToSingle(value), array, startIndex);
                    break;
                case TiffTagType.Double:
                    EndianBitConverter.CopyBytes(Convert.ToDouble(value), array, startIndex);
                    break;
                case TiffTagType.Long8:
                case TiffTagType.LongOffset:
                    EndianBitConverter.CopyBytes(Convert.ToUInt64(value), array, startIndex);
                    break;
                case TiffTagType.SLong8:
                    EndianBitConverter.CopyBytes(Convert.ToInt64(value), array, startIndex);
                    break;
            }
            return startIndex + dataSize;
        }
    }
}
