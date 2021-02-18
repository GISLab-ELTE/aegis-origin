/// <copyright file="LasfileWriter.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2021 Roberto Giachetta. Licensed under the
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
/// <author>Dániel Tanos</author>

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Lasfile
{
    public class LasfileWriter : GeometryStreamWriter
    {
        private BinaryWriter _writer;

        public LasPublicHeader Header { get; set; }

        public LasfileWriter(string path,LasPublicHeader header)
            : base(path, GeometryStreamFormats.Lasfile, null)
        {
            Header = header;
            _writer = new BinaryWriter(_baseStream, Encoding.ASCII, true);
            WritePublicHeader();
        }

        protected override void ApplyWriteGeometry(IGeometry geometry)
        {            
            _writer.Write(Convert.ToInt32(((geometry as LasPointBase).X - Header.OffsetX) / Header.ScaleFactorX));

            _writer.Write(Convert.ToInt32(((geometry as LasPointBase).Y - Header.OffsetY) / Header.ScaleFactorY));

            _writer.Write(Convert.ToInt32(((geometry as LasPointBase).Z - Header.OffsetZ) / Header.ScaleFactorZ));

            _writer.Write((geometry as LasPointBase).Intensity);

            Int32 bitfield = 0b00000000;

            if (Header.PointDataFormat < 6)
            {
                bitfield = ((geometry as LasPointBase).ReturnNumber & 0b00000111) | bitfield;
                bitfield = ((geometry as LasPointBase).TotalReturnNumber & 0b00111000) | bitfield;

                if ((geometry as LasPointBase).IsScanDirectionPositive)
                    bitfield = 0b01000000 | bitfield;

                if ((geometry as LasPointBase).IsFlightLineEdge)
                    bitfield = 0b10000000 | bitfield;

                _writer.Write(Convert.ToByte(bitfield));

                bitfield = 0b00000000;

                bitfield = ((geometry as LasPointBase).Classification & 0b00011111) | bitfield;

                if ((geometry as LasPointBase).IsSynthetic)
                    bitfield = bitfield | 0b00100000;

                if ((geometry as LasPointBase).IsKeyPoint)
                    bitfield = bitfield | 0b01000000;

                if ((geometry as LasPointBase).IsWithheld)
                    bitfield = bitfield | 0b10000000;

                _writer.Write(Convert.ToByte(bitfield));

                _writer.Write((geometry as LasPointFormat0).ScanAngle);

                _writer.Write((geometry as LasPointBase).UserData);
            }
            else
            {
                bitfield = ((geometry as LasPointBase).ReturnNumber & 0b00001111) | bitfield;
                bitfield = ((geometry as LasPointBase).TotalReturnNumber  & 0b11110000) | bitfield;

                _writer.Write(Convert.ToByte(bitfield));

                bitfield = 0b00000000;

                if ((geometry as LasPointBase).IsSynthetic)
                    bitfield = bitfield | 0b00000001;

                if ((geometry as LasPointBase).IsKeyPoint)
                    bitfield = bitfield | 0b00000010;

                if ((geometry as LasPointBase).IsWithheld)
                    bitfield = bitfield | 0b00000100;

                if ((geometry as LasPointFormat6).IsOverlap)
                    bitfield = bitfield | 0b00001000;

                bitfield = ((geometry as LasPointFormat6).ScannerChannel & 0b00110000) | bitfield;

                if ((geometry as LasPointBase).IsScanDirectionPositive)
                    bitfield = bitfield | 0b01000000;

                if ((geometry as LasPointBase).IsFlightLineEdge)
                    bitfield = bitfield | 0b10000000;

                _writer.Write(Convert.ToByte(bitfield));

                _writer.Write((geometry as LasPointBase).Classification);

                _writer.Write((geometry as LasPointBase).UserData);

                _writer.Write((geometry as LasPointFormat6).ScanAngle);
            }

            _writer.Write((geometry as LasPointBase).PointSourceId);

            if (Header.PointDataFormat == 1 || Header.PointDataFormat == 4)
            {
                _writer.Write((geometry as LasPointFormat1).GPSTime);
            }
            else if (Header.PointDataFormat == 3 || Header.PointDataFormat == 5)
            {
                _writer.Write((geometry as LasPointFormat3).GPSTime);
            }
            else if (Header.PointDataFormat > 5)
            {
                _writer.Write((geometry as LasPointFormat6).GPSTime);
            }

            if (Header.PointDataFormat == 2 || Header.PointDataFormat == 3 || Header.PointDataFormat == 5)
            {
                _writer.Write((geometry as LasPointFormat2).RedChannel);

                _writer.Write((geometry as LasPointFormat2).GreenChannel);

                _writer.Write((geometry as LasPointFormat2).BlueChannel);
            }
            else if (Header.PointDataFormat == 7 || Header.PointDataFormat == 8 || Header.PointDataFormat == 10)
            {
                _writer.Write((geometry as LasPointFormat7).RedChannel);

                _writer.Write((geometry as LasPointFormat7).GreenChannel);

                _writer.Write((geometry as LasPointFormat7).BlueChannel);
            }

            if (Header.PointDataFormat == 8 || Header.PointDataFormat == 10)
            {
                _writer.Write((geometry as LasPointFormat8).NearInfraredChannel);
            }

            if (Header.PointDataFormat == 4)
            {
                _writer.Write((geometry as LasPointFormat4).WavePacketDescriptorIndex);

                _writer.Write((geometry as LasPointFormat4).WavePacketDataOffset);

                _writer.Write((geometry as LasPointFormat4).WavePacketSize);

                _writer.Write((geometry as LasPointFormat4).ReturnPointWaveformLocation);

                _writer.Write((geometry as LasPointFormat4).Xt);

                _writer.Write((geometry as LasPointFormat4).Yt);

                _writer.Write((geometry as LasPointFormat4).Zt);
            }
            else if (Header.PointDataFormat == 5)
            {
                _writer.Write((geometry as LasPointFormat5).WavePacketDescriptorIndex);

                _writer.Write((geometry as LasPointFormat5).WavePacketDataOffset);

                _writer.Write((geometry as LasPointFormat5).WavePacketSize);

                _writer.Write((geometry as LasPointFormat5).ReturnPointWaveformLocation);

                _writer.Write((geometry as LasPointFormat5).Xt);

                _writer.Write((geometry as LasPointFormat5).Yt);

                _writer.Write((geometry as LasPointFormat5).Zt);
            }
            if (Header.PointDataFormat == 9)
            {
                _writer.Write((geometry as LasPointFormat9).WavePacketDescriptorIndex);

                _writer.Write((geometry as LasPointFormat9).WavePacketDataOffset);

                _writer.Write((geometry as LasPointFormat9).WavePacketSize);

                _writer.Write((geometry as LasPointFormat9).ReturnPointWaveformLocation);

                _writer.Write((geometry as LasPointFormat9).Xt);

                _writer.Write((geometry as LasPointFormat9).Yt);

                _writer.Write((geometry as LasPointFormat9).Zt);
            }
            if (Header.PointDataFormat == 10)
            {
                _writer.Write((geometry as LasPointFormat10).WavePacketDescriptorIndex);

                _writer.Write((geometry as LasPointFormat10).WavePacketDataOffset);

                _writer.Write((geometry as LasPointFormat10).WavePacketSize);

                _writer.Write((geometry as LasPointFormat10).ReturnPointWaveformLocation);

                _writer.Write((geometry as LasPointFormat10).Xt);

                _writer.Write((geometry as LasPointFormat10).Yt);

                _writer.Write((geometry as LasPointFormat10).Zt);
            }
        }

        private void WritePublicHeader()
        {
            string s = "LASF";
            foreach(char c in s)
            {
                _writer.Write(c);
            }

            _writer.Write(Header.FileSourceId);

            int bitfield = 0b00000;

            if(Header.IsGPSStandardTime)
            {
                bitfield = bitfield | 0b00001;
            }

            if(Header.IsWavePacketStorageExternal)
            {
                bitfield = bitfield | 0b00100;
            }
            else if (Header.IsWavePacketStorageInternal)
            {
                bitfield = bitfield | 0b00010;
            }

            if(Header.IsReturnNumberOriginSynthetical)
            {
                bitfield = bitfield | 0b01000;
            }

            if(Header.IsCRSWKT)
            {
                bitfield = bitfield | 0b10000;
            }

            _writer.Write(Convert.ToUInt16(bitfield));

            _writer.Write(Header.ProjectId1);

            _writer.Write(Header.ProjectId2);

            _writer.Write(Header.ProjectId3);

            _writer.Write(Header.ProjectId4);

            _writer.Write(Header.VersionMajor);

            _writer.Write(Header.VersionMinor);

            s = Header.SystemIdentifier + new string('\0', 32 - Header.SystemIdentifier.Length);
            foreach (char c in s)
            {
                _writer.Write(c);
            }

            s = Header.GeneratingSoftware + new string('\0', 32 - Header.GeneratingSoftware.Length);
            foreach (char c in s)
            {
                _writer.Write(c);
            }

            _writer.Write(Header.FileCreationDay);

            _writer.Write(Header.FileCreationYear);

            _writer.Write(Header.PublicHeaderSize);

            _writer.Write(Convert.ToUInt32(Header.PublicHeaderSize)); //PointDataOffset, headers written by this program will have no extra information between header and data

            _writer.Write(Header.VariableLengthRecordCount);

            _writer.Write(Header.PointDataFormat);

            _writer.Write(Header.PointDataLength);

            _writer.Write(Convert.ToUInt32(Header.PointCount));

            for (Int32 i = 0; i < 5; i++)
            {
                _writer.Write(Convert.ToUInt32(Header.PointCountPerReturn[i]));
            }

            _writer.Write(Header.ScaleFactorX);

            _writer.Write(Header.ScaleFactorY);

            _writer.Write(Header.ScaleFactorZ);

            _writer.Write(Header.OffsetX);

            _writer.Write(Header.OffsetY);

            _writer.Write(Header.OffsetZ);

            _writer.Write(Header.MaxX);

            _writer.Write(Header.MinX);

            _writer.Write(Header.MaxY);

            _writer.Write(Header.MinY);

            _writer.Write(Header.MaxZ);

            _writer.Write(Header.MinZ);

            if (Header.VersionMajor == 1 && Header.VersionMinor < 3)
            {
                return;
            }

            _writer.Write(Header.WavePacketRecordOffset);

            if (Header.VersionMajor == 1 && Header.VersionMinor < 4)
            {
                return;
            }

            _writer.Write(Header.ExtendedVariableLengthRecordOffset);

            _writer.Write(Header.ExtendedVariableLengthRecordCount);

            if (Header.PointCount == 0)
            {
                _writer.Write(Header.PointCount);

                for (Int32 i = 0; i < 15; i++)
                {
                    _writer.Write(Header.PointCountPerReturn[i]);
                }
            }
            else
            {
                for (Int32 i = 0; i < 16; i++)
                {
                    UInt64 a = 0;
                    _writer.Write(a);
                }
            }
        }
    }
}
