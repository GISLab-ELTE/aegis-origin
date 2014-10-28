/// <copyright file="ObliqueMercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents the Hotine Oblique Mercator projections.
    /// </summary>
    public abstract class ObliqueMercatorProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfProjectionCentre; // projection params
        protected Double _longitudeOfProjectionCentre;
        protected Double _scaleFactorOnInitialLine;
        protected Double _azimuthOfInitialLine;

        protected Double _b;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObliqueMercatorProjection" /> class.
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
        /// The parameter is not a double percision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        protected ObliqueMercatorProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 56

            _latitudeOfProjectionCentre = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfProjectionCentre]).BaseValue;
            _longitudeOfProjectionCentre = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfProjectionCentre]).BaseValue;
            _scaleFactorOnInitialLine = (Double)_parameters[CoordinateOperationParameters.ScaleFactorOnInitialLine];
            _azimuthOfInitialLine = ((Angle)_parameters[CoordinateOperationParameters.AzimuthOfInitialLine]).BaseValue;

            _b = Math.Sqrt(1 + (_ellipsoid.EccentricitySquare * Calculator.Cos4(_latitudeOfProjectionCentre) / (1 - _ellipsoid.EccentricitySquare)));
        }

        #endregion
    }
}
