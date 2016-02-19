/// <copyright file="SpectralGeometryStreamFormats.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.IO
{
    [IdentifiedObjectCollection(typeof(GeometryStreamFormat))]
    public static class SpectralGeometryStreamFormats
    {        
        #region Query fields

        private static GeometryStreamFormat[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeometryStreamFormat" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeometryStreamFormat" /> instances within the collection.</value>
        public static IList<GeometryStreamFormat> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(SpectralGeometryStreamFormats).GetProperties().
                                                         Where(property => property.Name != "All").
                                                         Select(property => property.GetValue(null, null) as GeometryStreamFormat).
                                                         ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeometryStreamFormat" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="GeometryStreamFormat" /> instances that match the specified identifier.</returns>
        public static IList<GeometryStreamFormat> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }
        /// <summary>
        /// Returns all <see cref="GeometryStreamFormat" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="GeometryStreamFormat" /> instances that match the specified name.</returns>
        public static IList<GeometryStreamFormat> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => System.Text.RegularExpressions.Regex.IsMatch(alias, name, System.Text.RegularExpressions.RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static GeometryStreamFormat _enviRawImage;
        private static GeometryStreamFormat _esriRawImage;
        private static GeometryStreamFormat _genericRawImage;
        private static GeometryStreamFormat _geoTiff;
        private static GeometryStreamFormat _tiff;

        #endregion

        #region Public static fields

        /// <summary>
        /// ENVI raw image.
        /// </summary>
        public static GeometryStreamFormat EnviRawImage
        {
            get
            {
                return _enviRawImage ?? (_enviRawImage =
                    new GeometryStreamFormat("AEGIS::610212", "ENVI raw image",
                                             null, null, "1.0",
                                             new String[] { "raw", "bip", "bil", "bsq", "*" }, null,
                                             new Type[] { typeof(ISpectralGeometry) },
                                             new GeometryModel[] { GeometryModel.Spatial2D }));
            }
        }

        /// <summary>
        /// Esri raw image.
        /// </summary>
        public static GeometryStreamFormat EsriRawImage
        {
            get
            {
                return _esriRawImage ?? (_esriRawImage =
                    new GeometryStreamFormat("AEGIS::610211", "Esri raw image",
                                             null, null, "1.0",
                                             new String[] { "bip", "bil", "bsq" }, null,
                                             new Type[] { typeof(ISpectralGeometry) },
                                             new GeometryModel[] { GeometryModel.Spatial2D }));
            }
        }

        /// <summary>
        /// Generic raw image.
        /// </summary>
        public static GeometryStreamFormat GenericRawImage
        {
            get
            {
                return _genericRawImage ?? (_genericRawImage =
                    new GeometryStreamFormat("AEGIS::610210", "Generic raw image", 
                                             null, null, "1.0",
                                             new String[] { "raw", "bip", "bil", "bsq", "*" }, null,
                                             new Type[] { typeof(ISpectralGeometry) },
                                             new GeometryModel[] { GeometryModel.Spatial2D },
                                             SpectralGeometryStreamParameters.ByteOrder,
                                             SpectralGeometryStreamParameters.BytesGapPerBand,
                                             SpectralGeometryStreamParameters.BytesPerBandRow,
                                             SpectralGeometryStreamParameters.BytesPerRow,
                                             SpectralGeometryStreamParameters.BytesSkipped,
                                             SpectralGeometryStreamParameters.ColumnDimension,
                                             SpectralGeometryStreamParameters.Layout,
                                             SpectralGeometryStreamParameters.NumberOfColumns,
                                             SpectralGeometryStreamParameters.NumerOfRows,
                                             SpectralGeometryStreamParameters.RadiometricResolution,
                                             SpectralGeometryStreamParameters.RowDimension,
                                             SpectralGeometryStreamParameters.SpectralResolution,
                                             SpectralGeometryStreamParameters.TieCoordinate));
            }
        }

        /// <summary>
        /// GeoTIFF.
        /// </summary>
        public static GeometryStreamFormat GeoTiff
        {
            get
            {
                return _geoTiff ?? (_geoTiff =
                    new GeometryStreamFormat("AEGIS::610202", "Geotagged Image File Format", 
                                             "© 2000 Spot Image Corporation, Niles Ritter and Mike Ruth.", new String[] { "GeoTIFF" }, "1.0",
                                             new String[] { "tif", "tiff" }, new String[] { "image/tiff", "image/tiff-fx" },
                                             new Type[] { typeof(ISpectralGeometry) },
                                             new GeometryModel[] { GeometryModel.Spatial2D }));
            }
        }        

        /// <summary>
        /// TIFF.
        /// </summary>
        public static GeometryStreamFormat Tiff
        {
            get
            {
                return _tiff ?? (_tiff =
                    new GeometryStreamFormat("AEGIS::610201", "Tagged Image File Format",
                                             "© 1986-1988, 1992 Adobe Systems, Inc.", new String[] { "TIFF" }, "6.0",
                                             new String[] { "tif", "tiff" }, new String[] { "image/tiff", "image/tiff-fx" },
                                             new Type[] { typeof(ISpectralGeometry) },
                                             new GeometryModel[] { GeometryModel.None }));
            }
        }

        #endregion
    }
}
