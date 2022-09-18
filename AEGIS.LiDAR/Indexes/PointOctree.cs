/// <copyright file="PointOctree.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a 3D Octree, which contains a collection of <typeparamref name="T" /> instances.
    /// </summary>
    public class PointOctree<T> : PointQuadTree<T>
    {
        /// <summary>
        /// Represents a node of the Octree.
        /// </summary>
        protected class PointOctreeNode : PointQuadTreeNode
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PointOctreeNode"/> class.
            /// </summary>
            /// <param name="envelope">The region of the node.</param>
            /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
            /// <param name="WorldSize">The size of the Node.</param>
            /// <param name="Depth">The depth of the node.</param>
            public PointOctreeNode(Envelope envelope, Double MinNodeSize, Double WorldSize, int Depth)
                : base(envelope, MinNodeSize, WorldSize, Depth)
            { }

            /// <summary>
            /// The number of children nodes.
            /// </summary>
            protected new const int NumOfChildren = 8;

            /// <summary>
            /// Creates the children nodes of a node.
            /// </summary>
            protected override void CreateChildren()
            {
                Double minX = this.Envelope.MinX;
                Double midX = this.Envelope.MinX + (this.Envelope.MaxX - this.Envelope.MinX) / 2;
                Double maxX = this.Envelope.MaxX;

                Double minY = this.Envelope.MinY;
                Double midY = this.Envelope.MinY + (this.Envelope.MaxY - this.Envelope.MinY) / 2;
                Double maxY = this.Envelope.MaxY;

                Double minZ = this.Envelope.MinZ;
                Double midZ = this.Envelope.MinZ + (this.Envelope.MaxZ - this.Envelope.MinZ) / 2;
                Double maxZ = this.Envelope.MaxZ;

                Double newSize = WorldSize / 2;
                int newDepth = Depth + 1;

                this.children.Add(new PointOctreeNode(new Envelope(minX, midX, minY, midY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(minX, midX, minY, midY, midZ, maxZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(minX, midX, midY, maxY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(minX, midX, midY, maxY, midZ, maxZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(midX, maxX, minY, midY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(midX, maxX, minY, midY, midZ, maxZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(midX, maxX, midY, maxY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeNode(new Envelope(midX, maxX, midY, maxY, midZ, maxZ), MinNodeSize, newSize, newDepth));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointOctree" /> class.
        /// </summary>
        /// <param name="envelope">The maximum indexed region.</param>
        /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
        public PointOctree(Envelope envelope, Double MinNodeSize) : base(MinNodeSize)
        {
            this.root = new PointOctreeNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointOctree" /> class.
        /// </summary>
        /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
        protected PointOctree(double MinNodeSize)
            : base(MinNodeSize)
        { }

        /// <summary>
        /// Clears all objects from the index.
        /// </summary>
        public override void Clear()
        {
            this.root = new PointOctreeNode(this.root.Envelope, MinNodeSize, this.root.WorldSize, 0);
        }

        /// <summary>
        /// Creates a new tree based on an unindexed object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="point">The coordinate of the object.</param>
        protected override void CreateNew(T obj, Coordinate point)
        {
            List<TreeObject> allPoints = this.GetAllWithCoords();
            Envelope envelope = Envelope.FromEnvelopes(this.root.Envelope, Envelope.FromCoordinates(point));
            this.root = new PointOctreeNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
            this.Add(obj, point);
            foreach (TreeObject treeObject in allPoints)
            {
                this.Add(treeObject.Obj, treeObject.Point);
            }
        }

        /// <summary>
        /// Rebuilds the tree. Useful if you removed a lot of objects from the index.
        /// </summary>
        public override void Rebuild()
        {
            List<TreeObject> allPoints = this.GetAllWithCoords();
            Envelope envelope = Envelope.FromCoordinates(allPoints.Select(x => x.Point));
            this.root = new PointOctreeNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
            foreach (TreeObject treeObject in allPoints)
            {
                this.Add(treeObject.Obj, treeObject.Point);
            }
        }
    }
}
