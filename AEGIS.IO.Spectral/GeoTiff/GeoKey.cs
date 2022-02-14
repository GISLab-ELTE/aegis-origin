/// <copyright file="GeoKey.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Defines values of GeoTIFF geo-keys.
    /// </summary>
    public static class GeoKey
    {
        /// <summary>
        /// The model type key.
        /// </summary>
        public const Int16 ModelType = 1024;

        /// <summary>
        /// The raster type key.
        /// </summary>
        public const Int16 RasterType = 1025;

        /// <summary>
        /// The citation key.
        /// </summary>
        public const Int16 Citation = 1026;

        /// <summary>
        /// The geodetic coordinate reference system type key.
        /// </summary>
        public const Int16 GeodeticCoordinateReferenceSystemType = 2048;

        /// <summary>
        /// The geodetic coordinate reference system citation key.
        /// </summary>
        public const Int16 GeodeticCoordinateReferenceSystemCitation = 2049;

        /// <summary>
        /// The geodetic datum key.
        /// </summary>
        public const Int16 GeodeticDatum = 2050;

        /// <summary>
        /// The geodetic prime meridian key.
        /// </summary>
        public const Int16 GeodeticPrimeMeridian = 2051;

        /// <summary>
        /// The geodetic linear units key.
        /// </summary>
        public const Int16 GeodeticLinearUnits = 2052;

        /// <summary>
        /// The geodetic linear unit size key.
        /// </summary>
        public const Int16 GeodeticLinearUnitSize = 2053;

        /// <summary>
        /// The geodetic angular units key.
        /// </summary>
        public const Int16 GeodeticAngularUnits = 2054;

        /// <summary>
        /// The geodetic angular unit size key.
        /// </summary>
        public const Int16 GeodeticAngularUnitSize = 2055;

        /// <summary>
        /// The geodetic ellipsoid key.
        /// </summary>
        public const Int16 GeodeticEllipsoid = 2056;

        /// <summary>
        /// The geodetic semi major axis key.
        /// </summary>
        public const Int16 GeodeticSemiMajorAxis = 2057;

        /// <summary>
        /// The geodetic semi minor axis key.
        /// </summary>
        public const Int16 GeodeticSemiMinorAxis = 2058;

        /// <summary>
        /// The geodetic inverse fLatitudetening key.
        /// </summary>
        public const Int16 GeodeticInverseFlattening = 2059;

        /// <summary>
        /// The geodetic azimuth units key.
        /// </summary>
        public const Int16 GeodeticAzimuthUnits = 2060;

        /// <summary>
        /// The geodetic prime meridian longitude key.
        /// </summary>
        public const Int16 GeodeticPrimeMeridianLongitude = 2061;

        /// <summary>
        /// The projected coordinate reference system type key.
        /// </summary>
        public const Int16 ProjectedCoordinateReferenceSystemType = 3072;

        /// <summary>
        /// The projected coordinate reference system citation key.
        /// </summary>
        public const Int16 ProjectedCoordinateReferenceSystemCitation = 3073;

        /// <summary>
        /// The projection key.
        /// </summary>
        public const Int16 Projection = 3074;

        /// <summary>
        /// The projection coordinate transformation key.
        /// </summary>
        public const Int16 ProjectionCoordinateTransformation = 3075;

        /// <summary>
        /// The projection linear unit.
        /// </summary>
        public const Int16 ProjectionLinearUnit = 3076;

        /// <summary>
        /// The projection linear unit size.
        /// </summary>
        public const Int16 ProjectionLinearUnitSize = 3077;

        /// <summary>
        /// The projection standard parallel 1.
        /// </summary>
        public const Int16 ProjectionStdParallel1 = 3078;

        /// <summary>
        /// The projection standard parallel 2.
        /// </summary>
        public const Int16 ProjectionStdParallel2 = 3079;

        /// <summary>
        /// The projection natural origin longitude.
        /// </summary>
        public const Int16 ProjectionNaturalOriginLongitude = 3080;

        /// <summary>
        /// The projection natural origin latitude.
        /// </summary>
        public const Int16 ProjectionNaturalOriginLatitude = 3081;

        /// <summary>
        /// The projection false easting.
        /// </summary>
        public const Int16 ProjectionFalseEasting = 3082;

        /// <summary>
        /// The projection false northing.
        /// </summary>
        public const Int16 ProjectionFalseNorthing = 3083;

        /// <summary>
        /// The projection false origin longitude.
        /// </summary>
        public const Int16 ProjectionFalseOriginLongitude = 3084;

        /// <summary>
        /// The projection false origin latitude.
        /// </summary>
        public const Int16 ProjectionFalseOriginLatitude = 3085;

        /// <summary>
        /// The projection false origin easting.
        /// </summary>
        public const Int16 ProjectionFalseOriginEasting = 3086;

        /// <summary>
        /// The projection false origin northing.
        /// </summary>
        public const Int16 ProjectionFalseOriginNorthing = 3087;

        /// <summary>
        /// The projection center longitude.
        /// </summary>
        public const Int16 ProjectionCenterLongitude = 3088;

        /// <summary>
        /// The projection center latitude.
        /// </summary>
        public const Int16 ProjectionCenterLatitude = 3089;

        /// <summary>
        /// The projection center easting.
        /// </summary>
        public const Int16 ProjectionCenterEasting = 3090;

        /// <summary>
        /// The projection center northing.
        /// </summary>
        public const Int16 ProjectionCenterNorthing = 3091;

        /// <summary>
        /// The projection scale at natural origin.
        /// </summary>
        public const Int16 ProjectionScaleAtNaturalOrigin = 3092;

        /// <summary>
        /// The projection scale at center.
        /// </summary>
        public const Int16 ProjectionScaleAtCenter = 3093;

        /// <summary>
        /// The projection azimuth angle.
        /// </summary>
        public const Int16 ProjectionAzimuthAngle = 3094;

        /// <summary>
        /// The projection straight vertical pole longitude.
        /// </summary>
        public const Int16 ProjectionStraightVerticalPoleLongitude = 3095;
    }
}
