/// <copyright file="GreinerHormannAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents the Greiner-Hormann clipping algorithm for determining internal and external polygon segments of two subject polygons.
    /// The algorithm is capable of clipping arbitrary polygons, including concave polygons and holes, however the self-intersection of polygons are not yet supported.
    /// </summary>
    /// <remarks>
    /// The Greiner-Hormann algorithm is based on the Weiler-Atherton algorithm, but with a different implementational approach.
    /// 
    /// The algorithm was extended to handle degenerate cases without vertex perturbation based on the proposal of Dae Hyun Kim and Myoung-Jun Kim [1].
    /// Degenerate cases consists of polygons with joint edges and polygons touching but not intersecting each other.
    /// 
    /// The case of self-intersecting polygons might be implemented based on the ideas of Anurag Chakraborty [2].
    /// 
    /// References:
    /// [1] D. H. Kim, M-J. Kim, An Extension of Polygon Clipping To Resolve Degenerate Cases, 2006
    /// [2] A. Chakraborty, An Extension Of Weiler-Atherton Algorithm To Cope With The Self-intersecting Polygon, 2014
    /// </remarks>
    public class GreinerHormannAlgorithm
    {
        #region Private types

        /// <summary>
        /// Defines the kinds of the intersection points.
        /// </summary>
        private enum IntersectionMode
        {
            /// <summary>
            /// Indicates that the position is not a real intersection point.
            /// </summary>
            None,

            /// <summary>
            /// Indicates that the intersection is an entry point.
            /// </summary>
            Entry,

            /// <summary>
            /// Indicates that the intersection is an exit point.
            /// </summary>
            Exit,

            /// <summary>
            /// Indicates that the intersection is a boundary point.
            /// </summary>
            /// <remarks>
            /// Boundary intersections are entry-exit or exit-entry intersections at a single position.
            /// </remarks>
            Boundary,
        }

        /// <summary>
        /// Represents the descriptor of an intersection point.
        /// </summary>
        private class Intersection
        {
            #region Public properties

            /// <summary>
            /// Gets or sets the position of the intersection.
            /// </summary>
            /// <value>The position.</value>
            public Coordinate Position { get; set; }

            /// <summary>
            /// Gets or sets the link to corresponding node for the first subject polygon.
            /// </summary>
            public LinkedListNode<Coordinate> NodeA { get; set; }

            /// <summary>
            /// Gets or sets the link to corresponding node for the second subject polygon.
            /// </summary>
            public LinkedListNode<Coordinate> NodeB { get; set; }

            /// <summary>
            /// Gets or sets the kind of the intersection.
            /// </summary>
            public IntersectionMode Mode { get; set; }

            /// <summary>
            /// Gets or sets the next intersection in the first subject polygon.
            /// </summary>
            public Intersection NextA { get; set; }

            /// <summary>
            /// Gets or sets the next intersection in the second subject polygon.
            /// </summary>
            public Intersection NextB { get; set; }

            #endregion
        }

        /// <summary>
        /// Represents a collection of intersection elements indexed by their positions.
        /// </summary>
        private class IntersectionCollection : KeyedCollection<Coordinate, Intersection>
        {
            #region KeyedCollection methods

            /// <summary>
            /// Extracts the key from the specified element.
            /// </summary>
            /// <returns>
            /// The key for the specified element.
            /// </returns>
            /// <param name="item">The element from which to extract the key.</param>
            protected override Coordinate GetKeyForItem(Intersection item)
            {
                return item.Position;
            }

            #endregion
        }

        /// <summary>
        /// Represents a polygon clip.
        /// </summary>
        private class PolygonClip
        {
            #region Public properties

            /// <summary>
            /// The shell of the clip.
            /// </summary>
            public IList<Coordinate> Shell { get; private set; }

            /// <summary>
            /// The holes of the clip.
            /// </summary>
            public List<IList<Coordinate>> Holes { get; private set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="PolygonClip" /> class.
            /// </summary>
            /// <param name="shell">The shell.</param>
            /// <param name="holes">The holes.</param>
            public PolygonClip(IList<Coordinate> shell = null, IEnumerable<IList<Coordinate>> holes = null)
            {
                Shell = shell;
                Holes = holes == null ? new List<IList<Coordinate>>() : new List<IList<Coordinate>>(holes);
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Adds a new hole to the polygon clip.
            /// </summary>
            /// <param name="hole">The hole.</param>
            /// <remarks>
            /// The method will reverse the orientation of the hole when necessary.
            /// </remarks>
            public void AddHole(IList<Coordinate> hole)
            {
                if (PolygonAlgorithms.Orientation(hole) == Orientation.CounterClockwise)
                {
                    hole = new List<Coordinate>(hole.Reverse());
                }
                Holes.Add(hole);
            }

            /// <summary>
            /// Adds a new hole to the polygon clip.
            /// </summary>
            /// <param name="hole">The hole.</param>
            /// <remarks>
            /// The method will reverse the orientation of the hole when necessary.
            /// </remarks>
            public void AddHole(IBasicLineString hole)
            {
                if (PolygonAlgorithms.Orientation(hole.Coordinates) == Orientation.CounterClockwise)
                {
                    hole = new BasicLineString(hole.Reverse());
                }
                Holes.Add(hole.Coordinates);
            }

            /// <summary>
            /// Returns the <see cref="PolygonClip"/> as an <see cref="IBasicPolygon"/>.
            /// </summary>
            /// <returns>The converted result polygon.</returns>
            public IBasicPolygon ToPolygon()
            {
                return new BasicPolygon(Shell, Holes);
            }

            #endregion
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
        /// The shell of the first polygon as a linked list.
        /// </summary>
        private LinkedList<Coordinate> _listA;

        /// <summary>
        /// The shell of the second polygon as a linked list.
        /// </summary>
        private LinkedList<Coordinate> _listB;

        /// <summary>
        /// The array containing the holes of the first polygon.
        /// </summary>
        /// <remarks>
        /// The holes are reversed into counterclockwise orientation in this list.
        /// </remarks>
        private List<IBasicLineString> _holesA;

        /// <summary>
        /// The array containing the holes of the second polygon.
        /// </summary>
        /// <remarks>
        /// The holes are reversed into counterclockwise orientation in this list.
        /// </remarks>
        private List<IBasicLineString> _holesB;

        /// <summary>
        /// The intersection collection.
        /// </summary>
        private IntersectionCollection _intersections;

        /// <summary>
        /// The list of internal clips.
        /// </summary>
        private List<PolygonClip> _internalPolygons;

        /// <summary>
        /// The list of external clips for the first polygon.
        /// </summary>
        private List<PolygonClip> _externalPolygonsA;

        /// <summary>
        /// The list of external clips for the second polygon.
        /// </summary>
        private List<PolygonClip> _externalPolygonsB;

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
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

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
                        _externalPolygonsA = null;
                        _externalPolygonsB = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the internal clips.
        /// </summary>
        /// <value>The list of polygons representing the internal clips of the two subject polygons.</value>
        public IList<IBasicPolygon> InternalPolygons
        {
            get
            {
                if (!_hasResult) Compute();
                return _internalPolygons.Select(clip => clip.ToPolygon()).ToList();
            }
        }

        /// <summary>
        /// Gets the external clips for the first polygon.
        /// </summary>
        /// <value>The list of polygons representing the external clips of the first subject polygon.</value>
        public IList<IBasicPolygon> ExternalFirstPolygons
        {
            get
            {
                if (!_hasResult) Compute();
                return _externalPolygonsA.Select(clip => clip.ToPolygon()).ToList();
            }
        }

        /// <summary>
        /// Gets the external clips for the second polygon.
        /// </summary>
        /// <value>The list of polygons representing the external clips of the second subject polygon.</value>
        public IList<IBasicPolygon> ExternalSecondPolygons
        {
            get
            {
                if (!_hasResult) Compute();
                return _externalPolygonsB.Select(clip => clip.ToPolygon()).ToList();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GreinerHormannAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public GreinerHormannAlgorithm(IBasicPolygon first, IBasicPolygon second, Boolean computeExternalClips = true, PrecisionModel precisionModel = null)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The first polygon is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The second polygon is null.");

            if (!PolygonAlgorithms.IsSimple(first.Shell.Coordinates) || !PolygonAlgorithms.IsSimple(second.Shell.Coordinates))
                throw new NotSupportedException("Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.");

            _polygonA = first;
            _polygonB = second;
            _computeExternalClips = computeExternalClips;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreinerHormannAlgorithm" /> class.
        /// </summary>
        /// <param name="first">The shell of the first polygon.</param>
        /// <param name="second">The shell of the second polygon.</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell of the first polygon is null.
        /// or
        /// The shell of the second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public GreinerHormannAlgorithm(IList<Coordinate> first, IList<Coordinate> second, Boolean computeExternalClips = true, PrecisionModel precisionModel = null)
        {
            if (first == null)
                throw new ArgumentNullException("first", "The shell of the first polygon is null.");
            if (second == null)
                throw new ArgumentNullException("second", "The shell of the second polygon is null.");

            if (!PolygonAlgorithms.IsSimple(first) || !PolygonAlgorithms.IsSimple(second))
                throw new NotSupportedException("Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.");
            
            _polygonA = new BasicPolygon(first);
            _polygonB = new BasicPolygon(second);
            _computeExternalClips = computeExternalClips;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _hasResult = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreinerHormannAlgorithm" /> class.
        /// </summary>
        /// <param name="firstShell">The shell of the first polygon (in counter-clockwise order).</param>
        /// <param name="firstHoles">The holes in the first polygon (in clockwise order).</param>
        /// <param name="secondShell">The shell of the second polygon (in counter-clockwise order).</param>
        /// <param name="secondHoles">The holes in the second polygon (in clockwise order).</param>
        /// <param name="computeExternalClips">A value indicating whether to compute the external clips.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public GreinerHormannAlgorithm(IList<Coordinate> firstShell, IEnumerable<IList<Coordinate>> firstHoles,
                                       IList<Coordinate> secondShell, IEnumerable<IList<Coordinate>> secondHoles,
                                       Boolean computeExternalClips = true, PrecisionModel precisionModel = null)
        {
            if (firstShell == null)
                throw new ArgumentNullException("firstShell", "The shell of the first polygon is null.");
            if (secondShell == null)
                throw new ArgumentNullException("secondShell", "The shell of the second polygon is null.");

            if (!PolygonAlgorithms.IsSimple(firstShell) || !PolygonAlgorithms.IsSimple(secondShell) ||
                firstHoles != null && firstHoles.Any(h => !PolygonAlgorithms.IsSimple(h.Reverse().ToList())) || 
                secondHoles != null && secondHoles.Any(h => !PolygonAlgorithms.IsSimple(h.Reverse().ToList())))
                throw new NotSupportedException("Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.");

            _polygonA = new BasicPolygon(firstShell, firstHoles);
            _polygonB = new BasicPolygon(secondShell, secondHoles);
            _computeExternalClips = computeExternalClips;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
            _hasResult = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the result of the initialized clipping algorithm.
        /// </summary>
        public void Compute()
        {
            Initialize();               // Initialize the algorithm.
            FindIntersections();        // Determine the intersections of the polygon shells.
            CategorizeIntersections();  // Categorize the type of the intersections.
            LinkIntersections();        // Link the intersections to the succeeding ones.
            AddHoleIntersections();     // Add the vertices of hole intersections to the polygon shells.

            // Intersection point exist.
            if (_intersections.Any(intersection => intersection.Mode == IntersectionMode.Entry))
            {
                ComputeInternalClips();

                if (_computeExternalClips)
                {
                    ComputeExternalClipsA();
                    ComputeExternalClipsB();
                }

                _intersections.Clear();
            }
            else // No intersection point found.
            {
                ComputeCompleteClips();
            }
            
            // Holes exist.
            if (_holesA.Count > 0 || _holesB.Count > 0)
            {
                // Compute the intersection of the holes in the subject polygons.
                // Holes of the internal clips are added to the polygons through the process.
                ComputeHoles();

                // Resolves hole degeneracies in the external clips.
                // Adds external holes to the appropriate polygons.
                if (_computeExternalClips)
                {
                    ComputeExternalHoles(_externalPolygonsA, _holesA);
                    ComputeExternalHoles(_externalPolygonsB, _holesB);
                }
            }

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
            _internalPolygons = new List<PolygonClip>();
            _externalPolygonsA = new List<PolygonClip>();
            _externalPolygonsB = new List<PolygonClip>();

            // Initialize lists.
            _listA = new LinkedList<Coordinate>(_polygonA.Shell);
            _listA.RemoveLast();
            _listB = new LinkedList<Coordinate>(_polygonB.Shell);
            _listB.RemoveLast();

            // Initialize holes.
            _holesA = new List<IBasicLineString>(_polygonA.Holes.Select(hole => new BasicLineString(hole.Reverse())));
            _holesB = new List<IBasicLineString>(_polygonB.Holes.Select(hole => new BasicLineString(hole.Reverse())));
        }

        /// <summary>
        /// Finds the intersections of the polygon shells.
        /// </summary>
        private void FindIntersections()
        {
            if (Envelope.FromCoordinates(_polygonA.Shell).Disjoint(Envelope.FromCoordinates(_polygonB.Shell)))
                return;

            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(new List<IList<Coordinate>> { _polygonA.Shell.Coordinates, _polygonB.Shell.Coordinates }, PrecisionModel);
            IList<Coordinate> positions = algorithm.Intersections;
            IList<Tuple<Int32, Int32>> edges = algorithm.EdgeIndices;

            for (Int32 i = 0; i < positions.Count; ++i)
            {
                if (_intersections.Contains(positions[i]))
                    continue;

                // Process the first polygon
                LinkedListNode<Coordinate> nodeA, nodeB;
                if (!_polygonA.Shell.Contains(positions[i]))
                {
                    // Insert intersection point into vertex lists
                    LinkedListNode<Coordinate> location = _listA.Find(_polygonA.Shell.Coordinates[edges[i].Item1]);

                    // Find the proper position for the intersection point when multiple intersection occurs on a single edge
                    while (location.Next != null && _intersections.Contains(location.Next.Value) &&
                           (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;

                    nodeA = _listA.AddAfter(location, positions[i]);
                }
                else
                    nodeA = _listA.Find(positions[i]);

                // Process the second polygon
                if (!_polygonB.Shell.Coordinates.Contains(positions[i]))
                {
                    LinkedListNode<Coordinate> location = _listB.Find(_polygonB.Shell.Coordinates[edges[i].Item2 - _polygonA.Shell.Count]);

                    while (location.Next != null && _intersections.Contains(location.Next.Value) &&
                           (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                        location = location.Next;

                    nodeB = _listB.AddAfter(location, positions[i]);
                }
                else
                    nodeB = _listB.Find(positions[i]);

                _intersections.Add(new Intersection { Position = positions[i], NodeA = nodeA, NodeB = nodeB });
            }
        }

        /// <summary>
        /// Finds the intersections of the subject polygons.
        /// </summary>
        private void CategorizeIntersections()
        {
            // If intersection points exist.
            if (_intersections.Count == 0)
                return;

            // Entering / Exiting intersection point separation (from A to B).
            LinkedListNode<Coordinate> currentNode = _listA.First;
            while (currentNode != null)
            {
                if (_intersections.Contains(currentNode.Value))
                {
                    LinkedListNode<Coordinate> prevNode = currentNode.Previous ?? _listA.Last;
                    LinkedListNode<Coordinate> nextNode = currentNode.Next ?? _listA.First;

                    Coordinate prevEdge = LineAlgorithms.Centroid(currentNode.Value, prevNode.Value, PrecisionModel);
                    Coordinate nextEdge = LineAlgorithms.Centroid(currentNode.Value, nextNode.Value, PrecisionModel);

                    RelativeLocation prevLocation = PolygonAlgorithms.Location(_polygonB.Shell.Coordinates, prevEdge, PrecisionModel);
                    RelativeLocation nextLocation = PolygonAlgorithms.Location(_polygonB.Shell.Coordinates, nextEdge, PrecisionModel);

                    // Entry
                    if (prevLocation == RelativeLocation.Exterior && nextLocation == RelativeLocation.Interior ||
                        prevLocation == RelativeLocation.Exterior && nextLocation == RelativeLocation.Boundary ||
                        prevLocation == RelativeLocation.Boundary && nextLocation == RelativeLocation.Interior)
                        _intersections[currentNode.Value].Mode = IntersectionMode.Entry;

                    // Exit
                    else if (prevLocation == RelativeLocation.Interior && nextLocation == RelativeLocation.Exterior ||
                             prevLocation == RelativeLocation.Boundary && nextLocation == RelativeLocation.Exterior ||
                             prevLocation == RelativeLocation.Interior && nextLocation == RelativeLocation.Boundary)
                        _intersections[currentNode.Value].Mode = IntersectionMode.Exit;
                    
                    // Entry + Exit / Exit + Entry
                    else if (prevLocation == RelativeLocation.Interior && nextLocation == RelativeLocation.Interior ||
                             prevLocation == RelativeLocation.Exterior && nextLocation == RelativeLocation.Exterior)
                        _intersections[currentNode.Value].Mode = IntersectionMode.Boundary;

                    // Boundary -> Boundary
                    else
                    {
                        //_intersections[currentNode.Value].Mode = IntersectionMode.None;
                        _intersections.Remove(currentNode.Value);
                    }
                }
                currentNode = currentNode.Next;
            }
        }

        /// <summary>
        /// Links the intersections to each other in a succeeding order.
        /// </summary>
        private void LinkIntersections()
        {
            if (_intersections.Count == 0)
                return;
            
            // Process the first polygon.
            Intersection firstIntersection = null;
            Intersection lastIntersection = null;
            LinkedListNode<Coordinate> currentNode = _listA.First;

            while (currentNode != null)
            {
                if (_intersections.Contains(currentNode.Value))
                {
                    if (lastIntersection == null)
                        firstIntersection = _intersections[currentNode.Value];
                    else
                        lastIntersection.NextA = _intersections[currentNode.Value];
                    lastIntersection = _intersections[currentNode.Value];
                }
                currentNode = currentNode.Next;
            }
            lastIntersection.NextA = firstIntersection;

            // Process the second polygon.
            firstIntersection = null;
            lastIntersection = null;
            currentNode = _listB.First;

            while (currentNode != null)
            {
                if (_intersections.Contains(currentNode.Value))
                {
                    if (lastIntersection == null)
                        firstIntersection = _intersections[currentNode.Value];
                    else
                        lastIntersection.NextB = _intersections[currentNode.Value];
                    lastIntersection = _intersections[currentNode.Value];
                }
                currentNode = currentNode.Next;
            }
            lastIntersection.NextB = firstIntersection;
        }

        /// <summary>
        /// Adds the intersection points of the holes and polygon shells to the linked lists containing the polygons' vertices.
        /// </summary>
        private void AddHoleIntersections()
        {
            foreach (IBasicLineString holeB in _holesB)
                AddHoleIntersections(_listA, holeB.Coordinates);
            foreach (IBasicLineString holeA in _holesA)
                AddHoleIntersections(_listB, holeA.Coordinates);
        }

        /// <summary>
        /// Adds intersection points of a polygon shell and a hole to the linked list containing the polygon's vertices.
        /// </summary>
        /// <param name="polygon">The linked list of the polygon shell.</param>
        /// <param name="hole">The hole.</param>
        private void AddHoleIntersections(LinkedList<Coordinate> polygon, IList<Coordinate> hole)
        {
            if (Envelope.FromCoordinates(polygon).Disjoint(Envelope.FromCoordinates(hole)))
                return;

            IList<Coordinate> shell = new List<Coordinate>(polygon);
            if (shell.Count > 0) shell.Add(polygon.First.Value);

            BentleyOttmannAlgorithm algorithm = new BentleyOttmannAlgorithm(new List<IList<Coordinate>> { shell, hole }, PrecisionModel);
            IList<Coordinate> positions = algorithm.Intersections;
            IList<Tuple<Int32, Int32>> edges = algorithm.EdgeIndices;

            for (Int32 i = 0; i < positions.Count; ++i)
            {
                if (polygon.Contains(positions[i]))
                    continue;

                // Insert intersection point into vertex lists
                LinkedListNode<Coordinate> location = polygon.Find(shell[edges[i].Item1]);

                // Find the proper position for the intersection point when multiple intersection occurs on a single edge
                while (location.Next != null && (positions[i] - location.Value).Length > (location.Next.Value - location.Value).Length)
                    location = location.Next;

                polygon.AddAfter(location, positions[i]);
            }
        }

        /// <summary>
        /// Compute the internal clips.
        /// </summary>
        private void ComputeInternalClips()
        {
            // Start form the entry points, last ones in a queue and follow the first subject polygon.
            List<Coordinate> startPositions =
                _intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry && intersection.Mode != intersection.NextA.Mode)
                              .Select(intersection => intersection.Position).ToList();

            while (startPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = _intersections[startPositions[0]].NodeA;
                Boolean isFollowingA = true;

                shell.Add(current.Value);
                startPositions.Remove(current.Value);

                do
                {
                    current = current.Next ?? current.List.First;
                    shell.Add(current.Value);

                    // Trace and remove inner loops.
                    Int32 match = shell.FindLastIndex(shell.Count - 2, shell.Count -2, coordinate => coordinate == current.Value);
                    if (match >= 0)
                    {
                        List<Coordinate> subResult = shell.GetRange(match, shell.Count - match);
                        if (subResult.Count > 3)
                            _internalPolygons.Add(new PolygonClip(subResult));
                        shell.RemoveRange(match, shell.Count - match - 1);
                    }

                    if (_intersections.Contains(current.Value))
                    {
                        if (_intersections[current.Value].Mode == IntersectionMode.Entry)
                            isFollowingA = true;
                        else if (_intersections[current.Value].Mode == IntersectionMode.Exit)
                            isFollowingA = false;

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        startPositions.Remove(current.Value);
                    }
                } while (current.Value != shell[0]);
                if (shell.Count > 3)
                    _internalPolygons.Add(new PolygonClip(shell));
            }
        }

        /// <summary>
        /// Compute the external clips for the first polygon.
        /// </summary>
        private void ComputeExternalClipsA()
        {
            // Start form the exit points, last ones in a queue and follow the first subject polygon.
            List<Coordinate> startPositions =
                _intersections.Where(intersection => intersection.Mode == IntersectionMode.Exit && intersection.Mode != intersection.NextA.Mode)
                              .Select(intersection => intersection.Position).ToList();

            while (startPositions.Count > 0)
            {
                List<Coordinate> shell = new List<Coordinate>();
                LinkedListNode<Coordinate> current = _intersections[startPositions[0]].NodeA;
                Boolean isFollowingA = true;

                shell.Add(current.Value);
                startPositions.Remove(current.Value);

                do
                {
                    if (isFollowingA)
                        current = current.Next ?? current.List.First;
                    else
                        current = current.Previous ?? current.List.Last;
                    shell.Add(current.Value);

                    // Trace and remove inner loops.
                    Int32 match = shell.FindLastIndex(shell.Count - 2, shell.Count - 2, coordinate => coordinate == current.Value);
                    if (match >= 0)
                    {
                        List<Coordinate> subResult = shell.GetRange(match, shell.Count - match);
                        if (subResult.Count > 3)
                            _internalPolygons.Add(new PolygonClip(subResult));
                        shell.RemoveRange(match, shell.Count - match - 1);
                    }

                    if (_intersections.Contains(current.Value))
                    {
                        if (_intersections[current.Value].Mode == IntersectionMode.Entry)
                            isFollowingA = false;
                        else if (_intersections[current.Value].Mode == IntersectionMode.Exit)
                            isFollowingA = true;

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        startPositions.Remove(current.Value);
                    }
                } while (current.Value != shell[0]);
                if (shell.Count > 3)
                    _externalPolygonsA.Add(new PolygonClip(shell));
            }
        }

        /// <summary>
        /// Compute the external clips for the second polygon.
        /// </summary>
        private void ComputeExternalClipsB()
        {
            // Start form the entry points, last ones in a queue and follow the second subject polygon.
            List<Coordinate> checkPositions =
                _intersections.Where(intersection => intersection.Mode == IntersectionMode.Entry && intersection.Mode != intersection.NextB.Mode)
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

                    // Trace and remove inner loops.
                    Int32 match = shell.FindLastIndex(shell.Count - 2, shell.Count - 2, coordinate => coordinate == current.Value);
                    if (match >= 0)
                    {
                        List<Coordinate> subResult = shell.GetRange(match, shell.Count - match);
                        if (subResult.Count > 3)
                            _internalPolygons.Add(new PolygonClip(subResult));
                        shell.RemoveRange(match, shell.Count - match - 1);
                    }

                    if (_intersections.Contains(current.Value))
                    {
                        if (_intersections[current.Value].Mode == IntersectionMode.Entry)
                            isFollowingA = false;
                        else if (_intersections[current.Value].Mode == IntersectionMode.Exit)
                            isFollowingA = true;

                        current = isFollowingA
                                      ? _intersections[current.Value].NodeA
                                      : _intersections[current.Value].NodeB;
                        checkPositions.Remove(current.Value);
                    }
                } while (current.Value != shell[0]);
                if (shell.Count > 3)
                    _externalPolygonsB.Add(new PolygonClip(shell));
            }
        }

        /// <summary>
        /// Completes complete clips (in case of no intersection).
        /// </summary>
        private void ComputeCompleteClips()
        {
            Boolean isAinB = _polygonA.Shell.All(position => !PolygonAlgorithms.InExterior(_polygonB.Shell.Coordinates, position, PrecisionModel));
            Boolean isBinA = _polygonB.Shell.All(position => !PolygonAlgorithms.InExterior(_polygonA.Shell.Coordinates, position, PrecisionModel));

            List<Coordinate> finalShellA = _listA.ToList();
            finalShellA.Add(finalShellA[0]);

            List<Coordinate> finalShellB = _listB.ToList();
            finalShellB.Add(finalShellB[0]);

            PolygonClip finalPolygonA = new PolygonClip(finalShellA);
            PolygonClip finalPolygonB = new PolygonClip(finalShellB);

            if (isAinB && isBinA)  // A equals B
            {
                _internalPolygons.Add(finalPolygonA);
            }
            else if (isAinB)       // B contains A
            {
                finalPolygonB.AddHole(finalPolygonA.Shell);
                _internalPolygons.Add(finalPolygonA);
                _externalPolygonsB.Add(finalPolygonB);
            }
            else if (isBinA)       // A contains B
            {
                finalPolygonA.AddHole(finalPolygonB.Shell);
                _internalPolygons.Add(finalPolygonB);
                _externalPolygonsA.Add(finalPolygonA);
            }
            else                   // A and B are distinct
            {
                _externalPolygonsA.Add(finalPolygonA);
                _externalPolygonsB.Add(finalPolygonB);
            }
        }

        /// <summary>
        /// Compute the intersection of the holes in the subject polygons.
        /// </summary>
        /// <remarks>Holes of the internal clips are added to the polygons through the process.</remarks>
        private void ComputeHoles()
        {
            if (Envelope.FromCoordinates(_polygonA.Shell).Disjoint(Envelope.FromCoordinates(_polygonB.Shell)))
                return;

            List<PolygonClip> externalB = new List<PolygonClip>();

            // Intersect holes in the first polygon with the internal shells
            if (_holesA.Count > 0)
            {
                List<PolygonClip> processedPolygons = new List<PolygonClip>();
                while (_internalPolygons.Count > 0)
                {
                    Boolean intersected = false;
                    for (Int32 i = 0; i < _holesA.Count; ++i)
                    {
                        // Internal parts cannot have holes inside them at this point
                        var algorithm =
                            new GreinerHormannAlgorithm(_holesA[i].Coordinates, _internalPolygons[0].Shell,
                                precisionModel: PrecisionModel);
                        if (algorithm.InternalPolygons.Count > 0)
                        {
                            intersected = true;

                            externalB.AddRange(algorithm._internalPolygons);
                            _holesA.AddRange(algorithm._externalPolygonsA.Select(polygon => new BasicLineString(polygon.Shell)));
                            _internalPolygons.AddRange(algorithm._externalPolygonsB);

                            _holesA.RemoveAt(i);
                            break;
                        }
                    }
                    if (!intersected)
                        processedPolygons.Add(_internalPolygons[0]);
                    _internalPolygons.RemoveAt(0);
                }
                _internalPolygons = processedPolygons;
            }

            // Intersect holes in the second polygon with the internal polygon
            if (_holesB.Count > 0)
            {
                List<PolygonClip> processedPolygons = new List<PolygonClip>();
                while (_internalPolygons.Count > 0)
                {
                    Boolean intersected = false;
                    for (Int32 i = 0; i < _holesB.Count; ++i)
                    {
                        var algorithm =
                            new GreinerHormannAlgorithm(_internalPolygons[0].Shell, _internalPolygons[0].Holes, _holesB[i].Coordinates, null,
                                precisionModel: PrecisionModel);
                        if (algorithm.InternalPolygons.Count > 0)
                        {
                            intersected = true;

                            _externalPolygonsA.AddRange(algorithm._internalPolygons);
                            _internalPolygons.AddRange(algorithm._externalPolygonsA);
                            _holesB.AddRange(algorithm._externalPolygonsB.Select(polygon => new BasicLineString(polygon.Shell)));

                            _holesB.RemoveAt(i);
                            break;
                        }
                    }
                    if (!intersected)
                        processedPolygons.Add(_internalPolygons[0]);
                    _internalPolygons.RemoveAt(0);
                }
                _internalPolygons = processedPolygons;
            }

            while (externalB.Count > 0)
            {
                Boolean intersected = false;
                for (Int32 i = 0; i < _holesB.Count; ++i)
                {
                    var algorithm =
                        new GreinerHormannAlgorithm(externalB[0].ToPolygon(), new BasicPolygon(_holesB[i].Coordinates),
                            precisionModel: PrecisionModel);
                    if (algorithm.InternalPolygons.Count > 0)
                    {
                        intersected = true;

                        // Clips in algorithm._internalPolygons are intersection of the already existing holes in the internals, therefore they are already covered.
                        _externalPolygonsB.AddRange(algorithm._externalPolygonsA);
                        _holesB.AddRange(algorithm._externalPolygonsB.Select(polygon => new BasicLineString(polygon.Shell)));

                        _holesB.RemoveAt(i);
                        break;
                    }
                }
                if (!intersected)
                    _externalPolygonsB.Add(externalB[0]);
                externalB.RemoveAt(0);
            }
        }

        /// <summary>
        /// Resolves hole degeneracies in the external clips.
        /// Adds external holes to the appropriate polygons.
        /// </summary>
        /// <remarks>Holes touching boundary of their shells are degenerate.</remarks>
        /// <param name="polygons">Polygons (without holes) to process.</param>
        /// <param name="holes">Possibly degenerate holes to process and locate.</param>
        private void ComputeExternalHoles(List<PolygonClip> polygons, List<IBasicLineString> holes)
        {
            List<IBasicLineString> processedHoles = new List<IBasicLineString>();
            while (holes.Count > 0)
            {
                Boolean intersected = false;
                for (Int32 i = 0; i < polygons.Count; ++i)
                {
                    if (ShamosHoeyAlgorithm.Intersects(new[] { polygons[i].Shell, holes[0].Coordinates }, PrecisionModel))
                    {
                        var clipping = new GreinerHormannAlgorithm(polygons[i].Shell, holes[0].Coordinates,
                            precisionModel: PrecisionModel);
                        clipping.Compute();

                        polygons.AddRange(clipping._externalPolygonsA);
                        polygons.RemoveAt(i);

                        intersected = true;
                        break;
                    }
                }

                if (!intersected)
                    processedHoles.Add(holes[0]);
                holes.RemoveAt(0);
            }
            holes.AddRange(processedHoles);

            foreach (IBasicLineString hole in holes)
            {
                PolygonClip containerClip =
                    polygons.First(clip => hole.All(coordinate => !PolygonAlgorithms.InExterior(clip.Shell, coordinate, PrecisionModel)));
                containerClip.AddHole(hole);
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="first">The first polygon.</param>
        /// <param name="second">The second polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public static IList<IBasicPolygon> Clip(IBasicPolygon first, IBasicPolygon second, PrecisionModel precisionModel = null)
        {
            return new GreinerHormannAlgorithm(first, second, false, precisionModel).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="first">The shell of the first polygon.</param>
        /// <param name="second">The shell of the second polygon.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The shell of the first polygon is null.
        /// or
        /// The shell of the second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public static IList<IBasicPolygon> Clip(IList<Coordinate> first, IList<Coordinate> second, PrecisionModel precisionModel = null)
        {
            return new GreinerHormannAlgorithm(first, second, false, precisionModel).InternalPolygons;
        }

        /// <summary>
        /// Computes the common parts of two subject polygons by clipping them.
        /// </summary>
        /// <param name="firstShell">The shell of the first polygon (in counter-clockwise order).</param>
        /// <param name="firstHoles">The holes in the first polygon (in clockwise order).</param>
        /// <param name="secondShell">The shell of the second polygon (in counter-clockwise order).</param>
        /// <param name="secondHoles">The holes in the second polygon (in clockwise order).</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The first polygon is null.
        /// or
        /// The second polygon is null.
        /// </exception>
        /// <exception cref="System.NotSupportedException">Self-intersecting polygons are not (yet) supported by the Greiner-Hormann algorithm.</exception>
        public static IList<IBasicPolygon> Clip(IList<Coordinate> firstShell, IEnumerable<IList<Coordinate>> firstHoles,
                                       IList<Coordinate> secondShell, IEnumerable<IList<Coordinate>> secondHoles,
                                       PrecisionModel precisionModel = null)
        {
            return new GreinerHormannAlgorithm(firstShell, firstHoles, secondShell, secondHoles, false, precisionModel).InternalPolygons;
        }

        #endregion
    }
}
