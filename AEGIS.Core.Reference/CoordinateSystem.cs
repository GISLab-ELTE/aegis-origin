/// <copyright file="CoordinateSystem.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a coordinate system.
    /// </summary>
    /// <remarks>
    /// A coordinate system (CS) is the non-repeating sequence of coordinate system axes that spans a given coordinate space. 
    /// A CS is derived from a set of mathematical rules for specifying how coordinates in a given space are to be assigned to points. 
    /// The coordinate values in a coordinate tuple shall be recorded in the order in which the coordinate system axes associations are recorded.
    /// </remarks>
    public class CoordinateSystem : IdentifiedObject
    {
        #region Private fields

        private readonly CoordinateSystemAxis[] _axis;
        private readonly CoordinateSystemType _type;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the dimension of the coordinate system.
        /// </summary>
        /// <value>The dimension of the coordinate system.</value>
        public Int32 Dimension { get { return _axis.Length; } }

        /// <summary>
        /// Gets the type of the coordinate system.
        /// </summary>
        /// <value>The type of the coordinate system.</value>
        public CoordinateSystemType Type { get { return _type; } }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> at the specified index.
        /// </summary>
        /// <value>The <see cref="CoordinateSystemAxis" /> at the specified index.</value>
        /// <param name="index">The index of the axis.</param>
        /// <returns>The axis at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is greater than or equal to the number of axis.
        /// </exception>
        public CoordinateSystemAxis this[Int32 index] { get { return GetAxis(index); } }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> with the specified name.
        /// </summary>
        /// <value>The <see cref="CoordinateSystemAxis" /> with the specified name.</value>
        /// <param name="name">The name of the axis.</param>
        /// <returns>The axis with the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The axis name is null.</exception>
        /// <exception cref="System.ArgumentException">The coordinate system does not contain an axis with the specified name.</exception>
        public CoordinateSystemAxis this[String name] { get { return GetAxis(name); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <param name="axis">The axis.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// No axis is specified for the coordinate system.
        /// </exception>
        /// <exception cref="System.ArgumentException">No axis is specified for the coordinate system.</exception>
        public CoordinateSystem(String identifier, CoordinateSystemType type, params CoordinateSystemAxis[] axis) : base(identifier, null)
        {
            if (axis == null)
                throw new ArgumentNullException("axis", "No axis is specified for the coordinate system.");
            if (axis.Length < 1)
                throw new ArgumentException("No axis is specified for the coordinate system.", "axis");

            _axis = axis;

            Name = Type.ToString() + " " + _axis.Length + "D CS. Axis: " +
                   _axis.Select(ax => ax.Name).Aggregate((x, y) => x + ", " + y) + ". Orientation: " +
                   _axis.Select(ax => ax.Direction.ToString()).Aggregate((x, y) => x + ", " + y) + ". UoM: " +
                   _axis.Select(ax => ax.Unit.ToString()).Aggregate((x, y) => x + ", " + y) + ".";
            _type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="axis">The axis.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// No axis is specified for the coordinate system.
        /// </exception>
        /// <exception cref="System.ArgumentException">No axis is specified for the coordinate system.</exception>
        public CoordinateSystem(String identifier, String name, CoordinateSystemType type, params CoordinateSystemAxis[] axis)
            : base(identifier, name)
        {
            if (axis == null)
                throw new ArgumentNullException("axis", "No axis is specified for the coordinate system.");
            if (axis.Length < 1)
                throw new ArgumentException("No axis is specified for the coordinate system.", "axis");

            _type = type;
            _axis = axis;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> at the specified index.
        /// </summary>
        /// <param name="index">The index of the axis.</param>
        /// <returns>The axis at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index is less than 0.
        /// or
        /// The index is greater than or equal to the number of axis.
        /// </exception>
        public CoordinateSystemAxis GetAxis(Int32 index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "The index is less than 0.");
            if (index >= _axis.Length)
                throw new ArgumentOutOfRangeException("index", "The index is greater than or equal to the number of axis.");

            return _axis[index];
        }

        /// <summary>
        /// Gets the <see cref="CoordinateSystemAxis" /> with the specified name.
        /// </summary>
        /// <param name="name">The name of the axis.</param>
        /// <returns>The axis with the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">The axis name is null.</exception>
        /// <exception cref="System.ArgumentException">The coordinate system does not contain an axis with the specified name.</exception>
        public CoordinateSystemAxis GetAxis(String name)
        {
            if (name == null)
                throw new ArgumentNullException("name", "The axis name is null.");

            CoordinateSystemAxis axis = _axis.Where(ax => ax.Name == name).FirstOrDefault();
            if (axis != null)
                return axis;
            else
                throw new ArgumentException("The coordinate system does not contain an axis with the specified name.", "name");
        }

        #endregion
    }
}
