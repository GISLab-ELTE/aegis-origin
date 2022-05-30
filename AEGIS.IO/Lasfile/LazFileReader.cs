/// <copyright file="LazFileReader.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roland Krisztandl</author>

using ELTE.AEGIS.Management;
using LASzip.Net;
using System;
using System.IO;
using System.Text;

namespace ELTE.AEGIS.IO.Lasfile
{
    /// <summary>
    /// Represents a LAZ file format reader using LASzip.Net.
    /// Supports LAS and LAZ files.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::610105", "LAZ file")]
    public class LazFileReader : GeometryStreamReader
    {
        private laszip _reader;
        private LasPublicHeader _header;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="LazFileReader"/> class.
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
        public LazFileReader(string path) : base(path, GeometryStreamFormats.Lazfile, null)
            => Initialize();

        /// <summary>
        /// Initializes a new instance of the <see cref="LazFileReader"/> class.
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
        public LazFileReader(Uri path) : base(path, GeometryStreamFormats.Lazfile, null)
            => Initialize();

        /// <summary>
        /// Initializes a new instance of the <see cref="LazFileReader"/> class.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <exception cref="ArgumentNullException">The stream is null.</exception>
        /// <exception cref="IOException">Error occurred during stream opening.</exception>
        /// <exception cref="InvalidDataException">The stream content is in invalid format.</exception>
        public LazFileReader(Stream stream) : base(stream, GeometryStreamFormats.Lazfile, null)
            => Initialize();

