/// <copyright file="BTree.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Gréta Bereczki</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Collections.SearchTree
{
    /// <summary>
    /// Represents a B-tree.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class BTree<TKey, TValue> : ISearchTree<TKey, TValue>
    {
        #region Public types

        /// <summary>
        /// Enumerates the elements of a <see cref="BTree{TKey, TValue}" />.
        /// </summary>
        /// <remarks>
        /// The enumerator performs a level order traversal of the B-tree.
        /// </remarks>
        [Serializable]
        public sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            #region Private fields

            private BTree<TKey, TValue> _localTree;
            private Int32 _localVersion;
            private Queue<Node> _queue;
            private Node _currentNode;
            private KeyValuePair<TKey, TValue> _current;
            private Int32 _currentIndex;

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

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="BTree{TKey, TValue}.Enumerator" /> class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            internal Enumerator(BTree<TKey, TValue> tree)
            {
                _localTree = tree;
                _localVersion = tree._version;

                _currentNode = tree._root;
                _current = default(KeyValuePair<TKey, TValue>);
                _currentIndex = 0;
                _queue = new Queue<Node>();
            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_currentNode == null)
                    return false;

                if (_currentIndex < _currentNode.CurrentKeyCount && _queue.Count != 0)
                {
                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentIndex], _currentNode.Values[_currentIndex]);
                    _currentIndex++;
                }
                else if (_currentIndex < _currentNode.CurrentKeyCount && _queue.Count == 0)
                {
                    return false;
                }
                else if (_currentIndex == _currentNode.CurrentKeyCount)
                {
                    _currentIndex = 0;
                    for (Int32 i = 0; i < _localTree._maximumChildCount; i++)
                    {
                        if (_currentNode.Children[i] != null)
                            _queue.Enqueue(_currentNode.Children[i]);
                    }
                    _queue.Dequeue();
                    if (_queue.Count != 0)
                    {
                        _currentNode = _queue.First();
                        _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentIndex], _currentNode.Values[_currentIndex]);
                        _currentIndex++;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _queue.Clear();
                _currentNode = _localTree._root;
                _current = default(KeyValuePair<TKey, TValue>);
                _currentIndex = 0;
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() { }

            #endregion
        }

        /// <summary>
        /// Enumerates the elements of a <see cref="BTree{TKey, TValue}" /> in multiple directions.
        /// </summary>
        [Serializable]
        public sealed class SearchTreeEnumerator : ISearchTreeEnumerator<TKey, TValue>
        {
            #region Private fields

            private BTree<TKey, TValue> _localTree;
            private Int32 _localVersion;
            private Node _currentNode;
            private KeyValuePair<TKey, TValue> _current;
            private Int32 _currentIndex;
            private List<Node> _list;
            private Int32 _currentNodeIndex;

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

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="BTree{TKey, TValue}.SearchTreeEnumerator" /> class.
            /// </summary>
            /// <param name="tree">The tree.</param>
            internal SearchTreeEnumerator(BTree<TKey, TValue> tree)
            {
                _localTree = tree;
                _localVersion = tree._version;

                _currentNode = tree._root;
                _current = default(KeyValuePair<TKey, TValue>);
                _currentIndex = -1;
                _list = new List<Node>();
                _currentNodeIndex = 0;
                _list.Add(tree._root);
            }

            #endregion

            #region BTreeEnumerator methods

            /// <summary>
            /// Advances the enumerator to the previous element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MovePrev()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_currentNode == null)
                    return false;

                if (_currentIndex > 0 && _currentNodeIndex >= 0 && _list.Count != 0)
                {
                    _currentIndex--;
                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentIndex], _currentNode.Values[_currentIndex]);
                }
                else if (_currentIndex == 0 && _currentNodeIndex <= 0)
                {
                    return false;
                }
                else if (_currentIndex == 0)
                {
                    _currentNodeIndex--;
                    _currentIndex = _currentNode.CurrentKeyCount;
                    if (_currentNodeIndex >= 0 && _list.Count != 0)
                    {

                        _currentNode = _list[_currentNodeIndex];
                        _currentIndex = _currentNode.CurrentKeyCount;
                        _currentIndex--;
                        _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentIndex], _currentNode.Values[_currentIndex]);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// Advances the enumerator to the minimal element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the minimal element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveMin()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_localTree._root == null)
                {
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                    _currentIndex = 0;
                    return false;
                }

                _currentNode = _localTree._root;

                while (_currentNode.Children[0] != null)
                {
                    _currentNode = _currentNode.Children[0];
                }

                _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[0], _currentNode.Values[0]);

                return true;
            }
            /// <summary>
            /// Advances the enumerator to the maximal element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the maximal element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveMax()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_localTree._root == null)
                {
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                    _currentIndex = 0;
                    return false;
                }

                _currentNode = _localTree._root;

                Int32 i = 0;
                while (i < _localTree._maximumChildCount - 1 && _currentNode.Children[i] != null)
                {
                    i++;
                }

                while (_currentNode.Children[i] != null)
                {
                    _currentNode = _currentNode.Children[i];

                    i = 0;
                    while (i < _localTree._maximumChildCount - 1 && _currentNode.Children[i] != null)
                    {
                        i++;
                    }
                }

                _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentNode.CurrentKeyCount - 1], _currentNode.Values[_currentNode.CurrentKeyCount - 1]);

                return true;
            }

            /// <summary>
            /// Advances the enumerator to the root element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the root element; <c>false</c> if the collection is empty.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveRoot()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_localTree._root == null)
                {
                    _currentNode = null;
                    _current = default(KeyValuePair<TKey, TValue>);
                    _currentIndex = 0;
                    return false;
                }

                _currentNode = _localTree._root;
                _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[0], _currentNode.Values[0]); 
                return true;

            }

            #endregion

            #region IEnumerator methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public Boolean MoveNext()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                if (_currentNode == null)
                    return false;

                if (_currentIndex < _currentNode.CurrentKeyCount - 1 && _list.Count != 0)
                {
                    _currentIndex++;
                    _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentIndex], _currentNode.Values[_currentIndex]);
                }
                else if (_currentIndex < _currentNode.CurrentKeyCount - 1 && _list.Count == 0)
                {
                    return false;
                }
                else if (_currentIndex == _currentNode.CurrentKeyCount - 1)
                {
                    _currentIndex = -1;

                    for (Int32 i = 0; i < _currentNode.CurrentKeyCount + 1; i++)
                    {
                        if (_currentNode.Children[i] != null)
                        {
                            Node n = _list.Find(ix => ix == _currentNode.Children[i]);
                            if (n == null)
                                _list.Add(_currentNode.Children[i]);
                        }
                    }

                    if (_list.Count != 0)
                    {
                        _currentNodeIndex++;
                        if (_list.Count != 0)
                        {
                            _currentIndex++;
                            _currentNode = _list[_currentNodeIndex];
                            _current = new KeyValuePair<TKey, TValue>(_currentNode.Keys[_currentIndex], _currentNode.Values[_currentIndex]);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
            public void Reset()
            {
                if (_localVersion != _localTree._version)
                    throw new InvalidOperationException("The collection was modified after the enumerator was created.");

                _currentNode = null;
                _current = default(KeyValuePair<TKey, TValue>);
                _currentIndex = -1;
                _list.Clear();
                _currentNodeIndex=0;
            }

            #endregion

            #region IDisposable methods

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() { }

            #endregion
        }

        #endregion

        #region Private types

        /// <summary>
        /// Represents a node in the B-tree.
        /// </summary>
        private class Node
        {
            #region Public fields

            /// <summary>
            /// The keys of the node.
            /// </summary>
            public TKey[] Keys;

            /// <summary>
            /// The values of the node.
            /// </summary>
            public TValue[] Values;

            /// <summary>
            /// The parent node.
            /// </summary>
            public Node Parent;

            /// <summary>
            /// The children of the node.
            /// </summary>
            public Node[] Children;

            /// <summary>
            /// The current number of keys in a node.
            /// </summary>
            public Int32 CurrentKeyCount;

            /// <summary>
            /// Indicates whether the node is a leaf.
            /// </summary>
            public Boolean IsLeaf;

            #endregion
        }

        #endregion

        #region Private fields

        private Node _root;
        private Int32 _version;
        private Int32 _height;
        private Int32 _nodeCount;
        private Int32 _minimizationFactor;
        private Int32 _minimumKeyCount;
        private Int32 _minimumChildCount;
        private Int32 _maximumKeyCount; 
        private Int32 _maximumChildCount;
        private IComparer<TKey> _comparer;
      
        #endregion

        #region ISearchTree properties

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="BTree{TKey, TValue}" />.
        /// </summary>
        /// <value>The number of elements actually contained in the <see cref="BTree{TKey, TValue}" />.</value>
        public Int32 Height { get { return GetHeight(); } }

        /// <summary>
        /// Gets the height of the <see cref="BTree{TKey, TValue}" />.
        /// </summary>
        /// <value>The height of the <see cref="BTree{TKey, TValue}" />.</value>
        public Int32 Count { get { return _nodeCount; } }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the <see cref="IComparer{T}" /> that is used to determine order of keys for the tree. 
        /// </summary>
        /// <value>The <see cref="IComparer{T}" /> generic interface implementation that is used to determine order of keys for the current <see cref="BTree{TKey, TValue}" /> and to provide hash values for the keys.</value>
        public IComparer<TKey> Comparer { get { return _comparer; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="maxNodeSize">The maximum size of a node in the tree.</param>
        /// <exception cref="System.ArgumentException">The maximum node size is less than 3.</exception>
        public BTree(Int32 maxNodeSize)
        {
            if (maxNodeSize < 3)
                throw new ArgumentException("The maximum node size is less than 3.", "maxNodeSize");

            _version = 0;
            _height = 0;
            _nodeCount = 1;

            _maximumKeyCount = maxNodeSize - 1;
            _maximumChildCount = maxNodeSize;
            _minimumKeyCount = (maxNodeSize - 1) / 2;
            _minimumChildCount = maxNodeSize / 2;
            _minimizationFactor = (maxNodeSize + 1) / 2;

            _root = new Node();
            _root.Keys = new TKey[_maximumKeyCount];
            _root.Values = new TValue[_maximumKeyCount];
            _root.Children = new Node[_maximumChildCount];
            _root.Parent = null;
            _root.CurrentKeyCount = 0;
            _root.IsLeaf = true;

            _comparer = Comparer<TKey>.Default;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="maxNodeSize">The maximum size of a node in the tree.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentException">The maximum node size is less than 3.</exception>
        public BTree(Int32 maxNodeSize, IComparer<TKey> comparer)
        {
            if (maxNodeSize < 3)
                throw new ArgumentException("The maximum node size is less than 3.", "maxNodeSize");

            _version = 0;
            _height = 0;
            _nodeCount = 1;

            _maximumKeyCount = maxNodeSize - 1;
            _maximumChildCount = maxNodeSize;
            _minimumKeyCount = (maxNodeSize - 1) / 2;
            _minimumChildCount = maxNodeSize / 2;
            _minimizationFactor = (maxNodeSize + 1) / 2;

            _root = new Node();
            _root.Keys = new TKey[_maximumKeyCount];
            _root.Values = new TValue[_maximumKeyCount];
            _root.Children = new Node[_maximumChildCount];
            _root.Parent = null;
            _root.CurrentKeyCount = 0;
            _root.IsLeaf = true;

            _comparer = comparer ?? Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="maxNodeSize">The maximum size of a node in the tree.</param>
        /// <param name="source">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="BTree{TKey, TValue}" />.</param>
        /// <exception cref="System.ArgumentException">The maximum node size is less than 3.</exception>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BTree(Int32 maxNodeSize, IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (maxNodeSize < 3)
                throw new ArgumentException("The maximum node size is less than 3.", "maxNodeSize");

            _height = 0;
            _version = 0;
            _nodeCount = 1;

            _maximumKeyCount = maxNodeSize - 1;
            _maximumChildCount = maxNodeSize;
            _minimumKeyCount = (maxNodeSize - 1) / 2;
            _minimumChildCount = maxNodeSize / 2;
            _minimizationFactor = (maxNodeSize + 1) / 2;

            _root = new Node();
            _root.Keys = new TKey[_maximumKeyCount];
            _root.Values = new TValue[_maximumKeyCount];
            _root.Children = new Node[_maximumChildCount];
            _root.Parent = null;
            _root.CurrentKeyCount = 0;
            _root.IsLeaf = true;

            _comparer = Comparer<TKey>.Default;

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                Insert(element.Key, element.Value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BTree{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="maxNodeSize">The maximum size of a node in the tree.</param>
        /// <param name="source">The <see cref="IEnumerable{KeyValuePair{TKey, TValue}}" /> whose elements are copied to the new <see cref="BTree{TKey, TValue}" />.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> implementation to use when comparing keys, or null to use the default <see cref="Comparer{T}" /> for the type of the key.</param>
        /// <exception cref="System.ArgumentException">The maximum node size is less than 3.</exception>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        public BTree(Int32 maxNodeSize, IEnumerable<KeyValuePair<TKey, TValue>> source, IComparer<TKey> comparer)
        {
            if (maxNodeSize < 3)
                throw new ArgumentException("The maximum node size is less than 3.", "maxNodeSize");
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");

            _height = 0;
            _version = 0;
            _nodeCount = 1;

            _maximumKeyCount = maxNodeSize - 1;
            _maximumChildCount = maxNodeSize;
            _minimumKeyCount = (maxNodeSize - 1) / 2;
            _minimumChildCount = maxNodeSize / 2;
            _minimizationFactor = (maxNodeSize + 1) / 2;

            _root = new Node();
            _root.Keys = new TKey[_maximumKeyCount];
            _root.Values = new TValue[_maximumKeyCount];
            _root.Children = new Node[_maximumChildCount];
            _root.Parent = null;
            _root.CurrentKeyCount = 0;
            _root.IsLeaf = true;

            _comparer = comparer ?? Comparer<TKey>.Default;

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                Insert(element.Key, element.Value);
            }
        }

        #endregion

        #region BTree methods

        /// <summary>
        /// Searches the <see cref="BTree{TKey, TValue}" /> for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value of the element with the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        /// <exception cref="System.ArgumentException">The tree does not contain the specified key.;key</exception>
        public TValue Search(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");
            Node node = SearchNode(_root, key);

            if (node == null)
                throw new ArgumentException("The tree does not contain the specified key.", "key");

            Int32 index = 0;
            for (Int32 i = 0; i < node.Keys.Length; i++)
            {
                if (_comparer.Compare(node.Keys[i], key) == 0)
                {
                    index = i;
                }
            }

            return node.Values[index];
        }

        /// <summary>
        /// Searches the <see cref="BTree{TKey, TValue}" /> for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value of the element with the specified key.</param>
        /// <returns><c>true</c> if the <see cref="BTree{TKey, TValue}" /> contains the element with the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key;The key is null.</exception>
        public Boolean TrySearch(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");
            Node node = SearchNode(_root, key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            Int32 index = 0;
            for (Int32 i = 0; i < node.Keys.Length; i++)
            {
                if (_comparer.Compare(node.Keys[i], key) == 0)
                {
                    index = i;
                }

            }
            value = node.Values[index];
            return true;
        }

        /// <summary>
        /// Removes the specified key from the tree.
        /// </summary>
        /// <param name="key">The key of the element to delete.</param>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            Node node = SearchNode(_root, key);
            if (node == null)
                return false;
            RemoveKey(_root, key);
            _version++;
            return true;
        }

        /// <summary>
        /// Decides whether a BTree contains a key or not.
        /// </summary>
        /// <param name="key">The key</param>
        public bool Contains(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");

            return (SearchNode(_root, key) != null);
        }

        /// <summary>
        /// Inserts the specified key/value pair to the tree.
        /// </summary>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert.</param>
        public void Insert(TKey key, TValue value)
        {
            if (_root == null)
                throw new ArgumentNullException("root", "The root is null");
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");
            if (value == null)
                throw new ArgumentNullException("value", "The value is null.");

            Node node = _root;
            if (node.CurrentKeyCount < 2 * _minimizationFactor - 1)
            {
                InsertIntoNotFullTree(node, key, value);
            }
            else
            {
                Node newNode = new Node();
                newNode.Children = new Node[_maximumChildCount];
                for (Int32 i = 0; i < newNode.Children.Length; i++)
                {
                    newNode.Children[i] = null;
                }

                newNode.Keys = new TKey[_maximumKeyCount];
                newNode.Values = new TValue[_maximumKeyCount];
                newNode.IsLeaf = false;
                newNode.CurrentKeyCount = 0;
                newNode.Children[0] = node;
                node.Parent = newNode; 
                _root = newNode;
                SplitChild(newNode, 0);
                InsertIntoNotFullTree(newNode, key, value);
            }

            _version++;
        }

        /// <summary>
        /// Removes all elements from the <see cref="BTree{TKey, TValue}" />.
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
        /// <returns>An <see cref="BTreeEnumerator{TKey, TValue}" /> object that can be used to iterate through the collection.</returns>
        public ISearchTreeEnumerator<TKey, TValue> GetTreeEnumerator()
        {
            return new SearchTreeEnumerator(this);
        }

        #endregion

        #region IEnumerable methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator{KeyValuePair{TKey, TValue}}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region Private BTree methods

        /// <summary>
        /// Calculates the height of a tree.
        /// </summary>
        /// <returns>Height of the tree.</returns>
        private Int32 GetHeight()
        {
            Node actualNode = _root;
            while (actualNode.Children[0] != null)
            {
                actualNode = actualNode.Children[0];
                _height++;
            }

            return _height;
        }

        /// <summary>
        /// Inserts the specified key/value pair to the tree.
        /// </summary>
        /// <param name="notFullNode">The node where the key/value pair will be inserted</param>
        /// <param name="key">The key of the element to insert.</param>
        /// <param name="value">The value of the element to insert.</param>
        private void InsertIntoNotFullTree(Node node, TKey key, TValue value)
        {

            Int32 index = node.CurrentKeyCount - 1;
            if (node.IsLeaf)
            {
                while (index >= 0 && _comparer.Compare(key, node.Keys[index]) < 0)
                {
                    node.Keys[index + 1] = node.Keys[index];
                    node.Values[index + 1] = node.Values[index];
                    index--;
                }
                node.Keys[index + 1] = key;
                node.Values[index + 1] = value;
                node.CurrentKeyCount++;
            }
            else
            {
                while (index >= 0 && _comparer.Compare(key, node.Keys[index]) < 0)
                {
                    index--;
                }

                index++;
                if (node.Children[index].CurrentKeyCount == 2 * _minimizationFactor - 1)
                {
                    SplitChild(node, index);
                    if (_comparer.Compare(key, node.Keys[index]) > 0)
                    {
                        index++;
                    }
                }
                InsertIntoNotFullTree(node.Children[index], key, value);
            }
        }

        /// <summary>
        /// Splits a node.
        /// </summary>
        /// <param name="node">A non-full node.</param>
        /// <param name="index">The indexth child of node is full.</param>
        private void SplitChild(Node node, Int32 index)
        {
            if (_nodeCount == 1)
                _nodeCount += 2;
            else
                _nodeCount++;
            Node newNode = new Node();
            newNode.Keys = new TKey[_maximumKeyCount];
            newNode.Values = new TValue[_maximumKeyCount];
            newNode.Children = new Node[_maximumChildCount];
            Node actNode = node.Children[index];
            newNode.CurrentKeyCount = _minimizationFactor - 1;
            newNode.IsLeaf = actNode.IsLeaf;
            for (Int32 j = 0; j < _minimizationFactor - 1; j++)
            {
                newNode.Keys[j] = actNode.Keys[j + _minimizationFactor];
                newNode.Values[j] = actNode.Values[j + _minimizationFactor];
            }
            if (!actNode.IsLeaf)
            {
                for (Int32 j = 0; j < _minimizationFactor; j++)
                {
                    newNode.Children[j] = actNode.Children[j + _minimizationFactor];
                    newNode.Children[j].Parent = newNode;
                }
            }
            actNode.CurrentKeyCount = _minimizationFactor - 1;

            for (Int32 j = node.CurrentKeyCount; j > index; j--)
            {
                node.Children[j + 1] = node.Children[j];
            }

            node.Children[index + 1] = newNode;
            newNode.Parent = node; //!!!

            for (Int32 j = node.CurrentKeyCount - 1; j > index - 1; j--)
            {
                node.Keys[j + 1] = node.Keys[j];
                node.Values[j + 1] = node.Values[j];
            }

            node.Keys[index] = actNode.Keys[_minimizationFactor - 1];
            node.Values[index] = actNode.Values[_minimizationFactor - 1];
            node.CurrentKeyCount++;
        }

        /// <summary>
        /// Searches the tree for an element with a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="node">The start node.</param>
        /// <returns>The element with the key or <c>null</c> if the key is not within the tree.</returns>
        private Node SearchNode(Node node, TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");
            if (node == null)
                throw new ArgumentNullException("node", "The node is null.");

            Int32 index = 0;
            while (index < node.CurrentKeyCount && _comparer.Compare(key, node.Keys[index]) > 0)
            {
                index++;
            }

            if (index < node.CurrentKeyCount && _comparer.Compare(key, node.Keys[index]) == 0)
                return node;
            if (node.IsLeaf)
                return null;
            else
                return SearchNode(node.Children[index], key);
        }

        /// <summary>
        /// Removes a key from a node.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="node">The node.</param>
        private void RemoveKey(Node node, TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key", "The key is null.");
            if (node == null)
                throw new ArgumentNullException("node", "The node is null.");

            if (node.IsLeaf)
            {
                Int32 index = 0;
                for (Int32 i = 0; i < node.Keys.Length; i++)
                {
                    if (_comparer.Compare(node.Keys[i], key) == 0)
                        index = i;
                }

                for (Int32 i = index; i < node.Keys.Length - 1; i++)
                {
                    node.Keys[i] = node.Keys[i + 1];
                    node.Values[i] = node.Values[i + 1];
                }
                node.CurrentKeyCount--;
            }
            else
            {
                Int32 index = -1;
                for (Int32 i = 0; i < node.Keys.Length; i++)
                {
                    if (_comparer.Compare(node.Keys[i], key) == 0)
                        index = i;
                }


                if (index != -1)
                {
                    Node leftChildNode = node.Children[index];
                    Node rightChildNode = node.Children[index + 1];
                    if (leftChildNode.CurrentKeyCount >= _minimizationFactor)
                    {
                        Node predecessorNode = leftChildNode;
                        Node erasureNode = predecessorNode;
                        while (!predecessorNode.IsLeaf)
                        {
                            erasureNode = predecessorNode;
                            predecessorNode = predecessorNode.Children[node.CurrentKeyCount - 1];
                        }
                        node.Keys[index] = predecessorNode.Keys[predecessorNode.CurrentKeyCount - 1];
                        node.Values[index] = predecessorNode.Values[predecessorNode.CurrentKeyCount - 1];
                        RemoveKey(erasureNode, node.Keys[index]);
                    }
                    else if (rightChildNode.CurrentKeyCount >= _minimizationFactor)
                    {
                        Node successorNode = rightChildNode;
                        Node erasureNode = successorNode;
                        while (!successorNode.IsLeaf)
                        {
                            erasureNode = successorNode;
                            successorNode = successorNode.Children[0];
                        }
                        node.Keys[index] = successorNode.Keys[0];
                        node.Values[index] = successorNode.Values[0];
                        RemoveKey(erasureNode, node.Keys[index]);
                    }
                    else
                    {
                        Int32 medianKeyIndex = MergeNodes(leftChildNode, rightChildNode);
                        MoveKey(node, index, 1, leftChildNode, medianKeyIndex);
                        RemoveKey(leftChildNode, key);
                    }
                }
                else
                {
                    index = SubtreeRootNodeIndex(node, key);
                    Node childNode = node.Children[index];
                    if (childNode.CurrentKeyCount == _minimizationFactor - 1)
                    {
                        Node leftChildSibling = (index - 1 >= 0) ? node.Children[index - 1] : null;
                        Node rightChildSibling = (index + 1 <= node.CurrentKeyCount) ? node.Children[index + 1] : null;
                        if (leftChildSibling != null && leftChildSibling.CurrentKeyCount >= _minimizationFactor)
                        {
                            ShiftRightByOne(childNode);
                            childNode.Keys[0] = node.Keys[index - 1];
                            childNode.Values[0] = node.Values[index - 1];
                            if (!childNode.IsLeaf)
                            {
                                childNode.Children[0] = leftChildSibling.Children[leftChildSibling.CurrentKeyCount];
                            }
                            childNode.CurrentKeyCount++;

                            node.Keys[index - 1] = leftChildSibling.Keys[leftChildSibling.CurrentKeyCount - 1];
                            node.Values[index - 1] = leftChildSibling.Values[leftChildSibling.CurrentKeyCount - 1];

                            RemoveFromNode(leftChildSibling, leftChildSibling.CurrentKeyCount - 1, 1);
                        }
                        else if (rightChildSibling != null && rightChildSibling.CurrentKeyCount >= _minimizationFactor)
                        {
                            childNode.Keys[childNode.CurrentKeyCount] = node.Keys[index];
                            childNode.Values[childNode.CurrentKeyCount] = node.Values[index];
                            if (!childNode.IsLeaf)
                            {
                                childNode.Children[childNode.CurrentKeyCount + 1] = rightChildSibling.Children[0];
                            }
                            childNode.CurrentKeyCount++;

                            node.Keys[index] = rightChildSibling.Keys[0];
                            node.Values[index] = rightChildSibling.Values[0];

                            RemoveFromNode(rightChildSibling, 0, 0);
                        }
                        else
                        {
                            if (leftChildSibling != null)
                            {
                                Int32 medianKeyIndex = MergeNodes(childNode, leftChildSibling);
                                MoveKey(node, index - 1, 0, childNode, medianKeyIndex);
                            }
                            else if (rightChildSibling != null)
                            {
                                Int32 medianKeyIndex = MergeNodes(childNode, rightChildSibling);
                                MoveKey(node, index, 1, childNode, medianKeyIndex);
                            }
                        }
                    }
                    RemoveKey(childNode, key);
                }
            }
        }

        /// <summary>
        /// Remove the indexth element from a node.
        /// </summary>
        private void RemoveFromNode(Node node, Int32 index, Int32 leftOrRightChild)
        {
            if (index >= 0)
            {
                Int32 i;
                for (i = index; i < node.CurrentKeyCount - 1; i++)
                {
                    node.Keys[i] = node.Keys[i + 1];
                    node.Values[i] = node.Values[i + 1];
                    if (!node.IsLeaf)
                    {
                        if (i >= index + leftOrRightChild)
                        {
                            node.Children[i] = node.Children[i + 1];
                        }
                    }
                }
                node.Keys[i] = default(TKey);
                node.Values[i] = default(TValue);
                if (!node.IsLeaf)
                {
                    if (i >= index + leftOrRightChild)
                    {
                        node.Children[i] = node.Children[i + 1];
                    }
                    node.Children[i + 1] = null;
                }
                node.CurrentKeyCount--;
            }
        }

        /// <summary>
        /// Merges two nodes.
        /// </summary>
        private Int32 MergeNodes(Node dstNode, Node srcNode)
        {
            _nodeCount--;
            Int32 medianKeyIndex;
            if (_comparer.Compare(srcNode.Keys[0], dstNode.Keys[dstNode.CurrentKeyCount - 1]) < 0)
            {
                Int32 i;
                // shift all elements of dstNode right by srcNode.mNumKeys + 1 to make place for the srcNode and the median key
                if (!dstNode.IsLeaf)
                {
                    dstNode.Children[srcNode.CurrentKeyCount + dstNode.CurrentKeyCount + 1] = dstNode.Children[dstNode.CurrentKeyCount];
                    dstNode.Children[srcNode.CurrentKeyCount + dstNode.CurrentKeyCount + 1].Parent = dstNode;
                }
                for (i = dstNode.CurrentKeyCount; i > 0; i--)
                {
                    dstNode.Keys[srcNode.CurrentKeyCount + i] = dstNode.Keys[i - 1];
                    dstNode.Values[srcNode.CurrentKeyCount + i] = dstNode.Values[i - 1];
                    if (!dstNode.IsLeaf)
                    {
                        dstNode.Children[srcNode.CurrentKeyCount + i] = dstNode.Children[i - 1];
                        dstNode.Children[srcNode.CurrentKeyCount + i].Parent = dstNode;
                    }
                }

                // clear the median key (element)
                medianKeyIndex = srcNode.CurrentKeyCount;
                dstNode.Keys[medianKeyIndex] = default(TKey);
                dstNode.Values[medianKeyIndex] = default(TValue);

                // copy the srcNode's elements into dstNode
                for (i = 0; i < srcNode.CurrentKeyCount; i++)
                {
                    dstNode.Keys[i] = srcNode.Keys[i];
                    dstNode.Values[i] = srcNode.Values[i];
                    if (!srcNode.IsLeaf)
                    {
                        dstNode.Children[i] = srcNode.Children[i];
                        dstNode.Children[i].Parent = dstNode;
                    }
                }
                if (!srcNode.IsLeaf)
                {
                    dstNode.Children[i] = srcNode.Children[i];
                    dstNode.Children[i].Parent = dstNode;
                }
            }
            else
            {
                // clear the median key (element)
                medianKeyIndex = dstNode.CurrentKeyCount;
                dstNode.Keys[medianKeyIndex] = default(TKey);
                dstNode.Values[medianKeyIndex] = default(TValue);

                // copy the srcNode's elements into dstNode
                Int32 offset = medianKeyIndex + 1;
                Int32 i;
                for (i = 0; i < srcNode.CurrentKeyCount; i++)
                {
                    dstNode.Keys[offset + i] = srcNode.Keys[i];
                    dstNode.Values[offset + i] = srcNode.Values[i];
                    if (!srcNode.IsLeaf)
                    {
                        dstNode.Children[offset + i] = srcNode.Children[i];
                        dstNode.Children[offset + i].Parent = dstNode;

                    }
                }
                if (!srcNode.IsLeaf)
                {
                    dstNode.Children[offset + i] = srcNode.Children[i];
                    dstNode.Children[offset + i] = dstNode;
                }
            }
            dstNode.CurrentKeyCount += srcNode.CurrentKeyCount;
            return medianKeyIndex;
        }

        /// <summary>
        /// Move the key from srcNode at index into dstNode at medianKeyIndex.
        /// </summary>
        private void MoveKey(Node srcNode, Int32 srcKeyIndex, Int32 childIndex, Node dstNode, Int32 medianKeyIndex)
        {
            dstNode.Keys[medianKeyIndex] = srcNode.Keys[srcKeyIndex];
            dstNode.Values[medianKeyIndex] = srcNode.Values[srcKeyIndex];
            dstNode.CurrentKeyCount++;

            RemoveFromNode(srcNode, srcKeyIndex, childIndex);

            if (srcNode == _root && srcNode.CurrentKeyCount == 0)
            {
                _root = dstNode;
            }
        }

        /// <summary>
        /// Get the index of a key.
        /// </summary>
        private Int32 SubtreeRootNodeIndex(Node node, TKey key)
        {
            for (Int32 i = 0; i < node.CurrentKeyCount; i++)
            {
                if (_comparer.Compare(key, node.Keys[i]) < 0)
                {
                    return i;
                }
            }
            return node.CurrentKeyCount;
        }

        /// <summary>
        /// Shift a node to right by one.
        /// </summary>
        private void ShiftRightByOne(Node node)
        {
            if (!node.IsLeaf)
            {
                node.Children[node.CurrentKeyCount + 1] = node.Children[node.CurrentKeyCount];
            }
            for (Int32 i = node.CurrentKeyCount - 1; i >= 0; i--)
            {
                node.Keys[i + 1] = node.Keys[i];
                node.Values[i + 1] = node.Values[i];
                if (!node.IsLeaf)
                {
                    node.Children[i + 1] = node.Children[i];
                }
            }
        }

        #endregion
    }
}

