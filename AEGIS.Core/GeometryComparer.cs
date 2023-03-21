// <copyright file="GeometryComparer.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a comparer for <see cref="IGeometry" /> instances.
    /// </summary>
    public class GeometryComparer : IComparer<IBasicGeometry>, IComparer<IBasicPoint>, IComparer<IBasicLineString>, IComparer<IBasicPolygon>, IComparer<IEnumerable<IBasicGeometry>>,
                                    IComparer<IGeometry>, IComparer<IPoint>, IComparer<ILineString>, IComparer<IPolygon>, IComparer<IEnumerable<IGeometry>>
    {
        #region Private static fields

        /// <summary>
        /// The order of geometry types for basic geometries.
        /// </summary>
        private static readonly Type[] _basicGeometryOrder;

        /// <summary>
        /// The order of geometry type for geometries.
        /// </summary>
        private static readonly Type[] _geometryOrder;


        #endregion

        #region Private fields

        private CoordinateComparer _comparer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="GeometryComparer" /> class.
        /// </summary>
        static GeometryComparer()
        {
            _geometryOrder = new Type[] { typeof(IPoint), typeof(IMultiPoint), typeof(ILineString), typeof(ILinearRing), typeof(IMultiLineString), typeof(IPolygon), typeof(IMultiPolygon), typeof(IGeometryCollection<IGeometry>) };
            _basicGeometryOrder = new Type[] { typeof(IBasicPoint), typeof(IBasicLineString), typeof(IBasicPolygon) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryComparer" /> class.
        /// </summary>
        public GeometryComparer() { _comparer = new CoordinateComparer(); }

        #endregion

        #region Basic geometry comparison

        /// <summary>
        /// Compares two <see cref="IBasicGeometry" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IBasicGeometry" /> to compare.</param>
        /// <param name="y">The second <see cref="IBasicGeometry" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Comparison of the specified geometries is not supported.</exception>
        public Int32 Compare(IBasicGeometry x, IBasicGeometry y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            if ((x is IGeometry) && (y is IGeometry))
                return Compare(x as IGeometry, y as IGeometry);

            // in case the classes are different, the order is specified by the following array

            Int32 xIndex = x.GetType().GetInterfaces().Max(interfaceType => Array.IndexOf(_basicGeometryOrder, interfaceType));
            Int32 yIndex = y.GetType().GetInterfaces().Max(interfaceType => Array.IndexOf(_basicGeometryOrder, interfaceType));

            if (xIndex < yIndex)
                return -1;
            if (xIndex > yIndex)
                return -1;

            switch (xIndex)
            {
                case 0:
                    return Compare(x as IBasicPoint, y as IBasicPoint);
                case 1:
                    // linearring should be compared as linestring
                    return Compare(x as IBasicLineString, y as IBasicLineString);
                case 2:
                    return Compare(x as IBasicPolygon, y as IBasicPolygon);
            }

            throw new NotSupportedException("Comparison of the specified geometries is not supported.");
        }

        /// <summary>
        /// Compares two <see cref="IBasicPoint" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IBasicPoint" /> to compare.</param>
        /// <param name="y">The second <see cref="IBasicPoint" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IBasicPoint x, IBasicPoint y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            return _comparer.Compare(x.Coordinate, y.Coordinate);
        }

        /// <summary>
        /// Compares two <see cref="IBasicLineString" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IBasicLineString" /> to compare.</param>
        /// <param name="y">The second <see cref="IBasicLineString" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IBasicLineString x, IBasicLineString y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            Int32 index = 0;

            // look for the first different coordinate in the linestring
            while (index < x.Count && index < y.Count)
            {
                Int32 comparison = _comparer.Compare(x.GetCoordinate(index), y.GetCoordinate(index));
                if (comparison != 0)
                    return comparison;
                index++;
            }

            // check whether there are additional coordinates in either linestring
            if (index < x.Count)
                return 1;
            if (index < y.Count)
                return -1;
            return 0;
        }

        /// <summary>
        /// Compares two <see cref="IBasicPolygon" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IBasicPolygon" /> to compare.</param>
        /// <param name="y">The second <see cref="IBasicPolygon" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IBasicPolygon x, IBasicPolygon y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            // check the shell
            Int32 shellCompare = Compare(x.Shell, y.Shell);
            if (shellCompare != 0)
                return shellCompare;

            Int32 index = 0;

            // look for the first different hole in the polygon
            while (index < x.HoleCount && index < y.HoleCount)
            {
                Int32 holeCompare = Compare(x.GetHole(index), y.GetHole(index));
                if (holeCompare != 0)
                    return holeCompare;
                index++;
            }

            // check whether there are additional holes in either polygon
            if (index < x.HoleCount)
                return 1;
            if (index < y.HoleCount)
                return -1;
            return 0;
        }


        /// <summary>
        /// Compares two <see cref="IEnumerable{IBasicGeometry}" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IEnumerable{IBasicGeometry}" /> to compare.</param>
        /// <param name="y">The second <see cref="IEnumerable{IBasicGeometry}" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IEnumerable<IBasicGeometry> x, IEnumerable<IBasicGeometry> y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            IEnumerator<IBasicGeometry> enumX = x.GetEnumerator();
            IEnumerator<IBasicGeometry> enumY = y.GetEnumerator();

            // look for the first different element in the collection
            while (enumX.MoveNext() && enumY.MoveNext())
            {
                Int32 comparison = Compare(enumX.Current, enumY.Current);
                if (comparison != 0)
                    return comparison;
            }

            // check whether there are additional elements in either collection
            if (enumX.MoveNext())
                return 1;
            if (enumY.MoveNext())
                return -1;
            return 0;
        }


        #endregion

        #region Geometry comparison

        /// <summary>
        /// Compares two <see cref="IGeometry" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IGeometry" /> to compare.</param>
        /// <param name="y">The second <see cref="IGeometry" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Comparison of the specified geometries is not supported.</exception>
        public Int32 Compare(IGeometry x, IGeometry y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            // in case the classes are different, the order is specified by the following array

            Int32 xIndex = x.GetType().GetInterfaces().Max(interfaceType => Array.IndexOf(_geometryOrder, interfaceType));
            Int32 yIndex = y.GetType().GetInterfaces().Max(interfaceType => Array.IndexOf(_geometryOrder, interfaceType));

            if (xIndex < yIndex)
                return -1;
            if (xIndex > yIndex)
                return -1;

            switch (xIndex)
            { 
                case 0:
                    return Compare(x as IPoint, y as IPoint);
                case 2:
                case 3: 
                    // linearring should be compared as linestring
                    return Compare(x as ILineString, y as ILineString);
                case 5:
                    return Compare(x as IPolygon, y as IPolygon);
                case 1:
                case 4:
                case 6:
                case 7:
                    // all collection types should be compared in the same manner
                    return Compare(x as IEnumerable<IGeometry>, y as IEnumerable<IGeometry>);
            }

            throw new NotSupportedException("Comparison of the specified geometries is not supported.");
        }

        /// <summary>
        /// Compares two <see cref="IPoint" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IPoint" /> to compare.</param>
        /// <param name="y">The second <see cref="IPoint" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IPoint x, IPoint y)
        {
            return Compare(x as IBasicPoint, y as IBasicPoint);
        }

        /// <summary>
        /// Compares two <see cref="ILineString" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="ILineString" /> to compare.</param>
        /// <param name="y">The second <see cref="ILineString" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(ILineString x, ILineString y)
        {
            return Compare(x as IBasicLineString, y as IBasicLineString);
        }

        /// <summary>
        /// Compares two <see cref="IPolygon" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IPolygon" /> to compare.</param>
        /// <param name="y">The second <see cref="IPolygon" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IPolygon x, IPolygon y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            // check the shell
            Int32 shellCompare = Compare(x.Shell, y.Shell);
            if (shellCompare != 0)
                return shellCompare;

            Int32 index = 0;

            // look for the first different hole in the polygon
            while (index < x.HoleCount && index < y.HoleCount)
            {
                Int32 holeCompare = Compare(x.GetHole(index), y.GetHole(index));
                if (holeCompare != 0)
                    return holeCompare;
                index++;
            }

            // check whether there are additional holes in either polygon
            if (index < x.HoleCount)
                return 1;
            if (index < y.HoleCount)
                return -1;
            return 0;
        }

        /// <summary>
        /// Compares two <see cref="IEnumerable{IGeometry}" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="IEnumerable{IGeometry}" /> to compare.</param>
        /// <param name="y">The second <see cref="IEnumerable{IGeometry}" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The x argument is null.
        /// or
        /// The y argument is null.
        /// </exception>
        public Int32 Compare(IEnumerable<IGeometry> x, IEnumerable<IGeometry> y)
        {
            if (x == null)
                throw new ArgumentNullException("x", "The x argument is null.");
            if (y == null)
                throw new ArgumentNullException("y", "The y argument is null.");
            if (x == y)
                return 0;

            IEnumerator<IGeometry> enumX = x.GetEnumerator();
            IEnumerator<IGeometry> enumY = y.GetEnumerator();

            // look for the first different element in the collection
            while (enumX.MoveNext() && enumY.MoveNext())
            {
                Int32 comparison = Compare(enumX.Current, enumY.Current);
                if (comparison != 0)
                    return comparison;
            }

            // check whether there are additional elements in either collection
            if (enumX.MoveNext())
                return 1;            
            if (enumY.MoveNext())
                return -1;            
            return 0;
        }

        #endregion
    }
}
