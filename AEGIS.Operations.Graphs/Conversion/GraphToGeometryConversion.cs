/// <copyright file="GraphToGeometryConversion.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Management;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Conversion
{
    /// <summary>
    /// Represents an operation for converting <see cref="GeometryGraph" /> instances to <see cref="IGeometry" /> representation.
    /// </summary>
    [OperationMethodImplementation("AEGIS::220110", "Graph to geometry conversion")]
    public class GraphToGeometryConversion : Operation<IGeometryGraph, IGeometry>
    {
        #region Private types

        /// <summary>
        /// Represents an edge with angle.
        /// </summary>
        private class AngularEdge
        {
            #region Public properties

            /// <summary>
            /// Gets the source coordinate.
            /// </summary>
            /// <value>The source coordinate.</value>
            public Coordinate Source { get; private set; }

            /// <summary>
            /// Gets the target coordinate.
            /// </summary>
            /// <value>The target coordinate.</value>
            public Coordinate Target { get; private set; }

            /// <summary>
            /// Gets the angle of the edge.
            /// </summary>
            /// <value>The angle to the baseline in counter-clockwise direction.</value>
            public Double Angle { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AngularEdge" /> class.
            /// </summary>
            /// <param name="source">The source coordinate.</param>
            /// <param name="target">The target coordinate.</param>
            /// <param name="angle">The angle coordinate.</param>
            public AngularEdge(Coordinate source, Coordinate target, Double angle)
            {
                Source = source;
                Target = target;
                Angle = angle;
            }

            #endregion
        }

        /// <summary>
        /// Represents a comparer for angular edges.
        /// </summary>
        private class AngularEdgeComparer : IComparer<AngularEdge>
        {
            #region Private fields

            /// <summary>
            /// The edge comparer.
            /// </summary>
            private CoordinateComparer _comparer;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AngularEdgeComparer" /> class.
            /// </summary>
            public AngularEdgeComparer()
            {
                _comparer = new CoordinateComparer();
            }

            #endregion

            #region IComparer methods

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
            public Int32 Compare(AngularEdge x, AngularEdge y)
            {
                Int32 coordinateCompare = _comparer.Compare(x.Source, y.Source);

                if (coordinateCompare == 0)
                    return x.Angle.CompareTo(y.Angle);

                return coordinateCompare;
            }

            #endregion
        }

        /// <summary>
        /// Represents a wedge.
        /// </summary>
        private class Wedge
        {
            #region Public properties

            /// <summary>
            /// Gets the first coordinate.
            /// </summary>
            /// <value>The first coordinate.</value>
            public Coordinate First { get; private set; }

            /// <summary>
            /// Gets the second coordinate.
            /// </summary>
            /// <value>The second coordinate.</value>
            public Coordinate Second { get; private set; }

            /// <summary>
            /// Gets the third coordinate.
            /// </summary>
            /// <value>The third coordinate.</value>
            public Coordinate Third { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Wedge" /> class.
            /// </summary>
            /// <param name="first">The first coordinate.</param>
            /// <param name="second">The second coordinate.</param>
            /// <param name="third">The third coordinate.</param>
            public Wedge(Coordinate first, Coordinate second, Coordinate third)
            {
                First = first;
                Second = second;
                Third = third;
            }

            #endregion
        }

        /// <summary>
        /// Represents an equality comparer for <see cref="IPoint" /> instances.
        /// </summary>
        private class PointEqualityComparer : IEqualityComparer<IPoint>
        {
            #region IEqualityComparer methods

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="IPoint" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="IPoint" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(IPoint x, IPoint y)
            {
                return x.Coordinate.Equals(y.Coordinate);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="IPoint" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            public Int32 GetHashCode(IPoint obj)
            {
                return obj.Coordinate.GetHashCode();
            }

            #endregion
        }

        /// <summary>
        /// Represents an equality comparer for <see cref="ILine" /> instances.
        /// </summary>
        private class LineEqualityComparer : IEqualityComparer<ILine>
        {
            #region IEqualityComparer methods

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="ILine" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="ILine" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(ILine x, ILine y)
            {
                return x.StartCoordinate.Equals(y.StartCoordinate) &&
                       x.EndCoordinate.Equals(y.EndCoordinate) ||
                       x.StartCoordinate.Equals(y.EndCoordinate) &&
                       x.EndCoordinate.Equals(y.StartCoordinate);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="ILine" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            public Int32 GetHashCode(ILine obj)
            {
                return obj.StartCoordinate.GetHashCode() ^ obj.EndCoordinate.GetHashCode();
            }

            #endregion
        }

        /// <summary>
        /// Represents an equality comparer for <see cref="IPolygon" /> instances.
        /// </summary>
        private class PolygonEqualityComparer : IEqualityComparer<IPolygon>
        {
            #region IEqualityComparer methods

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <see cref="IPolygon" /> to compare.</param>
            /// <param name="y">The second object of type <see cref="IPolygon" /> to compare.</param>
            /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
            public Boolean Equals(IPolygon x, IPolygon y)
            {
                return x.Shell.Coordinates.SequenceEqual(y.Shell.Coordinates);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The <see cref="IPolygon" /> for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            public Int32 GetHashCode(IPolygon obj)
            {
                return obj.Shell.Coordinates.Select(coordinate => coordinate.GetHashCode()).Aggregate((x, y) => x ^ y);
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The geometry factory used for producing the result. This field is read-only.
        /// </summary>
        private readonly IGeometryFactory _factory;

        /// <summary>
        /// The dimension of the result. This field is read-only.
        /// </summary>
        private readonly Int32 _dimension;

        /// <summary>
        /// The value indicating whether the geometry metadata is preserved. This field is read-only.
        /// </summary>
        private readonly Boolean _metadataPreservation;

        /// <summary>
        /// The set of produced points.
        /// </summary>
        private HashSet<IPoint> _points;

        /// <summary>
        /// The set of produced lines.
        /// </summary>
        private HashSet<ILine> _lines;

        /// <summary>
        /// The set of produced polygons.
        /// </summary>
        private HashSet<IPolygon> _polygons;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphToGeometryConversion" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public GraphToGeometryConversion(IGeometryGraph source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphToGeometryConversion" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public GraphToGeometryConversion(IGeometryGraph source, IGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, GraphOperationMethods.GraphToGeometryConversion, parameters)
        {
            _factory = ResolveParameter<IGeometryFactory>(CommonOperationParameters.GeometryFactory, _source.Factory);
            _dimension = Convert.ToInt32(ResolveParameter(GraphOperationParameters.GeometryDimension));
            _metadataPreservation = Convert.ToBoolean(ResolveParameter(CommonOperationParameters.MetadataPreservation));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _points = new HashSet<IPoint>(new PointEqualityComparer());
            _lines = new HashSet<ILine>(new LineEqualityComparer());
            _polygons = new HashSet<IPolygon>(new PolygonEqualityComparer());
        }

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            switch (_dimension)
            {
                case 0:
                    foreach (IGraphVertex vertex in _source.Vertices)
                        _points.Add(_factory.CreatePoint(vertex.Coordinate, _metadataPreservation ? vertex.Metadata : null));
                    break;
                case 1:
                    foreach (IGraphVertex vertex in _source.Vertices)
                        _points.Add(_factory.CreatePoint(vertex.Coordinate, _metadataPreservation ? vertex.Metadata : null));

                    foreach (IGraphVertex vertex in _source.Vertices)
                        foreach (IGraphEdge edge in _source.OutEdges(vertex))
                        { 
                            if (edge.Source.Coordinate.Equals(edge.Target.Coordinate))
                                continue;
                            _points.Remove(_factory.CreatePoint(edge.Source.Coordinate));
                            _points.Remove(_factory.CreatePoint(edge.Target.Coordinate));
                            _lines.Add(_factory.CreateLine(edge.Source.Coordinate, edge.Target.Coordinate, _metadataPreservation ? edge.Metadata : null));
                        }
                    break;
                case 2:
                    // source: Jiang, X. Y., Bunke, H.: An optimal algorithm for extracting the regions of a plane graph (1993)

                    List<Coordinate> coordinates = new List<Coordinate>();

                    // find all wedges
                    List<AngularEdge> edges = new List<AngularEdge>();
                    
                    foreach (IGraphVertex vertex in _source.Vertices)
                    {
                        Coordinate sourceCoordinate = vertex.Coordinate;
                        Coordinate baseCoordinate = vertex.Coordinate - new CoordinateVector(1, 0);

                        IEnumerable<IGraphEdge> outEdges = _source.OutEdges(vertex);

                        if (outEdges.Count() == 0)
                        {
                            coordinates.Add(vertex.Coordinate);
                            continue;
                        }
                        foreach (IGraphEdge edge in outEdges)
                        {
                            Coordinate target = edge.Target.Coordinate;

                            edges.Add(new AngularEdge(sourceCoordinate, target,
                                                      Coordinate.Orientation(sourceCoordinate, baseCoordinate, target) == Orientation.CounterClockwise ?
                                                          Coordinate.Angle(sourceCoordinate, baseCoordinate, target) :
                                                          Math.PI - Coordinate.Angle(sourceCoordinate, baseCoordinate, target)));
                        }
                    }

                    edges.Sort(new AngularEdgeComparer());

                    IEnumerable<IGrouping<Coordinate, AngularEdge>> groups = edges.GroupBy(angularEdge => angularEdge.Source);
                    
                    List<Wedge> wedges = new List<Wedge>();

                    foreach (IGrouping<Coordinate, AngularEdge> group in groups)
                    {
                        AngularEdge[] groupEdges = group.ToArray();
                        for (Int32 i = 0; i < groupEdges.Length - 1; i++)
                            wedges.Add(new Wedge(groupEdges[i + 1].Target, groupEdges[i].Source, groupEdges[i].Target));
                        wedges.Add(new Wedge(groupEdges[0].Target, groupEdges[0].Source, groupEdges[groupEdges.Length - 1].Target));
                    }

                    // grouping wedges to regions
                    List<List<Wedge>> faces = new List<List<Wedge>>();
                    List<List<Wedge>> leftOverEdges = new List<List<Wedge>>();

                    for (Int32 i = 0; i < wedges.Count; i++)
                    {
                        List<Wedge> wedgeSeries = new List<Wedge>();

                        Wedge currentWedge = wedges[i];

                        do
                        {
                            wedges.Remove(currentWedge);
                            wedgeSeries.Add(currentWedge);

                            currentWedge = wedges.FirstOrDefault(wedge => wedge.First.Equals(currentWedge.Second) && wedge.Second.Equals(currentWedge.Third));
                        }
                        while (currentWedge != null);

                        if (wedgeSeries[0].First.Equals(wedgeSeries[wedgeSeries.Count - 1].Second) && wedgeSeries[0].Second.Equals(wedgeSeries[wedgeSeries.Count - 1].Third))
                            faces.Add(wedgeSeries);
                        else
                            leftOverEdges.Add(wedgeSeries);
                    }

                    for (Int32 i = 0; i < faces.Count; i++)
                    {
                        List<Coordinate> shell = faces[i].Select(wedge => wedge.First).ToList();
                        shell.Add(shell[0]);

                        if (PolygonAlgorithms.Orientation(shell) == Orientation.CounterClockwise)
                        {
                            Dictionary<String, Object> metadata = null;
                            if (_metadataPreservation)
                            {
                                metadata = new Dictionary<String, Object>();

                                foreach (IMetadataCollection collection in shell.Select(coordinate => _source.GetVertex(coordinate).Metadata))
                                {
                                    foreach (String key in collection.Keys)
                                    {
                                        metadata[key] = collection[key];
                                    }
                                }
                            }

                            _polygons.Add(_factory.CreatePolygon(shell, metadata));
                        }
                    }

                    for (Int32 i = 0; i < leftOverEdges.Count; i++)
                    {
                        for (Int32 j = 0; j < leftOverEdges[i].Count; j++)
                        {
                            _lines.Add(_factory.CreateLine(leftOverEdges[i][j].First, 
                                                           leftOverEdges[i][j].Second,
                                                           _metadataPreservation ? _source.GetEdge(leftOverEdges[i][j].First, leftOverEdges[i][j].Second).Metadata : null));
                        }
                    }

                    foreach (Coordinate coordinate in coordinates)
                        _points.Add(_factory.CreatePoint(coordinate, 
                                                         _metadataPreservation ? _source.GetVertex(coordinate).Metadata : null));
                    break;
            }
        }

        /// <summary>
        /// Finalizes the result.
        /// </summary>
        protected override void FinalizeResult()
        {
            if (_source.VertexCount == 0)
                _result = _factory.CreateGeometryCollection();

            // if only points are in the graph
            if (_lines.Count == 0 && _polygons.Count == 0)
            {
                if (_points.Count == 1)
                    _result = _points.First();
                else 
                    _result = _factory.CreateMultiPoint(_points);
            }
            // if no faces are extracted
            else if (_polygons.Count == 0)
            {
                if (_points.Count == 0)
                {
                    if (_lines.Count == 1)
                        _result = _lines.First();
                    else
                        _result = _factory.CreateMultiLineString(_lines);
                }
                else
                {
                    _result = _factory.CreateGeometryCollection(_points.Concat<IGeometry>(_lines));
                }
            }
            // if faces are extracted
            else
            {
                if (_points.Count == 0 && _lines.Count == 0)
                {
                    if (_polygons.Count == 1)
                        _result = _polygons.First();
                    else
                        _result = _factory.CreateMultiPolygon(_polygons);
                }
                else
                    _result = _factory.CreateGeometryCollection(_points.Concat<IGeometry>(_lines).Concat<IGeometry>(_polygons));
            }
        }

        #endregion
    }
}
