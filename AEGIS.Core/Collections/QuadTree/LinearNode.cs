using System;

namespace ELTE.AEGIS.Collections.QuadTree
{
    /// <summary>
    /// Represents a node of the linear quadtree.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    /// <remarks>
    /// The linear quadtree node encapsulates a <see cref="RegionQuadTree{T}" /> node and extends it's semantics with linear indexing and neighbour level differences.
    /// </remarks>
    public class LinearNode<T>
    {
        #region Public properties

        /// <summary>
        /// Gets the region quastree wrapped by the node.
        /// </summary>
        public RegionQuadTree<T> Region { get; private set; }

        /// <summary>
        /// Gets the level difference to the north neighbor.
        /// </summary>
        public Int32? North { get; set; }

        /// <summary>
        /// Gets the level difference to the west neighbor.
        /// </summary>
        public Int32? West { get; set; }

        /// <summary>
        /// Gets the level difference to the south neighbor.
        /// </summary>
        public Int32? South { get; set; }

        /// <summary>
        /// Gets the level difference to the east neighbor.
        /// </summary>
        public Int32? East { get; set; }

        /// <summary>
        /// Gets the location code.
        /// </summary>
        public Int64 Code { get; private set; }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearNode{T}" /> class.
        /// </summary>
        /// <param name="node">The region suadtree node.</param>
        /// <param name="code">The code.</param>
        public LinearNode(RegionQuadTree<T> node, Int64 code)
        {
            Region = node;
            Code = code;
        }

        #endregion
    }
}
