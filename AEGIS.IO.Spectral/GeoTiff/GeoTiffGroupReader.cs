/// <copyright file="GeoTiffGroupReader.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
using System.Linq;

namespace ELTE.AEGIS.IO.GeoTiff
{
    /// <summary>
    /// Represents a group reader of GeoTIFF format files.
    /// </summary>
    public class GeoTiffGroupReader : GeometryStreamReader
    {
        #region Private fields

        /// <summary>
        /// The maximum number of bands. This field is constant.
        /// </summary>
        private const Int32 MaxBandCount = 50;

        /// <summary>
        /// The list of file paths.
        /// </summary>
        private List<String> _filePaths;
        
        /// <summary>
        /// The GeoTIFF metafile reader.
        /// </summary>
        private GeoTiffMetafileReader _metafileReader;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffGroupReader" /> class.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <exception cref="System.ArgumentNullException">The base path is null.</exception>
        public GeoTiffGroupReader(String basePath)
            : this(basePath, null)
        { 
        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffGroupReader" /> class.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The base path is null.</exception>
        public GeoTiffGroupReader(String basePath, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            if (basePath == null)
                throw new ArgumentNullException("basePath", "The base path is null.");

            Boolean includeMetadata = ResolveParameter<Boolean>(SpectralGeometryStreamParameters.IncludeMetadata);

            FileSystem fileSystem = FileSystem.GetFileSystemForPath(basePath);
            
            _filePaths = new List<String>();

            // the base path is a directory
            if (fileSystem.IsDirectory(basePath))
            {
                if (includeMetadata)
                {
                    _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(basePath, GeoTiffMetafilePathOption.IsDirectoryPath);
                }
                
                if (_metafileReader != null)
                {
                    // load files from the path specified by the metafile
                    List<String> filePaths = _metafileReader.ReadFilePaths().Select(path => fileSystem.Combine(basePath, path)).ToList();

                    // check whether the specified files exist
                    foreach (String filePath in filePaths)
                        if (fileSystem.Exists(filePath))
                            _filePaths.Add(filePath);                   
                }

                if (_filePaths.Count == 0)
                {
                    _filePaths = fileSystem.GetFiles(basePath, "*.tif", false).Union(fileSystem.GetFiles(basePath, "*.TIF", false)).ToList();
                }
            }
            else
            {
                String directoryPath = fileSystem.GetDirectory(basePath);
                String fileNameBase = fileSystem.GetFileNameWithoutExtension(basePath);
                String extension = fileSystem.GetExtension(basePath);

                if (includeMetadata)
                {
                    _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(fileSystem.Combine(directoryPath, fileNameBase + "*"), GeoTiffMetafilePathOption.IsSearchPattern);
                }

                if (_metafileReader != null)
                {
                    _filePaths = _metafileReader.ReadFilePaths().Select(path => fileSystem.Combine(directoryPath, path)).ToList();
                }
                else
                {
                    _filePaths = fileSystem.GetFiles(directoryPath, fileNameBase + "*" + extension, false).ToList();
                }                
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoTiffGroupReader" /> class.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="parameters">The parameters of the reader for the specific stream.</param>
        /// <exception cref="System.ArgumentNullException">The collection of file paths is null.</exception>
        public GeoTiffGroupReader(IEnumerable<String> filePaths, IDictionary<GeometryStreamParameter, Object> parameters)
            : base(SpectralGeometryStreamFormats.GeoTiff, parameters)
        {
            if (filePaths == null)
                throw new ArgumentNullException("basePath", "The collection of file paths is null.");

            _filePaths = filePaths.Where(filePath => filePath.ToLower().EndsWith(".tif")).ToList();

            Boolean includeMetadata = ResolveParameter<Boolean>(SpectralGeometryStreamParameters.IncludeMetadata);

            if (includeMetadata)
            {
                foreach (String path in filePaths.Where(filePath => !filePath.ToLower().EndsWith(".tif")))
                {
                    _metafileReader = GeoTiffMetafileReaderFactory.CreateReader(path, GeoTiffMetafilePathOption.IsMetafilePath);

                    if (_metafileReader != null)
                        break;
                }
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
               
            // read imaging data
            RasterImaging imaging = _metafileReader == null ? null : _metafileReader.ReadImaging();

            // read files
            List<ISpectralPolygon> polygons = new List<ISpectralPolygon>();

            Dictionary<GeometryStreamParameter, Object> parameters = new Dictionary<GeometryStreamParameter, Object>(Parameters);
            parameters[SpectralGeometryStreamParameters.IncludeMetadata] = false;

            foreach (String filePath in _filePaths)
            {
                try
                {
                    using (GeoTiffReader reader = new GeoTiffReader(filePath, parameters))
                    {
                        polygons.Add(reader.Read() as ISpectralPolygon);
                    }
                }
                catch { }
            }

            _filePaths.Clear();

            // if no polygons are available
            if (polygons.Count == 0)
                return null;

                // remove rasters with different size
                for (Int32 index = polygons.Count - 1; index >= 0; index--)
                {
                    if (polygons[index].Raster.NumberOfRows != polygons[0].Raster.NumberOfRows || polygons[index].Raster.NumberOfColumns != polygons[0].Raster.NumberOfColumns)
                    {
                        polygons.RemoveAt(index);
                        if (imaging != null)
                            imaging.RemoveBand(index);
                    }
                }

                // merge content
                return polygons[0].Factory.CreateSpectralPolygon(polygons, imaging);
        }

        #endregion
    }
}
