/// <copyright file="SimilarityTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a similarity transformation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::9621", "Similarity Transformation")]
    public class SimilarityTransformation : CoordinateTransformation<Coordinate>
    {
        #region Protected fields

        protected Double _ordinate1OfEvaluationPointInTarget;
        protected Double _ordinate2OfEvaluationPointInTarget;
        protected Double _scaleDifference;
        protected Double _rotationAngleOfSourceCoordinate;
        protected Double _m;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarityTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">The method requires parameteres which are not specified.</exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double percision floating-point number as required by the method.
        /// </exception>
        public SimilarityTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, CoordinateOperationMethods.SimilarityTransformation, parameters)
        {
            // source: EPSG Guidance Note number 7, part 2, page 110

            _ordinate1OfEvaluationPointInTarget = ((Length)_parameters[CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget]).BaseValue;
            _ordinate2OfEvaluationPointInTarget = ((Length)_parameters[CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget]).BaseValue;
            _scaleDifference = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleDifference]);
            _rotationAngleOfSourceCoordinate = ((Angle)_parameters[CoordinateOperationParameters.XAxisRotation]).BaseValue;
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
            // source: EPSG Guidance Note number 7, part 2, page 109

            Double x = _ordinate1OfEvaluationPointInTarget + (coordinate.X * _m * Math.Cos(_rotationAngleOfSourceCoordinate)) +
                                                             (coordinate.Y * _m * Math.Sin(_rotationAngleOfSourceCoordinate));

            Double y = _ordinate2OfEvaluationPointInTarget - (coordinate.X * _m * Math.Sin(_rotationAngleOfSourceCoordinate)) +
                                                             (coordinate.Y * _m * Math.Cos(_rotationAngleOfSourceCoordinate));

            return new Coordinate(x, y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 109

            Double x = ((coordinate.X - _ordinate1OfEvaluationPointInTarget) * Math.Cos(_rotationAngleOfSourceCoordinate) -
                       (coordinate.Y - _ordinate2OfEvaluationPointInTarget) * Math.Sin(_rotationAngleOfSourceCoordinate)) / _m;

            Double y = ((coordinate.X - _ordinate1OfEvaluationPointInTarget) * Math.Sin(_rotationAngleOfSourceCoordinate) +
                       (coordinate.Y - _ordinate2OfEvaluationPointInTarget) * Math.Cos(_rotationAngleOfSourceCoordinate)) / _m;

            return new Coordinate(x, y);
        }

        #endregion
    }
}
