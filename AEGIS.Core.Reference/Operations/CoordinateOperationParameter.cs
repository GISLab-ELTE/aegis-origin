/// <copyright file="CoordinateOperationParameter.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a coordinate operation parameter.
    /// </summary>
    public class CoordinateOperationParameter : IdentifiedObject
    {
        #region Private fields

        private readonly UnitQuantityType _unitType;
        private readonly String _description;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the quantity type of the measurement unit.
        /// </summary>
        /// <value>The quantity type of the measurement unit.</value>
        public UnitQuantityType UnitType { get { return _unitType; } }

        /// <summary>
        /// Gets the description of the parameter.
        /// </summary>
        /// <value>The description of the parameter.</value>
        public String Description { get { return _description; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationParameter" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="unitType">The quantity type of the unit.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationParameter(String identifier, String name, UnitQuantityType unitType, String description)
            : this(identifier, name, null, null, unitType, description)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationParameter" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="unitType">The quantity type of the unit.</param>
        /// <param name="description">The description.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationParameter(String identifier, String name, String remarks, String[] aliases, UnitQuantityType unitType, String description)
            : base(identifier, name, remarks, aliases)
        {
            _description = description;
            _unitType = unitType;
        }

        #endregion
    }
}
