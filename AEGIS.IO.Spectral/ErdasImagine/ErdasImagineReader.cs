/// <copyright file="ErdasImagineReader.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using ELTE.AEGIS.IO.ErdasImagine.Objects;
using ELTE.AEGIS.IO.ErdasImagine.Types;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.IO.ErdasImagine
{
    /// <summary>
    /// Represents an Erdas Imagine file format (IMG) file reader.
    /// </summary>
    /// <remarks>
    /// This format is used widely for processing remote sensing data, since it provides a framework for integrating sensor data and imagery from many sources.
    /// </remarks>
    [IdentifiedObjectInstance("AEGIS::610210", "Erdas Imagine file format")]
    public class ErdasImagineReader : GeometryStreamReader
    {
        #region Private fields

        /// <summary>
        /// The root entry.
        /// </summary>
        private EhfaEntry _rootEntry;

        /// <summary>
        /// The HFA dictionary.
        /// </summary>
        private HfaDictionary _dictionary;

        /// <summary>
        /// The list of image layers.
        /// </summary>
        private IList<ImageLayer> _layers;

        /// <summary>
        /// A value indicating whether the position is at the end of the stream.
        /// </summary>
        private Boolean _endOfStream;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ErdasImagineReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public ErdasImagineReader(String path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.ErdasImagine, parameters)
        {
            _endOfStream = false;

            try
            {
                ReadHeader();
                ReadLayers();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentOpenError, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErdasImagineReader" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The path is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty, or consists only of whitespace characters.
        /// or
        /// The path is in an invalid format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public ErdasImagineReader(Uri path, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(path, SpectralGeometryStreamFormats.ErdasImagine, parameters)
        {
            _endOfStream = false;

            try
            {
                ReadHeader();
                ReadLayers();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentOpenError, ex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErdasImagineReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.IO.IOException">Exception occurred during stream opening.</exception>
        public ErdasImagineReader(Stream stream, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(stream, SpectralGeometryStreamFormats.ErdasImagine, parameters)
        {
            _endOfStream = false;

            try
            {
                ReadHeader();
                ReadLayers();
            }
            catch (Exception ex)
            {
                throw new IOException(MessageContentOpenError, ex);
            }
        }

        #endregion

        #region Protected GeometryStreamReader methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise, <c>false</c>.</returns>
        protected override Boolean GetEndOfStream()
        {
            return _endOfStream;
        }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        protected override IGeometry ApplyReadGeometry()
        {
            ISpectralPolygon spectralPolygon = ResolveFactory(_layers[0].ReferenceSystem).CreateSpectralPolygon(_layers.Count, (Int32)_layers[0].Height, (Int32)_layers[0].Width, _layers.Max(layer => layer.RadiometricResolution), _layers[0].RasterMapper);
            
            for (Int32 bandIndex = 0; bandIndex < _layers.Count; bandIndex++)
            {
                _layers[bandIndex].ReadRasterBand(BaseStream, spectralPolygon.Raster[bandIndex]);
            }

            _endOfStream = true;
            return spectralPolygon;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the file header.
        /// </summary>
        private void ReadHeader()
        {
            EhfaStructure header = new EhfaStructure(EhfaStructureType.EhfaHeaderTagType);
            header.Read(BaseStream);

            Int32 headerLocation = header.GetValue<Int32>("headerPtr");
            BaseStream.Seek(headerLocation, SeekOrigin.Begin);

            EhfaStructure rootFile = new EhfaStructure(EhfaStructureType.EhfaFileType);
            rootFile.Read(BaseStream);

            Int32 dictionaryLocation = rootFile.GetValue<Int32>("dictionaryPtr");
            Int32 rootEntryLocation = rootFile.GetValue<Int32>("rootEntryPtr");

            BaseStream.Seek(dictionaryLocation, SeekOrigin.Begin);

            _dictionary = HfaDictionary.Parse(ReadDictionaryString());

            BaseStream.Seek(rootEntryLocation, SeekOrigin.Begin);
            _rootEntry = new EhfaEntry(_dictionary["Ehfa_Entry"], _dictionary);
            _rootEntry.Read(BaseStream);
        }

        /// <summary>
        /// Reads the image layers.
        /// </summary>
        private void ReadLayers()
        {
            _layers = new List<ImageLayer>();
            foreach (EhfaEntry entry in _rootEntry.Children.Where(x => x.DataType.Equals("Eimg_Layer")))
            {
                ImageLayer layer = new ImageLayer(_dictionary["Eimg_Layer"], entry);
                BaseStream.Seek(entry.DataLocation, SeekOrigin.Begin);
                layer.Read(BaseStream);
                entry.Data = layer;

                _layers.Add(layer);
            }
        }

        /// <summary>
        /// Reads the string containing the dictionary.
        /// </summary>
        /// <param name="baseStream">The base stream.</param>
        /// <returns>The dictionary in <see cref="String" /> format.</returns>
        private String ReadDictionaryString()
        {
            StringBuilder dictionaryBuilder = new StringBuilder();
            Boolean finished = false;
            Byte terminatorByte = Convert.ToByte('.');
            Byte[] buffer = new Byte[1024];
            String bufferString;

            do
            {
                BaseStream.Read(buffer, 0, buffer.Length);

                Int32 terminatorByteIndex = Array.IndexOf(buffer, terminatorByte);

                if (terminatorByteIndex != -1)
                {
                    bufferString = Encoding.UTF8.GetString(buffer, 0, terminatorByteIndex + 2);
                    finished = true;
                }
                else
                    bufferString = Encoding.UTF8.GetString(buffer);

                dictionaryBuilder.Append(bufferString);
            } while (!finished);

            return dictionaryBuilder.ToString();
        }

        #endregion
    }
}
