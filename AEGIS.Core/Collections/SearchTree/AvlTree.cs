/// <copyright file="AvlTree.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections;
using System.Collections.Generic;

namespace ELTE.AEGIS.Collections.SearchTree
{
    /// <summary>
    /// Represents an AVL tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class AvlTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
    {
        #region Protected types

        /// <summary>
        /// Represents a node of the AVL tree.
        /// </summary>
        protected class AvlNode : Node
        {
            #region Public fields

            /// <summary>
            /// The height of the subtree starting with the node.
            /// </summary>
            public Int32 Height;

            /// <summary>
            /// The balance of the node.
            /// </summary>
            public Int32 Balance;

            #endregion

            #region Public static methods

            /// <summary>
            /// Gets the height of a node.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns>The height of the node.</returns>
            public static Int32 GetHeight(Node node)
            {
                Int32 leftHeight = (node.LeftChild == null) ? -1 : (node.LeftChild as AvlNode).Height;
                Int32 rightHeight = (node.RightChild == null) ? -1 : (node.RightChild as AvlNode).Height;

                return Math.Max(rightHeight, leftHeight) + 1;
            }

            /// <summary>
            /// Gets the balance of a node.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns>The balance of the node.</returns>
            public static Int32 GetBalance(Node node) 
            {
                Int32 leftHeight = (node.LeftChild == null) ? -1 : (node.LeftChild as AvlNode).Height;
                Int32 rightHeight = (node.RightChild == null) ? -1 : (node.RightChild as AvlNode).Height;

                return rightHeight - leftHeight;
            }

            #endregion
        }

        #endregion

        #region ISearchTree properties

        /// <summary>
        /// Gets the height of the search tree.
        /// </summary>
        /// <value>The height of the search tree.</value>
        public override Int32 Height { get { return (_root as AvlNode).Height; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        public AvlTree()
        {
            _root = null;
            _nodeCount = 0;
            _version = 0;
            _comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvlTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        public AvlTree(IComparer<TKey> comparer)
        {
            _root = null;
            _nodeCount = 0;
            _version = 0;

            _comparer = comparer ?? Comparer<TKey>.Default;
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
                _root = new AvlNode { Key = key, Value = value, Height = 0, Balance = 0 };
                _nodeCount++;
                _version++;
                return;
            }

            AvlNode node = SearchNodeForInsertion(key) as AvlNode;
            if (node == null)
                throw new ArgumentException("An element with the same key already exists in the tree.", "key");

            if (_comparer.Compare(key, node.Key) < 0)
            {
                node.LeftChild = new AvlNode { Key = key, Value = value, Parent = node, Height = 0, Balance = 0 };
            }
            else
            {
                node.RightChild = new AvlNode { Key = key, Value = value, Parent = node, Height = 0, Balance = 0 };
            }

            Balance(node);
            _nodeCount++;
            _version++;
        }

        #endregion

        #region Protected BinarySearchTree methods

        /// <summary>
        /// Removes a node that has no children.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void RemoveNodeWithNoChild(BinarySearchTree<TKey, TValue>.Node node)
        {
            AvlNode parent = node.Parent as AvlNode;

            base.RemoveNodeWithNoChild(node);

            if (parent != null)
                Balance(parent);
        }

        /// <summary>
        /// Removes a node that has one child.
        /// </summary>
        /// <param name="node">The node.</param>
        protected override void RemoveNodeWithOneChild(BinarySearchTree<TKey, TValue>.Node node)
        {
            base.RemoveNodeWithOneChild(node);

            Balance(node as AvlNode);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Balances a subtree to comply with AVL property.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>0
        protected void Balance(AvlNode node)
        {
            while (node != null)
            {
                node.Balance = AvlNode.GetBalance(node);

                Int32 newHeight = AvlNode.GetHeight(node);
                if (newHeight == node.Height)
                    return;

                node.Height = newHeight;

                switch (node.Balance)
                {
                    case -2:
                        switch ((node.LeftChild as AvlNode).Balance)
                        {
                            case 0: // --,=
                            case -1: // --,-
                                node = RotateRight(node) as AvlNode;
                                (node.RightChild as AvlNode).Height = AvlNode.GetHeight(node.RightChild);
                                (node.RightChild as AvlNode).Balance = AvlNode.GetBalance(node.RightChild);
                                node.Height = AvlNode.GetHeight(node);
                                node.Balance = AvlNode.GetBalance(node);
                                break;
                            case 1: // --,+
                                RotateLeft(node.LeftChild);
                                node = RotateRight(node) as AvlNode;
                                (node.LeftChild as AvlNode).Height = AvlNode.GetHeight(node.LeftChild);
                                (node.LeftChild as AvlNode).Balance = AvlNode.GetBalance(node.LeftChild);
                                (node.RightChild as AvlNode).Height = AvlNode.GetHeight(node.RightChild);
                                (node.RightChild as AvlNode).Balance = AvlNode.GetBalance(node.RightChild);
                                node.Height = AvlNode.GetHeight(node);
                                node.Balance = AvlNode.GetBalance(node);
                                break;
                        }
                        break;
                    case 2:
                        switch ((node.RightChild as AvlNode).Balance)
                        {
                            case 0: // ++,=
                            case 1: // ++,+
                                node = RotateLeft(node) as AvlNode;
                                (node.LeftChild as AvlNode).Height = AvlNode.GetHeight(node.LeftChild);
                                (node.LeftChild as AvlNode).Balance = AvlNode.GetBalance(node.LeftChild);
                                node.Height = AvlNode.GetHeight(node);
                                node.Balance = AvlNode.GetBalance(node);
                                break;
                            case -1: // ++,-
                                RotateRight(node.RightChild);
                                node = RotateLeft(node) as AvlNode;
                                (node.LeftChild as AvlNode).Height = AvlNode.GetHeight(node.LeftChild);
                                (node.LeftChild as AvlNode).Balance = AvlNode.GetBalance(node.LeftChild);
                                (node.RightChild as AvlNode).Height = AvlNode.GetHeight(node.RightChild);
                                (node.RightChild as AvlNode).Balance = AvlNode.GetBalance(node.RightChild);
                                node.Height = AvlNode.GetHeight(node);
                                node.Balance = AvlNode.GetBalance(node);
                                break;
                        }
                        break;             
                }
                node = node.Parent as AvlNode;
            }
        }

        #endregion
    }
}
