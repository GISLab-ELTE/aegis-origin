/// <copyright file="GeoTiffGroupReader.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.IO.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Represents a group reader of GeoTIFF format files.
    /// </summary>
    public class GeoTiffGroupReader : GeometryStreamReader
    {
        #region Private fields

        private const Int32 MaxBandCount = 50;
        private List<String> _filePaths;
        private GeoTiffMetafileReader _metafileReader;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffGroupReader" /> class.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <exception cref="System.ArgumentNullException">The base path is null.</exception>
        public GeoTiffGroupReader(String basePath) 
            : base(SpectralGeometryStreamFormats.GeoTiff, null)
        {
            if (basePath == null)
                throw new ArgumentNullException("basePath", "The base path is null.");

            FileSystem fileSystem = FileSystem.GetFileSystemForPath(basePath);

            _filePaths = new List<String>();

            // the base path is a directory
            if (fileSystem.IsDirectory(basePath))
            {
                _filePaths = fileSystem.GetFiles(basePath).ToList();
                return;
            }

            // the base path includes part of the file name
            String directoryPath = fileSystem.GetDirectory(basePath);
            String fileNameBase = fileSystem.GetFileNameWithoutExtension(basePath);
            String fileNameExtension = fileSystem.GetExtension(basePath);

            // try with underscore
            Int32 fileNumber = 1;
            String currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase + "_" + fileNumber + fileNameExtension;


            for (Int32 bandIndex = 0; bandIndex < MaxBandCount; bandIndex++)
            {
                if (fileSystem.Exists(currentFileName))
                {
                    _filePaths.Add(currentFileName);
                }
                fileNumber++;
                currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase + "_" + fileNumber + fileNameExtension;
            }

            // try with underscore and band
            if (_filePaths.Count == 0)
            {
                fileNumber = 1;
                currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase + "_B" + fileNumber + fileNameExtension;

                for (Int32 bandIndex = 0; bandIndex < MaxBandCount; bandIndex++)
                {
                    if (fileSystem.Exists(currentFileName))
                    {
                        _filePaths.Add(currentFileName);
                    }
                    fileNumber++;
                    currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase + "_B" + fileNumber + fileNameExtension;
                }
            }

            // try without underscore
            if (_filePaths.Count == 0)
            {
                fileNumber = 1;
                currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase + fileNumber + fileNameExtension;

                for (Int32 bandIndex = 0; bandIndex < MaxBandCount; bandIndex++)
                {
                    if (fileSystem.Exists(currentFileName))
                    {
                        _filePaths.Add(currentFileName);
                    }
                    fileNumber++;
                    currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase + fileNumber + fileNameExtension;
                }
            }

            // try with replacement
            if (_filePaths.Count == 0)
            {
                fileNumber = 1;
                currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase.Replace("*", fileNumber.ToString()) + fileNameExtension;

                for (Int32 bandIndex = 0; bandIndex < MaxBandCount; bandIndex++)
                {
                    if (fileSystem.Exists(currentFileName))
                    {
                        _filePaths.Add(currentFileName);
                    }
                    fileNumber++;
                    currentFileName = directoryPath + fileSystem.DirectorySeparator + fileNameBase.Replace("*", fileNumber.ToString()) + fileNameExtension;
                }
            }

            try
            {
                _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(directoryPath + fileSystem.DirectorySeparator + fileNameBase.TrimEnd('B', '_') + "*", GeoTiffMetafilePathOption.IsSearchPattern);
            }
            catch { }
        }

        #endregion

        #region Protected GeometryStreamReader methods

        /// <summary>
        /// Returns a value indicating whether the end of the stream is reached.
        /// </summary>
        /// <returns><c>true</c> if the end of the stream is reached; otherwise, <c>false</c>.</returns>
        protected override Boolean GetEndOfStream()
        {
            return _filePaths.Count == 0;
        }

        /// <summary>
        /// Apply the read operation for a geometry.
        /// </summary>
        /// <returns>The geometry read from the stream.</returns>
        protected override IGeometry ApplyReadGeometry()
        {
            if (_filePaths.Count == 0)
                return null;
                
            List<ISpectralPolygon> polygons = new List<ISpectralPolygon>();

            foreach (String filePath in _filePaths)
            {
                if (filePath.Contains("hdfs"))
                {
                    FileStream fileStream = new FileStream(filePath, FileMode.Open);
                    ProxyStream proxyStream = new ProxyStream(fileStream, true, true);

                    using (GeoTiffReader reader = new GeoTiffReader(proxyStream))
                    {
                        polygons.Add(reader.Read() as ISpectralPolygon);
                    }
                }
                else
                {
                    using (GeoTiffReader reader = new GeoTiffReader(filePath))
                    {
                        polygons.Add(reader.Read() as ISpectralPolygon);
                    }
                }
            }

            _filePaths.Clear();

            // merge content
            return polygons[0].Factory.CreateSpectralPolygon(polygons, _metafileReader.ReadImaging());
        }

        #endregion
    }
}
