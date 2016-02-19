/// <copyright file="OrdinalReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an ordinal temporal reference system.
    /// </summary>
    public class OrdinalReferenceSystem : TemporalReferenceSystem
    {
        #region Protected fields

        protected List<OrdinalEra> _eras;
        protected IReferenceSystem _beginEndReferenceSystem;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the eras of the reference system.
        /// </summary>
        /// <value>A read-only list containing the eras of the reference system.</value>
        public IList<OrdinalEra> Eras { get { return _eras.AsReadOnly(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public OrdinalReferenceSystem(String identifier, String name)
            : this(identifier, name, null, null, null)
        {
            _eras = new List<OrdinalEra>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdinalReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected OrdinalReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope) 
        {
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
        public void AddEra(String identifier, String name, DateAndTime begin, Positioning.DateAndTime end)
        {
            if (_beginEndReferenceSystem == null)
                _beginEndReferenceSystem = begin.ReferenceSystem;

            if (!_beginEndReferenceSystem.Equals(begin.ReferenceSystem))
                throw new ArgumentException("The begining date has a different reference system to the eras.", "begin");
            if (!_beginEndReferenceSystem.Equals(end.ReferenceSystem))
                throw new ArgumentException("The ending date has a different reference system to the eras.", "end");

            _eras.Add(new OrdinalEra(identifier, name, this, begin, end));
        }        

        /// <summary>
        /// Returns the era for a specified date/time.
        /// </summary>
        /// <param name="dateTime">The date/time.</param>
        /// <returns>The era of <paramref name="dateTime" />.</returns>
        /// <exception cref="System.ArgumentException">The specified date/time has a different reference system to the eras.</exception>
        public OrdinalEra GetEra(DateAndTime dateTime)
        {
            if (!_beginEndReferenceSystem.Equals(dateTime.ReferenceSystem))
                throw new ArgumentException("The specified date/time has a different reference system to the eras.", "dateTime");

            for (Int32 i = 0; i < _eras.Count; i++)
            {
                if (dateTime.Instant >= _eras[i].Begin.Instant && dateTime.Instant <= _eras[i].End.Instant)
                    return _eras[i];
            }
            return null;
        }

        /// <summary>
        /// Returns the era for a specified instant.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>The era of <paramref name="instant" />.</returns>
        public OrdinalEra GetEra(Instant instant)
        {            
            for (Int32 i = 0; i < _eras.Count; i++)
            {
                if (instant >= _eras[i].Begin.Instant && instant <= _eras[i].End.Instant)
                    return _eras[i];
            }
            return null;
        }

        #endregion
    }
}
