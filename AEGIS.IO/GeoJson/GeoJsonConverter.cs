/// <copyright file="GeoJsonConverter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Norbert Vass</author>
/// <author>Máté Cserép</author>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace ELTE.AEGIS.IO.GeoJSON
{
    public static class GeoJsonConverter
    {
        #region Public conversion methods from Geometry to GeoJSON

        /// <summary>
        /// Converts Geometry to GeoJSON representation. Computes the FeatureCollection's 
        /// Coordinate Reference System (if needed) for optimal file size, then writes all geometries to file.
        /// </summary>
        /// <param name="geometry">The geometry to convert.</param>
        /// <param name="path">The path for the output file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The parameter is null.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The path is invalid.
        /// </exception>
        public static void ToGeoJson(this IGeometry geometry, string path)
        {
            if (geometry == null)
                throw new ArgumentNullException(nameof(geometry), "The geometries parameter is null");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "Path is null or contains only whitespaces");


            GeoJsonWriter writer = null;
            try
            {
                writer = new GeoJsonWriter(path);
                writer.Write(geometry);
            }
            catch (IOException ioex)
            {
                throw new IOException("Wrong file path.", ioex);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Geometry content is invalid", ex);
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        /// Converts Geometry to GeoJSON representation. Computes the FeatureCollection's 
        /// Coordinate Reference System (if needed) for optimal file size, then writes all geometries to file.
        /// </summary>
        /// <param name="geometries">The geometries to convert.</param>
        /// <param name="path">The path for the output file.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The parameter is null.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The path is invalid.
        /// </exception>
        public static void ToGeoJson(this IList<IGeometry> geometries, string path)
        {
            if (geometries == null)
                throw new ArgumentNullException(nameof(geometries), "The geometries parameter is null");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path), "Path is null or contains only whitespaces");
            if (geometries.Count == 0)
                return;

            GeoJsonWriter writer = null;
            try
            {
                writer = new GeoJsonWriter(path);
                writer.Write(geometries);
            }
            catch (IOException ioex)
            {
                throw new IOException("Wrong file path.", ioex);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Geometry content is invalid", ex);
            }
            finally
            {
                writer?.Close();
            }
        }

        #endregion
    }
}
