/// <copyright file="SpectralGeometryStreamParameters.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.RawImage;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.IO
{
    /// <summary>
    /// Represents a collection of known <see cref="GeometryStreamParameter" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(GeometryStreamParameter))]
    public class SpectralGeometryStreamParameters
    {
        #region Query fields

        private static GeometryStreamParameter[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeometryStreamParameter" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeometryStreamParameter" /> instances within the collection.</value>
        public static IList<GeometryStreamParameter> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(SpectralGeometryStreamParameters).GetProperties().
                                                            Where(property => property.Name != "All").
                                                            Select(property => property.GetValue(null, null) as GeometryStreamParameter).
                                                            ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeometryStreamParameter" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="GeometryStreamParameter" /> instances that match the specified identifier.</returns>
        public static IList<GeometryStreamParameter> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeometryStreamParameter" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="GeometryStreamParameter" /> instances that match the specified name.</returns>
        public static IList<GeometryStreamParameter> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => System.Text.RegularExpressions.Regex.IsMatch(alias, name, System.Text.RegularExpressions.RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static GeometryStreamParameter _byteOrder;
        private static GeometryStreamParameter _bytesGapPerBand;
        private static GeometryStreamParameter _bytesPerBandRow;
        private static GeometryStreamParameter _bytesPerRow;
        private static GeometryStreamParameter _bytesSkipped;
        private static GeometryStreamParameter _columnDimension;
        private static GeometryStreamParameter _geometryFactory;
        private static GeometryStreamParameter _geometryFactoryType;
        private static GeometryStreamParameter _layout;
        private static GeometryStreamParameter _numberOfColumns;
        private static GeometryStreamParameter _numberOfRows;
        private static GeometryStreamParameter _radiometricResolution;
        private static GeometryStreamParameter _rowDimension;
        private static GeometryStreamParameter _spectralResolution;
        private static GeometryStreamParameter _tieCoordinate;

        #endregion

        #region Public static fields

        /// <summary>
        /// Byte order.
        /// </summary>
        public static GeometryStreamParameter ByteOrder
        {
            get
            {
                return _byteOrder ?? (_byteOrder =
                    GeometryStreamParameter.CreateOptionalParameter<ByteOrder>("AEGIS::620004", "Byte order", 
                                                                               "The byte order of the specified file. The default value is little endian."));
            }
        }

        /// <summary>
        /// Bytes gap per band.
        /// </summary>
        public static GeometryStreamParameter BytesGapPerBand
        {
            get
            {
                return _bytesGapPerBand ?? (_bytesGapPerBand =
                    GeometryStreamParameter.CreateOptionalParameter<Int32>("AEGIS::625103", "Bytes gap per band", 
                                                                           "The number of bytes between bands in a BSQ format image. The default value is 0.",
                                                                           Conditions.IsNotNegative()));
            }
        }

        /// <summary>
        /// Bytes per band per row.
        /// </summary>
        public static GeometryStreamParameter BytesPerBandRow
        {
            get
            {
                return _bytesPerBandRow ?? (_bytesPerBandRow =
                    GeometryStreamParameter.CreateOptionalParameter<Int32>("AEGIS::615202", "Bytes per band per row", "The number of bytes per band per row. Used with BIL files when there are extra bits at the end of each band within a row that must be skipped.",
                                                                           Conditions.IsNotNegative()));
            }
        }

        /// <summary>
        /// Bytes per row.
        /// </summary>
        public static GeometryStreamParameter BytesPerRow
        {
            get
            {
                return _bytesPerRow ?? (_bytesPerRow =
                    GeometryStreamParameter.CreateOptionalParameter<Int32>("AEGIS::625114", "Bytes per row", "The total number of bytes of data per row. Used when there are extra trailing bits at the end of each row.", 
                                                                           Conditions.IsNotNegative()));
            }
        }

        /// <summary>
        /// Bytes skipped.
        /// </summary>
        public static GeometryStreamParameter BytesSkipped
        {
            get
            {
                return _bytesSkipped ?? (_bytesSkipped =
                    GeometryStreamParameter.CreateOptionalParameter<Int32>("AEGIS::625106", "Bytes skipped", "The number of bytes of data in the image file to skip to reach the start of the image data. The default value is 0.", 
                                                                           Conditions.IsNotNegative()));
            }
        }

        /// <summary>
        /// Column dimension.
        /// </summary>
        public static GeometryStreamParameter ColumnDimension
        {
            get
            {
                return _columnDimension ?? (_columnDimension =
                    GeometryStreamParameter.CreateOptionalParameter<Double>("AEGIS::625021", "Column dimension",
                                                                            "The x-dimension of a pixel in map units. The default value is 1.", new String[] { "X dimension" }, 
                                                                            1, 
                                                                            Conditions.IsPositive()));
            }
        }

        /// <summary>
        /// Geometry factory.
        /// </summary>
        public static GeometryStreamParameter GeometryFactory
        {
            get
            {
                return _geometryFactory ?? (_geometryFactory =
                    GeometryStreamParameter.CreateOptionalParameter<IGeometryFactory>("AEGIS::620002", "Geometry factory",
                                                                                      "The geometry factory used to produce the instances read from the specified format. If geometry factory is specified, the reference system of the factory is used instead of the reference system provided by the source."));
            }
        }

        /// <summary>
        /// Geometry factory type.
        /// </summary>
        public static GeometryStreamParameter GeometryFactoryType
        {
            get
            {
                return _geometryFactoryType ?? (_geometryFactoryType =
                    GeometryStreamParameter.CreateOptionalParameter<Type>("AEGIS::620001", "Geometry factory type",
                                                                          "The type of the geometry factory used to produce the instances read from the specified format. If geometry factory type is specified, an instance of this type will be used witk the reference system provided by the source.",
                                                                          Conditions.Implements<IGeometryFactory>()));
            }
        }

        /// <summary>
        /// Image layout.
        /// </summary>
        public static GeometryStreamParameter Layout
        {
            get
            {
                return _layout ?? (_layout =
                    GeometryStreamParameter.CreateRequiredParameter<RawImageLayout>("AEGIS::625202", "Image layout", 
                                                                                    "The organization of the bands in the image file."));
            }
        }

        /// <summary>
        /// Number of columns.
        /// </summary>
        public static GeometryStreamParameter NumberOfColumns
        {
            get
            {
                return _numberOfColumns ?? (_numberOfColumns =
                    GeometryStreamParameter.CreateRequiredParameter<Int32>("AEGIS::625011", "Number of columns",
                                                                           "The number of columns in the image.", new String[] { "Image width" },
                                                                           Conditions.IsNotNegative()));
            }
        }

        /// <summary>
        /// Number of rows.
        /// </summary>
        public static GeometryStreamParameter NumerOfRows
        {
            get
            {
                return _numberOfRows ?? (_numberOfRows =
                    GeometryStreamParameter.CreateRequiredParameter<Int32>("AEGIS::625010", "Number of rows",
                                                                           "The number of rows in the image.", new String[] { "Image height" },
                                                                           Conditions.IsNotNegative()));
            }
        }

        /// <summary>
        /// Radiometric resolution.
        /// </summary>
        public static GeometryStreamParameter RadiometricResolution
        {
            get
            {
                return _radiometricResolution ?? (_radiometricResolution =
                    GeometryStreamParameter.CreateRequiredParameter<Int32>("AEGIS::625015", "Radiometric resolution",
                                                                           "The number of bits specifiing a sinlge spectral value in the image.", new String[] { "Bits per sample", "Number of bits" },
                                                                           8,
                                                                           Conditions.IsPositive()));
            }
        }

        /// <summary>
        /// Row dimension.
        /// </summary>
        public static GeometryStreamParameter RowDimension
        {
            get
            {
                return _rowDimension ?? (_rowDimension =
                    GeometryStreamParameter.CreateOptionalParameter<Double>("AEGIS::625020", "Row dimension",
                                                                            "The y-dimension of a pixel in map units. The default value is 1.", new String[] { "Y dimension" },
                                                                            1,
                                                                            Conditions.IsPositive()));
            }
        }

        /// <summary>
        /// Spectral resolution.
        /// </summary>
        public static GeometryStreamParameter SpectralResolution
        {
            get
            {
                return _spectralResolution ?? (_spectralResolution =
                    GeometryStreamParameter.CreateOptionalParameter<Int32>("AEGIS::625012", "Spectral resolution",
                                                                           "The number of spectral bands in the image.", new String[] { "Number of bands" },
                                                                           1, Conditions.IsPositive()));
            }
        }

        /// <summary>
        /// Tie coordinate.
        /// </summary>
        public static GeometryStreamParameter TieCoordinate
        {
            get
            {
                return _tieCoordinate ?? (_tieCoordinate =
                    GeometryStreamParameter.CreateOptionalParameter<Coordinate>("AEGIS::625025", "Tie coordinate.",
                                                                                "The sptial coordinate of the upper left pixel.", new String[] { "Upper left coordinate", "Tie point" }));
            }
        }

        #endregion
    }
}
