/// <copyright file="WeilerAthertonAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
        /// The kinds of the intersection points.
        /// </summary>
        private enum IntersectionMode
        {
            Unknown,
            Entry,
            Exit,
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

        #endregion

        #region Private fields

        /// <summary>
        /// The shell of the first polygon.
        /// </summary>
        private IList<Coordinate> _shellA;

        /// <summary>
        /// The shell of the second polygon.
        /// </summary>
        private IList<Coordinate> _shellB;

        /// <summary>
        /// The holes of the first polygon.
        /// </summary>
        private IList<IList<Coordinate>> _holesA;

        /// <summary>
        /// The holes of the second polygon.
        /// </summary>
        private IList<IList<Coordinate>> _holesB;

        /// <summary>
        /// The intersection collection.
        /// </summary>
        private IntersectionCollection _intersections;

        /// <summary>
        /// The list of internal clips.
        /// </summary>
        private List<Clip> _internal;

        /// <summary>
        /// The list of external clips for the first polygon.
        /// </summary>
        private List<Clip> _externalA;

        /// <summary>
        /// The list of external clips for the second polygon.
        /// </summary>
        private List<Clip> _externalB;

        /// <summary>
        /// A value indicating whether the algorithm has computed the result.
        /// </summary>
        private Boolean _hasResult;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the common, internal clip parts.
        /// </summary>
        /// <value>The polygon coordinates of the common, internal clips.</value>
        public IList<Clip> Internal
        {
            get
            {
                if (!_hasResult) Compute();
                return _internal;
            }
        }

        /// <summary>
        /// Gets the external clips for the first subject polygon.
        /// </summary>
        /// <value>The polygon coordinates of the external clips for the first subject polygon.</value>
        public IList<Clip> ExternalA
        {
            get
            {
                if (!_hasResult) Compute();
                return _externalA;
            }
        }

        /// <summary>
        /// Gets the external clips for the second subject polygon.
        /// </summary>
        /// <value>The polygon coordinates of the external clips for the second subject polygon.</value>
        public IList<Clip> ExternalB
        {
            get
            {
                if (!_hasResult) Compute();
                return _externalB;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm"/> class.
        /// </summary>
        /// <param name="shellA">The source coordinates representing the first subject polygon.</param>
        /// <param name="shellB">The source coordinates representing the second subject polygon.</param>
        /// <exception cref="ArgumentNullException">One or both of the subject polygons are null.</exception>
        /// <exception cref="ArgumentException">One or both of the subject polygons do not have enough coordinates.</exception>
        public WeilerAthertonAlgorithm(IList<Coordinate> shellA, IList<Coordinate> shellB)
            :this(shellA, null, shellB, null)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="WeilerAthertonAlgorithm"/> class.
        /// </summary>
        /// <param name="shellA">The source coordinates representing the first subject polygon.</param>
        /// <param name="shellB">The source coordinates representing the second subject polygon.</param>
        /// <param name="holesA">The source coordinates representing the holes in the first subject polygon.</param>
        /// <param name="holesB">The source coordinates representing the holes in the second subject polygon.</param>
        /// <exception cref="ArgumentNullException">One or both of the subject polygons are null.</exception>
        /// <exception cref="ArgumentException">One or both of the subject polygons do not have enough coordinates.</exception>
        public WeilerAthertonAlgorithm(IList<Coordinate> shellA, IList<IList<Coordinate>> holesA,
                                       IList<Coordinate> shellB, IList<IList<Coordinate>> holesB)
        {
            Initialize(shellA, holesA, shellB, holesB);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the result of the initialized clipping algorithm.
        /// </summary>
        public void Compute()
        {
            _intersections = new IntersectionCollection();
            _internal = new List<Clip>();
            _externalA = new List<Clip>();
            _externalB = new List<Clip>();
            Boolean falseIntersections = false;

            var listShellA = new LinkedList<Coordinate>(_shellA);
            var listShellB = new LinkedList<Coordinate>(_shellB);
            listShellA.RemoveLast();
            listShellB.RemoveLast();

            // Look for intersections.
            Intersect(_shellA, listShellA, _shellB, listShellB);

            var listHolesA = new LinkedList<Coordinate>[_holesA.Count];
            for (Int32 i = 0; i < _holesA.Count; ++i)
            {
                listHolesA[i] = new LinkedList<Coordinate>(_holesA[i]);
                listHolesA[i].RemoveLast();
                Intersect(_holesA[i], listHolesA[i], _shellB, listShellB);
            }

            var listHolesB = new LinkedList<Coordinate>[_holesB.Count];
            for (Int32 i = 0; i < _holesB.Count; ++i)
            {
                listHolesB[i] = new LinkedList<Coordinate>(_holesB[i]);
                listHolesB[i].RemoveLast();
                Intersect(_shellA, listShellA, _holesB[i], listHolesB[i]);
            }

            for (Int32 i = 0; i < _holesA.Count; ++i)
                for (Int32 j = 0; j < _holesB.Count; ++j)
                    Intersect(_holesA[i], listHolesA[i], _holesB[j], listHolesB[j]);

            // Intersection points exist.
            if (_intersections.Count > 0)
            {
                // Entering / Exiting intersection point separation (from A to B).
                var currentNode = listShellA.First;
                while (currentNode != null)
                {
                    if (_intersections.Contains(currentNode.Value))
                    {
                        var prevNode = currentNode.Previous ?? listShellA.Last;
                        var nextNode = currentNode.Next ?? listShellA.First;

                        var prevCentroid = LineAlgorithms.Centroid(currentNode.Value, prevNode.Value);
                        var nextCentroid = LineAlgorithms.Centroid(currentNode.Value, nextNode.Value);

                        if (!PolygonAlgorithms.InInterior(_shellB, _holesB, prevCentroid) &&
                            PolygonAlgorithms.InInterior(_shellB, _holesB, nextCentroid))
                            _intersections[currentNode.Value].Mode = IntersectionMode.Entry;

                        else if (!PolygonAlgorithms.InExterior(_shellB, _holesB, prevCentroid) &&
                                 PolygonAlgorithms.InExterior(_shellB, _holesB, nextCentroid))
                            _intersections[currentNode.Value].Mode = IntersectionMode.Exit;

                        else if (PolygonAlgorithms.InExterior(_shellB, _holesB, prevCentroid) &&
                                 PolygonAlgorithms.InExterior(_shellB, _holesB, nextCentroid))
                            _intersections.Remove(currentNode.Value);

                        else
                            _intersections[currentNode.Value].Mode = IntersectionMode.Virtual;


                    }
                    currentNode = currentNode.Next;
                }

                foreach (var listHole in listHolesA)
                {
                    currentNode = listHole.First;
                    while (currentNode != null)
                    {
                        if (_intersections.Contains(currentNode.Value))
                        {
                            var prevNode = currentNode.Previous ?? listHole.Last;
                            var nextNode = currentNode.Next ?? listHole.First;

                            var prevCentroid = LineAlgorithms.Centroid(currentNode.Value, prevNode.Value);
                            var nextCentroid = LineAlgorithms.Centroid(currentNode.Value, nextNode.Value);

                            if (!PolygonAlgorithms.InInterior(_shellB, _holesB, prevCentroid) &&
                                PolygonAlgorithms.InInterior(_shellB, _holesB, nextCentroid))
                                _intersections[currentNode.Value].Mode = IntersectionMode.Entry;

                            else if (!PolygonAlgorithms.InExterior(_shellB, _holesB, prevCentroid) &&
                                     PolygonAlgorithms.InExterior(_shellB, _holesB, nextCentroid))
                                _intersections[currentNode.Value].Mode = IntersectionMode.Exit;

                            else if (PolygonAlgorithms.InExterior(_shellB, _holesB, prevCentroid) &&
                                     PolygonAlgorithms.InExterior(_shellB, _holesB, nextCentroid))
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
                falseIntersections = true;
            }

            // Determine contained holes with no intersection points.
            var containedHolesA = new List<Int32>(_holesA.Count);
            containedHolesA.AddRange(Enumerable.Range(0, _holesA.Count));
            for (Int32 i = 0; i < listHolesA.Length; ++i)
                if (_intersections.Any(intersection => listHolesA[i].Contains(intersection.Position)))
                    containedHolesA.Remove(i);

            var containedHolesB = new List<Int32>(_holesB.Count);
            containedHolesB.AddRange(Enumerable.Range(0, _holesB.Count));
            for (Int32 i = 0; i < listHolesB.Length; ++i)
                if (_intersections.Any(intersection => listHolesB[i].Contains(intersection.Position)))
                    containedHolesB.Remove(i);

            // Intersection points exist.
            if (_intersections.Count > 0)
            {
                // Internal
                List<Coordinate> checkPositions =
                    _intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry &&
                                                         intersection.NodeA.List == listShellA)
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
                            var next = current.Next ?? current.List.First;
                            var prev = current.Previous ?? current.List.Last;
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
                    } while (current.Value != shell[0]);
                    if (shell.Count > 3) _internal.Add(new Clip(shell));
                }

                // External A
                checkPositions = _intersections.Where(intersection => intersection.Mode == IntersectionMode.Exit)
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
                            var next = current.Next ?? current.List.First;
                            var prev = current.Previous ?? current.List.Last;
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
                    } while (current.Value != shell[0]);
                    if (shell.Count > 3) _externalA.Add(new Clip(shell));
                }

                // External B
                checkPositions = _intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry)
                                               .Select(intersection => intersection.Position).ToList();
                while (checkPositions.Count > 0)
                {
                    var shell = new List<Coordinate>();
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
                            var next = current.Next ?? current.List.First;
                            var prev = current.Previous ?? current.List.Last;
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
                    } while (current.Value != shell[0]);
                    if (shell.Count > 3) _externalB.Add(new Clip(shell));
                }

                // Holes created by intersecting holes
                checkPositions =
                    new List<Coordinate>(_intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry &&
                                                                              intersection.NodeA.List != listShellA &&
                                                                              intersection.NodeB.List != listShellB)
                                                       .Select(intersection => intersection.Position));
                while (checkPositions.Count > 0)
                {
                    var hole = new List<Coordinate>();
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
                            var next = current.Next ?? current.List.First;
                            var prev = current.Previous ?? current.List.Last;
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
                    AddHoleToResult(_internal, hole);
                }

                _intersections.Clear();
            }
            else // No intersection point found
            {
                Boolean isAinB = _shellA.All(position => !PolygonAlgorithms.InExterior(_shellB, _holesB, position));
                Boolean isBinA = _shellB.All(position => !PolygonAlgorithms.InExterior(_shellA, _holesA, position));

                var shellA = listShellA.ToList();
                shellA.Add(shellA.First());

                var shellB = listShellB.ToList();
                shellB.Add(shellB.First());

                if (isAinB && !falseIntersections) // B contains A
                {
                    _internal.Add(new Clip(shellA));
                    _externalB.Add(new Clip(shellB));
                }
                else if (isBinA && !falseIntersections) // A contains B
                {
                    _internal.Add(new Clip(shellB));
                    _externalA.Add(new Clip(shellA));
                }
                else // A and B are distinct
                {
                    _externalA.Add(new Clip(shellA));
                    _externalB.Add(new Clip(shellB));
                }
            }

            // Dealing with contained holes (not intersected in any way).
            ContainedHolesA:

                foreach (Int32 index in containedHolesA)
                {
                    foreach (Clip clip in _internal)
                        if (!PolygonAlgorithms.InExterior(clip.Shell, _holesA[index][0]))
                        {
                            _externalB.Add(new Clip(_holesA[index]));
                            AddHoleToResult(_internal, _holesA[index]);
                            goto ContainedHolesB;
                        }

                    for (Int32 i = 0; i < _externalA.Count; ++i)
                        if (!PolygonAlgorithms.InExterior(_externalA[i].Shell, _holesA[index][0]))
                        {
                            AddHoleToResult(_externalA, _holesA[index]);
                            goto ContainedHolesB;
                        }
                }

            ContainedHolesB:

                foreach (Int32 index in containedHolesB)
                {
                    for (Int32 i = 0; i < _internal.Count; ++i)
                        if (!PolygonAlgorithms.InExterior(_internal[i].Shell, _holesB[index][0]))
                        {
                            _externalA.Add(new Clip(_holesB[index]));
                            AddHoleToResult(_internal, _holesB[index]);
                            goto ContainedHolesEnd;
                        }

                    for (Int32 i = 0; i < _externalB.Count; ++i)
                        if (!PolygonAlgorithms.InExterior(_externalB[i].Shell, _holesB[index][0]))
                        {
                            AddHoleToResult(_externalB, _holesB[index]);
                            goto ContainedHolesEnd;
                        }
                }

            ContainedHolesEnd:

                _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the <see cref="WeilerAthertonAlgorithm"/>.
        /// </summary>
        /// <param name="shellA">The source coordinates representing the first subject polygon.</param>
        /// <param name="shellB">The source coordinates representing the second subject polygon.</param>
        /// <param name="holesA">The source coordinates representing the holes in the first subject polygon.</param>
        /// <param name="holesB">The source coordinates representing the holes in the second subject polygon.</param>
        /// <exception cref="ArgumentNullException">One or both of the subject polygons are null.</exception>
        /// <exception cref="ArgumentException">One or both of the subject polygons do not have enough coordinates.</exception>
        private void Initialize(IList<Coordinate> shellA, IList<IList<Coordinate>> holesA,
                                  IList<Coordinate> shellB, IList<IList<Coordinate>> holesB)
        {
            // Shell A
            if (shellA == null)
                throw new ArgumentNullException("shellA", "The first subject is null.");
            if (shellA.Count < 4)
                throw new ArgumentException("The first subject must contain at least 3 different coordinates.", "shellA");
            if (!shellA[0].Equals(shellA[shellA.Count - 1]))
                throw new ArgumentException("The first and the last coordinates of the first subject must be equal.", "shellA");
            _shellA = shellA;

            // Shell B
            if (shellB == null)
                throw new ArgumentNullException("shellB", "The second subject is null.");
            if (shellB.Count < 4)
                throw new ArgumentException("The second subject must contain at least 3 different coordinates.", "shellB");
            if (!shellB[0].Equals(shellB[shellB.Count - 1]))
                throw new ArgumentException("The first and the last coordinates of the second subject must be equal.", "shellB");
            _shellB = shellB;

            // Holes in subject A
            if (holesA != null)
            {
                foreach (IList<Coordinate> hole in holesA)
                {
                    if (hole == null)
                        throw new ArgumentNullException("holesA", "A hole in the first subject is null.");
                    if (hole.Count < 4)
                        throw new ArgumentException("A hole in the first subject does not contain at least 3 different coordinates.", "holesA");
                    if (!hole[0].Equals(hole[hole.Count - 1]))
                        throw new ArgumentException("The first and the last coordinates of a hole in the first subject are not equal.", "holesA");
                }
                _holesA = holesA;
            }
            else
                _holesA = new List<IList<Coordinate>>();

            // Holes in subject B
            if (holesB != null)
            {
                foreach (IList<Coordinate> hole in holesB)
                {
                    if (hole == null)
                        throw new ArgumentNullException("holesB", "A hole in the second subject is null.");
                    if (hole.Count < 4)
                        throw new ArgumentException("A hole in the  second subject does not contain at least 3 different coordinates.", "holesB");
                    if (!hole[0].Equals(hole[hole.Count - 1]))
                        throw new ArgumentException( "The first and the last coordinates of a hole in the second subject are not equal.", "holesB");
                }
                _holesB = holesB;
            }
            else
                _holesB = new List<IList<Coordinate>>();

            _hasResult = false;
        }

        /// <summary>
        /// Calculates and adds the found intersections to the collection <see cref="_intersections"/> between 2 polygons with holes.
        /// </summary>
        /// <param name="subjectA">The shell of the first subject polygon.</param>
        /// <param name="subjectB">The shell of the second subject polygon.</param>
        /// <param name="listA">The linked list of the nodes for the first subject polygon.</param>
        /// <param name="listB">The linked list of the nodes for the second subject polygon.</param>
        private void Intersect(IList<Coordinate> subjectA, LinkedList<Coordinate> listA,
                                 IList<Coordinate> subjectB, LinkedList<Coordinate> listB)
        {
            var algorithm = new BentleyOttmannAlgorithm(new[] {subjectA, subjectB});
            var positions = algorithm.Result;
            var edges = algorithm.Edges;

            for (Int32 i = 0; i < positions.Count; ++i)
            {
                if (_intersections.Contains(positions[i]))
                    continue;

                LinkedListNode<Coordinate> nodeA, nodeB;
                if (!subjectA.Contains(positions[i]))
                {
                    // Insert intersection point into vertex lists
                    var location = listA.Find(subjectA[edges[i].Item1]);
                    // Find the proper position for the intersection point when multiple intersection occurs on a single edge
                    while (location.Next != null && _intersections.Contains(location.Next.Value) &&
                           (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;
                    nodeA = listA.AddAfter(location, positions[i]);
                }
                else
                    nodeA = listA.Find(positions[i]);

                if (!subjectB.Contains(positions[i]))
                {
                    var location = listB.Find(subjectB[edges[i].Item2 - subjectA.Count]);
                    while (location.Next != null && _intersections.Contains(location.Next.Value) &&
                           (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;
                    nodeB = listB.AddAfter(location, positions[i]);
                }
                else
                    nodeB = listB.Find(positions[i]);

                _intersections.Add(new IntersectionElement {Position = positions[i], NodeA = nodeA, NodeB = nodeB});
            }
        }

        /// <summary>
        /// Adds the hole to the corresponding element in a shell set.
        /// </summary>
        /// <remarks>
        /// The method will look for the first shell in a linear search that contains the hole and execute the addition.
        /// </remarks>
        /// <param name="resultSet">The result set.</param>
        /// <param name="hole">The hole.</param>
        /// <returns><c>true</c> if the hole was added to a shell; otherwise <c>false</c>.</returns>
        private Boolean AddHoleToResult(IList<Clip> resultSet, IList<Coordinate> hole)
        {
            foreach (Clip clip in resultSet)
            {
                if (hole.All(coordinate => !PolygonAlgorithms.InExterior(clip.Shell, coordinate)))
                {
                    clip.Holes.Add(hole);
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
        /// <param name="shellA">The source coordinates representing the first subject polygon.</param>
        /// <param name="shellB">The source coordinates representing the second subject polygon.</param>
        /// <returns>The polygon coordinates of the common, internal clips.</returns>
        /// <exception cref="ArgumentNullException">One or both of the subject polygons are null.</exception>
        /// <exception cref="ArgumentException">One or both of the subject polygons do not have enough coordinates.</exception>
        public static IList<Clip> Intersection(IList<Coordinate> shellA, IList<Coordinate> shellB)
        {
            return Intersection(shellA, null, shellB, null);
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="shellA">The source coordinates representing the first subject polygon.</param>
        /// <param name="shellB">The source coordinates representing the second subject polygon.</param>
        /// <param name="holesA">The source coordinates representing the holes in the first subject polygon.</param>
        /// <param name="holesB">The source coordinates representing the holes in the second subject polygon.</param>
        /// <returns>The polygon coordinates of the common, internal clips.</returns>
        /// <exception cref="ArgumentNullException">One or both of the subject polygons are null.</exception>
        /// <exception cref="ArgumentException">One or both of the subject polygons do not have enough coordinates.</exception>
        public static IList<Clip> Intersection(IList<Coordinate> shellA, IList<IList<Coordinate>> holesA,
                                               IList<Coordinate> shellB, IList<IList<Coordinate>> holesB)
        {
            return new WeilerAthertonAlgorithm(shellA, holesA, shellB, holesB).Internal;
        }

        #endregion
    }
}
