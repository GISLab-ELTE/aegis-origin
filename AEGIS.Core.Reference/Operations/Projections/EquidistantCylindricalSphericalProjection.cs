// <copyright file="EquidistantCylindricalSphericalProjection.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents an Equidistant Cylindrical (Spherical) Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::1029", "Equidistant Cylindrical (Spherical)")]
    public class EquidistantCylindricalSphericalProjection : EquidistantCylindricalProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EquidistantCylindricalSphericalProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The defined operation method requires parameters.
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
        /// of
        /// The ellipsoid is not spherical.llipsoid
        /// </exception>
        public EquidistantCylindricalSphericalProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.EquidistantCylindricalSphericalProjection, parameters, ellipsoid, areaOfUse)
        {
            if (!_ellipsoid.IsSphere)
                throw new ArgumentException("The ellipsoid is not spherical.", "ellipsoid");
        }

        #endregion
    }
}
