// <copyright file="LasPublicHeaderTests.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using ELTE.AEGIS.IO.Lasfile;
using NUnit.Framework;

namespace ELTE.AEGIS.Tests.IO.Lasfile
{
    /// <summary>
    /// The <see cref="LasPublicHeader"/> unit testing class.
    /// </summary>
    /// <author>Antal Tar</author>
    public class LasPublicHeaderTests
    {
        private LasPublicHeader _header;

        [SetUp]
        public void SetUp()
        {
            // Initialize the header with default values.
            _header = new LasPublicHeader();
        }

        [Test]
        public void GPSTimeTypeTest()
        {
            // Default GPS Time settings.
            Assert.True(_header.IsGPSWeekTime);
            Assert.False(_header.IsGPSStandardTime);

            // Set the false to true.
            _header.IsGPSStandardTime = true;
            Assert.False(_header.IsGPSWeekTime);
            Assert.True(_header.IsGPSStandardTime);

            // Set the false to false.
            _header.IsGPSWeekTime = false;
            Assert.False(_header.IsGPSWeekTime);
            Assert.True(_header.IsGPSStandardTime);

            // Set the true to false.
            _header.IsGPSStandardTime = false;
            Assert.True(_header.IsGPSWeekTime);
            Assert.False(_header.IsGPSStandardTime);
        }

        [Test]
        public void WavePacketStorageTest()
        {
            // Default wawe packet location settings.
            Assert.False(_header.IsWavePacketStorageInternal);
            Assert.False(_header.IsWavePacketStorageExternal);

            // While both false set one to true.
            _header.IsWavePacketStorageExternal = true;
            Assert.False(_header.IsWavePacketStorageInternal);
            Assert.True(_header.IsWavePacketStorageExternal);

            // Set the false to true.
            _header.IsWavePacketStorageInternal = true;
            Assert.True(_header.IsWavePacketStorageInternal);
            Assert.False(_header.IsWavePacketStorageExternal);

            // Set the false to false.
            _header.IsWavePacketStorageExternal = false;
            Assert.True(_header.IsWavePacketStorageInternal);
            Assert.False(_header.IsWavePacketStorageExternal);

            // Set the true to false.
            _header.IsWavePacketStorageInternal = false;
            Assert.False(_header.IsWavePacketStorageInternal);
            Assert.False(_header.IsWavePacketStorageExternal);
        }

        [Test]
        public void CRSTypeTest()
        {
            // Default CRS settings.
            Assert.True(_header.IsCRSWKT);
            Assert.False(_header.IsCRSGeoTIFF);

            // Set the false to true.
            _header.IsCRSGeoTIFF = true;
            Assert.False(_header.IsCRSWKT);
            Assert.True(_header.IsCRSGeoTIFF);

            // Set the false to false.
            _header.IsCRSWKT = false;
            Assert.False(_header.IsCRSWKT);
            Assert.True(_header.IsCRSGeoTIFF);

            // Set the false to true.
            _header.IsCRSGeoTIFF = false;
            Assert.True(_header.IsCRSWKT);
            Assert.False(_header.IsCRSGeoTIFF);
        }

        [Test]
        public void SystemIdentifierTest()
        {
            // Default system identifier settings.
            Assert.AreEqual(String.Empty, _header.SystemIdentifier);

            // Set to maximum valid length.
            _header.SystemIdentifier = "OooooOooooOooooOooooOooooOooooOo";
            Assert.AreEqual("OooooOooooOooooOooooOooooOooooOo", _header.SystemIdentifier);

            // Set to null.
            _header.SystemIdentifier = null;
            Assert.AreEqual(String.Empty, _header.SystemIdentifier);

            // Set to minimum invalid length.
            Boolean result = false;

            try
            {
                _header.SystemIdentifier = "OooooOooooOooooOooooOooooOooooOoX";
            }
            catch (ArgumentException)
            {
                result = true;
            }

            Assert.AreEqual(String.Empty, _header.SystemIdentifier);
            Assert.True(result);
        }

