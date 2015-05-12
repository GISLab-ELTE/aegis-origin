/// <copyright file="GeoTiffMetafileReaderFactory.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
            return CreateReader(device, new Uri(path, UriKind.RelativeOrAbsolute));
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
                    return new DimapMetafileReader(stream);
                case "Landsat":
                    return new LandsatMetafileReader(stream);
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

            // try every possible device

            FileSystem fileSystem = FileSystem.GetFileSystemForPath(path);
            String directory = fileSystem.GetDirectory(path);
            String[] filePaths;

            switch (option)
            {
                case GeoTiffMetafilePathOption.IsMetafilePath:
                    foreach (MetafileFormat format in _readers.Keys)
                    {
                        try
                        {
                            return Activator.CreateInstance(_readers[format], path) as GeoTiffMetafileReader;
                        }
                        catch { }
                    }
                    break;
                case GeoTiffMetafilePathOption.IsGeoTiffFilePath:
                    
                    // search the extension
                    foreach (MetafileFormat format in _readers.Keys)
                    {
                        filePaths = new String[]
                        {
                            directory + fileSystem.DirectorySeparator + fileSystem.GetFileNameWithoutExtension(path) + "." + format.DefaultExtension,
                            directory + fileSystem.DirectorySeparator + fileSystem.GetFileNameWithoutExtension(path) + "." + format.DefaultExtension.ToLower(),
                            directory + fileSystem.DirectorySeparator + fileSystem.GetFileNameWithoutExtension(path) + "." + format.DefaultExtension.ToUpper(),
                        };

                        foreach (String filePath in filePaths)
                        {
                            if (!fileSystem.Exists(filePath))
                                continue;

                            try
                            {
                                return Activator.CreateInstance(_readers[format], filePath) as GeoTiffMetafileReader;
                            }
                            catch 
                            {
                            }
                        }
                    }

                    // search default name
                    foreach (MetafileFormat format in _readers.Keys)
                    {
                        if (!format.HasDefaultFileName)
                            continue;

                        if (!fileSystem.Exists(directory + fileSystem.DirectorySeparator + format.DefaultFileName))
                            continue;

                        try
                        {
                            return Activator.CreateInstance(_readers[format], directory + fileSystem.DirectorySeparator + format.DefaultFileName) as GeoTiffMetafileReader;
                        }
                        catch
                        {
                        }
                    }

                    // search default pattern
                    foreach (MetafileFormat format in _readers.Keys)
                    {
                        if (!format.HasDefaultNamingPattern)
                            continue;

                        filePaths = fileSystem.GetFiles(directory, format.DefaultNamingPattern, false);

                        foreach (String filePath in filePaths)
                        {
                            try
                            {
                                return Activator.CreateInstance(_readers[format], filePath) as GeoTiffMetafileReader;
                            }
                            catch
                            {
                            }
                        }
                    }


                    break;
                case GeoTiffMetafilePathOption.IsSearchPattern:
                    String pattern = path.Replace(directory + fileSystem.DirectorySeparator, String.Empty);
                    filePaths = fileSystem.GetFiles(directory, pattern, false);

                    foreach (MetafileFormat format in _readers.Keys)
                    {
                        foreach (String filePath in filePaths)
                        {
                            if (!fileSystem.Exists(filePath))
                                continue;

                            try
                            {
                                return Activator.CreateInstance(_readers[format], filePath) as GeoTiffMetafileReader;
                            }
                            catch
                            {
                            }
                        }
                    }
                    break;
            }

            throw new NotSupportedException("The metafile format is not supported.");
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
            return CreateReader(path.IsAbsoluteUri ? path.AbsolutePath : path.OriginalString, option);
        }

        #endregion
    }
}
