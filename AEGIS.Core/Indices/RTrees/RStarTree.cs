/// <copyright file="RstarTree.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
/// <author>Tamás Nagy</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Indices.RTrees
{

    /// <summary>
    /// Represents a 3D R*-Tree, which contains a collection of <see cref="IGeometry" /> instances.
    /// </summary>
    public class RStarTree : RTree
    {
        #region Private fields

        /// <summary>
        /// Indicates the levels where reinsertion happened during an insertion.
        /// </summary>
        Boolean[] _visitedLevels;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RStarTree" /> class.
        /// </summary>
        public RStarTree() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RStarTree" /> class.
        /// </summary>
        /// <param name="minChildren">The minimum number of children contained in a node.</param>
        /// <param name="maxChildren">The maximum number of children contained in a node.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The minimum number of child nodes is less than 1.
        /// or
        /// The maximum number of child nodes is less than or equal to the minimum number of child nodes.
        /// </exception>
        public RStarTree(Int32 minChildren, Int32 maxChildren) : base(minChildren, maxChildren) { }

        #endregion

        #region Protected methods

        /// <summary>
        /// Adds a node into the tree on a specified height. 
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="height">The height where the node should be inserted.</param>
        protected override void AddNode(RTreeNode node, Int32 height = -1)
        {
            _visitedLevels = new Boolean[Height];
            InsertInSpecifiedHeight(node, height, height);
        }

        /// <summary>
        /// Splits a node into two nodes.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="firstNode">The first produced node.</param>
        /// <param name="secondNode">The second produced node.</param>
        protected override void SplitNode(RTreeNode overflownNode, out RTreeNode firstNode, out RTreeNode secondNode) 
        {
            List<RTreeNode> alongChosenAxis = ChooseSplitAxis(overflownNode);

            Int32 minIndex = MinChildren;
            Double minOverlap = ComputeOverlap(Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, MinChildren).Select(x => x.Envelope)),
                    Envelope.FromEnvelopes(alongChosenAxis.GetRange(MinChildren, alongChosenAxis.Count - MinChildren).Select(x => x.Envelope)));
            // ChooseSplitIndex

            for (Int32 i = MinChildren + 1; i <= MaxChildren - MinChildren + 1; i++)
            {
                Double actOverlap = ComputeOverlap(Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, i).Select(x => x.Envelope)),
                    Envelope.FromEnvelopes(alongChosenAxis.GetRange(i, alongChosenAxis.Count - i).Select(x => x.Envelope)));
                if (minOverlap > actOverlap)
                {
                    minOverlap = actOverlap;
                    minIndex = i;
                }
                else if (minOverlap == actOverlap)
                {
                    Double minArea = Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, minIndex).Select(x => x.Envelope)).Surface +
                        Envelope.FromEnvelopes(alongChosenAxis.GetRange(minIndex, alongChosenAxis.Count - minIndex).Select(x => x.Envelope)).Surface;

                    Double actArea = Envelope.FromEnvelopes(alongChosenAxis.GetRange(0, i).Select(x => x.Envelope)).Surface +
                         Envelope.FromEnvelopes(alongChosenAxis.GetRange(i, alongChosenAxis.Count - i).Select(x => x.Envelope)).Surface;

                    if (minArea > actArea)
                        minIndex = i;
                }
            }

            firstNode = (overflownNode.Parent != null) ? new RTreeNode(overflownNode.Parent) : new RTreeNode(overflownNode.MaxChildren);
            secondNode = (overflownNode.Parent != null) ? new RTreeNode(overflownNode.Parent) : new RTreeNode(overflownNode.MaxChildren);

            foreach (RTreeNode node in alongChosenAxis.GetRange(0, minIndex))
                firstNode.AddChild(node);

            foreach (RTreeNode node in alongChosenAxis.GetRange(minIndex, alongChosenAxis.Count - minIndex))
                secondNode.AddChild(node);
        }

        /// <summary>
        /// Chooses a node where a new node should be added.
        /// </summary>
        /// <param name="envelope">The envelope of the new node.</param>
        /// <param name="height">The height where the new node should be added.</param>
        /// <returns>The node where the new node should be added.</returns>
        protected override RTreeNode ChooseNodeToAdd(Envelope envelope, Int32 height)
        {
            // source: The R*-tree: An Efficient and Robust Access Method for Points and Rectangles, page 4, method name in the paper: ChooseSubTree

            RTreeNode n = _root;
            int h = 0;
            while (!n.IsLeafContainer && h != height)
            {
                // if the child pointer points to leaf containers, determine the minimum overlap cost
                if (n.ChildrenCount > 0 && n.Children[0].IsLeafContainer)
                {
                    // determine the exact minimum overlap cost
                    n = ChooseNode(n.Children, envelope);

                    // another option is to determine the NEARLY minimum overlap cost
                }
                else // determine the minimum area cost
                {
                    Double minimumEnlargement = Double.MaxValue;
                    Double minimumSurface = Double.MaxValue;

                    foreach (RTreeNode child in n.Children)
                    {
                        Double enlargement = child.ComputeEnlargement(envelope);
                        if (minimumEnlargement > enlargement)
                        {
                            minimumEnlargement = enlargement;
                            n = child;
                        }
                        else if (minimumEnlargement == enlargement)
                        {
                            Double surface = child.Envelope.Surface;
                            if (minimumSurface > surface)
                            {
                                minimumSurface = surface;
                                n = child;
                            }
                        }
                    }
                }

                h++;
            }

            return n;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Inserts a node into the tree on a specified height.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="height">The height where the node should be inserted.</param>
        /// <param name="startHeight">If the tree was grown during the reinsert, then the original height of the tree.</param>
        private void InsertInSpecifiedHeight(RTreeNode node, Int32 height, Int32 startHeight)
        {
            RTreeNode nodeToInsert = ChooseNodeToAdd(node.Envelope, height);

            if (nodeToInsert == _root && nodeToInsert.ChildrenCount == 0)
                _height = 1;

            if (height == -1)
            {
                height = Height - 1;
                startHeight = height;
            }
                
            nodeToInsert.AddChild(node);

            if (nodeToInsert.IsOverflown)
            {
                Boolean canReInsert;
                Boolean splitted;

                do
                {
                    canReInsert = startHeight > 0 && !_visitedLevels[startHeight];
                    _visitedLevels[startHeight] = true;

                    splitted = OverflowTreatment(nodeToInsert, canReInsert, height)
                        && nodeToInsert.Parent.IsOverflown;

                    nodeToInsert = nodeToInsert.Parent;
                    height--;

                } while (splitted);
            }

            if (nodeToInsert != null)
                AdjustTree(nodeToInsert);

        }

        /// <summary>
        /// Reinserts or splits an overflown node.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="canReinsert">Indicates whether the elements of the node can reinserted.</param>
        /// <param name="height">The height of the overflown node.</param>
        /// <returns></returns>
        private Boolean OverflowTreatment(RTreeNode overflownNode, Boolean canReinsert, Int32 height)
        {
            if (canReinsert) // reinsert
            {
                ReInsert(overflownNode, height);
                return false;
            }
            else // split
            {
                RTreeNode first, second;
                SplitNode(overflownNode, out first, out second);

                if (overflownNode.Parent != null)
                {
                    overflownNode.Parent.RemoveChild(overflownNode);
                    overflownNode.Parent.AddChild(first);
                    overflownNode.Parent.AddChild(second);
                }
                else // case when the root is split
                {
                    _root = new RTreeNode(overflownNode.MaxChildren);
                    _root.AddChild(first);
                    _root.AddChild(second);
                    _height++;
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Reinserts children from the overflown node into the tree.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <param name="height">The height.</param>
        private void ReInsert(RTreeNode overflownNode, Int32 height)
        {
            // the number of elements we will reinsert
            // experiments shown that the 30% of the maximum children yields the best performance
            Int32 p = Convert.ToInt32(Math.Round(0.3 * MaxChildren)); 

            List<RTreeNode> nodesInOrder = overflownNode.Children.
                OrderByDescending(x => Coordinate.Distance(x.Envelope.Center, overflownNode.Envelope.Center)).Take(p).ToList();

            for (Int32 i = 0; i < p; i++)
                overflownNode.Children.Remove(nodesInOrder[i]);
            AdjustTree(overflownNode);

            Int32 startHeight = Height;

            for (Int32 i = 0; i < p; i++)
                InsertInSpecifiedHeight(nodesInOrder[i], height + (Height - startHeight), height);

        }

        /// <summary>
        /// Chooses the axis along which the split must be performed.
        /// </summary>
        /// <param name="overflownNode">The overflown node.</param>
        /// <returns>The nodes sorted by the axis along which the split must be performed</returns>
        private List<RTreeNode> ChooseSplitAxis(RTreeNode overflownNode)
        {
            Boolean isPlanar = overflownNode.Envelope.IsPlanar;

            Double sumX, sumY, sumZ;
            sumX = sumY = sumZ = 0;

            List<RTreeNode> orderedByX = overflownNode.Children.OrderBy(x => x.Envelope.MinX).ThenBy(x => x.Envelope.MaxX).ToList();
            List<RTreeNode> orderedByY = overflownNode.Children.OrderBy(x => x.Envelope.MinY).ThenBy(x => x.Envelope.MaxY).ToList();
            List<RTreeNode> orderedByZ = null;

            if (!isPlanar)
                orderedByZ = overflownNode.Children.OrderBy(x => x.Envelope.MinX).ThenBy(x => x.Envelope.MaxZ).ToList();

            for (int i = MinChildren; i <= MaxChildren - MinChildren + 1; i++)
            {
                sumX += ComputeMargin(Envelope.FromEnvelopes(orderedByX.GetRange(0, i).Select(x => x.Envelope))) +
                    ComputeMargin(Envelope.FromEnvelopes(orderedByX.GetRange(i, orderedByX.Count - i).Select(x => x.Envelope)));

                sumY += ComputeMargin(Envelope.FromEnvelopes(orderedByY.GetRange(0, i).Select(x => x.Envelope))) +
                    ComputeMargin(Envelope.FromEnvelopes(orderedByY.GetRange(i, orderedByY.Count - i).Select(x => x.Envelope)));

                if (!isPlanar)
                    sumZ += ComputeMargin(Envelope.FromEnvelopes(orderedByZ.GetRange(0, i).Select(x => x.Envelope))) +
                        ComputeMargin(Envelope.FromEnvelopes(orderedByZ.GetRange(i, orderedByZ.Count - i).Select(x => x.Envelope)));
            }

            if (!isPlanar && sumZ < sumX && sumZ < sumY)
                return orderedByZ;

            if (sumX <= sumY)
                return orderedByX;
            else
                return orderedByY;


        }

        /// <summary>
        /// Chooses which node should we enlarge with the new envelope to minimalize the overlap between the nodes
        /// </summary>
        /// <param name="entries">The nodes from we want to choose.</param>
        /// <param name="newEnvelope">The new envelope.</param>
        /// <returns></returns>
        private RTreeNode ChooseNode(List<RTreeNode> entries, Envelope newEnvelope)
        {
            RTreeNode minimalEntry = entries[0];
            Envelope enlargedEnvelope = Envelope.FromEnvelopes(minimalEntry.Envelope, newEnvelope);
            Double minimalOverlap = entries.Sum(x => x != entries[0] ? ComputeOverlap(enlargedEnvelope, x.Envelope) : 0);

            for (Int32 i = 1; i < entries.Count; i++)
            {
                enlargedEnvelope = Envelope.FromEnvelopes(entries[i].Envelope, newEnvelope);
                Double overlap = entries.Sum(x => x != entries[i] ? ComputeOverlap(enlargedEnvelope, x.Envelope) : 0);

                if (overlap < minimalOverlap)
                {
                    minimalOverlap = overlap;
                    minimalEntry = entries[i];
                }
                else if (overlap == minimalOverlap)
                {
                    if (entries[i].ComputeEnlargement(newEnvelope) < minimalEntry.ComputeEnlargement(newEnvelope))
                        minimalEntry = entries[i];
                }
            }
            return minimalEntry;
        }
        
        #endregion

        #region Private static methods

        /// <summary>
        /// Computes the margin of an envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        private static Double ComputeMargin(Envelope envelope)
        {
            return 2 * ((envelope.MaxX - envelope.MinX) + (envelope.MaxY - envelope.MinY) + (envelope.MaxZ - envelope.MinZ)); 
        }

        /// <summary>
        /// Computes the overlap of two envelopes.
        /// </summary>
        /// <param name="first">The first envelope.</param>
        /// <param name="second">The second envelope.</param>
        /// <returns>Overlap of the two envelopes</returns>
        private static Double ComputeOverlap(Envelope first, Envelope second)
        {
            Double minX, minY, minZ, maxX, maxY, maxZ;

            minX = Math.Max(first.MinX, second.MinX);
            minY = Math.Max(first.MinY, second.MinY);
            minZ = Math.Max(first.MinZ, second.MinZ);
            maxX = Math.Min(first.MaxX, second.MaxX);
            maxY = Math.Min(first.MaxY, second.MaxY);
            maxZ = Math.Min(first.MaxZ, second.MaxZ);

            if (minX > maxX || minY > maxY || minZ > maxZ)
                return 0;

            if (minZ == maxZ)
                return (maxX - minX) * (maxY - minY);
            else
                return (2 * (maxX - minX) * (maxY - minY)) + (2 * (maxX - minX) * (maxZ - minZ)) + (2 * (maxY - minY) * (maxZ - minZ));

        }

        #endregion
    }
}
