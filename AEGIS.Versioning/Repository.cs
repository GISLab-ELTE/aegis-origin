// <copyright file="Repository.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Versioning
{
    /// <summary>
    /// Represents a general versioning repository.
    /// </summary>
    /// <typeparam name="TVersionKey">The versioning key type of the repository.</typeparam>
    /// <typeparam name="TGeometryKey">The geometry key type of the repository.</typeparam>
    /// <author>Máté Cserép</author>
    public abstract class Repository<TVersionKey, TGeometryKey>
    {
        #region Protected fields

        /// <summary>
        /// Stores the revision model of the versioning repository.
        /// </summary>
        protected RevisionModel<TVersionKey> _revisionModel;

        /// <summary>
        /// Stores the storage object of the versioning repository.
        /// </summary>
        protected Storage<TVersionKey, TGeometryKey> _storage;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the initial version in the repository.
        /// </summary>
        public TVersionKey InitVersion
        {
            get { return _revisionModel.InitRevision.Version; }
        }

        /// <summary>
        /// Gets the version key of the head revision in the repository.
        /// </summary>
        /// <remarks>
        /// The head revision of a <see cref="Repository{TVersionKey,TGeometryKey}"/> is the newest revision in it.
        /// </remarks>
        public TVersionKey HeadVersion
        {
            get { return _revisionModel.HeadRevision.Version; }
        }

        /// <summary>
        /// Gets the version key of the the main revision in the repository.
        /// </summary>
        /// <remarks>
        /// The main revision of a <see cref="Repository{TVersionKey,TGeometryKey}"/> is the newest revision on the main - initial - branch.
        /// </remarks>
        public TVersionKey MainVersion
        {
            get { return _revisionModel.MainRevision.Version; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Repository{TVersionKey,TGeometryKey}"/> instance.
        /// </summary>
        /// <param name="storage">The storage of the current repository.</param>
        public Repository(Storage<TVersionKey, TGeometryKey> storage)
        {
            _storage = storage;
            _revisionModel = _storage.GetRevisionModel();

            if (_revisionModel == null)
                throw new InvalidOperationException("The given storage does not contain a revision model.");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the version key of the head revision on the given version's branch.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The version key of the head revision belonging to the given version's branch.</returns>
        public TVersionKey GetHeadVersion(TVersionKey version)
        {
            return _revisionModel.GetHeadRevision(version).Version;
        }

        /// <summary>
        /// Gets the version key of the origin revision on the given version's branch.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The version key of the origin revision belonging to the given version's branch.</returns>
        public TVersionKey GetOriginVersion(TVersionKey version)
        {
            return _revisionModel.GetRevision(version).OriginRevision.Version;
        }

        /// <summary>
        /// Gets the preceding version of the given version.
        /// </summary>
        /// <remarks>
        /// The preceding version is the version from the result of the <see cref="GetPrecedingVersions"/> method on which the given version's alternations were made.
        /// </remarks>
        /// <param name="version">The version key to query.</param>
        /// <returns>The version key of the preceding revision.</returns>
        public TVersionKey GetPrecedingVersion(TVersionKey version)
        {
            return _revisionModel.GetRevision(version).PrecedingRevision.Version;
        }

        /// <summary>
        /// Gets the versions that were merged with the result of the <see cref="GetPrecedingVersion"/> method to create the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The version keys of the merged revisions.</returns>
        public TVersionKey[] GetMergedVersions(TVersionKey version)
        {
            return _revisionModel.GetRevision(version).MergedRevisions.Select(revision => revision.Version).ToArray();
        }

        /// <summary>
        /// Gets all the versions that precedes the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The version keys of the preceding revisions.</returns>
        public TVersionKey[] GetPrecedingVersions(TVersionKey version)
        {
            return _revisionModel.GetRevision(version).PrecedingRevisions.Select(revision => revision.Version).ToArray();
        }

        /// <summary>
        /// Gets the versions that succeeds the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The version keys of the succeeding revisions.</returns>
        public TVersionKey[] GetSucceedingVersions(TVersionKey version)
        {
            return _revisionModel.GetRevision(version).SucceedingRevisions.Select(revision => revision.Version).ToArray();
        }

        /// <summary>
        /// Gets the revision history for the given version or between two versions.
        /// </summary>
        /// <remarks>
        /// When the optional end version is not given, or equals to <c>null</c>, or it is not a linear ancestor of the start version, the full revision history for the given start version will be returned.
        /// The first element of the output array is the given version and last one is the stop version - or <see cref="InitVersion"/> when not applied.
        /// </remarks>
        /// <param name="version">The version key to query.</param>
        /// <param name="stopVersion">The optional ancestor version key as a stop condition.</param>
        /// <returns>The linear version history for the given version or between the two given versions.</returns>
        public TVersionKey[] GetHistory(TVersionKey version, TVersionKey stopVersion = default(TVersionKey))
        {
            var history = new List<TVersionKey>();
            RevisionDescriptor<TVersionKey> current = _revisionModel.GetRevision(version);
            do
            {
                history.Add(current.Version);
                current = current.PrecedingRevision;
            } while (current != null && !current.Version.Equals(stopVersion));
            return history.ToArray();
        }

        /// <summary>
        /// Gets the revision history tree for the given version.
        /// </summary>
        /// <remarks>
        /// While the result of the <see cref="GetHistory"/> method only contains the linear history (version history with changeset alternations); 
        /// the result of this method contains all preceding, merged versions as a tree.
        /// The resulted dictionary contains the given version and all preceding versions as keys, and their preceding versions as values.
        /// </remarks>
        /// <param name="version">The version key to query.</param>
        /// <returns>The non-linear version history tree for the given version.</returns>
        public IDictionary<TVersionKey, TVersionKey[]> GetTree(TVersionKey version)
        {
            var tree    = new Dictionary<TVersionKey, TVersionKey[]>();
            var process = new Queue<RevisionDescriptor<TVersionKey>>();
            process.Enqueue(_revisionModel.GetRevision(version));

            while (process.Count != 0)
            {
                RevisionDescriptor<TVersionKey> current = process.Dequeue();
                tree.Add(current.Version, current.PrecedingRevisions.Select(revision => revision.Version).ToArray());

                foreach (RevisionDescriptor<TVersionKey> revision in current.PrecedingRevisions)
                    if (!process.Contains(revision) && !tree.ContainsKey(revision.Version))
                        process.Enqueue(revision);
            }
            return tree;
        }

        /// <summary>
        /// Gets the changeset associated with the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The changeset associated with the given version.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no changeset exists for the given version.</exception>
        public Changeset<TGeometryKey> GetChangeset(TVersionKey version)
        {
            return _storage.GetChangeset(version);
        }

        /// <summary>
        /// Gets the patch changeset between two versions.
        /// </summary>
        /// <param name="startVersion">The start version's key.</param>
        /// <param name="endVersion">The end version's key.</param>
        /// <returns>The changeset of the patch to apply.</returns>
        public Changeset<TGeometryKey> GetPatch(TVersionKey startVersion, TVersionKey endVersion)
        {
            ICollection<TVersionKey> reverseKeys, forwardKeys;
            GetPath(startVersion, endVersion, out reverseKeys, out forwardKeys);
            return GetPatch(reverseKeys, forwardKeys);
        }

        /// <summary>
        /// Gets the geometry state for the given version.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The geometries indexed with their associated geometry keys in the requested state.</returns>
        public abstract IDictionary<TGeometryKey, IGeometry> GetState(TVersionKey version);

        /// <summary>
        /// Commits a new version.
        /// </summary>
        /// <param name="precedingVersion">The preceding version's key.</param>
        /// <param name="modifications">The modification changeset associated with the new version.</param>
        /// <returns>The new version key.</returns>
        public TVersionKey Commit(TVersionKey precedingVersion, Changeset<TGeometryKey> modifications)
        {
            return Commit(precedingVersion, null, modifications);
        }

        /// <summary>
        /// Commits a new version.
        /// </summary>
        /// <param name="precedingVersion">The preceding version's key.</param>
        /// <param name="mergedVersions">The versions that were merged into this new revision alongside with the preceding version.</param>
        /// <param name="modifications">The modification changeset associated with the new version.</param>
        /// <returns>The new version key.</returns>
        public virtual TVersionKey Commit(TVersionKey precedingVersion, ICollection<TVersionKey> mergedVersions, Changeset<TGeometryKey> modifications)
        {
            if (modifications == null)
                throw new ArgumentNullException("modifications", "The modifications to commit cannot be null.");

            TVersionKey newVersion = _revisionModel.CreateRevision(precedingVersion, mergedVersions);
            _storage.StoreChangeset(newVersion, modifications);
            _storage.StoreRevisionModel(_revisionModel);
            return newVersion;
        }

        /// <summary>
        /// Creates a new snapshot for the given version.
        /// </summary>
        /// <remarks>
        /// When a snapshot already exists for the given version, it will be overwritten.
        /// </remarks>
        /// <param name="version">The version key to affect.</param>
        /// <returns>True if an existing snapshot was overwritten; otherwise, false.</returns>
        public virtual Boolean CreateSnapshot(TVersionKey version)
        {
            return _storage.StoreSnapshot(version, GetState(version));
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Gets the patch changeset between two versions.
        /// </summary>
        /// <param name="reverseKeys">The collection of version keys to patch backwards in the given order.</param>
        /// <param name="forwardKeys">The collection of version keys to patch forwards in the given order.</param>
        /// <returns>The changeset of the patch to apply.</returns>
        protected abstract Changeset<TGeometryKey> GetPatch(ICollection<TVersionKey> reverseKeys, ICollection<TVersionKey> forwardKeys);

        /// <summary>
        /// Gets the version key path between two versions.
        /// </summary>
        /// <remarks>
        /// The returned path is given with two key collections containing the version keys to patch backwards and forwards.
        /// </remarks>
        /// <param name="startVersion">The key of the start version.</param>
        /// <param name="endVersion">The key of the end version.</param>
        /// <param name="reverseKeys">The collection of version keys to patch backwards in the given order.</param>
        /// <param name="forwardKeys">The collection of version keys to patch forwards in the given order.</param>
        protected void GetPath(TVersionKey startVersion, TVersionKey endVersion, out ICollection<TVersionKey> reverseKeys, out ICollection<TVersionKey> forwardKeys)
        {
            TVersionKey[] startOriginHistory = GetOriginHistory(startVersion);
            TVersionKey[] endOriginHistory = GetOriginHistory(endVersion);
            TVersionKey originMatchVersion = FindCommonVersion(startOriginHistory, endOriginHistory);
            Int32 startMatchIdx = Array.FindIndex(startOriginHistory, version => version.Equals(originMatchVersion));
            Int32 endMatchIdx = Array.FindIndex(endOriginHistory, version => version.Equals(originMatchVersion));

            TVersionKey[] startMatchHistory;
            if (startMatchIdx > 0)
                startMatchHistory = GetHistory(startOriginHistory[startMatchIdx - 1],
                                                           startOriginHistory[startMatchIdx]);
            else
                startMatchHistory = new TVersionKey[] { startOriginHistory[startMatchIdx] };

            TVersionKey[] endMatchHistory;
            if (startMatchIdx > 0)
                endMatchHistory = GetHistory(endOriginHistory[endMatchIdx - 1],
                                                         endOriginHistory[endMatchIdx]);
            else
                endMatchHistory = new TVersionKey[] { endOriginHistory[endMatchIdx] };

            TVersionKey matchVersion = FindCommonVersion(startMatchHistory, endMatchHistory);
            List<TVersionKey> reverseHistory = GetHistory(startVersion, matchVersion).ToList();
            reverseHistory.RemoveAt(reverseHistory.Count - 1);
            List<TVersionKey> forwardHistory = GetHistory(endVersion, matchVersion).Reverse().ToList();
            forwardHistory.RemoveAt(forwardHistory.Count - 1);

            reverseKeys = reverseHistory;
            forwardKeys = forwardHistory;
        }

        /// <summary>
        /// Gets the origin-based revision history for the given version.
        /// </summary>
        /// <remarks>
        /// The origin-based revision history consists version and origin version pairs in a linear sequence until the initial revision is reached.
        /// The history contains no duplications when the origin of a version is itself. The first element of the output array is the given version.
        /// </remarks>
        /// <param name="version">The version key to query.</param>
        /// <returns>The linear origin-based version history for the given version.</returns>
        protected TVersionKey[] GetOriginHistory(TVersionKey version)
        {
            var history = new List<TVersionKey>();
            RevisionDescriptor<TVersionKey> current = _revisionModel.GetRevision(version);
            do
            {
                history.Add(current.Version);
                current = current.OriginRevision;
                if(!current.Version.Equals(history.Last()))
                    history.Add(current.Version);

                current = current.PrecedingRevision;
            } while (current != null);
            return history.ToArray();
        }

        /// <summary>
        /// Find the first common version in two version key collections.
        /// </summary>
        /// <remarks>
        /// Because no order is defined between two version keys, the cost of this method is <c>O(N*M)</c>, where <c>N</c> and <c>M</c> are the length of the two collections.
        /// </remarks>
        /// <param name="collectionA">The first version key collection to enumerate.</param>
        /// <param name="collectionB">The second version key collection to enumerate.</param>
        /// <returns>The first common version in the two version key collections.</returns>
        /// <exception cref="ArgumentException">When none common version was found.</exception>
        protected TVersionKey FindCommonVersion(ICollection<TVersionKey> collectionA, ICollection<TVersionKey> collectionB)
        {
            foreach (TVersionKey versionA in collectionA)
                if (collectionB.Contains(versionA))
                    return versionA;
            throw new ArgumentException("The second collection does not contain any key from the first one.", "collectionB");
        }

        /// <summary>
        /// Gets the closest version which has a snapshot stored and forward patching can produce the given version's state from it.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <param name="maxDistance">The maximum distance to search for a snapshot.</param>
        /// <returns>The found snapshot's version key, or null if none found - until the maximum distance was reached.</returns>
        protected TVersionKey GetSnapshotForward(TVersionKey version, Int32 maxDistance = Int32.MaxValue)
        {
            Int32 distance;
            return GetSnapshotForward(version, out distance, maxDistance);
        }

        /// <summary>
        /// Gets the closest version which has a snapshot stored and forward patching can produce the given version's state from it.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <param name="distance">The distance between the given version and the snapshot's version.</param>
        /// <param name="maxDistance">The maximum distance to search for a snapshot.</param>
        /// <returns>The found snapshot's version key, or null if none found - until the maximum distance was reached.</returns>
        protected TVersionKey GetSnapshotForward(TVersionKey version, out Int32 distance, Int32 maxDistance = Int32.MaxValue)
        {
            distance = 0;
            RevisionDescriptor<TVersionKey> current = _revisionModel.GetRevision(version);
            do
            {
                if (_storage.HasSnapshot(current.Version))
                    return current.Version;

                current = current.PrecedingRevision;
                ++distance;
            } while (current != null && distance < maxDistance);
            return default(TVersionKey);
        }

        /// <summary>
        /// Gets the closest version which has a snapshot stored and reverse patching can produce the given version's state from it.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <param name="maxDistance">The maximum distance to search for a snapshot.</param>
        /// <returns>The found snapshot's version key, or null if none found - until the maximum distance was reached.</returns>
        protected TVersionKey GetSnapshotReverse(TVersionKey version, Int32 maxDistance = Int32.MaxValue)
        {
            Int32 distance;
            return GetSnapshotReverse(version, out distance, maxDistance);
        }

        /// <summary>
        /// Gets the closest version which has a snapshot stored and reverse patching can produce the given version's state from it.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <param name="distance">The distance between the given version and the snapshot's version.</param>
        /// <param name="maxDistance">The maximum distance to search for a snapshot.</param>
        /// <returns>The found snapshot's version key, or null if none found - until the maximum distance was reached.</returns>
        protected TVersionKey GetSnapshotReverse(TVersionKey version, out Int32 distance, Int32 maxDistance = Int32.MaxValue)
        {
            var currentRound = new Queue<RevisionDescriptor<TVersionKey>>();
            var nextRound = new Queue<RevisionDescriptor<TVersionKey>>();

            distance = 0;
            currentRound.Enqueue(_revisionModel.GetRevision(version));
            do
            {
                RevisionDescriptor<TVersionKey> current = currentRound.Dequeue();
                if (_storage.HasSnapshot(current.Version))
                    return current.Version;

                foreach (RevisionDescriptor<TVersionKey> nextVersion in
                        current.SucceedingRevisions.Where(succRevision => succRevision.PrecedingRevision.Version.Equals(current.Version)))
                    nextRound.Enqueue(nextVersion);

                if (currentRound.Count == 0)
                {
                    currentRound = nextRound;
                    nextRound = new Queue<RevisionDescriptor<TVersionKey>>();
                    ++distance;
                }
            } while (distance < maxDistance && currentRound.Count > 0);
            return default(TVersionKey);
        }

        /// <summary>
        /// Gets the closest version which has a snapshot stored and patching can produce the given version's state from it.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <param name="distance">The distance between the given version and the snapshot's version.</param>
        /// <param name="maxDistance">The maximum distance to search for a snapshot.</param>
        /// <returns>The found snapshot's version key, or null if none found - until the maximum distance was reached.</returns>
        protected TVersionKey GetSnapshot(TVersionKey version, out Int32 distance, Int32 maxDistance = Int32.MaxValue)
        {
            var currentRound = new Queue<RevisionDescriptor<TVersionKey>>();
            var nextRound = new Queue<RevisionDescriptor<TVersionKey>>();

            distance = 0;
            RevisionDescriptor<TVersionKey> revision = _revisionModel.GetRevision(version);
            currentRound.Enqueue(revision);
            do
            {
                RevisionDescriptor<TVersionKey> current = currentRound.Dequeue();
                if (_storage.HasSnapshot(current.Version))
                    return current.Version;

                foreach (RevisionDescriptor<TVersionKey> nextVersion in
                        current.SucceedingRevisions.Where(succRevision => succRevision.PrecedingRevision.Version.Equals(current.Version)))
                    nextRound.Enqueue(nextVersion);

                if (currentRound.Count == 0)
                {
                    currentRound = nextRound;
                    nextRound = new Queue<RevisionDescriptor<TVersionKey>>();
                    ++distance;

                    if (revision.PrecedingRevision == null)
                        continue;
                    if (_storage.HasSnapshot(revision.PrecedingRevision.Version))
                        return revision.PrecedingRevision.Version;

                    // ReSharper disable AccessToModifiedClosure
                    foreach (RevisionDescriptor<TVersionKey> nextVersion in
                        revision.PrecedingRevision.SucceedingRevisions.Where(
                            succRevision =>
                                succRevision.PrecedingRevision.Version.Equals(revision.PrecedingRevision.Version) &&
                                    !succRevision.Version.Equals(revision.Version)))
                        nextRound.Enqueue(nextVersion);
                    // ReSharper restore AccessToModifiedClosure

                    revision = revision.PrecedingRevision;
                }
            } while (distance < maxDistance && currentRound.Count > 0);
            return default(TVersionKey);
        }

        #endregion
    }
}
