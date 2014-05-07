using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ELTE.AEGIS.Collections.QuadTree
{
    /// <summary>
    /// Represents a linea quadtree.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    /// <remarks>
    /// The source is Aaizawa
    /// </remarks>
    public class LinearQuadTree<T>
    {
        #region Public types

        /// <summary>
        /// Represents the neighbor's relative direction in a quadtree.
        /// </summary>
        public enum Direction
        {
            North,
            South,
            West,
            East
        };

        #endregion

        #region Private fields

        private Int64 _tx; // 01 repeated height - 1 times
        private Int64 _ty; // 10 repeated height - 1 times
        private Int64 _dw; // (-1, 0) encoded
        private Int64 _ds; // (0,-1) encoded
        private Int64 _de; // (1,0) encoded
        private Int64 _dn; // (0,1) encoded
        private Int64 _mask;
        private Int32 _maxLevel;
        private RegionQuadTree<T> _root;
        private List<LinearNode<T>> _q;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearQuadTree{T}" /> class.
        /// </summary>
        /// <param name="root">The root node.</param>
        public LinearQuadTree(RegionQuadTree<T> root) {
            _maxLevel = root.TreeHeight-1;
            this._root = root;
            Int64 ptx = 0;
            for (Int32 i = 0; i <= _maxLevel; i++)
            {
                ptx += Convert.ToInt64(Math.Pow(4, Convert.ToDouble(i)));
            }
            _tx = ptx;
            _ty = _tx << 1;
            _dw = Encode(-1, 0, _maxLevel);
            _ds = Encode(0, -1, _maxLevel);
            _de = Encode(1, 0, _maxLevel);
            _dn = Encode(0, 1, _maxLevel);
            _mask = Convert.ToInt64(Math.Pow(4,_maxLevel))-1;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the leafs of the tree.
        /// </summary>
        /// <returns>The liost of leafs.</returns>
        public List<LinearNode<T>> GetLeafs() {
            if(_q == null){
                _q = CalculateLeafProperties();
            }
            return _q;
        }

        /// <summary>
        /// Gets the neighbor of the given node in the given direction.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="d">The direction<./param>
        /// <returns>The neighbor, or <c>null</c> if there is no neighbor in the given direction.</returns>
        public LinearNode<T> GetNeighbor(LinearNode<T> node,Direction d) {
            // source: Aizawa, K. et al.: Constant Time Neighbor Finding in Quadtrees, ISCCSP 2008.

            Int32? dd = null;
            long dnq = 0;
            switch (d)
            {
                case Direction.North:
                    dd = node.North;
                    dnq = _dn;
                    break;
                case Direction.South:
                    dd = node.South;
                    dnq = _ds;
                    break;
                case Direction.East:
                    dd = node.East;
                    dnq = _de;
                    break;
                case Direction.West:
                    dd = node.West;
                    dnq = _dw;
                    break;
            }
            if (dd.HasValue)
            {
                Int64 mq;
                Int64 nq = node.Code;
                Int32 l = node.Region.Level;
                if (dd < 0)
                {
                    mq = Add(((nq >> 2 * (_maxLevel - l - dd.Value)) << 2 * (_maxLevel - l - dd.Value)), dnq << (2 * (_maxLevel - l - dd.Value)));
                }
                else
                {
                    mq = Add(nq, dnq << (2 * (_maxLevel - l)));
                }
                //
                LinearNode<T> result = GetNode(mq, _q);
                // Fix the algorithm
                if (dd > 0 && (d == Direction.South || d == Direction.West))
                {
                    return NodeOf(GetMostNorthEastRecursive(result.Region.Parent));
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private methods

        private RegionQuadTree<T> GetMostNorthEastRecursive(RegionQuadTree<T> p)
        {
            if (!p.IsLeaf)
            {
                return GetMostNorthEastRecursive(p.Childs[3]);
            }
            return p;
        }

        private LinearNode<T> NodeOf(RegionQuadTree<T> region) {
            foreach(LinearNode<T> node in _q){
                if(node.Region == region){
                    return node;
                }
            }
            return null;
        }
 
        private Int64 Add(Int64 left, Int64 right)
        {
            return AddQ(left, right, _tx, _ty, _mask);
        }

        private List<LinearNode<T>> CalculateLeafProperties()
        {
            // source: Aizawa, K. et al.: Constant Time Neighbor Finding in Quadtrees, ISCCSP 2008.

            _q = new List<LinearNode<T>>();
            LinearNode<T> node = new LinearNode<T>(_root, 0);
            _q.Add(node);
            LinearNode<T> firstGrey = GetFirstGrey(_q);
            while (firstGrey!= null)
            {
                SetNeighbourLevels(firstGrey);
                _q.Remove(firstGrey);
                LinearNode<T>[] children = new LinearNode<T>[4];
                children[0] = CreateChildNode(firstGrey, 0);
                children[1] = CreateChildNode(firstGrey, 1);
                children[2] = CreateChildNode(firstGrey, 2);
                children[3] = CreateChildNode(firstGrey, 3);
                SetChildNeighbourLevels(firstGrey, children);
                _q.InsertRange(0,children);
                firstGrey = GetFirstGrey(_q);
            }
            return _q;
        }

        private void SetChildNeighbourLevels(LinearNode<T> parent, LinearNode<T>[] children)
        {
            LinearNode<T> neighbor;
            Int32 l = parent.Region.Level + 1;
            children[0].East = 0;
            children[0].North = 0;
            if (parent.West != null)
            {
                children[0].West = (parent.West - 1);
                neighbor = GetNode(Add(children[0].Code, _dw << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.East = n.East + 1;
                }

            }
            if (parent.South != null)
            {
                children[0].South = parent.South - 1;
                neighbor = GetNode(Add(children[0].Code, _ds << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.North = n.North + 1;
                }
            }
            children[1].North = 0;
            children[1].West = 0;
            if (parent.East != null)
            {
                children[1].East = parent.East - 1;
                neighbor = GetNode(Add(children[1].Code, _de << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.West = n.West + 1;
                }
            }
            if (parent.South != null)
            {
                children[1].South = parent.South - 1;
                neighbor = GetNode(Add(children[1].Code, _ds << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.North = n.North + 1;
                }
            }

            children[2].South = 0;
            children[2].East = 0;
            if (parent.North != null)
            {
                children[2].North = parent.North - 1;
                neighbor = GetNode(Add(children[2].Code, _dn << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.South = n.South + 1;
                }
            }
            if (parent.West != null)
            {
                children[2].West = parent.West - 1;
                neighbor = GetNode(Add(children[2].Code, _dw << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.East = n.East + 1;
                }
            }
            children[3].South = 0;
            children[3].West = 0;
            if (parent.North != null)
            {
                children[3].North = parent.North - 1;
                neighbor = GetNode(Add(children[3].Code, _dn << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.South = n.South + 1;
                }
            }
            if (parent.East != null)
            {
                children[3].East = parent.East - 1;
                neighbor = GetNode(Add(children[3].Code, _de << (2 * (_maxLevel - l))), _q);
                if (neighbor != null && neighbor.Region.Level == l)
                {
                    LinearNode<T> n = neighbor;
                    n.West = n.West + 1;
                }
            }

        }

        private LinearNode<T> CreateChildNode(LinearNode<T> parent, Int32 p)
        {
            return new LinearNode<T>(parent.Region.Childs[p], GetChildCode(parent, p));
        }

        private Int64 GetChildCode(LinearNode<T> node, Int32 direction)
        {
            Int64 childCode = node.Code;
            childCode += Convert.ToInt64(Math.Pow(4, _maxLevel - node.Region.Level - 1)) * direction;
            return childCode;
        }

        private LinearNode<T> GetFirstGrey(List<LinearNode<T>> q)
        {
            foreach (LinearNode<T> node in q)
            {
                if (!node.Region.IsLeaf)
                {
                    return node;
                }
           }
            return null;
        }

        private void SetNeighbourLevels(LinearNode<T> firstGrey)
        {
            Int32 l = firstGrey.Region.Level;
            // EAST
            Int64 id = Add(firstGrey.Code, (_de << (2 * (_maxLevel - l))));
            LinearNode<T> node = GetNode(id, _q);
            if (node != null)
            {
                LinearNode<T> n = node;
                if (node.Region.Level == l && node.West != null)
                {
                    n.West = node.West + 1;
                }
            }
            // NORTH
            id = Add(firstGrey.Code, (_dn << (2 * (_maxLevel - l))));
            node = GetNode(id, _q);
            if (node != null)
            {
                LinearNode<T> n = node;
                if (node.Region.Level == l && node.South != null)
                {
                    n.South = node.South + 1;
                }
            }
            // WEST
            id = Add(firstGrey.Code, (_dw << (2 * (_maxLevel - l))));
            node = GetNode(id, _q);
            if (node != null)
            {
                LinearNode<T> n = node;
                if (node.Region.Level == l && node.East != null)
                {
                    n.East = node.East + 1;
                }
            }
            // SOUTH
            id = Add(firstGrey.Code, (_ds << (2 * (_maxLevel - l))));
            node = GetNode(id, _q);
            if (node != null)
            {
                LinearNode<T> n = node;
                if (node.Region.Level == l && node.North != null)
                {
                    n.North = node.North + 1;
                }
            }

        }

        private LinearNode<T> GetNode(Int64 id, List<LinearNode<T>> q)
        {
            foreach (LinearNode<T> node in q)
            {
                if(node.Code.Equals(id)){
                    return node;
                }
            }
            return null;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Add method on dilated integers.
        /// </summary>
        /// <param name="left">Operator left side</param>
        /// <param name="right">Operator right side</param>
        /// <param name="tx">The location of X in the code.</param>
        /// <param name="ty">The location of Y in the code.</param>
        /// <param name="wordLengthMask">Mask to ignore carrige.</param>
        /// <returns>The result of the addition.</returns>
        public static Int64 AddQ(Int64 left, Int64 right, Int64 tx, Int64 ty, Int64 wordLengthMask)
        {
            Int64 res = (((left | ty) + (right & tx)) & tx) | (((left | tx) + (right & ty)) & ty);
            return res & wordLengthMask;
        }

        /// <summary>
        /// Extends the number with gaps.
        /// </summary>
        /// <param name="k">Integer to dilate.</param>
        /// <param name="wordLength">Unencoded location code length.</param>
        /// <returns>The dilated integer.</returns>
        public static Int64 QDilate(Int64 k, Int32 wordLength)
        {
            BitArray bitArr = new BitArray(BitConverter.GetBytes(k));
            BitArray retBitArr = new BitArray(2 * wordLength);
            for (Int32 i = 0; i < wordLength; i++)
            {
                retBitArr.Set(2 * i, bitArr.Get(i));
            }
            byte[] arr = new byte[retBitArr.Length * 8];
            retBitArr.CopyTo(arr, 0);
            return BitConverter.ToInt64(arr, 0);
        }

        /// <summary>
        /// Encode the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate to.</param>
        /// <param name="y">The Y coordinate to.</param>
        /// <param name="r">Unencoded location.</param>
        /// <returns>Encoded location.</returns>
        public static Int64 Encode(Int64 x, Int64 y, Int32 r)
        {
            return QDilate(x, r) | (QDilate(y, r) << 1);
        }

        #endregion
    }
}
