/// <copyright file="LineAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Contains algorithms for computing line properties.
    /// </summary>
    public static class LineAlgorithms
    {
        #region Private static properties

        /// <summary>
        /// Gets the empty list.
        /// </summary>
        private static IList<Coordinate> EmptyList { get { return new List<Coordinate>(); } }

        #endregion

        #region Centroid computation

        /// <summary>
        /// Computes the centroid of a line.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <returns>The centroid of the line. The centroid is <c>Undefined</c> if either coordinates are invalid.</returns>
        public static Coordinate Centroid(Coordinate lineStart, Coordinate lineEnd)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid)
                return Coordinate.Undefined;

            return new Coordinate((lineStart.X + lineEnd.X) / 2, (lineStart.Y + lineEnd.Y) / 2, (lineStart.Z + lineEnd.Z) / 2);
        }

        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="coordinates">The coordinates of the line string.</param>
        /// <returns>>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
        /// <exception cref="System.ArgumentNullException">The list of coordinates is null.</exception>
        public static Coordinate Centroid(IList<Coordinate> coordinates)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The list of coordinates is null.");

            // simple cases
            if (coordinates.Count == 0 || !coordinates[0].IsValid)
                return Coordinate.Undefined;
            if (coordinates.Count == 1)
                return coordinates[0];

            // take the centroids of the edges weighted with the length of the edge
            Double centroidX = 0, centroidY = 0, centroidZ = 0;
            Double totalLength = 0;
            for (Int32 i = 0; i < coordinates.Count - 1; i++)
            {
                if (!coordinates[i + 1].IsValid)
                    return Coordinate.Undefined;

                Double length = Coordinate.Distance(coordinates[i], coordinates[i + 1]);
                totalLength += length;

                centroidX += length * (coordinates[i].X + coordinates[i + 1].X) / 2;
                centroidY += length * (coordinates[i].Y + coordinates[i + 1].Y) / 2;
                centroidZ += length * (coordinates[i].Z + coordinates[i + 1].Z) / 2;
            }

            centroidX /= totalLength;
            centroidY /= totalLength;
            centroidZ /= totalLength;

            return new Coordinate(centroidX, centroidY, centroidZ);
        }

        #endregion

        #region Coincides computation

        /// <summary>
        /// Determines whether two infinite lines coincide.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns><c>true</c> if the two lines coincide; otherwise, <c>false</c>.</returns>
        public static Boolean Coincides(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            if (!firstCoordinate.IsValid || !firstVector.IsValid || !secondCoordinate.IsValid || !secondVector.IsValid)
                return false;
            
            return CoordinateVector.IsParalell(firstVector, secondVector) && Distance(firstCoordinate, firstVector, secondCoordinate) <= Calculator.Tolerance;
        }

        #endregion

        #region Contains computation

        /// <summary>
        /// Determines whether a line contains a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid || !coordinate.IsValid)
                return false;

            // check for empty line
            if (lineStart.Equals(lineEnd))
                return lineStart.Equals(coordinate);

            // chekc the staring and ending coordinates
            if (lineStart.Equals(coordinate) || lineEnd.Equals(coordinate))
                return true;

            // check the envelope
            if (!Envelope.Contains(Coordinate.LowerBound(lineStart, lineEnd), Coordinate.UpperBound(lineStart, lineEnd), coordinate))
                return false;

            // check the distance from the line
            return Distance(lineStart, lineEnd, coordinate) <= Calculator.Tolerance;
        }

        /// <summary>
        /// Determines whether an infinite line contains a specified coordinate.
        /// </summary>
        /// <param name="lineCoordinate">The coordinate of the line.</param>
        /// <param name="lineVector">The direction vector of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineCoordinate, CoordinateVector lineVector, Coordinate coordinate)
        {
            if (!lineCoordinate.IsValid || !lineVector.IsValid || !coordinate.IsValid)
                return false;

            return Distance(lineCoordinate, lineVector, coordinate) <= Calculator.Tolerance;
        }

        #endregion

        #region Distance computation

        /// <summary>
        /// Computes the distance of a line to a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The distance of <paramref name="coordinate" /> from the line.</returns>
        public static Double Distance(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid || !coordinate.IsValid)
                return Double.NaN;

            // source: http://geomalgorithms.com/a02-_lines.html

            // check for empty line
            if (lineStart.Equals(lineEnd))
                return Coordinate.Distance(coordinate, lineStart);

            // compute directional vector
            CoordinateVector v = lineEnd - lineStart;

            // check for the starting and ending coordinates
            Double c1 = (coordinate - lineStart) * v;
            if (c1 <= 0)
                return Coordinate.Distance(coordinate, lineStart);
            Double c2 = v * v;
            if (c2 <= c1)
                return Coordinate.Distance(coordinate, lineEnd);

            // compute distance to the nearest coordinate
            return Coordinate.Distance(coordinate, lineStart + (c1 / c2) * v);
        }

        /// <summary>
        /// Computes the distance of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns>The distance of the two lines.</returns>
        public static Double Distance(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return Double.NaN;

            // source: http://geomalgorithms.com/a07-_distance.html

            CoordinateVector u = firstLineEnd - firstLineStart;
            CoordinateVector v = secondLineEnd - secondLineStart;
            CoordinateVector w = firstLineStart - secondLineStart;
            Double a = u * u;
            Double b = u * v;
            Double c = v * v;
            Double d = u * w;
            Double e = v * w;
            Double D = a * c - b * b;
            Double sc, sN, sD = D;
            Double tc, tN, tD = D;

            if (D < Calculator.Tolerance) // the lines are collinear
            {
                sN = 0.0;
                sD = 1.0; 
                tN = e;
                tD = c;
            }
            else
            {
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0.0)
                {
                    sN = 0.0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0) 
            {
                tN = 0.0;
                if (-d < 0.0)
                    sN = 0.0;
                else if (-d > a)
                    sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD) 
            {
                tN = tD;
                if ((-d + b) < 0.0)
                    sN = 0;
                else if ((-d + b) > a)
                    sN = sD;
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }
            sc = (Math.Abs(sN) < Calculator.Tolerance ? 0.0 : sN / sD);
            tc = (Math.Abs(tN) < Calculator.Tolerance ? 0.0 : tN / tD);

            CoordinateVector dP = w + (sc * u) - (tc * v);  

            return Math.Sqrt(dP * dP);
        }

        /// <summary>
        /// Computes the distance of an infinite line to a specified coordinate.
        /// </summary>
        /// <param name="lineCoordinate">The line coordinate.</param>
        /// <param name="lineVector">The line vector.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The distance of <paramref name="coordinate" /> to the line. The distance is <c>NaN</c> if not all parameters are valid.</returns>
        public static Double Distance(Coordinate lineCoordinate, CoordinateVector lineVector, Coordinate coordinate)
        {
            if (!lineCoordinate.IsValid || !lineVector.IsValid || !coordinate.IsValid)
                return Double.NaN;

            return Coordinate.Distance(coordinate, lineCoordinate + (coordinate - lineCoordinate) * lineVector * lineVector);
        }

        #endregion

        #region Intersects computation

        /// <summary>
        /// Determines whether two lines intersect.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            IList<Coordinate> intersection;
            return Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, out intersection);
        }

        /// <summary>
        /// Determines whether two lines intersect internally.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean InternalIntersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            IList<Coordinate> intersection;
            return InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, out intersection);
        }

        /// <summary>
        /// Determines whether two infinite lines intersect.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="intersection">The coordinate of intersection.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            if (!firstCoordinate.IsValid || !firstVector.IsValid || !secondCoordinate.IsValid || !secondVector.IsValid)
                return false;

            Coordinate intersection;
            return Intersection(firstCoordinate, firstVector, secondCoordinate, secondVector, out intersection);
        }

        /// <summary>
        /// Determines whether a line intersects with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static Boolean IntersectsWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid || !planeCoordinate.IsValid || !planeNormalVector.IsValid)
                return false;

            IList<Coordinate> intersection;
            return IntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, out intersection);
        }


        #endregion

        #region Intersection computation

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static IList<Coordinate> Intersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return EmptyList;

            IList<Coordinate> intersection;
            if (Intersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, out intersection))
                return intersection;
            else
                return EmptyList;
        }

        /// <summary>
        /// Computes the intersection of two infinite lines.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns>The coordinate of the intersection. The coordinate is <c>Undefined</c> if there is no intersection.</returns>
        public static Coordinate Intersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            if (!firstCoordinate.IsValid || !firstVector.IsValid || !secondCoordinate.IsValid || !secondVector.IsValid)
                return Coordinate.Undefined;

            Coordinate intersection;
            if (Intersection(firstCoordinate, firstVector, secondCoordinate, secondVector, out intersection))
                return intersection;
            else
                return Coordinate.Undefined;
        }

        /// <summary>
        /// Computes the internal intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static IList<Coordinate> InternalIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return EmptyList;

            IList<Coordinate> intersection;
            if (InternalIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, out intersection))
                return intersection;
            else
                return EmptyList;
        }

        /// <summary>
        /// Computes the intersection of a line with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <returns><c>true</c> if the line and the plane intersect; otherwise, <c>false</c>.</returns>
        public static IList<Coordinate> IntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid || !planeCoordinate.IsValid || !planeNormalVector.IsValid)
                return EmptyList;

            IList<Coordinate> intersection;
            if (IntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, out intersection))
                return intersection;
            else
                return EmptyList;
        }


        #endregion

        #region IsCollinear computation

        /// <summary>
        /// Determines whether two lines are collnear.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines are collinear; otherwise, <c>false</c>.</returns>
        public static Boolean IsCollinear(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            // compute the directional vectors
            CoordinateVector u = (firstLineEnd - firstLineStart).Normalize();
            CoordinateVector v = (secondLineEnd - secondLineStart).Normalize();

            return CoordinateVector.IsParalell(u, v) && Coordinate.Distance(secondLineStart, firstLineStart + (secondLineStart - firstLineStart) * u * u) <= Calculator.Tolerance;
        }

        #endregion

        #region IsParalell computation

        /// <summary>
        /// Determines whether two lines are parallel.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParalell(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            // check if the directional vectors are parallel
            return CoordinateVector.IsParalell(firstLineEnd - firstLineStart, secondLineEnd - secondLineStart);
        }

        /// <summary>
        /// Determines whether two infinite lines are parallel.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParalell(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            if (!firstCoordinate.IsValid || !firstVector.IsValid || !secondCoordinate.IsValid || !secondVector.IsValid)
                return false;

            return CoordinateVector.IsParalell(firstVector, secondVector);
        }

        #endregion

        #region Private intersection computation

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the lines intersect; otherwise, <c>false</c>.</returns>
        private static Boolean Intersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, out IList<Coordinate> intersection)
        {
            intersection = null;

            // source: http://geomalgorithms.com/a05-_intersect-1.html

            // check validity
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            // check for empty lines
            if (firstLineStart.Equals(firstLineEnd))
            {
                if (firstLineStart.Equals(secondLineStart) || firstLineStart.Equals(secondLineEnd))
                {
                    intersection = new List<Coordinate>() { firstLineStart };
                    return true;
                }
                if (Contains(secondLineStart, secondLineEnd, firstLineStart))
                {
                    intersection = new List<Coordinate>() { firstLineStart };
                    return true;
                }
                else
                    return false;
            }
            if (secondLineStart.Equals(secondLineEnd))
            {
                if (secondLineStart.Equals(firstLineStart) || secondLineStart.Equals(firstLineEnd))
                {
                    intersection = new List<Coordinate>() { secondLineStart };
                    return true;
                }
                if (Contains(firstLineStart, firstLineEnd, secondLineStart))
                {
                    intersection = new List<Coordinate>() { secondLineStart };
                    return true;
                }
                else
                    return false;
            }

            // check for equal lines
            if (firstLineStart.Equals(secondLineStart) && firstLineEnd.Equals(secondLineEnd) || firstLineEnd.Equals(secondLineStart) && firstLineStart.Equals(secondLineEnd))
            {
                intersection = new List<Coordinate>() { secondLineStart, secondLineEnd };
                return true;
            }

            // check for the envelope
            if (!Envelope.Overlaps(Coordinate.LowerBound(firstLineStart, firstLineEnd), Coordinate.UpperBound(firstLineStart, firstLineEnd),
                                   Coordinate.LowerBound(secondLineStart, secondLineEnd), Coordinate.UpperBound(secondLineStart, secondLineEnd)))
            {
                return false;
            }
            // compute the direction vectors
            CoordinateVector u = firstLineEnd - firstLineStart;
            CoordinateVector v = secondLineEnd - secondLineStart;
            CoordinateVector w = firstLineStart - secondLineStart;

            // check for parallel lines
            if (CoordinateVector.IsParalell(u, v))
            {
                // the starting or ending coordinate of the second line must be on the first line
                Double b = (secondLineStart - firstLineStart) * u / (u * u);
                if (Coordinate.Distance(secondLineStart, firstLineStart + b * u) >= Calculator.Tolerance)
                {
                    return false;
                }
                Double t0, t1;
                CoordinateVector z = firstLineEnd - secondLineStart;
                if (v.X != 0)
                {
                    t0 = Math.Min(w.X / v.X, z.X / v.X);
                    t1 = Math.Max(w.X / v.X, z.X / v.X);
                }
                else
                {
                    t0 = Math.Min(w.Y / v.Y, z.Y / v.Y);
                    t1 = Math.Max(w.Y / v.Y, z.Y / v.Y);
                }
                if (t0 > 1 || t1 < 0)
                {
                    return false;
                }

                intersection = new List<Coordinate>() { secondLineStart + Math.Max(t0, 0) * v, secondLineStart + Math.Min(t1, 1) * v };
                return true;
            }

            // the lines are projected into a plane
            Double d = CoordinateVector.PerpProduct(u, v);
            if (d == 0) // this should not happen (case of parallel lines)
            {
                return false;
            }

            Double sI = CoordinateVector.PerpProduct(v, w) / d;
            if (sI < 0 || sI > 1) // if the intersection is beyond the line
            {
                return false;
            }

            Double tI = CoordinateVector.PerpProduct(u, w) / d;
            if (tI < 0 || tI > 1)
            {
                return false;
            }

            // finally, we check if we computed one coordinate
            if (Coordinate.Distance(firstLineStart + sI * u, secondLineStart + tI * v) > Calculator.Tolerance)
            {
                return false;
            }

            intersection = new List<Coordinate>() { firstLineStart + sI * u, firstLineStart + sI * u };
            return true;
        }

        /// <summary>
        /// Computes the intersection of two infinite lines.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="intersection">The coordinate of intersection.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        private static Boolean Intersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, out Coordinate intersection)
        {
            // source: http://geomalgorithms.com/a05-_intersect-1.html

            intersection = Coordinate.Undefined;

            // if they are parallel, they must also be collinear
            if (CoordinateVector.IsParalell(firstVector, secondVector))
            {
                intersection = (Distance(firstCoordinate, firstVector, secondCoordinate) <= Calculator.Tolerance) ? firstCoordinate : Coordinate.Undefined;
                return true;
            }

            CoordinateVector v = firstCoordinate - secondCoordinate;

            Double d = CoordinateVector.PerpProduct(firstVector, secondVector);
            if (d == 0)
            {
                return false;
            }

            Double sI = CoordinateVector.PerpProduct(secondVector, v) / d;
            Double tI = CoordinateVector.PerpProduct(firstVector, v) / d;

            if (Coordinate.Distance(firstCoordinate + sI * firstVector, secondCoordinate + tI * secondVector) > Calculator.Tolerance)
            {
                return false;
            }

            intersection = firstCoordinate + sI * firstVector;
            return true;
        }

        /// <summary>
        /// Computes the internal intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the lines intersect; otherwise, <c>false</c>.</returns>
        private static Boolean InternalIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, out IList<Coordinate> intersection)
        {
            intersection = null;

            // source: http://geomalgorithms.com/a05-_intersect-1.html

            // check validity
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            // check for empty lines
            if (firstLineStart.Equals(firstLineEnd))
            {
                if (firstLineStart.Equals(secondLineStart) || firstLineStart.Equals(secondLineEnd))
                    return false;
                if (Contains(secondLineStart, secondLineEnd, firstLineStart))
                {
                    intersection = new List<Coordinate>() { firstLineStart };
                    return true;
                }
                else
                    return false;
            }
            if (secondLineStart.Equals(secondLineEnd))
            {
                if (secondLineStart.Equals(firstLineStart) || secondLineStart.Equals(firstLineEnd))
                    return false;
                if (Contains(firstLineStart, firstLineEnd, secondLineStart))
                {
                    intersection = new List<Coordinate>() { secondLineStart };
                    return true;
                }
                else
                    return false;
            }

            // check for equal lines
            if (firstLineStart.Equals(secondLineStart) && firstLineEnd.Equals(secondLineEnd) || firstLineEnd.Equals(secondLineStart) && firstLineStart.Equals(secondLineEnd))
            {
                intersection = new List<Coordinate>() { secondLineStart, secondLineEnd };
                return true;
            }

            // check for the envelope
            if (!Envelope.Overlaps(Coordinate.LowerBound(firstLineStart, firstLineEnd), Coordinate.UpperBound(firstLineStart, firstLineEnd),
                                   Coordinate.LowerBound(secondLineStart, secondLineEnd), Coordinate.UpperBound(secondLineStart, secondLineEnd)))
            {
                return false;
            }
            // compute the direction vectors
            CoordinateVector u = firstLineEnd - firstLineStart;
            CoordinateVector v = secondLineEnd - secondLineStart;
            CoordinateVector w = firstLineStart - secondLineStart;

            // check for parallel lines
            if (CoordinateVector.IsParalell(u, v))
            {
                // the starting or ending coordinate of the second line must be on the first line
                Double b = (secondLineStart - firstLineStart) * u / (u * u);
                if (Coordinate.Distance(secondLineStart, firstLineStart + b * u) >= Calculator.Tolerance)
                {
                    return false;
                }
                Double t0, t1;
                CoordinateVector z = firstLineEnd - secondLineStart;
                if (v.X != 0)
                {
                    t0 = Math.Min(w.X / v.X, z.X / v.X);
                    t1 = Math.Max(w.X / v.X, z.X / v.X);
                }
                else
                {
                    t0 = Math.Min(w.Y / v.Y, z.Y / v.Y);
                    t1 = Math.Max(w.Y / v.Y, z.Y / v.Y);
                }
                if (t0 >= 1 || t1 <= 0)
                {
                    return false;
                }

                intersection = new List<Coordinate>() { secondLineStart + Math.Max(t0, 0) * v, secondLineStart + Math.Min(t1, 1) * v };
                return true;
            }

            // the lines are projected into a plane
            Double d = CoordinateVector.PerpProduct(u, v);
            if (d == 0) // this should not happen (case of parallel lines)
            {
                return false;
            }

            Double sI = CoordinateVector.PerpProduct(v, w) / d;
            if (sI <= 0 || sI >= 1) // if the intersection is beyond the line
            {
                return false;
            }

            Double tI = CoordinateVector.PerpProduct(u, w) / d;
            if (tI <= 0 || tI >= 1)
            {
                return false;
            }

            // finally, we check if we computed one coordinate
            if (Coordinate.Distance(firstLineStart + sI * u, secondLineStart + tI * v) > Calculator.Tolerance)
            {
                return false;
            }

            intersection = new List<Coordinate>() { firstLineStart + sI * u, firstLineStart + sI * u };
            return true;
        }

        /// <summary>
        /// Computes the intersection of a line with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the line and the plane intersect; otherwise, <c>false</c>.</returns>
        private static Boolean IntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, out IList<Coordinate> intersection)
        {
            // source: http://geomalgorithms.com/a05-_intersect-1.html

            intersection = null;

            CoordinateVector u = lineEnd - lineStart;
            CoordinateVector w = lineStart - planeCoordinate;

            Double d = CoordinateVector.DotProduct(planeNormalVector, u);
            Double n = -CoordinateVector.DotProduct(planeNormalVector, w);

            if (Math.Abs(d) < Calculator.Tolerance) // line is parallel to plane
            {
                if (n == 0) // the line lies on the plane
                {
                    intersection = new List<Coordinate>() { lineStart, lineEnd };
                    return true;
                }
                else
                    return false;
            }

            Double sI = n / d;
            if (sI < 0 || sI > 1)
                return false;

            // compute segment intersection coordinate
            intersection = new List<Coordinate>() { lineStart + sI * u };
            return false;
        }

        #endregion
    }
}
