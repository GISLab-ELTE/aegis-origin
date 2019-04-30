/// <copyright file="RedBlackTree.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections.SearchTree
{
    /// <summary>
    /// Represents a red-black tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class RedBlackTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
    {
        #region Protected types

        /// <summary>
        /// Defines the node colors.
        /// </summary>
        protected enum NodeColor
        {
            Red, Black
        }

        /// <summary>
        /// Represents a node of the red-black tree.
        /// </summary>
        [Serializable]
        protected class RedBlackNode : Node
        {
            #region Public fields

            /// <summary>
            /// The color of the node.
            /// </summary>
            public NodeColor Color;

            #endregion
        }
       
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        public RedBlackTree() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedBlackTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        public RedBlackTree(IComparer<TKey> comparer) : base(comparer)
        {
        }

        #endregion

        #region ISearchTree methods

        /// <summary>
        /// Inserts the specified key/value pair to the tree.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the tree.</exception>
        public override void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            if (_root == null)
            {
                _root = new RedBlackNode { Key = key, Value = value, Color = NodeColor.Black };
                _nodeCount++;
                _version++;
                return;
            }

            RedBlackNode node = SearchNodeForInsertion(key) as RedBlackNode;
            if (node == null)
                throw new ArgumentException("An element with the same key already exists in the tree.", "key");

            if (_comparer.Compare(key, node.Key) < 0)
                node.LeftChild = new RedBlackNode { Key = key, Value = value, Parent = node, Color = NodeColor.Red };
            else
                node.RightChild = new RedBlackNode { Key = key, Value = value, Parent = node, Color = NodeColor.Red };

            BalanceInsert(node);
            _nodeCount++;
            _version++;
        }

        #endregion

        #region Protected SearchTree methods

        /// <summary>
        /// Removes a node that has no children.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void RemoveNodeWithNoChild(Node node)
        {
            if (node == _root)
            {
                _root = null;
            }

            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = null;
            else
                node.Parent.RightChild = null;

            BalanceRemove(node as RedBlackNode);

            node.Parent = null;
        }

        /// <summary>
        /// Removes a node that has one child.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void RemoveNodeWithOneChild(Node node)
        {
            Node successor = (node.LeftChild != null) ? node.LeftChild : node.RightChild;
            // the successor cannot have a child since 

            node.Key = successor.Key;
            node.Value = successor.Value;
            node.LeftChild = node.RightChild = null;

            if ((successor as RedBlackNode).Color == NodeColor.Black)
                BalanceRemove(successor as RedBlackNode);

            successor.Parent = null;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Balances the tree after insertion.
        /// </summary>
        /// <param name="node">The node.</param>
        protected void BalanceInsert(RedBlackNode node)
        {
            // source: http://en.wikipedia.org/wiki/Red%E2%80%93black_tree

            RedBlackNode uncle;

            // case 1: root inserted
            if (node.Parent == null)
            {
                node.Color = NodeColor.Black;
                return;
            }

            // case 2: parent is black
            if ((node.Parent as RedBlackNode).Color == NodeColor.Black)
            {
                return;
                // no correction needed
            }

            // case 3: parent and uncle are both red
            if ((node.Parent as RedBlackNode).Color == NodeColor.Red && (uncle = GetSiblingNode(node.Parent) as RedBlackNode) != null && uncle.Color == NodeColor.Red)
            {
                (node.Parent as RedBlackNode).Color = NodeColor.Black;
                uncle.Color = NodeColor.Black;
                (node.Parent.Parent as RedBlackNode).Color = NodeColor.Red;

                BalanceInsert(node.Parent.Parent as RedBlackNode);
                return;
            }

            if ((node.Parent as RedBlackNode).Color == NodeColor.Red && ((uncle = GetSiblingNode(node.Parent) as RedBlackNode) == null || uncle.Color == NodeColor.Black))
            {
                // case 4: parent is red, uncle is null or black, node is a right child, parent if left child
                if (node == node.Parent.RightChild && node.Parent == node.Parent.Parent.LeftChild)
                    if (node == node.Parent.LeftChild)
                        RotateLeft(node);
                    else
                        RotateRight(node);

                // case 5: parent is red, uncle is null or black, node is a left child, parent is left child
                if (node == node.Parent.LeftChild)
                    RotateRight(node.Parent.Parent);
                else
                    RotateLeft(node.Parent.Parent);
                (node.Parent as RedBlackNode).Color = NodeColor.Black;
                (node.Parent.RightChild as RedBlackNode).Color = NodeColor.Red;
            }
        }

        /// <summary>
        /// Balances the tree after removal.
        /// </summary>
        /// <param name="node">The node.</param>
        protected void BalanceRemove(RedBlackNode node)
        {
            // source: http://en.wikipedia.org/wiki/Red%E2%80%93black_tree

            if (node == null || node.Parent == null) // case 1: the node is the root
                return;

            RedBlackNode parent = node.Parent as RedBlackNode;
            RedBlackNode sibling = GetSiblingNode(node) as RedBlackNode;

            // case 2: the sibling is red
            if (sibling != null && sibling.Color == NodeColor.Red) 
            {
                parent.Color = NodeColor.Red;
                sibling.Color = NodeColor.Black;
                if (node == node.Parent.LeftChild)
                    RotateLeft(node.Parent);
                else
                    RotateRight(node.Parent);
                return;
            }

            // case 3: the sibling, the parent and the children of the sibling are black 
            if (parent.Color == NodeColor.Black && sibling == null || 
                ((sibling.LeftChild == null || (sibling.LeftChild as RedBlackNode).Color == NodeColor.Black) &&
                 (sibling.RightChild == null || (sibling.RightChild as RedBlackNode).Color == NodeColor.Black)))
            {
                if (sibling != null)
                    sibling.Color = NodeColor.Red;

                BalanceRemove(parent);
                return;
            }

            // case 4: the parent is black, the sibling and the children of the sibling are black 
            if (parent.Color == NodeColor.Red && sibling == null ||
                ((sibling.LeftChild == null || (sibling.LeftChild as RedBlackNode).Color == NodeColor.Black) &&
                 (sibling.RightChild == null || (sibling.RightChild as RedBlackNode).Color == NodeColor.Black)))
            {
                if (sibling != null)
                    sibling.Color = NodeColor.Red;
                parent.Color = NodeColor.Black;
                return;
            }

            // case 5: the sibling and the right child of the sibling are black, the left child of the sibling is red (transitions into case 6)
            if (node == node.Parent.LeftChild && sibling == null ||
                ((sibling.LeftChild == null || (sibling.LeftChild as RedBlackNode).Color == NodeColor.Red) &&
                 (sibling.RightChild == null || (sibling.RightChild as RedBlackNode).Color == NodeColor.Black)))
            {
                if (sibling != null)
                {
                    sibling.Color = NodeColor.Red;
                    (sibling.LeftChild as RedBlackNode).Color = NodeColor.Black;

                    RotateRight(sibling);
                }
            }
            if (node == node.Parent.LeftChild && sibling == null ||
                ((sibling.LeftChild == null || (sibling.LeftChild as RedBlackNode).Color == NodeColor.Black) &&
                 (sibling.RightChild == null || (sibling.RightChild as RedBlackNode).Color == NodeColor.Red)))
            {
                if (sibling != null)
                {
                    sibling.Color = NodeColor.Red;

                    (sibling.RightChild as RedBlackNode).Color = NodeColor.Black;

                    RotateLeft(sibling);
                }
            }

            // case 6: the sibling is black, the right child of the sibling is red
            if (sibling != null)
            {
                sibling.Color = parent.Color;

                if (node == node.Parent.LeftChild)
                {
                    if (sibling.RightChild != null)
                        (sibling.RightChild as RedBlackNode).Color = NodeColor.Black;
                    RotateLeft(parent);
                }
                else
                {
                    if (sibling.LeftChild != null)
                        (sibling.LeftChild as RedBlackNode).Color = NodeColor.Black;
                    RotateRight(parent);
                }
            }
            parent.Color = NodeColor.Black;
        }

        #endregion
    }
}
