/// <copyright file="GridReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;
using ELTE.AEGIS.Reference.Operations;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a grid reference system.
    /// </summary>
    public class GridReferenceSystem : CoordinateReferenceSystem
    {
        #region Private fields

        private readonly GridProjection _projection;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the coordinate projection used by the reference system.
        /// </summary>
        /// <value>The coordinate projection used by the reference system.</value>
        public GridProjection Projection { get { return _projection; } }

        #endregion

        #region ReferenceSystem properties

        /// <summary>
        /// Gets the type of the coordinate reference system.
        /// </summary>
        /// <value>The type of the coordinate reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Grid; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GridReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="datum">The datum.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The projection is null.
        /// </exception>
        public GridReferenceSystem(String identifier, String name, CoordinateSystem coordinateSystem, Datum datum, AreaOfUse areaOfUse, GridProjection projection)
            : this(identifier, name, null, null, null, coordinateSystem, datum, areaOfUse, projection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        /// <param name="datum">The datum.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The coordinate system is null.
        /// or
        /// The projection is null.
        /// </exception>
        public GridReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, CoordinateSystem coordinateSystem, Datum datum, AreaOfUse areaOfUse, GridProjection projection)
            : base(identifier, name, remarks, aliases, scope, coordinateSystem, datum, areaOfUse)
        {
            if (projection == null)
                throw new ArgumentNullException("projection", "The projection is null.");

            _projection = projection;
        }

        #endregion
    }
}
