/// <copyright file="ProjectedCoordinateReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
using System.Linq;
using ELTE.AEGIS.Reference.Operations;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a projected coordinate reference system.
    /// </summary>
    /// <remarks>
    /// A derived coordinate reference system which has a geodetic coordinate reference system as its base CRS and is converted using a map projection.
    /// </remarks>
    [Serializable]
    public class ProjectedCoordinateReferenceSystem : CoordinateReferenceSystem
    {
        #region Private fields

        private readonly GeographicCoordinateReferenceSystem _baseReferenceSystem;
        private readonly CoordinateProjection _projection;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the base geodetic coordinate reference system.
        /// </summary>
        /// <value>The geodetic coordinate reference system </value>
        public GeographicCoordinateReferenceSystem Base { get { return _baseReferenceSystem; } }

        /// <summary>
        /// Gets the coordinate projection used by the reference system.
        /// </summary>
        /// <value>The coordinate projection used by the reference system.</value>
        public CoordinateProjection Projection { get { return _projection; } }

        #endregion

        #region ReferenceSystem properties

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Projected; } }

        #endregion

        #region CoordinateReferenceSystem Properties

        /// <summary>
        /// Gets the datum of the coordinate reference system.
        /// </summary>
        /// <value>The datum of the coordinate reference system.</value>
        public new GeodeticDatum Datum
        {
            get { return base.Datum as GeodeticDatum; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectedCoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="baseReferenceSystem">The base reference system.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The base reference system is null.
        /// or
        /// The coordinate system is null.
        /// </exception>
        public ProjectedCoordinateReferenceSystem(String identifier, String name, GeographicCoordinateReferenceSystem baseReferenceSystem, CoordinateSystem coordinateSystem, AreaOfUse areaOfUse, CoordinateProjection projection)
            : this(identifier, name, null, null, null, baseReferenceSystem, coordinateSystem, areaOfUse, projection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectedCoordinateReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="baseReferenceSystem">The base reference system.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The base reference system is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The projection is null.
        /// </exception>
        public ProjectedCoordinateReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, GeographicCoordinateReferenceSystem baseReferenceSystem, CoordinateSystem coordinateSystem, AreaOfUse areaOfUse, CoordinateProjection projection)
            : base(identifier, name, remarks, aliases, scope, coordinateSystem, baseReferenceSystem != null ? baseReferenceSystem.Datum : null, areaOfUse)
        {
            if (baseReferenceSystem == null)
                throw new ArgumentNullException("baseReferenceSystem", "The base reference system is null.");
            
            _baseReferenceSystem = baseReferenceSystem;
            _projection = projection;
        }

        #endregion
    }
}
