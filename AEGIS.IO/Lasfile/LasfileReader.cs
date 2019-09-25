/// <copyright file="LasfileReader.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Antal Tar</author>

namespace ELTE.AEGIS.IO.Lasfile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using AEGIS.Management;

    /// <summary>
    /// Represents a LAS file (VERSION 1.4) format reader.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::610104", "LAS file")]
    public class LasfileReader : GeometryStreamReader
    {
        /// <summary>
        /// The binary reader.
        /// </summary>
        private BinaryReader _reader;

        /// <summary>
        /// The current position of the reader in bytes.
        /// </summary>
        private UInt64 _position;

        /// <summary>
        /// Gets the public header.
        /// </summary>
        public LasPublicHeader Header { get; private set; }

        /// <summary>
        /// Gets the number of readed points.
        /// </summary>
        public UInt64 ReadedPointCount { get; private set; }

        /// <summary>
        /// Gets the total number of points.
        /// </summary>
        public UInt64 TotalPointCount { get => Header != null ? Header.PointCount : 0; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LasfileReader"/> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public LasfileReader(String path) : this(path, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LasfileReader"/> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public LasfileReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, GeometryStreamFormats.Lasfile, parameters)
        {
            _reader = null;
            _position = 0;
            ReadedPointCount = 0;
            Header = null;

            try
            {
                _reader = new BinaryReader(_baseStream, Encoding.ASCII, true);

                ReadPublicHeader();
            }
            catch (Exception ex)
            {
                BaseStream.Close();

                throw new IOException(MessageContentReadError, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LasfileReader"/> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public LasfileReader(Uri path) : this(path, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LasfileReader"/> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public LasfileReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, GeometryStreamFormats.Lasfile, parameters)
        {
            _reader = null;
            _position = 0;
            ReadedPointCount = 0;
            Header = null;

            try
            {
                _reader = new BinaryReader(_baseStream, Encoding.ASCII, true);

                ReadPublicHeader();
            }
            catch (Exception ex)
            {
                BaseStream.Close();

                throw new IOException(MessageContentReadError, ex);
            }
        }

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise, <c>false</c>.</returns>
        protected override Boolean GetEndOfStream()
        {
            return ReadedPointCount == TotalPointCount;
        }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        /// <exception cref="IOException">The point data record format is not supported.</exception>
        protected override IGeometry ApplyReadGeometry()
        {
            if (_position < Header.PointDataOffset)
            {
                _baseStream.Seek(Header.PointDataOffset, SeekOrigin.Begin);
                _position = Header.PointDataOffset;
            }

            IGeometry buffer;
            Double x = (_reader.ReadInt32() * Header.ScaleFactorX) + Header.OffsetX;
            _position += 4;

            Double y = (_reader.ReadInt32() * Header.ScaleFactorY) + Header.OffsetY;
            _position += 4;

            Double z = (_reader.ReadInt32() * Header.ScaleFactorZ) + Header.OffsetZ;
            _position += 4;

            switch (Header.PointDataFormat)
            {
                case 0:
                    buffer = new LasPointFormat0(x, y, z);
                    break;
                case 1:
                    buffer = new LasPointFormat1(x, y, z);
                    break;
                case 2:
                    buffer = new LasPointFormat2(x, y, z);
                    break;
                case 3:
                    buffer = new LasPointFormat3(x, y, z);
                    break;
                case 4:
                    buffer = new LasPointFormat4(x, y, z);
                    break;
                case 5:
                    buffer = new LasPointFormat5(x, y, z);
                    break;
                case 6:
                    buffer = new LasPointFormat6(x, y, z);
                    break;
                case 7:
                    buffer = new LasPointFormat7(x, y, z);
                    break;
                case 8:
                    buffer = new LasPointFormat8(x, y, z);
                    break;
                case 9:
                    buffer = new LasPointFormat9(x, y, z);
                    break;
                default:
                    buffer = new LasPointFormat10(x, y, z);
                    break;
            }

            (buffer as LasPointBase).Intensity = _reader.ReadUInt16();
            _position += 2;

            var bitfield = Convert.ToInt32(_reader.ReadByte());
            _position += 1;

            if (Header.PointDataFormat < 6)
            {
                (buffer as LasPointBase).ReturnNumber = Convert.ToByte(bitfield & 0b00000111);
                (buffer as LasPointBase).TotalReturnNumber = Convert.ToByte(bitfield & 0b00111000);

                if ((bitfield & 0b01000000) == 0b01000000)
                    (buffer as LasPointBase).IsScanDirectionPositive = true;
                else
                    (buffer as LasPointBase).IsScanDirectionPositive = false;

                if ((bitfield & 0b10000000) == 0b10000000)
                    (buffer as LasPointBase).IsFlightLineEdge = true;
                else
                    (buffer as LasPointBase).IsFlightLineEdge = false;

                bitfield = Convert.ToInt32(_reader.ReadByte());
                _position += 1;

                (buffer as LasPointBase).Classification = Convert.ToByte(bitfield & 0b00011111);

                if ((bitfield & 0b00100000) == 0b00100000)
                    (buffer as LasPointBase).IsSynthetic = true;
                else
                    (buffer as LasPointBase).IsSynthetic = false;

                if ((bitfield & 0b01000000) == 0b01000000)
                    (buffer as LasPointBase).IsKeyPoint = true;
                else
                    (buffer as LasPointBase).IsKeyPoint = false;

                if ((bitfield & 0b10000000) == 0b10000000)
                    (buffer as LasPointBase).IsWithheld = true;
                else
                    (buffer as LasPointBase).IsWithheld = false;

                (buffer as LasPointFormat0).ScanAngle = _reader.ReadSByte();
                _position += 1;

                (buffer as LasPointBase).UserData = _reader.ReadByte();
                _position += 1;
            }
            else
            {
                (buffer as LasPointBase).ReturnNumber = Convert.ToByte(bitfield & 0b00001111);
                (buffer as LasPointBase).TotalReturnNumber = Convert.ToByte(bitfield & 0b11110000);

                bitfield = Convert.ToInt32(_reader.ReadByte());
                _position += 1;

                if ((bitfield & 0b00000001) == 0b00000001)
                    (buffer as LasPointBase).IsSynthetic = true;
                else
                    (buffer as LasPointBase).IsSynthetic = false;

                if ((bitfield & 0b00000010) == 0b00000010)
                    (buffer as LasPointBase).IsKeyPoint = true;
                else
                    (buffer as LasPointBase).IsKeyPoint = false;

                if ((bitfield & 0b00000100) == 0b00000100)
                    (buffer as LasPointBase).IsWithheld = true;
                else
                    (buffer as LasPointBase).IsWithheld = false;

                if ((bitfield & 0b00001000) == 0b00001000)
                    (buffer as LasPointFormat6).IsOverlap = true;
                else
                    (buffer as LasPointFormat6).IsOverlap = false;

                (buffer as LasPointFormat6).ScannerChannel = Convert.ToByte(bitfield & 0b00110000);

                if ((bitfield & 0b01000000) == 0b01000000)
                    (buffer as LasPointBase).IsScanDirectionPositive = true;
                else
                    (buffer as LasPointBase).IsScanDirectionPositive = false;

                if ((bitfield & 0b10000000) == 0b10000000)
                    (buffer as LasPointBase).IsFlightLineEdge = true;
                else
                    (buffer as LasPointBase).IsFlightLineEdge = false;

                (buffer as LasPointBase).Classification = _reader.ReadByte();
                _position += 1;

                (buffer as LasPointBase).UserData = _reader.ReadByte();
                _position += 1;

                (buffer as LasPointFormat6).ScanAngle = _reader.ReadInt16();
                _position += 2;
            }

            (buffer as LasPointBase).PointSourceId = _reader.ReadUInt16();
            _position += 2;

            if (Header.PointDataFormat == 1 || Header.PointDataFormat == 4)
            {
                (buffer as LasPointFormat1).GPSTime = _reader.ReadDouble();
                _position += 8;
            }
            else if (Header.PointDataFormat == 3 || Header.PointDataFormat == 5)
            {
                (buffer as LasPointFormat3).GPSTime = _reader.ReadDouble();
                _position += 8;
            }
            else if (Header.PointDataFormat > 5)
            {
                (buffer as LasPointFormat6).GPSTime = _reader.ReadDouble();
                _position += 8;
            }
            
            if (Header.PointDataFormat == 2 || Header.PointDataFormat == 3 || Header.PointDataFormat == 5)
            {
                (buffer as LasPointFormat2).RedChannel = _reader.ReadUInt16();
                _position += 2;

                (buffer as LasPointFormat2).GreenChannel = _reader.ReadUInt16();
                _position += 2;

                (buffer as LasPointFormat2).BlueChannel = _reader.ReadUInt16();
                _position += 2;
            }
            else if (Header.PointDataFormat == 7 || Header.PointDataFormat == 8 || Header.PointDataFormat == 10)
            {
                (buffer as LasPointFormat7).RedChannel = _reader.ReadUInt16();
                _position += 2;

                (buffer as LasPointFormat7).GreenChannel = _reader.ReadUInt16();
                _position += 2;

                (buffer as LasPointFormat7).BlueChannel = _reader.ReadUInt16();
                _position += 2;
            }

            if (Header.PointDataFormat == 8 || Header.PointDataFormat == 10)
            {
                (buffer as LasPointFormat8).NearInfraredChannel = _reader.ReadUInt16();
                _position += 2;
            }

            if (Header.PointDataFormat == 4)
            {
                (buffer as LasPointFormat4).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                (buffer as LasPointFormat4).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                (buffer as LasPointFormat4).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                (buffer as LasPointFormat4).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat4).Xt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat4).Yt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat4).Zt = _reader.ReadSingle();
                _position += 4;
            }
            else if (Header.PointDataFormat == 5)
            {
                (buffer as LasPointFormat5).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                (buffer as LasPointFormat5).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                (buffer as LasPointFormat5).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                (buffer as LasPointFormat5).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat5).Xt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat5).Yt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat5).Zt = _reader.ReadSingle();
                _position += 4;
            }
            if (Header.PointDataFormat == 9)
            {
                (buffer as LasPointFormat9).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                (buffer as LasPointFormat9).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                (buffer as LasPointFormat9).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                (buffer as LasPointFormat9).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat9).Xt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat9).Yt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat9).Zt = _reader.ReadSingle();
                _position += 4;
            }
            if (Header.PointDataFormat == 10)
            {
                (buffer as LasPointFormat10).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                (buffer as LasPointFormat10).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                (buffer as LasPointFormat10).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                (buffer as LasPointFormat10).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat10).Xt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat10).Yt = _reader.ReadSingle();
                _position += 4;

                (buffer as LasPointFormat10).Zt = _reader.ReadSingle();
                _position += 4;
            }

            ReadedPointCount++;

            return buffer;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }

                _position = 0;
                ReadedPointCount = 0;
                Header = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Reads the public header.
        /// </summary>
        /// <exception cref="InvalidDataException">Stream content is in an invalid format.</exception>
        private void ReadPublicHeader()
        {
            var  signature = new String(_reader.ReadChars(4));
            _position += 4;

            if (signature != "LASF")
                throw new InvalidDataException(MessageHeaderInvalid);

            var buffer = new LasPublicHeader();

            buffer.FileSourceId = _reader.ReadUInt16();
            _position += 2;

            var bitfield = Convert.ToInt32(_reader.ReadUInt16());
            _position += 2;

            if ((bitfield & 0b00001) == 0b00001)
                buffer.IsGPSStandardTime = true;
            else
                buffer.IsGPSWeekTime = true;

            if ((bitfield & 0b00100) == 0b00100)
                buffer.IsWavePacketStorageExternal = true;
            else if ((bitfield & 0b00010) == 0b00010)
                buffer.IsWavePacketStorageInternal = true;

            if ((bitfield & 0b01000) == 0b01000)
                buffer.IsReturnNumberOriginSynthetical = true;
            else
                buffer.IsReturnNumberOriginSynthetical = false;

            if ((bitfield & 0b10000) == 0b10000)
                buffer.IsCRSWKT = true;
            else
                buffer.IsCRSGeoTIFF = true;

            buffer.ProjectId1 = _reader.ReadUInt32();
            _position += 4;

            buffer.ProjectId2 = _reader.ReadUInt16();
            _position += 2;

            buffer.ProjectId3 = _reader.ReadUInt16();
            _position += 2;

            buffer.ProjectId4 = _reader.ReadUInt64();
            _position += 8;

            buffer.VersionMajor = _reader.ReadByte();
            _position += 1;

            buffer.VersionMinor = _reader.ReadByte();
            _position += 1;

            buffer.SystemIdentifier = new String(_reader.ReadChars(32)).Trim('\0');
            _position += 32;

            buffer.GeneratingSoftware = new String(_reader.ReadChars(32)).Trim('\0');
            _position += 32;

            buffer.FileCreationDay = _reader.ReadUInt16();
            _position += 2;

            buffer.FileCreationYear = _reader.ReadUInt16();
            _position += 2;

            buffer.PublicHeaderSize = _reader.ReadUInt16();
            _position += 2;

            buffer.PointDataOffset = _reader.ReadUInt32();
            _position += 4;

            buffer.VariableLengthRecordCount = _reader.ReadUInt32();
            _position += 4;

            buffer.PointDataFormat = _reader.ReadByte();
            _position += 1;

            if (buffer.PointDataFormat > 10)
                throw new InvalidDataException(MessageContentInvalid);

            buffer.PointDataLength = _reader.ReadUInt16();
            _position += 2;

            buffer.PointCount = Convert.ToUInt64(_reader.ReadUInt32());
            _position += 4;

            for (Int32 i = 0; i < 5; i++)
            {
                buffer.SetPointCount(i, Convert.ToUInt64(_reader.ReadUInt32()));
                _position += 4;
            }

            buffer.ScaleFactorX = _reader.ReadDouble();
            _position += 8;

            buffer.ScaleFactorY = _reader.ReadDouble();
            _position += 8;

            buffer.ScaleFactorZ = _reader.ReadDouble();
            _position += 8;

            buffer.OffsetX = _reader.ReadDouble();
            _position += 8;

            buffer.OffsetY = _reader.ReadDouble();
            _position += 8;

            buffer.OffsetZ = _reader.ReadDouble();
            _position += 8;

            buffer.MaxX = _reader.ReadDouble();
            _position += 8;

            buffer.MinX = _reader.ReadDouble();
            _position += 8;

            buffer.MaxY = _reader.ReadDouble();
            _position += 8;

            buffer.MinY = _reader.ReadDouble();
            _position += 8;

            buffer.MaxZ = _reader.ReadDouble();
            _position += 8;

            buffer.MinZ = _reader.ReadDouble();
            _position += 8;

            if (buffer.VersionMajor == 1 && buffer.VersionMinor < 3)
            {
                Header = buffer;

                return;
            }

            buffer.WavePacketRecordOffset = _reader.ReadUInt64();
            _position += 8;

            if (buffer.VersionMajor == 1 && buffer.VersionMinor < 4)
            {
                Header = buffer;

                return;
            }

            buffer.ExtendedVariableLengthRecordOffset = _reader.ReadUInt64();
            _position += 8;

            buffer.ExtendedVariableLengthRecordCount = _reader.ReadUInt32();
            _position += 4;

            if (buffer.PointCount == 0)
            {
                buffer.PointCount = _reader.ReadUInt64();
                _position += 8;

                for (Int32 i = 0; i < 15; i++)
                {
                    buffer.SetPointCount(i, _reader.ReadUInt64());
                    _position += 8;
                }
            }
            else
            {
                for (Int32 i = 0; i < 16; i++)
                {
                    _reader.ReadUInt64();
                    _position += 8;
                }
            }

            Header = buffer;
        }
    }
}
