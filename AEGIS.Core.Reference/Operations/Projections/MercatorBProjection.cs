// <copyright file="MercatorBProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Mercator (Variant B) projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9805", "Mercator (variant B)")]
    public class MercatorBProjection : MercatorProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MercatorBProjection" /> class.
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
        /// </exception>
        public MercatorBProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.MercatorBProjection, parameters, ellipsoid, areaOfUse)
        {
            Double latitudeOf1stStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
            _scaleFactorAtNaturalOrigin = Math.Cos(latitudeOf1stStandardParallel) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(latitudeOf1stStandardParallel));
        }

        #endregion
    }
}
