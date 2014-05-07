﻿/// <copyright file="PolygonAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
        #region IsSimple computation

        /// <summary>
        /// Determines whether a polygon is simple.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if no edges of the shell intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsSimple(IList<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // simple checks
            if (shell.Count == 0)
                return true;
            if (shell.Count < 3)
                return false;
            if (!shell[0].Equals(shell[shell.Count - 1]))
                return false;

            // check for orientation
            if (Orientation(shell) != AEGIS.Orientation.Undefined)
                return false;

            // check for edge intersections
            for (Int32 i = 0; i < shell.Count - 2; i++)
                for (Int32 j = i + 1; j < shell.Count - 1; j++)
                {
                    if (i == 0 && j == shell.Count - 2) // for the first and final edges the endpoints must match, 
                    {
                        if (LineAlgorithms.Contains(shell[0], shell[1], shell[shell.Count - 2]) ||
                            LineAlgorithms.Contains(shell[shell.Count - 1], shell[shell.Count - 2], shell[1]))
                            return false;
                    }
                    else if (j == i + 1) // for neighbour edges containement must be examined
                    {
                        if (LineAlgorithms.Contains(shell[i], shell[i + 1], shell[j + 1]) ||
                            LineAlgorithms.Contains(shell[j], shell[j + 1], shell[i]))
                            return false;
                    }
                    else // for every other edge intersection must be examined
                    {
                        if (LineAlgorithms.Intersects(shell[i], shell[i + 1], shell[j], shell[j + 1]))
                            return false;
                    }
                }

            return true;
        }

        #endregion

        #region IsConvex computation

        /// <summary>
        /// Determines whether a polygon is convex.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns><c>true</c> if the polygon is convex; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsConvex(IList<Coordinate> shell)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            if (shell.Count < 3)
                return false;

            // source: http://blog.csharphelper.com/2010/01/04/determine-whether-a-polygon-is-convex-in-c.aspx

            AEGIS.Orientation initialOrientation = Coordinate.Orientation(shell[shell.Count - 2], shell[0], shell[1]);

            for (Int32 i = 2; i < shell.Count; i++)
            {
                if (Coordinate.Orientation(shell[i - 2], shell[i - 1], shell[i]) != initialOrientation)
                    return false;
            }
            return true;
        }

        #endregion

        #region IsValid computation

        /// <summary>
        /// Determines whether a polygon is valid.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes.</param>
        /// <returns><c>true</c> if the polygon is valid; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean IsValid(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // check for existence
            if (shell.Count == 0 && holes == null)
                return true;
            if (shell.Count == 0 && holes.Any(hole => (hole != null || hole.Count > 0)))
                return false;

            // check for count and closure
            if (shell.Count < 4 || !shell[0].IsValid || !shell[0].Equals(shell[shell.Count - 1]))
                return false;

            // shell
            Double zValue = shell[0].Z;
            for (Int32 i = 1; i < shell.Count; i++)
            {
                if (!shell[i].IsValid || shell[i].Z != zValue) // check for validity and planarity
                    return false;
                if (shell[i - 1].Equals(shell[i])) // check for distinct coordinates
                    return false;
            }

            // holes
            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    // check for count and closure
                    if (hole.Count < 4 || !hole[0].IsValid || !hole[0].Equals(hole[hole.Count - 1]))
                        return false;

                    for (Int32 i = 1; i < hole.Count; i++)
                    {
                        if (!hole[i].IsValid || hole[i].Z != zValue) // check for validity and planarity
                            return false;
                        if (hole[i - 1].Equals(hole[i])) // check for distinct coordinates
                            return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region Area computation

        /// <summary>
        /// Computes the area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IEnumerable<Coordinate> shell)
        {
            return Math.Abs(SignedArea(shell));
        }

        /// <summary>
        /// Computes the area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IList<Coordinate> shell)
        {
            return Math.Abs(SignedArea(shell));
        }

        /// <summary>
        /// Computes the area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="holes">The coordinates of the polygon holes in reverse order to the shell.</param>
        /// <returns>The area of the polygon. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double Area(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            return Math.Abs(SignedArea(shell, holes));
        }

        /// <summary>
        /// Computes the area of a polygon.
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
        /// Computes the signed area of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The area of the polygon. The area is positive if the coordinates are in counterclockwise order; otherwise it is negative. If the polygon is not valid, <c>NaN</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Double SignedArea(IEnumerable<Coordinate> shell)
        {
            // source: http://geomalgorithms.com/a01-_area.html

            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            Double area = 0;
            IEnumerator<Coordinate> enumerator = shell.GetEnumerator();
            if (!enumerator.MoveNext())
                return Double.NaN;

            Coordinate first = enumerator.Current, second;
            Double zValue = first.Z;
            Int32 count = 1;

            // check for the validity of the coordinate
            if (!first.IsValid)
                return Double.NaN;

            while (enumerator.MoveNext())
            {
                // check for the validity of the coordinates
                second = enumerator.Current;
                if (second.Z != zValue || !second.IsValid)
                    return Double.NaN;

                // compute area
                area += first.X * second.Y - second.X * first.Y;
                count++;

                first = second;
            }

            // check for the number of coordinates
            if (count < 3)
                return Double.NaN;

            return -area / 2;
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

            Double zValue = shell[0].Z;
            if (shell[0].IsValid)
                return Double.NaN;

            Double area = 0;
            for (Int32 i = 0; i < shell.Count - 1; i++)
            {
                // check for the validity of the coordinates
                if (shell[i].Z != zValue || !shell[i].IsValid)
                    return Double.NaN;

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
        public static Double SignedArea(IEnumerable<Coordinate> shell, IEnumerable<IEnumerable<Coordinate>> holes)
        {
            Double area = SignedArea(shell);
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (hole != null)
                        area += SignedArea(hole);
                }
            }
            return area;
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
            Double area = SignedArea(shell);
            if (holes != null)
            {
                foreach (IEnumerable<Coordinate> hole in holes)
                {
                    if (hole != null)
                        area += SignedArea(hole);
                }
            }
            return area;
        }

        #endregion

        #region Orientation computation

        /// <summary>
        /// Computes the orientation of a polygon.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <returns>The orientation of the polygon. If the polygon is invalid <c>Undefined</c> is returned.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Orientation Orientation(IList<Coordinate> shell)
        {
            // source: http://geomalgorithms.com/a01-_area.html

            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            // check for polygon properties
            if (shell.Count < 3)
                return AEGIS.Orientation.Undefined;
            if (!shell[0].Equals(shell[shell.Count - 1]))
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
            while (topPrevIndex != topIndex && shell[topPrevIndex].Equals(shell[topIndex]) && shell[topPrevIndex].Equals(shell[topIndex]));

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
            while (topNextIndex != topIndex && shell[topNextIndex].Equals(shell[topIndex]) && shell[topNextIndex].Equals(shell[topIndex]));

            // heck if it is different
            if (shell[topIndex].Equals(shell[topNextIndex]))
                return AEGIS.Orientation.Undefined;

            // check the orientation of the three coordinates
            Orientation coordinateOrientation = Coordinate.Orientation(shell[topPrevIndex], shell[topIndex], shell[topNextIndex]);

            if (coordinateOrientation == AEGIS.Orientation.Collinear)
            {
                // if the orientation is collinear the X coordinates should be checked
                coordinateOrientation = (shell[topPrevIndex].X < shell[topNextIndex].X) ? AEGIS.Orientation.CounterClockwise : AEGIS.Orientation.Clockwise;
            }

            return coordinateOrientation;
        }

        #endregion

        #region Intersects computation

        /// <summary>
        /// Determines whether a polygon and a line intersect.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="lineStart">The starting coordinates of the line.</param>
        /// <param name="lineEnd">The ending coordinates of the line.</param>
        /// <returns><c>true</c> if the polygon and the line intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static Boolean Intersects(IList<Coordinate> shell, Coordinate lineStart, Coordinate lineEnd)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            IList<Coordinate> intersection = null;
            return Intersection(shell, lineStart, lineEnd, out intersection);
        }

        /// <summary>
        /// Determines whether two polygons intersect.
        /// </summary>
        /// <param name="firstShell">The first shell.</param>
        /// <param name="secondShell">The second shell.</param>
        /// <returns><c>true</c> if the two polygons intersect; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The first shell is null.
        /// or
        /// The second shell is null.
        /// </exception>
        public static Boolean Intersects(IList<Coordinate> firstShell, IList<Coordinate> secondShell)
        {
            if (firstShell == null)
                throw new ArgumentNullException("firstShell", "The first shell is null.");
            if (secondShell == null)
                throw new ArgumentNullException("secondShell", "The second shell is null.");

            IList<Coordinate> intersection;
            if (PolygonAlgorithms.IsConvex(firstShell) && PolygonAlgorithms.IsConvex(secondShell))
            {                
                return IntersectionOfConvexPolygons(firstShell, secondShell, out intersection);
            }
            else
            {
                return IntersectionOfConcavePolygons(firstShell, secondShell, out intersection);
            }
        }

        #endregion

        #region Intersection computation

        /// <summary>
        /// Computes the intersection of a polygon and a line.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="lineStart">The starting coordinates of the line.</param>
        /// <param name="lineEnd">The ending coordinates of the line.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        public static IList<Coordinate> Intersection(IList<Coordinate> shell, Coordinate lineStart, Coordinate lineEnd)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            IList<Coordinate> intersection = null;
            if (Intersection(shell, lineStart, lineEnd, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
        }

        #endregion

        #region Private intersection computation methods

        /// <summary>
        /// Computes the intersection of a polygon and a line.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="lineStart">The starting coordinates of the line.</param>
        /// <param name="lineEnd">The ending coordinates of the line.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the polygon and the line intersect; otherwise, <c>false</c>.</returns>
        private static Boolean Intersection(IList<Coordinate> shell, Coordinate lineStart, Coordinate lineEnd, out IList<Coordinate> intersection)
        {
            intersection = null;

            if (shell.Count < 3)
                return false;

            if (lineStart.Equals(lineEnd))
            {
                if (WindingNumberAlgorithm.IsInsidePolygon(shell, lineStart))
                {
                    intersection = new List<Coordinate>() { lineStart };
                    return true;
                }
                else
                {
                    return false;
                }
            }
                
            if (lineStart.Z == lineEnd.Z)
            {
                if (lineStart.Z != shell[0].Z)
                    return false;

                if (PolygonAlgorithms.IsConvex(shell))
                {
                    // in case of convex polygons a simplified algorithm can be used
                    return IntersectionWithConvexPolygon(shell, lineStart, lineEnd, out intersection);
                }
                else
                {
                    return IntersectionWithConcavePolygon(shell, lineStart, lineEnd, out intersection);
                }
            }
            else
            {
                IList<Coordinate> intersectionWithPlane = LineAlgorithms.IntersectionWithPlane(lineStart, lineEnd, shell[0], new CoordinateVector(0, 0, 1));

                if (intersectionWithPlane.Count == 0 || intersectionWithPlane.Count == 2)
                    return false;

                if (!WindingNumberAlgorithm.IsInsidePolygon(shell, intersectionWithPlane[0]))
                    return false;

                intersection = new List<Coordinate> { intersectionWithPlane[0] };
                return true;
            }
        }

        /// <summary>
        /// Computes the intersection of a convex polygon and a line located in the same plane.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="lineStart">The starting coordinates of the line.</param>
        /// <param name="lineEnd">The ending coordinates of the line.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the polygon and the line intersect; otherwise, <c>false</c>.</returns>
        private static Boolean IntersectionWithConvexPolygon(IList<Coordinate> shell, Coordinate lineStart, Coordinate lineEnd, out IList<Coordinate> intersection)
        {
            // source: http://geomalgorithms.com/a13-_intersect-4.html

            intersection = null;

            Double maxEntering = 0;
            Double minLeaving = 1;
            Double t, numerator, denominator;
            CoordinateVector lineDirection = lineEnd - lineStart;
            CoordinateVector edge;

            for (Int32 i = 0; i < shell.Count - 1; i++)
            {
                edge = shell[i + 1] - shell[i];
                numerator = CoordinateVector.PerpProduct(edge, lineStart - shell[i]);
                denominator = -CoordinateVector.PerpProduct(edge, lineDirection);

                // check whether the line is parallel to the edge
                if (Math.Abs(denominator) < Calculator.Tolerance)
                {
                    if (numerator < 0) // the segment is outside of the edge
                        return false;
                    else // the segment is inside, but does not intersect the edge
                        continue; 
                }

                t = numerator / denominator;

                // check whether the line enters at this edge
                if (denominator < 0)
                {
                    if (t > maxEntering) 
                    {
                        maxEntering = t;

                        // the line enters after it has left
                        if (maxEntering > minLeaving)
                        {
                            return false;   
                        }
                    }
                }
                // check whether the line leaves at this edge
                else
                {
                    if (t < minLeaving)
                    {
                        minLeaving = t;

                        // the line leaves before it has entered
                        if (minLeaving < maxEntering)
                        {
                            return false;
                        }
                    }
                }
            }

            // compute the intersection coordinates
            intersection = new List<Coordinate>() { lineStart + maxEntering * lineDirection, lineStart + minLeaving * lineDirection };
            return true;
        }

        /// <summary>
        /// Computes the intersection of a cancave polygon and a line located in the same plane.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="lineStart">The starting coordinates of the line.</param>
        /// <param name="lineEnd">The ending coordinates of the line.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the polygon and the line intersect; otherwise, <c>false</c>.</returns>
        private static Boolean IntersectionWithConcavePolygon(IList<Coordinate> shell, Coordinate lineStart, Coordinate lineEnd, out IList<Coordinate> intersection)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes the intersection of two convex polygons.
        /// </summary>
        /// <param name="firstShell">The coordinates of the first polygon shell.</param>
        /// <param name="secondShell">The coordinates of the second polygon shell.</param>
        /// <param name="intersection">The coordinates of the intersection polygon shell.</param>
        /// <returns><c>true</c> if the two polygons intersect; otherwise, <c>false</c>.</returns>
        private static Boolean IntersectionOfConvexPolygons(IList<Coordinate> firstShell, IList<Coordinate> secondShell, out IList<Coordinate> intersection)
        { 
            // source: Laszlo, M. J.: Computatonal Geometry and Computer Graphics in C++, p. 154
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes the intersection of two concave polygons.
        /// </summary>
        /// <param name="firstShell">The coordinates of the first polygon shell.</param>
        /// <param name="secondShell">The coordinates of the second polygon shell.</param>
        /// <param name="intersection">The coordinates of the intersection polygon shell.</param>
        /// <returns><c>true</c> if the two polygons intersect; otherwise, <c>false</c>.</returns>
        private static Boolean IntersectionOfConcavePolygons(IList<Coordinate> firstShell, IList<Coordinate> secondShell, out IList<Coordinate> intersection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
