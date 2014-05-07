/// <copyright file="SweepLine.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Collections.SearchTree;

namespace ELTE.AEGIS.Collections.SweepLine
{
    /// <summary>
    /// Represents a Sweep line.
    /// </summary>
    public class SweepLine
    {
        #region Private types

        /// <summary>
        /// Represents a comparer for <see cref="SweepLineSegment" /> instances.
        /// </summary>
        private sealed class SweepLineSegmentComparer : IComparer<SweepLineSegment>
        {
            #region IComparer methods

            /// <summary>
            /// Compares two <see cref="SweepLine.SweepLineSegment" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first <see cref="SweepLine.SweepLineSegment" /> to compare.</param>
            /// <param name="y">The second <see cref="SweepLine.SweepLineSegment" /> to compare.</param>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />.</returns>
            public Int32 Compare(SweepLineSegment x, SweepLineSegment y)
            {
                if (x.LeftCoordinate.X < y.LeftCoordinate.X) return -1;
                if (x.LeftCoordinate.X > y.LeftCoordinate.X) return 1;
                if (x.LeftCoordinate.Y < y.LeftCoordinate.Y) return -1;
                if (x.LeftCoordinate.Y > y.LeftCoordinate.Y) return 1;
                if (x.RightCoordinate.X < y.RightCoordinate.X) return -1;
                if (x.RightCoordinate.X > y.RightCoordinate.X) return 1;
                if (x.RightCoordinate.Y < y.RightCoordinate.Y) return -1;
                if (x.RightCoordinate.Y > y.RightCoordinate.Y) return 1;
                return 0;
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
            /// Initializes a new instance of the <see cref="SweepLine.SweepLineTree" /> class.
            /// </summary>
            public SweepLineTree() : base(new SweepLineSegmentComparer()) { _currentNode = null; }

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
            /// Sets the current segment.
            /// </summary>
            /// <param name="segment">The sweepline segment.</param>
            public void SetCurrent(SweepLineSegment segment)
            {
                _currentNode = SearchNode(segment);
            }

            /// <summary>
            /// Gets the previous segment.
            /// </summary>
            /// <returns>The previous sweepline segment.</returns>
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
            /// Gets the next segment.
            /// </summary>
            /// <returns>The next sweepline segment.</returns>
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
        private readonly SweepLineTree _tree;
        private readonly IComparer<Coordinate> _coordinateComparer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SweepLine" /> class.
        /// </summary>
        /// <param name="source">The source coordinates representing a line string.</param>
        public SweepLine(IList<Coordinate> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = source;
            _tree = new SweepLineTree();
            _coordinateComparer = new CoordinateComparer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SweepLine" /> class.
        /// </summary>
        /// <param name="source">The source coordinates.</param>
        public SweepLine(IEnumerable<IList<Coordinate>> source)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _source = new List<Coordinate>();

            foreach (IList<Coordinate> coordinateList in source)
            {
                if (coordinateList == null || coordinateList.Count < 2)
                    continue;

                for (Int32 i = 0; i <= coordinateList.Count - 1; i++)
                {
                    _source.Add(coordinateList[i]);
                }
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
        /// <returns>The Sweep line segment created by addition of <paramref name="e" />.</returns>
        /// <exception cref="System.ArgumentNullException">The event is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The event is not a left event.
        /// or
        /// The edge of the event is less than 0.
        /// or
        /// The edge of the event is greater than the number of edges in the source.
        /// </exception>
        public SweepLineSegment Add(Event e)
        {
            // source: http://geomalgorithms.com/a09-_intersect-3.html

            if (e == null)
                throw new ArgumentNullException("e", "The event is null.");
            if (e.Type == EventType.Right)
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
        /// <exception cref="System.ArgumentNullException">The event is null.</exception>
        public SweepLineSegment Search(Event e)
        {
            if (e == null)
                throw new ArgumentNullException("e", "The event is null.");

            if (e.Edge < 0 || e.Edge >= _source.Count - 1 || e.Type == EventType.Left)
                return null;

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
            {
                segmentAbove.Below = segment.Below;
            }
            if (segmentBelow != null)
            {
                segmentBelow.Above = segment.Above;
            }

            _tree.Remove(segment);
        }

        #endregion
    }
}
