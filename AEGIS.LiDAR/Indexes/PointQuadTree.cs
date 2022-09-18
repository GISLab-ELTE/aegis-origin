/// <copyright file="PointQuadTree.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roland Krisztandl</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.LiDAR.Indexes
{
    /// <summary>
    /// Represents a 2D Quad-tree, which contains a collection of <typeparamref name="T" /> instances.
    /// </summary>
    public partial class PointQuadTree<T> : IPointBasedSpatialIndex<T>
    {
        /// <summary>
        /// Represents an Object-Coordinate pair, which is stored in the index.
        /// </summary>
        public class TreeObject
        {
            /// <summary>
            /// Object content
            /// </summary>
            public T Obj;

            /// <summary>
            /// Object position
            /// </summary>
            public Coordinate Point;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="obj">Object content.</param>
            /// <param name="point">Object position.</param>
            public TreeObject(T obj, Coordinate point)
            {
                Obj = obj;
                Point = point;
            }
        }

        #region Fields, Properties
        /// <summary>
        /// The root of the tree.
        /// </summary>
        protected PointQuadTreeNode root;

        /// <summary>
        /// Gets a value indicating whether the index is read-only.
        /// </summary>
        /// <value><c>true</c> if the index is read-only; otherwise, <c>false</c>.</value>
        public Boolean IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the number of indexed points.
        /// </summary>
        /// <value>The number of indexed points.</value>
        public Int32 NumberOfPoints
        {
            get { return this.root.NumberOfPoints; }
        }

        /// <summary>
        /// Gets the number of nodes.
        /// </summary>
        /// <value>The number of nodes.</value>
        public Int32 NumberOfNodes
        {
            get { return this.root.NumberOfNodes + 1; }
        }

        /// <summary>
        /// Gets the Minimum node size for the index.
        /// </summary>
        public readonly Double MinNodeSize;

        /// <summary>
        /// Gets the Envelope of the whole index.
        /// </summary>
        public Envelope Envelope
        {
            get { return root.Envelope; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PointQuadTree" /> class.
        /// </summary>
        /// <param name="bound">The maximum indexed region.</param>
        /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
        public PointQuadTree(Envelope bound, Double MinNodeSize) : this(MinNodeSize)
        {
            this.root = new PointQuadTreeNode(bound, MinNodeSize, CalculateWordSize(bound), 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointQuadTree" /> class.
        /// </summary>
        /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
        protected PointQuadTree(Double MinNodeSize)
        {
            this.MinNodeSize = MinNodeSize;
        }
        #endregion

        /// <summary>
        /// Adds an object to the index.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        /// <param name="point">Coordinate of the object.</param>
        /// <exception cref="System.ArgumentNullException">The coordinate or object is null.</exception>
        public void Add(T obj, Coordinate point)
        {
            if (point == null)
                throw new ArgumentNullException("point", "Cannot add null point.");

            if (this.root.Envelope.Contains(point))
            {
                this.root.Add(obj, point);
            }
            else
            {
                this.CreateNew(obj, point);
            }
        }

        /// <summary>
        /// Removes the specified object from the index.
        /// </summary>
        /// <param name="obj">Object to remove</param>
        /// <param name="point">Coordinate of the object.</param>
        /// <returns><c>true</c> if the object is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The coordinate is null.</exception>
        public Boolean Remove(T obj, Coordinate point)
        {
            if (point == null)
                throw new ArgumentNullException("point", "point to remove must not be null.");

            return this.root.Remove(obj, point);
        }

        /// <summary>
        /// Removes an already existing TreeObject from the tree.
        /// </summary>
        /// <param name="obj">The TreeObject to remove.</param>
        /// <returns><c>true</c> if the object is indexed; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The obj is null.</exception>
        public Boolean Remove(TreeObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("treeobject", "TreeObject to remove must not be null.");

            return this.root.Remove(obj);
        }

        /// <summary>
        /// Removes all objects from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any objects are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Envelope to remove must not be null.");

            Boolean result = false;
            foreach (TreeObject obj in root.SearchWithCoords(envelope))
            {
                if (this.root.Remove(obj))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes all objects from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="points">The list of objects within the envelope.</param>
        /// <returns><c>true</c> if any objects are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope, out List<T> points)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Envelope to remove must not be null.");

            var objectsToRemove = root.SearchWithCoords(envelope);
            points = new List<T>(objectsToRemove.Count());

            foreach (TreeObject obj in objectsToRemove)
            {
                this.root.Remove(obj);
                points.Add(obj.Obj);
            }

            return points.Count != 0;
        }

        /// <summary>
        /// Searches the index for any objects contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of indexed objects located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public List<T> Search(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Search envelope must not be null.");

            return this.root.Search(envelope);
        }

        /// <summary>
        /// Searches the index for any objects contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The collection of indexed objects with their coordinates located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public List<TreeObject> SearchWithCoords(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "Search envelope must not be null.");

            return this.root.SearchWithCoords(envelope);
        }

        /// <summary>
        /// Clears all objects from the index.
        /// </summary>
        public virtual void Clear()
        {
            this.root = new PointQuadTreeNode(this.root.Envelope, MinNodeSize, this.root.WorldSize, 0);
        }

        /// <summary>
        /// Get all objects from the index.
        /// </summary>
        /// <returns>The collection of the indexed objects.</returns>
        public List<T> GetAll()
        {
            return this.root.GetAll();
        }

        /// <summary>
        /// Get all objects with their coordinates from the index.
        /// </summary>
        /// <returns>The collection of the indexed objects with their coordinates.</returns>
        public List<TreeObject> GetAllWithCoords()
        {
            return this.root.GetAllWithCoords();
        }

        /// <summary>
        /// Creates a new tree based on an unindexed object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="point">The coordinate of the object.</param>
        protected virtual void CreateNew(T obj, Coordinate point)
        {
            List<TreeObject> allPoints = this.GetAllWithCoords();
            Envelope envelope = Envelope.FromEnvelopes(this.root.Envelope, Envelope.FromCoordinates(point));
            this.root = new PointQuadTreeNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
            this.Add(obj, point);
            foreach(TreeObject treeObject in allPoints)
            {
                this.Add(treeObject.Obj, treeObject.Point);
            }
        }

        /// <summary>
        /// Calculates a new WorldSize based on the <paramref name="envelope"/>.
        /// </summary>
        /// <returns>The new WorldSize</returns>
        protected Double CalculateWordSize(Envelope envelope)
        {
            return Math.Max(envelope.MaxX - envelope.MinX, Math.Max(envelope.MaxY - envelope.MinY, envelope.MaxZ - envelope.MinZ));
        }

        /// <summary>
        /// Rebuilds the tree. Useful if you removed a lot of objects from the index.
        /// </summary>
        public virtual void Rebuild()
        {
            List<TreeObject> allPoints = this.GetAllWithCoords();
            Envelope envelope = Envelope.FromCoordinates(allPoints.Select(x => x.Point));
            this.root = new PointQuadTreeNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
            foreach (TreeObject treeObject in allPoints)
            {
                this.Add(treeObject.Obj, treeObject.Point);
            }
        }
    }
}
