/// <copyright file="OrdinalEra.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Temporal.Positioning;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents an ordinal era.
    /// </summary>
    public class OrdinalEra : IdentifiedObject
    {
        #region Protected fields

        protected OrdinalEra _group;
        protected List<OrdinalEra> _eras;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the reference system associated with the era.
        /// </summary>
        /// <value>The reference system associated with the era.</value>
        public OrdinalReferenceSystem ReferenceSystem { get; private set; }

        /// <summary>
        /// Gets the begin of the era.
        /// </summary>
        /// <value>The begin of the erad.</value>
        public DateAndTime Begin { get; private set; }

        /// <summary>
        /// Gets the end of the era.
        /// </summary>
        /// <value>The end of the erad.</value>
        public DateAndTime End { get; private set; }

        /// <summary>
        /// Gets the group of the era.
        /// </summary>
        /// <value>The ordinal era this instance belongs to.</value>
        public OrdinalEra Group { get { return _group; } }
        /// <summary>
        /// Gets the member eras.
        /// </summary>
        /// <value>A read-only list containing the member eras of this instance.</value>
        public IList<OrdinalEra> Eras { get { return _eras.AsReadOnly(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalEra" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        public OrdinalEra(String identifier, String name, OrdinalReferenceSystem referenceSystem) : base(identifier, name)
        {
            _group = null;
            _eras = new List<OrdinalEra>();
            ReferenceSystem = referenceSystem;
            Begin = End = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalEra" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="begin">The begin of the era.</param>
        /// <param name="end">The end of the era.</param>
        public OrdinalEra(String identifier, String name, OrdinalReferenceSystem referenceSystem, DateAndTime begin, DateAndTime end)
            : base(identifier, name)
        {
            _group = null;
            _eras = new List<OrdinalEra>();
            ReferenceSystem = referenceSystem;
            Begin = begin;
            End = end;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an era.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="begin">The beginning date.</param>
        /// <param name="end">The ending date.</param>
        /// <exception cref="System.ArgumentException">
        /// The begining date has a different reference system to the eras.
        /// or
        /// The ending date has a different reference system to the eras.
        /// </exception>
        public void AddEra(String identifier, String name, Positioning.DateAndTime begin, Positioning.DateAndTime end)
        {
            if (!Begin.ReferenceSystem.Equals(begin.ReferenceSystem))
                throw new ArgumentException("The begining date has a different reference system to the era.", "begin");
            if (!Begin.ReferenceSystem.Equals(end.ReferenceSystem))
                throw new ArgumentException("The ending date has a different reference system to the era.", "end");

            OrdinalEra era = new OrdinalEra(identifier, name, this.ReferenceSystem, begin, end);
            era._group = this;
            _eras.Add(era);
        }

        /// <summary>
        /// Adds an era.
        /// </summary>
        /// <param name="era">The era.</param>
        /// <exception cref="System.ArgumentNullException">The era is null.</exception>
        /// <exception cref="System.ArgumentException">The era is already in a group.</exception>
        public void AddEra(OrdinalEra era)
        {
            if (era == null)
                throw new ArgumentNullException("era", "The era is null.");
            if (era._group != null)
                throw new ArgumentException("The era is already in a group.", "era");

            era._group = this;
            _eras.Add(era);
        }

        /// <summary>
        /// Determines whether the specified era is a member of this instance.
        /// </summary>
        /// <param name="era">The era.</param>
        /// <returns><c>true</c> if <paramref name="era" /> is a member; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The era is null.</exception>
        public Boolean IsMember(OrdinalEra era)
        {
            if (era == null)
                throw new ArgumentNullException("era", "The era is null.");

            return era._group == this;
        }

        #endregion
    }
}
