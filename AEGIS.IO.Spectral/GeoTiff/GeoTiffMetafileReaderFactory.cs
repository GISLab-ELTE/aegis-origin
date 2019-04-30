/// <copyright file="GeoTiffMetafileReaderFactory.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.GeoTiff.Metafile;
using ELTE.AEGIS.IO.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Represents a factory type for producing <see cref="GeoTiffMetafileReader"/> instances.
    /// </summary>
    public class GeoTiffMetafileReaderFactory
    {
        #region Private fields

        private static Dictionary<MetafileFormat, Type> _readers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="GeoTiffMetafileReaderFactory" /> class.
        /// </summary>
        static GeoTiffMetafileReaderFactory()
        {
            _readers = new Dictionary<MetafileFormat, Type>();
            _readers.Add(GeotiffMetafileFormats.Dimap, typeof(DimapMetafileReader));
            _readers.Add(GeotiffMetafileFormats.Landsat, typeof(LandsatMetafileReader));
        }

        #endregion

        #region Factory methods for specific device

        /// <summary>
        /// Creates a metafile reader for the specified metafile path and device.
        /// </summary>
        /// <param name="device">The imaging device.</param>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The valid metafile reader for the path if any; otherwise, <c>null</c>.</returns>
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
            return CreateReader(device, new Uri(path, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path and device.
        /// </summary>
        /// <param name="device">The imaging device.</param>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The valid metafile reader for the path if any; otherwise, <c>null</c>.</returns>
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
            if (device == null)
                throw new ArgumentNullException("device", "The device is null.");
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");

            switch (device.Mission)
            {
                case "SPOT":
                    return new DimapMetafileReader(path);
                case "Landsat":
                    return new LandsatMetafileReader(path);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile stream and device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream of the metafile.</param>
        /// <returns>The valid metafile reader for the path if any; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.NotSupportedException">The specified device is not supported.</exception>
        public static GeoTiffMetafileReader CreateReader(ImagingDevice device, Stream stream)
        {
            if (device == null)
                throw new ArgumentNullException("device", "The device is null.");

            switch (device.Mission)
            {
                case "SPOT":
                    return new DimapMetafileReader(stream);
                case "Landsat":
                    return new LandsatMetafileReader(stream);
                default:
                    return null;
            }
        }

        #endregion

        #region Factory methods for any device

        /// <summary>
        /// Creates a metafile reader for the specified metafile path.
        /// </summary>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The valid metafile reader for the path if any; otherwise, <c>null</c>.</returns>
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
        /// <returns>The valid metafile reader for the path if any; otherwise, <c>null</c>.</returns>
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

            switch (option)
            {
                case GeoTiffMetafilePathOption.IsMetafilePath:
                    return GetReader(path);
                case GeoTiffMetafilePathOption.IsDirectoryPath:
                    return GetReaderInDirectory(path);
                case GeoTiffMetafilePathOption.IsGeoTiffFilePath:
                    return GetReaderFromGeoTiffFileName(path);
                case GeoTiffMetafilePathOption.IsSearchPattern:
                    return GetReaderFromSearchPattern(path);
            }

            return null;
        }

        /// <summary>
        /// Creates a metafile reader for the specified metafile path.
        /// </summary>
        /// <param name="path">The path of the metafile.</param>
        /// <returns>The valid metafile reader for the path if any; otherwise, <c>null</c>.</returns>
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
            return CreateReader(path.IsAbsoluteUri ? path.AbsolutePath : path.OriginalString, option);
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the metafile reader located at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="format">The format of the reader.</param>
        /// <returns>The metafile reader located at the specified path if available; otherwise, <c>null</c>.</returns>
        private static GeoTiffMetafileReader GetReader(String path, MetafileFormat format)
        {
            try
            {
                return Activator.CreateInstance(_readers[format], path) as GeoTiffMetafileReader;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Returns the metafile reader located at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The metafile reader located at the specified path if available; otherwise, <c>null</c>.</returns>
        private static GeoTiffMetafileReader GetReader(String path)
        {
            GeoTiffMetafileReader reader = null;
            foreach (MetafileFormat format in _readers.Keys)
            {
                if ((reader = GetReader(path, format)) != null)
                    return reader;
            }

            return reader;
        }

        /// <summary>
        /// Returns the metafile reader located at the specified directory.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        /// <returns>The metafile reader located at the specified path if available; otherwise, <c>null</c>.</returns>
        private static GeoTiffMetafileReader GetReaderInDirectory(String path)
        {
            GeoTiffMetafileReader reader = null;
            FileSystem fileSystem = FileSystem.GetFileSystemForPath(path);

            // try the default file name
            foreach (MetafileFormat format in _readers.Keys)
            {
                if (format.HasDefaultFileName)
                {
                    String filePath = fileSystem.Combine(path, format.DefaultFileName);
                    
                    if (fileSystem.Exists(filePath) && (reader = GetReader(filePath, format)) != null)
                        return reader;
                }
            }

            // try the default naming pattern and extension
            foreach (MetafileFormat format in _readers.Keys)
            {
                if (format.HasDefaultNamingPattern)
                {
                    foreach (String filePath in fileSystem.GetFiles(path, format.DefaultNamingPattern, false))
                    {
                        if ((reader = GetReader(filePath, format)) != null)
                            return reader;
                    }
                }
            }

            // try the default extension
            foreach (MetafileFormat format in _readers.Keys)
            {
                if (format.HasDefaultNamingPattern)
                {
                    foreach (String filePath in fileSystem.GetFiles(path, format.DefaultExtension.ToLower(), false).
                                                Concat(fileSystem.GetFiles(path, format.DefaultExtension.ToUpper(), false)))
                    {
                        if ((reader = GetReader(filePath, format)) != null)
                            return reader;
                    }
                }
            }
            
            return reader;
        }

        /// <summary>
        /// Returns the metafile reader located based on the GeoTIFF path.
        /// </summary>
        /// <param name="path">The path of the GeoTIFF file.</param>
        /// <returns>The metafile reader located at the specified path if available; otherwise, <c>null</c>.</returns>
        private static GeoTiffMetafileReader GetReaderFromGeoTiffFileName(String path)
        {
            GeoTiffMetafileReader reader = null;
            FileSystem fileSystem = FileSystem.GetFileSystemForPath(path);
            String directory = fileSystem.GetDirectory(path);

            // search the extension
            foreach (MetafileFormat format in _readers.Keys)
            {
                String[] filePaths = new String[]
                {
                    fileSystem.Combine(directory, fileSystem.GetFileNameWithoutExtension(path) + "." + format.DefaultExtension.ToLower()),
                    fileSystem.Combine(directory, fileSystem.GetFileNameWithoutExtension(path) + "." + format.DefaultExtension.ToUpper())
                };

                foreach (String filePath in filePaths)
                {
                    if (fileSystem.Exists(filePath) && (reader = GetReader(filePath, format)) != null)
                        return reader;
                }
            }

            // search the directory
            return GetReaderInDirectory(directory);
        }

        /// <summary>
        /// Returns the metafile reader located based on the naming pattern.
        /// </summary>
        /// <param name="path">The path including the naming pattern.</param>
        /// <returns>The metafile reader located at the specified path if available; otherwise, <c>null</c>.</returns>
        private static GeoTiffMetafileReader GetReaderFromSearchPattern(String path)
        {
            GeoTiffMetafileReader reader = null;
            FileSystem fileSystem = FileSystem.GetFileSystemForPath(path);
            String fileNamePattern = fileSystem.GetFileName(path);
            String directory = fileSystem.GetDirectory(path);
            String[] filePaths = fileSystem.GetFiles(directory, fileNamePattern, false);

            // check for matching paths
            foreach (MetafileFormat format in _readers.Keys)
            {
                foreach (String filePath in filePaths)
                {
                    if (format.IsMatchingName(filePath) && (reader = GetReader(filePath, format)) != null)
                        return reader;
                }
            }

            // check for all paths
            foreach (MetafileFormat format in _readers.Keys)
            {
                foreach (String filePath in filePaths)
                {
                    if ((reader = GetReader(filePath, format)) != null)
                        return reader;
                }
            }

            return reader;
        }

        #endregion
    }
}
