/// <copyright file="CoordinateProjection.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a coordinate projection.
    /// </summary>
    public abstract class CoordinateProjection : CoordinateConversion<GeoCoordinate, Coordinate>
    {
        #region Protected fields

        protected readonly Ellipsoid _ellipsoid;
        protected readonly AreaOfUse _areaOfUse;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid used by the operation.</value>
        public Ellipsoid Ellipsoid { get { return _ellipsoid; } }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the operation is applicable.</value>
        public AreaOfUse AreaOfUse { get { return _areaOfUse; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// or
        /// The ellipsoid is null.
        /// or
        /// The area of use is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public CoordinateProjection(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");

            if (areaOfUse == null)
                throw new ArgumentNullException("areaOfUse", "The area of use is null.");

            if (parameters != null)
            {
                foreach (CoordinateOperationParameter parameter in parameters.Keys)
                {
                    if ((parameters[parameter] is Length) && ((Length)parameters[parameter]).Unit != ellipsoid.SemiMajorAxis.Unit)
                        throw new ArgumentException("The parameter '" + parameter.Name + "' does not have the same measurement unit as the ellipsoid.", "parameters");
                }
            }

            _areaOfUse = areaOfUse;
            _ellipsoid = ellipsoid;
        }

        #endregion
    }
}
