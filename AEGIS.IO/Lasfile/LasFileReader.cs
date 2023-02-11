// <copyright file="LasFileReader.cs" company="Eötvös Loránd University (ELTE)">
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
    /// <remarks>
    /// Supported versions: LAS 1.0 - LAS 1.4 R14.
    /// </remarks>
    /// <author>Antal Tar</author>
    [IdentifiedObjectInstance("AEGIS::610104", "LAS file")]
    public class LasFileReader : GeometryStreamReader
    {
        #region Private fields

        /// <summary>
        /// The underlying binary reader.
        /// </summary>
        private BinaryReader _reader;

        /// <summary>
        /// The current position of the reader in bytes.
        /// </summary>
        private UInt64 _position;

        /// <summary>
        /// The public header.
        /// </summary>
        private LasPublicHeader _header;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a copy of the public header.
        /// </summary>
        public LasPublicHeader Header
        {
            get
            {
                return new LasPublicHeader()
                {
                    FileSourceId = _header.FileSourceId,
                    GPSTimeType = _header.GPSTimeType,
                    IsWavePacketStorageInternal = _header.IsWavePacketStorageInternal,
                    IsWavePacketStorageExternal = _header.IsWavePacketStorageExternal,
                    IsReturnNumberOriginSynthetical = _header.IsReturnNumberOriginSynthetical,
                    CRSType = _header.CRSType,
                    ProjectId1 = _header.ProjectId1,
                    ProjectId2 = _header.ProjectId2,
                    ProjectId3 = _header.ProjectId3,
                    ProjectId4 = _header.ProjectId4,
                    VersionMajor = _header.VersionMajor,
                    VersionMinor = _header.VersionMinor,
                    SystemIdentifier = _header.SystemIdentifier,
                    GeneratingSoftware = _header.GeneratingSoftware,
                    FileCreationDay = _header.FileCreationDay,
                    FileCreationYear = _header.FileCreationYear,
                    PublicHeaderSize = _header.PublicHeaderSize,
                    VariableLengthRecordCount = _header.VariableLengthRecordCount,
                    PointDataOffset = _header.PointDataOffset,
                    PointDataFormat = _header.PointDataFormat,
                    PointDataLength = _header.PointDataLength,
                    PointCount = _header.PointCount,
                    PointCountPerReturn = _header.PointCountPerReturn,
                    ScaleFactorX = _header.ScaleFactorX,
                    ScaleFactorY = _header.ScaleFactorY,
                    ScaleFactorZ = _header.ScaleFactorZ,
                    OffsetX = _header.OffsetX,
                    OffsetY = _header.OffsetY,
                    OffsetZ = _header.OffsetZ,
                    MaxX = _header.MaxX,
                    MinX = _header.MinX,
                    MaxY = _header.MaxY,
                    MinY = _header.MinY,
                    MaxZ = _header.MaxZ,
                    MinZ = _header.MinZ,
                    WavePacketRecordOffset = _header.WavePacketRecordOffset,
                    ExtendedVariableLengthRecordOffset = _header.ExtendedVariableLengthRecordOffset,
                    ExtendedVariableLengthRecordCount = _header.ExtendedVariableLengthRecordCount
                };
            }
        }

        /// <summary>
        /// Gets the number of readed points.
        /// </summary>
        public UInt64 ReadedPointCount { get; private set; }

        /// <summary>
        /// Gets the total number of points.
        /// </summary>
        public UInt64 TotalPointCount
        {
            get => _header is null ? 0 : _header.PointCount;
        }

        #endregion

        #region Constructor and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LasFileReader"/> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="ArgumentNullException">The path is null.</exception>
        /// <exception cref="ArgumentException">
        /// The path is empty, or consists only whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="IOException">Error occurred during stream opening.</exception>
        /// <exception cref="InvalidDataException">The stream content is in invalid format.</exception>
        public LasFileReader(String path) : base(path, GeometryStreamFormats.Lasfile, null)
            => Initialize();

        /// <summary>
        /// Initializes a new instance of the <see cref="LasFileReader"/> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="ArgumentNullException">The path is null.</exception>
        /// <exception cref="ArgumentException">
        /// The path is empty, or consists only whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="IOException">Error occurred during stream opening.</exception>
        /// <exception cref="InvalidDataException">The stream content is in invalid format.</exception>
        public LasFileReader(Uri path) : base(path, GeometryStreamFormats.Lasfile, null)
            => Initialize();

        /// <summary>
        /// Initializes a new instance of the <see cref="LasFileReader"/> class.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <exception cref="ArgumentNullException">The stream is null.</exception>
        /// <exception cref="IOException">Error occurred during stream opening.</exception>
        /// <exception cref="InvalidDataException">The stream content is in invalid format.</exception>
        public LasFileReader(Stream stream) : base(stream, GeometryStreamFormats.Lasfile, null)
            => Initialize();

        /// <summary>
        /// Finalizes an instance of the <see cref="LasFileReader" /> class.
        /// </summary>
        ~LasFileReader()
        {
            Dispose(false);
        }

        #endregion

        #region GeometryStreamReader protected methods

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
        /// <exception cref="EndOfStreamException">Error occurred during stream reading.</exception>
        protected override IGeometry ApplyReadGeometry()
        {
            if (EndOfStream)
                throw new EndOfStreamException(MessageContentReadError);

            if (_header.PointDataFormat > 10)
                throw new InvalidDataException(MessageContentInvalid);

            UInt64 expectedPosition =
                _header.PointDataOffset + ReadedPointCount * _header.PointDataLength;

            if (_position != expectedPosition)
            {
                _baseStream.Position = _header.PointDataOffset;
                _position = _header.PointDataOffset;

                while (_position < expectedPosition)
                {
                    _reader.ReadByte();
                    _position++;
                }
            }

            Double x = (_reader.ReadInt32() * _header.ScaleFactorX) + _header.OffsetX;
            _position += 4;

            Double y = (_reader.ReadInt32() * _header.ScaleFactorY) + _header.OffsetY;
            _position += 4;

            Double z = (_reader.ReadInt32() * _header.ScaleFactorZ) + _header.OffsetZ;
            _position += 4;

            IGeometry point;

            switch (Header.PointDataFormat)
            {
                case 0:
                    point = new LasPointFormat0(x, y, z);
                    break;
                case 1:
                    point = new LasPointFormat1(x, y, z);
                    break;
                case 2:
                    point = new LasPointFormat2(x, y, z);
                    break;
                case 3:
                    point = new LasPointFormat3(x, y, z);
                    break;
                case 4:
                    point = new LasPointFormat4(x, y, z);
                    break;
                case 5:
                    point = new LasPointFormat5(x, y, z);
                    break;
                case 6:
                    point = new LasPointFormat6(x, y, z);
                    break;
                case 7:
                    point = new LasPointFormat7(x, y, z);
                    break;
                case 8:
                    point = new LasPointFormat8(x, y, z);
                    break;
                case 9:
                    point = new LasPointFormat9(x, y, z);
                    break;
                default:
                    point = new LasPointFormat10(x, y, z);
                    break;
            }

            ((LasPointBase) point).Intensity = _reader.ReadUInt16();
            _position += 2;

            var bitfield = Convert.ToInt32(_reader.ReadByte());
            _position += 1;

            if (_header.PointDataFormat < 6)
            {
                ((LasPointBase) point).ReturnNumber = Convert.ToByte(bitfield & 0b00000111);
                ((LasPointBase) point).TotalReturnNumber = Convert.ToByte((bitfield >> 3) & 0b00000111);

                if ((bitfield & 0b01000000) == 0b01000000)
                    ((LasPointBase) point).IsScanDirectionPositive = true;
                else
                    ((LasPointBase) point).IsScanDirectionPositive = false;

                if ((bitfield & 0b10000000) == 0b10000000)
                    ((LasPointBase) point).IsFlightLineEdge = true;
                else
                    ((LasPointBase) point).IsFlightLineEdge = false;

                bitfield = Convert.ToInt32(_reader.ReadByte());
                _position += 1;

                ((LasPointBase) point).Classification = Convert.ToByte(bitfield & 0b00011111);

                if ((bitfield & 0b00100000) == 0b00100000)
                    ((LasPointBase) point).IsSynthetic = true;
                else
                    ((LasPointBase) point).IsSynthetic = false;

                if ((bitfield & 0b01000000) == 0b01000000)
                    ((LasPointBase) point).IsKeyPoint = true;
                else
                    ((LasPointBase) point).IsKeyPoint = false;

                if ((bitfield & 0b10000000) == 0b10000000)
                    ((LasPointBase) point).IsWithheld = true;
                else
                    ((LasPointBase) point).IsWithheld = false;

                ((LasPointFormat0) point).ScanAngle = _reader.ReadSByte();
                _position += 1;

                ((LasPointBase) point).UserData = _reader.ReadByte();
                _position += 1;
            }
            else
            {
                ((LasPointBase) point).ReturnNumber = Convert.ToByte(bitfield & 0b00001111);
                ((LasPointBase) point).TotalReturnNumber = Convert.ToByte((bitfield >> 4) & 0b00001111);

                bitfield = Convert.ToInt32(_reader.ReadByte());
                _position += 1;

                if ((bitfield & 0b00000001) == 0b00000001)
                    ((LasPointBase) point).IsSynthetic = true;
                else
                    ((LasPointBase) point).IsSynthetic = false;

                if ((bitfield & 0b00000010) == 0b00000010)
                    ((LasPointBase) point).IsKeyPoint = true;
                else
                    ((LasPointBase) point).IsKeyPoint = false;

                if ((bitfield & 0b00000100) == 0b00000100)
                    ((LasPointBase) point).IsWithheld = true;
                else
                    ((LasPointBase) point).IsWithheld = false;

                if ((bitfield & 0b00001000) == 0b00001000)
                    ((LasPointFormat6) point).IsOverlap = true;
                else
                    ((LasPointFormat6) point).IsOverlap = false;

                ((LasPointFormat6) point).ScannerChannel = Convert.ToByte((bitfield >> 4) & 0b00000011);

                if ((bitfield & 0b01000000) == 0b01000000)
                    ((LasPointBase) point).IsScanDirectionPositive = true;
                else
                    ((LasPointBase) point).IsScanDirectionPositive = false;

                if ((bitfield & 0b10000000) == 0b10000000)
                    ((LasPointBase) point).IsFlightLineEdge = true;
                else
                    ((LasPointBase) point).IsFlightLineEdge = false;

                ((LasPointBase) point).Classification = _reader.ReadByte();
                _position += 1;

                ((LasPointBase) point).UserData = _reader.ReadByte();
                _position += 1;

                ((LasPointFormat6) point).ScanAngle = _reader.ReadInt16();
                _position += 2;
            }

            ((LasPointBase) point).PointSourceId = _reader.ReadUInt16();
            _position += 2;

            if (_header.PointDataFormat == 1 || _header.PointDataFormat == 4)
            {
                ((LasPointFormat1) point).GPSTime = _reader.ReadDouble();
                _position += 8;
            }
            else if (_header.PointDataFormat == 3 || _header.PointDataFormat == 5)
            {
                ((LasPointFormat3) point).GPSTime = _reader.ReadDouble();
                _position += 8;
            }
            else if (_header.PointDataFormat > 5)
            {
                ((LasPointFormat6) point).GPSTime = _reader.ReadDouble();
                _position += 8;
            }

            if (_header.PointDataFormat == 2 || _header.PointDataFormat == 3 || _header.PointDataFormat == 5)
            {
                ((LasPointFormat2) point).RedChannel = _reader.ReadUInt16();
                _position += 2;

                ((LasPointFormat2) point).GreenChannel = _reader.ReadUInt16();
                _position += 2;

                ((LasPointFormat2) point).BlueChannel = _reader.ReadUInt16();
                _position += 2;
            }
            else if (_header.PointDataFormat == 7 || _header.PointDataFormat == 8 || _header.PointDataFormat == 10)
            {
                ((LasPointFormat7) point).RedChannel = _reader.ReadUInt16();
                _position += 2;

                ((LasPointFormat7) point).GreenChannel = _reader.ReadUInt16();
                _position += 2;

                ((LasPointFormat7) point).BlueChannel = _reader.ReadUInt16();
                _position += 2;
            }

            if (_header.PointDataFormat == 8 || _header.PointDataFormat == 10)
            {
                ((LasPointFormat8) point).NearInfraredChannel = _reader.ReadUInt16();
                _position += 2;
            }

            if (_header.PointDataFormat == 4)
            {
                ((LasPointFormat4) point).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                ((LasPointFormat4) point).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                ((LasPointFormat4) point).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                ((LasPointFormat4) point).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat4) point).Xt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat4) point).Yt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat4) point).Zt = _reader.ReadSingle();
                _position += 4;
            }
            else if (_header.PointDataFormat == 5)
            {
                ((LasPointFormat5) point).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                ((LasPointFormat5) point).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                ((LasPointFormat5) point).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                ((LasPointFormat5) point).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat5) point).Xt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat5) point).Yt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat5) point).Zt = _reader.ReadSingle();
                _position += 4;
            }
            else if (_header.PointDataFormat == 9)
            {
                ((LasPointFormat9) point).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                ((LasPointFormat9) point).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                ((LasPointFormat9) point).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                ((LasPointFormat9) point).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat9) point).Xt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat9) point).Yt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat9) point).Zt = _reader.ReadSingle();
                _position += 4;
            }
            else if (_header.PointDataFormat == 10)
            {
                ((LasPointFormat10) point).WavePacketDescriptorIndex = _reader.ReadByte();
                _position += 1;

                ((LasPointFormat10) point).WavePacketDataOffset = _reader.ReadUInt64();
                _position += 8;

                ((LasPointFormat10) point).WavePacketSize = _reader.ReadUInt32();
                _position += 4;

                ((LasPointFormat10) point).ReturnPointWaveformLocation = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat10) point).Xt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat10) point).Yt = _reader.ReadSingle();
                _position += 4;

                ((LasPointFormat10) point).Zt = _reader.ReadSingle();
                _position += 4;
            }

            ReadedPointCount++;
            expectedPosition += _header.PointDataLength;

            while (_position < expectedPosition)
            {
                _reader.ReadByte();
                _position++;
            }

            return point;
        }

        #endregion

        #region IDisposable protected methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                    _reader.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the instance and reads the public header.
        /// </summary>
        /// <exception cref="IOException">Error occurred during stream opening.</exception>
        /// <exception cref="InvalidDataException">The stream content is in invalid format.</exception>
        private void Initialize()
        {
            try
            {
                _reader = new BinaryReader(_baseStream, Encoding.ASCII, true);
                _position = 0;

                ReadedPointCount = 0;
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentOpenError, ex);
            }

            var signature = new String(_reader.ReadChars(4));
            _position += 4;

            if (signature != "LASF")
            {
                Dispose();
                throw new InvalidDataException(MessageHeaderInvalid);
            }

            var header = new LasPublicHeader();

            header.FileSourceId = _reader.ReadUInt16();
            _position += 2;

            var bitfield = Convert.ToInt32(_reader.ReadUInt16());
            _position += 2;

            if ((bitfield & 0b00001) == 0b00001)
                header.IsGPSStandardTime = true;
            else
                header.IsGPSWeekTime = true;

            if ((bitfield & 0b00010) == 0b00010)
                header.IsWavePacketStorageInternal = true;
            else if ((bitfield & 0b00100) == 0b00100)
                header.IsWavePacketStorageExternal = true;

            if ((bitfield & 0b01000) == 0b01000)
                header.IsReturnNumberOriginSynthetical = true;
            else
                header.IsReturnNumberOriginSynthetical = false;

            if ((bitfield & 0b10000) == 0b10000)
                header.IsCRSWKT = true;
            else
                header.IsCRSGeoTIFF = true;

            header.ProjectId1 = _reader.ReadUInt32();
            _position += 4;

            header.ProjectId2 = _reader.ReadUInt16();
            _position += 2;

            header.ProjectId3 = _reader.ReadUInt16();
            _position += 2;

            header.ProjectId4 = _reader.ReadUInt64();
            _position += 8;

            header.VersionMajor = _reader.ReadByte();
            _position += 1;

            header.VersionMinor = _reader.ReadByte();
            _position += 1;

            header.SystemIdentifier = new String(_reader.ReadChars(32)).Trim('\0');
            _position += 32;

            header.GeneratingSoftware = new String(_reader.ReadChars(32)).Trim('\0');
            _position += 32;

            header.FileCreationDay = _reader.ReadUInt16();
            _position += 2;

            header.FileCreationYear = _reader.ReadUInt16();
            _position += 2;

            header.PublicHeaderSize = _reader.ReadUInt16();
            _position += 2;

            header.PointDataOffset = _reader.ReadUInt32();
            _position += 4;

            header.VariableLengthRecordCount = _reader.ReadUInt32();
            _position += 4;

            header.PointDataFormat = _reader.ReadByte();
            _position += 1;

            header.PointDataLength = _reader.ReadUInt16();
            _position += 2;

            header.PointCount = Convert.ToUInt64(_reader.ReadUInt32());
            _position += 4;

            for (Int32 i = 0; i < 5; i++)
            {
                header.SetPointCount(i, Convert.ToUInt64(_reader.ReadUInt32()));
                _position += 4;
            }

            header.ScaleFactorX = _reader.ReadDouble();
            _position += 8;

            header.ScaleFactorY = _reader.ReadDouble();
            _position += 8;

            header.ScaleFactorZ = _reader.ReadDouble();
            _position += 8;

            header.OffsetX = _reader.ReadDouble();
            _position += 8;

            header.OffsetY = _reader.ReadDouble();
            _position += 8;

            header.OffsetZ = _reader.ReadDouble();
            _position += 8;

            header.MaxX = _reader.ReadDouble();
            _position += 8;

            header.MinX = _reader.ReadDouble();
            _position += 8;

            header.MaxY = _reader.ReadDouble();
            _position += 8;

            header.MinY = _reader.ReadDouble();
            _position += 8;

            header.MaxZ = _reader.ReadDouble();
            _position += 8;

            header.MinZ = _reader.ReadDouble();
            _position += 8;

            if (header.VersionMajor == 1 && header.VersionMinor > 2 || header.VersionMajor > 1)
            {
                header.WavePacketRecordOffset = _reader.ReadUInt64();
                _position += 8;
            }

            if (header.VersionMajor == 1 && header.VersionMinor > 3 || header.VersionMajor > 1)
            {
                header.ExtendedVariableLengthRecordOffset = _reader.ReadUInt64();
                _position += 8;

                header.ExtendedVariableLengthRecordCount = _reader.ReadUInt32();
                _position += 4;

                header.PointCount = _reader.ReadUInt64();
                _position += 8;

                for (Int32 i = 0; i < 15; i++)
                {
                    header.SetPointCount(i, _reader.ReadUInt64());
                    _position += 8;
                }
            }

            _header = header;
        }

        #endregion
    }
}
