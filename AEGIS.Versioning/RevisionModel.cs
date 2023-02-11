// <copyright file="RevisionModel.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the revision model for a versioning repository.
    /// </summary>
    /// <typeparam name="TVersionKey">The versioning key type of the model.</typeparam>
    /// <author>Máté Cserép</author>
    public class RevisionModel<TVersionKey>
    {
        #region Protected fields

        /// <summary>
        /// Factory object for creating versioning keys.
        /// </summary>
        protected IKeyFactory<TVersionKey> _keyFactory;

        /// <summary>
        /// Stores the mapping between the version keys and their revision descriptors.
        /// </summary>
        protected IDictionary<TVersionKey, RevisionDescriptor<TVersionKey>> _revisions;

        /// <summary>
        /// Stores the mapping between the branches' origin and head versions.
        /// </summary>
        protected IDictionary<TVersionKey, TVersionKey> _branchHeadVersions;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the initial revision of the revision model.
        /// </summary>
        /// <remarks>
        /// The initial revision is created automatically by creating an instance of the <see cref="RevisionModel{TVersionKey}"/> class.
        /// </remarks>
        public RevisionDescriptor<TVersionKey> InitRevision { get; protected set; }

        /// <summary>
        /// Gets the head revision of the revision model.
        /// </summary>
        /// <remarks>
        /// The head revision of a <see cref="RevisionModel{TVersionKey}"/> is the newest revision in it.
        /// </remarks>
        public RevisionDescriptor<TVersionKey> HeadRevision { get; protected set; }

        /// <summary>
        /// Gets the main revision of the revision model.
        /// </summary>
        /// <remarks>
        /// The main revision of a <see cref="RevisionModel{TVersionKey}"/> is the newest revision on the main - the <see cref="InitRevision"/>'s - branch.
        /// </remarks>
        public RevisionDescriptor<TVersionKey> MainRevision
        {
            get { return GetHeadRevision(InitRevision.Version); }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///Create a new instance of the <see cref="RevisionModel{TVersionKey}"/> class.
        /// </summary>
        /// <param name="keyFactory">The <see cref="IKeyFactory{TKey}"/> object to create the versioning keys.</param>
        public RevisionModel(IKeyFactory<TVersionKey> keyFactory)
        {
            if (keyFactory == null)
                throw new ArgumentNullException("keyFactory", "The key factory cannot be null.");
            _keyFactory = keyFactory;

            _revisions = new Dictionary<TVersionKey, RevisionDescriptor<TVersionKey>>
                {
                    {_keyFactory.LastKey, new RevisionDescriptor<TVersionKey>(_keyFactory.LastKey, null)}
                };
            _branchHeadVersions = new Dictionary<TVersionKey, TVersionKey>
                {
                    {_keyFactory.LastKey, _keyFactory.LastKey}
                };
            HeadRevision = InitRevision = _revisions[_keyFactory.LastKey];
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the revision descriptor for the given version key.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The revision descriptor belonging to the given version key.</returns>
        public RevisionDescriptor<TVersionKey> GetRevision(TVersionKey version)
        {
            if (version.Equals(default(TVersionKey)))
                throw new ArgumentNullException("version", "The version key cannot be null or the default value.");

            return _revisions[version];
        }

        /// <summary>
        /// Gets the head revision descriptor on the given version key's branch.
        /// </summary>
        /// <param name="version">The version key to query.</param>
        /// <returns>The head revision descriptor belonging to the given version key's branch.</returns>
        public RevisionDescriptor<TVersionKey> GetHeadRevision(TVersionKey version)
        {
            return _revisions[_branchHeadVersions[GetRevision(version).OriginRevision.Version]];
        }

        /// <summary>
        /// Creates and adds a new revision to the current revision model.
        /// </summary>
        /// <param name="precedingVersion">The version preceding the new revision.</param>
        /// <param name="mergedVersions">The versions to be merged with the preceding version to create the new revision.</param>
        /// <returns>The version key of the newly created revision.</returns>
        public TVersionKey CreateRevision(TVersionKey precedingVersion, ICollection<TVersionKey> mergedVersions = null)
        {
            if (precedingVersion.Equals(default(TVersionKey)))
                throw new ArgumentNullException("precedingVersion", "The preceding version key cannot be null or the default value.");

            RevisionDescriptor<TVersionKey> precedingRevision = _revisions[precedingVersion];
            RevisionDescriptor<TVersionKey>[] mergedRevisions = null;
            if (mergedVersions != null)
            {
                mergedRevisions = new RevisionDescriptor<TVersionKey>[mergedVersions.Count];
                Int32 idx = 0;
                foreach (TVersionKey mergedVersion in mergedVersions)
                {
                    mergedRevisions[idx] = GetRevision(mergedVersion);
                    ++idx;
                }
            }

            TVersionKey newVersion = _keyFactory.CreateKey();
            var newRevision = new RevisionDescriptor<TVersionKey>(newVersion, precedingRevision, mergedRevisions);
            _revisions.Add(newVersion, newRevision);

            if (!newRevision.IsOrigin)
                _branchHeadVersions[newRevision.OriginRevision.Version] = newVersion;
            else
                _branchHeadVersions[newVersion] = newVersion;
            HeadRevision = newRevision;

            return newVersion;
        }

        #endregion
    }
}
