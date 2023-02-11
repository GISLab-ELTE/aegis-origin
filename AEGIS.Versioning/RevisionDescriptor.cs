// <copyright file="RevisionDescriptor.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a revision node in the versioning model.
    /// </summary>
    /// <typeparam name="TVersionKey">The versioning key type of the descriptor.</typeparam>
    /// <author>Máté Cserép</author>
    public class RevisionDescriptor<TVersionKey>
    {
        #region Protected fields

        /// <summary>
        /// Stores the <see cref="RevisionDescriptor{TVersionKey}"/> objects for the revisions that succeeds the current revision.
        /// </summary>
        /// <remarks>
        /// The current revision is either the <see cref="PrecedingRevision"/> or one of the <see cref="MergedRevisions"/> for all the revisions stored in this field.
        /// </remarks>
        protected ISet<RevisionDescriptor<TVersionKey>> _succeedingRevisions = new HashSet<RevisionDescriptor<TVersionKey>>();

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the version key for the current revision.
        /// </summary>
        public TVersionKey Version { get; protected set; }

        /// <summary>
        /// Gets the origin revision of the branch of the current revision.
        /// </summary>
        public RevisionDescriptor<TVersionKey> OriginRevision { get; protected set; }

        /// <summary>
        /// Gets the preceding revision of the current revision.
        /// </summary>
        /// <remarks>
        /// The preceding revision is the revision from all the <see cref="PrecedingRevisions"/> on which the current revision's alternations were made.
        /// </remarks>
        public RevisionDescriptor<TVersionKey> PrecedingRevision { get; protected set; }

        /// <summary>
        /// Gets the revisions that were merged with the <see cref="PrecedingRevision"/> to create the current revision.
        /// </summary>
        public RevisionDescriptor<TVersionKey>[] MergedRevisions { get; protected set; }

        /// <summary>
        /// Gets the revisions that succeeds the current revision.
        /// </summary>
        public RevisionDescriptor<TVersionKey>[] SucceedingRevisions
        {
            get { return _succeedingRevisions.ToArray(); }
        }

        /// <summary>
        /// Gets all the revisions that precedes the current revision.
        /// </summary>
        /// <remarks>
        /// This property contains the values of both the <see cref="PrecedingRevision"/> and the <see cref="MergedRevisions"/> properties.
        /// </remarks>
        public RevisionDescriptor<TVersionKey>[] PrecedingRevisions
        {
            get
            {
                var precedingRevisions = new RevisionDescriptor<TVersionKey>[MergedRevisions.Length + (PrecedingRevision == null ? 0 : 1)];

                if (PrecedingRevision != null)
                {
                    precedingRevisions[0] = PrecedingRevision;
                    if (MergedRevisions.Length > 0)
                        MergedRevisions.CopyTo(precedingRevisions, 1);
                }
                return precedingRevisions;
            }
        }

        /// <summary>
        /// Gets whether the current revision is the head revision of its own branch.
        /// </summary>
        public Boolean IsHead { get; protected set; }

        /// <summary>
        /// Gets whether the current revision is the origin revision of its own branch.
        /// </summary>
        public Boolean IsOrigin
        {
            get { return OriginRevision.Version.Equals(Version); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an instance of the <see cref="RevisionDescriptor{TVersionKey}"/> class.
        /// </summary>
        /// <param name="version">The version of the revision.</param>
        /// <param name="precedingRevision">The revision preceding the current revision.</param>
        /// <param name="mergedRevisions">The other revisions beside the preceding revision to be merged in the current revision.</param>
        public RevisionDescriptor(
            TVersionKey version,
            RevisionDescriptor<TVersionKey> precedingRevision,
            ICollection<RevisionDescriptor<TVersionKey>> mergedRevisions = null)
        {
            if (version.Equals(default(TVersionKey)))
                throw new ArgumentNullException("version", "The version key cannot be null or the default value.");
            if (mergedRevisions == null)
                mergedRevisions = new RevisionDescriptor<TVersionKey>[0];
            if (precedingRevision == null && mergedRevisions.Count > 0)
                throw new ArgumentException("A preceding revision must be given when further merge parent revisions are set.",
                                            "precedingRevision");

            Version = version;
            PrecedingRevision = precedingRevision;
            MergedRevisions = mergedRevisions.ToArray();
            OriginRevision = precedingRevision == null || !precedingRevision.IsHead ? this : precedingRevision.OriginRevision;
            IsHead = true;

            if (precedingRevision != null)
                precedingRevision.AddSucceedingRevision(this);
            foreach (RevisionDescriptor<TVersionKey> mergedRevision in mergedRevisions)
                mergedRevision.AddSucceedingRevision(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Add a succeeding revision to the current revision.
        /// </summary>
        /// <param name="revision">The succeeding revision to add.</param>
        /// <returns>True, if the given revision wasn't added before; otherwise, false.</returns>
        protected Boolean AddSucceedingRevision(RevisionDescriptor<TVersionKey> revision)
        {
            if (revision == null)
                throw new ArgumentNullException("revision", "The branch revision to add cannot be null.");

            if (revision.PrecedingRevision.Version.Equals(Version))
                IsHead = false;
            return _succeedingRevisions.Add(revision);
        }

        #endregion
    }
}
