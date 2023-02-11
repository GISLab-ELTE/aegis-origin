// <copyright file="PointQuadTreeNode.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.LiDAR.Indexes
{
    public partial class PointQuadTree<T> : IPointBasedSpatialIndex<T>
    {
        /// <summary>
        /// Represents a node of the Quad-tree.
        /// </summary>
        /// <author>Roland Krisztandl</author>
        protected class PointQuadTreeNode
        {
            /// <summary>
            /// The children nodes of this node. Each internal node must always have 4 children.
            /// </summary>
            protected readonly List<PointQuadTreeNode> children;

            /// <summary>
            /// The envelope of the node.
            /// </summary>
            protected readonly Envelope envelope;

            /// <summary>
            /// The objects stored in this node.
            /// </summary>
            protected readonly List<TreeObject> contents;

            /// <summary>
            /// If there are already NumObjectsAllowed in a node, we split it into children.
            /// </summary>
            protected const int NumObjectsAllowed = 8;

            /// <summary>
            /// The number of children nodes.
            /// </summary>
            protected const int NumOfChildren = 4;

            /// <summary>
            /// Nodes will stop splitting if the new nodes would be smaller than this.
            /// </summary>
            protected readonly Double MinNodeSize;

            /// <summary>
            /// The size of the current Node.
            /// </summary>
            public readonly Double WorldSize;

            /// <summary>
            /// The depth of this node. (The root is 0.)
            /// </summary>
            public readonly int Depth;

            /// <summary>
            /// Initializes a new instance of the <see cref="PointQuadTreeNode"/> class.
            /// </summary>
            /// <param name="envelope">The region of the node.</param>
            /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
            /// <param name="WorldSize">The size of the Node.</param>
            /// <param name="Depth">The depth of the node.</param>
            public PointQuadTreeNode(Envelope envelope, Double MinNodeSize, Double WorldSize, int Depth)
            {
                this.envelope = envelope;
                this.MinNodeSize = MinNodeSize;
                this.WorldSize = WorldSize;
                this.Depth = Depth;
                this.children = new List<PointQuadTreeNode>(NumOfChildren);
                this.contents = new List<TreeObject>(NumObjectsAllowed);
            }

            /// <summary>
            /// Gets the region of the node.
            /// </summary>
            /// <value>The region of the node.</value>
            public Envelope Envelope
            {
                get
                {
                    return this.envelope;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the node is a leaf node.
            /// </summary>
            /// <value><c>true</c> if the node is a leaf node, otherwise <c>false</c>.</value>
            public Boolean IsLeaf { get { return this.children.Count == 0; } }

            /// <summary>
            /// Gets a value indicating whether the node is empty or not.
            /// </summary>
            /// <value><c>true</c> if the node is a leaf node and has no objects, otherwise <c>false</c>.</value>
            public Boolean IsEmpty { get { return this.contents.Count == 0 && this.IsLeaf; } }

            /// <summary>
            /// Gets the contents of this node.
            /// </summary>
            /// <value>The objects stored in this node.</value>
            public List<TreeObject> Contents { get { return this.contents; } }

            /// <summary>
            /// Gets the number of points in this subtree.
            /// </summary>
            public Int32 NumberOfPoints
            {
                get
                {
                    Int32 count = 0;
                    foreach (PointQuadTreeNode node in this.children)
                        count += node.NumberOfPoints;

                    count += this.contents.Count;
                    return count;
                }
            }

            /// <summary>
            /// Gets the number of children nodes in this subtree.
            /// </summary>
            public Int32 NumberOfNodes
            {
                get
                {
                    Int32 count = 0;
                    foreach (PointQuadTreeNode node in this.children)
                        count += node.NumberOfNodes;

                    count += this.children.Count;
                    return count;
                }
            }

            /// <summary>
            /// Searches this subtree for objects contained in the envelope.
            /// </summary>
            /// <param name="envelope">The envelope which has to contain the objects.</param>
            /// <returns>A list of objects from the tree which are inside the envelope.</returns>
            public List<T> Search(Envelope envelope)
            {
                List<T> result = new List<T>();

                // Recursively call Search for children whose bound intersects with the envelope.
                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.IsEmpty)
                        continue;

                    if (envelope.Intersects(child.envelope))
                        result.AddRange(child.Search(envelope));
                }

                // Adding the valid contents from this node
                foreach (var content in this.contents)
                {
                    if (envelope.Contains(content.Point))
                        result.Add(content.Obj);
                }

                return result;
            }

            /// <summary>
            /// Searches this subtree for objects with their coordinates contained in the envelope.
            /// </summary>
            /// <param name="envelope">The envelope which has to contain the objects.</param>
            /// <returns>A list of objects with their coordinates from the tree which are inside the envelope.</returns>
            public List<TreeObject> SearchWithCoords(Envelope envelope)
            {
                List<TreeObject> result = new List<TreeObject>();

                // Recursively call Search for children whose bound intersects with the envelope.
                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.IsEmpty)
                        continue;

                    if (envelope.Intersects(child.envelope))
                        result.AddRange(child.SearchWithCoords(envelope));
                }

                // Adding the valid contents from this node
                foreach (var content in this.contents)
                {
                    if (envelope.Contains(content.Point))
                        result.Add(content);
                }

                return result;
            }

            /// <summary>
            /// Returns every object located in this node and its children.
            /// </summary>
            /// <returns>A list of object from the tree.</returns>
            public List<T> GetAll()
            {
                List<T> result = new List<T>();

                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.IsEmpty)
                        continue;

                    result.AddRange(child.GetAll());
                }

                foreach (var content in this.contents)
                {
                    result.Add(content.Obj);
                }

                return result;
            }

            /// <summary>
            /// Returns every object with their coordinates located in this node and its children.
            /// </summary>
            /// <returns>A list of object with their coordinates from the tree.</returns>
            public List<TreeObject> GetAllWithCoords()
            {
                List<TreeObject> result = new List<TreeObject>();

                // Recursively call Search for children whose bound intersects with the envelope.
                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.IsEmpty)
                        continue;

                    result.AddRange(child.GetAllWithCoords());
                }

                // Adding the valid contents from this node
                foreach (var content in this.contents)
                {
                    result.Add(content);
                }

                return result;
            }

            /// <summary>
            /// Adds an object to this subtree.
            /// </summary>
            /// <param name="obj">Object to add.</param>
            /// <param name="point">Coordinate of the object.</param>
            public void Add(T obj, Coordinate point)
            {
                // If this is a leaf node without contents, add point to this node and return
                if (this.IsLeaf && (contents.Count < NumObjectsAllowed || WorldSize / 2 < MinNodeSize))
                {
                    this.contents.Add(new TreeObject(obj, point));
                    return;
                }

                // If this is a leaf node with contents, create children and subdivide current contents between the children
                if (this.children.Count == 0)
                {
                    this.CreateChildren();
                    this.SubdivideContents();
                }

                // Find child which has an envelope that contains current point, and recursively call Add on that child, then return
                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.envelope.Contains(point))
                    {
                        child.Add(obj, point);
                        return;
                    }
                }

                // If we got here it means that this node's envelope contains point, but no child's envelope does. Add point to this node.
                this.contents.Add(new TreeObject(obj, point));
                throw new Exception("The point fits to the node, but does not fit its childern");
            }

            /// <summary>
            /// Removes the specified object from the index.
            /// </summary>
            /// <param name="obj">Object to remove</param>
            /// <param name="point">Coordinate of the object.</param>
            /// <returns><c>true</c> if the removal was successful, otherwise <c>false</c>.</returns>
            public Boolean Remove(T obj, Coordinate point)
            {
                for (int i = 0; i < Contents.Count; i++)
                {
                    if (Contents[i].Obj.Equals(obj) && Contents[i].Point.Equals(point))
                    {
                        return Contents.Remove(Contents[i]);
                    }
                }

                var result = false;
                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.envelope.Contains(point))
                    {
                        result = result || child.Remove(obj, point);
                    }
                }

                return result;
            }

            /// <summary>
            /// Removes an already existing TreeObject from the tree.
            /// </summary>
            /// <param name="obj">The TreeObject to remove.</param>
            /// <returns><c>true</c> if the object is indexed; otherwise <c>false</c>.</returns>
            public Boolean Remove(TreeObject obj)
            {
                if (Contents.Remove(obj))
                    return true;

                var result = false;
                foreach (PointQuadTreeNode child in this.children)
                {
                    if (child.envelope.Contains(obj.Point))
                    {
                        result = result || child.Remove(obj);
                    }
                }

                return result;
            }

            /// <summary>
            /// Creates the children nodes of a node.
            /// </summary>
            protected virtual void CreateChildren()
            {
                Double midX = this.envelope.MinX + (this.envelope.MaxX - this.envelope.MinX) / 2;
                Double midY = this.envelope.MinY + (this.envelope.MaxY - this.envelope.MinY) / 2;

                Double newSize = WorldSize / 2;
                int newDepth = Depth + 1;

                this.children.Add(new PointQuadTreeNode(new Envelope(this.envelope.MinX, midX, this.envelope.MinY, midY, 0, 0), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointQuadTreeNode(new Envelope(midX, this.envelope.MaxX, this.envelope.MinY, midY, 0, 0), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointQuadTreeNode(new Envelope(this.envelope.MinX, midX, midY, this.envelope.MaxY, 0, 0), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointQuadTreeNode(new Envelope(midX, this.envelope.MaxX, midY, this.envelope.MaxY, 0, 0), MinNodeSize, newSize, newDepth));
            }

            /// <summary>
            /// Subdivides a nodes objects between its children.
            /// </summary>
            protected void SubdivideContents()
            {
                bool found;
                foreach (var content in this.contents)
                {
                    found = false;
                    foreach (PointQuadTreeNode child in this.children)
                    {
                        if (found) break;
                        else if (child.Envelope.Contains(content.Point))
                        {
                            child.Add(content.Obj, content.Point);
                            found = true;
                        }
                    }
                }

                contents.Clear();
            }
        }
    }
}
