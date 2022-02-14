/// <copyright file="Datum.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a datum.
    /// </summary>
    /// <remarks>
    /// A datum specifies the relationship of a coordinate system to an object, thus creating a coordinate reference system. 
    /// For geodetic and vertical coordinate reference systems, the datum relates the coordinate system to the Earth. 
    /// With other types of coordinate reference systems, the datum may relate the coordinate system to another physical or virtual object. 
    /// A datum uses a parameter or set of parameters that determine the location of the origin of the coordinate reference system. 
    /// Each datum subtype can be associated with only specific types of coordinate reference systems.
    /// </remarks>
    public abstract class Datum : IdentifiedObject
    {
        #region Private fields

        private readonly String _anchorPoint;
        private readonly String _realizationEpoch;
        private readonly String _scope;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the anchor point of the datum.
        /// </summary>
        /// <value>A description, possibly including coordinates of an identified point or points, of the relationship used to anchor the coordinate system to the Earth or alternate object.</value>
        public virtual String AnchorPoint { get { return _anchorPoint; } }

        /// <summary>
        /// Gets the realization epoch of the datum.
        /// </summary>
        /// <value>The time after which this datum definition is valid.</value>
        public virtual String RealizationEpoch { get { return _realizationEpoch; } }

        /// <summary>
        /// Gets the scope of the datum.
        /// </summary>
        /// <value>Description of usage, or limitations of usage, for which this datum is valid.</value>
        public String Scope { get { return _scope; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Datum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected Datum(String identifier, String name, String anchorPoint, String realizationEpoch)
            : this(identifier, name, null, null, anchorPoint, realizationEpoch, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Datum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <param name="scope">The scope.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected Datum(String identifier, String name, String remarks, String[] aliases, String anchorPoint, String realizationEpoch, String scope)
            : base(identifier, name)
        {
            _anchorPoint = anchorPoint ?? String.Empty;
            _realizationEpoch = realizationEpoch ?? String.Empty;
            _scope = scope ?? String.Empty;
        }

        #endregion
    }
}
