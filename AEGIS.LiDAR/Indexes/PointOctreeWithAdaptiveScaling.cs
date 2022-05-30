/// <copyright file="PointOctreeWithAdaptiveScaling.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.IO.Lasfile;
using ELTE.AEGIS.LiDAR.Operations.Subsampling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.LiDAR.Indexes
{
    /// <summary>
    /// Represents a 3D Octree, which contains a collection of LasPointBase instances.
    /// This version of the Octree has the option to generate and store subsamples in the nodes.
    /// </summary>
    public class PointOctreeWithAdaptiveScaling : PointOctree<LasPointBase>
    {
        /// <summary>
        /// Represents a node of the PointOctreeWithAdaptiveScaling.
        /// </summary>
        protected class PointOctreeSamplingNode : PointOctreeNode
        {
            /// <summary>
            /// Contains the subsample stored in this node.
            /// </summary>
            public List<LasPointBase> Subsample;

            /// <summary>
            /// Stores the maximum depth of the subsample.
            /// </summary>
            public int SubsampleMaxDepth;

            /// <summary>
            /// Initializes a new instance of the <see cref="PointOctreeSamplingNode"/> class.
            /// </summary>
            /// <param name="envelope">The envelope of the node.</param>
            /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
            /// <param name="WorldSize">The size of the Node.</param>
            /// <param name="Depth">The depth of the node.</param>
            public PointOctreeSamplingNode(Envelope envelope, Double MinNodeSize, Double WorldSize, int Depth)
                : base(envelope, MinNodeSize, WorldSize, Depth)
            {
                Subsample = new List<LasPointBase>();
            }

            /// <summary>
            /// Collects the subsamples from the nodes. Closer the <paramref name="coordinate"/> to the node bigger the depth.
            /// Ignore's the coordinate's Z (height) value.
            /// </summary>
            /// <returns>The adaptive subsample.</returns>
            public List<LasPointBase> GetSubsamples(Coordinate coordinate)
            {
                List<LasPointBase> result = new List<LasPointBase>();

                System.Diagnostics.Debug.WriteLine(Depth);

                if (Depth > SubsampleMaxDepth)
                    result.AddRange(GetAll());
                else if (ContainsXY(envelope, coordinate))
                {
                    foreach (PointOctreeSamplingNode child in children)
                    {
                        if (child.IsEmpty)
                            continue;

                        result.AddRange(child.GetSubsamples(coordinate));
                    }
                }
                else
                {
                    if (Subsample.Count == 0) result.AddRange(GetAll());
                    else result.AddRange(Subsample);
                }

                return result;
            }

            private Boolean ContainsXY(Envelope envelope, Coordinate coordinate)
            {
                return envelope.Minimum.X <= coordinate.X && coordinate.X <= envelope.Maximum.X &&
                       envelope.Minimum.Y <= coordinate.Y && coordinate.Y <= envelope.Maximum.Y;
            }

            /// <summary>
            /// Generates the subsamples.
            /// </summary>
            /// <param name="depth">Generates the subsamples for every node until it reaches this depth. (Root is 0, root's children are 1...)</param>
            public void GenerateSubsamples(int depth)
            {
                SubsampleMaxDepth = depth;

                if (Depth > depth || NumberOfPoints == 0)
                    return;

                foreach (PointOctreeSamplingNode child in children)
                {
                    if (child.IsEmpty)
                        continue;
                    else
                        child.GenerateSubsamples(depth);
                }

                double cellSize = WorldSize / 100;
                var points = GetAll();
                var sampling = new RandomGridSubsampling(points, cellSize, Environment.ProcessorCount);
                this.Subsample = sampling.Execute();
            }

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

                this.children.Add(new PointOctreeSamplingNode(new Envelope(minX, midX, minY, midY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(minX, midX, minY, midY, midZ, maxZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(minX, midX, midY, maxY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(minX, midX, midY, maxY, midZ, maxZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(midX, maxX, minY, midY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(midX, maxX, minY, midY, midZ, maxZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(midX, maxX, midY, maxY, minZ, midZ), MinNodeSize, newSize, newDepth));
                this.children.Add(new PointOctreeSamplingNode(new Envelope(midX, maxX, midY, maxY, midZ, maxZ), MinNodeSize, newSize, newDepth));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointOctreeWithAdaptiveScaling" /> class.
        /// </summary>
        /// <param name="envelope">The maximum indexed region.</param>
        /// <param name="MinNodeSize">Nodes will stop splitting if the new nodes would be smaller than this.</param>
        public PointOctreeWithAdaptiveScaling(Envelope envelope, Double MinNodeSize) : base(MinNodeSize)
        {
            this.root = new PointOctreeSamplingNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
        }

        /// <summary>
        /// Collects the subsamples from the nodes. Closer the <paramref name="coordinate"/> to the node bigger the depth.
        /// </summary>
        /// <returns>The adaptive subsample.</returns>
        public List<LasPointBase> GetSubsamples(Coordinate coordinate)
        {
            return (root as PointOctreeSamplingNode).GetSubsamples(coordinate);
        }

        /// <summary>
        /// Generates the subsamples.
        /// </summary>
        /// <param name="depth">Generates the subsamples for every node until it reaches this depth. (Root is 0, root's children are 1...)</param>
        public void GenerateSubsamples(int depth)
        {
            (root as PointOctreeSamplingNode).GenerateSubsamples(depth);
        }

        /// <summary>
        /// Clears all objects from the index.
        /// </summary>
        public override void Clear()
        {
            this.root = new PointOctreeSamplingNode(this.root.Envelope, MinNodeSize, this.root.WorldSize, 0);
        }

        /// <summary>
        /// Creates a new tree based on an unindexed object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="point">The coordinate of the object.</param>
        protected override void CreateNew(LasPointBase obj, Coordinate point)
        {
            List<TreeObject> allPoints = this.GetAllWithCoords();
            Envelope envelope = Envelope.FromEnvelopes(this.root.Envelope, Envelope.FromCoordinates(point));
            this.root = new PointOctreeSamplingNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
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
            this.root = new PointOctreeSamplingNode(envelope, MinNodeSize, CalculateWordSize(envelope), 0);
            foreach (TreeObject treeObject in allPoints)
            {
                this.Add(treeObject.Obj, treeObject.Point);
            }
        }
    }
}
