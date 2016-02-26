/// <copyright file="PolygonAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Contains algorithms for computing planar polygon properties.
    /// </summary>
    public static class PolygonAlgorithms
    {
        #region Area computation

        /// <summary>
        /// Computes the area of the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Double Area(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return Math.Abs(SignedArea(polygon));
        }

        /// <summary>
        /// Computes the area of the specified polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IList<Coordinate> shell)
        {
            return Math.Abs(SignedArea(shell));
        }

        /// <summary>
        /// Computes the area of the specified polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes in reverse order to the shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            return Math.Abs(SignedArea(shell, holes));
        }

        /// <summary>
        /// Computes the signed area of the specified polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The signed area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Double SignedArea(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");
            
            return SignedArea(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates));
        }

        /// <summary>
        /// Computes the signed area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. The area is positive if the coordinates are in counterclockwise order; otherwise it is negative. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double SignedArea(IList<Coordinate> shell)
        {
            // source: http://geomalgorithms.com/a01-_area.html

            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // check for the number of coordinates
            if (shell.Count < 3)
                return 0;

            Double area = 0;
            for (Int32 i = 0; i < shell.Count - 1; i++)
            {
                // compute area
                area += shell[i].X * shell[i + 1].Y - shell[i + 1].X * shell[i].Y;
            }
            return -area / 2;
        }

        /// <summary>
        /// Computes the signed area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes in reverse order to the shell.</param>
        /// <returns>The area of the polygon. The area is positive if the coordinates of the shell are in counterclockwise order; otherwise it is negative. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double SignedArea(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            Double area = SignedArea(shell);
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    if (hole != null)
                        area += SignedArea(hole);
                }
            }
            return area;
        }

        #endregion

        #region IsConvex computation

        /// <summary>
        /// Determines whether the specified polygon is convex.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Boolean IsConvex(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            if (polygon.HoleCount > 0 && polygon.Holes.Count(hole => hole != null) > 0)
                return false;

            return IsConvex(polygon.Shell.Coordinates, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the specified polygon is convex.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Boolean IsConvex(IBasicPolygon polygon, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            if (polygon.HoleCount > 0 && polygon.Holes.Count(hole => hole != null) > 0)
                return false;

            return IsConvex(polygon.Shell.Coordinates, precision);
        }

        /// <summary>
        /// Determines whether the specified polygon is convex.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsConvex(IList<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsConvex(shell, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the specified polygon is convex.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsConvex(IList<Coordinate> shell, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            if (shell.Count < 4)
                return false;

            // source: http://blog.csharphelper.com/2010/01/04/determine-whether-a-polygon-is-convex-in-c.aspx

            AEGIS.Orientation baseOrientation = AEGIS.Orientation.Collinear;

            for (Int32 shellIndex = 2; shellIndex < shell.Count && baseOrientation == AEGIS.Orientation.Collinear; shellIndex++)
            {
                baseOrientation = Coordinate.Orientation(shell[shellIndex - 2], shell[shellIndex - 1], shell[shellIndex], precision);
            }

            if (baseOrientation == AEGIS.Orientation.Collinear)
                return false;

            for (Int32 shellIndex = 2; shellIndex < shell.Count; shellIndex++)
            {
                if (Coordinate.Orientation(shell[shellIndex - 2], shell[shellIndex - 1], shell[shellIndex], precision) != baseOrientation &&
                    Coordinate.Orientation(shell[shellIndex - 2], shell[shellIndex - 1], shell[shellIndex], precision) != AEGIS.Orientation.Collinear)
                    return false;
            }

            return true;
        }

        #endregion

        #region IsSimple computation

        /// <summary>
        /// Determines whether the specified polygon is simple.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns><c>true</c> if no edges of the shell intersect and there are no holes in the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Boolean IsSimple(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            if (polygon.HoleCount > 0 && polygon.Holes.Count(hole => hole != null) > 0)
                return false;

            return IsSimple(polygon.Shell.Coordinates);
        }

        /// <summary>
        /// Determines whether the specified polygon is simple.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precision">The precision.</param>
        /// <returns><c>true</c> if no edges of the shell intersect and there are no holes in the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Boolean IsSimple(IBasicPolygon polygon, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            if (polygon.HoleCount > 0 && polygon.Holes.Count(hole => hole != null) > 0)
                return false;

            return IsSimple(polygon.Shell.Coordinates, precision);
        }

        /// <summary>
        /// Determines whether the specified polygon is simple.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if no edges of the shell intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsSimple(IList<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsSimple(shell, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the specified polygon is simple.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precision">The precision.</param>
        /// <returns><c>true</c> if no edges of the shell intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsSimple(IList<Coordinate> shell, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            if (precision == null)
                precision = PrecisionModel.Default;

            // simple checks
            if (shell.Count == 0)
                return true;
            if (shell.Count < 4)
                return false;
            if (shell[0] != shell[shell.Count - 1])
                return false;

            // check for orientation
            if (Orientation(shell, precision) == AEGIS.Orientation.Undefined)
                return false;

            // check for edge intersections
            for (Int32 firstEdgeIndex = 0; firstEdgeIndex < shell.Count - 2; firstEdgeIndex++)
                for (Int32 secondEdgeIndex = firstEdgeIndex + 1; secondEdgeIndex < shell.Count - 1; secondEdgeIndex++)
                {
                    if (firstEdgeIndex == 0 && secondEdgeIndex == shell.Count - 2) // for the first and final edges the endpoints must match, 
                    {
                        if (LineAlgorithms.Contains(shell[0], shell[1], shell[shell.Count - 2], precision) ||
                            LineAlgorithms.Contains(shell[shell.Count - 1], shell[shell.Count - 2], shell[1], precision))
                            return false;
                    }
                    else if (secondEdgeIndex == firstEdgeIndex + 1) // for neighbor edges containment must be examined
                    {
                        if (LineAlgorithms.Contains(shell[firstEdgeIndex], shell[firstEdgeIndex + 1], shell[secondEdgeIndex + 1], precision) ||
                            LineAlgorithms.Contains(shell[secondEdgeIndex], shell[secondEdgeIndex + 1], shell[firstEdgeIndex], precision))
                            return false;
                    }
                    else // for every other edge intersection must be examined
                    {
                        if (LineAlgorithms.Intersects(shell[firstEdgeIndex], shell[firstEdgeIndex + 1], shell[secondEdgeIndex], shell[secondEdgeIndex + 1], precision))
                            return false;
                    }
                }

            return true;
        }

        #endregion

        #region IsValid computation

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return IsValid(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precision">The precision.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IBasicPolygon polygon, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return IsValid(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), precision);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IList<Coordinate> shell)
        {
            return IsValid(shell, null, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precision">The precision.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IList<Coordinate> shell, PrecisionModel precision)
        {
            return IsValid(shell, null, precision);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsValid(shell, holes, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether the specified polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // check for existence
            if (shell.Count == 0 && holes == null)
                return true;

            if (shell.Count == 0 && holes.Any(hole => (hole != null || hole.Count > 0)))
                return false;

            // check for count and closure
            if (shell.Count < 4 || !shell[0].IsValid || shell[0] != shell[shell.Count - 1])
                return false;

            // shell
            for (Int32 i = 1; i < shell.Count; i++)
            {
                if (!shell[i].IsValid || shell[i].Z != shell[0].Z) // check for validity and planarity
                    return false;
                if (shell[i - 1] == shell[i]) // check for distinct coordinates
                    return false;
            }

            IList<IList<Coordinate>> ringList = new List<IList<Coordinate>>();
            ringList.Add(shell);

            // holes
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    // check for count and closure
                    if (hole.Count < 4 || !hole[0].IsValid || hole[0] != hole[hole.Count - 1])
                        return false;

                    for (Int32 i = 1; i < hole.Count; i++)
                    {
                        if (!hole[i].IsValid || hole[i].Z != shell[0].Z) // check for validity and planarity
                            return false;
                        if (hole[i - 1] == hole[i]) // check for distinct coordinates
                            return false;
                    }

                    ringList.Add(hole);
                }
            }

            // check for any intersection
            if (ShamosHoeyAlgorithm.Intersects(ringList, precision))
                return false;

            return true;
        }

        #endregion

        #region Orientation computation

        /// <summary>
        /// Computes the orientation of a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>The orientation of the polygon shell. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Orientation Orientation(IBasicPolygon polygon)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return Orientation(polygon.Shell.Coordinates, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the orientation of a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The orientation of the polygon shell. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static Orientation Orientation(IBasicPolygon polygon, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return Orientation(polygon.Shell.Coordinates, precision);
        }

        /// <summary>
        /// Computes the orientation of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The orientation of the polygon. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Orientation Orientation(IList<Coordinate> shell)
        {
            return Orientation(shell, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the orientation of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The orientation of the polygon. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Orientation Orientation(IList<Coordinate> shell, PrecisionModel precision)
        {
            // source: http://geomalgorithms.com/a01-_area.html

            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // check for polygon properties
            if (shell.Count < 4)
                return AEGIS.Orientation.Undefined;
            if (shell[0] != shell[shell.Count - 1])
                return AEGIS.Orientation.Undefined;

            // check for planarity
            for (Int32 i = 1; i < shell.Count; i++)
                if (shell[i].Z != shell[0].Z)
                    return AEGIS.Orientation.Undefined;

            // look for collinear coordinates
            Double minX = shell.Min(coordinate => coordinate.X);
            Double minY = shell.Min(coordinate => coordinate.Y);
            Double maxX = shell.Max(coordinate => coordinate.X);
            Double maxY = shell.Max(coordinate => coordinate.Y);

            if (minX == maxX || minY == maxY)
                return AEGIS.Orientation.Collinear;

            // look for the top Y coordinate
            Int32 topIndex = 0;
            for (Int32 i = 0; i < shell.Count; i++)
            {
                if (shell[topIndex].Y < shell[i].Y)
                    topIndex = i;
            }

            // look for the last distinct coordinate before the top Y coordinate
            Int32 topPrevIndex = topIndex;
            do
            {
                topPrevIndex--;
                if (topPrevIndex < 0)
                    topPrevIndex += (shell.Count - 1);
            }
            while (topPrevIndex != topIndex && shell[topPrevIndex] == shell[topIndex] && shell[topPrevIndex] == shell[topIndex]);

            // check if it is different
            if (shell[topIndex].Equals(shell[topPrevIndex]))
                return AEGIS.Orientation.Undefined;

            // look for the first distinct coordinate after the top Y coordinate
            Int32 topNextIndex = topIndex;
            do
            {
                topNextIndex = (topNextIndex + 1) % (shell.Count - 1);
                if (topNextIndex < 0)
                    topNextIndex += shell.Count;
            }
            while (topNextIndex != topIndex && shell[topNextIndex] == shell[topIndex] && shell[topNextIndex] == shell[topIndex]);

            // check if it is different
            if (shell[topIndex] == shell[topNextIndex])
                return AEGIS.Orientation.Undefined;

            // check the orientation of the three coordinates
            Orientation coordinateOrientation = Coordinate.Orientation(shell[topPrevIndex], shell[topIndex], shell[topNextIndex], precision);

            if (coordinateOrientation == AEGIS.Orientation.Collinear)
            {
                // if the orientation is collinear the X coordinates should be checked
                coordinateOrientation = (shell[topPrevIndex].X < shell[topNextIndex].X) ? AEGIS.Orientation.CounterClockwise : AEGIS.Orientation.Clockwise;
            }

            return coordinateOrientation;
        }

        #endregion

        #region Location computation

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IList<Coordinate> shell, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return ComputeLocation(shell, null, coordinate, null);
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return ComputeLocation(shell, null, coordinate, precision);
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IBasicPolygon polygon, Coordinate coordinate)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return ComputeLocation(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), coordinate, null);
        }

        /// <summary>
        /// Computes the location of the specified coordinate with respect to a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The relative location of the coordinate with respect to the polygon.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        public static RelativeLocation Location(IBasicPolygon polygon, Coordinate coordinate, PrecisionModel precision)
        { 
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return ComputeLocation(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean OnBoundary(IList<Coordinate> shell, Coordinate coordinate)
        {
            return OnBoundary(shell, null, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean OnBoundary(IList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            return OnBoundary(shell, null, coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        public static Boolean OnBoundary(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return OnBoundary(shell, holes, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is on the boundary of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is on the boundary of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        public static Boolean OnBoundary(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            // Check whether the coordinate is on the shell of the polygon.
            for (Int32 coordIndex = 0; coordIndex < shell.Count - 1 || (coordIndex < shell.Count && shell[0] != shell[shell.Count - 1]); ++coordIndex)
                if (LineAlgorithms.Contains(shell[coordIndex], shell[(coordIndex + 1) % shell.Count], coordinate, precision))
                    return true;

            // Check whether the coordinate is on the shell of any holes in the polygon.
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    for (Int32 coordIndex = 0; coordIndex < hole.Count - 1 || (coordIndex < hole.Count && hole[0] != hole[hole.Count - 1]); ++coordIndex)
                        if (LineAlgorithms.Contains(hole[coordIndex], hole[(coordIndex + 1) % hole.Count], coordinate, precision))
                            return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes whether the given coordinate is in the interior of the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is in the interior of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IBasicPolygon polygon, Coordinate coordinate)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");
            
            return InInterior(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is in the interior of the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is in the interior of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IBasicPolygon polygon, Coordinate coordinate, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");

            return InInterior(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is in the interior of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is in the interior of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IBasicLineString shell, IList<IBasicLineString> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            return InInterior(shell.Coordinates, holes == null ? null : holes.Select(hole => hole.Coordinates), coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is in the interior of the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is in the interior of the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IBasicLineString shell, IList<IBasicLineString> holes, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            return InInterior(shell.Coordinates, holes == null ? null : holes.Select(hole => hole.Coordinates), coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IList<Coordinate> shell, Coordinate coordinate)
        {
            return InInterior(shell, null, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            return InInterior(shell, null, coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            return ComputeLocation(shell, holes, coordinate, PrecisionModel.Default) == RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is inside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is inside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InInterior(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            return ComputeLocation(shell, holes, coordinate, precision) == RelativeLocation.Interior;
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IBasicPolygon polygon, Coordinate coordinate)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");
            

            return InExterior(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The polygon is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IBasicPolygon polygon, Coordinate coordinate, PrecisionModel precision)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "The polygon is null.");


            return InExterior(polygon.Shell.Coordinates, polygon.Holes.Select(hole => hole.Coordinates), coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IList<Coordinate> shell, Coordinate coordinate)
        {
            return InExterior(shell, null, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IList<Coordinate> shell, Coordinate coordinate, PrecisionModel precision)
        {
            return InExterior(shell, null, coordinate, precision);
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            return ComputeLocation(shell, holes, coordinate, PrecisionModel.Default) == RelativeLocation.Exterior;
        }

        /// <summary>
        /// Computes whether the given coordinate is outside the polygon.
        /// </summary>
        /// <param name="shell">The polygon shell.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate to check.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the coordinate is outside the polygon; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">One or more holes are null.</exception>
        /// <remarks>
        /// Positions on the boundary of the polygon shell (or its holes) are neither inside or outside the polygon.
        /// </remarks>
        public static Boolean InExterior(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precision)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (holes != null && holes.Any(hole => hole == null))
                throw new ArgumentException("One or more holes are null.", "holes");

            return ComputeLocation(shell, holes, coordinate, precision) == RelativeLocation.Exterior;
        }

        #endregion

        #region Private location computation methods

        /// <summary>
        /// Computes the location of the specified coordinate within a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The examined coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The location of the coordinate.</returns>
        private static RelativeLocation ComputeLocation(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precision) 
        {
            if (!coordinate.IsValid)
                return RelativeLocation.Undefined;

            if (shell.Count < 3 || shell.Any(coord => !coord.IsValid))
                return RelativeLocation.Undefined;

            if (!Envelope.FromCoordinates(shell).Contains(coordinate))
                return RelativeLocation.Exterior;

            WindingNumberAlgorithm algorithm = new WindingNumberAlgorithm(shell, coordinate, true, precision);
            algorithm.Compute();

            // Probably the Winding Number algorithm already detected that the coordinate is on the boundary of the polygon shell.
            if (algorithm.IsOnBoundary.Value)
                return RelativeLocation.Boundary;

            // The value of notExterior is true if the coordinate is not outside the polygon - but it can also be on the boundary.
            if (algorithm.Result == 0)
                return RelativeLocation.Exterior;

            // If the coordinate is not outside, the holes are also required to be checked.
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    if (hole.Count < 3 || hole.Any(coord => !coord.IsValid))
                        return RelativeLocation.Undefined;

                    algorithm = new WindingNumberAlgorithm(hole, coordinate, true, precision);
                    algorithm.Compute();

                    // Probably the Winding Number algorithm already detected that the coordinate is on the boundary of a hole shell.
                    if (algorithm.IsOnBoundary.Value)
                        return RelativeLocation.Boundary;

                    // If the coordinate is inside a hole (or probably on its boundary), then it cannot be inside the polygon.
                    if (algorithm.Result != 0)
                        return RelativeLocation.Exterior;
                }
            }

            return RelativeLocation.Interior;
        }

        #endregion
    }
}
