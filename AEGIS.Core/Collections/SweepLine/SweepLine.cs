/// <copyright file="SweepLine.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Collections.SearchTree;

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents a Sweep line.
    /// </summary>
    public sealed class SweepLine
    {
        #region Private types

        /// <summary>
        /// Represents a comparer for <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLineSegment"/> instances.
        /// </summary>
        private sealed class SweepLineSegmentComparer : IComparer<SweepLineSegment>
        {
            #region Private fields

            /// <summary>
            /// Stores the intersection point the sweepline has already passed.
            /// </summary>
            /// <remarks>
            /// Intersection points are registered in the containing dictionary with the segment possessing a smaller Y value for the right coordinate as a key.
            /// If the 2 segments equals for this property, the intersection point is registered for both segments.
            /// Data belonging to edges which were completely passed by the sweepline can be truncated.
            /// </remarks>
            private IDictionary<SweepLineSegment, ISet<SweepLineSegment>> _intersections =
                new Dictionary<SweepLineSegment, ISet<SweepLineSegment>>();

            #endregion

            #region IComparer methods

            /// <summary>
            /// Compares two <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLineSegment"/> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <remarks>
            /// The comparator applies a complex above-below relationship between the arguments.
            /// </remarks>
            /// <param name="x">The first <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLineSegment"/> to compare.</param>
            /// <param name="y">The second <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLineSegment"/> to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.</returns>
            public Int32 Compare(SweepLineSegment x, SweepLineSegment y)
            {
                if (LineAlgorithms.Intersects(x.LeftCoordinate, x.RightCoordinate,
                                              y.LeftCoordinate, y.RightCoordinate))
                {
                    CoordinateVector xDiff = x.RightCoordinate - x.LeftCoordinate;
                    Double xGradient = xDiff.X == 0 ? Double.MaxValue : xDiff.Y / xDiff.X;

                    CoordinateVector yDiff = y.RightCoordinate - y.LeftCoordinate;
                    Double yGradient = yDiff.X == 0 ? Double.MaxValue : yDiff.Y / yDiff.X;

                    Int32 result = yGradient.CompareTo(xGradient);
                    if (result == 0) result = x.LeftCoordinate.X.CompareTo(y.LeftCoordinate.X);
                    if (result == 0) result = y.LeftCoordinate.Y.CompareTo(x.LeftCoordinate.Y);
                    if (result == 0) result = x.RightCoordinate.X.CompareTo(y.RightCoordinate.X);
                    if (result == 0) result = y.RightCoordinate.Y.CompareTo(x.RightCoordinate.Y);
                    if (HasIntersection(x, y)) result *= -1;
                    return result;
                }

                if (x.LeftCoordinate.X < y.LeftCoordinate.X)
                {
                    Double[] vertColl = new[] { x.LeftCoordinate.Y, x.RightCoordinate.Y };
                    var leftLineStart = new Coordinate(y.LeftCoordinate.X, vertColl.Min());
                    var leftLineEnd = new Coordinate(y.LeftCoordinate.X, vertColl.Max());
                    IList<Coordinate> startIntersections = LineAlgorithms.Intersection(x.LeftCoordinate, x.RightCoordinate,
                                                                                       leftLineStart, leftLineEnd);
                    if (startIntersections.Count > 0)
                        return startIntersections[0].Y.CompareTo(y.LeftCoordinate.Y);
                    else
                        return x.LeftCoordinate.X.CompareTo(y.LeftCoordinate.X);
                }

                if (x.LeftCoordinate.X > y.LeftCoordinate.X)
                {
                    Double[] vertColl = new[] { y.LeftCoordinate.Y, y.RightCoordinate.Y };
                    var leftLineStart = new Coordinate(x.LeftCoordinate.X, vertColl.Min());
                    var leftLineEnd = new Coordinate(x.LeftCoordinate.X, vertColl.Max());
                    IList<Coordinate> startIntersections = LineAlgorithms.Intersection(leftLineStart, leftLineEnd,
                                                                                       y.LeftCoordinate, y.RightCoordinate);

                    if (startIntersections.Count > 0)
                        return x.LeftCoordinate.Y.CompareTo(startIntersections[0].Y);
                    else
                        return x.LeftCoordinate.X.CompareTo(y.LeftCoordinate.X);
                }

                return x.LeftCoordinate.Y.CompareTo(y.LeftCoordinate.Y);
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Determines whether an intersection point has been passed by the sweepline between the 2 given arguments.
            /// </summary>
            /// <param name="x">First sweepline segment.</param>
            /// <param name="y">Second sweepline segment.</param>
            /// <returns><c>true</c> if an intersection point has been passed by the sweepline between the 2 given arguments; otherwise, <c>false</c>.</returns>
            public Boolean HasIntersection(SweepLineSegment x, SweepLineSegment y)
            {
                if (x.RightCoordinate.X <= y.RightCoordinate.X)
                {
                    try
                    {
                        return _intersections[x].Contains(y);
                    }
                    catch (KeyNotFoundException)
                    {
                        return false;
                    }
                }
                if (y.RightCoordinate.X <= x.RightCoordinate.X)
                {
                    try
                    {
                        return _intersections[y].Contains(x);
                    }
                    catch (KeyNotFoundException)
                    {
                        return false;
                    }
                }
                return false;
            }

            /// <summary>
            /// Registers a passed intersection point by the sweepline between the 2 given arguments.
            /// </summary>
            /// <param name="x">First sweepline segment.</param>
            /// <param name="y">Second sweepline segment.</param>
            public void AddIntersection(SweepLineSegment x, SweepLineSegment y)
            {
                if (x.RightCoordinate.X <= y.RightCoordinate.X)
                {
                    if (!_intersections.ContainsKey(x))
                        _intersections.Add(x, new HashSet<SweepLineSegment>());
                    _intersections[x].Add(y);
                }
                if (y.RightCoordinate.X <= x.RightCoordinate.X)
                {
                    if (!_intersections.ContainsKey(y))
                        _intersections.Add(y, new HashSet<SweepLineSegment>());
                    _intersections[y].Add(x);
                }
            }

            /// <summary>
            /// Removes the passed intersection points registered to the <paramref name="segment"/> as a key.
            /// </summary>
            /// <param name="segment">The key segment.</param>
            public void RemoveIntersection(SweepLineSegment segment)
            {
                _intersections.Remove(segment);
            }

            #endregion
        }

        /// <summary>
        /// Represents a Sweep line tree.
        /// </summary>
        private sealed class SweepLineTree : AvlTree<SweepLineSegment, SweepLineSegment>
        {
            #region Private fields

            private Node _currentNode;

            #endregion

            #region Public properties

            /// <summary>
            /// Gets the current Sweep line segment.
            /// </summary>
            public SweepLineSegment Current { get { return _currentNode.Key; } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLine.SweepLineTree"/> class.
            /// </summary>
            public SweepLineTree() : base(new SweepLineSegmentComparer()) { _currentNode = null; }

            #endregion

            #region Public methods

            /// <summary>
            /// Inserts the specified segment to the tree.
            /// </summary>
            /// <param name="segment">The segment.</param>
            /// <exception cref="System.ArgumentNullException">segment;The segment is null.</exception>
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
            /// Gets the previous (below) <see cref="SweepLineSegment"/> for the <see cref="Current"/> one.
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
            /// Gets the next (above) <see cref="SweepLineSegment"/> for the <see cref="Current"/> one.
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
        }

        #endregion

        #region Private fields

        private readonly IList<Coordinate> _source;
        private readonly IList<Int32> _endpoints;

        private readonly SweepLineTree _tree;
        private readonly IComparer<Coordinate> _coordinateComparer;

        #endregion

        #region Private properties

        /// <summary>
        /// Gets whether the source of the <see cref="SweepLine"/> contains any closed line string.
        /// </summary>
        private Boolean HasSourcedClosed
        {
            get { return _endpoints != null && _endpoints.Count > 0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLine"/> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a line string.</param>
        public SweepLine(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source;
            _tree = new SweepLineTree();
            _coordinateComparer = new CoordinateComparer();

            if (source.Count >= 2 && source.First() == source.Last())
                _endpoints = new List<Int32> { 0, source.Count - 2 };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.Collections.SweepLine.SweepLine"/> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        public SweepLine(IEnumerable<IList<Coordinate>> source)
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

            _tree = new SweepLineTree();
            _coordinateComparer = new CoordinateComparer();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a new event to the Sweep line.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The Sweep line segment created by addition of <paramref name="event" />.</returns>
        /// <exception cref="System.ArgumentNullException">e;The event is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The event is not a left event.;e
        /// or
        /// The edge of the event is less than 0.;e
        /// or
        /// The edge of the event is greater than the number of edges in the source.;e
        /// </exception>
        public SweepLineSegment Add(EndPointEvent e)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (e == null)
                throw new ArgumentNullException("e", "The event is null.");
            if (e.Type != EventType.Left)
                throw new ArgumentException("The event is not a left event.", "e");
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
        /// Searches the Sweep line for an event.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The segment associated with the event.</returns>
        /// <exception cref="System.ArgumentNullException">e;The event is null.</exception>
        public SweepLineSegment Search(EndPointEvent e)
        {
            if (e == null)
                throw new ArgumentNullException("e", "The event is null.");
            if (e.Type != EventType.Right)
                throw new ArgumentException("The event is not a right event.", "e");
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
        /// Removes the specified Sweep line segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <exception cref="System.ArgumentNullException">segment;The segment is null.</exception>
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
            {
                segmentAbove.Below = segment.Below;
            }
            if (segmentBelow != null)
            {
                segmentBelow.Above = segment.Above;
            }

            _tree.Remove(segment);

            var comparer = ((SweepLineSegmentComparer)_tree.Comparer);
            comparer.RemoveIntersection(segment);
        }

        /// <summary>
        /// Performs the order modifying effect of a possible intersection point between two directly adjacent segments.
        /// </summary>
        /// <remarks>
        /// An intersection event may become invalid if the order of the segments were altered or the intersection point have been already passed by the sweepline since the enqueuing of the event.
        /// This method is safe to be applied for invalid intersections.
        /// </remarks>
        /// <param name="x">First segment.</param>
        /// <param name="y">Second segment.</param>
        /// <returns><c>true</c> if a new, valid intersection point was found and passed between <paramref name="x"/> and <paramref name="y"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Segment <paramref name="x"/> and <paramref name="y"/> do not intersect each other.</exception>
        public Boolean Intersect(SweepLineSegment x, SweepLineSegment y)
        {
            if (!LineAlgorithms.Intersects(x.LeftCoordinate, x.RightCoordinate,
                                           y.LeftCoordinate, y.RightCoordinate))
                throw new InvalidOperationException("The given segments do not intersect each other.");

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

            var comparer = ((SweepLineSegmentComparer)_tree.Comparer);
            if (comparer.HasIntersection(x, y))
                return false;

            _tree.Remove(x);
            _tree.Remove(y);
            comparer.AddIntersection(x, y);
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
        /// Determines whether 2 edges of a line string or a polygon shell are adjacent.
        /// </summary>
        /// <param name="xEdge">Identifier of the first edge.</param>
        /// <param name="yEdge">Identifier of the second edge.</param>
        /// <returns><c>true</c> if the 2 edges are adjacent; otherwise, <c>false</c>.</returns>
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
    }
}

