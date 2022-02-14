/// <copyright file="GeneralPolynomialTransformation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using System.Reflection;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a General Polynomial Transformation.
    /// </summary>
    public class GeneralPolynomialTransformation : CoordinateTransformation<Coordinate>
    {
        #region Protected fields

        protected Int32 _degree;
        protected Double _ordinate1OfEvaluationPointInSource;
        protected Double _ordinate2OfEvaluationPointInSource;
        protected Double _ordinate1OfEvaluationPointInTarget;
        protected Double _ordinate2OfEvaluationPointInTarget;
        protected Double _scalingFactorForSourceCoordinateDifferences;
        protected Double _scalingFactorForTargetCoordinateDifferences;
        protected List<Double> _parametersAList;
        protected List<Double> _parametersBList;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPolynomialTransformation" /> class.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// No general polynomial transformation is available for the given degree.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// </exception>
        public GeneralPolynomialTransformation(Int32 degree, Dictionary<CoordinateOperationParameter, Object> parameters)
            : base(GetMethod(degree).Identifier, GetMethod(degree).Name, GetMethod(degree), parameters)
        {
            _ordinate1OfEvaluationPointInSource = Convert.ToDouble(_parameters[CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource]);
            _ordinate2OfEvaluationPointInSource = Convert.ToDouble(_parameters[CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource]);
            _ordinate1OfEvaluationPointInTarget = Convert.ToDouble(_parameters[CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget]);
            _ordinate2OfEvaluationPointInTarget = Convert.ToDouble(_parameters[CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget]);
            _scalingFactorForSourceCoordinateDifferences = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences]);
            _scalingFactorForTargetCoordinateDifferences = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences]);

            _parametersAList = new List<Double>();
            _parametersBList = new List<Double>();
            Int32 i = 0;
            foreach (CoordinateOperationParameter parameter in _method.Parameters)
            {
                if (parameter.Name[0] == 'A')
                    _parametersAList.Add(Convert.ToDouble(_parameters[parameter]));
                if (parameter.Name[0] == 'B')
                    _parametersBList.Add(Convert.ToDouble(_parameters[parameter]));
                i++;
            }
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
            // source: EPSG Guidance Note number 7, part 2, page 98

            Double u = _scalingFactorForSourceCoordinateDifferences * (coordinate.X - _ordinate1OfEvaluationPointInSource);
            Double v = _scalingFactorForSourceCoordinateDifferences * (coordinate.Y - _ordinate2OfEvaluationPointInSource);
            Double dX = _parametersAList[0], dY = _parametersBList[0];

            Int32 l = 0;
            for (Int32 j = 1; j <= _degree; j++)
            {
                Int32 m = Convert.ToInt32(Calculator.Sum(1, j + 1)) - Convert.ToInt32(Calculator.Sum(1, j));
                for (Int32 k = 0; k < m; k++)
                {
                    dX += _parametersAList[l] * Calculator.Pow(u, j - k) * Calculator.Pow(v, k) / _scalingFactorForTargetCoordinateDifferences;
                    dY += _parametersBList[l] * Calculator.Pow(u, j - k) * Calculator.Pow(v, k) / _scalingFactorForTargetCoordinateDifferences;
                    l++;
                }
            }
            return new Coordinate(coordinate.X - _ordinate1OfEvaluationPointInSource + _ordinate1OfEvaluationPointInTarget + dX, 
                                  coordinate.Y - _ordinate2OfEvaluationPointInSource + _ordinate2OfEvaluationPointInTarget + dY);
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

        #region Private utility methods

        /// <summary>
        /// Gets the coordinate operation method.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <returns>The coordinate operation method for <see cref="degree" />.</returns>
        /// <exception cref="System.ArgumentException">No general polynomial transformation is available for the given degree.</exception>
        private static CoordinateOperationMethod GetMethod(Int32 degree)
        {
            PropertyInfo propertyInfo = typeof(CoordinateOperationMethods).GetProperty("GeneralPolynomial" + 2, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
            if (propertyInfo == null)
                throw new ArgumentException("No general polynomial transformation is available for the given degree.", "degree");

            return propertyInfo.GetValue(null, null) as CoordinateOperationMethod;
        }

        #endregion
    }
}
