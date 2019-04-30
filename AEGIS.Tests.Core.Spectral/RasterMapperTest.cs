/// <copyright file="RasterMapperTest.cs" company="Eötvös Loránd University (ELTE)">
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
using NUnit.Framework;
using System;

namespace ELTE.AEGIS.Tests
{
    /// <summary>
    /// Test fixture for the <see cref="RasterMapper"/> class.
    /// </summary>
    [TestFixture]
    public class RasterMapperTest
    {
        #region Test methods

        /// <summary>
        /// Test case for the constructor.
        /// </summary>
        [Test]
        public void RasterMapperConstructorTest()
        {
            RasterMapper mapper = new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));

            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(MatrixFactory.CreateIdentity(4), mapper.RasterTransformation);
            Assert.AreEqual(MatrixFactory.CreateIdentity(4), mapper.GeometryTransformation);
            Assert.AreEqual(1, mapper.ColumnSize);
            Assert.AreEqual(1, mapper.RowSize);
            Assert.AreEqual(new CoordinateVector(1, 0), mapper.ColumnVector);
            Assert.AreEqual(new CoordinateVector(0, 1), mapper.RowVector);
            Assert.AreEqual(new CoordinateVector(1, 1, 1), mapper.Scale);
            Assert.AreEqual(Coordinate.Empty, mapper.Translation);

            // exceptions

