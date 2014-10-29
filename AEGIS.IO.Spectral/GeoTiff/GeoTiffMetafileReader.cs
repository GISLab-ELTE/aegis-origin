/// <copyright file="GeoTiffMetafileReader.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Represents a type for reading GeoTIFF metafiles.
    /// </summary>
    public abstract class GeoTiffMetafileReader : IDisposable
    {
        #region Protected fields

        protected readonly Stream _stream;
        private Boolean _disposed;

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets the default extension of the metafile.
        /// </summary>
        protected abstract String DefaultExtension { get; }

        /// <summary>
        /// Gets the default file name of the metafile.
        /// </summary>
        protected abstract String DefaultFileName { get; }

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
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
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        protected GeoTiffMetafileReader(String path, GeoTiffMetafilePathOption option)
        {
            FileSystem fileSystem = FileSystem.GetFileSystemForPath(path);

            switch (option)
            {
                case GeoTiffMetafilePathOption.IsMetafilePath:
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }

                    throw new FileNotFoundException("The metafile does not exist.");

                case GeoTiffMetafilePathOption.IsGeoTiffFilePath:
                    // change the extension
                    path = fileSystem.GetDirectory(path) + fileSystem.DirectorySeparator + fileSystem.GetFileNameWithoutExtension(path) + "." + DefaultExtension.ToLower();
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }

                    path = fileSystem.GetDirectory(path) + fileSystem.DirectorySeparator + fileSystem.GetFileNameWithoutExtension(path) + "." + DefaultExtension.ToUpper();
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }

                    // change the file name
                    path = fileSystem.GetDirectory(path) + fileSystem.DirectorySeparator + DefaultFileName.ToLower();
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }
                    path = fileSystem.GetDirectory(path) + fileSystem.DirectorySeparator + DefaultFileName.ToUpper();
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }
                    
                    throw new FileNotFoundException("The metafile does not exist.");

                case GeoTiffMetafilePathOption.IsDirectoryPath:
                    path = fileSystem.GetDirectory(path) + fileSystem.DirectorySeparator + DefaultFileName.ToLower();
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }
                    path = fileSystem.GetDirectory(path) + fileSystem.DirectorySeparator + DefaultFileName.ToUpper();
                    if (fileSystem.Exists(path))
                    {
                        _stream = fileSystem.OpenFile(path, FileMode.Open);
                        break;
                    }
                    
                    throw new FileNotFoundException("The metafile does not exist.");
            }

            _disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="option">The path option.</param>
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
        /// The caller does not have the required permission for the path.
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">The metafile does not exist.</exception>
        protected GeoTiffMetafileReader(Uri path, GeoTiffMetafilePathOption option)
            : this(path.AbsolutePath, option)
        {         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffMetafileReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        protected GeoTiffMetafileReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "The stream is null.");

            _stream = stream;
            _disposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GeoTiffMetafileReader"/> class.
        /// </summary>
        ~GeoTiffMetafileReader()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Reads the device information stored in the metafile.
        /// </summary>
        /// <returns>The device information.</returns>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during metafile reading.</exception>
        public ImagingDevice ReadDeviceData() 
        { 
            if (_disposed) 
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadDeviceFromStream();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads the imaging information stored in the metafile.
        /// </summary>
        /// <value>The imaging data.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during metafile reading.</exception>
        public RasterImaging ReadImaging()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadImagingFromStream();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads the raster mapping stored in the metafile.
        /// </summary>
        /// <value>The raster mapping.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during metafile reading.</exception>
        public RasterMapper ReadMapping()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadMappingFromStream();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads the reference system stored in the metafile.
        /// </summary>
        /// <value>The reference system.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during metafile reading.</exception>
        public IReferenceSystem ReadReferenceSystem()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadReferenceSystemFromStream();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads additional metadata stored in the metafile.
        /// </summary>
        /// <value>The dictionary containing additional metadata.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during metafile reading.</exception>
        public IDictionary<String, Object> ReadMetadata()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadMetadataFromStream();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during metafile reading.", ex);
            }
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Releases all resources used by the reader.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected fields

        /// <summary>
        /// Reads the device information stored in the metafile stream.
        /// </summary>
        /// <returns>The device data.</returns>
        protected virtual ImagingDevice ReadDeviceFromStream() { return null; }

        /// <summary>
        /// Reads the imaging information stored in the metafile stream.
        /// </summary>
        /// <returns>The imaging data.</returns>
        protected virtual RasterImaging ReadImagingFromStream() { return null; }

        /// <summary>
        /// Reads the raster mapping stored in the metafile stream.
        /// </summary>
        /// <returns>The raster mapping.</returns>
        protected virtual RasterMapper ReadMappingFromStream() { return null; }

        /// <summary>
        /// Reads the reference system stored in the metafile stream.
        /// </summary>
        /// <returns>The reference system.</returns>
        protected virtual IReferenceSystem ReadReferenceSystemFromStream() { return null; }

        /// <summary>
        /// Reads additional metadata stored in the metafile stream.
        /// </summary>
        /// <returns>The dictionary containing additional metadata.</returns>
        protected virtual IDictionary<String, Object> ReadMetadataFromStream() { return new Dictionary<String, Object>(); }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Dispose managed resources.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _stream.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
