// <copyright file="HelmertTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Helmert Transformation (with 7 parameters).
    /// </summary>
    public abstract class HelmertTransformation : GeocentricTransformation
    {
        #region Protected fields

        protected Double _xAxisTranslation; // transformation params
        protected Double _yAxisTranslation;
        protected Double _zAxisTranslation;
        protected Double _xAxisRotation;
        protected Double _yAxisRotation;
        protected Double _zAxisRotation;
        protected Double _scaleDifference;
        protected Double _m; // transformation constants
        protected Double[,] _inverseParams;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HelmertTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// </exception>
        protected HelmertTransformation(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, method, parameters)
        {
            _xAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.XAxisTranslation]).BaseValue;
            _yAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.YAxisTranslation]).BaseValue;
            _zAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.ZAxisTranslation]).BaseValue;
            _xAxisRotation = ((Angle)_parameters[CoordinateOperationParameters.XAxisRotation]).BaseValue;
            _yAxisRotation = ((Angle)_parameters[CoordinateOperationParameters.YAxisRotation]).BaseValue;
            _zAxisRotation = ((Angle)_parameters[CoordinateOperationParameters.ZAxisRotation]).BaseValue;
            _scaleDifference = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleDifference]);

            _m = 1 + _scaleDifference * 1E-6;
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 123

            Double x = _m * (coordinate.X - _zAxisRotation * coordinate.Y + _yAxisRotation * coordinate.Z) + _xAxisTranslation;
            Double y = _m * (_zAxisRotation * coordinate.X + coordinate.Y - _xAxisRotation * coordinate.Z) + _yAxisTranslation;
            Double z = _m * (-_yAxisRotation * coordinate.X + _xAxisRotation * coordinate.Y + coordinate.Z) + _zAxisTranslation;

            return new Coordinate(x, y, z);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 123

            Double x = 1 / _m * (_inverseParams[0, 0] * (coordinate.X - _xAxisTranslation) + _inverseParams[0, 1] * (coordinate.Y - _yAxisTranslation) + _inverseParams[0, 2] * (coordinate.Z - _zAxisTranslation));
            Double y = 1 / _m * (_inverseParams[1, 0] * (coordinate.X - _xAxisTranslation) + _inverseParams[1, 1] * (coordinate.Y - _yAxisTranslation) + _inverseParams[1, 2] * (coordinate.Z - _zAxisTranslation));
            Double z = 1 / _m * (_inverseParams[2, 0] * (coordinate.X - _xAxisTranslation) + _inverseParams[2, 1] * (coordinate.Y - _yAxisTranslation) + _inverseParams[2, 2] * (coordinate.Z - _zAxisTranslation));

            return new Coordinate(x, y, z);
        }

        #endregion
    }
}
