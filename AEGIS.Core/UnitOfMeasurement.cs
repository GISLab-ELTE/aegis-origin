/// <copyright file="UnitOfMeasurement.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a unit of measurement (UOM).
    /// </summary>
    [Serializable]
    public class UnitOfMeasurement : IdentifiedObject
    {
        #region Private fields

        /// <summary>
        /// The symbol. This field is read-only.
        /// </summary>
        private readonly String _symbol;

        /// <summary>
        /// The multiple from the SI base unit. This field is read-only.
        /// </summary>
        private readonly Double _baseMultiple;

        /// <summary>
        /// The type of the unit. This field is read-only.
        /// </summary>
        private readonly UnitQuantityType _type;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the symbol.
        /// </summary>
        /// <value>The <see cref="T:System.String"/> containing the symbol of the unit.</value>
        public String Symbol { get { return _symbol; } }

        /// <summary>
        /// Gets the multiple from the SI base unit.
        /// </summary>
        /// <value>The value that is used as multiple when computing the SI base value.</value>
        public Double BaseMultiple { get { return _baseMultiple; } }

        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        /// <value>The type of the unit.</value>
        public UnitQuantityType Type { get { return _type; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfMeasurement" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="baseMultiple">The multiple from the SI base unit.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public UnitOfMeasurement(String identifier, String name, String symbol, Double baseMultiple, UnitQuantityType type) : 
            this(identifier, name, null, null, symbol, baseMultiple, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfMeasurement" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="symbol">The symbol.</param>
        /// <param name="baseMultiple">The multiple from the SI base unit.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The base multiple is 0.</exception>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public UnitOfMeasurement(String identifier, String name, String remarks, String[] aliases, String symbol, Double baseMultiple, UnitQuantityType type)
            : base(identifier, name, remarks, aliases)
        {
            if (baseMultiple == 0)
                throw new ArgumentOutOfRangeException("The base multiple is 0.", "baseMultiple");

            _symbol = symbol;
            _baseMultiple = baseMultiple;
            _type = type;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that contains both identifier, name and symbol of the current unit of measurement.</returns>
        public override String ToString()
        {
            return base.ToString() + " (" + Symbol + ")";
        }

        #endregion
    }
}
