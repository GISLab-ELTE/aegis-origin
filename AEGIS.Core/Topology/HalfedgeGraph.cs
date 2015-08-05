/// <copyright file="HalfedgeGraph.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.Topology
{
    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    /// <remarks>
    /// The implementation of the core topological model was inspired by Alexander Kolliopoulos's <a href="http://www.dgp.toronto.edu/~alexk/">Lydos library</a>.
    /// </remarks>
    public sealed partial class HalfedgeGraph : IHalfedgeGraph
    {
        #region Private fields

        /// <summary>
        /// Stores the collection of halfedges in the graph.
        /// </summary>
        private List<Halfedge> _halfedges = new List<Halfedge>();

        /// <summary>
        /// Stores the collection of vertices in the graph.
        /// </summary>
        private VertexCollection _vertices = new VertexCollection();

        /// <summary>
        /// Stores the collection of edges in the graph.
        /// </summary>
        private List<Edge> _edges = new List<Edge>();

        /// <summary>
        /// Stores the collection of faces in the graph.
        /// </summary>
        private List<Face> _faces = new List<Face>();

        private Tag _currentTag;

        #endregion

        #region IHalfedgeGraph properties

        /// <summary>
        /// Gets the collection of halfedges in the graph.
        /// </summary>
        public IEnumerable<IHalfedge> Halfedges
        {
            get { return _halfedges.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the collection of vertices in the graph.
        /// </summary>
        public IEnumerable<IVertex> Vertices
        {
            get { return _vertices.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the collection of edges in the graph.
        /// </summary>
        public IEnumerable<IEdge> Edges
        {
            get { return _edges.AsReadOnly(); }
        }

        /// <summary>
        /// Gets the collection of faces in the graph.
        /// </summary>
        public IEnumerable<IFace> Faces
        {
            get { return _faces.AsReadOnly(); }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Merges another graph into the current instance.
        /// </summary>
        /// <param name="other">The other graph.</param>
        /// <exception cref="System.ArgumentNullException">The other graph is null.</exception>
        public void MergeGraph(IHalfedgeGraph other)
        {
            if (other == null)
                throw new ArgumentNullException("other", "The other graph is null.");

            Retag(Tag.A);
            foreach (var oFace in other.Faces)
            {
                IList<Coordinate> shellPositions = oFace.Vertices.Select(vertex => vertex.Position).ToList();
                IList<IList<Coordinate>> holePositions = oFace.Holes != null
                                                                   ? oFace.Holes.Select(
                                                                       face => face.Vertices.Select(vertex => vertex.Position).ToList() as IList<Coordinate>).ToList()
                                                                   : null;

                MergeFace(shellPositions, holePositions, true);
            }
        }

        #endregion

        #region IHalfedgeGraph methods

        /// <summary>
        /// Removes all elements from the graph.
        /// </summary>
        public void Clear()
        {
            _edges.Clear();
            _faces.Clear();
            _halfedges.Clear();
            _vertices.Clear();
        }

        /// <summary>
        /// Trims internal data structures to their current size.
        /// </summary>
        /// <remarks>
        /// Call this method to prevent excess memory usage when the graph is done being built.
        /// </remarks>
        public void TrimExcess()
        {
            _edges.TrimExcess();
            _faces.TrimExcess();
            _halfedges.TrimExcess();
        }

        /// <summary>
        /// Checks halfedge connections to verify that a valid topology graph was constructed.
        /// </summary>
        /// <remarks>
        /// Checking for proper topology in every case when a face is added would slow down
        /// graph construction significantly, so this method may be called once when a graph
        /// is complete to ensure that topology is manifold (with boundary).
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the graph is non-manifold.</exception>
        public void VerifyTopology()
        {
            foreach (Halfedge halfedge in _halfedges)
            {
                if (halfedge != halfedge.Opposite.Opposite)
                    throw new InvalidOperationException("A halfedge's opposite's opposite is not itself.");

                if (halfedge.Edge != halfedge.Opposite.Edge)
                    throw new InvalidOperationException("Opposite halfedges do not belong to the same edge.");

                if (halfedge.ToVertex.Halfedge.Opposite.ToVertex != halfedge.ToVertex)
                    throw new InvalidOperationException("The halfedge-vertex mapping is corrupted.");

                if (halfedge.Previous.Next != halfedge)
                    throw new InvalidOperationException("A halfedge's previous next is not itself.");

                if (halfedge.Next.Previous != halfedge)
                    throw new InvalidOperationException("A halfedge's next previous is not itself.");

                if (halfedge.Next.Face != halfedge.Face)
                    throw new InvalidOperationException("Adjacent halfedges do not belong to the same face.");

                // Make sure each halfedge is reachable from the vertex it originates from.
                if (!halfedge.FromVertex.Halfedges.Contains(halfedge))
                    throw new InvalidOperationException("A halfedge is not reachable from the vertex it originates from.");
            }
        }

        /// <summary>
        /// Adds an (isolated) vertex to the graph.
        /// </summary>
        /// <remarks>
        /// When a vertex already exists at the given position, it will be returned instead of creating a new one.
        /// </remarks>
        /// <param name="position">The position of the vertex.</param>
        /// <returns>The vertex created by this method.</returns>
        public IVertex AddVertex(Coordinate position)
        {
            return GetVertex(position);
        }

        /// <summary>
        /// Adds a face to the graph.
        /// </summary>
        /// <remarks>
        /// Please note, that for this method the position of the holes' vertices must also be given in counter-clockwise order.
        /// </remarks>
        /// <param name="shell">The position of the face's vertices in counter-clockwise order.</param>
        /// <param name="holes">The position of the holes' vertices in counter-clockwise order.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddFace(IBasicPolygon polygon)
        {
            return AddFace(polygon.Shell, polygon.Holes);
        }

        /// <summary>
        /// Adds a face to the graph.
        /// </summary>
        /// <remarks>
        /// Please note, that for this method the position of the holes' vertices must also be given in counter-clockwise order.
        /// </remarks>
        /// <param name="shell">The position of the face's vertices in counter-clockwise order.</param>
        /// <param name="holes">The position of the holes' vertices in counter-clockwise order.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddFace(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes = null)
        {
            Face shellFace = CreateFace(shell.Select(GetVertex).ToArray());
            if (holes != null)
                foreach (IList<Coordinate> hole in holes)
                {
                    Face holeFace = CreateFace(hole.Select(GetVertex).ToArray());
                    holeFace.Parent = shellFace;
                    shellFace.Holes.Add(holeFace);
                }
            return shellFace;
        }

        /// <summary>
        /// Adds a face to the graph.
        /// </summary>
        /// <remarks>
        /// Please note, that for this method the position of the holes' vertices must also be given in counter-clockwise order.
        /// </remarks>
        /// <param name="shell">The position of the face's vertices in counter-clockwise order.</param>
        /// <param name="holes">The position of the holes' vertices in counter-clockwise order.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddFace(IBasicLineString shell, IEnumerable<IBasicLineString> holes = null)
        {
            Face shellFace = CreateFace(shell.Select(GetVertex).ToArray());
            if (holes != null)
                foreach (IBasicLineString hole in holes)
                {
                    Face holeFace = CreateFace(hole.Select(GetVertex).ToArray());
                    holeFace.Parent = shellFace;
                    shellFace.Holes.Add(holeFace);
                }
            return shellFace;
        }

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <remarks>
        /// The algorithm may be forced by the <see cref="forced"/> parameter to remove the adjacent faces of the vertex.
        /// </remarks>
        /// <param name="position">The position of the vertex to remove.</param>
        /// <param name="forced">Force the method to remove all adjacent faces when the vertex is not isolated.</param>
        /// <returns><c>true</c> when the coordinate to remove exists in the graph; otherwise <c>false</c>.</returns>
        public Boolean RemoveVertex(Coordinate position, Boolean forced = false)
        {
            if (!_vertices.Contains(position)) return false;
            RemoveVertex(_vertices[position], forced);
            return true;
        }

        #endregion

        #region IGeometry support methods

        /// <summary>
        /// Adds a (supported type of) geometry to the graph.
        /// </summary>
        /// <remarks>
        /// The supported types are <see cref="IPoint"/>, <see cref="ILinearRing"/>, <see cref="IPolygon"/>, <see cref="IMultiPoint"/>, and <see cref="IMultiPolygon"/>.
        /// </remarks>
        /// <param name="geometry">The geometry to add.</param>
        public void AddGeometry(IGeometry geometry)
        {
            if (geometry is IPoint)
                AddPoint(geometry as IPoint);
            else if (geometry is ILinearRing)
                AddLinearRing(geometry as ILinearRing);
            else if (geometry is IPolygon)
                AddPolygon(geometry as IPolygon);
            else if (geometry is IMultiPoint)
                AddMultiPoint(geometry as IMultiPoint);
            else if (geometry is IMultiPolygon)
                AddMultiPolygon(geometry as IMultiPolygon);
        }

        /// <summary>
        /// Adds an (isolated) point to the graph.
        /// </summary>
        /// <remarks>
        /// When a point already exists at the given position, it will be returned instead of creating a new one.
        /// </remarks>
        /// <param name="point">The point to add.</param>
        /// <returns>The vertex created by this method.</returns>
        public IVertex AddPoint(IPoint point)
        {
            return AddVertex(point.Coordinate);
        }

        /// <summary>
        /// Adds a linear ring to the graph.
        /// </summary>
        /// <param name="linearRing">The linear ring to add.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddLinearRing(ILinearRing linearRing)
        {
            return AddFace(linearRing.Take(linearRing.Count - 1).ToArray());
        }

        /// <summary>
        /// Adds a polygon to the graph.
        /// </summary>
        /// <param name="polygon">The polygon to add.</param>
        /// <returns>The face created by this method.</returns>
        public IFace AddPolygon(IPolygon polygon)
        {
            return AddFace(polygon.Shell.Take(polygon.Shell.Count - 1).ToArray(),
                           polygon.Holes.Select(hole => hole.Take(hole.Count - 1).ToArray()));
        }

        /// <summary>
        /// Adds multiple (isolated) points to the graph.
        /// </summary>
        /// <param name="multiPoint">The points to add.</param>
        /// <returns>The vertices created by this method.</returns>
        public IVertex[] AddMultiPoint(IMultiPoint multiPoint)
        {
            var vertices = new IVertex[multiPoint.Count];
            for (Int32 i = 0; i < multiPoint.Count; ++i)
                vertices[i] = AddPoint(multiPoint[i]);
            return vertices;
        }

        /// <summary>
        /// Adds multiple polygons to the graph.
        /// </summary>
        /// <param name="multiPolygon">The polygons to add.</param>
        /// <returns>The faces created by this method.</returns>
        public IFace[] AddMultiPolygon(IMultiPolygon multiPolygon)
        {
            var faces = new IFace[multiPolygon.Count];
            for (Int32 i = 0; i < multiPolygon.Count; ++i)
                faces[i] = AddPolygon(multiPolygon[i]);
            return faces;
        }

        /// <summary>
        /// Merges a (supported type of) geometry into the graph.
        /// </summary>
        /// <param name="geometry">The geometry to merge.</param>
        /// <exception cref="System.ArgumentException">The specified geometry type is not supported.</exception>
        /// <remarks>The supported types are <see cref="ILinearRing" />, <see cref="IPolygon" />, and <see cref="IMultiPolygon" />.</remarks>
        public void MergeGeometry(IGeometry geometry)
        {
            if (geometry is IPoint)
                AddPoint(geometry as IPoint);
            else if (geometry is ILinearRing)
                MergeLinearRing(geometry as ILinearRing);
            else if (geometry is IPolygon)
                MergePolygon(geometry as IPolygon);
            else if (geometry is IMultiPoint)
                AddMultiPoint(geometry as IMultiPoint);
            else if (geometry is IMultiPolygon)
                MergeMultiPolygon(geometry as IMultiPolygon);

            throw new ArgumentException("The specified geometry type is not supported.");
        }

        /// <summary>
        /// Merges a linear ring into the graph.
        /// </summary>
        /// <param name="linearRing">The linear ring to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        public IFace[] MergeLinearRing(ILinearRing linearRing)
        {
            return MergeFace(linearRing.Take(linearRing.Count - 1).ToArray());
        }

        /// <summary>
        /// Merges a polygon into the graph.
        /// </summary>
        /// <param name="polygon">The polygon to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        public IFace[] MergePolygon(IPolygon polygon)
        {
            return MergeFace(polygon.Shell.Take(polygon.Shell.Count - 1).ToList(),
                             polygon.Holes.Select(hole => hole.Take(hole.Count - 1).ToList() as IList<Coordinate>).ToList());
        }

        /// <summary>
        /// Merges multiple polygons into the graph.
        /// </summary>
        /// <param name="multiPolygon">The polygons to merge.</param>
        /// <returns>The new faces created by this method.</returns>
        public IFace[] MergeMultiPolygon(IMultiPolygon multiPolygon)
        {
            var faces = new List<IFace>(multiPolygon.Count);
            foreach (var polygon in multiPolygon)
                faces.AddRange(MergePolygon(polygon));
            return faces.ToArray();
        }

        /// <summary>
        /// Converts the graph to a collection of geometries.
        /// </summary>
        /// <param name="factory">The geometry factory used to produce the polygon.</param>
        /// <returns>A collection of geometries representing the topology graph.</returns>
        public IGeometry ToGeometry(IGeometryFactory factory = null)
        {
            if (factory == null)
                factory = FactoryRegistry.GetFactory<IGeometryFactory>();

            List<IGeometry> resultCollection = new List<IGeometry>();

            foreach (Face face in _faces)
            {
                if (!face.IsHole)
                    resultCollection.Add(face.ToGeometry(factory));
            }

            if (resultCollection.Count == 0)
                return null;

            if (resultCollection.Count == 1)
                return resultCollection[0];

            return factory.CreateGeometryCollection(resultCollection);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns a string representing the connections of vertices for each face in the graph.
        /// </summary>
        /// <returns>The position for each vertex of each face on a line of the string.</returns>
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var face in _faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    sb.Append(vertex.Position);
                    sb.Append(" -> ");
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Merges the specified coordinates into the graph.
        /// </summary>
        /// <param name="shellPositions">The coordinates of the shell.</param>
        /// <param name="holePositions">The coordinates of the holes.</param>
        /// <param name="tagging">A value indicating whether tagging should be performed.</param>
        /// <returns>The array of faces creates by the method.</returns>
        private IFace[] MergeFace(IBasicLineString shellPositions, IEnumerable<IBasicLineString> holePositions = null, Boolean tagging = false)
        {
            return MergeFace(shellPositions.Coordinates, holePositions == null ? null : holePositions.Select(hole => hole.Coordinates));
        }

        /// <summary>
        /// Merges the specified coordinates into the graph.
        /// </summary>
        /// <param name="shellPositions">The coordinates of the shell.</param>
        /// <param name="holePositions">The coordinates of the holes.</param>
        /// <param name="tagging">A value indicating whether tagging should be performed.</param>
        /// <returns>The array of faces creates by the method.</returns>
        private IFace[] MergeFace(IList<Coordinate> shellPositions, IEnumerable<IList<Coordinate>> holePositions = null, Boolean tagging = false)
        {
            // Retrieve the vertex positions of the shell faces.
            var shellFaces = _faces.Where(face => !face.IsHole).ToList();
            var facePositions = new Dictionary<Int32, IList<Coordinate>>(shellFaces.Count);
            foreach (var face in shellFaces)
                facePositions.Add(face.Index, face.Vertices.Select(vertex => vertex.Position).ToList());

            // Determine the possible faces that collide with the parameter face.
            Face[] collisionFaces = _faces.Where(face => BentleyOttmannAlgorithm.Intersection(new[]
            {
                facePositions[face.Index],
                shellPositions
            }).Count > 0).ToArray();

            IList<IBasicPolygon> clipsInternal = new List<IBasicPolygon>();  // internal, common clips
            IList<IBasicPolygon> clipsExternalOld = new List<IBasicPolygon>(); // external clips of the already existing topology graph
            IList<IBasicPolygon> clipsExternalNew = new List<IBasicPolygon>(); // external clips of the parameter face that are ready to be added to the the graph
            IList<IBasicPolygon> clipsExternalRecursive = new List<IBasicPolygon>(); // external clips of the parameter face that are required to be re-processed

            // If there were any colliding faces, process the first one.
            if (collisionFaces.Length > 0)
            {
                // Calculate the the internal and external clips with the colliding the faces.
                var currentShellPositions = facePositions[collisionFaces[0].Index];
                var currentHolePositions = collisionFaces[0].Holes != null
                                               ? collisionFaces[0].Holes.Select(face =>
                                                                                face.Vertices.Select(vertex =>
                                                                                                     vertex.Position).ToList() as
                                                                                IList<Coordinate>).ToList()
                                               : null;
                var algorithm = new GreinerHormannAlgorithm(currentShellPositions, /*currentHolePositions,*/
                                                            shellPositions/*, holePositions*/);

                // Internal clips.
                clipsInternal = algorithm.InternalPolygons;

                // External clips of the already existing topology graph.
                foreach (IBasicPolygon clip in algorithm.ExternalFirstPolygons)
                {
                    var otherCollisionFaces =
                        collisionFaces.Where(
                                             face =>
                                             GreinerHormannAlgorithm.Clip(facePositions[face.Index], clip.Shell.Coordinates).Count > 0)
                                      .ToArray();
                    if (otherCollisionFaces.Length == 0 ||
                        otherCollisionFaces.Length == 1 && otherCollisionFaces[0] == collisionFaces[0])
                        clipsExternalOld.Add(clip);
                }

                // External clips of the parameter face ...
                foreach (IBasicPolygon clip in algorithm.ExternalSecondPolygons)
                {
                    var otherCollisionFaces =
                        collisionFaces.Where(
                                             face =>
                                             GreinerHormannAlgorithm.Clip(facePositions[face.Index], clip.Shell.Coordinates).Count > 0)
                                      .ToArray();
                    if (otherCollisionFaces.Length == 0 ||
                        otherCollisionFaces.Length == 1 && otherCollisionFaces[0] == collisionFaces[0])
                        clipsExternalNew.Add(clip); // ... that are ready to be added to the the graph.
                    else
                        clipsExternalRecursive.Add(clip); // ... that are required to be re-processed.
                }

                // Remove the processed collided face from the graph, because the new clips will be added.
                RemoveFace(collisionFaces[0]);
            }
            else // If there were none colliding faces, the whole face can be added to the graph.
                clipsExternalNew.Add(new BasicPolygon(shellPositions, holePositions));

            var newFaces = new List<IFace>(clipsInternal.Count + clipsExternalOld.Count + clipsExternalNew.Count);
            if (tagging) _currentTag = Tag.Both;
            newFaces.AddRange(clipsInternal.Select(operands => AddFace(operands.Shell, operands.Holes)));
            if (tagging) _currentTag = Tag.A;
            newFaces.AddRange(clipsExternalOld.Select(operands => AddFace(operands.Shell, operands.Holes)));
            if (tagging) _currentTag = Tag.B;
            newFaces.AddRange(clipsExternalNew.Select(operands => AddFace(operands.Shell, operands.Holes)));
            if (tagging) _currentTag = Tag.None;
            foreach (var polygon in clipsExternalRecursive)
                newFaces.AddRange(MergeFace(polygon.Shell, polygon.Holes, tagging));
            return newFaces.ToArray();
        }

        /// <summary>
        /// Adds a halfedge to the halfedge list.
        /// </summary>
        /// <param name="halfedge">The halfedge to add.</param>
        private void AppendToHalfedgeList(Halfedge halfedge)
        {
            halfedge.Index = _halfedges.Count;
            _halfedges.Add(halfedge);
        }

        /// <summary>
        /// Adds a vertex to the vertex list.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        private void AppendToVertexList(Vertex vertex)
        {
            vertex.Index = _vertices.Count;
            vertex.Graph = this;
            vertex.Tag = _currentTag;
            _vertices.Add(vertex);
        }

        /// <summary>
        /// Adds an edge to the edge list.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        private void AppendToEdgeList(Edge edge)
        {
            edge.Index = _edges.Count;
            _edges.Add(edge);
        }

        /// <summary>
        /// Adds a face to the face list.
        /// </summary>
        /// <param name="face">The face to add.</param>
        private void AppendToFaceList(Face face)
        {
            face.Index = _faces.Count;
            _faces.Add(face);
        }

        /// <summary>
        /// Removes a halfedge from the halfedge list.
        /// </summary>
        /// <param name="halfedge">The halfedge to remove.</param>
        private void RemoveFromHalfedgeList(Halfedge halfedge)
        {
            foreach (var item in _halfedges.Where(item => item.Index > halfedge.Index))
                item.Index -= 1;
            _halfedges.Remove(halfedge);
        }

        /// <summary>
        /// Removes a vertex from the vertex list.
        /// </summary>
        /// <param name="vertex">The vertex to remove.</param>
        private void RemoveFromVertexList(Vertex vertex)
        {
            foreach (var item in _vertices.Where(item => item.Index > vertex.Index))
                item.Index -= 1;
            _vertices.Remove(vertex);
        }

        /// <summary>
        /// Removes an edge from the edge list.
        /// </summary>
        /// <param name="edge">The edge to remove.</param>
        private void RemoveFromEdgeList(Edge edge)
        {
            foreach (var item in _edges.Where(item => item.Index > edge.Index))
                item.Index -= 1;
            _edges.Remove(edge);
        }

        /// <summary>
        /// Removes a face from the face list.
        /// </summary>
        /// <param name="face">The face to remove.</param>
        private void RemoveFromFaceList(Face face)
        {
            foreach (var item in _faces.Where(item => item.Index > face.Index))
                item.Index -= 1;
            _faces.Remove(face);
        }

        /// <summary>
        /// Gets an existing vertex by position or creates a new vertex in the graph.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <returns>The vertex and the given position.</returns>
        private Vertex GetVertex(Coordinate position)
        {
            try
            {
                _vertices[position].Tag |= _currentTag;
                return _vertices[position];
            }
            catch (KeyNotFoundException)
            {
                return CreateVertex(position);
            }
        }

        /// <summary>
        /// Creates a new, isolated vertex in the graph.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <returns>The vertex created by this method.</returns>
        private Vertex CreateVertex(Coordinate position)
        {
            var vertex = new Vertex(position);
            AppendToVertexList(vertex);
            return vertex;
        }

        /// <summary>
        /// Creates a new face in the graph.
        /// </summary>
        /// <param name="vertices">The vertices of the face in counter-clockwise order.</param>
        /// <returns>The face created by this method.</returns>
        /// <exception cref="ArgumentNullException">Thrown when a null vertex is given.</exception>
        /// <exception cref="ArgumentException">Thrown when fewer than three vertices are given or an inconvenient vertex is given.</exception>
        /// <exception cref="InvalidOperationException">Thrown when cannot form a valid face with the given vertices and the existing topology.</exception>
        private Face CreateFace(params Vertex[] vertices)
        {
            #region Initalization

            Int32 n = vertices.Length;

            // Require at least 3 vertices.
            if (n < 3)
                throw new ArgumentException("Cannot create a polygon with fewer than three vertices.", "vertices");

            Halfedge[] halfedges = new Halfedge[n];
            Boolean[] isNewEdge = new Boolean[n];
            Boolean[] isIsolatedVertex = new Boolean[n];

            #endregion

            #region Input validation

            // Make sure input is (mostly) acceptable before making any changes to the graph.
            for (Int32 i = 0; i < n; ++i)
            {
                if (vertices[i] == null)
                    throw new ArgumentNullException("vertices", "Can't add a null vertex to a face.");
                if (!vertices[i].OnBoundary)
                    throw new ArgumentException("Can't add an edge to a vertex on the interior of a graph.", "vertices");

                // Calculate the index of the following vertex.
                Int32 j = (i + 1) % n;

                // Find existing halfedges for this face.
                halfedges[i] = vertices[i].FindHalfedgeTo(vertices[j]);
                isNewEdge[i] = halfedges[i] == null;
                isIsolatedVertex[i] = vertices[i].Halfedge == null;

                if (!isNewEdge[i] && !halfedges[i].OnBoundary)
                    throw new InvalidOperationException("Can't add more than two faces to an edge.");
            }

            #endregion

            #region Create the face and the new edges

            // Create face.
            Face face = new Face();
            AppendToFaceList(face);

            // Create new edges.
            for (Int32 i = 0; i < n; ++i)
            {
                if (isNewEdge[i])
                {
                    // Calculate the index of the following vertex.
                    Int32 j = (i + 1) % n;

                    // Create new edge.
                    Edge edge = new Edge();
                    AppendToEdgeList(edge);

                    // Create new halfedges.
                    halfedges[i] = new Halfedge();
                    AppendToHalfedgeList(halfedges[i]);

                    halfedges[i].Opposite = new Halfedge();
                    AppendToHalfedgeList(halfedges[i].Opposite);

                    // Connect opposite halfedge to inner halfedge.
                    halfedges[i].Opposite.Opposite = halfedges[i];

                    // Connect edge to halfedges.
                    edge.HalfedgeA = halfedges[i];

                    // Connect half edges to edge.
                    halfedges[i].Edge = edge;
                    halfedges[i].Opposite.Edge = edge;

                    // Connect halfedges to vertices.
                    halfedges[i].ToVertex = vertices[j];
                    halfedges[i].Opposite.ToVertex = vertices[i];

                    // Connect vertex to outgoing halfedge if it doesn't have one yet.
                    if (isIsolatedVertex[i])
                        vertices[i].Halfedge = halfedges[i];
                }

                if (halfedges[i].Face != null)
                    throw new InvalidOperationException("An inner halfedge already has a face assigned to it.");

                // Connect inner halfedge to face.
                halfedges[i].Face = face;
            }

            #endregion

            #region Adjust halfedge connections

            // Connect next/previous halfedges.
            for (Int32 i = 0; i < n; ++i)
            {
                // Calculate the index of the following vertex.
                Int32 j = (i + 1) % n;

                // Outer halfedges
                if (isNewEdge[i] && isNewEdge[j] && !isIsolatedVertex[j]) // Both edges are new and vertex has faces connected already.
                {
                    // Find the closing halfedge of the first available opening.
                    Halfedge closeHalfedge = vertices[j].Halfedges.First(h => h.Face == null);
                    Halfedge openHalfedge = closeHalfedge.Previous;

                    // Link new outer halfedges into this opening.
                    halfedges[i].Opposite.Previous = openHalfedge;
                    openHalfedge.Next = halfedges[i].Opposite;
                    halfedges[j].Opposite.Next = closeHalfedge;
                    closeHalfedge.Previous = halfedges[j].Opposite;
                }
                else if (isNewEdge[i] && isNewEdge[j]) // Both edges are new.
                {
                    halfedges[i].Opposite.Previous = halfedges[j].Opposite;
                    halfedges[j].Opposite.Next = halfedges[i].Opposite;
                }
                else if (isNewEdge[i] && !isNewEdge[j]) // This is new, next is old.
                {
                    halfedges[i].Opposite.Previous = halfedges[j].Previous;
                    halfedges[j].Previous.Next = halfedges[i].Opposite;
                }
                else if (!isNewEdge[i] && isNewEdge[j]) // This is old, next is new.
                {
                    halfedges[i].Next.Previous = halfedges[j].Opposite;
                    halfedges[j].Opposite.Next = halfedges[i].Next;
                }
                else if (!isNewEdge[i] && !isNewEdge[j] && halfedges[i].Next != halfedges[j]) // Relink faces before adding new edges if they are in the way of a new face.
                {
                    Halfedge closeHalfedge = halfedges[i].Opposite;

                    // Find the closing halfedge of the opening opposite the opening halfedge i is on.
                    do
                    {
                        closeHalfedge = closeHalfedge.Previous.Opposite;
                    } while (closeHalfedge.Face != null &&
                        closeHalfedge != halfedges[j] && closeHalfedge != halfedges[i].Opposite);

                    if (closeHalfedge == halfedges[j] || closeHalfedge == halfedges[i].Opposite)
                        throw new InvalidOperationException("Unable to find an opening to relink an existing face.");

                    Halfedge openHalfedge = closeHalfedge.Previous;

                    // Remove group of faces between two openings, close up gap to form one opening.
                    openHalfedge.Next = halfedges[i].Next;
                    halfedges[i].Next.Previous = openHalfedge;

                    // Insert group of faces into target opening.
                    halfedges[j].Previous.Next = closeHalfedge;
                    closeHalfedge.Previous = halfedges[j].Previous;
                }

                // Inner halfedges.
                halfedges[i].Next = halfedges[j];
                halfedges[j].Previous = halfedges[i];
            }

            #endregion

            #region Finalization

            // Connect face to an inner halfedge.
            face.Halfedge = halfedges[0];
            return face;

            #endregion
        }

        /// <summary>
        /// Removes a vertex from the graph.
        /// </summary>
        /// <param name="vertex">The vertex to remove from the graph.</param>
        /// <param name="forced">Force the method to remove all adjacent faces when the vertex is not isolated.</param>
        /// <exception cref="System.ArgumentException">The given vertex is not located in the current graph.</exception>
        /// <exception cref="System.InvalidOperationException">The selected vertex is not isolated, force is required to remove entire face.</exception>
        /// <remarks>The algorithm may be forced by the <see cref="forced" /> parameter to remove the adjacent faces of the vertex.</remarks>
        private void RemoveVertex(Vertex vertex, Boolean forced = false)
        {
            if (vertex.Graph != this)
                throw new ArgumentException("The given vertex is not located in the current graph.", "vertex");

            if (vertex.Halfedge == null)
                RemoveFromVertexList(vertex);
            else if (forced)
            {
                foreach (Face face in vertex.Faces.ToList())
                    RemoveFace(face, false);
                RemoveFromVertexList(vertex);
            }
            else
                throw new InvalidOperationException("The selected vertex is not isolated, force is required to remove entire face.");
        }

        /// <summary>
        /// Removes a face from the graph.
        /// </summary>
        /// <param name="face">The face to remove.</param>
        /// <param name="clean">The remove operation is clean when the remaining isolated vertices are removed from the graph.</param>
        private void RemoveFace(Face face, Boolean clean = true)
        {
            IEnumerable<Vertex> removeVertices = face.Vertices.Where(f => f.Faces.Count() == 1).ToList();

            foreach (Halfedge halfedge in face.Halfedges.ToList())
            {
                halfedge.Face = null;
                if (halfedge.Opposite.Face == null)
                {
                    if (halfedge.Previous.Opposite.Face != null)
                    {
                        halfedge.Previous.Next = halfedge.Opposite.Next;
                        halfedge.Opposite.Next.Previous = halfedge.Previous;
                    }
                    if (halfedge.Next.Opposite.Face != null)
                    {
                        halfedge.Next.Previous = halfedge.Opposite.Previous;
                        halfedge.Opposite.Previous.Next = halfedge.Next;
                    }

                    if (halfedge.FromVertex.Halfedge == halfedge)
                        halfedge.FromVertex.Halfedge = halfedge.FromVertex.Halfedges.FirstOrDefault(h => h.Face != face && h.Face != null);
                    if (halfedge.ToVertex.Halfedge == halfedge.Opposite)
                        halfedge.ToVertex.Halfedge = halfedge.ToVertex.Halfedges.FirstOrDefault(h => h.Face != face && h.Face != null);

                    RemoveFromEdgeList(halfedge.Edge);
                    RemoveFromHalfedgeList(halfedge);
                    RemoveFromHalfedgeList(halfedge.Opposite);
                }
            }

            foreach (Vertex vertex in removeVertices)
            {
                vertex.Halfedge = null;
                if (clean)
                    RemoveVertex(vertex);
            }
            RemoveFromFaceList(face);
        }

        /// <summary>
        /// Replaces all tags in the graph with the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        private void Retag(Tag tag)
        {
            _currentTag = tag;
            foreach (var vertex in _vertices)
                vertex.Tag = tag;
        }

        #endregion
    }
}
