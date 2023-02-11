// <copyright file="EsriRawImageReader.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.IO.Storage;
using ELTE.AEGIS.IO.WellKnown;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.IO.RawImage
{
    /// <summary>
    /// Represents an Esri raw image format reader.
    /// </summary>
    public class EsriRawImageReader : RawImageReader
    {
        #region Private fields

        private readonly String _basePath;
        private readonly String _baseFileName;
        private readonly FileSystem _fileSystem;
        private IReferenceSystem _referenceSystem;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EsriRawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public EsriRawImageReader(String path) 
            : base(path, SpectralGeometryStreamFormats.EsriRawImage, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path);

            try
            {
                ReadHeader();
                ReadReferenceSystem();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during stream opening.", ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EsriRawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public EsriRawImageReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.EsriRawImage, parameters)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path);

            try
            {
                ReadHeader();
                ReadReferenceSystem();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during stream opening.", ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EsriRawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public EsriRawImageReader(Uri path)
            : base(path, SpectralGeometryStreamFormats.EsriRawImage, null)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path.AbsolutePath);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path.AbsolutePath);

            try
            {
                ReadHeader();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during stream opening.", ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EsriRawImageReader" /> class.
        /// </summary>
        /// <param name="path">The file path to be read.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public EsriRawImageReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.EsriRawImage, parameters)
        {
            _fileSystem = FileSystem.GetFileSystemForPath(path);

            _basePath = _fileSystem.GetDirectory(path.AbsolutePath);
            _baseFileName = _fileSystem.GetFileNameWithoutExtension(path.AbsolutePath);

            try
            {
                ReadHeader();
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occurred during stream opening.", ex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the header file.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The header file is not available.</exception>
        private void ReadHeader()
        {
            String headerFileName;
            if (_fileSystem.Exists(_basePath + _fileSystem.DirectorySeparator + _baseFileName + ".hdr"))
                headerFileName = _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".hdr";
            else if (_fileSystem.Exists(_basePath + _fileSystem.DirectorySeparator + _baseFileName + ".HDR"))
                headerFileName = _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".HDR";
            else
                throw new FileNotFoundException("The header file is not available.", _basePath + _fileSystem.DirectorySeparator + _baseFileName + ".hdr");

            using (StreamReader reader = new StreamReader(headerFileName))
            {
                Double upperLeftX = 0, upperLeftY = 0, vectorX = 0, vectorY = 0;
                Regex regex = new Regex(@"\s");

                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    if (String.IsNullOrEmpty(line) || line[0] == '#')
                        continue;

                    String[] lineParts = regex.Split(line.ToUpper()).Where(linePart => !String.IsNullOrEmpty(linePart)).ToArray();

                    if (lineParts.Length >= 2)
                    {
                        switch (lineParts[0])
                        { 
                            case "BYTEORDER":
                                _byteOrder = (lineParts[1].Equals("I")) ? ByteOrder.LittleEndian : ByteOrder.BigEndian;
                                break;
                            case "LAYOUT":
                                switch (lineParts[1])
                                {
                                    case "BIP":
                                        _layout = RawImageLayout.BandInterlevedByPixel;
                                        break;
                                    case "BIL":
                                        _layout = RawImageLayout.BandInterlevedByLine;
                                        break;
                                    case "BSQ":
                                        _layout = RawImageLayout.BandSequential;
                                        break;
                                }
                                break;
                            case "NROWS":
                                _numberOfRows = Int32.Parse(lineParts[1]);
                                break;
                            case "NCOLS":
                                _numberOfColumns = Int32.Parse(lineParts[1]);
                                break;
                            case "NBANDS":
                                _numberOfBands = Int32.Parse(lineParts[1]);
                                break;
                            case "NBITS":
                                _radiometricResolution = Int32.Parse(lineParts[1]);
                                break;
                            case "ULXMAP":
                                upperLeftX = Double.Parse(lineParts[1], CultureInfo.InvariantCulture);
                                break;
                            case "ULYMAP":
                                upperLeftY = Double.Parse(lineParts[1], CultureInfo.InvariantCulture);
                                break;
                            case "XDIM":
                                vectorX = Double.Parse(lineParts[1], CultureInfo.InvariantCulture);
                                break;
                            case "YDIM":
                                vectorY = Double.Parse(lineParts[1], CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                }

                _bytesGapPerBand = 0;
                _bytesPerBandRow = (Int32)Math.Ceiling(_numberOfColumns * _radiometricResolution / 8.0);
                _bytesPerRow = (Int32)Math.Ceiling(_numberOfRows * _numberOfColumns * _radiometricResolution / 8.0);
                _bytesSkipped = 0;

                if (vectorX != 0 && vectorY != 0)
                    _mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, upperLeftX, upperLeftY, 0, vectorX, vectorY, 0);
            }
        }
        /// <summary>
        /// Reads the reference system.
        /// </summary>
        private void ReadReferenceSystem()
        {
            // read the reference system from the WKT text
            using (StreamReader reader = new StreamReader(_fileSystem.OpenFile(_basePath + _fileSystem.DirectorySeparator + _baseFileName + ".prj", FileMode.Open)))
            {
                StringBuilder builder = new StringBuilder();
                while (!reader.EndOfStream)
                    builder.Append(reader.ReadLine().Trim());

                _referenceSystem = WellKnownTextConverter.ToReferenceSystem(builder.ToString());
            }
        }

        #endregion
    }
}
