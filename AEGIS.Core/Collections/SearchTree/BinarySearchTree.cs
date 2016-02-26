/// <copyright file="BinarySearchTree.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Roberto Giachetta</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections.SearchTree
{
    /// <summary>
    /// Represents a binary search tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class BinarySearchTree<TKey, TValue> : ISearchTree<TKey, TValue>
    {
        #region Public types

        /// <summary>
        /// Enumerates the elements of a search tree.
        /// </summary>
        /// <remarks>
        /// The enumerator performs an inorder traversal of the search tree thereby resulting in key/values pairs ordered according to the specified comparator of the search tree.
        /// </remarks>
        [Serializable]
        public sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            #region Private fields

            private BinarySearchTree<TKey, TValue> _localTree;
            private Int32 _localVersion;
            private Stack<Node> _stack;
            private Node _traversible;
            private KeyValuePair<TKey, TValue> _current;
            private Boolean _disposed;

            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public KeyValuePair<TKey, TValue> Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.-
            /// </value>
            Object IEnumerator.Current
            {
                get { return _current; }
            }

            #endregion

            #region Constructor and destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="BinarySearchTree{TKey, TValue}.Enumerator" /> class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            internal Enumerator(BinarySearchTree<TKey, TValue> tree)
            {
                _localTree = tree;
                _localVersion = tree._version;

                _stack = new Stack<Node>();
                _traversible = tree._root;
                _current = default(KeyValuePair<TKey, TValue>);
                _disposed = false;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="Enumerator"/> class.
            /// </summary>
            ~Enumerator()
            {
                Dispose(false);
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                while (_traversible != null)
                {
                    _stack.Push(_traversible);
                    _traversible = _traversible.LeftChild;
                }

                if (_stack.Count == 0)
                {
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                _traversible = _stack.Pop();
                _current = new KeyValuePair<TKey, TValue>(_traversible.Key, _traversible.Value);
                _traversible = _traversible.RightChild;
                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _stack.Clear();
                _traversible = _localTree._root;
                _current = default(KeyValuePair<TKey, TValue>);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() 
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            private void Dispose(Boolean disposing)
            {
                _disposed = true;

                if (disposing)
                {
                    _localTree = null;
                    _stack.Clear();
                    _stack = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                }
            }

            #endregion
        }

        /// <summary>
        /// Enumerates the elements of a search tree in multiple directions.
        /// </summary>
        [Serializable]
        public sealed class SearchTreeEnumerator : ISearchTreeEnumerator<TKey, TValue>
        {
            #region Private fields

            private BinarySearchTree<TKey, TValue> _localTree;
            private Int32 _localVersion;
            private KeyValuePair<TKey, TValue> _current;
            private Node _currentNode;
            private Boolean _disposed;

            #endregion

            #region IEnumerator properties

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            public KeyValuePair<TKey, TValue> Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            /// <value>
            /// The element at the current position of the enumerator.
            /// </value>
            object IEnumerator.Current
            {
                get { return _current; }
            }

            #endregion

            #region Constructors and destructor

            /// <summary>
            /// Initializes a new instance of the <see cref="BinarySearchTree{TKey, TValue}.SearchTreeEnumerator" /> class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            internal SearchTreeEnumerator(BinarySearchTree<TKey, TValue> tree)
            {
                _localTree = tree;
                _localVersion = tree._version;
                _current = default(KeyValuePair<TKey, TValue>);
                _currentNode = null;
                _disposed = false;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="Enumerator"/> class.
            /// </summary>
            ~SearchTreeEnumerator()
            {
                Dispose(false);
            }

            #endregion

            #region ISearchTreeEnumerator methods

            /// <summary>
            /// Advances the enumerator to the previous element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MovePrev()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_currentNode == null)
                    return false;

                if (_currentNode.LeftChild != null)
                {
                    _currentNode = _currentNode.LeftChild;

                    while (_currentNode.RightChild != null)
                    {
                        _currentNode = _currentNode.RightChild;
                    }

                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                    return true;
                }

                while (_currentNode.Parent != null && _currentNode.Parent.LeftChild == _currentNode)
                {
                    _currentNode = _currentNode.Parent;
                }

                if (_currentNode.Parent != null && _currentNode.Parent.RightChild == _currentNode)
                {
                    _currentNode = _currentNode.Parent;
                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                    return true;
                }
                else
                {
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
            }

            /// <summary>
            /// Advances the enumerator to the minimal element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the minimal element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveMin()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_localTree._root == null)
                {
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                _currentNode = _localTree._root;

                while (_currentNode.LeftChild != null)
                {
                    _currentNode = _currentNode.LeftChild;
                }
                _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                return true;
            }

            /// <summary>
            /// Advances the enumerator to the maximal element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the maximal element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveMax()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_localTree._root == null)
                {
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                _currentNode = _localTree._root;

                while (_currentNode.RightChild != null)
                {
                    _currentNode = _currentNode.RightChild;
                }
                _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                return true;
            }

            /// <summary>
            /// Advances the enumerator to the root element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the root element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveRoot()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_localTree._root == null)
                {
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }

                _currentNode = _localTree._root;
                _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                return true;
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_currentNode == null)
                    return false;

                if (_currentNode.RightChild != null)
                {
                    _currentNode = _currentNode.RightChild;

                    while (_currentNode.LeftChild != null)
                    {
                        _currentNode = _currentNode.LeftChild;
                    }

                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                    return true;
                }

                while (_currentNode.Parent != null && _currentNode.Parent.RightChild == _currentNode)
                {
                    _currentNode = _currentNode.Parent;
                }

                if (_currentNode.Parent != null && _currentNode.Parent.LeftChild == _currentNode)
                {
                    _currentNode = _currentNode.Parent;
                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Key, _currentNode.Value);
                    return true;
                }
                else
                {
                    _current = default(KeyValuePair<TKey, TValue>);
                    return false;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.ObjectDisposedException">The object is disposed.</exception>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _currentNode = null;
                _current = default(KeyValuePair<TKey, TValue>);
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_disposed)
                    return;

                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Private methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
            private void Dispose(Boolean disposing)
            {
                _disposed = true;

                if (disposing)
                {
                    _localTree = null;
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                }
            }

            #endregion
        }

        #endregion

        #region Protected types

        /// <summary>
        /// Represents a node of the search tree.
        /// </summary>
        [Serializable]
        protected class Node
        {
            #region Public fields

            /// <summary>
            /// The key of the node.
            /// </summary>
            public TKey Key;

            /// <summary>
            /// The value of the node.
            /// </summary>
            public TValue Value;

            /// <summary>
            /// The parent node.
            /// </summary>
            public Node Parent;

            /// <summary>
            /// The left child node.
            /// </summary>
            public Node LeftChild;

            /// <summary>
            /// The right child node.
            /// </summary>
            public Node RightChild;

            #endregion
        }

        #endregion

        #region Protected fields

        protected Node _root;
        protected Int32 _nodeCount;
        protected Int32 _version;
        protected IComparer<TKey> _comparer;

        #endregion

        #region ISearchTree properties

        /// <summary>
        /// Gets the number of elements actually contained in the search tree.
        /// </summary>
        /// <value>The number of elements actually contained in the search tree.</value>
        public Int32 Count { get { return _nodeCount; } }
        /// <summary>
        /// Gets the height of the search tree.
        /// </summary>
        /// <value>The height of the search tree.</value>
        public virtual Int32 Height { get { return GetTreeHeight(_root); } }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the <see cref="IComparer{T}" /> that is used to determine order of keys for the tree. 
        /// </summary>
        /// <value>The <see cref="IComparer{T}" /> generic interface implementation that is used to determine order of keys for the current search tree and to provide hash values for the keys.</value>
        public IComparer<TKey> Comparer { get { return _comparer; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the search tree class.
        /// </summary>
        public BinarySearchTree()
        {
            _root = null;
            _nodeCount = 0;
            _version = 0;
            _comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the search tree class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        public BinarySearchTree(IComparer<TKey> comparer)
        {
            _root = null;
            _nodeCount = 0;
            _version = 0;

            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        #endregion

        #region ISearchTree methods

        /// <summary>
        /// Searches the search tree for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value of the element with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        /// <exception cref="System.ArgumentException">The tree does not contain the specified key.</exception>
        public TValue Search(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            Node node = SearchNode(key);
            if (node == null)
                throw new ArgumentException("The tree does not contain the specified key.", "key");

            return node.Value;
        }

        /// <summary>
        /// Searches the <see cref="ISearchTree{TKey, TValue}" /> for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value of the element with the specified key.</param>
        /// <returns><c>true</c> if the <see cref="ISearchTree{TKey, TValue}" /> contains the element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        public Boolean TrySearch(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            Node node = SearchNode(key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }

            value = node.Value;
            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="ISearchTree{TKey, TValue}" /> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="ISearchTree{TKey, TValue}" />.</param>
        /// <returns><c>true</c> if the <see cref="ISearchTree{TKey, TValue}" /> contains the element with the specified key; otherwise, <c>false</c>.</returns>
        public Boolean Contains(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            return (SearchNode(key) != null);
        }

        /// <summary>
        /// Inserts the specified key/value pair to the tree.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert. The value can be <c>null</c> for reference types.</param>
        /// <exception cref="System.ArgumentNullException">The key is null.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the tree.</exception>
        public virtual void Insert(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            if (_root == null)
            {
                _root = new Node { Key = key, Value = value };
                _nodeCount++;
                _version++;
                return;
            }

            Node node = SearchNodeForInsertion(key);
            if (node == null)
                throw new ArgumentException("An element with the same key already exists in the tree.", "key");

            if (_comparer.Compare(key, node.Key) < 0)
                node.LeftChild = new Node { Key = key, Value = value, Parent = node };
            else
                node.RightChild = new Node { Key = key, Value = value, Parent = node };

            _nodeCount++;
            _version++;
        }

        /// <summary>
        /// Removes an element with the specified key from the search tree.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><c>true</c> if the search tree contains the element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        public virtual Boolean Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            Node node = SearchNode(key);
            if (node == null)
                return false;

            RemoveNode(node);

            _nodeCount--;
            _version++;
            return true;
        }

        /// <summary>
        /// Removes all elements from the search tree.
        /// </summary>
        public virtual void Clear()
        {
            _root = null;
            _nodeCount = 0;
            _version++;
        }

        /// <summary>
        /// Returns a search tree enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="ISearchTreeEnumerator{TKey, TValue}" /> object that can be used to iterate through the collection.</returns>
        public ISearchTreeEnumerator<TKey, TValue> GetTreeEnumerator()
        {
            return new SearchTreeEnumerator(this);
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Searches the tree for an element with a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The element with the key or <c>null</c> if the key is not within the tree.</returns>
        protected Node SearchNode(TKey key)
        {
            Node node = _root;
            Int32 compare;

            while (node != null)
            {
                compare = _comparer.Compare(key, node.Key);

                if (compare == 0)
                    break;
                node = compare < 0 ? node.LeftChild : node.RightChild;
            }
            return node;
        }

        /// <summary>
        ///  Searches the tree for an element under witch a new element with a specific key can be inserted.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <returns>The element or <c>null</c> if an element with the same key already exists in the tree.</returns>
        protected Node SearchNodeForInsertion(TKey key)
        {
            Node parent = null;
            Node node = _root;
            Int32 compare;

            while (node != null)
            {
                parent = node;
                compare = _comparer.Compare(key, node.Key);

                if (compare == 0)
                    return null;

                node = compare < 0 ? node.LeftChild : node.RightChild;
            }
            return parent;
        }

        /// <summary>
        /// Removes an element from the tree.
        /// </summary>
        /// <param name="node">The node.</param>
        protected void RemoveNode(Node node)
        {
            // case of no children the node should by simply detached
            if (node.LeftChild == null && node.RightChild == null)
            {
                RemoveNodeWithNoChild(node);
                return;
            }

            // case of one child 
            if (node.LeftChild == null || node.RightChild == null)
            {
                RemoveNodeWithOneChild(node);
                return;
            }

            // case of two children
            Node successor = node.RightChild;
            while (successor.LeftChild != null)
                successor = successor.LeftChild;

            node.Key = successor.Key;
            node.Value = successor.Value;

            // check the right child of the successor (left child is null after the loop)
            if (successor.RightChild == null)
            {
                RemoveNodeWithNoChild(successor);
            }
            else
            {
                RemoveNodeWithOneChild(successor);
            }
        }

        /// <summary>
        /// Removes a node that has no children.
        /// </summary>
        /// <param name="node">The node.</param>
        protected virtual void RemoveNodeWithNoChild(Node node)
        {
            if (node == _root)
            {
                _root = null;
                return;
            }

            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = null;
            else
                node.Parent.RightChild = null;

            node.Parent = null;
        }

        /// <summary>
        /// Removes a node that has one child.
        /// </summary>
        /// <param name="node">The node.</param>
        protected virtual void RemoveNodeWithOneChild(Node node) 
        {
            Node successor = node.LeftChild ?? node.RightChild;

            node.Key = successor.Key;
            node.Value = successor.Value;
            node.LeftChild = successor.LeftChild;
            node.RightChild = successor.RightChild;

            if (node.LeftChild != null)
                node.LeftChild.Parent = node;
            if (node.RightChild != null)
                node.RightChild.Parent = node;

            successor.LeftChild = successor.RightChild = successor.Parent = null;
        }

        /// <summary>
        /// Rotates a subtree to the left.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        /// <returns>The root node of the rotated subtree.</returns>
        protected virtual Node RotateLeft(Node node)
        {
            Node rightChild = node.RightChild;
            Node rightLeftChild = rightChild.LeftChild;
            Node parentNode = node.Parent;

            rightChild.Parent = parentNode;
            rightChild.LeftChild = node;
            node.RightChild = rightLeftChild;
            node.Parent = rightChild;

            if (rightLeftChild != null)
            {
                rightLeftChild.Parent = node;
            }

            if (node == _root)
            {
                _root = rightChild;
            }
            else if (parentNode.RightChild == node)
            {
                parentNode.RightChild = rightChild;
            }
            else
            {
                parentNode.LeftChild = rightChild;
            }

            return rightChild;
        }

        /// <summary>
        /// Rotates a subtree to the right.
        /// </summary>
        /// <param name="node">The root node of the subtree.</param>
        /// <returns>The root node of the rotated subtree.</returns>
        protected virtual Node RotateRight(Node node)
        {
            Node leftChild = node.LeftChild;
            Node leftRightChild = leftChild.RightChild;
            Node parentNode = node.Parent;

            leftChild.Parent = parentNode;
            leftChild.RightChild = node;
            node.LeftChild = leftRightChild;
            node.Parent = leftChild;

            if (leftRightChild != null)
            {
                leftRightChild.Parent = node;
            }

            if (node == _root)
            {
                _root = leftChild;
            }
            else if (parentNode.LeftChild == node)
            {
                parentNode.LeftChild = leftChild;
            }
            else
            {
                parentNode.RightChild = leftChild;
            }

            return leftChild;
        }

        #endregion

        #region Protected static methods

        /// <summary>
        /// Gets the height of the tree.
        /// </summary>
        /// <param name="node">The starting node.</param>
        /// <returns>The height of the tree.</returns>
        protected static Int32 GetTreeHeight(Node node)
        {
            if (node == null)
                return -1;

            Int32 leftHeight = GetTreeHeight(node.LeftChild) + 1;
            Int32 rightHeight = GetTreeHeight(node.RightChild) + 1;

            if (leftHeight >= rightHeight)
                return leftHeight;
            else
                return rightHeight;
        }

        /// <summary>
        /// Gets the sibling of a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The sibling of <paramref name="node" />.</returns>
        protected static Node GetSiblingNode(Node node)
        {
            if (node == null || node.Parent == null)
                return null;

            if (node.Parent.LeftChild != null && node.Parent.LeftChild == node)
                return node.Parent.RightChild;

            return node.Parent.LeftChild;
        }

        #endregion
    }
}
