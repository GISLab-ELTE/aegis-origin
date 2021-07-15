/// <copyright file="LasFileReaderTests.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Antal Tar</author>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ELTE.AEGIS.IO.Lasfile;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.IO.Lasfile
{
    /// <summary>
    /// An xUnit support class to create testing environment for unit testing.
    /// </summary>
    public class LasFileFixture : IDisposable
    {
        private readonly String _testFolder;

        public LasFileFixture()
        {
            _testFolder = Path.Combine(Path.GetTempPath(), "LasFileReaderTests");
            Directory.CreateDirectory(_testFolder);

            using (BinaryWriter writer = new BinaryWriter(File.Create(NotLasFilePath), Encoding.ASCII))
            {
                writer.Write(new Char[4] { 'F', 'A', 'K', 'E' });
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create(LegacyLasFilePath), Encoding.ASCII))
            {
                // Header.

                Char[] sysid = new Char[32];
                sysid[0] = 'O';
                sysid[1] = 'T';
                sysid[2] = 'H';
                sysid[3] = 'E';
                sysid[4] = 'R';

                Char[] gensw = new Char[32];
                gensw[0] = 'L';
                gensw[1] = 'a';
                gensw[2] = 's';
                gensw[3] = 'F';
                gensw[4] = 'i';
                gensw[5] = 'l';
                gensw[6] = 'e';
                gensw[7] = 'R';
                gensw[8] = 'e';
                gensw[9] = 'a';
                gensw[10] = 'd';
                gensw[11] = 'e';
                gensw[12] = 'r';
                gensw[13] = 'U';
                gensw[14] = 'n';
                gensw[15] = 'i';
                gensw[16] = 't';
                gensw[17] = 'T';
                gensw[18] = 'e';
                gensw[19] = 's';
                gensw[20] = 't';
                gensw[21] = 's';

                writer.Write(new Char[4] { 'L', 'A', 'S', 'F' });

                for (var i = 0; i < 4; i++)
                    writer.Write((Byte)0b00000000);

                writer.Write((UInt32)81);
                writer.Write((UInt16)50);
                writer.Write((UInt16)12);
                writer.Write((UInt64)974);
                writer.Write((Byte)1);
                writer.Write((Byte)0);
                writer.Write(sysid);
                writer.Write(gensw);
                writer.Write((UInt16)286);
                writer.Write((UInt16)2020);
                writer.Write((UInt16)227);
                writer.Write((UInt32)227);
                writer.Write((UInt32)0);
                writer.Write((Byte)0);
                writer.Write((UInt16)20);
                writer.Write((UInt32)5);
                writer.Write((UInt32)3);
                writer.Write((UInt32)2);

                for (var i = 2; i < 5; i++)
                    writer.Write((UInt32)0);

                writer.Write((Double)0.01);
                writer.Write((Double)0.05);
                writer.Write((Double)0.10);
                writer.Write((Double)(-0.50));
                writer.Write((Double)0.00);
                writer.Write((Double)0.50);
                writer.Write((Double)100);
                writer.Write((Double)0);
                writer.Write((Double)101);
                writer.Write((Double)1);
                writer.Write((Double)102);
                writer.Write((Double)2);

                // Point 1.

                writer.Write((Int32)100);
                writer.Write((Int32)20);
                writer.Write((Int32)10);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b01010001));
                writer.Write(Convert.ToByte(0b01100001));
                writer.Write((SByte)0);
                writer.Write((Byte)0);
                writer.Write((UInt16)0);

                // Point 2.

                writer.Write((Int32)200);
                writer.Write((Int32)40);
                writer.Write((Int32)20);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b01010010));
                writer.Write(Convert.ToByte(0b00100010));
                writer.Write((SByte)0);
                writer.Write((Byte)0);
                writer.Write((UInt16)0);

                // Point 3.

                writer.Write((Int32)300);
                writer.Write((Int32)60);
                writer.Write((Int32)30);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b01010001));
                writer.Write(Convert.ToByte(0b00100011));
                writer.Write((SByte)0);
                writer.Write((Byte)0);
                writer.Write((UInt16)0);

                // Point 4.

                writer.Write((Int32)400);
                writer.Write((Int32)80);
                writer.Write((Int32)40);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b01010010));
                writer.Write(Convert.ToByte(0b00100100));
                writer.Write((SByte)0);
                writer.Write((Byte)0);
                writer.Write((UInt16)0);

                // Point 5.

                writer.Write((Int32)500);
                writer.Write((Int32)100);
                writer.Write((Int32)50);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b11001001));
                writer.Write(Convert.ToByte(0b10100101));
                writer.Write((SByte)0);
                writer.Write((Byte)0);
                writer.Write((UInt16)0);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create(CurrentLasFilePath), Encoding.ASCII))
            {
                // Header.
                
                Char[] sysid = new Char[32];
                sysid[0] = 'O';
                sysid[1] = 'T';
                sysid[2] = 'H';
                sysid[3] = 'E';
                sysid[4] = 'R';

                Char[] gensw = new Char[32];
                gensw[0] = 'L';
                gensw[1] = 'a';
                gensw[2] = 's';
                gensw[3] = 'F';
                gensw[4] = 'i';
                gensw[5] = 'l';
                gensw[6] = 'e';
                gensw[7] = 'R';
                gensw[8] = 'e';
                gensw[9] = 'a';
                gensw[10] = 'd';
                gensw[11] = 'e';
                gensw[12] = 'r';
                gensw[13] = 'U';
                gensw[14] = 'n';
                gensw[15] = 'i';
                gensw[16] = 't';
                gensw[17] = 'T';
                gensw[18] = 'e';
                gensw[19] = 's';
                gensw[20] = 't';
                gensw[21] = 's';

                writer.Write(new Char[4] { 'L', 'A', 'S', 'F' });
                writer.Write((UInt16)39);
                writer.Write(Convert.ToUInt16(0b11001));
                writer.Write((UInt32)81);
                writer.Write((UInt16)50);
                writer.Write((UInt16)12);
                writer.Write((UInt64)974);
                writer.Write((Byte)1);
                writer.Write((Byte)4);
                writer.Write(sysid);
                writer.Write(gensw);
                writer.Write((UInt16)286);
                writer.Write((UInt16)2020);
                writer.Write((UInt16)375);
                writer.Write((UInt32)375);
                writer.Write((UInt32)0);
                writer.Write((Byte)7);
                writer.Write((UInt16)36);

                for (var i = 0; i < 6; i++)
                    writer.Write((UInt32)0);

                writer.Write((Double)0.01);
                writer.Write((Double)0.05);
                writer.Write((Double)0.10);
                writer.Write((Double)(-0.50));
                writer.Write((Double)0.00);
                writer.Write((Double)0.50);
                writer.Write((Double)100);
                writer.Write((Double)0);
                writer.Write((Double)101);
                writer.Write((Double)1);
                writer.Write((Double)102);
                writer.Write((Double)2);
                writer.Write((UInt64)0);
                writer.Write((UInt64)0);
                writer.Write((UInt32)0);
                writer.Write((UInt64)5);
                writer.Write((UInt64)3);
                writer.Write((UInt64)2);

                for (var i = 2; i < 15; i++)
                    writer.Write((UInt64)0);

                // Point 1.

                writer.Write((Int32)100);
                writer.Write((Int32)20);
                writer.Write((Int32)10);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100001));
                writer.Write(Convert.ToByte(0b01000011));
                writer.Write((Byte)1);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)1);
                writer.Write((UInt16)2);
                writer.Write((UInt16)3);

                // Point 2.

                writer.Write((Int32)200);
                writer.Write((Int32)40);
                writer.Write((Int32)20);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100010));
                writer.Write(Convert.ToByte(0b01000001));
                writer.Write((Byte)2);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)10);
                writer.Write((UInt16)20);
                writer.Write((UInt16)30);

                // Point 3.

                writer.Write((Int32)300);
                writer.Write((Int32)60);
                writer.Write((Int32)30);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100001));
                writer.Write(Convert.ToByte(0b01000001));
                writer.Write((Byte)3);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)1);
                writer.Write((UInt16)2);
                writer.Write((UInt16)3);

                // Point 4.

                writer.Write((Int32)400);
                writer.Write((Int32)80);
                writer.Write((Int32)40);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100010));
                writer.Write(Convert.ToByte(0b01000001));
                writer.Write((Byte)4);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)10);
                writer.Write((UInt16)20);
                writer.Write((UInt16)30);

                // Point 5.

                writer.Write((Int32)500);
                writer.Write((Int32)100);
                writer.Write((Int32)50);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00010001));
                writer.Write(Convert.ToByte(0b11011101));
                writer.Write((Byte)5);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)1);
                writer.Write((UInt16)2);
                writer.Write((UInt16)3);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create(FutureLasFilePath), Encoding.ASCII))
            {
                // Header.

                Char[] sysid = new Char[32];
                sysid[0] = 'O';
                sysid[1] = 'T';
                sysid[2] = 'H';
                sysid[3] = 'E';
                sysid[4] = 'R';

                Char[] gensw = new Char[32];
                gensw[0] = 'L';
                gensw[1] = 'a';
                gensw[2] = 's';
                gensw[3] = 'F';
                gensw[4] = 'i';
                gensw[5] = 'l';
                gensw[6] = 'e';
                gensw[7] = 'R';
                gensw[8] = 'e';
                gensw[9] = 'a';
                gensw[10] = 'd';
                gensw[11] = 'e';
                gensw[12] = 'r';
                gensw[13] = 'U';
                gensw[14] = 'n';
                gensw[15] = 'i';
                gensw[16] = 't';
                gensw[17] = 'T';
                gensw[18] = 'e';
                gensw[19] = 's';
                gensw[20] = 't';
                gensw[21] = 's';

                writer.Write(new Char[4] { 'L', 'A', 'S', 'F' });
                writer.Write((UInt16)39);
                writer.Write(Convert.ToUInt16(0b01100));
                writer.Write((UInt32)81);
                writer.Write((UInt16)50);
                writer.Write((UInt16)12);
                writer.Write((UInt64)974);
                writer.Write((Byte)1);
                writer.Write((Byte)5);
                writer.Write(sysid);
                writer.Write(gensw);
                writer.Write((UInt16)286);
                writer.Write((UInt16)2020);
                writer.Write((UInt16)376);
                writer.Write((UInt32)376);
                writer.Write((UInt32)0);
                writer.Write((Byte)11);
                writer.Write((UInt16)37);

                for (var i = 0; i < 6; i++)
                    writer.Write((UInt32)0);

                writer.Write((Double)0.01);
                writer.Write((Double)0.05);
                writer.Write((Double)0.10);
                writer.Write((Double)(-0.50));
                writer.Write((Double)0.00);
                writer.Write((Double)0.50);
                writer.Write((Double)100);
                writer.Write((Double)0);
                writer.Write((Double)101);
                writer.Write((Double)1);
                writer.Write((Double)102);
                writer.Write((Double)2);
                writer.Write((UInt64)0);
                writer.Write((UInt64)0);
                writer.Write((UInt32)0);
                writer.Write((UInt64)5);
                writer.Write((UInt64)3);
                writer.Write((UInt64)2);
                writer.Write((Byte)0);

                for (var i = 2; i < 15; i++)
                    writer.Write((UInt64)0);

                // Point 1.

                writer.Write((Int32)100);
                writer.Write((Int32)20);
                writer.Write((Int32)10);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100001));
                writer.Write(Convert.ToByte(0b01000011));
                writer.Write((Byte)1);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)1);
                writer.Write((UInt16)2);
                writer.Write((UInt16)3);
                writer.Write((Byte)0);

                // Point 2.

                writer.Write((Int32)200);
                writer.Write((Int32)40);
                writer.Write((Int32)20);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100010));
                writer.Write(Convert.ToByte(0b01000001));
                writer.Write((Byte)2);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)10);
                writer.Write((UInt16)20);
                writer.Write((UInt16)30);
                writer.Write((Byte)0);

                // Point 3.

                writer.Write((Int32)300);
                writer.Write((Int32)60);
                writer.Write((Int32)30);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100001));
                writer.Write(Convert.ToByte(0b01000001));
                writer.Write((Byte)3);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)1);
                writer.Write((UInt16)2);
                writer.Write((UInt16)3);
                writer.Write((Byte)0);

                // Point 4.

                writer.Write((Int32)400);
                writer.Write((Int32)80);
                writer.Write((Int32)40);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00100010));
                writer.Write(Convert.ToByte(0b01000001));
                writer.Write((Byte)4);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)10);
                writer.Write((UInt16)20);
                writer.Write((UInt16)30);
                writer.Write((Byte)0);

                // Point 5.

                writer.Write((Int32)500);
                writer.Write((Int32)100);
                writer.Write((Int32)50);
                writer.Write((UInt16)0);
                writer.Write(Convert.ToByte(0b00010001));
                writer.Write(Convert.ToByte(0b11001101));
                writer.Write((Byte)5);
                writer.Write((Byte)0);
                writer.Write((Int16)0);
                writer.Write((UInt16)0);
                writer.Write((Double)0);
                writer.Write((UInt16)1);
                writer.Write((UInt16)2);
                writer.Write((UInt16)3);
                writer.Write((Byte)0);
            }
        }

        public void Dispose()
        {
            File.Delete(NotLasFilePath);
            File.Delete(LegacyLasFilePath);
            File.Delete(CurrentLasFilePath);
            File.Delete(FutureLasFilePath);
            Directory.Delete(_testFolder);
        }

        public String NotLasFilePath { get => Path.Combine(_testFolder, "notlas.fake"); }

        public String LegacyLasFilePath { get => Path.Combine(_testFolder, "legacy.las"); }

        public String CurrentLasFilePath { get => Path.Combine(_testFolder, "current.las"); }

        public String FutureLasFilePath { get => Path.Combine(_testFolder, "future.las"); }
    }

    /// <summary>
    /// The <see cref="LasFileReader"/> unit testing class.
    /// </summary>
    [TestFixture]
    public class LasFileReaderTests
    {
        private LasFileFixture _fixture;

        [OneTimeSetUp]
        public void Init()
        {
            _fixture = new LasFileFixture();
        }

        [Test]
        public void OpenFileNotSupportedFormatTest()
        {
            Boolean result = false;

            try
            {
                LasFileReader reader = new LasFileReader("file://" + _fixture.NotLasFilePath);
            }
            catch (InvalidDataException)
            {
                result = true;
            }

            Assert.True(result);
        }

        [Test]
        public void OpenFileInUseTest()
        {
            using (File.OpenWrite(new Uri(_fixture.CurrentLasFilePath).AbsolutePath))
            {
                Boolean result = false;

                try
                {
                    LasFileReader reader = new LasFileReader("file://" + _fixture.CurrentLasFilePath);
                }
                catch (IOException)
                {
                    result = true;
                }

                Assert.True(result);
            }
        }

        [Test]
        public void OpenFileAndGetHeaderTest()
        {
            using (LasFileReader reader = new LasFileReader("file://" + _fixture.CurrentLasFilePath))
            {
                LasPublicHeader header = reader.Header;

                Assert.AreEqual(AEGIS.IO.GeometryStreamFormats.Lasfile, reader.Format);
                Assert.AreEqual((UInt16)39, header.FileSourceId);
                Assert.AreEqual(GPSTimeType.Standard, header.GPSTimeType);
                Assert.False(header.IsWavePacketStorageInternal);
                Assert.False(header.IsWavePacketStorageExternal);
                Assert.True(header.IsReturnNumberOriginSynthetical);
                Assert.AreEqual(CRSType.WKT, header.CRSType);
                Assert.AreEqual((UInt32)81, header.ProjectId1);
                Assert.AreEqual((UInt16)50, header.ProjectId2);
                Assert.AreEqual((UInt16)12, header.ProjectId3);
                Assert.AreEqual((UInt64)974, header.ProjectId4);
                Assert.AreEqual((Byte)1, header.VersionMajor);
                Assert.AreEqual((Byte)4, header.VersionMinor);
                Assert.AreEqual("OTHER", header.SystemIdentifier);
                Assert.AreEqual("LasFileReaderUnitTests", header.GeneratingSoftware);
                Assert.AreEqual((UInt16)286, header.FileCreationDay);
                Assert.AreEqual((UInt16)2020, header.FileCreationYear);
                Assert.AreEqual((UInt16)375, header.PublicHeaderSize);
                Assert.AreEqual((UInt32)375, header.PointDataOffset);
                Assert.AreEqual((UInt32)0, header.VariableLengthRecordCount);
                Assert.AreEqual((Byte)7, header.PointDataFormat);
                Assert.AreEqual((UInt16)36, header.PointDataLength);
                Assert.AreEqual((UInt32)0, header.LegacyPointCount);

                for (var i = 0; i < 5; i++)
                    Assert.AreEqual((UInt32)0, header.LegacyPointCountPerReturn[i]);

                Assert.AreEqual((Double)0.01, header.ScaleFactorX);
                Assert.AreEqual((Double)0.05, header.ScaleFactorY);
                Assert.AreEqual((Double)0.10, header.ScaleFactorZ);
                Assert.AreEqual((Double)(-0.50), header.OffsetX);
                Assert.AreEqual((Double)0.00, header.OffsetY);
                Assert.AreEqual((Double)0.50, header.OffsetZ);
                Assert.AreEqual((Double)100, header.MaxX);
                Assert.AreEqual((Double)0, header.MinX);
                Assert.AreEqual((Double)101, header.MaxY);
                Assert.AreEqual((Double)1, header.MinY);
                Assert.AreEqual((Double)102, header.MaxZ);
                Assert.AreEqual((Double)2, header.MinZ);
                Assert.AreEqual((UInt64)0, header.WavePacketRecordOffset);
                Assert.AreEqual((UInt64)0, header.ExtendedVariableLengthRecordOffset);
                Assert.AreEqual((UInt32)0, header.ExtendedVariableLengthRecordCount);
                Assert.AreEqual((UInt64)5, header.PointCount);
                Assert.AreEqual((UInt64)3, header.PointCountPerReturn[0]);
                Assert.AreEqual((UInt64)2, header.PointCountPerReturn[1]);
                    
                for (var i = 2; i < 15; i++)
                    Assert.AreEqual((UInt64)0, header.PointCountPerReturn[i]);
            }
        }

        [Test]
        public void ReadPointTest()
        {
            using (LasFileReader reader = new LasFileReader("file://" + _fixture.CurrentLasFilePath))
            {
                Assert.AreEqual((UInt64)0, reader.ReadedPointCount);
                Assert.AreEqual((UInt64)5, reader.TotalPointCount);
                Assert.False(reader.EndOfStream);

                // Point 1.

                LasPointFormat7 point = reader.Read() as LasPointFormat7;
                Assert.AreEqual((UInt64)1, reader.ReadedPointCount);
                Assert.AreEqual((UInt64)5, reader.TotalPointCount);
                Assert.False(reader.EndOfStream);

                Assert.NotNull(point);
                Assert.AreEqual((Double)0.50, point.X);
                Assert.AreEqual((Double)1.00, point.Y);
                Assert.AreEqual((Double)1.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)1, point.ReturnNumber);
                Assert.AreEqual((Byte)2, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.True(point.IsKeyPoint);
                Assert.False(point.IsWithheld);
                Assert.False(point.IsOverlap);
                Assert.AreEqual((Byte)0, point.ScannerChannel);
                Assert.True(point.IsScanDirectionPositive);
                Assert.False(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)1, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);
                Assert.AreEqual((Double)0, point.GPSTime);
                Assert.AreEqual((UInt16)1, point.RedChannel);
                Assert.AreEqual((UInt16)2, point.GreenChannel);
                Assert.AreEqual((UInt16)3, point.BlueChannel);

                // Point 2.

                point = reader.Read() as LasPointFormat7;
                Assert.AreEqual((UInt64)2, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                Assert.NotNull(point);
                Assert.AreEqual((Double)1.50, point.X);
                Assert.AreEqual((Double)2.00, point.Y);
                Assert.AreEqual((Double)2.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)2, point.ReturnNumber);
                Assert.AreEqual((Byte)2, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.False(point.IsKeyPoint);
                Assert.False(point.IsWithheld);
                Assert.False(point.IsOverlap);
                Assert.AreEqual((Byte)0, point.ScannerChannel);
                Assert.True(point.IsScanDirectionPositive);
                Assert.False(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)2, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);
                Assert.AreEqual((Double)0, point.GPSTime);
                Assert.AreEqual((UInt16)10, point.RedChannel);
                Assert.AreEqual((UInt16)20, point.GreenChannel);
                Assert.AreEqual((UInt16)30, point.BlueChannel);

                reader.Read();
                Assert.AreEqual((UInt64)3, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                reader.Read();
                Assert.AreEqual((UInt64)4, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                // Point 5.

                point = reader.Read() as LasPointFormat7;
                Assert.AreEqual((UInt64)5, reader.ReadedPointCount);
                Assert.True(reader.EndOfStream);

                Assert.NotNull(point);
                Assert.AreEqual((Double)4.50, point.X);
                Assert.AreEqual((Double)5.00, point.Y);
                Assert.AreEqual((Double)5.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)1, point.ReturnNumber);
                Assert.AreEqual((Byte)1, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.False(point.IsKeyPoint);
                Assert.True(point.IsWithheld);
                Assert.True(point.IsOverlap);
                Assert.AreEqual((Byte)1, point.ScannerChannel);
                Assert.True(point.IsScanDirectionPositive);
                Assert.True(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)5, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);
                Assert.AreEqual((Double)0, point.GPSTime);
                Assert.AreEqual((UInt16)1, point.RedChannel);
                Assert.AreEqual((UInt16)2, point.GreenChannel);
                Assert.AreEqual((UInt16)3, point.BlueChannel);
                
                Boolean result = false;

                try
                {
                    reader.Read();
                }
                catch (InvalidDataException)
                {
                    result = true;
                }

                Assert.True(result);
            }
        }

        [Test]
        public void ReadMultiplePointsTest()
        {
            using (LasFileReader reader = new LasFileReader("file://" + _fixture.CurrentLasFilePath))
            {
                Assert.AreEqual((UInt64)0, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                IList<IGeometry> points = reader.Read(2);
                Assert.AreEqual(2, points.Count);
                Assert.AreEqual((UInt64)2, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                LasPointFormat7 point = points[0] as LasPointFormat7;
                Assert.NotNull(point);
                Assert.AreEqual((Double)0.50, point.X);
                Assert.AreEqual((Double)1.00, point.Y);
                Assert.AreEqual((Double)1.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)1, point.ReturnNumber);
                Assert.AreEqual((Byte)2, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.True(point.IsKeyPoint);
                Assert.False(point.IsWithheld);
                Assert.False(point.IsOverlap);
                Assert.AreEqual((Byte)0, point.ScannerChannel);
                Assert.True(point.IsScanDirectionPositive);
                Assert.False(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)1, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);
                Assert.AreEqual((Double)0, point.GPSTime);
                Assert.AreEqual((UInt16)1, point.RedChannel);
                Assert.AreEqual((UInt16)2, point.GreenChannel);
                Assert.AreEqual((UInt16)3, point.BlueChannel);

                points = reader.ReadToEnd();
                Assert.AreEqual(3, points.Count);
                Assert.AreEqual((UInt64)5, reader.ReadedPointCount);
                Assert.True(reader.EndOfStream);

                point = points[2] as LasPointFormat7;
                Assert.NotNull(point);
                Assert.AreEqual((Double)4.50, point.X);
                Assert.AreEqual((Double)5.00, point.Y);
                Assert.AreEqual((Double)5.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)1, point.ReturnNumber);
                Assert.AreEqual((Byte)1, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.False(point.IsKeyPoint);
                Assert.True(point.IsWithheld);
                Assert.True(point.IsOverlap);
                Assert.AreEqual((Byte)1, point.ScannerChannel);
                Assert.True(point.IsScanDirectionPositive);
                Assert.True(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)5, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);
                Assert.AreEqual((Double)0, point.GPSTime);
                Assert.AreEqual((UInt16)1, point.RedChannel);
                Assert.AreEqual((UInt16)2, point.GreenChannel);
                Assert.AreEqual((UInt16)3, point.BlueChannel);
            }
        }

        [Test]
        public void OpenFileLegacyFormatAndGetHeader()
        {
            using (LasFileReader reader = new LasFileReader("file://" + _fixture.LegacyLasFilePath))
            {
                LasPublicHeader header = reader.Header;

                Assert.AreEqual((UInt32)81, header.ProjectId1);
                Assert.AreEqual((UInt16)50, header.ProjectId2);
                Assert.AreEqual((UInt16)12, header.ProjectId3);
                Assert.AreEqual((UInt64)974, header.ProjectId4);
                Assert.AreEqual((Byte)1, header.VersionMajor);
                Assert.AreEqual((Byte)0, header.VersionMinor);
                Assert.AreEqual("OTHER", header.SystemIdentifier);
                Assert.AreEqual("LasFileReaderUnitTests", header.GeneratingSoftware);
                Assert.AreEqual((UInt16)286, header.FileCreationDay);
                Assert.AreEqual((UInt16)2020, header.FileCreationYear);
                Assert.AreEqual((UInt16)227, header.PublicHeaderSize);
                Assert.AreEqual((UInt32)227, header.PointDataOffset);
                Assert.AreEqual((UInt32)0, header.VariableLengthRecordCount);
                Assert.AreEqual((Byte)0, header.PointDataFormat);
                Assert.AreEqual((UInt16)20, header.PointDataLength);
                Assert.AreEqual((UInt32)5, header.LegacyPointCount);
                Assert.AreEqual((UInt32)3, header.LegacyPointCountPerReturn[0]);
                Assert.AreEqual((UInt32)2, header.LegacyPointCountPerReturn[1]);

                for (var i = 2; i < 5; i++)
                    Assert.AreEqual((UInt32)0, header.LegacyPointCountPerReturn[i]);

                Assert.AreEqual((Double)0.01, header.ScaleFactorX);
                Assert.AreEqual((Double)0.05, header.ScaleFactorY);
                Assert.AreEqual((Double)0.10, header.ScaleFactorZ);
                Assert.AreEqual((Double)(-0.50), header.OffsetX);
                Assert.AreEqual((Double)0.00, header.OffsetY);
                Assert.AreEqual((Double)0.50, header.OffsetZ);
                Assert.AreEqual((Double)100, header.MaxX);
                Assert.AreEqual((Double)0, header.MinX);
                Assert.AreEqual((Double)101, header.MaxY);
                Assert.AreEqual((Double)1, header.MinY);
                Assert.AreEqual((Double)102, header.MaxZ);
                Assert.AreEqual((Double)2, header.MinZ);
                Assert.AreEqual((UInt64)0, header.WavePacketRecordOffset);
                Assert.AreEqual((UInt64)0, header.ExtendedVariableLengthRecordOffset);
                Assert.AreEqual((UInt32)0, header.ExtendedVariableLengthRecordCount);
                Assert.AreEqual((UInt64)5, header.PointCount);
                Assert.AreEqual((UInt64)3, header.PointCountPerReturn[0]);
                Assert.AreEqual((UInt64)2, header.PointCountPerReturn[1]);

                for (var i = 2; i < 15; i++)
                    Assert.AreEqual((UInt64)0, header.PointCountPerReturn[i]);
            }
        }

        [Test]
        public void ReadLegacyPointTest()
        {
            using (LasFileReader reader = new LasFileReader("file://" + _fixture.LegacyLasFilePath))
            {
                Assert.AreEqual((UInt64)0, reader.ReadedPointCount);
                Assert.AreEqual((UInt64)5, reader.TotalPointCount);
                Assert.False(reader.EndOfStream);

                // Point 1.

                LasPointFormat0 point = reader.Read() as LasPointFormat0;
                Assert.AreEqual((UInt64)1, reader.ReadedPointCount);
                Assert.AreEqual((UInt64)5, reader.TotalPointCount);
                Assert.False(reader.EndOfStream);

                Assert.NotNull(point);
                Assert.AreEqual((Double)0.50, point.X);
                Assert.AreEqual((Double)1.00, point.Y);
                Assert.AreEqual((Double)1.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)1, point.ReturnNumber);
                Assert.AreEqual((Byte)2, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.True(point.IsKeyPoint);
                Assert.False(point.IsWithheld);
                Assert.True(point.IsScanDirectionPositive);
                Assert.False(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)1, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);

                // Point 2.

                point = reader.Read() as LasPointFormat0;
                Assert.AreEqual((UInt64)2, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                Assert.NotNull(point);
                Assert.AreEqual((Double)1.50, point.X);
                Assert.AreEqual((Double)2.00, point.Y);
                Assert.AreEqual((Double)2.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)2, point.ReturnNumber);
                Assert.AreEqual((Byte)2, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.False(point.IsKeyPoint);
                Assert.False(point.IsWithheld);
                Assert.True(point.IsScanDirectionPositive);
                Assert.False(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)2, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);

                reader.Read();
                Assert.AreEqual((UInt64)3, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                reader.Read();
                Assert.AreEqual((UInt64)4, reader.ReadedPointCount);
                Assert.False(reader.EndOfStream);

                // Point 5.

                point = reader.Read() as LasPointFormat0;
                Assert.AreEqual((UInt64)5, reader.ReadedPointCount);
                Assert.True(reader.EndOfStream);

                Assert.NotNull(point);
                Assert.AreEqual((Double)4.50, point.X);
                Assert.AreEqual((Double)5.00, point.Y);
                Assert.AreEqual((Double)5.50, point.Z);
                Assert.AreEqual((UInt16)0, point.Intensity);
                Assert.AreEqual((Byte)1, point.ReturnNumber);
                Assert.AreEqual((Byte)1, point.TotalReturnNumber);
                Assert.True(point.IsSynthetic);
                Assert.False(point.IsKeyPoint);
                Assert.True(point.IsWithheld);
                Assert.True(point.IsScanDirectionPositive);
                Assert.True(point.IsFlightLineEdge);
                Assert.AreEqual((Byte)5, point.Classification);
                Assert.AreEqual((Byte)0, point.UserData);
                Assert.AreEqual((Int16)0, point.ScanAngle);
                Assert.AreEqual((UInt16)0, point.PointSourceId);

                Boolean result = false;

                try
                {
                    reader.Read();
                }
                catch (InvalidDataException)
                {
                    result = true;
                }

                Assert.True(result);
            }
        }

        [Test]
        public void OpenFileNotSupportedPointVersionAndReadTest()
        {
            using (LasFileReader reader = new LasFileReader("file://" + _fixture.FutureLasFilePath))
            {
                LasPublicHeader header = reader.Header;

                Assert.AreEqual((UInt16)39, header.FileSourceId);
                Assert.AreEqual(GPSTimeType.Week, header.GPSTimeType);
                Assert.False(header.IsWavePacketStorageInternal);
                Assert.True(header.IsWavePacketStorageExternal);
                Assert.True(header.IsReturnNumberOriginSynthetical);
                Assert.AreEqual(CRSType.GeoTIFF, header.CRSType);
                Assert.AreEqual((UInt32)81, header.ProjectId1);
                Assert.AreEqual((UInt16)50, header.ProjectId2);
                Assert.AreEqual((UInt16)12, header.ProjectId3);
                Assert.AreEqual((UInt64)974, header.ProjectId4);
                Assert.AreEqual((Byte)1, header.VersionMajor);
                Assert.AreEqual((Byte)5, header.VersionMinor);
                Assert.AreEqual("OTHER", header.SystemIdentifier);
                Assert.AreEqual("LasFileReaderUnitTests", header.GeneratingSoftware);
                Assert.AreEqual((UInt16)286, header.FileCreationDay);
                Assert.AreEqual((UInt16)2020, header.FileCreationYear);
                Assert.AreEqual((UInt16)376, header.PublicHeaderSize);
                Assert.AreEqual((UInt32)376, header.PointDataOffset);
                Assert.AreEqual((UInt32)0, header.VariableLengthRecordCount);
                Assert.AreEqual((Byte)11, header.PointDataFormat);
                Assert.AreEqual((UInt16)37, header.PointDataLength);
                Assert.AreEqual((UInt32)0, header.LegacyPointCount);

                for (var i = 0; i < 5; i++)
                    Assert.AreEqual((UInt32)0, header.LegacyPointCountPerReturn[i]);

                Assert.AreEqual((Double)0.01, header.ScaleFactorX);
                Assert.AreEqual((Double)0.05, header.ScaleFactorY);
                Assert.AreEqual((Double)0.10, header.ScaleFactorZ);
                Assert.AreEqual((Double)(-0.50), header.OffsetX);
                Assert.AreEqual((Double)0.00, header.OffsetY);
                Assert.AreEqual((Double)0.50, header.OffsetZ);
                Assert.AreEqual((Double)100, header.MaxX);
                Assert.AreEqual((Double)0, header.MinX);
                Assert.AreEqual((Double)101, header.MaxY);
                Assert.AreEqual((Double)1, header.MinY);
                Assert.AreEqual((Double)102, header.MaxZ);
                Assert.AreEqual((Double)2, header.MinZ);
                Assert.AreEqual((UInt64)0, header.WavePacketRecordOffset);
                Assert.AreEqual((UInt64)0, header.ExtendedVariableLengthRecordOffset);
                Assert.AreEqual((UInt32)0, header.ExtendedVariableLengthRecordCount);
                Assert.AreEqual((UInt64)5, header.PointCount);
                Assert.AreEqual((UInt64)3, header.PointCountPerReturn[0]);
                Assert.AreEqual((UInt64)2, header.PointCountPerReturn[1]);

                for (var i = 2; i < 15; i++)
                    Assert.AreEqual((UInt64)0, header.PointCountPerReturn[i]);

                Boolean result = false;

                try
                {
                    reader.Read();
                }
                catch (InvalidDataException)
                {
                    result = true;
                }

                Assert.True(result);
            }
        }
    }
}
