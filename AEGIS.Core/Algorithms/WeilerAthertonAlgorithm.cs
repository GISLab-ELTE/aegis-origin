/// <copyright file="WeilerAthertonAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents the Weiler-Atherton clipping algorithm for determining intersection of line strings.
    /// </summary>
    /// <remarks>
    /// The Weiler-Atherton algorithm is capable of clipping a concave polygon with interior holes to the boundaries of another concave polygon, also with interior holes.
    /// For description about the algorithm see <a href="http://cs1.bradley.edu/public/jcm/weileratherton.html">here</a> or <a href="http://www.anirudh.net/practical_training/main/node10.html">here</a>.
    /// </remarks>
    public class WeilerAthertonAlgorithm
    {
        #region Private types

        /// <summary>
        /// Defines the kinds of the intersection points.
        /// </summary>
        private enum IntersectionMode
        {
            /// <summary>
            /// Indicates that the intersection is an entry point.
            /// </summary>
            Entry,

            /// <summary>
            /// Indicates that the intersection is an exit point.
            /// </summary>
            Exit,

            /// <summary>
            /// Indicates that the intersection is neither entry, nor exit.
            /// </summary>
            Virtual
        }

        /// <summary>
        /// Represents the descriptor of an intersection point.
        /// </summary>
        private class IntersectionElement
        {
            /// <summary>
            /// Gets or sets the position of the intersection.
            /// </summary>
            /// <value>The position.</value>
            public Coordinate Position { get; set; }

            /// <summary>
            /// Gets or sets the link to corresponding node for the first subject polygon.
            /// </summary>
            /// <value>The node a.</value>
            public LinkedListNode<Coordinate> NodeA { get; set; }

            /// <summary>
            /// Gets or sets the link to corresponding node for the second subject polygon.
            /// </summary>
            /// <value>The node b.</value>
            public LinkedListNode<Coordinate> NodeB { get; set; }

            /// <summary>
            /// Gets or sets the kind of the intersection.
            /// </summary>
            /// <value>The mode.</value>
            public IntersectionMode Mode { get; set; }
        }

        /// <summary>
        /// Represents a collection of intersection elements indexed by their positions.
        /// </summary>
        private class IntersectionCollection : KeyedCollection<Coordinate, IntersectionElement>
        {
            /// <summary>
            /// Extracts the key from the specified element.
            /// </summary>
            /// <returns>
            /// The key for the specified element.
            /// </returns>
            /// <param name="item">The element from which to extract the key.</param>
            protected override Coordinate GetKeyForItem(IntersectionElement item)
            {
                return item.Position;
            }
        }

        /// <summary>
        /// Represents a polygon clip.
        /// </summary>
        private class Clip
        {
            /// <summary>
            /// The shell of the clip.
            /// </summary>
            public IList<Coordinate> Shell { get; set; }

            /// <summary>
            /// The holes of the clip.
            /// </summary>
            public List<IList<Coordinate>> Holes { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Clip" /> class.
            /// </summary>
            public Clip()
                : this(null, null)
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="Clip" /> class.
            /// </summary>
            /// <param name="shell">The shell.</param>
            public Clip(IList<Coordinate> shell)
                : this(shell, null)
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="Clip" /> class.
            /// </summary>
            /// <param name="shell">The shell.</param>
            /// <param name="holes">The holes.</param>
            public Clip(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes)
            {
                Shell = shell;

                if (holes == null)
                    Holes = new List<IList<Coordinate>>();
                else
                    Holes = new List<IList<Coordinate>>(holes);
            }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The first polygon.
        /// </summary>
        private readonly IBasicPolygon _polygonA;

        /// <summary>
        /// The second polygon.
        /// </summary>
        private readonly IBasicPolygon _polygonB;

        /// <summary>
        /// The intersection collection.
        /// </summary>
        private IntersectionCollection _intersections;

        /// <summary>
        /// The shell of the first polygon.
        /// </summary>
        private LinkedList<Coordinate> _shellA;

        /// <summary>
        /// The shell of the second polygon.
        /// </summary>
        private LinkedList<Coordinate> _shellB;

        /// <summary>
        /// The array containing the holes of the first polygon.
        /// </summary>
        private LinkedList<Coordinate>[] _holesA;

        /// <summary>
        /// The array containing the holes of the second polygon.
        /// </summary>
        private LinkedList<Coordinate>[] _holesB;

        /// <summary>
        /// The list of hole indices contained within the first polygon.
        /// </summary>
        private List<Int32> _containedHoleIndicesA;

        /// <summary>
        /// The list of hole indices contained within the second polygon.
        /// </summary>
        private List<Int32> _containedHoleIndicesB;

        /// <summary>
        /// The list of internal clips.
        /// </summary>
        private List<Clip> _internalClips;

        /// <summary>
        /// The list of external clips for the first polygon.
        /// </summary>
        private List<Clip> _externalClipsA;

        /// <summary>
        /// The list of external clips for the second polygon.
        /// </summary>
        private List<Clip> _externalClipsB;

        /// <summary>
        /// A vlaue indicating whether the found intersections are phony.
        /// </summary>
        private Boolean _phonyIntersections;

        /// <summary>
        /// A value indicating whether the algorithm has computed the result.
        /// </summary>
        private Boolean _hasResult;

        /// <summary>
        /// A value indicating whether to compute the external clips.
        /// </summary>
        private Boolean _computeExternalClips;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the first polygon.
        /// </summary>
        /// <value>The first polygon.</value>
        public IBasicPolygon FirstPolygon { get { return _polygonA; } }

        /// <summary>
        /// Gets the second polygon.
        /// </summary>
        /// <value>The second polygon.</value>
        public IBasicPolygon SecondPolygon { get { return _polygonB; } }

        /// <summary>
        /// A value indicating whether to compute the external clips of the polygons.
        /// </summary>
        /// <value><c>true</c> if the algorithm should compute the external clips of the polygons, otherwise <c>false</c>.</value>
        public Boolean ComputeExternalClips 
        { 
            get { return _computeExternalClips; }
            set
            {
                if (_computeExternalClips != value)
                {
                    _computeExternalClips = value;

                    if (_computeExternalClips)
                    {
                        _hasResult = false;
                    }
                    else
                    {
                        _externalClipsA = null;
                        _externalClipsB = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the internal clips.
        /// </summary>
        /// <value>The list of polygons representing the internal clips of the two subject polygons.</value>
        public IList<IBasicPolygon> InternalClips
        {
            get
            {
                if (!_hasResult) 
                    Compute();
                return _internalClips.Select(clip => new BasicPolygon(clip.Shell, clip.Holes)).ToList<IBasicPolygon>();
            }
        }

        /// <summary>
        /// Gets the external clips for the first polygon.
        /// </summary>
        /// <value>The list of polygons representing the external clips of the first subject polygon.</value>
        public IList<IBasicPolygon> ExternalClipsA
        {
            get
            {
                if (!_hasResult) 
                    Compute();
                return _externalClipsA.Select(clip => new BasicPolygon(clip.Shell, clip.Holes)).ToList<IBasicPolygon>();
            }
        }

        /// <summary>
        /// Gets the external clips for the second polygon.
        /// </summary>
        /// <value>The list of polygons representing the external clips of the second subject polygon.</value>
        public IList<IBasicPolygon> ExternalClipsB
        {
            get
            {
                if (!_hasResult) 
                    Compute();
                return _externalClipsB.Select(clip => new BasicPolygon(clip.Shell, clip.Holes)).ToList<IBasicPolygon>();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm" /> class.
        /// </summary>
        /// <param name="polygonA">The first polygon.</param>
        /// <param name="polygonB">The second polygon.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public WeilerAthertonAlgorithm(IBasicPolygon polygonA, IBasicPolygon polygonB)
            : this(polygonA, polygonB, true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm" /> class.
        /// </summary>
        /// <param name="polygonA">The first polygon.</param>
        /// <param name="polygonB">The second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public WeilerAthertonAlgorithm(IBasicPolygon polygonA, IBasicPolygon polygonB, Boolean computeExternalClips)
        {
            if (polygonA == null)
                throw new ArgumentNullException("first", "The first polygon is null.");
            if (polygonB == null)
                throw new ArgumentNullException("second", "The second polygon is null.");

            _polygonA = polygonA;
            _polygonB = polygonB;
            _hasResult = false;
            _computeExternalClips = computeExternalClips;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm" /> class.
        /// </summary>
        /// <param name="shellA">The shell of the first polygon.</param>
        /// <param name="shellB">The shell of the second polygon.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell of the first polygon is null.
        /// or
        /// The shell of the second polygon is null.
        /// </exception>
        public WeilerAthertonAlgorithm(IList<Coordinate> shellA, IList<Coordinate> shellB)
            : this(shellA, shellB, true) 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm" /> class.
        /// </summary>
        /// <param name="shellA">The shell of the first polygon.</param>
        /// <param name="shellB">The shell of the second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell of the first polygon is null.
        /// or
        /// The shell of the second polygon is null.
        /// </exception>
        public WeilerAthertonAlgorithm(IList<Coordinate> shellA, IList<Coordinate> shellB, Boolean computeExternalClips)
        {
            if (shellA == null)
                throw new ArgumentNullException("shellA", "The shell of the first polygon is null.");            
            if (shellB == null)
                throw new ArgumentNullException("shellB", "The shell of the second polygon is null.");
            
            _polygonA = new BasicPolygon(shellA);
            _polygonB = new BasicPolygon(shellB);
            _hasResult = false;
            _computeExternalClips = computeExternalClips;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public WeilerAthertonAlgorithm(IList<Coordinate> firstShell, IEnumerable<IList<Coordinate>> firstHoles, IList<Coordinate> secondShell, IEnumerable<IList<Coordinate>> secondHoles)
            : this (firstShell, firstHoles, secondShell, secondHoles, true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public WeilerAthertonAlgorithm(IList<Coordinate> firstShell, IEnumerable<IList<Coordinate>> firstHoles, IList<Coordinate> secondShell, IEnumerable<IList<Coordinate>> secondHoles, Boolean computeExternalClips)
        {
            if (firstShell == null)
                throw new ArgumentNullException("firstShell", "The shell of the first polygon is null.");
            if (secondShell == null)
                throw new ArgumentNullException("secondShell", "The shell of the second polygon is null.");

            _polygonA = new BasicPolygon(firstShell, firstHoles);
            _polygonB = new BasicPolygon(secondShell, secondHoles);
            _hasResult = false;
            _computeExternalClips = computeExternalClips;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the result of the initialized clipping algorithm.
        /// </summary>
        public void Compute()
        {
            Initialize();
            FindIntersections();

            // Determine contained holes with no intersection points.
            _containedHoleIndicesA = Enumerable.Range(0, _polygonA.HoleCount).ToList();
            for (Int32 holeIndex = 0; holeIndex < _holesA.Length; ++holeIndex)
                if (_intersections.Any(intersection => _holesA[holeIndex].Contains(intersection.Position)))
                {
                    _containedHoleIndicesA.Remove(holeIndex);
                }

            _containedHoleIndicesB = Enumerable.Range(0, _polygonB.HoleCount).ToList();
            for (Int32 holeIndex = 0; holeIndex < _holesB.Length; ++holeIndex)
                if (_intersections.Any(intersection => _holesB[holeIndex].Contains(intersection.Position)))
                {
                    _containedHoleIndicesB.Remove(holeIndex);
                }

            // Intersection points exist.
            if (_intersections.Count > 0)
            {
                ComputeInternalClips();

                if (_computeExternalClips)
                {
                    ComputeExternalClipsA();
                    ComputeExternalClipsB();
                }

                // Holes created by intersecting holes.
                ComputeInternalClipHoles();

                _intersections.Clear();
            }
            else // No intersection point found.
            {
                ComputeCompleteClips();
            }

            // Dealing with contained holes (not intersected in any way).
            ComputeContainedHolesA();

            if (_hasResult)
                return;
            
            ComputeContainedHolesB();
            
            _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the computation.
        /// </summary>
        private void Initialize()
        {
            // Initialize results.

            _intersections = new IntersectionCollection();
            _internalClips = new List<Clip>();
            _externalClipsA = new List<Clip>();
            _externalClipsB = new List<Clip>();
            _phonyIntersections = false;

            // Initialize lists.

            _shellA = new LinkedList<Coordinate>(_polygonA.Shell);
            _shellA.RemoveLast();
            _shellB = new LinkedList<Coordinate>(_polygonB.Shell);
            _shellB.RemoveLast();

            _holesA = new LinkedList<Coordinate>[_polygonA.HoleCount];
            for (Int32 holeIndex = 0; holeIndex < _polygonA.HoleCount; ++holeIndex)
            {
                _holesA[holeIndex] = new LinkedList<Coordinate>(_polygonA.Holes[holeIndex]);
                _holesA[holeIndex].RemoveLast();
            }

            _holesB = new LinkedList<Coordinate>[_polygonB.HoleCount];
            for (Int32 holeIndex = 0; holeIndex < _polygonB.HoleCount; ++holeIndex)
            {
                _holesB[holeIndex] = new LinkedList<Coordinate>(_polygonB.Holes[holeIndex]);
                _holesB[holeIndex].RemoveLast();
            }
        }

        /// <summary>
        /// Finds the intersections of the subject polygons.
        /// </summary>
        private void FindIntersections()
        {
            // Look for intersections.
            FindIntersections(_polygonA.Shell.Coordinates, _shellA, _polygonB.Shell.Coordinates, _shellB);

            for (Int32 holeIndexA = 0; holeIndexA < _polygonA.HoleCount; ++holeIndexA)
            {
                FindIntersections(_polygonA.Holes[holeIndexA].Coordinates, _holesA[holeIndexA], _polygonB.Shell.Coordinates, _shellB);
            }

            for (Int32 holeIndexB = 0; holeIndexB < _polygonB.HoleCount; ++holeIndexB)
            {
                FindIntersections(_polygonA.Shell.Coordinates, _shellA, _polygonB.Holes[holeIndexB].Coordinates, _holesB[holeIndexB]);
            }

            for (Int32 holeIndexA = 0; holeIndexA < _polygonA.HoleCount; ++holeIndexA)
                for (Int32 holeIndexB = 0; holeIndexB < _polygonB.HoleCount; ++holeIndexB)
                    FindIntersections(_polygonA.Holes[holeIndexA].Coordinates, _holesA[holeIndexA], _polygonB.Holes[holeIndexB].Coordinates, _holesB[holeIndexB]);

            // If intersection points exist.
            if (_intersections.Count > 0)
            {
                // Entering / Exiting intersection point separation (from A to B).
                LinkedListNode<Coordinate> currentNode = _shellA.First;
                while (currentNode != null)
                {
                    if (_intersections.Contains(currentNode.Value))
                    {
                        LinkedListNode<Coordinate> prevNode = currentNode.Previous ?? _shellA.Last;
                        LinkedListNode<Coordinate> nextNode = currentNode.Next ?? _shellA.First;

                        Coordinate prevCentroid = LineAlgorithms.Centroid(currentNode.Value, prevNode.Value);
                        Coordinate nextCentroid = LineAlgorithms.Centroid(currentNode.Value, nextNode.Value);

                        if (!PolygonAlgorithms.InInterior(_polygonB, prevCentroid) &&
                            PolygonAlgorithms.InInterior(_polygonB, nextCentroid))
                        {
                            _intersections[currentNode.Value].Mode = IntersectionMode.Entry;
                        }
                        else if (!PolygonAlgorithms.InExterior(_polygonB, prevCentroid) &&
                                 PolygonAlgorithms.InExterior(_polygonB, nextCentroid))
                        {
                            _intersections[currentNode.Value].Mode = IntersectionMode.Exit;
                        }
                        else if (PolygonAlgorithms.InExterior(_polygonB, prevCentroid) &&
                                 PolygonAlgorithms.InExterior(_polygonB, nextCentroid))
                        {
                            _intersections.Remove(currentNode.Value);
                        }
                        else
                        {
                            _intersections[currentNode.Value].Mode = IntersectionMode.Virtual;
                        }

                    }
                    currentNode = currentNode.Next;
                }

                foreach (LinkedList<Coordinate> listHole in _holesA)
                {
                    currentNode = listHole.First;
                    while (currentNode != null)
                    {
                        if (_intersections.Contains(currentNode.Value))
                        {
                            LinkedListNode<Coordinate> prevNode = currentNode.Previous ?? listHole.Last;
                            LinkedListNode<Coordinate> nextNode = currentNode.Next ?? listHole.First;

                            Coordinate prevCentroid = LineAlgorithms.Centroid(currentNode.Value, prevNode.Value);
                            Coordinate nextCentroid = LineAlgorithms.Centroid(currentNode.Value, nextNode.Value);

                            if (!PolygonAlgorithms.InInterior(_polygonB, prevCentroid) &&
                                PolygonAlgorithms.InInterior(_polygonB, nextCentroid))
                                _intersections[currentNode.Value].Mode = IntersectionMode.Entry;

                            else if (!PolygonAlgorithms.InExterior(_polygonB, prevCentroid) &&
                                     PolygonAlgorithms.InExterior(_polygonB, nextCentroid))
                                _intersections[currentNode.Value].Mode = IntersectionMode.Exit;

                            else if (PolygonAlgorithms.InExterior(_polygonB, prevCentroid) &&
                                     PolygonAlgorithms.InExterior(_polygonB, nextCentroid))
                                _intersections.Remove(currentNode.Value);

                            else
                                _intersections[currentNode.Value].Mode = IntersectionMode.Virtual;
                        }
                        currentNode = currentNode.Next;
                    }
                }
            }

            // Detect false intersection between subject polygons.
            // Tangent relationship may result non-entering intersection points.
            if (_intersections.Count > 0 &&
                _intersections.FirstOrDefault(intersection => intersection.Mode == IntersectionMode.Entry) == null)
            {
                _intersections.Clear();
                _phonyIntersections = true;
            }
        }

        /// <summary>
        /// Finds the intersections of the specified rings.
        /// </summary>
        /// <param name="ringA">The shell a.</param>
        /// <param name="listA">The list of coordinates in the first polygon.</param>
        /// <param name="ringB">The shell b.</param>
        /// <param name="listB">The list of coordinates in the second polygon.</param>
        private void FindIntersections(IList<Coordinate> ringA, LinkedList<Coordinate> listA, IList<Coordinate> ringB, LinkedList<Coordinate> listB)
        {
            if (Envelope.FromCoordinates(ringA).Disjoint(Envelope.FromCoordinates(ringB)))
                return;

            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(new List<IList<Coordinate>> { ringA, ringB });
            IList<Coordinate> positions = algorithm.Intersections;
            IList<Tuple<Int32, Int32>> edges = algorithm.EdgeIndices;

            for (Int32 i = 0; i < positions.Count; ++i)
            {
                if (_intersections.Contains(positions[i]))
                    continue;

                LinkedListNode<Coordinate> nodeA, nodeB;
                if (!ringA.Contains(positions[i]))
                {
                    // Insert intersection point into vertex lists
                    LinkedListNode<Coordinate> location = listA.Find(ringA[edges[i].Item1]);

                    // Find the proper position for the intersection point when multiple intersection occurs on a single edge
                    while (location.Next != null && _intersections.Contains(location.Next.Value) &&
                           (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;

                    nodeA = listA.AddAfter(location, positions[i]);
                }
                else
                {
                    nodeA = listA.Find(positions[i]);
                }

                if (!ringB.Contains(positions[i]))
                {
                    LinkedListNode<Coordinate> location = listB.Find(ringB[edges[i].Item2 - ringA.Count]);

                    while (location.Next != null && _intersections.Contains(location.Next.Value) &&
                           (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;

                    nodeB = listB.AddAfter(location, positions[i]);
                }
                else
                {
                    nodeB = listB.Find(positions[i]);
                }

                _intersections.Add(new IntersectionElement { Position = positions[i], NodeA = nodeA, NodeB = nodeB });
            }
        }

        /// <summary>
        /// Compute the internal clips.
        /// </summary>
        private void ComputeInternalClips()
        {
            List<Coordinate> checkPositions =
                    _intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry && intersection.NodeA.List == _shellA)
                                  .Select(intersection => intersection.Position).ToList();

            while (checkPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = _intersections[checkPositions[0]].NodeA;
                Boolean isFollowingA = true;

                shell.Add(current.Value);
                checkPositions.Remove(current.Value);

                do
                {
                    current = current.Next ?? current.List.First;
                    shell.Add(current.Value);

                    if (_intersections.Contains(current.Value))
                    {
                        LinkedListNode<Coordinate> next = current.Next ?? current.List.First;
                        LinkedListNode<Coordinate> prev = current.Previous ?? current.List.Last;
                        if (!(_intersections.Contains(next.Value) && _intersections.Contains(prev.Value) &&
                              (_intersections[next.Value].Mode == IntersectionMode.Entry &&
                               _intersections[prev.Value].Mode == IntersectionMode.Exit ||
                               _intersections[prev.Value].Mode == IntersectionMode.Entry &&
                               _intersections[next.Value].Mode == IntersectionMode.Exit))) 
                        {
                            isFollowingA = !isFollowingA;
                        }

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        checkPositions.Remove(current.Value);
                    }
                } while (current.Value != shell[0]);

                if (shell.Count > 3)
                    _internalClips.Add(new Clip(shell));
            }
        }

        /// <summary>
        /// Compute the external clips for the first polygon.
        /// </summary>
        private void ComputeExternalClipsA()
        {
            List<Coordinate> checkPositions = _intersections.Where(intersection => intersection.Mode == IntersectionMode.Exit)
                                                            .Select(intersection => intersection.Position).ToList();
            while (checkPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = _intersections[checkPositions[0]].NodeA;
                Boolean isFollowingA = true;

                shell.Add(current.Value);
                checkPositions.Remove(current.Value);

                do
                {
                    if (isFollowingA)
                        current = current.Next ?? current.List.First;
                    else
                        current = current.Previous ?? current.List.Last;
                    shell.Add(current.Value);

                    if (_intersections.Contains(current.Value))
                    {
                        LinkedListNode<Coordinate> next = current.Next ?? current.List.First;
                        LinkedListNode<Coordinate> prev = current.Previous ?? current.List.Last;

                        if (!(_intersections.Contains(next.Value) && _intersections.Contains(prev.Value) &&
                              (_intersections[next.Value].Mode == IntersectionMode.Entry &&
                               _intersections[prev.Value].Mode == IntersectionMode.Exit ||
                               _intersections[prev.Value].Mode == IntersectionMode.Entry &&
                               _intersections[next.Value].Mode == IntersectionMode.Exit)))
                        {
                            isFollowingA = !isFollowingA;
                        }

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        checkPositions.Remove(current.Value);
                    }
                } while (current.Value != shell[0]);

                if (shell.Count > 3)
                    _externalClipsA.Add(new Clip(shell));
            }
        }

        /// <summary>
        /// Compute the external clips for the second polygon.
        /// </summary>
        private void ComputeExternalClipsB()
        {
            List<Coordinate> checkPositions = checkPositions = _intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry)
                                                   .Select(intersection => intersection.Position).ToList();
            while (checkPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = _intersections[checkPositions[0]].NodeB;
                Boolean isFollowingA = false;

                shell.Add(current.Value);
                checkPositions.Remove(current.Value);

                do
                {
                    if (!isFollowingA)
                        current = current.Next ?? current.List.First;
                    else
                        current = current.Previous ?? current.List.Last;
                    shell.Add(current.Value);

                    if (_intersections.Contains(current.Value))
                    {
                        LinkedListNode<Coordinate> next = current.Next ?? current.List.First;
                        LinkedListNode<Coordinate> prev = current.Previous ?? current.List.Last;

                        if (!(_intersections.Contains(next.Value) && _intersections.Contains(prev.Value) &&
                              (_intersections[next.Value].Mode == IntersectionMode.Entry &&
                               _intersections[prev.Value].Mode == IntersectionMode.Exit ||
                               _intersections[prev.Value].Mode == IntersectionMode.Entry &&
                               _intersections[next.Value].Mode == IntersectionMode.Exit)))
                        {
                            isFollowingA = !isFollowingA;
                        }

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        checkPositions.Remove(current.Value);
                    }
                } while (current.Value != shell[0]);

                if (shell.Count > 3)
                    _externalClipsB.Add(new Clip(shell));
            }
        }

        /// <summary>
        /// Completes complete clips (in case of no intersection).
        /// </summary>
        public void ComputeCompleteClips()
        {
            Boolean isAinB = _polygonA.Shell.Coordinates.All(position => !PolygonAlgorithms.InExterior(_polygonB, position));
            Boolean isBinA = _polygonB.Shell.Coordinates.All(position => !PolygonAlgorithms.InExterior(_polygonA, position));

            List<Coordinate> finalShellA = _shellA.ToList();
            finalShellA.Add(finalShellA[0]);

            List<Coordinate> finalShellB = _shellB.ToList();
            finalShellB.Add(finalShellB[0]);

            if (isAinB && !_phonyIntersections) // B contains A
            {
                _internalClips.Add(new Clip(finalShellA));
                _externalClipsB.Add(new Clip(finalShellB));
            }
            else if (isBinA && !_phonyIntersections) // A contains B
            {
                _internalClips.Add(new Clip(finalShellB));
                _externalClipsA.Add(new Clip(finalShellA));
            }
            else // A and B are distinct
            {
                _externalClipsA.Add(new Clip(finalShellA));
                _externalClipsB.Add(new Clip(finalShellB));
            }
        }

        /// <summary>
        /// Compute the holes contained within the first polygon.
        /// </summary>
        private void ComputeContainedHolesA() 
        {
            foreach (Int32 index in _containedHoleIndicesA)
            {
                foreach (Clip clip in _internalClips)
                    if (!PolygonAlgorithms.InExterior(clip.Shell, _polygonA.Holes[index].Coordinates[0]))
                    {
                        _externalClipsB.Add(new Clip { Shell = _polygonA.Holes[index].Coordinates });
                        AddHoleToResult(_internalClips, _polygonA.Holes[index].Coordinates);
                        ComputeContainedHolesB();

                        if (_hasResult)
                            return;
                    }

                foreach (Clip clip in _externalClipsA)
                    if (!PolygonAlgorithms.InExterior(clip.Shell, _polygonA.Holes[index].Coordinates[0]))
                    {
                        AddHoleToResult(_externalClipsA, _polygonA.Holes[index].Coordinates);
                        ComputeContainedHolesB();

                        if (_hasResult)
                            return;
                    }
            }
        }

        /// <summary>
        /// Compute the holes contained within the second polygon.
        /// </summary>
        private void ComputeContainedHolesB()
        {
            foreach (Int32 index in _containedHoleIndicesB)
            {
                foreach(Clip clip in _internalClips)
                    if (!PolygonAlgorithms.InExterior(clip.Shell, _polygonB.Holes[index].Coordinates[0]))
                    {
                        _externalClipsA.Add(new Clip { Shell = _polygonB.Holes[index].Coordinates });
                        AddHoleToResult(_internalClips, _polygonB.Holes[index].Coordinates);
                        _hasResult = true;
                        return;
                    }

                foreach (Clip clip in _externalClipsB)
                    if (!PolygonAlgorithms.InExterior(clip.Shell, _polygonB.Holes[index].Coordinates[0]))
                    {
                        AddHoleToResult(_externalClipsB, _polygonB.Holes[index].Coordinates);
                        _hasResult = true;
                        return;
                    }
            }
        }

        /// <summary>
        /// Computes the holes of the internal clips.
        /// </summary>
        private void ComputeInternalClipHoles()
        {
            List<Coordinate> checkPositions =
                new List<Coordinate>(_intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry &&
                                                                          intersection.NodeA.List != _shellA &&
                                                                          intersection.NodeB.List != _shellB)
                                                   .Select(intersection => intersection.Position));
            while (checkPositions.Count > 0)
            {
                List<Coordinate> hole = new List<Coordinate>();
                LinkedListNode<Coordinate> current = _intersections[checkPositions[0]].NodeA;
                Boolean isFollowingA = true;

                hole.Add(current.Value);
                checkPositions.Remove(current.Value);

                do
                {
                    current = current.Previous ?? current.List.Last;
                    hole.Add(current.Value);

                    if (_intersections.Contains(current.Value))
                    {
                        LinkedListNode<Coordinate> next = current.Next ?? current.List.First;
                        LinkedListNode<Coordinate> prev = current.Previous ?? current.List.Last;
                        if (!(_intersections.Contains(next.Value) && _intersections.Contains(prev.Value) &&
                              (_intersections[next.Value].Mode == IntersectionMode.Entry &&
                               _intersections[prev.Value].Mode == IntersectionMode.Exit ||
                               _intersections[prev.Value].Mode == IntersectionMode.Entry &&
                               _intersections[next.Value].Mode == IntersectionMode.Exit)))
                            isFollowingA = !isFollowingA;

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        checkPositions.Remove(current.Value);
                    }
                } while (current.Value != hole[0]);

                AddHoleToResult(_internalClips, hole);
            }
        }



        /// <summary>
        /// Adds the hole to the corresponding element in a shell set.
        /// </summary>
        /// <param name="resultSet">The result set.</param>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the hole was added to a shell; otherwise <c>false</c>.</returns>
        /// <remarks>
        /// The method will look for the first shell in a linear search that contains the hole and execute the addition.
        /// </remarks>
        private Boolean AddHoleToResult(IList<Clip> resultSet, IList<Coordinate> hole)
        {

            for (Int32 resultIndex = 0; resultIndex < resultSet.Count; resultIndex++)
            {
                if (hole.All(coordinate => !PolygonAlgorithms.InExterior(resultSet[resultIndex].Shell, coordinate)))
                {

                    resultSet[resultIndex].Holes.Add(hole);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="polygonA">The first polygon.</param>
        /// <param name="polygonB">The second polygon.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        public static IList<IBasicPolygon> Intersection(IBasicPolygon polygonA, IBasicPolygon polygonB)
        {
            return new WeilerAthertonAlgorithm(polygonA, polygonB, false).InternalClips;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="shellA">The shell of the first polygon.</param>
        /// <param name="shellB">The shell of the second polygon.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell of the first polygon is null.
        /// or
        /// The shell of the second polygon is null.
        /// </exception>
        public static IList<IBasicPolygon> Intersection(IList<Coordinate> shellA, IList<Coordinate> shellB)
        {
            return new WeilerAthertonAlgorithm(shellA, shellB, false).InternalClips;
        }

        #endregion
    }
}