            Assert.Throws<ArgumentNullException>(() => new RasterMapper(RasterMapMode.ValueIsArea, null));
            Assert.Throws<ArgumentException>(() => new RasterMapper(RasterMapMode.ValueIsArea, new Matrix(1, 4)));
            Assert.Throws<ArgumentException>(() => new RasterMapper(RasterMapMode.ValueIsArea, new Matrix(4, 1)));
            Assert.Throws<ArgumentException>(() => new RasterMapper(RasterMapMode.ValueIsArea, new Matrix(4, 4, Double.NaN)));
            Assert.Throws<ArgumentException>(() => new RasterMapper(RasterMapMode.ValueIsArea, new Matrix(4, 4)));
            Assert.Throws<ArgumentException>(() =>
                {
                    Matrix matrix = new Matrix(4, 4);
                    matrix[3, 0] = matrix[3, 1] = matrix[3, 2] = matrix[3, 3] = 1;
                    new RasterMapper(RasterMapMode.ValueIsArea, matrix);
                });
            Assert.Throws<NotSupportedException>(() => new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateDiagonal(0, 0, 0, 1)));
        }

        /// <summary>
        /// Test case for the <see cref="FromTransformation"/> method.
        /// </summary>
        [Test]
        public void RasterMapperFromTransformationTest()
        {
            // matrix

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));

            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(MatrixFactory.CreateIdentity(4), mapper.GeometryTransformation);


            // coordinate and vector

            mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, new Coordinate(300, 1000, 0), new CoordinateVector(10, 3));

            Assert.AreEqual(new Coordinate(300, 1000), mapper.Translation);
            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(10, mapper.ColumnSize);
            Assert.AreEqual(3, mapper.RowSize);
            Assert.AreEqual(new CoordinateVector(10, 0), mapper.ColumnVector);
            Assert.AreEqual(new CoordinateVector(0, 3), mapper.RowVector);


            // all values

            mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 300, 1000, 0, 10, 3, 0);

            Assert.AreEqual(new Coordinate(300, 1000), mapper.Translation);
            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(10, mapper.ColumnSize);
            Assert.AreEqual(3, mapper.RowSize);
            Assert.AreEqual(new CoordinateVector(10, 0), mapper.ColumnVector);
            Assert.AreEqual(new CoordinateVector(0, 3), mapper.RowVector);


            // exceptions

            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, Coordinate.Undefined, new CoordinateVector(0, 1, 0)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, Coordinate.Empty, new CoordinateVector(Double.NaN, Double.NaN, Double.NaN)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, Coordinate.Empty, new CoordinateVector(0, 1, 0)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, Coordinate.Empty, new CoordinateVector(1, 0, 0)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, Double.NaN, 0, 0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, Double.NaN, 0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, 0, Double.NaN, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, 0, 0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, 0, 0, 0, 1, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, 0, 0, Double.NaN, 1, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, 0, 0, 1, Double.NaN, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 0, 0, 0, 1, 1, Double.NaN));
        }

        /// <summary>
        /// Test case for the <see cref="FromMapper"/> method.
        /// </summary>
        [Test]
        public void RasterMapperFromMapperTest()
        {
            // matrix from identity

            RasterMapper sourceMapper = new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));

            RasterMapper mapper = RasterMapper.FromMapper(sourceMapper, MatrixFactory.CreateIdentity(4));

            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(MatrixFactory.CreateIdentity(4), mapper.GeometryTransformation);


            // coordinate and vector from identity

            mapper = RasterMapper.FromMapper(sourceMapper, new Coordinate(300, 1000, 0), new CoordinateVector(10, 3));

            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(new CoordinateVector(10, 0), mapper.ColumnVector);
            Assert.AreEqual(new CoordinateVector(0, 3), mapper.RowVector);
            Assert.AreEqual(new Coordinate(300, 1000), mapper.Translation);


            // all values from identity

            mapper = RasterMapper.FromMapper(sourceMapper, 300, 1000, 0, 10, 3, 0);

            Assert.AreEqual(RasterMapMode.ValueIsArea, mapper.Mode);
            Assert.AreEqual(new CoordinateVector(10, 0), mapper.ColumnVector);
            Assert.AreEqual(new CoordinateVector(0, 3), mapper.RowVector);
            Assert.AreEqual(new Coordinate(300, 1000), mapper.Translation);


            // exceptions

            Assert.Throws<ArgumentNullException>(() => RasterMapper.FromMapper(mapper, null));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, new Matrix(1, 4)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, new Matrix(4, 1)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, new Matrix(4, 4, Double.NaN)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, new Matrix(4, 4)));
            Assert.Throws<ArgumentException>(() =>
            {
                Matrix matrix = new Matrix(4, 4);
                matrix[3, 0] = matrix[3, 1] = matrix[3, 2] = matrix[3, 3] = 1;
                RasterMapper.FromMapper(mapper, matrix);
            });
            Assert.Throws<NotSupportedException>(() => RasterMapper.FromMapper(mapper, MatrixFactory.CreateDiagonal(0, 0, 0, 1)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, Coordinate.Undefined, new CoordinateVector(0, 1, 0)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, Coordinate.Empty, new CoordinateVector(Double.NaN, Double.NaN, Double.NaN)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, Coordinate.Empty, new CoordinateVector(0, 1, 0)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, Coordinate.Empty, new CoordinateVector(1, 0, 0)));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, Double.NaN, 0, 0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, Double.NaN, 0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, 0, Double.NaN, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, 0, 0, 1, 0, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, 0, 0, 0, 1, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, 0, 0, Double.NaN, 1, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, 0, 0, 1, Double.NaN, 0));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromMapper(mapper, 0, 0, 0, 1, 1, Double.NaN));
        }

        /// <summary>
        /// Test case for the <see cref="FromCoordinates"/> method.
        /// </summary>
        [Test]
        public void RasterMapperFromCoordinatesTest()
        {
            RasterMapper sourceMapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, new Coordinate(300, 1000, 0), new CoordinateVector(10, 3));

            RasterMapper mapper = RasterMapper.FromCoordinates(RasterMapMode.ValueIsCoordinate, 
                new RasterCoordinate(0, 0, sourceMapper.MapCoordinate(0, 0)),
                new RasterCoordinate(100, 0, sourceMapper.MapCoordinate(100, 0)),
                new RasterCoordinate(100, 100, sourceMapper.MapCoordinate(100, 100)),
                new RasterCoordinate(0, 100, sourceMapper.MapCoordinate(0, 100))
            );

            Assert.AreEqual(RasterMapMode.ValueIsCoordinate, mapper.Mode);
            Assert.AreEqual(sourceMapper.ColumnVector, mapper.ColumnVector);
            Assert.AreEqual(sourceMapper.RowVector, mapper.RowVector);
            Assert.AreEqual(sourceMapper.Translation.X, mapper.Translation.X, 0.000001);
            Assert.AreEqual(sourceMapper.Translation.Y, mapper.Translation.Y, 0.000001);
            Assert.AreEqual(sourceMapper.Translation.Z, mapper.Translation.Z, 0.000001);

            // exceptions

            Assert.Throws<ArgumentNullException>(() => RasterMapper.FromCoordinates(RasterMapMode.ValueIsCoordinate, null));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromCoordinates(RasterMapMode.ValueIsCoordinate, new RasterCoordinate(0, 0, sourceMapper.MapCoordinate(0, 0)), new RasterCoordinate(0, 100, sourceMapper.MapCoordinate(0, 100))));
            Assert.Throws<ArgumentException>(() => RasterMapper.FromCoordinates(RasterMapMode.ValueIsCoordinate, new RasterCoordinate(0, 0, sourceMapper.MapCoordinate(0, 0)), new RasterCoordinate(100, 0, sourceMapper.MapCoordinate(100, 0))));
        }

        /// <summary>
        /// Test case for the <see cref="MapCoordinate"/> method.
        /// </summary>
        [Test]
        public void RasterMapperMapCoordinateTest()
        {
            // value is coordinate

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 300, 1000, 0, 10, 3, 0);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Coordinate coordinate = mapper.MapCoordinate(j, i);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);

                    coordinate = mapper.MapCoordinate((Double)j, (Double)i);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);

                    coordinate = mapper.MapCoordinate(j, i, RasterMapMode.ValueIsCoordinate);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);

                    coordinate = mapper.MapCoordinate(j, i, RasterMapMode.ValueIsArea);

                    Assert.AreEqual(295 + i * 10, coordinate.X);
                    Assert.AreEqual(998.5 + j * 3, coordinate.Y);
                }


            // value is area

            mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 300, 1000, 0, 10, 3, 1);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Coordinate coordinate = mapper.MapCoordinate(j, i);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);

                    coordinate = mapper.MapCoordinate((Double)j, (Double)i);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);

                    coordinate = mapper.MapCoordinate(j, i, RasterMapMode.ValueIsCoordinate);

                    Assert.AreEqual(305 + i * 10, coordinate.X);
                    Assert.AreEqual(1001.5 + j * 3, coordinate.Y);

                    coordinate = mapper.MapCoordinate(j, i, RasterMapMode.ValueIsArea);

                    Assert.AreEqual(300 + i * 10, coordinate.X);
                    Assert.AreEqual(1000 + j * 3, coordinate.Y);
                }
        }

        /// <summary>
        /// Test case for the <see cref="MapRaster"/> method.
        /// </summary>
        [Test]
        public void RasterMapperMapRasterTest()
        {
            // value is coordinate

            RasterMapper mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsCoordinate, 300, 1000, 0, 10, 3, 1);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Int32 rowIndex, columnIndex;
                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), out rowIndex, out columnIndex);

                    Assert.AreEqual(i, columnIndex);
                    Assert.AreEqual(j, rowIndex);

                    Double floatRowIndex, floatColumnIndex;

                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), out floatRowIndex, out floatColumnIndex);

                    Assert.AreEqual(i, floatColumnIndex);
                    Assert.AreEqual(j, floatRowIndex);

                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), RasterMapMode.ValueIsCoordinate, out floatRowIndex, out floatColumnIndex);

                    Assert.AreEqual(i, floatColumnIndex);
                    Assert.AreEqual(j, floatRowIndex);

                    mapper.MapRaster(new Coordinate(295 + i * 10, 998.5 + j * 3), RasterMapMode.ValueIsArea, out floatRowIndex, out floatColumnIndex);

                    Assert.AreEqual(i, floatColumnIndex);
                    Assert.AreEqual(j, floatRowIndex);
                }


            // value is area

            mapper = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, 300, 1000, 0, 10, 3, 1);

            for (Int32 i = -100; i < 100; i += 3)
                for (Int32 j = -100; j < 100; j += 3)
                {
                    Int32 rowIndex, columnIndex;
                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), out rowIndex, out columnIndex);

                    Assert.AreEqual(i, columnIndex);
                    Assert.AreEqual(j, rowIndex);

                    Double floatRowIndex, floatColumnIndex;

                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), out floatRowIndex, out floatColumnIndex);

                    Assert.AreEqual(i, floatColumnIndex);
                    Assert.AreEqual(j, floatRowIndex);

                    mapper.MapRaster(new Coordinate(305 + i * 10, 1001.5 + j * 3), RasterMapMode.ValueIsCoordinate, out floatRowIndex, out floatColumnIndex);

                    Assert.AreEqual(i, floatColumnIndex);
                    Assert.AreEqual(j, floatRowIndex);

                    mapper.MapRaster(new Coordinate(300 + i * 10, 1000 + j * 3), RasterMapMode.ValueIsArea, out floatRowIndex, out floatColumnIndex);

                    Assert.AreEqual(i, floatColumnIndex);
                    Assert.AreEqual(j, floatRowIndex);
                }
        }

        /// <summary>
        /// Test case for the <see cref="Equals"/> method.
        /// </summary>
        [Test]
        public void RasterMapperEqualsTest()
        {
            RasterMapper mapper1 = new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));
            RasterMapper mapper2 = new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));

            Assert.IsTrue(mapper1.Equals(mapper1));
            Assert.IsFalse(mapper1.Equals(null));
            Assert.IsTrue(mapper1.Equals(mapper2));
            Assert.IsTrue(mapper1.Equals((Object)mapper1));
            Assert.IsFalse(mapper1.Equals((Object)null));
            Assert.IsTrue(mapper1.Equals((Object)mapper2));

            mapper2 = new RasterMapper(RasterMapMode.ValueIsCoordinate, MatrixFactory.CreateIdentity(4));

            Assert.IsFalse(mapper1.Equals(mapper2));
            Assert.IsFalse(mapper1.Equals((Object)mapper2));

            mapper2 = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, new Coordinate(300, 1000, 0), new CoordinateVector(10, 3));

            Assert.IsFalse(mapper1.Equals(mapper2));
            Assert.IsFalse(mapper1.Equals((Object)mapper2));


        }

        /// <summary>
        /// Test case for the <see cref="GetHashCode"/> method.
        /// </summary>
        [Test]
        public void RasterMapperGetHashCodeTest()
        {
            RasterMapper mapper1 = new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));
            RasterMapper mapper2 = new RasterMapper(RasterMapMode.ValueIsArea, MatrixFactory.CreateIdentity(4));

            Assert.AreEqual(mapper1.GetHashCode(), mapper2.GetHashCode());

            mapper2 = new RasterMapper(RasterMapMode.ValueIsCoordinate, MatrixFactory.CreateIdentity(4));

            Assert.AreNotEqual(mapper1.GetHashCode(), mapper2.GetHashCode());

            mapper2 = RasterMapper.FromTransformation(RasterMapMode.ValueIsArea, new Coordinate(300, 1000, 0), new CoordinateVector(10, 3));

            Assert.AreNotEqual(mapper1.GetHashCode(), mapper2.GetHashCode());
        }

        #endregion
    }
}
