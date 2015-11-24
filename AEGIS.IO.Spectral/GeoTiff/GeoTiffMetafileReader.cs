/// <copyright file="GeoTiffMetafileReader.cs" company="Eötvös Loránd University (ELTE)">
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

        /// <summary>
        /// The underlying stream.
        /// </summary>
        protected readonly Stream _stream;

        /// <summary>
        /// A value indicating whether the reader is disposed.
        /// </summary>
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
        protected GeoTiffMetafileReader(String path)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");

            _stream = new MemoryStream(File.ReadAllBytes(path));

            _disposed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffMetafileReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>ó
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
        protected GeoTiffMetafileReader(Uri path)
            : this(path.IsAbsoluteUri ? path.AbsolutePath : path.OriginalString)
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
        /// <exception cref="System.IO.IOException">Exception occurred during metafile reading.</exception>
        public ImagingDevice ReadDeviceData() 
        { 
            if (_disposed) 
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadDeviceInternal();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads the imaging information stored in the metafile.
        /// </summary>
        /// <value>The imaging data.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during metafile reading.</exception>
        public RasterImaging ReadImaging()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadImagingInternal();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads the raster mapping stored in the metafile.
        /// </summary>
        /// <value>The raster mapping.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during metafile reading.</exception>
        public RasterMapper ReadMapping()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadMappingInternal();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads the reference system stored in the metafile.
        /// </summary>
        /// <value>The reference system.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during metafile reading.</exception>
        public IReferenceSystem ReadReferenceSystem()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadReferenceSystemInternal();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads additional metadata stored in the metafile.
        /// </summary>
        /// <value>The dictionary containing additional metadata.</value>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.IO.IOException">Exception occurred during metafile reading.</exception>
        public IDictionary<String, Object> ReadMetadata()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadMetadataInternal();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during metafile reading.", ex);
            }
        }

        /// <summary>
        /// Reads file paths stored in the metafile.
        /// </summary>
        /// <returns>The list of file paths.</returns>
        public IList<String> ReadFilePaths()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            try
            {
                return ReadFilePathsInternal();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during metafile reading.", ex);
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
        /// Reads the device information stored in the metafile.
        /// </summary>
        /// <returns>The device data.</returns>
        protected virtual ImagingDevice ReadDeviceInternal() { return null; }

        /// <summary>
        /// Reads the imaging information stored in the metafile.
        /// </summary>
        /// <returns>The imaging data.</returns>
        protected virtual RasterImaging ReadImagingInternal() { return null; }

        /// <summary>
        /// Reads the raster mapping stored in the metafile.
        /// </summary>
        /// <returns>The raster mapping.</returns>
        protected virtual RasterMapper ReadMappingInternal() { return null; }

        /// <summary>
        /// Reads the reference system stored in the metafile.
        /// </summary>
        /// <returns>The reference system.</returns>
        protected virtual IReferenceSystem ReadReferenceSystemInternal() { return null; }

        /// <summary>
        /// Reads additional metadata stored in the metafile.
        /// </summary>
        /// <returns>The dictionary containing additional metadata.</returns>
        protected virtual IDictionary<String, Object> ReadMetadataInternal() { return new Dictionary<String, Object>(); }

        /// <summary>
        /// Reads file paths stored in the metafile.
        /// </summary>
        /// <returns>The list of file paths.</returns>
        protected virtual IList<String> ReadFilePathsInternal() { return null; }

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
