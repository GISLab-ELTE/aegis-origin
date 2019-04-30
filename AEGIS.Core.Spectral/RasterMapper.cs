/// <copyright file="RasterMapper.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type performing mapping between raster space and geometry space.
    /// </summary>
    public class RasterMapper : IEquatable<RasterMapper>
    {
        #region Private fields

        /// <summary>
        /// The raster transformation matrix.
        /// </summary>
        private Matrix _rasterTransformation;

        /// <summary>
        /// The hash code of the current object.
        /// </summary>
        private Int32 _hashCode;

        /// <summary>
        /// A value indicating whether the transformation is linear (only translation and scale are used).
        /// </summary>
        private Boolean _isLinearTransformation;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the mapping mode.
        /// </summary>
        /// <value>The value indicating whether the coordinates are mapped to the center or the upper left corner of the raster pixel.</value>
        public RasterMapMode Mode { get; private set; }

        /// <summary>
        /// Gets the translation coordinate.
        /// </summary>
        /// <value>The tie coordinate located at the upper left corner of the raster.</value>
        public Coordinate Translation
        {
            get
            {
                return new Coordinate(GeometryTransformation[0, 3], GeometryTransformation[1, 3], GeometryTransformation[2, 3]);
            }
        }

        /// <summary>
        /// Gets the scale vector.
        /// </summary>
        /// <value>The vector containing the scale in all dimensions.</value>
        public CoordinateVector Scale
        {
            get
            {
                return new CoordinateVector(GeometryTransformation[0, 0], GeometryTransformation[1, 1], GeometryTransformation[2, 2]);
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
                return ColumnVector.Length;
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
                return RowVector.Length;
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
        public Matrix GeometryTransformation { get; private set; }

        /// <summary>
        /// Gets the transformation matrix used for computing raster coordinates.
        /// </summary>
        /// <value>A 4x4 matrix used for computing raster coordinates from geometry coordinates.</value>
        public Matrix RasterTransformation 
        { 
            get 
            {
                if (_rasterTransformation == null)
                    _rasterTransformation = GeometryTransformation.Invert();

                return _rasterTransformation;
            } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterMapper" /> class.
        /// </summary>
        /// <param name="mode">The raster mapping mode.</param>
        /// <param name="transformation">The transformation from raster space to geometry space.</param>
        /// <exception cref="System.ArgumentNullException">The transformation is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of columns in the transformation is not equal to 4.
        /// or
        /// The number of rows in the transformation is not equal to 4.
        /// or
        /// The transformation contains invalid values.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified transformation is not supported.</exception>
        public RasterMapper(RasterMapMode mode, Matrix transformation)
        {
            if (transformation == null)
                throw new ArgumentNullException("transformation", "The transformation is null.");
            if (transformation.NumberOfColumns != 4)
                throw new ArgumentException("The number of columns in the transformation is not equal to 4.", "transformation");
            if (transformation.NumberOfRows != 4)
                throw new ArgumentException("The number of rows in the transformation is not equal to 4.", "transformation");
            
            // check for invalid values
            if (transformation.Any(value => Double.IsNaN(value) || Double.IsInfinity(value)))
                throw new ArgumentException("The transformation contains invalid values.", "transformation");
            if (transformation[3, 0] != 0 || transformation[3, 1] != 0 || transformation[3, 2] != 0 || transformation[3, 3] != 1)
                throw new ArgumentException("The transformation contains invalid values.", "transformation");

            // check for unsupported transformation
            if (transformation[0, 0] == 0 || transformation[1, 1] == 0 || 
                transformation[2, 0] != 0 || transformation[2, 1] != 0 || 
                transformation[0, 2] != 0 || transformation[1, 2] != 0)
                throw new NotSupportedException("The specified transformation is not supported.");

            _isLinearTransformation = (transformation[0, 1] == 0 && transformation[1, 0] == 0); // rotation is not specified

            Mode = mode;
            GeometryTransformation = transformation;

            if (!_isLinearTransformation)
                _rasterTransformation = transformation.Invert();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Maps the coordinate at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the coordinate.</param>
        /// <param name="columnIndex">The zero-based column index of the coordinate.</param>
        /// <returns>The coordinate located at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public Coordinate MapCoordinate(Int32 rowIndex, Int32 columnIndex)
        {
            if (_isLinearTransformation)
                return new Coordinate(GeometryTransformation[0, 0] * columnIndex + GeometryTransformation[0, 3], 
                                      GeometryTransformation[1, 1] * rowIndex + GeometryTransformation[1, 3],
                                      GeometryTransformation[2, 3]);

            Vector result = GeometryTransformation * new Vector(columnIndex, rowIndex, 0, 1);

            return new Coordinate(result[0], result[1], result[2]);
        }

        /// <summary>
        /// Maps the coordinate at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the coordinate.</param>
        /// <param name="columnIndex">The zero-based column index of the coordinate.</param>
        /// <returns>The coordinate located at the specified index.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public Coordinate MapCoordinate(Double rowIndex, Double columnIndex)
        {
            if (_isLinearTransformation)
                return new Coordinate(GeometryTransformation[0, 0] * columnIndex + GeometryTransformation[0, 3],
                                      GeometryTransformation[1, 1] * rowIndex + GeometryTransformation[1, 3],
                                      GeometryTransformation[2, 3]);

            Vector result = GeometryTransformation * new Vector(columnIndex, rowIndex, 0, 1);

            return new Coordinate(result[0], result[1], result[2]);
        }

        /// <summary>
        /// Maps the coordinate at a specified row and column index.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the coordinate.</param>
        /// <param name="columnIndex">The zero-based column index of the coordinate.</param>
        /// <param name="mode">The mapping mode.</param>
        /// <returns>The coordinate located at the specified index with respect to the specified mode.</returns>
        /// <exception cref="System.InvalidOperationException">The mapping of the raster is not defined.</exception>
        public Coordinate MapCoordinate(Double rowIndex, Double columnIndex, RasterMapMode mode)
        {
            if (mode == Mode)
            {
                return MapCoordinate(rowIndex, columnIndex);
            }
            else
            {
                switch (mode)
                { 
                    case RasterMapMode.ValueIsArea:
                        return MapCoordinate(rowIndex - 0.5, columnIndex - 0.5);
                    default: // ValueIsCoordinate
                        return MapCoordinate(rowIndex + 0.5, columnIndex + 0.5);
                }
            }
        }
        
        /// <summary>
        /// Maps the raster row and column index at the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        public void MapRaster(Coordinate coordinate, out Int32 rowIndex, out Int32 columnIndex)
        {
            if (_isLinearTransformation)
            {
                columnIndex = Convert.ToInt32((coordinate.X - GeometryTransformation[0, 3]) / GeometryTransformation[0, 0]);
                rowIndex = Convert.ToInt32((coordinate.Y - GeometryTransformation[1, 3]) / GeometryTransformation[1, 1]);
            }
            else
            {
                Vector result = RasterTransformation * new Vector(coordinate.X, coordinate.Y, coordinate.Z, 1);

                columnIndex = Convert.ToInt32(result[0]);
                rowIndex = Convert.ToInt32(result[1]);
            }
        }

        /// <summary>
        /// Maps the raster row and column index at the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        public void MapRaster(Coordinate coordinate, out Double rowIndex, out Double columnIndex)
        {
            if (_isLinearTransformation)
            {
                columnIndex = (coordinate.X - GeometryTransformation[0, 3]) / GeometryTransformation[0, 0];
                rowIndex =  (coordinate.Y - GeometryTransformation[1, 3]) / GeometryTransformation[1, 1];
            }
            else
            {
                Vector result = RasterTransformation * new Vector(coordinate.X, coordinate.Y, coordinate.Z, 1);

                columnIndex = result[0];
                rowIndex = result[1];
            }
        }

        /// <summary>
        /// Maps the raster row and column index at the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="mode">The mapping mode.</param>
        /// <param name="rowIndex">The zero-based column index of the value.</param>
        /// <param name="columnIndex">The zero-based row index of the value.</param>
        public void MapRaster(Coordinate coordinate, RasterMapMode mode, out Double rowIndex, out Double columnIndex)
        {
            if (mode == Mode)
            {
                MapRaster(coordinate, out rowIndex, out columnIndex);
            }
            else
            {
                switch (mode)
                {
                    case RasterMapMode.ValueIsArea:
                        MapRaster(coordinate, out rowIndex, out columnIndex);
                        rowIndex += 0.5;
                        columnIndex += 0.5;
                        break;
                    default: // ValueIsCoordinate
                        MapRaster(coordinate, out rowIndex, out columnIndex);
                        rowIndex -= 0.5;
                        columnIndex -= 0.5;
                        break;
                }
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

            return Mode.Equals(other.Mode) && GeometryTransformation.SequenceEqual(other.GeometryTransformation);
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

            return (obj is RasterMapper) && Mode.Equals((obj as RasterMapper).Mode) && GeometryTransformation.SequenceEqual((obj as RasterMapper).GeometryTransformation);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            if (_hashCode == 0)
            {
                _hashCode = Mode.GetHashCode();
                foreach (Double value in GeometryTransformation)
                    _hashCode ^= value.GetHashCode();
                _hashCode ^= 674502721;
            }

            return _hashCode;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a raster mapper from raster coordinates.
        /// </summary>
        /// <param name="mode">The raster mapping mode.</param>
        /// <param name="coordinates">The array of coordinates.</param>
        /// <returns>The raster mapper created by linear interpolation of the coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The array of coordinates is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of coordinates with distinct column index is less than 2.
        /// or
        /// The number of coordinates with distinct row index is less than 2.
        /// </exception>
        public static RasterMapper FromCoordinates(RasterMapMode mode, params RasterCoordinate[] coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("The array of coordinates is null.", "coordinates");

            return FromCoordinates(mode, coordinates as IList<RasterCoordinate>);
        }

        /// <summary>
        /// Creates a raster mapper from raster coordinates.
        /// </summary>
        /// <param name="mode">The raster mapping mode.</param>
        /// <param name="coordinates">The list of coordinates.</param>
        /// <returns>The raster mapper created by linear interpolation of the coordinates.</returns>
        /// <exception cref="System.ArgumentNullException">The array of coordinates is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of coordinates with distinct column index is less than 2.
        /// or
        /// The number of coordinates with distinct row index is less than 2.
        /// </exception>
        public static RasterMapper FromCoordinates(RasterMapMode mode, IList<RasterCoordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("The list of coordinates is null.", "coordinates");
            if (coordinates.Select(coordinate => coordinate.ColumnIndex).Distinct().Count() < 2)
                throw new ArgumentException("The number of coordinates with distinct column index is less than 2.", "coordinates");
            if (coordinates.Select(coordinate => coordinate.RowIndex).Distinct().Count() < 2)
                throw new ArgumentException("The number of coordinates with distinct row index is less than 2.", "coordinates");

            // compute linear equation system in both dimensions

            Int32[] columnIndices = coordinates.Select(coordinate => coordinate.ColumnIndex).Distinct().ToArray();
            Int32[] rowIndices = coordinates.Select(coordinate => coordinate.ColumnIndex).Distinct().ToArray();

            Matrix matrix = new Matrix(coordinates.Count, 3);
            Vector vectorX = new Vector(coordinates.Count);
            Vector vectorY = new Vector(coordinates.Count);

            for (Int32 coordinateIndex = 0; coordinateIndex < coordinates.Count; coordinateIndex++)
            { 
                matrix[coordinateIndex, 0] = coordinates[coordinateIndex].ColumnIndex;
                matrix[coordinateIndex, 1] = coordinates[coordinateIndex].RowIndex;
                matrix[coordinateIndex, 2] = 1;

                vectorX[coordinateIndex] = coordinates[coordinateIndex].Coordinate.X;
                vectorY[coordinateIndex] = coordinates[coordinateIndex].Coordinate.Y;
            }

            // solve equation using least squares method
            Vector resultX = LUDecomposition.SolveEquation(matrix.Transpone() * matrix, matrix.Transpone() * vectorX);
            Vector resultY = LUDecomposition.SolveEquation(matrix.Transpone() * matrix, matrix.Transpone() * vectorY);

            // merge the results into a matrix
            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = resultX[0];
            transformation[0, 1] = resultX[1];
            transformation[0, 3] = resultX[2];
            transformation[1, 0] = resultY[0];
            transformation[1, 1] = resultY[1];
            transformation[1, 3] = resultY[2];
            transformation[3, 3] = 1;

            return new RasterMapper(mode, transformation);
        }

        /// <summary>
        /// Creates a raster mapper from another mapper.
        /// </summary>
        /// <param name="mapper">The source raster mapper.</param>
        /// <param name="transformation">The transformation from raster space to geometry space.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        /// <exception cref="System.ArgumentNullException">The transformation is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of columns in the transformation is not equal to 4.
        /// or
        /// The number of rows in the transformation is not equal to 4.
        /// or
        /// The transformation contains invalid values.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified transformation is not supported.</exception>
        public static RasterMapper FromMapper(RasterMapper mapper, Matrix transformation)
        {
            if (transformation == null)
                throw new ArgumentNullException("transformation", "The transformation is null.");
            if (transformation.NumberOfColumns != 4)
                throw new ArgumentException("The number of columns in the transformation is not equal to 4.", "transformation");
            if (transformation.NumberOfRows != 4)
                throw new ArgumentException("The number of rows in the transformation is not equal to 4.", "transformation");

            // check for invalid values
            if (transformation.Any(value => Double.IsNaN(value) || Double.IsInfinity(value)))
                throw new ArgumentException("The transformation contains invalid values.", "transformation");
            if (transformation[3, 0] != 0 || transformation[3, 1] != 0 || transformation[3, 2] != 0 || transformation[3, 3] != 1)
                throw new ArgumentException("The transformation contains invalid values.", "transformation");

            // check for unsupported transformation
            if (transformation[0, 0] == 0 || transformation[1, 1] == 0 ||
                transformation[2, 0] != 0 || transformation[2, 1] != 0 ||
                transformation[0, 2] != 0 || transformation[1, 2] != 0)
                throw new NotSupportedException("The specified transformation is not supported.");

            return new RasterMapper(mapper.Mode, mapper.GeometryTransformation * transformation);
        }

        /// <summary>
        /// Creates a raster mapper from another mapper.
        /// </summary>
        /// <param name="mapper">The source raster mapper.</param>
        /// <param name="translation">The translation coordinate.</param>
        /// <param name="scale">The scale vector.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        /// <exception cref="System.ArgumentException">
        /// The translation is invalid.
        /// or
        /// The scale is invalid.
        /// or
        /// The scale of the X dimension is equal to 0.
        /// or
        /// The scale of the Y dimension is equal to 0.
        /// </exception>
        public static RasterMapper FromMapper(RasterMapper mapper, Coordinate translation, CoordinateVector scale)
        {
            if (!translation.IsValid)
                throw new ArgumentException("The translation is invalid.", "translation");
            if (!scale.IsValid)
                throw new ArgumentException("The scale is invalid.", "scale");
            if (scale.X == 0)
                throw new ArgumentException("The scale of the X dimension is equal to 0.", "scale");
            if (scale.Y == 0)
                throw new ArgumentException("The scale of the Y dimension is equal to 0.", "scale");

            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = mapper.GeometryTransformation[0, 0] * scale.X;
            transformation[1, 1] = mapper.GeometryTransformation[1, 1] * scale.Y;
            transformation[2, 2] = mapper.GeometryTransformation[2, 2] * scale.Z;
            transformation[0, 3] = mapper.GeometryTransformation[0, 3] + translation.X;
            transformation[1, 3] = mapper.GeometryTransformation[1, 3] + translation.Y;
            transformation[2, 3] = mapper.GeometryTransformation[2, 3] + translation.Z;
            transformation[3, 3] = 1;

            return new RasterMapper(mapper.Mode, transformation);
        }

        /// <summary>
        /// Creates a raster mapper from another mapper.
        /// </summary>
        /// <param name="mapper">The source raster mapper.</param>
        /// <param name="translationX">The translation in X dimension.</param>
        /// <param name="translationY">The translation in Y dimension.</param>
        /// <param name="translationZ">The translation in Z dimension.</param>
        /// <param name="scaleX">The scale in X dimension.</param>
        /// <param name="scaleY">The scale in Y dimension.</param>
        /// <param name="scaleZ">The scale in Z dimension.</param>
        /// <returns>
        /// The raster mapper based on the specified transformation.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// The translation of the X dimension is not a number.
        /// or
        /// The translation of the Y dimension is not a number.
        /// or
        /// The translation of the Z dimension is not a number.
        /// or
        /// The scale of the X dimension is not a number or is equal to 0.
        /// or
        /// The scale of the Y dimension is not a number or is equal to 0.
        /// or
        /// The scale of the Z dimension is not a number.
        /// </exception>
        public static RasterMapper FromMapper(RasterMapper mapper, Double translationX, Double translationY, Double translationZ, Double scaleX, Double scaleY, Double scaleZ)
        {
            if (Double.IsNaN(translationX))
                throw new ArgumentException("The translation of the X dimension is not a number.", "translationX");
            if (Double.IsNaN(translationY))
                throw new ArgumentException("The translation of the Y dimension is not a number.", "translationY");
            if (Double.IsNaN(translationZ))
                throw new ArgumentException("The translation of the Z dimension is not a number.", "translationZ");
            if (Double.IsNaN(scaleX) || scaleX == 0)
                throw new ArgumentException("The scale of the X dimension is not a number or is equal to 0.", "scaleX");
            if (Double.IsNaN(scaleY) || scaleY == 0)
                throw new ArgumentException("The scale of the Y dimension is not a number or is equal to 0.", "scaleY");
            if (Double.IsNaN(scaleZ))
                throw new ArgumentException("The scale of the Z dimension is not a number.", "scaleZ");

            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = scaleX;
            transformation[1, 1] = scaleY;
            transformation[2, 2] = scaleZ;
            transformation[0, 3] = translationX;
            transformation[1, 3] = translationY;
            transformation[2, 3] = translationZ;
            transformation[3, 3] = 1;

            return new RasterMapper(mapper.Mode, mapper.GeometryTransformation * transformation);
        }

        /// <summary>
        /// Creates a raster mapper from the transformation.
        /// </summary>
        /// <param name="mode">The raster mapping mode.</param>
        /// <param name="transformation">The transformation from raster space to geometry space.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        /// <exception cref="System.ArgumentNullException">The transformation is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The number of columns in the transformation is not equal to 4.
        /// or
        /// The number of rows in the transformation is not equal to 4.
        /// or
        /// The transformation contains invalid values.
        /// </exception>
        /// <exception cref="System.NotSupportedException">The specified transformation is not supported.</exception>
        public static RasterMapper FromTransformation(RasterMapMode mode, Matrix transformation)
        {
            return new RasterMapper(mode, transformation);
        }

        /// <summary>
        /// Creates a raster mapper from the transformation.
        /// </summary>
        /// <param name="mode">The raster mapping mode.</param>
        /// <param name="translation">The translation coordinate.</param>
        /// <param name="scale">The scale vector.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        /// <exception cref="System.ArgumentException">
        /// The translation is invalid.
        /// or
        /// The scale is invalid.
        /// or
        /// The scale of the X dimension is equal to 0.
        /// or
        /// The scale of the Y dimension is equal to 0.
        /// </exception>
        public static RasterMapper FromTransformation(RasterMapMode mode, Coordinate translation, CoordinateVector scale)
        {
            if (!translation.IsValid)
                throw new ArgumentException("The translation is invalid.", "translation");
            if (!scale.IsValid)
                throw new ArgumentException("The scale is invalid.", "scale");
            if (scale.X == 0)
                throw new ArgumentException("The scale of the X dimension is equal to 0.", "scale");
            if (scale.Y == 0)
                throw new ArgumentException("The scale of the Y dimension is equal to 0.", "scale");

            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = scale.X;
            transformation[1, 1] = scale.Y;
            transformation[2, 2] = scale.Z;
            transformation[0, 3] = translation.X;
            transformation[1, 3] = translation.Y;
            transformation[2, 3] = translation.Z;
            transformation[3, 3] = 1;

            return new RasterMapper(mode, transformation);
        }

        /// <summary>
        /// Creates a raster mapper from the transformation.
        /// </summary>
        /// <param name="mode">The raster mapping mode.</param>
        /// <param name="translationX">The translation in X dimension.</param>
        /// <param name="translationY">The translation in Y dimension.</param>
        /// <param name="translationZ">The translation in Z dimension.</param>
        /// <param name="scaleX">The scale in X dimension.</param>
        /// <param name="scaleY">The scale in Y dimension.</param>
        /// <param name="scaleZ">The scale in Z dimension.</param>
        /// <returns>The raster mapper based on the specified transformation.</returns>
        /// <exception cref="System.ArgumentException">
        /// The translation of the X dimension is not a number.
        /// or
        /// The translation of the Y dimension is not a number.
        /// or
        /// The translation of the Z dimension is not a number.
        /// or
        /// The scale of the X dimension is not a number or is equal to 0.
        /// or
        /// The scale of the Y dimension is not a number or is equal to 0.
        /// or
        /// The scale of the Z dimension is not a number.
        /// </exception>
        public static RasterMapper FromTransformation(RasterMapMode mode, Double translationX, Double translationY, Double translationZ, Double scaleX, Double scaleY, Double scaleZ)
        {
            if (Double.IsNaN(translationX))
                throw new ArgumentException("The translation of the X dimension is not a number.", "translationX");
            if (Double.IsNaN(translationY))
                throw new ArgumentException("The translation of the Y dimension is not a number.", "translationY");
            if (Double.IsNaN(translationZ))
                throw new ArgumentException("The translation of the Z dimension is not a number.", "translationZ");
            if (Double.IsNaN(scaleX) || scaleX == 0)
                throw new ArgumentException("The scale of the X dimension is not a number or is equal to 0.", "scaleX");
            if (Double.IsNaN(scaleY) || scaleY == 0)
                throw new ArgumentException("The scale of the Y dimension is not a number or is equal to 0.", "scaleY");
            if (Double.IsNaN(scaleZ))
                throw new ArgumentException("The scale of the Z dimension is not a number.", "scaleZ");

            Matrix transformation = new Matrix(4, 4);
            transformation[0, 0] = scaleX;
            transformation[1, 1] = scaleY;
            transformation[2, 2] = scaleZ;
            transformation[0, 3] = translationX;
            transformation[1, 3] = translationY;
            transformation[2, 3] = translationZ;
            transformation[3, 3] = 1;

            return new RasterMapper(mode, transformation);
        }

        #endregion
    }
}