        /// <summary>
        /// Finalizes an instance of the <see cref="LazFileReader" /> class.
        /// </summary>
        ~LazFileReader()
        {
            Dispose(false);
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
        /// Closes the reader.
        /// </summary>
        public override void Close()
        {
            _reader.close_reader();
            base.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                    _reader.close_reader();
            }

            base.Dispose(disposing);
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

            LasPointBase point;

            _reader.read_point();
            var coordArray = new double[3];
            _reader.get_coordinates(coordArray);

            switch (_header.PointDataFormat)
            {
                case 0:
                    point = new LasPointFormat0(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 1:
                    point = new LasPointFormat1(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 2:
                    point = new LasPointFormat2(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 3:
                    point = new LasPointFormat3(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 4:
                    point = new LasPointFormat4(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 5:
                    point = new LasPointFormat5(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 6:
                    point = new LasPointFormat6(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 7:
                    point = new LasPointFormat7(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 8:
                    point = new LasPointFormat8(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                case 9:
                    point = new LasPointFormat9(coordArray[0], coordArray[1], coordArray[2]);
                    break;
                default:
                    point = new LasPointFormat10(coordArray[0], coordArray[1], coordArray[2]);
                    break;
            }

            //set point properties
            laszip_point lazPoint = _reader.get_point_pointer();

            point.Intensity = lazPoint.intensity;

            if(_header.PointDataFormat < 6)
            {
                point.ReturnNumber = lazPoint.return_number;
                point.TotalReturnNumber = lazPoint.number_of_returns;
                point.IsScanDirectionPositive = lazPoint.scan_direction_flag == 1;
                point.IsFlightLineEdge = lazPoint.edge_of_flight_line == 1;

                point.Classification = lazPoint.classification;
                point.IsSynthetic = lazPoint.synthetic_flag == 1;
                point.IsKeyPoint = lazPoint.keypoint_flag == 1;
                point.IsWithheld = lazPoint.withheld_flag == 1;

                ((LasPointFormat0)point).ScanAngle = lazPoint.scan_angle_rank;
            }
            else
            {
                point.ReturnNumber = lazPoint.extended_return_number;
                point.TotalReturnNumber = lazPoint.extended_number_of_returns;

                byte bitfield = lazPoint.extended_classification_flags;
                point.IsSynthetic = ((bitfield >> 0) & 1) == 1;
                point.IsKeyPoint = ((bitfield >> 1) & 1) == 1;
                point.IsWithheld = ((bitfield >> 2) & 1) == 1;
                ((LasPointFormat6)point).IsOverlap = ((bitfield >> 3) & 1) == 1;

                ((LasPointFormat6)point).ScannerChannel = lazPoint.extended_scanner_channel;

                point.IsScanDirectionPositive = lazPoint.scan_direction_flag == 1;
                point.IsFlightLineEdge = lazPoint.edge_of_flight_line == 1;

                point.Classification = lazPoint.extended_classification;

                ((LasPointFormat6)point).ScanAngle = lazPoint.extended_scan_angle;
                ((LasPointFormat6)point).GPSTime = lazPoint.gps_time;
            }

            point.UserData = lazPoint.user_data;
            point.PointSourceId = lazPoint.point_source_ID;

            if (_header.PointDataFormat == 1 || _header.PointDataFormat == 4)
            {
                ((LasPointFormat1)point).GPSTime = lazPoint.gps_time;
            }
            else if (_header.PointDataFormat == 3 || _header.PointDataFormat == 5)
            {
                ((LasPointFormat3)point).GPSTime = lazPoint.gps_time;
            }

            if (_header.PointDataFormat == 2 || _header.PointDataFormat == 3 || _header.PointDataFormat == 5)
            {
                ((LasPointFormat2)point).RedChannel = lazPoint.rgb[0];
                ((LasPointFormat2)point).GreenChannel = lazPoint.rgb[1];
                ((LasPointFormat2)point).BlueChannel = lazPoint.rgb[2];
            }
            else if (_header.PointDataFormat == 7 || _header.PointDataFormat == 8)
            {
                ((LasPointFormat7)point).RedChannel = lazPoint.rgb[0];
                ((LasPointFormat7)point).GreenChannel = lazPoint.rgb[1];
                ((LasPointFormat7)point).BlueChannel = lazPoint.rgb[2];
            }
            else if (_header.PointDataFormat == 10)
            {
                ((LasPointFormat10)point).RedChannel = lazPoint.rgb[0];
                ((LasPointFormat10)point).GreenChannel = lazPoint.rgb[1];
                ((LasPointFormat10)point).BlueChannel = lazPoint.rgb[2];
            }

            if (_header.PointDataFormat == 8)
            {
                ((LasPointFormat8)point).NearInfraredChannel = lazPoint.rgb[3];
            }
            else if (_header.PointDataFormat == 10)
            {
                ((LasPointFormat10)point).NearInfraredChannel = lazPoint.rgb[3];
            }

            //WAVES for FORMAT 4, 5, 9, 10
            byte[] wavepacket = lazPoint.wave_packet;
            if (_header.PointDataFormat == 4)
            {
                ((LasPointFormat4)point).WavePacketDescriptorIndex = wavepacket[0];
                ((LasPointFormat4)point).WavePacketDataOffset = BitConverter.ToUInt64(wavepacket, 1);
                ((LasPointFormat4)point).WavePacketSize = BitConverter.ToUInt32(wavepacket, 9);
                ((LasPointFormat4)point).ReturnPointWaveformLocation = BitConverter.ToSingle(wavepacket, 13);
                ((LasPointFormat4)point).Xt = BitConverter.ToSingle(wavepacket, 17);
                ((LasPointFormat4)point).Yt = BitConverter.ToSingle(wavepacket, 21);
                ((LasPointFormat4)point).Zt = BitConverter.ToSingle(wavepacket, 25);
            }
            else if (_header.PointDataFormat == 5)
            {
                ((LasPointFormat5)point).WavePacketDescriptorIndex = wavepacket[0];
                ((LasPointFormat5)point).WavePacketDataOffset = BitConverter.ToUInt64(wavepacket, 1);
                ((LasPointFormat5)point).WavePacketSize = BitConverter.ToUInt32(wavepacket, 9);
                ((LasPointFormat5)point).ReturnPointWaveformLocation = BitConverter.ToSingle(wavepacket, 13);
                ((LasPointFormat5)point).Xt = BitConverter.ToSingle(wavepacket, 17);
                ((LasPointFormat5)point).Yt = BitConverter.ToSingle(wavepacket, 21);
                ((LasPointFormat5)point).Zt = BitConverter.ToSingle(wavepacket, 25);
            }
            else if (_header.PointDataFormat == 9)
            {
                ((LasPointFormat9)point).WavePacketDescriptorIndex = wavepacket[0];
                ((LasPointFormat9)point).WavePacketDataOffset = BitConverter.ToUInt64(wavepacket, 1);
                ((LasPointFormat9)point).WavePacketSize = BitConverter.ToUInt32(wavepacket, 9);
                ((LasPointFormat9)point).ReturnPointWaveformLocation = BitConverter.ToSingle(wavepacket, 13);
                ((LasPointFormat9)point).Xt = BitConverter.ToSingle(wavepacket, 17);
                ((LasPointFormat9)point).Yt = BitConverter.ToSingle(wavepacket, 21);
                ((LasPointFormat9)point).Zt = BitConverter.ToSingle(wavepacket, 25);
            }
            else if (_header.PointDataFormat == 10)
            {
                ((LasPointFormat10)point).WavePacketDescriptorIndex = wavepacket[0];
                ((LasPointFormat10)point).WavePacketDataOffset = BitConverter.ToUInt64(wavepacket, 1);
                ((LasPointFormat10)point).WavePacketSize = BitConverter.ToUInt32(wavepacket, 9);
                ((LasPointFormat10)point).ReturnPointWaveformLocation = BitConverter.ToSingle(wavepacket, 13);
                ((LasPointFormat10)point).Xt = BitConverter.ToSingle(wavepacket, 17);
                ((LasPointFormat10)point).Yt = BitConverter.ToSingle(wavepacket, 21);
                ((LasPointFormat10)point).Zt = BitConverter.ToSingle(wavepacket, 25);
            }

            ReadedPointCount++;

            return point;
        }

        /// <summary>
        /// Initializes the instance and reads the public header.
        /// </summary>
        /// <exception cref="IOException">Error occurred during stream opening.</exception>
        private void Initialize()
        {
            _reader = new laszip();

            ReadedPointCount = 0;

            if (_reader.open_reader_stream(_baseStream, out _) != 0)
            {
                string errMsg = _reader.get_error();
                throw new IOException(errMsg);
            }

            //set header properties
            var lazHeader = _reader.get_header_pointer();

            LasPublicHeader header = new LasPublicHeader();

            header.FileSourceId = lazHeader.file_source_ID;

            var bitfield = Convert.ToInt32(lazHeader.global_encoding);

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

            header.ProjectId1 = lazHeader.project_ID_GUID_data_1;
            header.ProjectId2 = lazHeader.project_ID_GUID_data_2;
            header.ProjectId3 = lazHeader.project_ID_GUID_data_3;
            header.ProjectId4 = BitConverter.ToUInt64(lazHeader.project_ID_GUID_data_4, 0);
            header.VersionMajor = lazHeader.version_major;
            header.VersionMinor = lazHeader.version_minor;
            header.SystemIdentifier = Encoding.ASCII.GetString(lazHeader.system_identifier);
            header.GeneratingSoftware = Encoding.ASCII.GetString(lazHeader.generating_software);
            header.FileCreationDay = lazHeader.file_creation_day;
            header.FileCreationYear = lazHeader.file_creation_year;
            header.PublicHeaderSize = lazHeader.header_size;
            header.VariableLengthRecordCount = lazHeader.number_of_variable_length_records;
            header.PointDataOffset = lazHeader.offset_to_point_data;
            header.PointDataFormat = lazHeader.point_data_format;
            header.PointDataLength = lazHeader.point_data_record_length;
            header.PointCount = lazHeader.number_of_point_records != 0 ?
                lazHeader.number_of_point_records : lazHeader.extended_number_of_point_records;
            header.PointCountPerReturn = lazHeader.number_of_point_records != 0 ?
                ConvertToULongArray(lazHeader.number_of_points_by_return) : lazHeader.extended_number_of_points_by_return;
            header.ScaleFactorX = lazHeader.x_scale_factor;
            header.ScaleFactorY = lazHeader.y_scale_factor;
            header.ScaleFactorZ = lazHeader.z_scale_factor;
            header.OffsetX = lazHeader.x_offset;
            header.OffsetY = lazHeader.y_offset;
            header.OffsetZ = lazHeader.z_offset;
            header.MaxX = lazHeader.max_x;
            header.MinX = lazHeader.min_x;
            header.MaxY = lazHeader.max_y;
            header.MinY = lazHeader.min_y;
            header.MaxZ = lazHeader.max_z;
            header.MinZ = lazHeader.min_z;
            header.WavePacketRecordOffset = lazHeader.start_of_waveform_data_packet_record;
            header.ExtendedVariableLengthRecordOffset = lazHeader.start_of_first_extended_variable_length_record;
            header.ExtendedVariableLengthRecordCount = lazHeader.number_of_extended_variable_length_records;

            _header = header;
        }

        private static ulong[] ConvertToULongArray(uint[] input)
        {
            ulong[] output = new ulong[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i];
            }
            return output;
        }
    }
}
