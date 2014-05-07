/// <copyright file="RasterMapper.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type performing affine mapping between geometry and raster space.
    /// </summary>
    public class RasterMapper : IEquatable<RasterMapper>
    {
        #region Private fields

        private readonly RasterMapMode _mode;
        private readonly Matrix _tranformationToGeometry;
        private readonly Matrix _tranformationToRaster;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the mapping mode.
        /// </summary>
        /// <value>The value indicating whether the coordinates are mapped to the center or the upper left corner of the raster pixel.</value>
        public RasterMapMode Mode { get { return _mode; } }

        /// <summary>
        /// Gets the tie coordinate of the geometry space.
        /// </summary>
        /// <value>The coordinate located at the upper left corner of the raster.</value>
        public Coordinate TieCoordinate
        {
            get
            {
                return new Coordinate(_tranformationToGeometry[0, 3], _tranformationToGeometry[1, 3], _tranformationToGeometry[2, 3]);
            }
        }

        /// <summary>
        /// Gets the size of the columns in geometry space.
        /// </summary>
        /// <value>The size of the columns in geometry space.</value>
        public Double ColumnSize
        {
            get
            {
                return _tranformationToGeometry[0, 0];
            }
        }

        /// <summary>
        /// Gets the size of the rows in geometry space.
        /// </summary>
        /// <value>The size of the rows in geometry space.</value>
        public Double RowSize
        {
            get
            {
                return _tranformationToGeometry[1, 1];
            }
        }

        /// <summary>
        /// Gets the vector difference of the columns in geometry space.
        /// </summary>
        /// <value>The vector difference of the columns in geometry space.</value>
        public CoordinateVector ColumnVector
        {
            get
            {
                return MapCoordinate(0, 1) - MapCoordinate(0, 0);
            }
        }

        /// <summary>
        /// Gets the vector difference of the rows in geometry space.
        /// </summary>
        /// <value>The vector difference of the rows in geometry space.</value>
        public CoordinateVector RowVector
        {
            get
            {
                return MapCoordinate(1, 0) - MapCoordinate(0, 0);
            }
        }

        /// <summary>
        /// Gets the transformation matrix used for computing geometry coordinates.
        /// </summary>
        /// <value>A 4x4 matrix used for computing geometry coordinates from raster coordinates.</value>
        public Matrix TransformationToGeometry
        {
            get { return _tranformationToGeometry; }
        }

        /// <summary>
        /// Gets the transformation matrix used for computing raster coordinates.
        /// </summary>
        /// <value>A 4x4 matrix used for computing raster coordinates from geometry coordinates.</value>
        public Matrix TransformationToRaster
        {
            get { return _tranformationToRaster; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMapper" /> class.
        /// </summary>
        /// <param name="transformation">The transformation from raster space to geometry space.</param>
        /// <param name="mode">The rster mapping mode.</param>
        /// <exception cref="System.ArgumentNullException">The transformation is null.</exception>
        public RasterMapper(Matrix transformation, RasterMapMode mode)
        {
            if (transformation == null)
                throw new ArgumentNullException("transformation", "The transformation is null.");

            _tranformationToGeometry = transformation;
            _tranformationToRaster = transformation.Invert();
            _mode = mode;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Maps the coordinate at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the coordinate.</param>
        /// <param name="columnIndex">The zero-based column index of the coordinate.</param>
        /// <returns>The coordinate located at the upper left corner of the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public Coordinate MapCoordinate(Int32 rowIndex, Int32 columnIndex)
        {
            Vector result;
            switch (_mode)
            {
                case RasterMapMode.ValueIsArea:
                    result = _tranformationToGeometry * new Vector(rowIndex + 0.5, columnIndex + 0.5, 0, 1);
                    break;
                default:
                    result = _tranformationToGeometry * new Vector(rowIndex, columnIndex, 0, 1);
                    break;
            }

            return new Coordinate(result[0], result[1], result[2]);
        }

        /// <summary>
        /// Maps the raster coordinate at a specified geometry coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        public void MapRaster(Coordinate coordinate, out Int32 rowIndex, out Int32 columnIndex)
        {
            Vector result = _tranformationToRaster * new Vector(coordinate.X, coordinate.Y, coordinate.Z, 1);
            switch (_mode)
            {
                case RasterMapMode.ValueIsArea:
                    rowIndex = Convert.ToInt32(result[0] - 0.5);
                    columnIndex = Convert.ToInt32(result[1] - 0.5);
                    break;
                default:
                    rowIndex = Convert.ToInt32(result[0]);
                    columnIndex = Convert.ToInt32(result[1]);
                    break;
            }
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="RasterMapper" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="RasterMapper" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(RasterMapper other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return _mode.Equals(other._mode) && _tranformationToGeometry.Equals(other._tranformationToGeometry);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (obj is RasterMapper) && _mode.Equals((obj as RasterMapper)._mode) && _tranformationToGeometry.Equals((obj as RasterMapper)._tranformationToGeometry);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return _mode.GetHashCode() ^ _tranformationToGeometry.GetHashCode() ^ 674502721;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a raster mapper from the transformation.
        /// </summary>
        /// <param name="transformation">The transformation from raster space to geometry space.</param>
        /// <param name="mode">The raster mapping mode.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        /// <exception cref="System.ArgumentNullException">The transformation is null.</exception>
        public static RasterMapper FromTransformation(Matrix transformation, RasterMapMode mode)
        {
            return new RasterMapper(transformation, mode);
        }

        /// <summary>
        /// Creates a raster mapper from the transformation.
        /// </summary>
        /// <param name="translationX">The translation in X dimension.</param>
        /// <param name="translationY">The translation in Y dimension.</param>
        /// <param name="translationZ">The translation in Z dimension.</param>
        /// <param name="scaleX">The scale in X dimension.</param>
        /// <param name="scaleY">The scale in Y dimension.</param>
        /// <param name="scaleZ">The scale in Z dimension.</param>
        /// <param name="mode">The raster mapping mode.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        public static RasterMapper FromTransformation(Double translationX, Double translationY, Double translationZ, Double scaleX, Double scaleY, Double scaleZ, RasterMapMode mode)
        {
            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = scaleX;
            transformation[1, 1] = scaleY;
            transformation[2, 2] = scaleZ;
            transformation[0, 3] = translationX;
            transformation[1, 3] = translationY;
            transformation[2, 3] = translationZ;

            return new RasterMapper(transformation, mode);
        }

        /// <summary>
        /// Creates a raster mapper from the raster and geometry coordinates.
        /// </summary>
        /// <param name="mappings">The mappings (in clockwise order).</param>
        /// <param name="mode">The raster mapping mode.</param>
        /// <returns>The raster mapper based on the specified mappings.</returns>
        /// <exception cref="System.ArgumentNullException">The list of mappings is null.</exception>
        public static RasterMapper FromMappings(IList<RasterMapping> mappings, RasterMapMode mode)
        {
            if (mappings == null)
                throw new ArgumentNullException("mappings", "The list of mappings is null.");

            // TODO: add mapping count checking

            Vector[] rasterVectors = mappings.Select(mapping => new Vector(mapping.RowIndex, mapping.ColumnIndex, 0, 1)).ToArray();
            Vector[] coordianteVectors = mappings.Select(mapping => new Vector(mapping.Coordinate.X, mapping.Coordinate.Y, mapping.Coordinate.Z, 1)).ToArray();

            // TODO: add matrix creation

            return null;
        }

        #endregion
    }
}
