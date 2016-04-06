using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Topology
{
    /// <summary>
    /// Defines a provider for geometry identifiers.
    /// </summary>
    public interface IIdentifierProvider
    {
        /// <summary>
        /// Retrieves the identifier from an <see cref="IGeometry"/>.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The identifier.</returns>
        ISet<Int32> GetIdentifiers(IGeometry geometry);
    }
}
