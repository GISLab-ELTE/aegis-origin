/// <copyright file="TemporalPosition.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Metadata;
using ELTE.AEGIS.Temporal.Reference;
using System;

namespace ELTE.AEGIS.Temporal.Positioning
{
    /// <summary>
    /// Represents a position in a specified temporal reference system.
    /// </summary>
    public abstract class TemporalPosition : ITemporalPosition
    {
        #region Private fields

        private TemporalReferenceSystem _referenceSystem;
        private IMetadataCollection _metadata;

        #endregion

        #region Protected fields

        protected Instant _instant;

        #endregion

        #region ITemporalPosition properties

        /// <summary>
        /// Gets the instant associated with the temporal position.
        /// </summary>
        /// <value>The instant associated with the temporal position.</value>
        public virtual Instant Instant 
        { 
            get { return _instant; }
            set 
            {
                if (_instant != value)
                {
                    _instant = value;
                    OnPositionChanged();
                }
            }
        }

        /// <summary>
        /// Gets the reference system of the <see cref="TemporalPosition" />.
        /// </summary>
        /// <value>The reference system of the temporal position.</value>
        public IReferenceSystem ReferenceSystem { get { return _referenceSystem; } }

        #endregion

        #region IMetadataProvider properties

        /// <summary>
        /// Gets the metadata container.
        /// </summary>
        public IMetadataCollection Metadata { get { return _metadata; } }

        /// <summary>
        /// Gets or sets the metadata value for a specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The metadata value with the <paramref name="key" /> if it exists; otherwise, <c>null</c>.</returns>
        public Object this[String key]
        {
            get
            {
                Object value = null;
                if (_metadata != null)
                    _metadata.TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (_metadata == null)
                    _metadata = new MetadataFactory().CreateCollection();
                _metadata[key] = value;
            }
        }

        #endregion

        #region ITemporalPosition events

        /// <summary>
        /// Occurs when the temporal position is changed.
        /// </summary>
        public event EventHandler PositionChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporalPosition" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        protected TemporalPosition(TemporalReferenceSystem referenceSystem, IMetadataCollection metadata)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException("referenceSystem", "The reference system is null.");

            _referenceSystem = referenceSystem;
            _metadata = metadata;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporalPosition" /> class.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <param name="referenceSystem">The reference system.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">The reference system is null.</exception>
        protected TemporalPosition(Instant instant, TemporalReferenceSystem referenceSystem, IMetadataCollection metadata)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException("referenceSystem", "The reference system is null.");

            _instant = instant;
            _referenceSystem = referenceSystem;
            _metadata = metadata;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> representation of this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> representation of this instance.</returns>
        public override String ToString()
        {
            return "Pos " + _instant.ToString() + "";
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Called when the temporal position is changed.
        /// </summary>
        protected void OnPositionChanged()
        {
            if (PositionChanged != null)
                PositionChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
