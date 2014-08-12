/// <copyright file="GeoTiffMetafileReaderFactory.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.IO.GeoTiff.Metafile;
using System;
using System.IO;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Represents a factory type for producing <see cref="GeoTiffMetafileReader"/> instances.
    /// </summary>
    public class GeoTiffMetafileReaderFactory
    {
        // TODO: refactor to service locator

        #region Factory methods for specific device

        /// <summary>
        /// Creates a metafile reader for the specified metafile path and device.
        /// </summary>
        /// <param name="device">The imaging device.</param>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The device is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified device is not supported.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        public static GeoTiffMetafileReader CreateReader(ImagingDevice device, String path)
        {
            return CreateReader(device, path, GeoTiffMetafilePathOption.IsMetafilePath);
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path and device.
        /// </summary>
        /// <param name="device">The imaging device.</param>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The device is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified device is not supported.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        public static GeoTiffMetafileReader CreateReader(ImagingDevice device, Uri path)
        {
            return CreateReader(device, path, GeoTiffMetafilePathOption.IsMetafilePath);
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path and device.
        /// </summary>
        /// <param name="device">The imaging device.</param>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The device is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified device is not supported.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        public static GeoTiffMetafileReader CreateReader(ImagingDevice device, String path, GeoTiffMetafilePathOption option)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");            
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            return CreateReader(device, new Uri(path, UriKind.RelativeOrAbsolute), option);
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path and device.
        /// </summary>
        /// <param name="device">The imaging device.</param>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The device is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified device is not supported.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        public static GeoTiffMetafileReader CreateReader(ImagingDevice device, Uri path, GeoTiffMetafilePathOption option)
        {
            if (device == null)
                throw new ArgumentNullException("device", "The device is null.");

            switch (device.Mission)
            {
                case "SPOT":
                    return new SpotMetafileReader(path, option);
                default:
                    throw new NotSupportedException("The specified device is not supported.");
            }
        }


        /// <summary>
        /// Creates a metafile reader for the specified metafile stream and device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream of the metafile.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.NotSupportedException">The specified device is not supported.</exception>
        public static GeoTiffMetafileReader CreateReader(ImagingDevice device, Stream stream)
        {
            if (device == null)
                throw new ArgumentNullException("device", "The device is null.");

            switch (device.Mission)
            {
                case "SPOT":
                    return new SpotMetafileReader(stream);
                default:
                    throw new NotSupportedException("The specified device is not supported.");
            }
        }

        #endregion

        #region Factory methods for any device

        /// <summary>
        /// Creates a metafile reader for the specified metafile path.
        /// </summary>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The metafile format is not supported.</exception>
        public static GeoTiffMetafileReader CreateReader(String path)
        {
            return CreateReader(path, GeoTiffMetafilePathOption.IsMetafilePath);
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The metafile format is not supported.</exception>
        public static GeoTiffMetafileReader CreateReader(String path, GeoTiffMetafilePathOption option)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");

            return CreateReader(new Uri(path, UriKind.RelativeOrAbsolute), option);
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path.
        /// </summary>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The metafile format is not supported.</exception>
        public static GeoTiffMetafileReader CreateReader(Uri path)
        {
            return CreateReader(path, GeoTiffMetafilePathOption.IsMetafilePath);
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
        /// <returns>The produced metafile reader.</returns>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The path is a zero-length string, contains only white space, or contains one or more invalid characters.
        /// or
        /// The path, file name, or both exceed the system-defined maximum length.
        /// </exception>
        /// <exception cref="System.UnauthorizedAccessException">
        /// The file on path is hidden.
        /// or
        /// The file on path is read-only.
        /// or
        /// The caller does not have the required permission for the path.</exception>
        /// <exception cref="System.NotSupportedException">The metafile format is not supported.</exception>
        public static GeoTiffMetafileReader CreateReader(Uri path, GeoTiffMetafilePathOption option)
        {
            // try every possible device

            try
            {
                return new SpotMetafileReader(path, option);
            }
            catch { }

            throw new NotSupportedException("The metafile format is not supported.");
        }

        #endregion
    }
}
