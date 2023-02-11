// <copyright file="Storage.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Versioning
{
    /// <summary>
    /// Represents a general versioning storages.
    /// </summary>
    /// <typeparam name="TVersionKey">The versioning key type of the storage.</typeparam>
    /// <typeparam name="TGeometryKey">The geometry key type of the storage.</typeparam>
    /// <author>Máté Cserép</author>
    public abstract class Storage<TVersionKey, TGeometryKey>
    {
        #region Public methods

        /// <summary>
        /// Gets the <see cref="RevisionModel{TVersionKey}"/> instance from the storage.
        /// </summary>
        /// <returns>Null if no revision model has been persisted yet; otherwise the revision model stored in the storage.</returns>
        public abstract RevisionModel<TVersionKey> GetRevisionModel();

        /// <summary>
        /// Persists the given <see cref="RevisionModel{TVersionKey}"/> instance in the storage.
        /// </summary>
        /// <param name="revisionModel">The revision model.</param>
        public abstract void StoreRevisionModel(RevisionModel<TVersionKey> revisionModel);

        /// <summary>
        /// Gets the changeset associated with the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The changeset associated with the given version.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no changeset exists for the given version.</exception>
        public Changeset<TGeometryKey> GetChangeset(TVersionKey version)
        {
            if (!HasChangeset(version))
                throw new KeyNotFoundException(String.Format("No changeset found for the given version {0}.", version));
            return RetrieveChangeset(version);
        }

        /// <summary>
        /// Persists the given changeset for the given version.
        /// </summary>
        /// <param name="version">The version key to affect.</param>
        /// <param name="modifications">The changeset.</param>
        /// <returns>True if a changeset already existed for the given version; otherwise, false.</returns>
        public abstract Boolean StoreChangeset(TVersionKey version, Changeset<TGeometryKey> modifications);

        /// <summary>
        /// Determines whether a changeset exists for the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>True if a changeset exists for the given version; otherwise, false.</returns>
        public abstract Boolean HasChangeset(TVersionKey version);

        /// <summary>
        /// Removes the changeset for the given version.
        /// </summary>
        /// <remarks>
        /// No exception is thrown when no changeset exists for the given version.
        /// </remarks>
        /// <param name="version">The version key to affect.</param>
        /// <returns>True if a changeset existed for the given version; otherwise, false.</returns>
        public abstract Boolean RemoveChangeset(TVersionKey version);

        /// <summary>
        /// Gets the snapshot for the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The snapshot stored for the requested version.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when no snapshot exists for the given version.</exception>
        public IDictionary<TGeometryKey, IGeometry> GetSnapshot(TVersionKey version)
        {
            if (!HasSnapshot(version))
                throw new KeyNotFoundException(String.Format("No snapshot found for the given version {0}.", version));
            return RetrieveSnapshot(version);
        }

        /// <summary>
        /// Persists a new snapshot for the given version.
        /// </summary>
        /// <remarks>
        /// When a snapshot already exists for the given version, it will be overwritten.
        /// </remarks>
        /// <param name="version">The version key to affect.</param>
        /// <param name="state">The state object to store.</param>
        /// <returns>True if an existing snapshot was overwritten; otherwise, false.</returns>
        public abstract Boolean StoreSnapshot(TVersionKey version, IDictionary<TGeometryKey, IGeometry> state);

        /// <summary>
        /// Determines whether a snapshot exists for the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>True if a snapshot exists for the given version; otherwise, false.</returns>
        public abstract Boolean HasSnapshot(TVersionKey version);

        /// <summary>
        /// Removes the snapshot for the given version.
        /// </summary>
        /// <remarks>
        /// No exception is thrown when no snapshot exists for the given version.
        /// </remarks>
        /// <param name="version">The version key to affect.</param>
        /// <returns>True if a snapshot existed for the given version; otherwise, false.</returns>
        public abstract Boolean RemoveSnapshot(TVersionKey version);

        #endregion

        #region Protected methods

        /// <summary>
        /// Retrieves the changeset associated with the given version.
        /// </summary>
        /// <remarks>
        /// The existence of the changeset is a required precondition for this method.
        /// </remarks>
        /// <param name="version">The version key to query.</param>
        /// <returns>The changeset associated with the given version.</returns>
        protected abstract Changeset<TGeometryKey> RetrieveChangeset(TVersionKey version);

        /// <summary>
        /// Retrieves the snapshot for the given version.
        /// </summary>
        /// <remarks>
        /// The existence of the snapshot is a required precondition for this method.
        /// </remarks>
        /// <param name="version">The version key to query.</param>
        /// <returns>The snapshot stored for the requested version.</returns>
        protected abstract IDictionary<TGeometryKey, IGeometry> RetrieveSnapshot(TVersionKey version);

        #endregion
    }
}