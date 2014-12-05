/// <copyright file="RTree.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Nagy</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Indices.Spatial.RTree
{
    /// <summary>
    /// Represents a 3D R-Tree, which contains a collection of <see cref="IGeometry" /> instances.
    /// </summary>
    public class RTree : ISpatialIndex
    {
        #region Protected types

        /// <summary>
        /// Represents a node of the R-tree.
        /// </summary>
        protected class RTreeNode
        {
            #region Private fields

            private Envelope _envelope;
            private RTreeNode _parent;

            #endregion

            #region Public properties

            /// <summary>
            /// Gets or sets the parent node.
            /// </summary>
            /// <value>The parent node.</value>
            public RTreeNode Parent
            {
                get { return _parent; }
                set
                {
                    _parent = value;
                    if (MaxChildren == 0 && _parent != null)
                        MaxChildren = _parent.MaxChildren;
                }
            }
            /// <summary>
            /// Gets or sets the child nodes.
            /// </summary>
            /// <value>The list of child nodes.</value>
            public List<RTreeNode> Children { get; private set; }

            /// <summary>
            /// Gets the geometry contained in the node.
            /// </summary>
            /// <value>The geometry contained in the node if the node is a leaf node; otherwise, <c>null</c>.</value>
            public IGeometry Geometry { get; private set; }

            /// <summary>
            /// The minimum bounding envelope of the node.
            /// </summary>
            /// <value>The minimum bounding envelope of all descendant geometries.</value>
            public Envelope Envelope
            {
                get { return (Geometry != null ? Geometry.Envelope : _envelope); }
                private set { _envelope = value; }
            }

            /// <summary>
            /// Gets the maximum number of the children that can be contained by the node.
            /// </summary>
            /// <value>The maximum number of the children that can be contained by the node.</value>
            public Int32 MaxChildren { get; private set; }

            /// <summary>
            /// Gets the number of children contained by the node.
            /// </summary>
            /// <value>The number of the children contained by the node.</value>
            public Int32 ChildrenCount { get { return (Children != null) ? Children.Count : 0; } }

            /// <summary>
            /// Gets a value indicating whether the node contains only leaf nodes.
            /// </summary>
            /// <value><c>true</c> if the node is a leaf, or has leaf children; otherwise, <c>false</c>.</value>
            public Boolean IsLeafContainer { get { return (!IsLeaf && (Children == null || Children[0].IsLeaf)); } }

            /// <summary>
            /// Gets a value indicating whether the node is a leaf node.
            /// </summary>
            /// <value><c>true</c> if the node contains a  otherwise, <c>false</c>.</value>
            public Boolean IsLeaf { get { return Geometry != null; } }

            /// <summary>
            /// Gets a value indicating whether the node is full.
            /// </summary>
            /// <value><c>true</c> if the node contains the maximum number of elements; otherwise, <c>false</c>.</value>
            public Boolean IsFull { get { return (ChildrenCount == MaxChildren); } }

            /// <summary>
            /// Gets a value indicating whether the node is overflown.
            /// </summary>
            /// <value><c>true</c> if the node contains more than the maximum number of elements; otherwise, <c>false</c>.
            /// </value>
            public Boolean IsOverflown { get { return (ChildrenCount > MaxChildren); } }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="RTreeNode" /> class.
            /// </summary>
            /// <param name="maxChildren">The maximum number of entries that can be contained in the node.</param>
            public RTreeNode(Int32 maxChildren)
            {
                MaxChildren = maxChildren;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RTreeNode" /> class.
            /// </summary>
            /// <param name="parent">The parent of the node.</param>
            public RTreeNode(RTreeNode parent)
            {
                Parent = parent;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RTreeNode" /> class.
            /// </summary>
            /// <param name="parent">The parent of the node.</param>
            /// <param name="geometry">The <see cref="IGeometry" /> contained by the node.</param>
            public RTreeNode(IGeometry geometry, RTreeNode parent = null)
                : this(parent)
            {
                Geometry = geometry;
            }

            #endregion

            #region Public methods

            /// <summary>
            /// Adds a new child to the node.
            /// </summary>
            /// <param name="child">The child node.</param>
            public void AddChild(RTreeNode child)
            {

                child.Parent = this;
                if (Children == null)
                    Children = new List<RTreeNode>(MaxChildren);

                Children.Add(child);
                CorrectBounding(child.Envelope);
            }

            /// <summary>
            /// Removes a child of the node.
            /// </summary>
            /// <param name="node">The node to be removed.</param>
            public void RemoveChild(RTreeNode node)
            {
                if (node == null)
                    throw new ArgumentNullException("The node is null", "node");
                if (Children == null || !Children.Contains(node))
                    throw new ArgumentException("The specified node is not a child node.", "nodeToRemove");

                Children.Remove(node);

                if (ChildrenCount == 0)
                    Children = null;

                CorrectBounding();
            }

            /// <summary>
            /// Corrects the node's minimum bounding envelope.
            /// </summary>
            /// <param name="enlargingEnvelope">The envelope the node needs to be extended with, or <c>null</c>, if the envelope needs to be shrinked.</param>
            public void CorrectBounding(Envelope enlargingEnvelope = null)
            {
                if (enlargingEnvelope == null) //possible element removing from the children
                {
                    Envelope = (ChildrenCount > 0) ? Envelope.FromEnvelopes(Children.Select(x => x.Envelope)) : null;
                    return;
                }

                if (Envelope == null)
                    Envelope = enlargingEnvelope;
                else if (!Envelope.Contains(enlargingEnvelope))
                    Envelope = Envelope.FromEnvelopes(new Envelope[] { Envelope, enlargingEnvelope });
            }

            /// <summary>
            /// Computes how much the envelope's surface is enlarged with the parameter envelope.
            /// </summary>
            /// <param name="envelope">The envelope the node needs to be enlarged with.</param>
            /// <returns>The enlargement.</returns>
            public Double ComputeEnlargement(Envelope envelope)
            {
                if (Envelope.Contains(envelope))
                    return 0;

                Envelope enlarged = Envelope.FromEnvelopes(new Envelope[] { Envelope, envelope });

                return enlarged.Surface - Envelope.Surface;
            }

            /// <summary>
            /// Determines whether the node contains the parameter child.
            /// </summary>
            /// <param name="child">The child.</param>
            /// <returns></returns>
            public Boolean ContainsChild(RTreeNode child)
            {
                return (Children != null) && Children.Contains(child);
            }

            #endregion
        }

        #endregion

        #region Private constants

        /// <summary>
        /// The default minimum child count. This field is constant.
        /// </summary>
        private const Int32 DefaultMinChildCount = 2;

        /// <summary>
        /// The default maximum child count. This field is constant.
        /// </summary>
        private const Int32 DefaultMaxChildCount = 8;

        #endregion

        #region Protected fields

        protected RTreeNode _root;
        protected Int32 _numberOfGeometries;
        protected Int32 _height;
        protected Int32 _minChildren;

        #endregion

        #region ISpatialIndex properties

        /// <summary>
        /// Gets the number of indexed geometries.
        /// </summary>
        /// <value>The number of indexed geometries.</value>
        public Int32 NumberOfGeometries 
        {
            get { return _numberOfGeometries; }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the maximum number of children contained in a node.
        /// </summary>
        /// <value>The maximum number of children contained in a node.</value>
        public Int32 MaxChildren { get { return _root.MaxChildren; } }

        /// <summary>
        /// Gets the minimum number of children contained in a node.
        /// </summary>
        /// <value>The minimum number of children contained in a node.</value>
        public Int32 MinChildren { get { return _minChildren; } }

        /// <summary>
        /// Gets the height of the tree.
        /// </summary>
        /// <value>The number of levels in the tree under the root node.</value>
        public Int32 Height { get { return _height; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RTree" /> class.
        /// </summary>
        public RTree() : this(DefaultMinChildCount, DefaultMaxChildCount) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RTree" /> class.
        /// </summary>
        /// <param name="minChildren">The minimum number of child nodes.</param>
        /// <param name="maxChildren">The maximum number of child nodes.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The minimum number of child nodes is less than 1.
        /// or
        /// The maximum number of child nodes is less than 2.
        /// or
        /// The maximum number of child nodes is less than double of the minimum number.
        /// </exception>
        public RTree(Int32 minChildren, Int32 maxChildren)
        {
            if (minChildren < 1)
                throw new ArgumentOutOfRangeException("minChildren", "The minimum number of child nodes is less than 1.");
            if (maxChildren < 2)
                throw new ArgumentOutOfRangeException("maxChildren", "The maximum number of child nodes is less than 2.");
            if (minChildren > maxChildren / 2)
                throw new ArgumentOutOfRangeException("maxChildren", "The maximum number of child nodes is less than double of the minimum number.");

            _root = new RTreeNode(maxChildren);
            _numberOfGeometries = 0;
            _height = 0;
            _minChildren = minChildren;
        }

        #endregion

        #region ISpatialIndex methods

        /// <summary>
        /// Adds a geometry to the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public void Add(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            AddNode(new RTreeNode(geometry));
            _numberOfGeometries++;
        }

        /// <summary>
        /// Adds multiple geometries to the index.
        /// </summary>
        /// <param name="collection">The geometry collection.</param>
        /// <exception cref="System.ArgumentNullException">The collection is null.</exception>
        public void Add(IEnumerable<IGeometry> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection", "The collection is null.");

            foreach (IGeometry geometry in collection)
            {
                if (geometry != null)
                {
                    AddNode(new RTreeNode(geometry));
                    _numberOfGeometries++;
                }
            }
        }

        /// <summary>
        /// Searches the index for any geometries contained within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The list of geometries located within the envelope.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public IList<IGeometry> Search(Envelope envelope)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "The envelope is null.");

            List<IGeometry> result = new List<IGeometry>();
            SearchNode(_root, envelope, result);
            return result;
        }

        /// <summary>
        /// Determines whether the specified geometry is indexed.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the specified geometry is indexed; otherwisem <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public virtual Boolean Contains(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            return ContainsGeometry(geometry, _root);
        }

        /// <summary>
        /// Removes the specified geometry from the index.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c> if the geometry is indexed; ortherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        public virtual Boolean Remove(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            if (!RemoveGeometry(geometry))
                return false;

            _numberOfGeometries--;
            return true;
        }

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope)
        {
            IList<IGeometry> geometries;

            return Remove(envelope, out geometries);
        }

        /// <summary>
        /// Removes all geometries from the index within the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="geometries">The list of geometries within the envelope.</param>
        /// <returns><c>true</c> if any geometries are within the envelope; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The envelope is null.</exception>
        public Boolean Remove(Envelope envelope, out IList<IGeometry> geometries)
        {
            if (envelope == null)
                throw new ArgumentNullException("envelope", "The envelope is null.");

            geometries = new List<IGeometry>();
            SearchNode(_root, envelope, geometries);

            if (geometries.Count == 0)
                return false;

            for (Int32 i = 0; i < geometries.Count; i++)
                RemoveGeometry(geometries[i]);

            _numberOfGeometries -= geometries.Count;
            return true;
        }

        /// <summary>
        /// Clears all geometries from the index.
        /// </summary>
        public void Clear()
        {
            _root = new RTreeNode(_root.MaxChildren);
            _height = 0;
            _numberOfGeometries = 0;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Adds a node into the tree on a specified height. 
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="height">The height where the node should be inserted.</param>
        protected virtual void AddNode(RTreeNode node, Int32 height = -1)
        {
            RTreeNode nodeToInsert = ChooseNodeToAdd(node.Envelope, height);

            if (!nodeToInsert.IsFull)
            {
                if (nodeToInsert == _root && nodeToInsert.ChildrenCount == 0)
                    _height = 1;

                nodeToInsert.AddChild(node);
                AdjustTree(nodeToInsert);
            }
            else
            {
                nodeToInsert.AddChild(node);

                RTreeNode firstNode, secondNode;
                SplitNode(nodeToInsert, out firstNode, out secondNode);
                AdjustTree(firstNode, secondNode, nodeToInsert);
            }
        }

        /// <summary>
        /// Collects all geometries inside an envelope.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        /// <param name="envelope">The envelope.</param>
        /// <param name="geometries">The list of geometries which are inside the envelope.</param>
        protected void SearchNode(RTreeNode node, Envelope envelope, IList<IGeometry> geometries)
        {
            if (node.Children == null)
                return;

            if (node.IsLeafContainer)
            {
                foreach (RTreeNode leaf in node.Children)
                    if (envelope.Contains(leaf.Geometry.Envelope))
                        geometries.Add(leaf.Geometry);
            }
            else
            {
                foreach (RTreeNode child in node.Children)
                    if (envelope.Intersects(child.Envelope))
                        SearchNode(child, envelope, geometries);
            }
        }

        /// <summary>
        /// Determines whether the tree contains a geometry in a specified subtree.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="node">The root node of the subtree.</param>
        /// <returns><c>true</c>, if the element contains the geometry, otherwise, <c>false</c>.</returns>
        protected Boolean ContainsGeometry(IGeometry geometry, RTreeNode node)
        {
            if (node == null)
                return false;

            if (node.IsLeafContainer && node.Children != null && node.Children.Any(x => x.Geometry == geometry))
                return true;

            Boolean result = false;

            for (Int32 i = 0; i < node.ChildrenCount && !result; i++)
            {
                if (!node.Envelope.Contains(geometry.Envelope))
                    continue;

                result = ContainsGeometry(geometry, node.Children[i]);
            }
                
            return result;
        }

        /// <summary>
        /// Removes the specified geometry from the tree.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns><c>true</c>, if the tree contains the geometry, otherwise, <c>false</c>.</returns>
        protected virtual Boolean RemoveGeometry(IGeometry geometry)
        {
            RTreeNode leafContainer = null;
            FindLeafContainer(geometry, _root, ref leafContainer);

            if (leafContainer == null)
                return false;

            RTreeNode nodeToRemove = leafContainer.Children.FirstOrDefault(x => x.Geometry == geometry);

            if (nodeToRemove == null)
                return false;

            leafContainer.RemoveChild(nodeToRemove);

            CondenseTree(leafContainer);

            while (_root.ChildrenCount == 1 && !_root.IsLeafContainer)
            {
                _root = _root.Children[0];
                _root.Parent = null;
                _height--;
            }

            if (_root.ChildrenCount == 0) // occurs when the last element is removed
                Clear();

            return true;
        }

        /// <summary>
        /// Adjusts the tree after insertion, and corrects the bounding envelope of the nodes.
        /// </summary>
        /// <param name="node">The node where the adjustment starts.</param>
        /// <param name="splitted">The second part of the node if the original node was splitted.</param>
        /// <param name="nodeToRemove">The original node which should be removed if the original node was splitted.</param>
        protected void AdjustTree(RTreeNode node, RTreeNode splitted = null, RTreeNode nodeToRemove = null)
        {
            RTreeNode n = node;
            RTreeNode nn = splitted;

            while (n.Parent != null)
            {
                RTreeNode parent = n.Parent;

                parent.CorrectBounding(n.Envelope);

                if (nn == null)
                {
                    n = parent;
                }
                else
                {
                    if (nodeToRemove != null)
                    {
                        parent.RemoveChild(nodeToRemove);
                        parent.AddChild(n);
                    }

                    if (!parent.IsFull)
                    {
                        parent.AddChild(nn);
                        n = parent;
                        nn = null;
                    }
                    else
                    {
                        parent.AddChild(nn);
                        SplitNode(parent, out n, out nn);

                        nodeToRemove = parent;
                    }
                }
            }

            // create new root node if the root is splitted
            if (nn != null)
            {
                _root = new RTreeNode(n.MaxChildren);
                _root.AddChild(n);
                _root.AddChild(nn);

                _height++;
            }

        }

        /// <summary>
        /// Splits a node into two nodes.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="firstNode">The first produced node.</param>
        /// <param name="secondNode">The second produced node.</param>
        protected virtual void SplitNode(RTreeNode overflownNode, out RTreeNode firstNode, out RTreeNode secondNode)
        {
            RTreeNode firstSeed, secondSeed;
            PickSeeds(overflownNode.Children, out firstSeed, out secondSeed);

            firstNode = (overflownNode.Parent != null) ? new RTreeNode(overflownNode.Parent) : new RTreeNode(overflownNode.MaxChildren);
            secondNode = (overflownNode.Parent != null) ? new RTreeNode(overflownNode.Parent) : new RTreeNode(overflownNode.MaxChildren);

            firstNode.AddChild(firstSeed);
            secondNode.AddChild(secondSeed);

            overflownNode.Children.Remove(firstSeed);
            overflownNode.Children.Remove(secondSeed);

            while (overflownNode.ChildrenCount > 0)
            {
                RTreeNode node = PickNext(overflownNode.Children);

                if (firstNode.ChildrenCount + overflownNode.ChildrenCount <= MinChildren)
                    firstNode.AddChild(node);
                else if (secondNode.ChildrenCount + overflownNode.ChildrenCount <= MinChildren)
                    secondNode.AddChild(node);
                else
                {
                    Double firstEnlargement = firstNode.ComputeEnlargement(node.Envelope);
                    Double secondEnlargement = secondNode.ComputeEnlargement(node.Envelope);

                    if (firstEnlargement < secondEnlargement)
                        firstNode.AddChild(node);
                    else if (firstEnlargement > secondEnlargement)
                        secondNode.AddChild(node);
                    else
                    {
                        if (firstNode.Envelope.Surface < secondNode.Envelope.Surface)
                            firstNode.AddChild(node);
                        else
                            secondNode.AddChild(node);
                    }
                }
            }
        }

        /// <summary>
        /// Chooses seed elements for splitting a node using the linear cost algorithm.
        /// </summary>
        /// <param name="nodes">The nodes contained by the overflown node.</param>
        /// <param name="firstNode">The first seed node.</param>
        /// <param name="secondNode">The second seed node.</param>
        protected virtual void PickSeeds(IList<RTreeNode> nodes, out RTreeNode firstNode, out RTreeNode secondNode)
        {
            // the linear cost algorithm chooses the two points which are 
            // the furthest away from each other considering the three axis

            RTreeNode highestLowX = nodes[0];
            RTreeNode highestLowY = nodes[0];
            RTreeNode highestLowZ = nodes[0];

            for (Int32 i = 1; i < nodes.Count; i++)
            {
                if (highestLowX.Envelope.MinX < nodes[i].Envelope.MinX)
                    highestLowX = nodes[i];
                if (highestLowY.Envelope.MinY < nodes[i].Envelope.MinY)
                    highestLowY = nodes[i];
                if (highestLowZ.Envelope.MinZ < nodes[i].Envelope.MinZ)
                    highestLowZ = nodes[i];
            }

            RTreeNode lowestHighX = nodes.FirstOrDefault(x => x != highestLowX);
            RTreeNode lowestHighY = nodes.FirstOrDefault(x => x != highestLowY);
            RTreeNode lowestHighZ = nodes.FirstOrDefault(x => x != highestLowZ);

            Double minX, minY, minZ;
            minX = minY = minZ = Double.MaxValue;

            Double maxX, maxY, maxZ;
            maxX = maxY = maxZ = Double.MinValue;

            foreach (RTreeNode node in nodes)
            {
                if (node.Envelope.MaxX < lowestHighX.Envelope.MaxX && node != highestLowX)
                    lowestHighX = node;

                if (node.Envelope.MaxY < lowestHighY.Envelope.MaxY && node != highestLowY)
                    lowestHighY = node;

                if (node.Envelope.MaxZ < lowestHighZ.Envelope.MaxZ && node != highestLowZ)
                    lowestHighZ = node;

                if (node.Envelope.MinX < minX)
                    minX = node.Envelope.MinX;
                if (node.Envelope.MaxX > maxX)
                    maxX = node.Envelope.MaxX;

                if (node.Envelope.MinY < minY)
                    minY = node.Envelope.MinY;
                if (node.Envelope.MaxY > maxY)
                    maxY = node.Envelope.MaxY;

                if (node.Envelope.MinZ < minZ)
                    minZ = node.Envelope.MinZ;
                if (node.Envelope.MaxZ > maxZ)
                    maxZ = node.Envelope.MaxZ;
            }

            Double normalizedDistanceX = (highestLowX.Envelope.MinX - lowestHighX.Envelope.MaxX) / (maxX - minX);
            Double normalizedDistanceY = (highestLowY.Envelope.MinY - lowestHighY.Envelope.MaxY) / (maxY - minY);


            if (normalizedDistanceX >= normalizedDistanceY)
            {
                firstNode = lowestHighX;
                secondNode = highestLowX;
            }
            else
            {
                firstNode = lowestHighY;
                secondNode = highestLowY;
            }

            if (!lowestHighZ.Envelope.IsPlanar || !highestLowZ.Envelope.IsPlanar)
            {
                Double normalizedDistanceZ = (highestLowZ.Envelope.MinZ - lowestHighZ.Envelope.MaxZ) / (maxZ - minZ);

                if ((firstNode == lowestHighX && normalizedDistanceZ > normalizedDistanceX) ||
                    (firstNode == lowestHighY && normalizedDistanceZ > normalizedDistanceX))
                {
                    firstNode = lowestHighZ;
                    secondNode = highestLowZ;
                }
            }
        }

        /// <summary>
        /// Chooses the next node for uploading the splitted nodes with the original node's children.
        /// </summary>
        /// <param name="nodes">The children of the original node.</param>
        /// <returns>The chosen next node.</returns>
        /// <exception cref="System.ArgumentException">The node collection is empty.;nodes</exception>
        protected virtual RTreeNode PickNext(IList<RTreeNode> nodes)
        {
            if (nodes.Count == 0)
                throw new ArgumentException("The node collection is empty.", "nodes");

            RTreeNode result = nodes[0];
            nodes.RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Chooses a node where a new node should be added.
        /// </summary>
        /// <param name="envelope">The envelope of the new node.</param>
        /// <param name="height">The height where the new node should be added.</param>
        /// <returns>The node where the new node should be added.</returns>
        protected virtual RTreeNode ChooseNodeToAdd(Envelope envelope, Int32 height)
        {
            RTreeNode p = _root;

            Int32 i = 0;

            while (!p.IsLeafContainer && i != height)
            {

                Double minimumEnlargement = Double.MaxValue;
                Double minimumSurface = Double.MaxValue;
                RTreeNode selectedLeaf = null;
                foreach (RTreeNode child in p.Children)
                {
                    Double enlargement = child.ComputeEnlargement(envelope);
                    if (minimumEnlargement > enlargement)
                    {
                        minimumEnlargement = enlargement;
                        selectedLeaf = child;
                    }
                    else if (minimumEnlargement == enlargement)
                    {

                        Double surface = child.Envelope.Surface;
                        if (minimumSurface > surface)
                        {
                            minimumSurface = surface;
                            selectedLeaf = child;
                        }
                    }
                }

                p = selectedLeaf;
                i++;
            }

            return p;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Searches the leaf container which contains the leaf node.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="node">The node where the search starts.</param>
        /// <param name="resultLeafContainer">The found leaf container.</param>
        private void FindLeafContainer(IGeometry geometry, RTreeNode node, ref RTreeNode resultLeafContainer)
        {
            if (!node.IsLeafContainer)
            {
                foreach (RTreeNode child in node.Children)
                    if (child.Envelope.Contains(geometry.Envelope))
                        FindLeafContainer(geometry, child, ref resultLeafContainer);
            }
            else
            {
                foreach (RTreeNode child in node.Children)
                    if (child.Geometry == geometry && resultLeafContainer == null)
                        resultLeafContainer = node;
            }
        }

        /// <summary>
        /// Condenses the tree after removing a leaf node.
        /// </summary>
        /// <param name="leafContainerNode">The node which contained the removed node.</param>
        private void CondenseTree(RTreeNode leafContainerNode)
        {
            Dictionary<RTreeNode, Int32> deletedNodes = new Dictionary<RTreeNode, Int32>();
            RTreeNode p = leafContainerNode;

            Int32 height = Height - 1;

            while (p.Parent != null)
            {
                RTreeNode parent = p.Parent;
                if (p.ChildrenCount < MinChildren)
                {
                    parent.RemoveChild(p);

                    if (p.Children != null)
                        foreach (RTreeNode node in p.Children)
                            deletedNodes.Add(node, height);
                }
                else
                    p.CorrectBounding();

                p = parent;
                height--;
            }

            Int32 startHeight = Height;
            foreach (KeyValuePair<RTreeNode, Int32> reInsert in deletedNodes)
                AddNode(reInsert.Key, reInsert.Value + (Height - startHeight));
        }

        #endregion
    }
}
