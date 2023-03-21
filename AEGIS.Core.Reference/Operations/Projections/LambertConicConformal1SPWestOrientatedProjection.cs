// <copyright file="LambertConicConformal1SPWestOrientatedProjection.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Lambert Conic Conformal (1SP West Orientated) projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::9826", "Lambert Conic Conformal (West Orientated)")]
    public class LambertConicConformal1SPWestOrientatedProjection : LambertConicConformal1SPProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPWestOrientatedProjection" /> class.
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
        public LambertConicConformal1SPWestOrientatedProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.LambertConicConformal1SPWestOrientatedProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Compute the coordinate easting and northing based on the R and Theta values.
        /// </summary>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        protected override void ComputeCoordinate(Double r, Double theta, out Double easting, out Double northing)
        {
            // source: EPSG Guidance Note number 7, part 2, page 21

            easting = _falseEasting - r * Math.Sin(theta);
            northing = _falseNorthing + _r0 - r * Math.Cos(theta);
        }

        /// <summary>
        /// Compute the transformations parameters R and Theta based on the easting and northing values.
        /// </summary>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        protected override void ComputeParams(Double easting, Double northing, out Double r, out Double theta)
        {
            // source: EPSG Guidance Note number 7, part 2, page 21

            theta = Math.Atan((_falseEasting - easting) / (_r0 - (northing - _falseNorthing)));
            r = Math.Sign(_n) * Math.Sqrt(Calculator.Square(_falseEasting - easting) + Calculator.Square(_r0 - (northing - _falseNorthing)));
        }

        #endregion
    }
}
