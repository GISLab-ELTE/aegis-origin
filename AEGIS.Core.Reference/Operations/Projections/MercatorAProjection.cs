/// <copyright file="MercatorAProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Mercator (Variant A) projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9804", "Mercator (variant A)")]
    public class MercatorAProjection : MercatorProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MercatorAProjection" /> class.
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
        /// or
        /// The latitude of natural origin is not zero.
        /// </exception>
        public MercatorAProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.MercatorAProjection, parameters, ellipsoid, areaOfUse)
        {
            Double latitudeOfNaturalOrigin = ((Angle)parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;

            if (latitudeOfNaturalOrigin != 0)
                throw new ArgumentException("The latitude of natural origin is not zero.", "parameters");

            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);
        }

        #endregion
    }
}
