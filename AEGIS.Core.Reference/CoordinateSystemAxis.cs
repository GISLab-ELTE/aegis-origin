/// <copyright file="CoordinateSystemAxis.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a coordinate system axis.
    /// </summary>
    public class CoordinateSystemAxis : IdentifiedObject
    {
        #region Private fields

        private AxisDirection _direction;
        private UnitOfMeasurement _unit;
        private Double _minimum;
        private Double _maximum;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the direction of the axis.
        /// </summary>
        /// <value>Direction of this coordinate system axis (or in the case of Cartesian projected coordinates, the direction of this coordinate system axis locally).</value>
        public AxisDirection Direction { get { return _direction; } }

        /// <summary>
        /// Gets the unit of measurement.
        /// </summary>
        /// <value>The unit of measurement.</value>
        public UnitOfMeasurement Unit { get { return _unit; } }

        /// <summary>
        /// Gets the minimum value of the axis.
        /// </summary>
        /// <value>The minimum value normally allowed for this axis, in the unit for the axis.</value>
        public Double Minimum { get { return _minimum; } }

        /// <summary>
        /// Gets the maximum value of the axis.
        /// </summary>
        /// <value>The maximum value normally allowed for this axis, in the unit for the axis.</value>
        public Double Maximum { get { return _maximum; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemAxis" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateSystemAxis(String identifier, String name, AxisDirection direction, UnitOfMeasurement unit) 
            : base(identifier, name)
        {
            _direction = direction;
            _unit = unit;

            _minimum = Double.NegativeInfinity;
            _maximum = Double.PositiveInfinity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystemAxis" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction of the axis.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <param name="minimum">The minimum value of the axis.</param>
        /// <param name="maximum">The maximum value of the axis.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">The minimum value must be less than the maximum value.</exception>
        public CoordinateSystemAxis(String identifier, String name, AxisDirection direction, UnitOfMeasurement unit, Double minimum, Double maximum)
            : this(identifier, name, direction, unit)
        {
            if (maximum <= minimum)
                throw new ArgumentException("The minimum value must be less than the maximum value.", "minimum");

            _minimum = minimum;
            _maximum = maximum;
        }

        #endregion
    }
}
