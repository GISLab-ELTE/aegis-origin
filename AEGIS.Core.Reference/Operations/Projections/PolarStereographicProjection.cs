/// <copyright file="PolarStereographicProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Polar Stereographic Projection.
    /// </summary>
    public abstract class PolarStereographicProjection : CoordinateProjection
    {
        #region Protected fields

        protected OperationAspect _operationAspect;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarStereographicAProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
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
        public PolarStereographicProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the t value.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        protected Double ComputeT(Double latitude)
        {
            Double t = 0.0;
            
            switch (_operationAspect)
            {
                case OperationAspect.NorthPolar:
                    t = Math.Tan(Constants.PI / 4 - latitude / 2) * Math.Pow((1 + _ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 - _ellipsoid.Eccentricity * Math.Sin(latitude)), _ellipsoid.Eccentricity / 2);
                    break;
                case OperationAspect.SouthPolar:
                    t = Math.Tan(Constants.PI / 4 + latitude / 2) / Math.Pow((1 + _ellipsoid.Eccentricity * Math.Sin(latitude)) / (1 - _ellipsoid.Eccentricity * Math.Sin(latitude)), _ellipsoid.Eccentricity / 2);
                    break;
            }
            return t;
        }

        #endregion
    }
}
