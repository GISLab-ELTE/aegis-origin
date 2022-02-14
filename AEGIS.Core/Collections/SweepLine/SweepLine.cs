/// <copyright file="SweepLine.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Collections.SearchTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents a sweep line.
    /// </summary>
    public sealed class SweepLine
    {
        #region Private types

        /// <summary>
        /// Defines the intersection between two sweep line segments.
        /// </summary>
        private enum SweepLineIntersection         
        { 
            /// <summary>
            /// Indicates that the intersection does not exist.
            /// </summary>
            NotExists, 
            
            /// <summary>
            /// Indicates that the intersection was not passed.
            /// </summary>
            NotPassed, 
            
            /// <summary>
            /// Indicates that the intersection was passed.
            /// </summary>
            Passed 
        }

        /// <summary>
        /// Represents a comparer for <see cref="SweepLineSegment" /> instances.
        /// </summary>
        private sealed class SweepLineSegmentComparer : IComparer<SweepLineSegment>
        {
            #region Private fields

            /// <summary>
            /// Stores the precision model.
            /// </summary>
            private readonly PrecisionModel _precisionModel;

            /// <summary>
            /// Stores the horizontal (X coordinate) position of the sweep line.
            /// </summary>
            private Double _sweepLinePosition;

            /// <summary>
            /// Stores the intersections that has already been processed at the current sweep line position.
            /// </summary>
            /// <remarks>
            /// Intersection points are registered in the containing set with both possible ordering.
            /// </remarks>
            private readonly ISet<Tuple<SweepLineSegment, SweepLineSegment>> _intersections;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SweepLineSegmentComparer"/> class.
            /// </summary>
            /// <param name="precisionModel">The precision model.</param>
            public SweepLineSegmentComparer(PrecisionModel precisionModel)
            {
                _precisionModel = precisionModel;
                _sweepLinePosition = Double.MinValue;
                _intersections = new HashSet<Tuple<SweepLineSegment, SweepLineSegment>>();
            }

            #endregion

            #region IComparer methods

            /// <summary>
            /// Compares two <see cref="SweepLineSegment" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <remarks>
            /// The comparator applies a above-below relationship between the arguments, where a "greater" segment is above the another one.
            /// </remarks>
            /// <param name="first">The first <see cref="SweepLineSegment" /> to compare.</param>
            /// <param name="second">The second <see cref="SweepLineSegment" /> to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
            /// <exception cref="System.InvalidOperationException">Cannot compare non-overlapping sweep line segments.</exception>
            public Int32 Compare(SweepLineSegment first, SweepLineSegment second)
            {
                // Comparing non-overlapping segments is not supported.
                if(first.RightCoordinate.X < second.LeftCoordinate.X || first.LeftCoordinate.X > second.RightCoordinate.X)
                    throw new InvalidOperationException("Cannot compare non-overlapping sweep line segments.");

                // The segments intersect.
                SweepLineIntersection intersection = GetIntersection(first, second);
                if (intersection != SweepLineIntersection.NotExists)
                {
                    CoordinateVector xDiff = first.RightCoordinate - first.LeftCoordinate;
                    Double xGradient = xDiff.X == 0 ? Double.MaxValue : xDiff.Y / xDiff.X;

                    CoordinateVector yDiff = second.RightCoordinate - second.LeftCoordinate;
                    Double yGradient = yDiff.X == 0 ? Double.MaxValue : yDiff.Y / yDiff.X;

                    Int32 result = yGradient.CompareTo(xGradient);
                    if (result == 0) result = first.LeftCoordinate.X.CompareTo(second.LeftCoordinate.X);
                    if (result == 0) result = second.LeftCoordinate.Y.CompareTo(first.LeftCoordinate.Y);
                    if (result == 0) result = first.RightCoordinate.X.CompareTo(second.RightCoordinate.X);
                    if (result == 0) result = second.RightCoordinate.Y.CompareTo(first.RightCoordinate.Y);
                    if (result == 0) result = second.Edge.CompareTo(first.Edge);
                    if (intersection == SweepLineIntersection.Passed) result *= -1;
                    return result;
                }

                // The segments do not intersect.
                if (first.LeftCoordinate.X < second.LeftCoordinate.X)
                {
                    Double[] verticalCollection = new[] { first.LeftCoordinate.Y, first.RightCoordinate.Y };
                    var verticalLineStart = new Coordinate(second.LeftCoordinate.X, verticalCollection.Min());
                    var verticalLineEnd = new Coordinate(second.LeftCoordinate.X, verticalCollection.Max());
                    IList<Coordinate> startIntersections = LineAlgorithms.Intersection(first.LeftCoordinate, first.RightCoordinate,
                                                                                       verticalLineStart, verticalLineEnd, 
                                                                                       _precisionModel);

                    // due to precision tolerance degeneracy we might not found the intersection
                    return startIntersections.Count > 0
                        ? startIntersections[0].Y.CompareTo(second.LeftCoordinate.Y)
                        : ((first.LeftCoordinate.Y + first.RightCoordinate.Y) / 2.0).CompareTo(second.LeftCoordinate.Y);
                }

                if (first.LeftCoordinate.X > second.LeftCoordinate.X)
                {
                    Double[] verticalCollection = new[] { second.LeftCoordinate.Y, second.RightCoordinate.Y };
                    var verticalLineStart = new Coordinate(first.LeftCoordinate.X, verticalCollection.Min());
                    var verticalLineEnd = new Coordinate(first.LeftCoordinate.X, verticalCollection.Max());
                    IList<Coordinate> startIntersections = LineAlgorithms.Intersection(verticalLineStart, verticalLineEnd,
                                                                                       second.LeftCoordinate, second.RightCoordinate,
                                                                                       _precisionModel);

                    return startIntersections.Count > 0
                        ? first.LeftCoordinate.Y.CompareTo(startIntersections[0].Y)
                        : first.LeftCoordinate.Y.CompareTo((second.LeftCoordinate.Y + second.RightCoordinate.Y) / 2.0);
                }

                // first.LeftCoordinate.X == second.LeftCoordinate.X
                return first.LeftCoordinate.Y.CompareTo(second.LeftCoordinate.Y);
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Gets the intersection type between the two given sweep line segments.
            /// </summary>
            /// <param name="x">First sweep line segment.</param>
            /// <param name="y">Second sweep line segment.</param>
            /// <returns>The type of intersection that exists between the two sweep line segments, considering the position of the sweepline.</returns>
            public SweepLineIntersection GetIntersection(SweepLineSegment x, SweepLineSegment y)
            {
                IList<Coordinate> intersections = LineAlgorithms.Intersection(x.LeftCoordinate, x.RightCoordinate, 
                                                                              y.LeftCoordinate, y.RightCoordinate, 
                                                                              _precisionModel);

                if (intersections.Count == 0)
                    return SweepLineIntersection.NotExists;

                if (intersections[0].X < _sweepLinePosition)
                    return SweepLineIntersection.Passed;

                if (intersections[0].X > _sweepLinePosition)
                    return SweepLineIntersection.NotPassed;

                return (_intersections.Contains(Tuple.Create(x, y)) || _intersections.Contains(Tuple.Create(y, x)))
                    ? SweepLineIntersection.Passed
                    : SweepLineIntersection.NotPassed;
            }

            /// <summary>
            /// Registers a passed intersection point by the sweep line between the two given arguments.
            /// </summary>
            /// <remarks>
            /// The position of the sweep line is updated when necessary.
            /// </remarks>
            /// <param name="x">First sweep line segment.</param>
            /// <param name="y">Second sweep line segment.</param>
            public void PassIntersection(SweepLineSegment x, SweepLineSegment y)
            {
                IList<Coordinate> intersections = LineAlgorithms.Intersection(x.LeftCoordinate, x.RightCoordinate, 
                                                                              y.LeftCoordinate, y.RightCoordinate,
                                                                              _precisionModel);

                if (intersections.Count == 0)
                    return;

                if (intersections[0].X > _sweepLinePosition)
                {
                    _sweepLinePosition = intersections[0].X;
                    _intersections.Clear();
                }

                _intersections.Add(Tuple.Create(x, y));
                _intersections.Add(Tuple.Create(y, x));
            }

            #endregion
        }

        /// <summary>
        /// Represents a sweep line tree.
        /// </summary>
        private sealed class SweepLineTree : AvlTree<SweepLineSegment, SweepLineSegment>
        {
            #region Private fields

            /// <summary>
            /// The currently selected node.
            /// </summary>
            private Node _currentNode;

            #endregion

            #region Public properties

            /// <summary>
            /// Gets the current sweep line segment.
            /// </summary>
            public SweepLineSegment Current { get { return _currentNode.Key; } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SweepLineTree" /> class.
            /// </summary>
            /// <param name="precisionModel">The precision model.</param>
            public SweepLineTree(PrecisionModel precisionModel) : base(new SweepLineSegmentComparer(precisionModel)) { _currentNode = null; }

            #endregion

            #region Public methods

            /// <summary>
            /// Inserts the specified segment to the tree.
            /// </summary>
            /// <param name="segment">The segment.</param>
            /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
            public void Insert(SweepLineSegment segment)
            {
                if (segment == null)
                    throw new ArgumentNullException("segment", "The segment is null.");

                if (_root == null)
                {
                    _root = new AvlNode { Key = segment, Value = segment, Balance = 0 };
                    _currentNode = _root;
                    _nodeCount++;
                    _version++;
                    return;
                }

                AvlNode node = SearchNodeForInsertion(segment) as AvlNode;
                if (node == null)
                    return;

                if (_comparer.Compare(segment, node.Key) < 0)
                {
                    node.LeftChild = new AvlNode { Key = segment, Value = segment, Parent = node, Balance = 0 };
                    node.Balance = -1;
                    _currentNode = node.LeftChild;
                }
                else
                {
                    node.RightChild = new AvlNode { Key = segment, Value = segment, Parent = node, Balance = 0 };
                    node.Balance = 1;
                    _currentNode = node.RightChild;
                }

                Balance(node);
                _nodeCount++;
                _version++;
            }

            /// <summary>
            /// Sets the current.
            /// </summary>
            /// <param name="segment">The segment.</param>
            public void SetCurrent(SweepLineSegment segment)
            {
                _currentNode = SearchNode(segment);
            }

            /// <summary>
            /// Gets the previous (below) <see cref="SweepLineSegment" /> for the <see cref="Current" /> one.
            /// </summary>
            /// <returns>The previous segment.</returns>
            public SweepLineSegment GetPrev()
            {
                if (_currentNode == null)
                    return null;

                Node prevNode = _currentNode;
                if (prevNode.LeftChild != null)
                {
                    prevNode = prevNode.LeftChild;

                    while (prevNode.RightChild != null)
                    {
                        prevNode = prevNode.RightChild;
                    }
                    return prevNode.Key;
                }

                while (prevNode.Parent != null && prevNode.Parent.LeftChild == prevNode)
                {
                    prevNode = prevNode.Parent;
                }

                if (prevNode.Parent != null && prevNode.Parent.RightChild == prevNode)
                {
                    return prevNode.Parent.Key;
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Gets the next (above) <see cref="SweepLineSegment" /> for the <see cref="Current" /> one.
            /// </summary>
            /// <returns>The next segment.</returns>
            public SweepLineSegment GetNext()
            {
                if (_currentNode == null)
                    return null;

                Node nextNode = _currentNode;
                if (nextNode.RightChild != null)
                {
                    nextNode = nextNode.RightChild;

                    while (nextNode.LeftChild != null)
                    {
                        nextNode = nextNode.LeftChild;
                    }

                    return nextNode.Key;
                }

                while (nextNode.Parent != null && nextNode.Parent.RightChild == nextNode)
                {
                    nextNode = nextNode.Parent;
                }

                if (nextNode.Parent != null && nextNode.Parent.LeftChild == nextNode)
                {
                    return nextNode.Parent.Key;
                }
                else
                {
                    return null;
                }
            }

            #endregion

            #region DEBUG methods

            /// <summary>
            /// Prints debug information about representation of the tree.
            /// </summary>
            [System.Diagnostics.Conditional("DEBUG")]
            public void Debug()
            {
                if (_root != null)
                {
                    System.Diagnostics.Debug.Write("Root: ");
                    Debug(_root, 0);
                }
            }

            /// <summary>
            /// Prints debug information about the specified subtree.
            /// </summary>
            /// <param name="current">The current node.</param>
            /// <param name="depth">The current depth of the traversal.</param>
            [System.Diagnostics.Conditional("DEBUG")]
            private void Debug(Node current, Int32 depth)
            {
                System.Diagnostics.Debug.WriteLine("F:{0} T:{1} E:{2}", current.Key.LeftCoordinate, current.Key.RightCoordinate, current.Key.Edge);

                if (current.LeftChild != null)
                {
                    System.Diagnostics.Debug.Write(new String(' ', (depth + 1) * 2) + "Below: ");
                    Debug(current.LeftChild, depth + 1);
                }

                if (current.RightChild != null)
                {
                    System.Diagnostics.Debug.Write(new String(' ', (depth + 1) * 2) + "Above: ");
                    Debug(current.RightChild, depth + 1);
                }
            }

            #endregion
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The source coordinates.
        /// </summary>
        private readonly IList<Coordinate> _source;

        /// <summary>
        /// The list of endpoint indices.
        /// </summary>
        private readonly IList<Int32> _endpoints;

        /// <summary>
        /// The sweep line tree.
        /// </summary>
        private readonly SweepLineTree _tree;

        /// <summary>
        /// The coordinate comparer.
        /// </summary>
        private readonly IComparer<Coordinate> _coordinateComparer;

        #endregion

        #region Private properties

        /// <summary>
        /// Gets whether the source of the <see cref="SweepLine" /> contains any closed line string.
        /// </summary>
        private Boolean HasSourcedClosed
        {
            get { return _endpoints != null && _endpoints.Count > 0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SweepLine" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a line string.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public SweepLine(IList<Coordinate> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source;
            _tree = new SweepLineTree(precisionModel ?? PrecisionModel.Default);
            _coordinateComparer = new CoordinateComparer();

            if (source.Count >= 2 && source.First() == source.Last())
                _endpoints = new List<Int32> { 0, source.Count - 2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SweepLine" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public SweepLine(IEnumerable<IList<Coordinate>> source, PrecisionModel precisionModel = null)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = new List<Coordinate>();
            _endpoints = new List<Int32>();

            foreach (IList<Coordinate> coordinateList in source)
            {
                if (coordinateList == null || coordinateList.Count < 2)
                    continue;

                if (coordinateList.First() == coordinateList.Last())
                    _endpoints.Add(_source.Count);
                for (Int32 i = 0; i <= coordinateList.Count - 1; i++)
                    _source.Add(coordinateList[i]);
                if (coordinateList.First() == coordinateList.Last())
                    _endpoints.Add(_source.Count - 2);
            }

            _tree = new SweepLineTree(precisionModel ?? PrecisionModel.Default);
            _coordinateComparer = new CoordinateComparer();
            
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a new endpoint event to the sweep line.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The sweep line segment created by addition of <paramref name="e" />.</returns>
        /// <exception cref="System.ArgumentNullException">e;The event is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The edge of the event is less than 0.;e
        /// or
        /// The edge of the event is greater than the number of edges in the source.;e
        /// </exception>
        public SweepLineSegment Add(EndPointEvent e)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (e == null)
                throw new ArgumentNullException("e", "The event is null.");
            if (e.Edge < 0)
                throw new ArgumentException("The edge of the event is less than 0.", "e");
            if (e.Edge >= _source.Count - 1)
                throw new ArgumentException("The edge of the event is greater than the number of edges in the source.", "e");

            SweepLineSegment segment = new SweepLineSegment { Edge = e.Edge };
            Coordinate c1 = _source[e.Edge];
            Coordinate c2 = _source[e.Edge + 1];

            if (_coordinateComparer.Compare(c1, c2) <= 0)
            {
                segment.LeftCoordinate = c1;
                segment.RightCoordinate = c2;
            }
            else
            {
                segment.LeftCoordinate = c2;
                segment.RightCoordinate = c1;
            }

            _tree.Insert(segment);
            SweepLineSegment segmentAbove = _tree.GetNext();
            SweepLineSegment segmentBelow = _tree.GetPrev();
            if (segmentAbove != null)
            {
                segment.Above = segmentAbove;
                segment.Above.Below = segment;
            }
            if (segmentBelow != null)
            {
                segment.Below = segmentBelow;
                segment.Below.Above = segment;
            }
            return segment;
        }

        /// <summary>
        /// Adds a new intersection event to the sweep line.
        /// Performs the order modifying effect of a possible intersection point between two directly adjacent segments.
        /// </summary>
        /// <remarks>
        /// An intersection event may become invalid if the order of the segments were altered or the intersection point has been already passed by the sweep line since the enqueuing of the event.
        /// This method is safe to be applied for invalid intersections.
        /// </remarks>
        /// <param name="e">The event.</param>
        /// <returns><c>true</c> if a new, valid intersection point was found and passed between <paramref name="x" /> and <paramref name="y" />; otherwise <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Segment <paramref name="x" /> and <paramref name="y" /> do not intersect each other.</exception>
        public Boolean Add(IntersectionEvent e)
        {
            return Intersect(e.Below, e.Above);
        }

        /// <summary>
        /// Searches the sweep line for an endpoint event.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The segment associated with the event.</returns>
        /// <exception cref="System.ArgumentNullException">e;The event is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The edge of the event is less than 0.
        /// or
        /// The edge of the event is greater than the number of edges in the source.
        /// </exception>
        public SweepLineSegment Search(EndPointEvent e)
        {
            if (e == null)
                throw new ArgumentNullException("e", "The event is null.");
            if (e.Edge < 0)
                throw new ArgumentException("The edge of the event is less than 0.", "e");
            if (e.Edge >= _source.Count - 1)
                throw new ArgumentException("The edge of the event is greater than the number of edges in the source.", "e");

            SweepLineSegment segment = new SweepLineSegment() { Edge = e.Edge };
            Coordinate c1 = _source[e.Edge];
            Coordinate c2 = _source[e.Edge + 1];

            if (_coordinateComparer.Compare(c1, c2) < 0)
            {
                segment.LeftCoordinate = c1;
                segment.RightCoordinate = c2;
            }
            else
            {
                segment.LeftCoordinate = c2;
                segment.RightCoordinate = c1;
            }

            SweepLineSegment segmentResult;
            if (_tree.TrySearch(segment, out segmentResult))
                return segmentResult;
            else
                return null;
        }

        /// <summary>
        /// Removes the specified sweep line segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <exception cref="System.ArgumentNullException">The segment is null.</exception>
        public void Remove(SweepLineSegment segment)
        {
            if (segment == null)
                throw new ArgumentNullException("segment", "The segment is null.");

            _tree.SetCurrent(segment);
            if (_tree.Current == null)
                return;

            SweepLineSegment segmentAbove = _tree.GetNext();
            SweepLineSegment segmentBelow = _tree.GetPrev();

            if (segmentAbove != null)
                segmentAbove.Below = segment.Below;
            if (segmentBelow != null)
                segmentBelow.Above = segment.Above;

            _tree.Remove(segment);
        }

        /// <summary>
        /// Performs the order modifying effect of a possible intersection point between two directly adjacent segments.
        /// </summary>
        /// <remarks>
        /// An intersection event may become invalid if the order of the segments were altered or the intersection point has been already passed by the sweep line since the enqueuing of the event.
        /// This method is safe to be applied for invalid intersections.
        /// </remarks>
        /// <param name="x">First segment.</param>
        /// <param name="y">Second segment.</param>
        /// <returns><c>true</c> if a new, valid intersection point was found and passed between <paramref name="x" /> and <paramref name="y" />; otherwise <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Segment <paramref name="x" /> and <paramref name="y" /> do not intersect each other.</exception>
        public Boolean Intersect(SweepLineSegment x, SweepLineSegment y)
        {
            var comparer = ((SweepLineSegmentComparer)_tree.Comparer);
            SweepLineIntersection intersection = comparer.GetIntersection(x, y);

            if (intersection == SweepLineIntersection.NotExists)
                throw new InvalidOperationException("The given segments do not intersect each other.");
            if (intersection == SweepLineIntersection.Passed)
                return false;

            /*
             * Segment order before intersection: belowBelow <-> below <-> above <-> aboveAbove
             * Segment order after intersection:  belowBelow <-> above <-> below <-> aboveAbove
             */
            SweepLineSegment below, above;
            if (x.Above == y)
            {
                below = x;
                above = y;
            }
            else if (y.Above == x)
            {
                below = y;
                above = x;
            }
            else
                return false;

            _tree.Remove(x);
            _tree.Remove(y);
            comparer.PassIntersection(x, y);
            _tree.Insert(x);
            _tree.Insert(y);

            SweepLineSegment belowBelow = below.Below;
            SweepLineSegment aboveAbove = above.Above;

            below.Above = aboveAbove;
            below.Below = above;
            above.Below = belowBelow;
            above.Above = below;

            if (belowBelow != null)
                belowBelow.Above = above;
            if (aboveAbove != null)
                aboveAbove.Below = below;

            return true;
        }

        /// <summary>
        /// Determines whether two edges of a line string or a polygon shell are adjacent.
        /// </summary>
        /// <param name="xEdge">Identifier of the first edge.</param>
        /// <param name="yEdge">Identifier of the second edge.</param>
        /// <returns><c>true</c> if the two edges are adjacent; otherwise, <c>false</c>.</returns>
        public Boolean IsAdjacent(Int32 xEdge, Int32 yEdge)
        {
            if (Math.Abs(xEdge - yEdge) == 1)
                return true;

            if (HasSourcedClosed)
            {
                Int32 xIndex = _endpoints.IndexOf(xEdge),
                      yIndex = _endpoints.IndexOf(yEdge);
                if (xIndex >= 0 && yIndex >= 0 && Math.Abs(xIndex - yIndex) == 1 &&
                    (xIndex < yIndex && xIndex % 2 == 0 || xIndex > yIndex && yIndex % 2 == 0))
                    return true;
            }

            return false;
        }

        #endregion

        #region DEBUG methods

        /// <summary>
        /// Prints debug information about representation of this instance.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public void Debug()
        {
            System.Diagnostics.Debug.WriteLine("====================");
            System.Diagnostics.Debug.WriteLine("SWEEPLINE DEBUG START");
            _tree.Debug();
            System.Diagnostics.Debug.WriteLine("SWEEPLINE DEBUG END");
            System.Diagnostics.Debug.WriteLine("====================");
        }

        #endregion
    }
}

