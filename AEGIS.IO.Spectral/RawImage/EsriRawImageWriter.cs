/// <copyright file="EsriRawImageWriter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using ELTE.AEGIS.IO.Storage;
using ELTE.AEGIS.IO.WellKnown;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.IO;

namespace ELTE.AEGIS.IO.Spectral.RawImage
{
    /// <summary>
    /// Represents an Esri raw image format writer.
    /// </summary>
    [IdentifiedObjectInstance("AEGIS::610211", "Esri raw image")]
    public class EsriRawImageWriter : RawImageWriter
    {
        #region Private fields

        private readonly String _basePath;
        private readonly String _baseFileName;
        private readonly FileSystem _fileSystem;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EsriRawImageWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public EsriRawImageWriter(String path)
            : base(path, SpectralGeometryStreamFormats.EsriRawImage, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EsriRawImageWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public EsriRawImageWriter(Uri path)
            : base(path, SpectralGeometryStreamFormats.EsriRawImage, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path.AbsolutePath);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path.AbsolutePath);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Writes the header file.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        protected override void WriteGeometryMetadata(IDictionary<String, Object> metadata)
        {
            String headerFileName;
            if (_fileSystem.Exists(_basePath + _fileSystem.DirectorySeparator + _baseFileName + ".hdr"))
                headerFileName = _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".hdr";
            else if (_fileSystem.Exists(_basePath + _fileSystem.DirectorySeparator + _baseFileName + ".HDR"))
                headerFileName = _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".HDR";
            else
                headerFileName = _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".hdr";

            using (StreamWriter writer = new StreamWriter(headerFileName))
            {
                writer.WriteLine("NROWS " + _numberOfRows.ToString());
                writer.WriteLine("NCOLS " + _numberOfColumns.ToString());
                writer.WriteLine("NBANDS " + _numberOfBands.ToString());
                writer.WriteLine("NBITS " + _radiometricResolution.ToString());
                writer.WriteLine("BYTEORDER " + _byteOrder.ToString());
                writer.WriteLine("LAYOUT " + _layout.ToString());
                writer.WriteLine("SKIPBYTES " + _bytesSkipped.ToString());
                writer.WriteLine("ULXMAP " + _upperLeftX.ToString());
                writer.WriteLine("ULYMAP " + _upperLeftY.ToString());
                writer.WriteLine("XDIM " + _vectorX.ToString());
                writer.WriteLine("YDIM " + _vectorY.ToString());
                writer.WriteLine("BANDGAPBYTES 0 ");
                writer.WriteLine("BANDROWBYTES " + _bytesPerBandRow.ToString());
                writer.WriteLine("TOTALROWBYTES " + _bytesPerRow.ToString());
            }
        }

        /// <summary>
        /// Writes the reference system file.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        protected override void WriteGeometryReferenceSystem(IReferenceSystem referenceSystem)
        {
            using (StreamWriter writer = new StreamWriter(_fileSystem.CreateFile(_basePath + _fileSystem.DirectorySeparator + _baseFileName + ".prj")))
            {
                String refSystem = WellKnownTextConverter.ToWellKnownText(referenceSystem);
                writer.Write(refSystem);
            }
        }

        #endregion
    }
}