        [Test]
        public void GeneratingSoftwareTest()
        {
            // Default generating software settings.
            Assert.AreEqual(String.Empty, _header.GeneratingSoftware);

            // Set to maximum valid length.
            _header.GeneratingSoftware = "OooooOooooOooooOooooOooooOooooOo";
            Assert.AreEqual("OooooOooooOooooOooooOooooOooooOo", _header.GeneratingSoftware);

            // Set to null.
            _header.GeneratingSoftware = null;
            Assert.AreEqual(String.Empty, _header.GeneratingSoftware);

            // Set to minimum invalid length.
            Boolean result = false;

            try
            {
                _header.GeneratingSoftware = "OooooOooooOooooOooooOooooOooooOoX";
            }
            catch (ArgumentException)
            {
                result = true;
            }

            Assert.AreEqual(String.Empty, _header.GeneratingSoftware);
            Assert.True(result);
        }

        [Test]
        public void FileCreationDateTest()
        {
            // Default file creation settings.
            Assert.AreEqual(0, _header.FileCreationDay);
            Assert.AreEqual(0, _header.FileCreationYear);
            Assert.Null(_header.FileCreationDate);

            // Set the file creation day to 0 and the file creation year to 2020.
            // This is an invalid file creation date.
            _header.FileCreationDay = 0;
            _header.FileCreationYear = 2020;
            Assert.AreEqual(0, _header.FileCreationDay);
            Assert.AreEqual(2020, _header.FileCreationYear);
            Assert.Null(_header.FileCreationDate);

            // Set the file creation day to 283 and the file creation year to 0.
            // This is an invalid file creation date.
            _header.FileCreationDay = 283;
            _header.FileCreationYear = 0;
            Assert.AreEqual(283, _header.FileCreationDay);
            Assert.AreEqual(0, _header.FileCreationYear);
            Assert.Null(_header.FileCreationDate);

            // Set the file creation day to 283 and the file creation year to 2020.
            // This is a valid file creation date.
            _header.FileCreationDay = 283;
            _header.FileCreationYear = 2020;
            Assert.AreEqual(283, _header.FileCreationDay);
            Assert.AreEqual(2020, _header.FileCreationYear);
            Assert.AreEqual(new DateTime(2020, 10, 9), _header.FileCreationDate);

            // Set file creation date to 1995. 06. 16.
            // This is a valid file creation date.
            _header.FileCreationDate = new DateTime(1995, 6, 16);
            Assert.AreEqual(167, _header.FileCreationDay);
            Assert.AreEqual(1995, _header.FileCreationYear);
            Assert.AreEqual(new DateTime(1995, 6, 16), _header.FileCreationDate);

            // Set file creation day to 500.
            // This is an invalid file creation date.
            _header.FileCreationDay = 500;
            Assert.AreEqual(500, _header.FileCreationDay);
            Assert.AreEqual(1995, _header.FileCreationYear);
            Assert.Null(_header.FileCreationDate);
        }

        [Test]
        public void PointCountTest()
        {
            // Default point count settings.
            Assert.AreEqual((UInt64)0, _header.PointCount);
            Assert.AreEqual((UInt32)0, _header.LegacyPointCount);

            // Set to maximum valid legacy value.
            _header.PointCount = (UInt64)UInt32.MaxValue;
            Assert.AreEqual((UInt64)UInt32.MaxValue, _header.PointCount);
            Assert.AreEqual(UInt32.MaxValue, _header.LegacyPointCount);

            // Set to minimum invalid legacy value.
            _header.PointCount = (UInt64)UInt32.MaxValue + 1;
            Assert.AreEqual((UInt64)UInt32.MaxValue + 1, _header.PointCount);
            Assert.AreEqual((UInt32)0, _header.LegacyPointCount);

            // Set to maximum valid value.
            _header.PointCount = UInt64.MaxValue;
            Assert.AreEqual(UInt64.MaxValue, _header.PointCount);
            Assert.AreEqual((UInt32)0, _header.LegacyPointCount);

            // While point count is 1 set point data format to minimum invalid legacy value.
            _header.PointCount = 1;
            _header.PointDataFormat = 6;
            Assert.AreEqual((UInt64)1, _header.PointCount);
            Assert.AreEqual((UInt32)0, _header.LegacyPointCount);
        }

