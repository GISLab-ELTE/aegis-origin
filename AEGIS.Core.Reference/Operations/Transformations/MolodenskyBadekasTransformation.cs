/// <copyright file="MolodenskyBadekasTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Molodensky-Badekas Transformation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::1034", "Molodensky-Badekas (geocentric domain)")]
    public class MolodenskyBadekasTransformation : GeocentricTransformation
    {
        #region Protected fields

        protected Double _xAxisTranslation;
        protected Double _yAxisTranslation;
        protected Double _zAxisTranslation;
        protected Double _xAxisRotation;
        protected Double _yAxisRotation;
        protected Double _zAxisRotation;
        protected Double _scaleDifference;
        protected Double _ordinate1OfEvaluationPoint;
        protected Double _ordinate2OfEvaluationPoint;
        protected Double _ordinate3OfEvaluationPoint;
        protected Double _m;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MolodenskyBadekasTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
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
        protected MolodenskyBadekasTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, CoordinateOperationMethods.MolodenskyBadekasTransformation, parameters)
        {
            // source: EPSG Guidance Note number 7, part 2, page 125

            _xAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.XAxisTranslation]).BaseValue;
            _yAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.YAxisTranslation]).BaseValue;
            _zAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.ZAxisTranslation]).BaseValue;
            _xAxisRotation = ((Length)_parameters[CoordinateOperationParameters.XAxisRotation]).BaseValue;
            _yAxisRotation = ((Length)_parameters[CoordinateOperationParameters.YAxisRotation]).BaseValue;
            _zAxisRotation = ((Length)_parameters[CoordinateOperationParameters.ZAxisRotation]).BaseValue;
            _scaleDifference = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleDifference]);
            _ordinate1OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate1OfEvaluationPoint]).BaseValue;
            _ordinate2OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate2OfEvaluationPoint]).BaseValue;
            _ordinate3OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate3OfEvaluationPoint]).BaseValue;

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
            // source: EPSG Guidance Note number 7, part 2, page 125

            Double x = _m * ((coordinate.X - _ordinate1OfEvaluationPoint) - _zAxisRotation * (coordinate.Y - _ordinate2OfEvaluationPoint) + _yAxisRotation * (coordinate.Z - _ordinate3OfEvaluationPoint)) + _ordinate1OfEvaluationPoint + _xAxisTranslation;
            Double y = _m * (_zAxisRotation * (coordinate.X - _ordinate1OfEvaluationPoint) + (coordinate.Y - _ordinate2OfEvaluationPoint) - _xAxisRotation * (coordinate.Z - _ordinate3OfEvaluationPoint)) + _ordinate2OfEvaluationPoint + _yAxisTranslation;
            Double z = _m * (-_yAxisRotation * (coordinate.X - _ordinate1OfEvaluationPoint) + _xAxisRotation * (coordinate.Y - _ordinate2OfEvaluationPoint) + (coordinate.Z - _ordinate3OfEvaluationPoint)) + _ordinate3OfEvaluationPoint + _zAxisTranslation;

            return new Coordinate(x, y, z);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            throw new NotSupportedException("Coordinate operation is not reversible.");
        }

        #endregion
    }
}
