/// <copyright file="CoordinateFrameRotation.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Coordinate Frame Rotation transformation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::1032", "Coordinate Frame Rotation (geocentric domain)")]
    public class CoordinateFrameRotation : HelmertTransformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateFrameRotation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameteres which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double percision floating-point number as required by the method.
        /// </exception>
        public CoordinateFrameRotation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, CoordinateOperationMethods.CoordinateFrameRotationGeocentricDomain, parameters)
        {
            // source: EPSG Guidance Note number 7, part 2, page 123

            _xAxisRotation = -_xAxisRotation;
            _yAxisRotation = -_yAxisRotation;
            _zAxisRotation = -_zAxisRotation;

            Double det = 1 + Calculator.Square(_xAxisRotation) + Calculator.Square(_xAxisRotation) + Calculator.Square(_xAxisRotation);
            _inverseParams = new Double[3, 3];
            _inverseParams[0, 0] = 1 / det * (1 + Calculator.Square(_xAxisRotation));
            _inverseParams[0, 1] = 1 / det * (_zAxisRotation + _xAxisRotation * _yAxisRotation);
            _inverseParams[0, 2] = 1 / det * (_xAxisRotation * _zAxisRotation - _yAxisRotation);
            _inverseParams[1, 0] = 1 / det * (_xAxisRotation * _yAxisRotation - _zAxisRotation);
            _inverseParams[1, 1] = 1 / det * (1 + Calculator.Square(_yAxisRotation));
            _inverseParams[1, 2] = 1 / det * (_xAxisRotation + _yAxisRotation * _zAxisRotation);
            _inverseParams[2, 0] = 1 / det * (_xAxisRotation * _zAxisRotation + _yAxisRotation);
            _inverseParams[2, 1] = 1 / det * (_yAxisRotation * _zAxisRotation - _xAxisRotation);
            _inverseParams[2, 2] = 1 / det * (1 + Calculator.Square(_zAxisRotation));
        }

        #endregion
    }
}