        [Test]
        public void PointCountPerReturnTest()
        {
            // Default point count per return settings.
            Assert.AreEqual
                (new List<UInt64>(15) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 0, 0, 0, 0, 0 }, _header.LegacyPointCountPerReturn);

            // Set to null.
            Boolean result = false;

            try
            {
                _header.PointCountPerReturn = null;
            }
            catch (ArgumentNullException)
            {
                result = true;
            }

            Assert.AreEqual
                (new List<UInt64>(15) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 0, 0, 0, 0, 0 }, _header.LegacyPointCountPerReturn);
            Assert.True(result);

            // Set to maximum valid length and the last element to maximum valid value.
            _header.PointCountPerReturn =
                new List<UInt64>(15) { 2, 0, 1, 5, 8, 8, 8, 8, 8, 3, 9, 3, 9, 0, UInt64.MaxValue };
            Assert.AreEqual(new List<UInt64>(15) { 2, 0, 1, 5, 8, 8, 8, 8, 8, 3, 9, 3, 9, 0, UInt64.MaxValue },
                _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 2, 0, 1, 5, 8 }, _header.LegacyPointCountPerReturn);

            // Set the first five element, the fifth element to maximum valid legacy value.
            _header.PointCountPerReturn = new List<UInt64>(15) { 2, 0, 2, 0, UInt32.MaxValue };
            Assert.AreEqual(new List<UInt64>(15) { 2, 0, 2, 0, UInt32.MaxValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 2, 0, 2, 0, UInt32.MaxValue }, _header.LegacyPointCountPerReturn);

            // Set to minimum invalid length.
            result = false;

            try
            {
                _header.PointCountPerReturn =
                    new List<UInt64>(15) { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            }
            catch (ArgumentException)
            {
                result = true;
            }

            Assert.AreEqual(new List<UInt64>(15) { 2, 0, 2, 0, UInt32.MaxValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 2, 0, 2, 0, UInt32.MaxValue }, _header.LegacyPointCountPerReturn);
            Assert.True(result);

            // Set the point count to maximum valid legacy value.
            _header.PointCount = (UInt64)UInt32.MaxValue;
            Assert.AreEqual(new List<UInt32>(5) { 2, 0, 2, 0, UInt32.MaxValue }, _header.LegacyPointCountPerReturn);

            // Set the point count to minimum invalid legacy value.
            _header.PointCount = (UInt64)UInt32.MaxValue + 1;
            Assert.AreEqual(new List<UInt32>(5) { 0, 0, 0, 0, 0 }, _header.LegacyPointCountPerReturn);

            // Set the point count to maximum valid legacy value
            // and the fifth element to minimum invalid legacy value.
            _header.PointCount = (UInt64)UInt32.MaxValue;
            _header.PointCountPerReturn[4] = (UInt64)UInt32.MaxValue + 1;
            Assert.AreEqual(new List<UInt64>(15) { 2, 0, 2, 0, (UInt64)UInt32.MaxValue + 1,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 0, 0, 0, 0, 0 }, _header.LegacyPointCountPerReturn);

            // While point count is 15 and all element is 1
            // set point data format to minimum invalid legacy value.
            _header.PointCount = 15;
            _header.PointCountPerReturn =
                    new List<UInt64>(15) { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            _header.PointDataFormat = 6;
            Assert.AreEqual(new List<UInt64>(15) { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                _header.PointCountPerReturn);
            Assert.AreEqual(new List<UInt32>(5) { 0, 0, 0, 0, 0 }, _header.LegacyPointCountPerReturn);
        }

        [Test]
        public void GetSetPointCountTest()
        {
            // While default settings get all valid indexes.
            for (var i = 0; i < 15; i++)
            {
                Assert.AreEqual((UInt64)0, _header.GetPointCount(i));

                if (i < 5)
                    Assert.AreEqual((UInt32)0, _header.GetLegacyPointCount(i));
            }

            // Set the first element to maximum valid legacy value,
            // the second element to 1,
            // the last element to maximum valid value and get all valid indexes.
            _header.SetPointCount(0, UInt32.MaxValue);
            _header.SetPointCount(1, 1);
            _header.SetPointCount(14, UInt64.MaxValue);
            Assert.AreEqual((UInt64)UInt32.MaxValue, _header.GetPointCount(0));
            Assert.AreEqual(UInt32.MaxValue, _header.GetLegacyPointCount(0));
            Assert.AreEqual((UInt64)1, _header.GetPointCount(1));
            Assert.AreEqual((UInt32)1, _header.GetLegacyPointCount(1));

            for (var i = 2; i < 14; i++)
            {
                Assert.AreEqual((UInt64)0, _header.GetPointCount(i));

                if (i < 5)
                    Assert.AreEqual((UInt32)0, _header.GetLegacyPointCount(i));
            }

            Assert.AreEqual(UInt64.MaxValue, _header.GetPointCount(14));

            // Set the point count to maximum valid legacy value and get the second element.
            _header.PointCount = UInt32.MaxValue;
            Assert.AreEqual((UInt64)1, _header.GetPointCount(1));
            Assert.AreEqual((UInt32)1, _header.GetLegacyPointCount(1));

            // Set the point count to minimum invalid legacy value and get the second element.
            _header.PointCount = (UInt64)UInt32.MaxValue + 1;
            Assert.AreEqual((UInt64)1, _header.GetPointCount(1));
            Assert.AreEqual((UInt32)0, _header.GetLegacyPointCount(1));

            // Set the point count to maximum valid legacy value,
            // the fourth element to minimum invalid legacy value
            // and get the second and fourth element.
            _header.PointCount = UInt32.MaxValue;
            _header.SetPointCount(3, (UInt64)UInt32.MaxValue + 1);
            Assert.AreEqual((UInt64)1, _header.GetPointCount(1));
            Assert.AreEqual((UInt32)0, _header.GetLegacyPointCount(1));
            Assert.AreEqual((UInt64)UInt32.MaxValue + 1, _header.GetPointCount(3));
            Assert.AreEqual((UInt32)0, _header.GetLegacyPointCount(3));

            // While point count is 15 and all point count per return value is 1
            // set point data format format to minimum invalid legacy value and get the third value.
            _header.PointCount = 15;

            for (var i = 0; i < 15; i++)
                _header.SetPointCount(i, 1);

            _header.PointDataFormat = 6;
            Assert.AreEqual((UInt32)0, _header.GetLegacyPointCount(2));

            // Out of range tests.
            Boolean result = false;

            try
            {
                _header.SetPointCount(-1, 39);
            }
            catch (IndexOutOfRangeException)
            {
                result = true;
            }

            Assert.True(result);
            result = false;

            try
            {
                _header.SetPointCount(15, 39);
            }
            catch (IndexOutOfRangeException)
            {
                result = true;
            }

            Assert.True(result);
            result = false;

            try
            {
                _header.GetPointCount(-1);
            }
            catch (IndexOutOfRangeException)
            {
                result = true;
            }

            Assert.True(result);
            result = false;

            try
            {
                _header.GetPointCount(15);
            }
            catch (IndexOutOfRangeException)
            {
                result = true;
            }

            Assert.True(result);
            result = false;

            try
            {
                _header.GetLegacyPointCount(-1);
            }
            catch (IndexOutOfRangeException)
            {
                result = true;
            }

            Assert.True(result);
            result = false;

            try
            {
                _header.GetLegacyPointCount(5);
            }
            catch (IndexOutOfRangeException)
            {
                result = true;
            }

            Assert.True(result);
        }
    }
}
