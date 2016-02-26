/// <copyright file="LineAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Contains algorithms for computing line properties.
    /// </summary>
    public static class LineAlgorithms
    {
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
        /// Computes the centroid of a line.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The centroid of the line. The centroid is <c>Undefined</c> if either coordinates are invalid.</returns>
        public static Coordinate Centroid(Coordinate lineStart, Coordinate lineEnd, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            return precision.MakePrecise(Centroid(lineStart, lineEnd));
        }

        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <returns>>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
        /// <exception cref="System.ArgumentNullException">The line string is null.</exception>
        public static Coordinate Centroid(IBasicLineString lineString)
        {
            if (lineString == null)
                throw new ArgumentNullException("lineString", "The line string is null.");

            return Centroid(lineString.Coordinates);
        }

        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="lineString">The line string.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
        /// <exception cref="System.ArgumentNullException">The line string is null.</exception>
        public static Coordinate Centroid(IBasicLineString lineString, PrecisionModel precision)
        {
            if (lineString == null)
                throw new ArgumentNullException("lineString", "The line string is null.");

            return Centroid(lineString.Coordinates, precision); 
        }

        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="coordinates">The coordinates of the line string.</param>
        /// <returns>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
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


        /// <summary>
        /// Computes the centroid of a line string.
        /// </summary>
        /// <param name="coordinates">The coordinates of the line string.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The centroid of the line string. The centroid is <c>Undefined</c> if either coordinates are invalid or there are no coordinates specified.</returns>
        /// <exception cref="System.ArgumentNullException">The list of coordinates is null.</exception>
        public static Coordinate Centroid(IList<Coordinate> coordinates, PrecisionModel precision)
        {
            if (coordinates == null)
                throw new ArgumentNullException("coordinates", "The list of coordinates is null.");

            if (precision == null)
                precision = PrecisionModel.Default;

            return precision.MakePrecise(Centroid(coordinates));
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
            return Coincides(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two infinite lines coincide.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines coincide; otherwise, <c>false</c>.</returns>
        public static Boolean Coincides(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precision)
        {
            if (!firstCoordinate.IsValid || !firstVector.IsValid || !secondCoordinate.IsValid || !secondVector.IsValid)
                return false;

            if (precision == null)
                precision = PrecisionModel.Default;

            return CoordinateVector.IsParallel(firstVector, secondVector, precision) &&
                   Distance(firstCoordinate, firstVector, secondCoordinate) <= Math.Max(precision.Tolerance(firstCoordinate, secondCoordinate), precision.Tolerance(firstVector, secondVector));
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
            return Contains(lineStart, lineEnd, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether a line contains a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate, PrecisionModel precision)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid || !coordinate.IsValid)
                return false;

            if (precision == null)
                precision = PrecisionModel.Default;

            // check for empty line
            if (lineStart == lineEnd)
                return lineStart == coordinate;

            // check the staring and ending coordinates
            if (lineStart == coordinate || coordinate == lineEnd)
                return true;

            // check the envelope
            if (!Envelope.Contains(lineStart, lineEnd, coordinate))
                return false;

            // check the distance from the line
            return Distance(lineStart, lineEnd, coordinate, precision) <= precision.Tolerance(lineStart, lineEnd, coordinate);
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
            return Contains(lineCoordinate, lineVector, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether an infinite line contains a specified coordinate.
        /// </summary>
        /// <param name="lineCoordinate">The coordinate of the line.</param>
        /// <param name="lineVector">The direction vector of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the line contains the <paramref name="coordinate" />; otherwise, <c>false</c>.</returns>
        public static Boolean Contains(Coordinate lineCoordinate, CoordinateVector lineVector, Coordinate coordinate, PrecisionModel precision)
        {
            if (!lineCoordinate.IsValid || !lineVector.IsValid || !coordinate.IsValid)
                return false;

            if (precision == null)
                precision = PrecisionModel.Default;

            return Distance(lineCoordinate, lineVector, coordinate) <= precision.Tolerance(lineCoordinate, coordinate);
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
            return Distance(lineStart, lineEnd, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the distance of a line to a specified coordinate.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The distance of <paramref name="coordinate" /> from the line.</returns>
        public static Double Distance(Coordinate lineStart, Coordinate lineEnd, Coordinate coordinate, PrecisionModel precision)
        {
            if (!lineStart.IsValid || !lineEnd.IsValid || !coordinate.IsValid)
                return Double.NaN;

            // source: http://geomalgorithms.com/a02-_lines.html

            if (precision == null)
                precision = PrecisionModel.Default;

            // check for empty line
            if (lineStart == lineEnd)
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
            return Distance(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, PrecisionModel.Default);
        }

        /// <summary>
        /// Computes the distance of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The distance of the two lines.</returns>
        public static Double Distance(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
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

            if (precision == null)
                precision = PrecisionModel.Default;

            Double tolerance = precision.Tolerance(firstLineStart, firstLineStart, secondLineStart, secondLineEnd);

            if (D <= tolerance) // the lines are collinear
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
            sc = (Math.Abs(sN) <= tolerance ? 0.0 : sN / sD);
            tc = (Math.Abs(tN) <= tolerance ? 0.0 : tN / tD);

            CoordinateVector dP = w + (sc * u) - (tc * v);

            if (dP.IsNull)
                return 0;

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

            Double x = Math.Abs(lineVector.Y * coordinate.X + (-lineVector.X) * coordinate.Y - (lineVector.Y * lineCoordinate.X + ((-lineVector.X) * lineCoordinate.Y)));
            Double y = Math.Sqrt(Math.Pow(lineVector.Y, 2) + Math.Pow(lineVector.X, 2));

            return x / y;
        }

        #endregion

        #region Intersects computation

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
            IList<Coordinate> intersection;
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, PrecisionModel.Default, out intersection);
        }

        /// <summary>
        /// Determines whether two lines intersect internally.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean InternalIntersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            IList<Coordinate> intersection;
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, precision, out intersection);
        }

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
            IList<Coordinate> intersection;
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, PrecisionModel.Default, out intersection);
        }

        /// <summary>
        /// Determines whether two lines intersect.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            IList<Coordinate> intersection;
            return ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, precision, out intersection);
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
            Coordinate intersection;
            return ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default, out intersection);
        }

        /// <summary>
        /// Determines whether two infinite lines intersect.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="intersection">The coordinate of intersection.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        public static Boolean Intersects(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            Coordinate intersection;
            return ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, precision, out intersection);
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
            IList<Coordinate> intersection;
            return ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, PrecisionModel.Default, out intersection);
        }

        /// <summary>
        /// Determines whether a line intersects with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static Boolean IntersectsWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            IList<Coordinate> intersection;
            return ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, precision, out intersection);
        }

        #endregion

        #region Intersection computation

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
            IList<Coordinate> intersection;
            if (ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, PrecisionModel.Default, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
        }

        /// <summary>
        /// Computes the internal intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static IList<Coordinate> InternalIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            IList<Coordinate> intersection;
            if (ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, false, precision, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
        }

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
            IList<Coordinate> intersection;
            if (ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, PrecisionModel.Default, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
        }

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection; or nothing if no intersection exists.</returns>
        public static IList<Coordinate> Intersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            IList<Coordinate> intersection;
            if (ComputeIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, true, precision, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
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
            Coordinate intersection;
            if (ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default, out intersection))
                return intersection;
            else
                return Coordinate.Undefined;
        }

        /// <summary>
        /// Computes the intersection of two infinite lines.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns>The coordinate of the intersection. The coordinate is <c>Undefined</c> if there is no intersection.</returns>
        public static Coordinate Intersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            Coordinate intersection;
            if (ComputeIntersection(firstCoordinate, firstVector, secondCoordinate, secondVector, precision, out intersection))
                return intersection;
            else
                return Coordinate.Undefined;
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
            IList<Coordinate> intersection;
            if (ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, PrecisionModel.Default, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
        }

        /// <summary>
        /// Computes the intersection of a line with a plane.
        /// </summary>
        /// <param name="lineStart">The starting coordinate of the line.</param>
        /// <param name="lineEnd">The ending coordinate of the line.</param>
        /// <param name="planeCoordinate">A coordinate located on the plane.</param>
        /// <param name="planeNormalVector">The normal vector of the plane.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the line and the plane intersect; otherwise, <c>false</c>.</returns>
        public static IList<Coordinate> IntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, PrecisionModel precision)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

            IList<Coordinate> intersection;
            if (ComputeIntersectionWithPlane(lineStart, lineEnd, planeCoordinate, planeNormalVector, precision, out intersection))
                return intersection;
            else
                return new List<Coordinate>();
        }

        #endregion

        #region IsCollinear computation

        /// <summary>
        /// Determines whether two lines are collinear.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines are collinear; otherwise, <c>false</c>.</returns>
        public static Boolean IsCollinear(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return IsCollinear(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two lines are collinear.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines are collinear; otherwise, <c>false</c>.</returns>
        public static Boolean IsCollinear(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            if (precision == null)
                precision = PrecisionModel.Default;

            // compute the directional vectors
            CoordinateVector u = (firstLineEnd - firstLineStart).Normalize();
            CoordinateVector v = (secondLineEnd - secondLineStart).Normalize();

            return CoordinateVector.IsParallel(u, v, precision) && 
                   Coordinate.Distance(secondLineStart, firstLineStart + (secondLineStart - firstLineStart) * u * u) <= precision.Tolerance(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd);
        }

        #endregion

        #region IsParallel computation

        /// <summary>
        /// Determines whether two lines are parallel.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd)
        {
            return IsParallel(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two lines are parallel.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, PrecisionModel precision)
        {
            if (!firstLineStart.IsValid || !firstLineEnd.IsValid || !secondLineStart.IsValid || !secondLineEnd.IsValid)
                return false;

            // check if the directional vectors are parallel
            return CoordinateVector.IsParallel(firstLineEnd - firstLineStart, secondLineEnd - secondLineStart, precision);
        }

        /// <summary>
        /// Determines whether two infinite lines are parallel.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector)
        {
            return IsParallel(firstCoordinate, firstVector, secondCoordinate, secondVector, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether two infinite lines are parallel.
        /// </summary>
        /// <param name="firstCoordinate">The coordinate of the first line.</param>
        /// <param name="firstVector">The direction vector of the first line.</param>
        /// <param name="secondCoordinate">The coordinate of the second line.</param>
        /// <param name="secondVector">The direction vector of the second line.</param>
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines are parallel; otherwise, <c>false</c>.</returns>
        public static Boolean IsParallel(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precision)
        {
            if (!firstCoordinate.IsValid || !firstVector.IsValid || !secondCoordinate.IsValid || !secondVector.IsValid)
                return false;

            return CoordinateVector.IsParallel(firstVector, secondVector, precision);
        }

        #endregion

        #region Private intersection computation methods

        /// <summary>
        /// Computes the intersection of two lines.
        /// </summary>
        /// <param name="firstLineStart">The starting coordinate of the first line.</param>
        /// <param name="firstLineEnd">The ending coordinate of the first line.</param>
        /// <param name="secondLineStart">The starting coordinate of the second line.</param>
        /// <param name="secondLineEnd">The ending coordinate of the second line.</param>
        /// <param name="includeBoundary">A value indicating whether to include intersection on the boundary.</param>
        /// <param name="precision">The precision model.</param>
        /// <param name="intersection">A list containing the staring and ending coordinate of the intersection; or the single coordinate of intersection.</param>
        /// <returns><c>true</c> if the lines intersect; otherwise, <c>false</c>.</returns>
        private static Boolean ComputeIntersection(Coordinate firstLineStart, Coordinate firstLineEnd, Coordinate secondLineStart, Coordinate secondLineEnd, Boolean includeBoundary, PrecisionModel precision, out IList<Coordinate> intersection)
        {
            if (precision == null)
                precision = PrecisionModel.Default;

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
                    intersection = new List<Coordinate>() { precision.MakePrecise(firstLineStart) };
                    return true;
                }
                if (Contains(secondLineStart, secondLineEnd, firstLineStart, precision))
                {
                    intersection = new List<Coordinate>() { precision.MakePrecise(firstLineStart) };
                    return true;
                }
                else
                    return false;
            }
            if (secondLineStart.Equals(secondLineEnd))
            {
                if (secondLineStart.Equals(firstLineStart) || secondLineStart.Equals(firstLineEnd))
                {
                    intersection = new List<Coordinate>() { precision.MakePrecise(secondLineStart) };
                    return true;
                }
                if (Contains(firstLineStart, firstLineEnd, secondLineStart, precision))
                {
                    intersection = new List<Coordinate>() { precision.MakePrecise(secondLineStart) };
                    return true;
                }
                else
                    return false;
            }

            // check for equal lines
            if (firstLineStart.Equals(secondLineStart) && firstLineEnd.Equals(secondLineEnd) || 
                firstLineEnd.Equals(secondLineStart) && firstLineStart.Equals(secondLineEnd))
            {
                intersection = new List<Coordinate>() { precision.MakePrecise(secondLineStart), precision.MakePrecise(secondLineEnd) };
                return true;
            }

            // check for the envelope
            if (!Envelope.Intersects(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd))
            {
                return false;
            }

            // compute the direction vectors
            CoordinateVector u = firstLineEnd - firstLineStart;
            CoordinateVector v = secondLineEnd - secondLineStart;
            CoordinateVector w = firstLineStart - secondLineStart;

            // check for parallel lines
            if (CoordinateVector.IsParallel(u, v, precision))
            {
                // the starting or ending coordinate of the second line must be on the first line (or vica verssa)
                Double b1 = (secondLineStart - firstLineStart) * u / (u * u);
                Double b2 = (firstLineStart - secondLineStart) * v / (v * v);

                if (Coordinate.Distance(secondLineStart, firstLineStart + b1 * u) > precision.Tolerance(secondLineStart, firstLineStart) &&
                    Coordinate.Distance(firstLineStart, secondLineStart + b2 * v) > precision.Tolerance(firstLineStart, secondLineStart))
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

                intersection = new List<Coordinate>()
                {
                    precision.MakePrecise(secondLineStart + Math.Max(t0, 0) * v),
                    precision.MakePrecise(secondLineStart + Math.Min(t1, 1) * v)
                };
                return true;
            }

            // the lines are projected into a plane
            Double d = CoordinateVector.PerpProduct(u, v);
            if (d == 0) // this should not happen (case of parallel lines)
            {
                return false;
            }

            Double sI = CoordinateVector.PerpProduct(v, w) / d;
            Double tI = CoordinateVector.PerpProduct(u, w) / d;

            if (includeBoundary)
            {
                if (sI < 0 || sI > 1 || tI < 0 || tI > 1) // if the intersection is beyond the line
                {
                    return false;
                }
            }
            else
            {
                if (sI <= 0 || sI >= 1 || tI <= 0 || tI >= 1) // if the intersection is beyond the line
                {
                    return false;
                }
            }

            // finally, we check if we computed one coordinate
            if (Coordinate.Distance(firstLineStart + sI * u, secondLineStart + tI * v) > precision.Tolerance(secondLineStart, firstLineStart))
            {
                return false;
            }

            intersection = new List<Coordinate>() { precision.MakePrecise(firstLineStart + sI * u) };
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
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the two lines intersect; otherwise, <c>false</c>.</returns>
        private static Boolean ComputeIntersection(Coordinate firstCoordinate, CoordinateVector firstVector, Coordinate secondCoordinate, CoordinateVector secondVector, PrecisionModel precision, out Coordinate intersection)
        {
            // source: http://geomalgorithms.com/a05-_intersect-1.html

            intersection = Coordinate.Undefined;

            Double tolerance = precision.Tolerance(firstCoordinate, firstCoordinate + firstVector, secondCoordinate, secondCoordinate + secondVector);

            // if they are parallel, they must also be collinear
            if (CoordinateVector.IsParallel(firstVector, secondVector))
            {
                intersection = (Distance(firstCoordinate, firstVector, secondCoordinate) <= tolerance) ? precision.MakePrecise(firstCoordinate) : Coordinate.Undefined;
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

            if (Coordinate.Distance(firstCoordinate + sI * firstVector, secondCoordinate + tI * secondVector) > tolerance)
            {
                return false;
            }

            intersection = precision.MakePrecise(firstCoordinate + sI * firstVector);
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
        /// <param name="precision">The precision model.</param>
        /// <returns><c>true</c> if the line and the plane intersect; otherwise, <c>false</c>.</returns>
        private static Boolean ComputeIntersectionWithPlane(Coordinate lineStart, Coordinate lineEnd, Coordinate planeCoordinate, CoordinateVector planeNormalVector, PrecisionModel precision, out IList<Coordinate> intersection)
        {
            // source: http://geomalgorithms.com/a05-_intersect-1.html

            intersection = null;

            CoordinateVector u = lineEnd - lineStart;
            CoordinateVector w = lineStart - planeCoordinate;

            Double d = CoordinateVector.DotProduct(planeNormalVector, u);
            Double n = -CoordinateVector.DotProduct(planeNormalVector, w);

            if (Math.Abs(d) <= precision.Tolerance(lineStart, lineEnd, planeCoordinate)) // line is parallel to plane
            {
                if (n == 0) // the line lies on the plane
                {
                    intersection = new List<Coordinate>
                    {
                        precision.MakePrecise(lineStart),
                        precision.MakePrecise(lineEnd)
                    };
                    return true;
                }
                else
                    return false;
            }

            Double sI = n / d;
            if (sI < 0 || sI > 1)
                return false;

            // compute segment intersection coordinate
            intersection = new List<Coordinate> { precision.MakePrecise(lineStart + sI * u) };
            return false;
        }

        #endregion
    }
}
