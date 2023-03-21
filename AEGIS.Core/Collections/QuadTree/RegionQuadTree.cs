// <copyright file="RegionQuadTree.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Collections.QuadTree
{
    /// <summary>
    /// Represents a region quadtree.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    /// <remarks>
    /// The region quadtree represents a partition of space in two dimensions by decomposing the region into four equal quadrants,
    /// subquadrants, and so on with each leaf node containing data corresponding to a specific subregion.
    /// Each node in the tree either has exactly four children, or has no children (a leaf node).
    /// </remarks>
    public class RegionQuadTree<T>
    {
        #region Private fields

        private Int32 _treeHeight;
        private RegionQuadTree<T> _parent;
        private RegionQuadTree<T>[] _childs = null;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the region's left upper X coordinate.
        /// </summary>
        public Double X { get; private set; }

        /// <summary>
        /// Gets the region's left upper Y coordinate.
        /// </summary>
        public Double Y { get; private set; }

        /// <summary>
        /// Gets the width of the region.
        /// </summary>
        public Double Width { get; private set; }

        /// <summary>
        /// Gets the height of the region.
        /// </summary>
        public Double Height { get; private set; }

        /// <summary>
        /// Gets the element within the region.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets the level of the node.
        /// </summary>
        public Int32 Level { get; private set; }

        /// <summary>
        /// Gets the height of the tree.
        /// </summary>
        public Int32 TreeHeight { get { return _treeHeight; } private set { _treeHeight = value; if (_parent != null) _parent.TreeHeight += 1; } }

        /// <summary>
        /// Gets a value indicating whether the node can be divided.
        /// </summary>
        public Boolean CanDivide { get { return IsLeaf && Width > 1 && Height > 1; } }
        
        /// <summary>
        /// Gets a value indicating whether the node is a leaf.
        /// </summary>
        public Boolean IsLeaf { get { return _childs == null; } }
        
        /// <summary>
        /// Returns the children of the node.
        /// </summary>
        public RegionQuadTree<T>[] Childs { get { return _childs; } }
        
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        public RegionQuadTree<T> Parent { get { return _parent; } set { _parent = value; Level = value.Level + 1; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionQuadTree{T}" /> class.
        /// </summary>
        /// <param name="x">The X coordinate of the upper left corner.</param>
        /// <param name="y">The Y coordinate of the upper left corner.</param>
        /// <param name="width">The width of the region.</param>
        /// <param name="height">The height of the region.</param>
        public RegionQuadTree(Double x, Double y, Double width, Double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Level = 0;
            TreeHeight = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Divides the node into four child nodes.
        /// </summary>
        public void Subdivide()
        {
            if (CanDivide)
            {
                _childs = new RegionQuadTree<T>[4];
                Double childWidth = Width / 2;
                Double childHeight = Height / 2;

                _childs[2] = new RegionQuadTree<T>(X, Y, childWidth, childHeight);
                _childs[2].Parent = this;
                _childs[3] = new RegionQuadTree<T>(X + childWidth, Y, childWidth, childHeight);
                _childs[3].Parent = this;
                _childs[0] = new RegionQuadTree<T>(X, Y + childHeight, childWidth, childHeight);
                _childs[0].Parent = this;
                _childs[1] = new RegionQuadTree<T>(X + childWidth, Y + childHeight, childWidth, childHeight);
                _childs[1].Parent = this;

                this.TreeHeight += 1;
            }
        }
        /// <summary>
        /// Gets the leaf nodes of the tree.
        /// </summary>
        /// <returns>The list of leaf nodes.</returns>
        public List<RegionQuadTree<T>> GetLeafs()
        {
            List<RegionQuadTree<T>> list = new List<RegionQuadTree<T>>();
            if (IsLeaf)
            {
                list.Add(this);
            }
            else
            {
                for (Int32 i = 0; i < 4; i++)
                {
                    list.AddRange(_childs[i].GetLeafs());
                }
            }
            return list;
        }

        #endregion
    }
}
