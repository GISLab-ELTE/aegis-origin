/// <copyright file="TopoJsonConverter.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Norbert Vass</author>

using ELTE.AEGIS.Geometry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace ELTE.AEGIS.IO.TopoJSON
{
    public static class TopoJsonConverter
    {
        #region Private types

        /// <summary>
        /// Type for pointing to the first and the last coordinate of an arc in the coordinates array.
        /// </summary>
        private struct ArcIndex
        {
            public int First { get; set; }
            public int Last { get; set; }
        }

        /// <summary>
        /// Provides information about a coordinate.
        /// </summary>
        private class CoordinateInfo
        {
            public bool IsJunction { get; set; }
            public List<KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>> Neighbours { get; set; }
        }

        #endregion

        #region Private static fields

        /// <summary>
        /// Used to write to the output json file.
        /// </summary>
        private static JsonTextWriter _jsonwriter;

        /// <summary>
        /// The arcs stored during writing. Will be written out after we wrote all geometries.
        /// </summary>
        private static List<List<List<double>>> _arcs;

        #endregion

        #region Public conversion methods from Geometry to TopoJSON

        /// <summary>
        /// Converts Geometry to TopoJSON representation.
        /// </summary>
        /// <param name="geometries">The geometries wanted to write out.</param>
        /// <param name="path">The path for the output file.</param>
        /// <param name="qFact0">The quantization factor for coordinate X.</param>
        /// <param name="qFact1">The quantization factor for coordinate Y.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The parameter is null.
        /// </exception>
        /// <exception cref="System.IOException">
        /// The path is invalid.
        /// </exception>
        public static void ToTopoJson(this IList<IGeometry> geometries, string path, double qFact0 = 10000.0, double qFact1 = 10000.0)
        {
            if (geometries == null)
                throw new ArgumentNullException("geometries", "The geometries parameter is null");
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path", "Path is null or contains only whitespaces");

            double b0 = Math.Log10(qFact0);
            double b1 = Math.Log10(qFact1);
            if (b0 != Math.Round(b0, 0) || b1 != Math.Round(b1, 0))
                throw new ArgumentException("The quantization factors must be a power of 10");
            if (geometries.Count == 0)
                return;

            List<IGeometry> geos = new List<IGeometry>(geometries);
            _arcs = new List<List<List<double>>>();
            try
            {
                StreamWriter writer = new StreamWriter(path, false);
                _jsonwriter = new JsonTextWriter(writer);

                //start quantizing
                //compute the scale and translate arrays
                double[] scale = new double[2];
                double[] translate = new double[2];
                ComputeQuantizingArrays(geos, scale, translate, qFact0, qFact1);

                //quantize every point and lineString
                for (int i = 0; i < geos.Count; i++)
                    geos[i] = QuantizeGeometry(geos[i], scale, translate);
                //quantization finished

                /*time to create the topology. It has 4 steps:
                 * 1st step: Extract all lineStrings and lineRings -> the two must be separated
                 *              if a lineString is a lineRing, it doesn't matter, we store as lineString.
                 *              We store all the coordinates from linestrings and lines.
                 * 2nd step: Join. Determine junctions (the points which 2 or more lines (or rings) included by).
                 * 3rd step: Cut. Cut lines and rings at junctions.
                 * 4th step: Dedup. Removing duplicates from cutted arcs. If an arc equals another arc's
                 *                  reverse form, then this duplicate is also eliminated.
                 * 
                 * The created topology then stored in the _arcs array.
                 * */

                //1st step: Extract.
                List<ArcIndex> lines = new List<ArcIndex>();
                List<ArcIndex> rings = new List<ArcIndex>();
                List<Coordinate> coordinates = new List<Coordinate>();

                //int index = -1;
                for (int i = 0; i < geos.Count; i++)
                    ExtractGeometry(geos[i], ref coordinates, ref lines, ref rings);

                //2nd step: Join
                //HashSet<Coordinate> junctions;
                ISet<Coordinate> junctions = ComputeJunctions(coordinates, lines, rings);

                //3rd step: Cut
                CutJunctionsByPoint(ref coordinates, ref lines, ref rings, junctions);

                //4th step: Dedup
                //2 things in 1: during deduplicating, we save the arcs to _arcs list.
                DeduplicateArcs(coordinates, lines, rings);
                //topology creating finished

                //start to write data
                _jsonwriter.WriteStartObject();
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Topology");                

                _jsonwriter.WritePropertyName("objects");
                _jsonwriter.WriteStartObject();

                int objnum = 1;

                for (int i = 0; i < geos.Count; i++)
                {
                    if (geos[i].Metadata != null && geos[i].Metadata.ContainsKey("NAME"))
                        WriteObject(geos[i], geos[i].Metadata["NAME"].ToString());
                    else
                    {
                        WriteObject(geos[i], "object" + objnum.ToString());
                        objnum++;
                    }
                }

                //delta-encode all arcs
                for (int i = 0; i < _arcs.Count; i++)
                {
                    double prevx = _arcs[i][0][0];
                    double prevy = _arcs[i][0][1];

                    for (int j = 1; j < _arcs[i].Count; j++)
                    {
                        _arcs[i][j][0] -= prevx;
                        _arcs[i][j][1] -= prevy;

                        prevx += _arcs[i][j][0];
                        prevy += _arcs[i][j][1];
                    }
                }                

                //change topology's transform
                scale[0] = 1.0 / scale[0];
                scale[1] = 1.0 / scale[1];
                translate[0] = -translate[0];
                translate[1] = -translate[1];

                _jsonwriter.WriteEndObject();

                WriteTransform(scale, translate);

                _jsonwriter.WritePropertyName("arcs");
                WriteArcs();                
                _jsonwriter.WriteEndObject();
            }
            catch (IOException ioex)
            {
                throw new IOException("Wrong file path.", ioex);
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Geometry content is invalid", ex);
            }
            finally
            {                
                _jsonwriter.Close();
                _arcs.Clear();
            }
        }

        #endregion

        #region Private static methods

        #region Private static extract methods
        /// <summary>
        /// Separates linestrings and polygons to lines and rings lists.
        /// </summary>
        /// <param name="geometry">The geometry to be separated.</param>
        /// <param name="coordinates">The coordinates list where we store the geometry's coordinates.</param>
        /// <param name="lines">List of the lines.</param>
        /// <param name="rings">List of the rings.</param>
        private static void ExtractGeometry(IGeometry geometry, ref List<Coordinate> coordinates, ref List<ArcIndex> lines, ref List<ArcIndex> rings/*, ref int index*/)
        {
            if (geometry is ILineString)
            {
                ExtractLineString(geometry as ILineString, ref coordinates, ref lines/*, ref index*/);
            }
            else if (geometry is IMultiLineString)
            {
                for (int i = 0; i < (geometry as IMultiLineString).Count; i++)
                    ExtractLineString((geometry as IMultiLineString)[i], ref coordinates, ref lines/*, ref index*/);
            }
            else if (geometry is IPolygon)
            {
                ExtractPolygon(geometry as IPolygon, ref coordinates, ref rings/*, ref index*/);
            }
            else if (geometry is IMultiPolygon)
            {
                for (int i = 0; i < (geometry as IMultiPolygon).Count; i++)
                    ExtractPolygon((geometry as IMultiPolygon)[i], ref coordinates, ref rings/*, ref index*/);
            }
            else if (geometry is IGeometryCollection<IGeometry>)
            {
                for (int i = 0; i < (geometry as IGeometryCollection<IGeometry>).Count; i++)
                    ExtractGeometry((geometry as IGeometryCollection<IGeometry>)[i], ref coordinates, ref lines, ref rings/*, ref index*/);
            }
            else if (!(geometry is IPoint || geometry is IMultiPoint))
                throw new NotSupportedException("The following geometry type not supported by TopoJSON: " + geometry.Name);
        }

        /// <summary>
        /// Extracts a LineString. It's coordinates are stored in coordinates list. 
        /// It's first and last coordinate index stored in lines.
        /// </summary>
        /// <param name="line">The linestring.</param>
        /// <param name="coordinates">The coordinates list.</param>
        /// <param name="lines">The arc indexes list.</param>
        private static void ExtractLineString(ILineString line, ref List<Coordinate> coordinates, ref List<ArcIndex> lines/*, ref int index*/)
        {
            int index = coordinates.Count - 1;
            for (int i = 0; i < line.Coordinates.Count; i++)
            {
                index++;
                coordinates.Add(line.Coordinates[i]);
            }

            lines.Add(new ArcIndex { First = index - line.Coordinates.Count + 1, Last = index});
        }

        /// <summary>
        /// Extracts a Polygon. It's coordinates are stored in coordinates list. 
        /// It's first and last coordinate index stored in lines.
        /// </summary>
        /// <param name="poly">The polygon.</param>
        /// <param name="coordinates">The coordinates list.</param>
        /// <param name="lines">The arc indexes list.</param>
        private static void ExtractPolygon(IPolygon poly, ref List<Coordinate> coordinates, ref List<ArcIndex> rings/*, ref int index*/)
        {
            int index = coordinates.Count - 1;
            for (int i = 0; i < poly.Shell.Coordinates.Count; i++)
            {
                index++;
                coordinates.Add(poly.Shell.Coordinates[i]);
            }

            rings.Add(new ArcIndex { First = index - poly.Shell.Coordinates.Count + 1, Last = index});

            for (int i = 0; i < poly.HoleCount; i++)
            {
                for (int j = 0; j < poly.Holes[i].Coordinates.Count; j++)
                {
                    index++;
                    coordinates.Add(poly.Holes[i].Coordinates[j]);
                }

                rings.Add(new ArcIndex { First = index - poly.Holes[i].Coordinates.Count + 1, Last = index });
            }
        }

        #endregion

        #region Private static join methods

        /// <summary>
        /// Computes junctions and stores them in a Set.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="lines">The linestrings' indexes.</param>
        /// <param name="rings">The polygons' indexes.</param>
        private static ISet<Coordinate> ComputeJunctions(List<Coordinate> coordinates, List<ArcIndex> lines, List<ArcIndex> rings)
        {
            /*neighboursByCoordinate: contains the neighbours of all coordinates
             * The key is the Coordinate from which we want to determine if it is a junction or not (and we store some neighbours with it).
             * The value is a CoordinateInfo, which has
             *      isJunction: boolean which tells if this coordinate is a junction or not
             *      NeighBours: List of KeyValuePairs, which elements has
             *          
             *              Key: integer, the number of the arc which owns the value
             *              Value: KeyValuePair of two coordinates, the Key is the previous coordinate, the Value is the right. We store
             *                      neighbours of only one arc's coordinates, if the coordinate appears in another arc
             *                      and the neighbours are different, then we know that this is a junction 
             *                      (or if the neigbour coordinates matches, then it is not junction, and we store it too).
            */
            HashSet<Coordinate> junctions = new HashSet<Coordinate>();
            Dictionary<Coordinate, CoordinateInfo> neighboursByCoordinate = 
                new Dictionary<Coordinate, CoordinateInfo>();

            for (int i = 0; i < lines.Count; i++)
            {
                int linestart = lines[i].First;
                int lineend = lines[i].Last;

                for (int linemid = linestart; linemid <= lineend; linemid++ )
                {
                    Coordinate prev = linemid != linestart ? coordinates[linemid - 1] : new Coordinate(double.NaN, double.NaN);
                    Coordinate c = coordinates[linemid];
                    Coordinate next = linemid != lineend ? coordinates[linemid + 1] : new Coordinate(double.NaN, double.NaN);

                    if (neighboursByCoordinate.ContainsKey(c))
                    {
                        if (!neighboursByCoordinate[c].IsJunction)
                        {
                            bool l = true;
                            var kvp = neighboursByCoordinate[c].Neighbours[0];

                            if (kvp.Key == i)
                            {
                                for (int j = 0; j < neighboursByCoordinate[c].Neighbours.Count && l; j++)
                                {
                                    if (((kvp.Value.Key == prev && kvp.Value.Value == next)
                                    || (kvp.Value.Key == next && kvp.Value.Value == prev)))
                                        l = false;
                                }

                                if (l)
                                    neighboursByCoordinate[c].Neighbours.Add
                                    (
                                        new KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>(i,
                                        new KeyValuePair<Coordinate, Coordinate>(prev, next))
                                    );
                            }
                            else
                            {
                                for (int j = 0; j < neighboursByCoordinate[c].Neighbours.Count && l; j++)
                                {
                                    kvp = neighboursByCoordinate[c].Neighbours[j];

                                    if (!((kvp.Value.Key == prev && kvp.Value.Value == next)
                                        || (kvp.Value.Key == next && kvp.Value.Value == prev)))
                                    {
                                        l = false;
                                        neighboursByCoordinate[c].IsJunction = true;
                                    }
                                }
                            }                            
                        }
                    }                        
                    else
                        neighboursByCoordinate.Add(c, new CoordinateInfo {
                            IsJunction = false,
                            Neighbours = new List<KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>>
                        {
                            new KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>(i, new KeyValuePair<Coordinate, Coordinate>(prev, next))
                        }});
                }
            }

            for (int i = 0, ii = lines.Count; i < rings.Count; i++, ii++)
            {
                int ringstart = rings[i].First;
                int ringend = rings[i].Last;

                for (int ringmid = ringstart; ringmid <= ringend; ringmid++ )
                {
                    Coordinate prev = ringmid != ringstart ? coordinates[ringmid - 1] : new Coordinate(double.NaN, double.NaN);
                    Coordinate c = coordinates[ringmid];
                    Coordinate next = ringmid != ringend ? coordinates[ringmid + 1] : new Coordinate(double.NaN, double.NaN);

                    if (neighboursByCoordinate.ContainsKey(c))
                    {
                        if (!neighboursByCoordinate[c].IsJunction)
                        {
                            bool l = true;
                            var kvp = neighboursByCoordinate[c].Neighbours[0];

                            if (kvp.Key == ii)
                            {
                                for (int j = 0; j < neighboursByCoordinate[c].Neighbours.Count && l; j++)
                                {
                                    if ((kvp.Value.Key == prev && kvp.Value.Value == next)
                                    || (kvp.Value.Key == next && kvp.Value.Value == prev))
                                        l = false;
                                }

                                if (l)
                                    neighboursByCoordinate[c].Neighbours.Add
                                    (
                                        new KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>(ii,
                                        new KeyValuePair<Coordinate, Coordinate>(prev, next))
                                    );
                            }
                            else
                            {
                                for (int j = 0; j < neighboursByCoordinate[c].Neighbours.Count && l; j++)
                                {
                                    kvp = neighboursByCoordinate[c].Neighbours[j];

                                    if (!((kvp.Value.Key == prev && kvp.Value.Value == next)
                                        || (kvp.Value.Key == next && kvp.Value.Value == prev)))
                                    {
                                        l = false;
                                        neighboursByCoordinate[c].IsJunction = true;
                                    }
                                }
                            }
                        }
                    }                        
                    else
                        neighboursByCoordinate.Add(c, new CoordinateInfo {
                            IsJunction = false,
                            Neighbours = new List<KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>>
                        {
                            new KeyValuePair<int, KeyValuePair<Coordinate, Coordinate>>(i, new KeyValuePair<Coordinate, Coordinate>(prev, next))
                        }});
                }
            }

            foreach (var element in neighboursByCoordinate)
            {
                if (element.Value.IsJunction)
                {
                    junctions.Add(element.Key);
                }
            }

            return junctions;
        }

        #endregion

        #region Private static cut methods
        /// <summary>
        /// Cuts the polygons and linestrings at junctions.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="lines">The linestrings.</param>
        /// <param name="rings">The polygons.</param>
        /// <param name="junctions">The junctions.</param>
        private static void CutJunctionsByPoint(ref List<Coordinate> coordinates, ref List<ArcIndex> lines, ref List<ArcIndex> rings, ISet<Coordinate> junctions)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                ArcIndex line = lines[i];
                int linemid = line.First;
                int lineend = line.Last;

                for (linemid++; linemid < lineend; linemid++)
                {
                    if (junctions.Contains(coordinates[linemid]))
                    {
                        ArcIndex newindex = new ArcIndex { First = linemid, Last = lineend };
                        lineend = linemid;
                        lines[i] = new ArcIndex { First = line.First, Last = linemid };
                        lines.Insert(i + 1, newindex);
                    }
                }
            }

            for (int i = 0; i < rings.Count; i++)
            {
                ArcIndex ring = rings[i];
                int ringstart = ring.First;
                int ringmid = ringstart;
                int ringend = ring.Last;
                bool ringfixed = junctions.Contains(coordinates[ringstart]);

                for (ringmid++; ringmid < ringend; ringmid++)
                {
                    if (junctions.Contains(coordinates[ringmid]))
                    {
                        if (ringfixed)
                        {
                            ArcIndex newindex = new ArcIndex { First = ringmid, Last = ringend };
                            ringend = ringmid;
                            rings[i] = new ArcIndex { First = ringstart, Last = ringmid };
                            rings.Insert(i + 1, newindex);
                        }
                        else
                        {
                            //for the first junction, we can rotate the array rather than cut

                            //coordinates.Reverse(ringstart, ringend - ringstart + 1);
                            ReverseCoordinates(ref coordinates, ringstart, ringend);
                            //coordinates.Reverse(ringstart, ringmid - ringstart + 1);
                            ReverseCoordinates(ref coordinates, ringstart, ringstart + (ringend - ringmid));
                            //coordinates.Reverse(ringmid, ringend - ringmid + 1);
                            ReverseCoordinates(ref coordinates, ringstart + (ringend - ringmid), ringend);

                            coordinates[ringend] = coordinates[ringstart];
                            ringfixed = true;
                            ringmid = ringstart; // cause the rotating, we may have skipped junctions, so restart the ring
                        }
                    }
                }
            }
        }

        private static void ReverseCoordinates(ref List<Coordinate> coordinates, int start, int end)
        {
            end--;
            for (int mid = start + (end-- - start) / 2; start < mid; ++start, --end)
            {
                Coordinate temp = coordinates[start];
                coordinates[start] = coordinates[end];
                coordinates[end] = temp;
            }
        }

        #endregion

        #region Private static deduplicate methods
        /// <summary>
        /// Remove duplicates. Also stores deduplicated arcs to _arcs.
        /// </summary>
        /// <param name="coordinates">The coordinates.</param>
        /// <param name="lines">The linestrings.</param>
        /// <param name="rings">The polygons.</param>
        private static void DeduplicateArcs(List<Coordinate> coordinates, List<ArcIndex> lines, List<ArcIndex> rings)
        {
            int lineNum = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                int linestart = lines[i].First;
                int lineend = lines[i].Last;
                List<List<double>> arc = new List<List<double>>();

                for (;linestart <= lineend; linestart++)
                {
                    Coordinate c = coordinates[linestart];
                    List<double> coord = new List<double> { c.X, c.Y };
                    if (c.Z != 0.0)
                        coord.Add(c.Z);

                    arc.Add(coord);
                }
                
                bool isSame = false;
                for (int a = 0; a < _arcs.Count && !isSame; a++)
                {
                    if (IsEqualArcs(_arcs[a], arc))
                        isSame = true;
                    else if (IsEqualArcs(_arcs[a], arc.AsEnumerable().Reverse().ToList()))
                        isSame = true;
                }

                if (!isSame)
                {
                    lineNum++;
                    _arcs.Add(arc);
                }
            }

            for (int i = 0; i < rings.Count; i++)
            {
                int ringstart = rings[i].First;
                int ringend = rings[i].Last;
                List<List<double>> arc = new List<List<double>>();

                for (;ringstart <= ringend; ringstart++)
                {
                    Coordinate c = coordinates[ringstart];
                    List<double> coord = new List<double> { c.X, c.Y };
                    if (c.Z != 0.0)
                        coord.Add(c.Z);

                    arc.Add(coord);
                }

                bool isSame = false;
                for (int a = 0; a < _arcs.Count && !isSame; a++)
                {
                    if (IsEqualArcs(_arcs[a], arc))
                        isSame = true;
                    else if (IsEqualArcs(_arcs[a], arc.AsEnumerable().Reverse().ToList()))
                        isSame = true;
                }

                if (!isSame) 
                {
                    bool l = true;
                    int off1 = FindMinimumOffset(arc);
                    int size = arc.Count;
                    for (int j = lineNum; j < _arcs.Count && l; j++)
                    {
                        if (size == _arcs[j].Count)
                        {
                            //we find the minimum point of the two rings, and then we start the comparing from that point
                            int off2 = FindMinimumOffset(_arcs[j]);
                            bool equal = true;
                            for (int k = 0; k < size && equal; k++)
                            {
                                List<double> point1 = arc[(k + off1) % size];
                                List<double> point2 = _arcs[j][(k + off2) % size];

                                if (!(point1.Count == point2.Count && point1[0] == point2[0] && point1[1] == point2[1] && (point1.Count == 2 || point1.Count == 3 && point1[2] == point2[2])))
                                    equal = false;
                            }

                            if (equal)
                            {
                                int revoff1 = size - 1 - off1;
                                List<List<double>> revarc = arc.AsEnumerable().Reverse().ToList();

                                for (int k = 0; k < size && equal; k++)
                                {
                                    List<double> point1 = revarc[(k + off1) % size];
                                    List<double> point2 = _arcs[j][(k + off2) % size];

                                    if (!(point1.Count == point2.Count && point1[0] == point2[0] && point1[1] == point2[1] && (point1.Count == 2 || point1.Count == 3 && point1[2] == point2[2])))
                                        equal = false;
                                }

                                if (equal)
                                    l = false;
                            }
                        }
                    }

                    if (l)
                        _arcs.Add(arc);
                }
            }
        }
        /// <summary>
        /// Determines whether two arcs are the same or not.
        /// </summary>
        /// <param name="arc1">The first arc.</param>
        /// <param name="arc2">The second arc.</param>
        /// <returns></returns>
        private static bool IsEqualArcs(List<List<double>> arc1, List<List<double>> arc2)
        {
            if (arc1.Count != arc2.Count)
                return false;

            for (int i = 0; i < arc1.Count; i++)
            {
                if (!arc1[i].SequenceEqual(arc2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Finds the minimum offset of a polygon.
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        private static int FindMinimumOffset(List<List<double>> arc)
        {
            int loc = 0;
            List<double> min = arc[0];

            for (int i = 1; i < arc.Count; i++)
            {
                if (min[0] > arc[i][0] || min[0] == arc[i][0] && min[1] > arc[i][1])
                {
                    loc = i;
                    min = arc[i];
                }
            }

            return loc;
        }

        #endregion

        #region Private static quantizing methods

        /// <summary>
        /// Computes the topology transform member (scale and translate arrays).
        /// </summary>
        /// <param name="geometries">The geometries.</param>
        /// <param name="scale">The scale array.</param>
        /// <param name="translate">The translate array.</param>
        /// <param name="Q0">The first quantization factor.</param>
        /// <param name="Q1">The second quantization factor.</param>
        private static void ComputeQuantizingArrays(IList<IGeometry> geometries, double[] scale, double[] translate, double Q0, double Q1)
        {
            // we need the bounding box for all geometries
            // get the minimum and the maximum X and Y coordinates.
            double minX = geometries[0].Envelope.MinX;
            double maxX = geometries[0].Envelope.MaxX;
            double minY = geometries[0].Envelope.MinY;
            double maxY = geometries[0].Envelope.MaxY;

            for (int i = 1; i < geometries.Count; i++)
            {
                if (geometries[i].Envelope.MinX < minX)
                    minX = geometries[i].Envelope.MinX;
                if (geometries[i].Envelope.MaxX > maxX)
                    maxX = geometries[i].Envelope.MaxX;

                if (geometries[i].Envelope.MinY < minY)
                    minY = geometries[i].Envelope.MinY;
                if (geometries[i].Envelope.MaxY > maxY)
                    maxY = geometries[i].Envelope.MaxY;
            }
            // compute the translate array
            translate[0] = -minX;
            translate[1] = -minY;
            //compute the scale array
            scale[0] = maxX - minX > 0 ? (Q1 - 1) / (maxX - minX) * Q0 / Q1 : 1;
            scale[1] = maxY - minY > 0 ? (Q1 - 1) / (maxY - minY) * Q0 / Q1 : 1;
        }

        /// <summary>
        /// Quantizes a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="scale">Scale array.</param>
        /// <param name="translate">Translate array.</param>
        /// <returns>The quantized geometry.</returns>
        private static IGeometry QuantizeGeometry(IGeometry geometry, double[] scale, double[] translate)
        {
            if (geometry is IPoint)
                geometry = QuantizePoint(geometry as IPoint, scale, translate);
            else if (geometry is IMultiPoint)
            {
                List<IPoint> points = new List<IPoint>();
                for (int i = 0; i < (geometry as IMultiPoint).Count; i++)
                    points.Add(QuantizePoint((geometry as IMultiPoint)[i], scale, translate));

                geometry = new MultiPoint(points, geometry.PrecisionModel, geometry.ReferenceSystem, geometry.Metadata);
            }
            else if (geometry is ILineString)
                geometry = QuantizeLine(geometry as ILineString, scale, translate);
            else if (geometry is IMultiLineString)
            {
                List<ILineString> lines = new List<ILineString>();
                for (int i = 0; i < (geometry as IMultiLineString).Count; i++)
                    lines.Add(QuantizeLine((geometry as IMultiLineString)[i], scale, translate));

                geometry = new MultiLineString(lines, geometry.PrecisionModel, geometry.ReferenceSystem, geometry.Metadata);
            }
            else if (geometry is IPolygon)
            {
                geometry = QuantizePolygon(geometry as IPolygon, scale, translate);
            }
            else if (geometry is IMultiPolygon)
            {
                List<IPolygon> ps = new List<IPolygon>();
                for (int i = 0; i < (geometry as IMultiPolygon).Count; i++)
                    ps.Add(QuantizePolygon((geometry as IMultiPolygon)[i], scale, translate));

                geometry = new MultiPolygon(ps, geometry.PrecisionModel, geometry.ReferenceSystem, geometry.Metadata);
            }
            else if (geometry is IGeometryCollection<IGeometry>)
            {
                GeometryFactory fact = new GeometryFactory(geometry.PrecisionModel, geometry.ReferenceSystem);

                List<IGeometry> list = new List<IGeometry>();
                for (int i = 0; i < (geometry as IGeometryCollection<IGeometry>).Count; i++)
                {
                    list.Add(QuantizeGeometry((geometry as IGeometryCollection<IGeometry>)[i], scale, translate));
                }

                geometry = fact.CreateGeometryCollection<IGeometry>(list, geometry.Metadata);
            }
            else throw new NotSupportedException("The following geometry type not supported by TopoJSON: " + geometry.Name);

            return geometry;
        }

        /// <summary>
        /// Quantizes a point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="scale">The scale array.</param>
        /// <param name="translate">The translate array.</param>
        /// <returns>The quantized point.</returns>
        private static IPoint QuantizePoint(IPoint point, double[] scale, double[] translate) 
        {
            point.X = Math.Round((point.X + translate[0]) * scale[0]);
            point.Y = Math.Round((point.Y + translate[1]) * scale[1]);
            return point;
        }

        /// <summary>
        /// Quantizes a coordinate.
        /// </summary>
        /// <param name="coord">The point as coordinate.</param>
        /// <param name="scale">The scale array.</param>
        /// <param name="translate">The translate array.</param>
        /// <returns>The quantized coordinate.</returns>
        private static Coordinate QuantizePoint(Coordinate coord, double[] scale, double[] translate)
        {
            return new Coordinate(Math.Round((coord.X + translate[0]) * scale[0]), Math.Round((coord.Y + translate[1]) * scale[1]), coord.Z);
        }

        /// <summary>
        /// Quantizes a linestring.
        /// </summary>
        /// <param name="line">The linestring.</param>
        /// <param name="scale">The scale array.</param>
        /// <param name="translate">The translate array.</param>
        /// <returns>The quantized linestring.</returns>
        private static ILineString QuantizeLine(ILineString line, double[] scale, double[] translate) 
        {
            GeometryFactory fact = new GeometryFactory(line.PrecisionModel, line.ReferenceSystem);

            Coordinate c = QuantizePoint(line.Coordinates[0], scale, translate);

            List<Coordinate> coords = new List<Coordinate> {c};

            for (int i = 1; i < line.Count; i++)
            {
                c = QuantizePoint(line.Coordinates[i], scale, translate);
                coords.Add(c);
            }

            return fact.CreateLineString(coords, line.Metadata);
        }

        /// <summary>
        /// Quantizes a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="scale">The scale array.</param>
        /// <param name="translate">The translate array.</param>
        /// <returns>The quantized polygon.</returns>
        private static IPolygon QuantizePolygon(IPolygon polygon, double[] scale, double[] translate)
        {
            ILineString shell = QuantizeLine(polygon.Shell, scale, translate);
            List<ILineString> holes = new List<ILineString>();
            for (int i = 0; i < polygon.HoleCount; i++)
                holes.Add(QuantizeLine(polygon.Holes[i], scale, translate));

            return new Polygon(shell, holes, polygon.PrecisionModel, polygon.ReferenceSystem, polygon.Metadata);
        }

        #endregion

        #region Private static writing methods

        /// <summary>
        /// Writes out the topology's transform member.
        /// </summary>
        /// <param name="scale">The scale array.</param>
        /// <param name="translate">The translate array.</param>
        private static void WriteTransform(double[] scale, double[] translate)
        {
            _jsonwriter.WritePropertyName("transform");
            _jsonwriter.WriteStartObject();

            _jsonwriter.WritePropertyName("scale");
            _jsonwriter.WriteStartArray();
            _jsonwriter.WriteValue(scale[0]);
            _jsonwriter.WriteValue(scale[1]);
            _jsonwriter.WriteEndArray();

            _jsonwriter.WritePropertyName("translate");
            _jsonwriter.WriteStartArray();
            _jsonwriter.WriteValue(translate[0]);
            _jsonwriter.WriteValue(translate[1]);
            _jsonwriter.WriteEndArray();

            _jsonwriter.WriteEndObject();
        }

        /// <summary>
        /// Writes out the topology's arcs.
        /// </summary>
        private static void WriteArcs()
        {
            _jsonwriter.WriteStartArray();
            for (int i = 0; i < _arcs.Count; i++)
            {
                _jsonwriter.WriteStartArray();
                for (int j = 0; j < _arcs[i].Count; j++)
                {
                    _jsonwriter.WriteStartArray();
                    for (int k = 0; k < _arcs[i][j].Count; k++)
                    {
                        _jsonwriter.WriteValue((int) _arcs[i][j][k]);
                    }
                    _jsonwriter.WriteEndArray();
                }
                _jsonwriter.WriteEndArray();
            }
            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes the Bounding Box array to TopoJSON file.
        /// </summary>
        /// <param name="envelope">The geometry's envelope.</param>
        /// <param name="is2D">The given envelope is 2 dimensional or not.</param>
        private static void WriteEnvelope(Envelope envelope, bool is2D)
        {
            _jsonwriter.WritePropertyName("bbox");
            _jsonwriter.WriteStartArray();
            if (is2D)
            {
                _jsonwriter.WriteValue(envelope.MinX); _jsonwriter.WriteValue(envelope.MinY);
                _jsonwriter.WriteValue(envelope.MaxX); _jsonwriter.WriteValue(envelope.MaxY);
            }
            else
            {
                _jsonwriter.WriteValue(envelope.MinX); _jsonwriter.WriteValue(envelope.MinY);
                _jsonwriter.WriteValue(envelope.MinZ); _jsonwriter.WriteValue(envelope.MaxX);
                _jsonwriter.WriteValue(envelope.MaxY); _jsonwriter.WriteValue(envelope.MaxZ);
            }
            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out the properties of a geometry to TopoJSON file.
        /// </summary>
        /// <param name="metadata">The metadata of a geometry.</param>
        private static void WriteProperties(IMetadataCollection metadata)
        {
            if (metadata == null || metadata.Count == 0)
                return;

            JsonSerializer serializer = new JsonSerializer();

            foreach (KeyValuePair<string, object> k in metadata)
            {
                if (k.Key != "OBJECTID")
                {
                    _jsonwriter.WritePropertyName(k.Key);
                    serializer.Serialize(_jsonwriter, k.Value);
                }
            }
        }

        /// <summary>
        /// Writes out an object of the topology to TopoJSON file.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="objectname"></param>
        private static void WriteObject(IGeometry geometry, string objectname)
        {
            _jsonwriter.WritePropertyName(objectname);
            WriteGeometry(geometry);
        }

        /// <summary>
        /// Writes out a geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        private static void WriteGeometry(IGeometry geometry)
        {
            _jsonwriter.WriteStartObject();
            if (geometry is IPoint)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Point");
                _jsonwriter.WritePropertyName("coordinates");
                WritePoint(geometry as IPoint);
            }
            else if (geometry is IMultiPoint)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiPoint");
                _jsonwriter.WritePropertyName("coordinates");
                WriteMultiPoint(geometry as IMultiPoint);
            }
            else if (geometry is ILineString)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("LineString");
                _jsonwriter.WritePropertyName("arcs");
                WriteLineString(geometry as ILineString);
            }
            else if (geometry is IMultiLineString)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiLineString");
                _jsonwriter.WritePropertyName("arcs");
                WriteMultiLineString(geometry as IMultiLineString);
            }
            else if (geometry is IPolygon)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("Polygon");
                _jsonwriter.WritePropertyName("arcs");
                WritePolygon(geometry as IPolygon);
            }
            else if (geometry is IMultiPolygon)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("MultiPolygon");
                _jsonwriter.WritePropertyName("arcs");
                WriteMultiPolygon(geometry as IMultiPolygon);
            }
            else if (geometry is IGeometryCollection<IGeometry>)
            {
                _jsonwriter.WritePropertyName("type");
                _jsonwriter.WriteValue("GeometryCollection");
                _jsonwriter.WritePropertyName("geometries");
                WriteGeometryCollection(geometry as IGeometryCollection<IGeometry>);
            }
            else
                throw new NotSupportedException("Geometry type " + geometry.Name + " is not supported by TopoJSON.");
            

            bool l1 = geometry.Metadata != null && geometry.Metadata.Count > 0;
            bool l2 = geometry.ReferenceSystem != null;
            if (l1 || l2)
            {
                bool l3 = false;
                if (l1)
                {
                    int a = 0;
                    if (geometry.Metadata.ContainsKey("OBJECTID") && geometry.Metadata["OBJECTID"] != null)
                    {
                        a = 1;
                        _jsonwriter.WritePropertyName("id");
                        _jsonwriter.WriteValue(geometry.Metadata["OBJECTID"]);
                    }

                    l3 = geometry.Metadata.Count > 0 + a;
                }

                if (l2 || l3)
                {
                    _jsonwriter.WritePropertyName("properties");
                    _jsonwriter.WriteStartObject();

                    WriteProperties(geometry.Metadata);

                    if (l2)
                    {
                        _jsonwriter.WritePropertyName("crs");
                        _jsonwriter.WriteValue(geometry.ReferenceSystem.Identifier);
                    }

                    _jsonwriter.WriteEndObject();
                }
            }

            _jsonwriter.WriteEndObject();
        }

        /// <summary>
        /// Writes out a point to TopoJSON file.
        /// </summary>
        /// <param name="point">The point.</param>
        private static void WritePoint(IPoint point)
        {
            _jsonwriter.WriteStartArray();
            _jsonwriter.WriteValue((int) point.X);
            _jsonwriter.WriteValue((int) point.Y);

            if (point.SpatialDimension == 3)
                _jsonwriter.WriteValue((int) point.Z);

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a multipoint to TopoJSON file.
        /// </summary>
        /// <param name="point">The multipoint.</param>
        private static void WriteMultiPoint(IMultiPoint point)
        {
            _jsonwriter.WriteStartArray();

            for (int i = 0; i < point.Count; i++)
            {
                WritePoint(point[i]);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a linestring to TopoJSON file.
        /// </summary>
        /// <param name="lstr">The linestring.</param>
        private static void WriteLineString(ILineString lstr)
        {
            bool is3D = lstr.SpatialDimension == 3;
            IList<int> indexes = ComputeIndexes(lstr.Coordinates, is3D);
            _jsonwriter.WriteStartArray();

            for (int i = 0; i < indexes.Count; i++)
            {
                _jsonwriter.WriteValue(indexes[i]);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a multilinestring to TopoJSON file.
        /// </summary>
        /// <param name="mlstr">The multilinestring.</param>
        private static void WriteMultiLineString(IMultiLineString mlstr)
        {
            bool is3D = mlstr.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            foreach (ILineString l in mlstr)
            {
                WriteLineString(l);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a polygon to TopoJSON file.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        private static void WritePolygon(IPolygon polygon)
        {
            bool is3D = polygon.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            WriteLineString(polygon.Shell);

            foreach (ILinearRing lr in polygon.Holes)
            {
                WriteLineString(lr);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a multipolygon to TopoJSON file.
        /// </summary>
        /// <param name="polygon">The multipolygon.</param>
        private static void WriteMultiPolygon(IMultiPolygon polygon)
        {
            bool is3D = polygon.SpatialDimension == 3;
            _jsonwriter.WriteStartArray();

            foreach (IPolygon lr in polygon)
            {
                WritePolygon(lr);
            }

            _jsonwriter.WriteEndArray();
        }

        /// <summary>
        /// Writes out a geometrycollection to TopoJSON file.
        /// </summary>
        /// <param name="collection">The collection.</param>
        private static void WriteGeometryCollection(IGeometryCollection<IGeometry> collection)
        {
            _jsonwriter.WriteStartArray();

            foreach (IGeometry geometry in collection)
            {
                WriteGeometry(geometry);
            }
            _jsonwriter.WriteEndArray();
        }

        #endregion

        /// <summary>
        /// Computes the and creates the arc index list of a LineString (indexes from the topology's arcs).
        /// </summary>
        /// <param name="coords">The coordinates.</param>
        /// <param name="is3D">The given coordinates are 3 dimensionals or not.</param>
        /// <returns></returns>
        private static IList<int> ComputeIndexes(IList<Coordinate> coords, bool is3D)
        {
            List<int> indexes = new List<int>();

            bool l = false;
            int startIndex = 0;
            for (int i = 0; i < _arcs.Count && !l; i++)
            {
                int prev = startIndex;
                if (IsMatchingArcs(coords, _arcs[i], is3D, ref startIndex))
                {
                    indexes.Add(i);
                    i = -1;
                    if (startIndex == coords.Count)
                        l = true;
                    else
                        startIndex--;
                }
                else
                {
                    startIndex = prev;
                    if (IsMatchingArcs(coords, _arcs[i].AsEnumerable().Reverse().ToList(), is3D, ref startIndex))
                    {
                        indexes.Add(i * -1 - 1);
                        i = -1;
                        if (startIndex == coords.Count)
                            l = true;
                        else
                            startIndex--;
                    }
                    else
                        startIndex = prev;
                }
            }
            // if we haven't found any arcs matching, create a new one
            // in normal case, this operation isn't needed
            if (!l)
            {
                List<List<double>> arc = new List<List<double>>();
                for (int i = startIndex; i < coords.Count; i++)
                {
                    List<double> coord = new List<double>() { coords[i].X, coords[i].Y };
                    if (is3D)
                        coord.Add(coords[i].Z);

                    arc.Add(coord);
                }

                _arcs.Add(arc);

                indexes.Add(_arcs.Count - 1);
            }

            return indexes;
        }

        /// <summary>
        /// Determines whether two arcs are the same or not.
        /// </summary>
        /// <param name="coords">The first arc.</param>
        /// <param name="arc">The second arc.</param>
        /// <param name="is3D">The given arcs are 3 dimensionals or not.</param>
        /// <param name="startIndex">The starting index from where we want to start the check in the coords list.</param>
        /// <returns></returns>
        private static bool IsMatchingArcs(IList<Coordinate> coords, List<List<double>> arc, bool is3D, ref int startIndex)
        {
            for (int j = 0; j < arc.Count; j++)
            {
                if (coords.Count > startIndex)
                {
                    List<double> coord = new List<double>() { coords[startIndex].X, coords[startIndex].Y };
                    if (is3D)
                        coord.Add(coords[startIndex].Z);

                    if (coord.SequenceEqual(arc[j]))
                        startIndex++;
                    else
                    {
                        startIndex = startIndex - j + 1;
                        return false;
                    }
                }
                else
                {
                    startIndex = startIndex - j + 1;
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}