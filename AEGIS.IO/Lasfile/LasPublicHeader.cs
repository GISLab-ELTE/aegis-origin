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
        #region Private fields

        /// <summary>
        /// The flag indicating whether the wave packets are stored internally.
        /// </summary>
        private Boolean _isWavePacketStorageInternal;

        /// <summary>
        /// The flag indicating whether the wave packets are stored externally.
        /// </summary>
        private Boolean _isWavePacketStorageExternal;

        /// <summary>
        /// The system identifier.
        /// </summary>
        private String _systemIdentifier = String.Empty;

        /// <summary>
        /// The generating software.
        /// </summary>
        private String _generatingSoftware = String.Empty;

        /// <summary>
        /// The number of point data records by return.
        /// </summary>
        private List<UInt64> _pointCounts =
                new List<UInt64>(15) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        #endregion

        #region Public properties

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
            get => GPSTimeType == GPSTimeType.Week;
            set => GPSTimeType = (value ? GPSTimeType.Week : GPSTimeType.Standard);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the point records use Standard GPS Time.
        /// </summary>
        public Boolean IsGPSStandardTime
        {
            get => GPSTimeType == GPSTimeType.Standard;
            set => GPSTimeType = value ? GPSTimeType.Standard : GPSTimeType.Week;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the waveform data packets are stored internally.
        /// </summary>
        public Boolean IsWavePacketStorageInternal
        {
            get => _isWavePacketStorageInternal;
            set
            {
                if (value)
                {
                    _isWavePacketStorageInternal = true;
                    _isWavePacketStorageExternal = false;
                }
                else
                {
                    _isWavePacketStorageInternal = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the waveform data packets are stored externally.
        /// </summary>
        public Boolean IsWavePacketStorageExternal
        {
            get => _isWavePacketStorageExternal;
            set
            {
                if (value)
                {
                    _isWavePacketStorageExternal = true;
                    _isWavePacketStorageInternal = false;
                }
                else
                {
                    _isWavePacketStorageExternal = false;
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
            get => _systemIdentifier;
            set
            {
                if (value is null)
                {
                    _systemIdentifier = String.Empty;
                }
                else if (value.Length > 32)
                {
                    throw new ArgumentException
                        ("The given system identifier string is too long. Maximum length: 32.");
                }
                else
                {
                    _systemIdentifier = value;
                }

            }
        }

        /// <summary>
        /// Gets or sets the generating software.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the given string is longer than 32.</exception>
        public String GeneratingSoftware
        {
            get => _generatingSoftware;
            set
            {
                if (value is null)
                {
                    _generatingSoftware = String.Empty;
                }
                else if (value.Length > 32)
                {
                    throw new ArgumentException
                        ("The given generating software string is too long. Maximum length: 32.");
                }
                else
                {
                    _generatingSoftware = value;
                }
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
        /// <remarks>
        /// If the file creation date is not given the value is null.
        /// </remarks>
        public DateTime? FileCreationDate
        {
            get
            {
                if (FileCreationDay < 1 || FileCreationYear < 1)
                    return null;

                DateTime date = new DateTime(FileCreationYear, 1, 1).AddDays(FileCreationDay - 1);

                if (date.Year != FileCreationYear)
                    return null;

                return date;
            }
            set
            {
                if (value is null)
                {
                    FileCreationDay = 0;
                    FileCreationYear = 0;
                }
                else
                {
                    FileCreationDay = Convert.ToUInt16(value.Value.DayOfYear);
                    FileCreationYear = Convert.ToUInt16(value.Value.Year);
                }
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
        /// Gets the number of point records in backward compatibility format.
        /// If backward compatibility is not supported the value is 0.
        /// </summary>
        public UInt32 LegacyPointCount
        {
            get
            {
                if (PointDataFormat > 5)
                    return 0;

                if (PointCount > UInt32.MaxValue)
                    return 0;

                return Convert.ToUInt32(PointCount);
            }
        }

        /// <summary>
        /// Gets or sets the number of point records by return.
        /// It uses a list that stores the individual numbers.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the list is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the length of the list is greater than 15.
        /// </exception>
        public IList<UInt64> PointCountPerReturn
        {
            get => _pointCounts;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(PointCountPerReturn), "The given list is null.");

                if (value.Count > 15)
                    throw new ArgumentException("The given list is too long. Maximum length: 15.");

                for (var i = 0; i < 15; i++)
                {
                    if (value.Count > i)
                        _pointCounts[i] = value[i];
                    else
                        _pointCounts[i] = 0;
                }
            }
        }

        /// <summary>
        /// Gets the number of point records by return in backward compatibility format.
        /// It uses a list that stores the individual numbers.
        /// If backward compatibility is not supported all value is 0.
        /// </summary>
        public IList<UInt32> LegacyPointCountPerReturn
        {
            get
            {
                List<UInt32> value = new List<UInt32>(5);

                if (PointDataFormat > 5)
                    return new List<UInt32>(5) {0, 0, 0, 0, 0};

                if (PointCount > UInt32.MaxValue)
                    return new List<UInt32>(5) {0, 0, 0, 0, 0};

                for (var i = 0; i < 5; i++)
                {
                    if (_pointCounts[i] > UInt32.MaxValue)
                        return new List<UInt32>(5) {0, 0, 0, 0, 0};

                    value.Add(Convert.ToUInt32(_pointCounts[i]));
                }

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
        /// Gets or sets the offset to the first byte of the waveform data packet record.
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

        #endregion

        #region Public methods

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

            return _pointCounts[index];
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

            if (PointDataFormat > 5)
                return 0;

            if (PointCount > UInt32.MaxValue)
                return 0;

            for (var i = 0; i < 5; i++)
            {
                if (_pointCounts[i] > UInt32.MaxValue)
                    return 0;
            }

            return Convert.ToUInt32(_pointCounts[index]);
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

            _pointCounts[index] = value;
        }

        #endregion
    }
}
