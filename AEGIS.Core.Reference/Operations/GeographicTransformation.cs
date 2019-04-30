/// <copyright file="GeographicTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a geographic transformation.
    /// </summary>
    public abstract class GeographicTransformation : CoordinateTransformation<GeoCoordinate>
    {
        #region Private fields

        private readonly CoordinateReferenceSystem _source;
        private readonly CoordinateReferenceSystem _target;
        private readonly AreaOfUse _areaOfUse;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the source coordinate reference system.
        /// </summary>
        /// <value>The source coordinate reference system.</value>
        public CoordinateReferenceSystem Source { get { return _source; } }

        /// <summary>
        /// Gets the target coordinate reference system.
        /// </summary>
        /// <value>The target coordinate reference system.</value>
        public CoordinateReferenceSystem Target { get { return _target; } }

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the operation is applicable.</value>
        public AreaOfUse AreaOfUse { get { return _areaOfUse; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
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
        protected GeographicTransformation(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, CoordinateReferenceSystem source, CoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source coordinate reference system is null.");
            if (target == null)
                throw new ArgumentNullException("target", "The target coordinate reference system is null.");
            if (areaOfUse == null)
                throw new ArgumentNullException("areaOfUse", "The area of use is null.");

            _source = source;
            _target = target;
            _areaOfUse = areaOfUse;
        }

        #endregion
    }
}
