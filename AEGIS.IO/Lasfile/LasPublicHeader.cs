/// <copyright file="LasPublicHeader.cs" company="Eötvös Loránd University (ELTE)">
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

    /// <summary>
    /// Represents a public header of a LAS file (VERSION 1.4).
    /// </summary>
    public class LasPublicHeader
    {
        /// <summary>
        /// The flag indicating whether the wave packets are stored internally.
        /// </summary>
        private Boolean isWavePacketStorageInternal;

        /// <summary>
        /// The flag indicating whether the wave packets are stored externally.
        /// </summary>
        private Boolean isWavePacketStorageExternal;

        /// <summary>
        /// The system identifier.
        /// </summary>
        private String systemIdentifier;

        /// <summary>
        /// The generating software.
        /// </summary>
        private String generatingSoftware;

        /// <summary>
        /// The number of point data records by return.
        /// </summary>
        private List<UInt64> dividedPointCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LasPublicHeader"/> class.
        /// </summary>
        public LasPublicHeader()
        {
            isWavePacketStorageInternal = false;
            isWavePacketStorageExternal = false;
            systemIdentifier = String.Empty;
            generatingSoftware = String.Empty;
            dividedPointCount = new List<UInt64>(15) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        /// <summary>
        /// Gets or sets the file source key.
        /// </summary>
        public UInt16 FileSourceId { get; set; }

        /// <summary>
        /// Gets or sets the GPS time type.
        /// </summary>
        public GPSTimeType GPSTimeType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the point records use GPS Week Time.
        /// </summary>
        public Boolean IsGPSWeekTime
        {
            get => GPSTimeType == GPSTimeType.GPSWeekTime;
            set => GPSTimeType = (value ? GPSTimeType.GPSWeekTime : GPSTimeType.GPSStandardTime);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the point records use Standard GPS Time.
        /// </summary>
        public Boolean IsGPSStandardTime
        {
            get => GPSTimeType == GPSTimeType.GPSStandardTime;
            set => GPSTimeType = (value ? GPSTimeType.GPSStandardTime : GPSTimeType.GPSWeekTime);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the wave packets are stored internally.
        /// </summary>
        public Boolean IsWavePacketStorageInternal
        {
            get => isWavePacketStorageInternal;
            set
            {
                if (value)
                {
                    isWavePacketStorageInternal = true;
                    isWavePacketStorageExternal = false;
                }
                else
                {
                    isWavePacketStorageInternal = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the wave packets are stored externally.
        /// </summary>
        public Boolean IsWavePacketStorageExternal
        {
            get => isWavePacketStorageExternal;
            set
            {
                if (value)
                {
                    isWavePacketStorageExternal = true;
                    isWavePacketStorageInternal = false;
                }
                else
                {
                    isWavePacketStorageExternal = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the return numbers were generated synthetically.
        /// </summary>
        public Boolean IsReturnNumberOriginSynthetical { get; set; }

        /// <summary>
        /// Gets or sets the CRS type.
        /// </summary>
        public CRSType CRSType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the CRS is GeoTIFF.
        /// </summary>
        public Boolean IsCRSGeoTIFF
        {
            get => CRSType == CRSType.GeoTIFF;
            set => CRSType = (value ? CRSType.GeoTIFF : CRSType.WKT);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the CRS is WKT.
        /// </summary>
        public Boolean IsCRSWKT
        {
            get => CRSType == CRSType.WKT;
            set => CRSType = (value ? CRSType.WKT : CRSType.GeoTIFF);
        }

        /// <summary>
        /// Gets or sets the project key data 1.
        /// </summary>
        public UInt32 ProjectId1 { get; set; }

        /// <summary>
        /// Gets or sets the project key data 2.
        /// </summary>
        public UInt16 ProjectId2 { get; set; }

        /// <summary>
        /// Gets or sets the project key data 3.
        /// </summary>
        public UInt16 ProjectId3 { get; set; }

        /// <summary>
        /// Gets or sets the project key data 4.
        /// </summary>
        public UInt64 ProjectId4 { get; set; }

        /// <summary>
        /// Gets or sets the major LAS file version.
        /// </summary>
        public Byte VersionMajor { get; set; }

        /// <summary>
        /// Gets or sets the minor LAS file version.
        /// </summary>
        public Byte VersionMinor { get; set; }

        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the given string is longer than 32.</exception>
        public String SystemIdentifier
        {
            get => systemIdentifier;
            set
            {
                if (value.Length > 32)
                    throw new ArgumentException("The given system identifier string is too long. Maximum length: 32.");

                systemIdentifier = value;
            }
        }

        /// <summary>
        /// Gets or sets the generating software.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the given string is longer than 32.</exception>
        public String GeneratingSoftware
        {
            get => generatingSoftware;
            set
            {
                if (value.Length > 32)
                    throw new ArgumentException("The given generating software string is too long. Maximum length: 32.");

                generatingSoftware = value;
            }
        }

        /// <summary>
        /// Gets or sets the file creation day of the year.
        /// </summary>
        public UInt16 FileCreationDay { get; set; }

        /// <summary>
        /// Gets or sets the file creation year.
        /// </summary>
        public UInt16 FileCreationYear { get; set; }

        /// <summary>
        /// Gets the file creation date or null if not given.
        /// </summary>
        public DateTime? FileCreationDate
        {
            get
            {
                if (FileCreationDay < 1 || FileCreationYear < 1)
                    return null;
                
                return new DateTime(FileCreationYear, 1, 1).AddDays(FileCreationDay - 1);
            }
        }

        /// <summary>
        /// Gets or sets the public header size in bytes.
        /// </summary>
        public UInt16 PublicHeaderSize { get; set; }

        /// <summary>
        /// Gets or sets the number of variable length records.
        /// </summary>
        public UInt32 VariableLengthRecordCount { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes to the first point data record.
        /// </summary>
        public UInt32 PointDataOffset { get; set; }

        /// <summary>
        /// Gets or sets the format of each point data record.
        /// </summary>
        public Byte PointDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the size of each point data record in bytes.
        /// </summary>
        public UInt16 PointDataLength { get; set; }

        /// <summary>
        /// Gets or sets the number of point records.
        /// </summary>
        public UInt64 PointCount { get; set; }

        /// <summary>
        /// Gets the number of point records or 0 if backward compatibility not supported.
        /// </summary>
        public UInt32 LegacyPointCount
        {
            get
            {
                if (PointCount > UInt32.MaxValue)
                    return 0;

                return Convert.ToUInt32(PointCount);
            }
        }

        /// <summary>
        /// Gets or sets the number of point records by return (15 returns).
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the given list is longer than 15.</exception>
        public IList<UInt64> DividedPointCount
        {
            get => dividedPointCount;
            set
            {
                if (value.Count > 15)
                    throw new ArgumentException("The given list is too long. Maximum length: 15.");

                for (Int32 i = 0; i < 15; i++)
                {
                    if (value.Count > i)
                        dividedPointCount[i] = value[i];
                    else
                        dividedPointCount[i] = 0;
                }
            }
        }

        /// <summary>
        /// Gets the number of point records by return (5 returns), each value is 0 if backward compatibility not supported.
        /// </summary>
        public IList<UInt32> LegacyDividedPointCount
        {
            get
            {
                List<UInt32> value = new List<UInt32>(5) { 0, 0, 0, 0, 0 };

                if (PointCount > UInt32.MaxValue)
                    return value;

                for (Int32 i = 0; i < 5; i++)
                {
                    if (dividedPointCount[i] > UInt32.MaxValue)
                        return value;
                }

                for (Int32 i = 0; i < 5; i++)
                    value[i] = Convert.ToUInt32(dividedPointCount[i]);

                return value;
            }
        }

        /// <summary>
        /// Gets or sets the X scale factor.
        /// </summary>
        public Double ScaleFactorX { get; set; }

        /// <summary>
        /// Gets or sets the Y scale factor.
        /// </summary>
        public Double ScaleFactorY { get; set; }

        /// <summary>
        /// Gets or sets the Z scale factor.
        /// </summary>
        public Double ScaleFactorZ { get; set; }

        /// <summary>
        /// Gets or sets the X offset.
        /// </summary>
        public Double OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the Y offset.
        /// </summary>
        public Double OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the Z offset.
        /// </summary>
        public Double OffsetZ { get; set; }

        /// <summary>
        /// Gets or sets the unscaled maximum value of X.
        /// </summary>
        public Double MaxX { get; set; }

        /// <summary>
        /// Gets or sets the unscaled minimum value of X.
        /// </summary>
        public Double MinX { get; set; }

        /// <summary>
        /// Gets or sets the unscaled maximum value of Y.
        /// </summary>
        public Double MaxY { get; set; }

        /// <summary>
        /// Gets or sets the unscaled minimum value of Y.
        /// </summary>
        public Double MinY { get; set; }

        /// <summary>
        /// Gets or sets the unscaled maximum value of Z.
        /// </summary>
        public Double MaxZ { get; set; }

        /// <summary>
        /// Gets or sets the unscaled minimum value of Z.
        /// </summary>
        public Double MinZ { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes to the first wave packet record.
        /// </summary>
        public UInt64 WavePacketRecordOffset { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes to the first extended variable length record.
        /// </summary>
        public UInt64 ExtendedVariableLengthRecordOffset { get; set; }

        /// <summary>
        /// Gets or sets the number of extended variable length records.
        /// </summary>
        public UInt32 ExtendedVariableLengthRecordCount { get; set; }

        /// <summary>
        /// Gets the number of point records in the given return.
        /// </summary>
        /// <param name="index">The null based index of the return.</param>
        /// <returns>The number of point records in the given return.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the given index is lower than 0 or greater than 14.</exception>
        public UInt64 GetPointCount(Int32 index)
        {
            if (index < 0 || index > 14)
                throw new IndexOutOfRangeException("The given index is out of range. Valid: 0 - 14.");

            return dividedPointCount[index];
        }

        /// <summary>
        /// Gets the number of point records in the given return or 0 if backward compatibility not supported.
        /// </summary>
        /// <param name="index">The null based index of the return.</param>
        /// <returns>The number of point records in the given return.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the given index is lower than 0 or greater than 4.</exception>
        public UInt32 GetLegacyPointCount(Int32 index)
        {
            if (index < 0 || index > 4)
                throw new IndexOutOfRangeException("The given index is out of range. Valid: 0 - 4.");

            if (PointCount > UInt32.MaxValue)
                return 0;

            for (Int32 i = 0; i < 5; i++)
            {
                if (dividedPointCount[i] > UInt32.MaxValue)
                    return 0;
            }

            return Convert.ToUInt32(dividedPointCount[index]);
        }

        /// <summary>
        /// Sets the number of point records in the given return.
        /// </summary>
        /// <param name="index">The null based index of the return.</param>
        /// <param name="value">The number of point records in the given return.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the given index is lower than 0 or greater than 14.</exception>
        public void SetPointCount(Int32 index, UInt64 value)
        {
            if (index < 0 || index > 14)
                throw new IndexOutOfRangeException("The given index is out of range. Valid: 0 - 14.");

            dividedPointCount[index] = value;
        }
    }
}
