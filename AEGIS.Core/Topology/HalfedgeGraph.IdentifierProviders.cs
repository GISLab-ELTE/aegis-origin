using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Topology
{
    /// <summary>
    /// Represents a halfedge graph data structure that stores topology.
    /// </summary>
    public partial class HalfedgeGraph
    {
        /// <summary>
        /// Represents a fixed, <c>null</c> resulting geometry identifier provider.
        /// </summary>
        /// <seealso cref="ELTE.AEGIS.Topology.IIdentifierProvider" />
        public class NullIdentifierProvider : IIdentifierProvider
        {
            #region Implementation of IIdentifierProvider

            /// <summary>
            /// Retrieves the identifier from an <see cref="IGeometry"/>.
            /// </summary>
            /// <param name="geometry">The geometry.</param>
            /// <returns>The identifier.</returns>
            public ISet<int> GetIdentifiers(IGeometry geometry)
            {
                return null;
            }

            #endregion
        }

        /// <summary>
        /// Represents a fixed resulting geometry identifier provider.
        /// </summary>
        /// <seealso cref="ELTE.AEGIS.Topology.IIdentifierProvider" />
        public class FixedIdentifierProvider : IIdentifierProvider
        {
            #region Private fields

            /// <summary>
            /// The fixed identifier to return.
            /// </summary>
            private readonly ISet<Int32> _identifier;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="FixedIdentifierProvider"/> class.
            /// </summary>
            /// <param name="identifier">The identifier.</param>
            public FixedIdentifierProvider(Int32 identifier)
            {
                _identifier = new HashSet<Int32>() { identifier };
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FixedIdentifierProvider"/> class.
            /// </summary>
            /// <param name="identifiers">The identifiers.</param>
            public FixedIdentifierProvider(ISet<Int32> identifiers)
            {
                _identifier = identifiers;
            }

            #endregion

            #region Implementation of IIdentifierProvider

            /// <summary>
            /// Retrieves the identifier from an <see cref="IGeometry"/>.
            /// </summary>
            /// <param name="geometry">The geometry.</param>
            /// <returns>The identifier.</returns>
            public ISet<Int32> GetIdentifiers(IGeometry geometry)
            {
                return _identifier;
            }

            #endregion
        }

        /// <summary>
        /// Represents a geometry identifier provider based on field name.
        /// </summary>
        /// <seealso cref="ELTE.AEGIS.Topology.IIdentifierProvider" />
        public class FieldIdentifierProvider : IIdentifierProvider
        {
            #region Private fields

            /// <summary>
            /// The regular expression to match the field name.
            /// </summary>
            private readonly Regex _field;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="FieldIdentifierProvider"/> class.
            /// </summary>
            /// <param name="field">The field name.</param>
            public FieldIdentifierProvider(String field)
            {
                _field = new Regex("^" + field + "$", RegexOptions.IgnoreCase);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FieldIdentifierProvider"/> class.
            /// </summary>
            /// <param name="field">The regular expression to match the field name.</param>
            public FieldIdentifierProvider(Regex field)
            {
                _field = field;
            }

            #endregion

            #region Implementation of IIdentifierProvider

            /// <summary>
            /// Retrieves the identifier from an <see cref="IGeometry" />.
            /// </summary>
            /// <param name="geometry">The geometry.</param>
            /// <returns>The identifier.</returns>
            /// <exception cref="System.Collections.Generic.KeyNotFoundException">No key with appropriate integer value found.</exception>
            public ISet<Int32> GetIdentifiers(IGeometry geometry)
            {
                HashSet<Int32> identifiers = new HashSet<Int32>();
                foreach (Object value in geometry.Metadata
                    .Where(data => _field.IsMatch(data.Key))
                    .Select(data => data.Value))
                {
                    try
                    {
                        identifiers.Add(Convert.ToInt32(value));
                    }
                    catch
                    {
                        continue;
                    }
                }
                return identifiers;
            }

            #endregion
        }
    }
}
