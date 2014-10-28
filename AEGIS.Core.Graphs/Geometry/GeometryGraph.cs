/// <copyright file="GeometryGraph.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Geometry
{
    /// <summary>
    /// Represents a graph form of geometry.
    /// </summary>
    public class GeometryGraph : IGeometryGraph
    {
        #region Public types

        /// <summary>
        /// Represents a graph enumerator implementing breath-first search.
        /// </summary>
        public sealed class BreadthFirstEnumerator : IEnumerator<IGraphVertex>
        {
            #region Private fields

            private GeometryGraph _localGraph;
            private Int32 _localVersion;
            private Queue<IGraphVertex> _queue;
            private HashSet<IGraphVertex> _finishedVertices;
            private IGraphVertex _current;
            private Boolean _disposed;
           
            #endregion

            #region IEnumerator properties
           
            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.</value>
            public IGraphVertex Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>The element at the current position of the enumerator.</value>
            object IEnumerator.Current
            {
                get { return _current; }
            }
            
            #endregion

            #region Constructors and destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="BreadthFirstEnumerator" /> class.
            /// </summary>
            /// <param name="heap">The graph.</param>
            internal BreadthFirstEnumerator(GeometryGraph graph)
            {
                _localGraph = graph;
                _localVersion = graph._version;
                _queue = new Queue<IGraphVertex>();
                _finishedVertices = new HashSet<IGraphVertex>();
                _disposed = false;

                Reset();
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="BreadthFirstEnumerator" /> class.
            /// </summary>
            ~BreadthFirstEnumerator()
            {
                Dispose(false);
            }
           
            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localGraph._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_finishedVertices.Count == _localGraph._vertexList.Count)
                {
                    _current = null;
                    return false;
                }

                if (_queue.Count == 0){
                    for (Int32 i = 0; i < _localGraph._vertexList.Count; i++)
                    {
                        if (!_finishedVertices.Contains(_localGraph._vertexList[i]))
                        {
                            _queue.Enqueue(_localGraph._vertexList[i]);
                            break;
                        }
                    }
                }

                _current = _queue.Dequeue();
                _finishedVertices.Add(_current);

                foreach (IGraphEdge edge in _localGraph.OutEdges(_current))
                {
                    if (!_finishedVertices.Contains(edge.Target))
                    {
                        _queue.Enqueue(edge.Target);
                    }
                }

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localGraph._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _queue.Clear();
                _finishedVertices.Clear();
                _current = null;
            }

            #endregion

            #region IDisposeable methods
           
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
            
            #endregion

            #region Private methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            private void Dispose(Boolean disposing)
            {
                _disposed = true;

                if (disposing)
                {
                    _localGraph = null;
                    _finishedVertices.Clear();
                    _finishedVertices = null;
                    _queue.Clear();
                    _queue = null;
                    _current = null;
                }
            }

            #endregion
        }

        /// <summary>
        /// Represents a graph enumerator implementing depth-first search.
        /// </summary>
        public sealed class DepthFirstEnumerator : IEnumerator<IGraphVertex>
        {
            #region Private fields

            private GeometryGraph _localGraph;
            private Int32 _localVersion;
            private Stack<IGraphVertex> _stack;
            private HashSet<IGraphVertex> _finishedVertices;
            private IGraphVertex _current;
            private Boolean _disposed;
         
            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public IGraphVertex Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.-
            /// </value>
            object IEnumerator.Current
            {
                get { return _current; }
            }

            #endregion

            #region Constructors and destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="DepthFirstEnumerator" /> class.
            /// </summary>
            /// <param name="heap">The graph.</param>
            internal DepthFirstEnumerator(GeometryGraph localGraph) {
                _localGraph = localGraph;
                _localVersion = localGraph._version;
                _stack = new Stack<IGraphVertex>();
                _finishedVertices = new HashSet<IGraphVertex>();
                _disposed = false;

                Reset();
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="DepthFirstEnumerator" /> class.
            /// </summary>
            ~DepthFirstEnumerator()
            {
                Dispose(false);
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localGraph._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_finishedVertices.Count == _localGraph._vertexList.Count)
                {
                    _current = null;
                    return false;
                }

                if (_stack.Count == 0)
                {
                    for (Int32 i = 0; i < _localGraph._vertexList.Count; i++)
                    {
                        if (!_finishedVertices.Contains(_localGraph._vertexList[i]))
                        {
                            _stack.Push(_localGraph._vertexList[i]);
                            break;
                        }
                    }
                }

                _current = _stack.Pop();
                _finishedVertices.Add(_current);

                foreach (IGraphEdge edge in _localGraph.OutEdges(_current))
                {
                    if (!_finishedVertices.Contains(edge.Target))
                    {
                        _stack.Push(edge.Target);
                    }
                }

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.ObjectDisposedException">The object is diposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localGraph._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _stack.Clear();
                _finishedVertices.Clear();
                _current = null;
            }

            #endregion

            #region IDisposeable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            private void Dispose(Boolean disposing)
            {
                _disposed = true;

                if (disposing)
                {
                    _localGraph = null;
                    _finishedVertices.Clear();
                    _finishedVertices = null;
                    _stack.Clear();
                    _stack = null;
                    _current = null;
                }
            }

            #endregion
        }

        #endregion

        #region Protected types

        /// <summary>
        /// Represents an equality comparer for vertices.
        /// </summary>
        protected class VertexEqualityComparer : IEqualityComparer<IGraphVertex>
        {
            #region IEqualityComparer methods

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="GraphVertex" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="GraphVertex" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(IGraphVertex x, IGraphVertex y)
            {
                if (ReferenceEquals(x, null)) return ReferenceEquals(y, null);
                if (ReferenceEquals(x, y)) return true;

                return false;
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="GraphVertex" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
            public Int32 GetHashCode(IGraphVertex obj)
            {
                if (obj == null)
                    throw new ArgumentNullException("obj", "The type of obj is a reference type and obj is null.");

                return obj.GetHashCode();
            }

            #endregion
        }

        /// <summary>
        /// Represents an equality comparer for edges.
        /// </summary>
        protected class EdgeEqualityComparer : IEqualityComparer<IGraphEdge>
        {
            #region IEqualityComparer methods

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="GraphEdge" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="GraphEdge" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(IGraphEdge x, IGraphEdge y)
            {
                if (ReferenceEquals(x, null)) return ReferenceEquals(y, null);
                if (ReferenceEquals(x, y)) return true;

                return false;
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="GraphEdge" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
            public Int32 GetHashCode(IGraphEdge obj)
            {
                if (obj == null)
                    throw new ArgumentNullException("obj", "The type of obj is a reference type and obj is null.");

                return obj.GetHashCode();
            }

            #endregion
        }

        #endregion

        #region Private static fields

        /// <summary>
        /// The empty edge set. This field is read-only.
        /// </summary>
        private static readonly ISet<IGraphEdge> EmptyEdgeSet = new HashSet<IGraphEdge>();

        #endregion

        #region Private fields

        /// <summary>
        /// The envelope of the gtraph.
        /// </summary>
        private Envelope _envelope;

        /// <summary>
        /// The boundary of the graph.
        /// </summary>
        private IGeometry _boundary;

        /// <summary>
        /// The metadata.
        /// </summary>
        private IMetadataCollection _metadata;

        #endregion

        #region Protected fields

        /// <summary>
        /// The geometry graph factory.
        /// </summary>
        protected readonly IGeometryGraphFactory _factory;

        /// <summary>
        /// The list of vertices.
        /// </summary>
        protected List<IGraphVertex> _vertexList;

        /// <summary>
        /// The list of edges.
        /// </summary>
        protected List<IGraphEdge> _edgeList;

        /// <summary>
        /// The version of the graph.
        /// </summary>
        protected Int32 _version;

        /// <summary>
        /// The adjacency dictionary of the source vertices.
        /// </summary>
        protected Dictionary<IGraphVertex, HashSet<IGraphEdge>> _adjacencySource;

        /// <summary>
        /// The adjacency dictionary of the target vertices.
        /// </summary>
        protected Dictionary<IGraphVertex, HashSet<IGraphEdge>> _adjacencyTarget;

        /// <summary>
        /// The comparer used for determining the equality of vertices.
        /// </summary>
        protected IEqualityComparer<IGraphVertex> _vertexEqualityComparer;

        /// <summary>
        /// The comparer used for determining the equality of edges.
        /// </summary>
        protected IEqualityComparer<IGraphEdge> _edgeEqualityComparer;

        #endregion

        #region IGeometry properties

        /// <summary>
        /// Gets the factory of the geometry.
        /// </summary>
        /// <value>The factory implementation the geometry was constructed by.</value>
        public IGeometryFactory Factory { get { return _factory.GetFactory<IGeometryFactory>(); } }

        /// <summary>
        /// Gets the precision model of the geometry.
        /// </summary>
        /// <value>The precision model of the geometry.</value>
        public PrecisionModel PrecisionModel { get { return _factory.GetFactory<IGeometryFactory>().PrecisionModel; } }

        /// <summary>
        /// Gets the general name of the geometry.
        /// </summary>
        /// <value>The general name of the specific geometry.</value>
        public String Name { get { return "Geometry graph"; } }

        /// <summary>
        /// Gets the inherent dimension of the geometry.
        /// </summary>
        public Int32 Dimension
        {
            get
            {
                if (_adjacencySource.Count == 0)
                    return 0;

                Double sourceDimensionValue = _adjacencySource.First().Key.Coordinate.Z;
                Boolean sourceIs2D = _adjacencySource.All(p => p.Key.Coordinate.Z == sourceDimensionValue);
                if (!sourceIs2D) return 3;

                return 2;
            }
        }

        /// <summary>
        /// Gets the coordinate dimension of the geometry.
        /// </summary>
        /// <value>The coordinate dimension of the geometry. The coordinate dimension is equal to the dimension of the reference system, if provided.</value>
        public Int32 CoordinateDimension { get { return (ReferenceSystem != null) ? ReferenceSystem.Dimension : SpatialDimension; } }

        /// <summary>
        /// Gets the spatial dimension of the geometry.
        /// </summary>
        /// <value>The spatial dimension of the geometry. The spatial dimension is always less than or equal to the coordinate dimension.</value>
        public Int32 SpatialDimension { get { return (Envelope.Minimum.Z != 0 || Envelope.Maximum.Z != 0) ? 3 : 2; } }

        /// <summary>
        /// Gets the model of the geometry.
        /// </summary>
        /// <value>The model of the geometry.</value>
        public GeometryModel GeometryModel { get { return (CoordinateDimension == 3) ? GeometryModel.Spatial3D : GeometryModel.Spatial2D; } }

        /// <summary>
        /// Gets the reference system of the geometry.
        /// </summary>
        /// <value>The reference system of the geometry.</value>
        public IReferenceSystem ReferenceSystem { get { return _factory.GetFactory<IGeometryFactory>().ReferenceSystem; } }

        /// <summary>
        /// Gets the minimum bounding envelope of the geometry.
        /// </summary>
        /// <value>The minimum bounding box of the geometry.</value>
        public Envelope Envelope { get { return _envelope ?? (_envelope = ComputeEnvelope()); } }

        /// <summary>
        /// Gets the bounding <see cref="IGeometry" />.
        /// </summary>
        /// <value>The boundary of the geometry.</value>
        public IGeometry Boundary { get { return _boundary ?? (_boundary = ComputeBoundary()); } }

        /// <summary>
        /// Gets the centroid of the geometry.
        /// </summary>
        public Coordinate Centroid
        {
            get
            {
                Double centroidX = 0, centroidY = 0, centroidZ = 0;
                Int32 count = 0;
                foreach (KeyValuePair<IGraphVertex, HashSet<IGraphEdge>> kvp in _adjacencySource)
                {
                    centroidX += kvp.Key.Coordinate.X;
                    centroidY += kvp.Key.Coordinate.Y;
                    centroidZ += kvp.Key.Coordinate.Z;
                    ++count;
                    foreach (IGraphEdge edge in kvp.Value)
                    {
                        centroidX += (edge.Source.Coordinate.X + edge.Target.Coordinate.X) / 2;
                        centroidY += (edge.Source.Coordinate.Y + edge.Target.Coordinate.Y) / 2;
                        centroidZ += (edge.Source.Coordinate.Z + edge.Target.Coordinate.Z) / 2;
                        ++count;
                    }
                }
                centroidX /= count;
                centroidY /= count;
                centroidZ /= count;
                return PrecisionModel.MakePrecise(new Coordinate(centroidX, centroidY, centroidZ));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is empty.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be empty; otherwise, <c>false</c>.</value>
        public Boolean IsEmpty
        {
            get
            {
                return _adjacencySource.Count == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        public Boolean IsSimple
        {
            get
            {
                HashSet<Coordinate> nodeCoordinatePool = new HashSet<Coordinate>();
                foreach (KeyValuePair<IGraphVertex, HashSet<IGraphEdge>> item in _adjacencySource)
                {
                    if (nodeCoordinatePool.Contains(item.Key.Coordinate)) return false;
                    else nodeCoordinatePool.Add(item.Key.Coordinate);
                    HashSet<Coordinate> edgeCoordinatePool = new HashSet<Coordinate>();
                    foreach (IGraphEdge edge in item.Value)
                    {
                        if (_vertexEqualityComparer.Equals(edge.Source, edge.Target)) return false;
                        if (edgeCoordinatePool.Contains(edge.Target.Coordinate)) return false;
                        else edgeCoordinatePool.Add(item.Key.Coordinate);
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the geometry is simple.
        /// </summary>
        /// <value><c>true</c> if the geometry is considered to be simple; otherwise, <c>false</c>.</value>
        public Boolean IsValid
        {
            get
            {
                return _adjacencySource.Keys.All(vertex => vertex.Coordinate.IsValid);
            }
        }

        #endregion

        #region IGeometryGraph properties

        /// <summary>
        /// Gets the number of vertices.
        /// </summary>
        /// <value>The number of vertices in the graph.</value>
        public virtual Int32 VertexCount { get { return _adjacencySource.Count; } }

        /// <summary>
        /// Gets the number of edges.
        /// </summary>
        /// <value>The number of edges in the graph.</value>
        public virtual Int32 EdgeCount { get { return _edgeList != null ? _edgeList.Count : _adjacencySource.Values.Sum(hashSet => hashSet.Count); } }

        /// <summary>
        /// Gets the list of vertices.
        /// </summary>
        /// <value>The read-only list of vertices.</value>
        public virtual IList<IGraphVertex> Vertices { get { return _vertexList.AsReadOnly(); } }

        /// <summary>
        /// Gets the list of edges.
        /// </summary>
        /// <value>The read-only list of edges.</value>
        public virtual IList<IGraphEdge> Edges
        {
            get
            {
                if (_edgeList == null)
                {
                    _edgeList = new List<IGraphEdge>();
                    foreach (HashSet<IGraphEdge> set in _adjacencySource.Values)
                    {
                        _edgeList.AddRange(set);
                    }
                }
                return _edgeList.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer" /> object used by the graph for comparing vertices.
        /// </summary>
        /// <value>The <see cref="IEqualityComparer" /> object used by the graph for comparing vertices.</value>
        public IEqualityComparer<IGraphVertex> VertexComparer { get { return _vertexEqualityComparer; } }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer" /> object used by the graph for comparing edges.
        /// </summary>
        /// <value>The <see cref="IEqualityComparer" /> object used by the graph for comparing edges.</value>
        public IEqualityComparer<IGraphEdge> EdgeComparer { get { return _edgeEqualityComparer; } }

        #endregion

        #region IMetadataProvider properties

        /// <summary>
        /// Gets the metadata collection.
        /// </summary>
        /// <value>The metadata collection.</value>
        public IMetadataCollection Metadata { get { return _metadata; } }

        /// <summary>
        /// Gets or sets the metadata value for a specified key.
        /// </summary>
        /// <param name="key">The key of the metadata.</param>
        /// <returns>The metadata value with the <paramref name="key" /> if it exists; otherwise, <c>null</c>.</returns>
        public Object this[String key]
        {
            get
            {
                Object value = null;
                if (_metadata != null)
                    _metadata.TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (_metadata == null)
                    _metadata = _factory.GetFactory<IGeometryFactory>().CreateMetadata();
                _metadata[key] = value;
            }
        }

        #endregion

        #region IGeometry events

        /// <summary>
        /// Occurs when the geometry is changed.
        /// </summary>
        public event EventHandler GeometryChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryGraph" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public GeometryGraph(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata)
            : this(referenceSystem, metadata, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryGraph" /> class.
        /// </summary>
        /// <param name="vertexEqualityComparer">The vertex equality comparer.</param>
        /// <param name="edgeEqualityComparer">The edge equality comparer.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public GeometryGraph(IReferenceSystem referenceSystem, IDictionary<String, Object> metadata, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
            : this(new GeometryGraphFactory(referenceSystem == null ? AEGIS.Factory.DefaultInstance<GeometryFactory>() : AEGIS.Factory.GetInstance<GeometryFactory>(referenceSystem)), metadata, vertexEqualityComparer, edgeEqualityComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryGraph" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public GeometryGraph(IGeometryGraphFactory factory, IDictionary<String, Object> metadata)
            : this(factory, metadata, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryGraph" /> class.
        /// </summary>
        /// <param name="vertexEqualityComparer">The vertex equality comparer.</param>
        /// <param name="edgeEqualityComparer">The edge equality comparer.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        public GeometryGraph(IGeometryGraphFactory factory, IDictionary<String, Object> metadata, IEqualityComparer<IGraphVertex> vertexEqualityComparer, IEqualityComparer<IGraphEdge> edgeEqualityComparer)
        {
            _factory = factory ?? AEGIS.Factory.DefaultInstance<GeometryGraphFactory>();
            _metadata = _factory.GetFactory<IGeometryFactory>().CreateMetadata(metadata);

            _envelope = null;
            _boundary = null;

            _vertexEqualityComparer = vertexEqualityComparer ?? new VertexEqualityComparer();
            _edgeEqualityComparer = edgeEqualityComparer ?? new EdgeEqualityComparer();

            _version = 0;
            _vertexList = new List<IGraphVertex>();
            _edgeList = null;
            _adjacencySource = new Dictionary<IGraphVertex, HashSet<IGraphEdge>>(_vertexEqualityComparer);
            _adjacencyTarget = new Dictionary<IGraphVertex, HashSet<IGraphEdge>>(_vertexEqualityComparer);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the outgoing edges of a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The read-only set containing edges with <paramref="vertex"> as source.</returns>
        public virtual ISet<IGraphEdge> OutEdges(IGraphVertex vertex) { return Contains(vertex) ? _adjacencySource[vertex].AsReadOnly() : EmptyEdgeSet.AsReadOnly(); }

        /// <summary>
        /// Returns the incoming edges of a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The read-only set containing edges with <paramref="vertex"> as target.</returns>
        public virtual ISet<IGraphEdge> InEdges(IGraphVertex vertex) { return Contains(vertex) ? _adjacencyTarget[vertex].AsReadOnly() : EmptyEdgeSet.AsReadOnly(); }

        /// <summary>
        /// Adds a new vertex to a graph.
        /// </summary>
        /// <param name="coordinate">The coordinate of the vertex.</param>
        /// <returns>The vertex created at the <paramref name="coordinate" />.</returns>
        public virtual IGraphVertex AddVertex(Coordinate coordinate)
        {
            return AddVertex(PrecisionModel.MakePrecise(coordinate), null);
        }

        /// <summary>
        /// Adds a new vertex to a graph.
        /// </summary>
        /// <param name="coordinate">The coordinate of the vertex.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The vertex created at the <paramref name="coordinate" />.</returns>
        public virtual IGraphVertex AddVertex(Coordinate coordinate, IDictionary<String, Object> metadata)
        {
            GraphVertex vertex = new GraphVertex(this, PrecisionModel.MakePrecise(coordinate), metadata);

            if (_adjacencySource.ContainsKey(vertex))
                return null;

            _vertexList.Add(vertex);
            _adjacencySource.Add(vertex, new HashSet<IGraphEdge>(_edgeEqualityComparer));
            _adjacencyTarget.Add(vertex, new HashSet<IGraphEdge>(_edgeEqualityComparer));
            _version++;

            OnGeometryChanged();

            return vertex;
        }

        /// <summary>
        /// Returns a vertex at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The vertex located at the <paramref name="coordinate" /> if any; otherwise, <c>null</c>.</returns>
        public virtual IGraphVertex GetVertex(Coordinate coordinate)
        {
            coordinate = PrecisionModel.MakePrecise(coordinate);
            return _vertexList.FirstOrDefault(t => t.Coordinate.Equals(coordinate));
        }

        /// <summary>
        /// Returns all vertices located at a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The list of vertices located at the <paramref name="coordinate" />.</returns>
        public virtual IList<IGraphVertex> GetAllVertices(Coordinate coordinate)
        {
            coordinate = PrecisionModel.MakePrecise(coordinate);
            return _vertexList.Where(t => t.Coordinate.Equals(coordinate)).ToList<IGraphVertex>();
        }

        /// <summary>
        /// Determines whether the graph contains the specified coordinate.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns><c>true</c> if the graph contains the <paramref name="vertex" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The vertex is null.</exception>
        public virtual Boolean Contains(IGraphVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException("vertex", "The vertex is null.");

            return (vertex is GraphVertex) && (vertex.Graph == this) && _adjacencySource.ContainsKey(vertex);
        }

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        public virtual IGraphEdge AddEdge(Coordinate source, Coordinate target)
        {
            return AddEdge(source, target, null);
        }

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        public virtual IGraphEdge AddEdge(Coordinate source, Coordinate target, IDictionary<String, Object> metadata)
        {
            source = PrecisionModel.MakePrecise(source);
            target = PrecisionModel.MakePrecise(target);

            IGraphVertex sourceVertex = GetVertex(source);
            IGraphVertex targetVertex = GetVertex(target);

            if (sourceVertex == null)
                sourceVertex = AddVertex(source);
            if (targetVertex == null)
                targetVertex = AddVertex(target);

            return AddEdge(sourceVertex, targetVertex, metadata);
        }


        /// <summary>
        /// Add a new edge to the graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        public virtual IGraphEdge AddEdge(IGraphVertex source, IGraphVertex target)
        {
            return AddEdge(source, target, null);
        }

        /// <summary>
        /// Add a new edge to the graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The edge created between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        public virtual IGraphEdge AddEdge(IGraphVertex source, IGraphVertex target, IDictionary<String, Object> metadata)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source vertex is null.");
            if (target == null)
                throw new ArgumentNullException("target", "The target vertex is null.");

            HashSet<IGraphEdge> sourceResult, targetResult;

            if (!(source is GraphVertex) || source.Graph != this || !_adjacencySource.TryGetValue(source, out sourceResult))
                throw new ArgumentException("The source vertex is not within the graph.", "source");
            if (!(target is GraphVertex) || target.Graph != this || !_adjacencyTarget.TryGetValue(target, out targetResult))
                throw new ArgumentException("The target vertex is not within the graph.", "target");

            GraphEdge edge = new GraphEdge(this, source as GraphVertex, target as GraphVertex, metadata);
            sourceResult.Add(edge);
            targetResult.Add(edge);

            OnGeometryChanged();

            return edge;
        }

        /// <summary>
        /// Returns an edge between the specified coordinates.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns>The first edge between <paramref name="source" /> and <paramref name="target" /> coordinates.</returns>
        public virtual IGraphEdge GetEdge(Coordinate source, Coordinate target)
        {
            source = PrecisionModel.MakePrecise(source);
            target = PrecisionModel.MakePrecise(target);

            foreach (GraphVertex vertex in _adjacencySource.Keys)
            {
                if (vertex.Coordinate.Equals(source))
                {
                    IGraphEdge selectedEdge = _adjacencySource[vertex].FirstOrDefault(edge => edge.Target.Coordinate.Equals(target));
                    if (selectedEdge != null)
                        return selectedEdge;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns an edge between the specified vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns>The edge between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        public virtual IGraphEdge GetEdge(IGraphVertex source, IGraphVertex target)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source vertex is null.");
            if (target == null)
                throw new ArgumentNullException("target", "The target vertex is null.");

            HashSet<IGraphEdge> sourceResult;

            if (!(source is GraphVertex) || source.Graph != this || !_adjacencySource.TryGetValue(source, out sourceResult))
                throw new ArgumentException("The source vertex is not within the graph.", "source");
            if (!(target is GraphVertex) || target.Graph != this || !_adjacencyTarget.ContainsKey(target))
                throw new ArgumentException("The target vertex is not within the graph.", "target");

            return sourceResult.FirstOrDefault(edge => _vertexEqualityComparer.Equals(edge.Target, target));
        }

        /// <summary>
        /// Returns all edges between the specified coordinates.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns>The read-only list of edges between <paramref name="source" /> and <paramref name="target" /> coordinates.</returns>
        public virtual IList<IGraphEdge> GetAllEdges(Coordinate source, Coordinate target)
        {
            source = PrecisionModel.MakePrecise(source);
            target = PrecisionModel.MakePrecise(target);

            List<IGraphEdge> edges = new List<IGraphEdge>();

            foreach (GraphVertex vertex in _adjacencySource.Keys)
            {
                if (vertex.Coordinate.Equals(source))
                {
                    edges.AddRange(_adjacencySource[vertex].Where(edge => edge.Target.Coordinate.Equals(target)));
                }
            }

            return edges;
        }

        /// <summary>
        /// Returns all edges between the specified vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns>The read-only list of edges between <paramref name="source" /> and <paramref name="target" /> vertices.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The source vertex is not within the graph.
        /// or
        /// The target vertex is not within the graph.
        /// </exception>
        public virtual IList<IGraphEdge> GetAllEdges(IGraphVertex source, IGraphVertex target)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source vertex is null.");
            if (target == null)
                throw new ArgumentNullException("target", "The target vertex is null.");

            HashSet<IGraphEdge> sourceResult;

            if (!(source is GraphVertex) || source.Graph != this || !_adjacencySource.TryGetValue(source, out sourceResult))
                throw new ArgumentException("The source vertex is not within the graph.", "source");
            if (!(target is GraphVertex) || target.Graph != this || !_adjacencyTarget.ContainsKey(target))
                throw new ArgumentException("The target vertex is not within the graph.", "target");

            return sourceResult.Where(edge => _vertexEqualityComparer.Equals(edge.Target, target)).ToList<IGraphEdge>();
        }

        /// <summary>
        /// Returns the nearest vertex to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The nearest vertex to <paramref name="coordinate" /> if the graph is not empty; otherwise, <c>null</c>.</returns>
        public IGraphVertex GetNearestVertex(Coordinate coordinate)
        {
            coordinate = PrecisionModel.MakePrecise(coordinate);

            GraphVertex minVertex = null;
            Double minDistance = Double.MaxValue;
            foreach (GraphVertex vertex in _vertexList)
                if (Coordinate.Distance(vertex.Coordinate, coordinate) < minDistance)
                {
                    minVertex = vertex;
                    minDistance = Coordinate.Distance(vertex.Coordinate, coordinate);
                }
            return minVertex;
        }

        /// <summary>
        /// Returns the nearest vertex to a specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="accuracy">The accuracy of the localization.</param>
        /// <returns>The first vertex within the specified <paramref name="accuracy" /> to <paramref name="coordinate" /> if any; otherwise, <c>null</c>.</returns>
        public IGraphVertex GetNearestVertex(Coordinate coordinate, Double accuracy)
        {
            coordinate = PrecisionModel.MakePrecise(coordinate);

            return _vertexList.FirstOrDefault(vertex => Coordinate.Distance(vertex.Coordinate, coordinate) <= accuracy);
        }

        /// <summary>
        /// Removes all vertices located at the specified vertex from the graph.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if at least one vertex was located and removed from the <paramref name="coordinate" />; otherwise false.</returns>
        public virtual Boolean RemoveVertex(Coordinate coordinate)
        {
            coordinate = PrecisionModel.MakePrecise(coordinate);

            // get all vertices with the specified coordinate
            IGraphVertex[] vertices = _adjacencySource.Keys.Where(v => v.Coordinate.Equals(coordinate)).ToArray();

            if (vertices.Length == 0) // if none, nothing to do
                return false;

            for (Int32 i = 0; i < vertices.Length; i++) // remove all vertices
                RemoveVertex(vertices[i]);

            return true;
        }

        /// <summary>
        /// Removes the specified vertex from the graph.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns><c>true</c> if <paramref name="vertex" /> is within the graph and has been removed; otherwise false.</returns>
        /// <exception cref="System.ArgumentNullException">vertex;The vertex is null.</exception>
        public virtual Boolean RemoveVertex(IGraphVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException("vertex", "The vertex is null.");

            if (!(vertex is GraphVertex) || vertex.Graph != this)
                return false;

            if (!_adjacencySource.ContainsKey(vertex))
                return false;

            _adjacencySource.Remove(vertex);
            foreach (GraphVertex v in _adjacencySource.Keys)
            {
                _adjacencySource[v].RemoveWhere(edge => _vertexEqualityComparer.Equals(edge.Target, vertex));
            }

            _adjacencyTarget.Remove(vertex);
            foreach (GraphVertex v in _adjacencySource.Keys)
            {
                _adjacencyTarget[v].RemoveWhere(edge => _vertexEqualityComparer.Equals(edge.Source, vertex));
            }
            _vertexList.Remove(vertex);
            _version++;

            OnGeometryChanged();

            return true;
        }

        /// <summary>
        /// Removes the edge from the graph.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns><c>true</c> if the edgee was located and removed from the graph; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The edge is null.</exception>
        public virtual Boolean RemoveEdge(IGraphEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException("edge", "The edge is null.");

            if (!(edge is GraphEdge) || edge.Graph != this)
                return false;

            Int32 removeCount = 0;

            foreach (GraphVertex v in _adjacencySource.Keys)
            {
                removeCount += _adjacencySource[v].RemoveWhere(e => _edgeEqualityComparer.Equals(e, edge));
            }
            foreach (GraphVertex v in _adjacencyTarget.Keys)
            {
                removeCount += _adjacencyTarget[v].RemoveWhere(e => _edgeEqualityComparer.Equals(e, edge));
            }

            if (removeCount > 0)
            {
                OnGeometryChanged();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all edges between the source and target coordinates from the graph.
        /// </summary>
        /// <param name="source">The source coordinate.</param>
        /// <param name="target">The target coordinate.</param>
        /// <returns><c>true</c> if at least one edge was located and removed between <paramref name="source" /> and <paramref name="target" />; otherwise, <c>false</c>.</returns>
        public virtual Boolean RemoveEdge(Coordinate source, Coordinate target)
        {
            source = PrecisionModel.MakePrecise(source);
            target = PrecisionModel.MakePrecise(target);

            if (_vertexList.Count(vertex => vertex.Coordinate.Equals(source) || vertex.Coordinate.Equals(target)) == 0)
                return false;

            Int32 removeCount = 0;
            foreach (HashSet<IGraphEdge> edges in _adjacencySource.Where(item => item.Key.Coordinate.Equals(source)).Select(item => item.Value))
            {
                removeCount += edges.RemoveWhere(edge => edge.Target.Coordinate.Equals(target));
            }

            foreach (HashSet<IGraphEdge> edges in _adjacencyTarget.Where(item => item.Key.Coordinate.Equals(source)).Select(item => item.Value))
            {
                removeCount += edges.RemoveWhere(edge => edge.Source.Coordinate.Equals(target));
            }

            if (removeCount > 0)
            {
                OnGeometryChanged();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all edges between the source and target vertices from the graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <returns><c>true</c> if at least one edge was located and removed between <paramref name="source" /> and <paramref name="target" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The source vertex is null.
        /// or
        /// The target vertex is null.
        /// </exception>
        public virtual Boolean RemoveEdge(IGraphVertex source, IGraphVertex target)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source vertex is null.");
            if (target == null)
                throw new ArgumentNullException("target", "The target vertex is null.");

            HashSet<IGraphEdge> sourceEdges, targetEdges;

            if (!(source is GraphVertex) || source.Graph != this || !_adjacencySource.TryGetValue(source, out sourceEdges))
                return false;
            if (!(target is GraphVertex) || target.Graph != this || !_adjacencyTarget.TryGetValue(source, out targetEdges))
                return false;

            Int32 removeCount = 0;
            removeCount += sourceEdges.RemoveWhere(edge => _vertexEqualityComparer.Equals(edge.Target, target)); // remove all edges with the specified target
            removeCount += targetEdges.RemoveWhere(edge => _vertexEqualityComparer.Equals(edge.Source, source)); // remove all edges with the specified source

            if (removeCount > 0)
            {
                OnGeometryChanged();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the graph.
        /// </summary>
        /// <param name="strategy">The strategy of the enumeration.</param>
        /// <returns>An <see cref="IEnumerator{IGraphVertex}" /> object that can be used to iterate through the graph.</returns>
        public IEnumerator<IGraphVertex> GetEnumerator(EnumerationStrategy strategy)
        {
            if (strategy == EnumerationStrategy.DepthFirst)
                return new DepthFirstEnumerator(this);
            else
                return new BreadthFirstEnumerator(this);
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a clone of the <see cref="GeometryGraph" /> instance.
        /// </summary>
        /// <returns>The deep copy of the <see cref="GeometryGraph" /> instance.</returns>
        public virtual Object Clone()
        {
            GeometryGraph result = new GeometryGraph(_factory, _metadata, _vertexEqualityComparer, _edgeEqualityComparer);
            CloneToGraph(this, result);
            return result;
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through a graph.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{IGraphVertex}" /> object that can be used to iterate through the graph.</returns>
        public IEnumerator<IGraphVertex> GetEnumerator()
        {
            return new BreadthFirstEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a graph.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the graph.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BreadthFirstEnumerator(this);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return Name + " [" + VertexCount + ", " + EdgeCount + "]";
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Clones a specified source graph to a target graph.
        /// </summary>
        /// <param name="source">Source graph.</param>
        /// <param name="target">Target graph.</param>
        protected static void CloneToGraph(GeometryGraph source, GeometryGraph target)
        {
            Dictionary<IGraphVertex, IGraphVertex> sourceToTargetVertexMapper = new Dictionary<IGraphVertex, IGraphVertex>();
            Dictionary<IGraphVertex, IGraphVertex> vertexToVertexMapper = new Dictionary<IGraphVertex, IGraphVertex>(target._vertexEqualityComparer);

            foreach (KeyValuePair<IGraphVertex, HashSet<IGraphEdge>> item in source._adjacencySource)
            {
                IGraphVertex n = target.AddVertex(item.Key.Coordinate, item.Key.Metadata), m;
                if (!vertexToVertexMapper.TryGetValue(n, out m))
                {
                    sourceToTargetVertexMapper.Add(item.Key, n);
                    vertexToVertexMapper.Add(n, n);
                }
                else
                {
                    sourceToTargetVertexMapper.Add(item.Key, vertexToVertexMapper[n]);
                }
            }

            foreach (KeyValuePair<IGraphVertex, HashSet<IGraphEdge>> item in source._adjacencySource)
            {
                IGraphVertex from = sourceToTargetVertexMapper[item.Key];
                foreach (IGraphEdge edge in item.Value)
                {
                    IGraphVertex to = sourceToTargetVertexMapper[edge.Target];
                    target.AddEdge(from, to, source.Factory.CreateMetadata(edge.Metadata));
                }
            }
        }

        /// <summary>
        /// Called when the geometry is changed.
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
            EventHandler geometryChanged = GeometryChanged;

            _envelope = null;
            _boundary = null;

            _edgeList = null;
            _version++;

            if (geometryChanged != null)
                geometryChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Computes the minimal bounding envelope of the geometry.
        /// </summary>
        /// <returns>The minimum bounding box of the geometry.</returns>
        protected Envelope ComputeEnvelope()
        {
            return Envelope.FromCoordinates(_vertexList.Select(vertex => vertex.Coordinate));
        }

        /// <summary>
        /// Computes the boundary of the geometry.
        /// </summary>
        /// <returns>The closure of the combinatorial boundary of the geometry.</returns>
        protected IGeometry ComputeBoundary()
        {
            // TODO: compute proper bundary for graph
            return null;
        }

        #endregion
    }

}
