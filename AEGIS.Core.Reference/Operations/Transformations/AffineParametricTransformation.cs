/// <copyright file="AffineParametricTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an affine parametric transformation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9624 ", "Affine parametric transformation")]
    public class AffineParametricTransformation : CoordinateTransformation<Coordinate>
    {
        #region Private fields

        protected Double _A0; // transformation params
        protected Double _A1;
        protected Double _A2;
        protected Double _B0;
        protected Double _B1;
        protected Double _B2;
        protected Double _A0Inv;
        protected Double _A1Inv;
        protected Double _A2Inv;
        protected Double _B0Inv;
        protected Double _B1Inv;
        protected Double _B2Inv;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AffineParametricTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">The method requires parameters which are not specified.</exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// </exception>
        public AffineParametricTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, CoordinateOperationMethods.AffineParametricTransformation, parameters)
        {
            _A0 = Convert.ToDouble(_parameters[CoordinateOperationParameters.A0]);
            _A1 = Convert.ToDouble(_parameters[CoordinateOperationParameters.A1]);
            _A2 = Convert.ToDouble(_parameters[CoordinateOperationParameters.A2]);
            _B0 = Convert.ToDouble(_parameters[CoordinateOperationParameters.B0]);
            _B1 = Convert.ToDouble(_parameters[CoordinateOperationParameters.B1]);
            _B2 = Convert.ToDouble(_parameters[CoordinateOperationParameters.B2]);

            Double d = _A1 * _B2 - _A2 * _B1;
            _A0Inv = (_A2 * _B0 - _B2 * _A0) / d;
            _B0Inv = (_B1 * _A0 - _A1 * _B0) / d;
            _A1Inv = _B2 / d;
            _A2Inv = -_A2 / d;
            _B1Inv = -_B1 / d;
            _B2Inv = _A1 / d;
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
            return new Coordinate(_A0 + _A1 * coordinate.X + _A2 * coordinate.Y, _B0 + _B1 * coordinate.X + _B2 * coordinate.Y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            return new Coordinate(_A0Inv + _A1Inv * coordinate.X + _A2Inv * coordinate.Y, _B0Inv + _B1Inv * coordinate.X + _B2Inv * coordinate.Y);
        }

        #endregion
    }
}
