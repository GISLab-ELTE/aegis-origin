// <copyright file="PolynomialTransformationFactory.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using System.Reflection;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Numerics.LinearAlgebra;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Provides factory methods for creating polynomial transformations.
    /// </summary>
    public static class PolynomialTransformationFactory
    {
        #region Public static methods

        /// <summary>
        /// Creates a general (not invertible) polynomial transformation.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="sourceCoordinates">The source coordinates.</param>
        /// <param name="targetCoordinates">The target coordinates.</param>
        /// <returns>The general polynomial transformation created from the specified coordinates.</returns>
        /// <exception cref="System.ArgumentException">
        /// The degree is not valid.
        /// or
        /// The amount of source coordinates is insufficient for the specified degree.
        /// or
        /// The amount of target coordinates is insufficient for the specified degree.
        /// or
        /// The amount of source and target coordinates does not match.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The list of source coordinates is null.
        /// or
        /// The list of target coordinates is null.
        /// </exception>
        public static GeneralPolynomialTransformation CreateGeneralTransformation(Int32 degree, Coordinate[] sourceCoordinates, Coordinate[] targetCoordinates)
        {
            return CreateGeneralTransformation(degree, Coordinate.Empty, sourceCoordinates, Coordinate.Empty, targetCoordinates);
        }

        /// <summary>
        /// Creates a general (not invertible) polynomial transformation.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="sourceCoordinates">The source coordinates.</param>
        /// <param name="targetCoordinates">The target coordinates.</param>
        /// <returns>The general polynomial transformation created from the specified coordinates.</returns>
        /// <exception cref="System.ArgumentException">
        /// The degree is not valid.
        /// or
        /// The amount of source coordinates is insufficient for the specified degree.
        /// or
        /// The amount of target coordinates is insufficient for the specified degree.
        /// or
        /// The amount of source and target coordinates does not match.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The list of source coordinates is null.
        /// or
        /// The list of target coordinates is null.
        /// </exception>
        public static GeneralPolynomialTransformation CreateGeneralTransformation(Int32 degree, IList<Coordinate> sourceCoordinates, IList<Coordinate> targetCoordinates)
        {
            return CreateGeneralTransformation(degree, Coordinate.Empty, sourceCoordinates, Coordinate.Empty, targetCoordinates);
        }

        /// <summary>
        /// Creates a general (not invertible) polynomial transformation.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="sourceEvaluationCoordinate">The source evaluation coordinate.</param>
        /// <param name="sourceCoordinates">The source coordinates.</param>
        /// <param name="targetEvaluationCoordinate">The target evaluation coordinate.</param>
        /// <param name="targetCoordinates">The target coordinates.</param>
        /// <returns>The general polynomial transformation created from the specified coordinates.</returns>
        /// <exception cref="System.ArgumentException">
        /// The degree is not valid.
        /// or
        /// The amount of source coordinates is insufficient for the specified degree.
        /// or
        /// The amount of target coordinates is insufficient for the specified degree.
        /// or
        /// The amount of source and target coordinates does not match.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// The list of source coordinates is null.
        /// or
        /// The list of target coordinates is null.
        /// </exception>
        public static GeneralPolynomialTransformation CreateGeneralTransformation(Int32 degree,
                                                                                  Coordinate sourceEvaluationCoordinate, IList<Coordinate> sourceCoordinates,
                                                                                  Coordinate targetEvaluationCoordinate, IList<Coordinate> targetCoordinates)
        {
            if (degree < 1)
                throw new ArgumentException("The degree is not valid.", "degree");

            Int32 size = Convert.ToInt32(Calculator.Sum(1, degree + 1));

            if (sourceCoordinates == null)
                throw new ArgumentNullException("The list of source coordinates is null.", "sourceCoordinates");
            if (sourceCoordinates.Count < size)
                throw new ArgumentException("The amount of source coordinates is insufficient for the specified degree.", "sourceCoordinates");
            if (targetCoordinates == null)
                throw new ArgumentNullException("The list of target coordinates is null.", "targetCoordinates");
            if (targetCoordinates.Count < size)
                throw new ArgumentException("The amount of target coordinates is insufficient for the specified degree.", "targetCoordinates");
            if (sourceCoordinates.Count != targetCoordinates.Count)
                throw new ArgumentException("The amount of source and target coordinates does not match.", "targetCoordinates");

            // compute scaling factors
            Double sourceScalingFactor = 2 / (sourceCoordinates.Select(coordinate => coordinate.X).Average() + sourceCoordinates.Select(coordinate => coordinate.Y).Average());
            Double targetScalingFactor = 2 / (targetCoordinates.Select(coordinate => coordinate.X).Average() + targetCoordinates.Select(coordinate => coordinate.Y).Average());

            Matrix sourceMatrixA = new Matrix(size, size);
            Matrix sourceMatrixB = new Matrix(size, size);
            Vector targetVectorX = new Vector(size);
            Vector targetVectorY = new Vector(size);

            // compute coordinates
            Int32 l = 0;
            for (Int32 i = 0; i < sourceCoordinates.Count; i++)
            {
                sourceMatrixA[i, 0] = 1;
                sourceMatrixB[i, 0] = 1;
                l = 1;

                for (Int32 j = 1; j <= degree; j++)
                {
                    Int32 m = Convert.ToInt32(Calculator.Sum(1, j + 1)) - Convert.ToInt32(Calculator.Sum(1, j));
                    for (Int32 k = 0; k < m; k++)
                    {
                        Double u = (sourceCoordinates[i].X - sourceEvaluationCoordinate.X) * sourceScalingFactor;
                        Double v = (sourceCoordinates[i].Y - sourceEvaluationCoordinate.Y) * sourceScalingFactor;

                        sourceMatrixA[i, l] = Calculator.Pow(u, j - k) * Calculator.Pow(v, k);
                        sourceMatrixB[i, l] = Calculator.Pow(u, j - k) * Calculator.Pow(v, k);

                        l++;
                    }
                }
                targetVectorX[i] = (targetCoordinates[i].X - targetEvaluationCoordinate.X) * targetScalingFactor;
                targetVectorY[i] = (targetCoordinates[i].Y - targetEvaluationCoordinate.Y) * targetScalingFactor;
            }

            // compute coefficients
            Vector resultA = LUDecomposition.SolveEquation(sourceMatrixA, targetVectorX);
            Vector resultB = LUDecomposition.SolveEquation(sourceMatrixB, targetVectorY);

            // compute parameters
            Dictionary<CoordinateOperationParameter, Object> parameters = GenerateParameters(degree, sourceEvaluationCoordinate, targetEvaluationCoordinate, sourceScalingFactor, targetScalingFactor, resultA, resultB);

            return new GeneralPolynomialTransformation(degree, parameters);
        }
        
        #endregion

        #region Private static methods

        /// <summary>
        /// Generates the parameters.
        /// </summary>
        /// <param name="degree">The degree of the polynomial.</param>
        /// <param name="sourceEvaluationCoordinate">The source evaluation coordinate.</param>
        /// <param name="targetEvaluationCoordinate">The target evaluation coordinate.</param>
        /// <param name="sourceScalingFactor">The source scaling factor.</param>
        /// <param name="targetScalingFactor">The target scaling factor.</param>
        /// <param name="paramVectorA">The parameter vector A.</param>
        /// <param name="paramVectorB">The parameter vector B.</param>
        /// <returns>The parameters of the polynomial transformation.</returns>
        private static Dictionary<CoordinateOperationParameter, Object> GenerateParameters(Int32 degree, 
                                                                                           Coordinate sourceEvaluationCoordinate, Coordinate targetEvaluationCoordinate, 
                                                                                           Double sourceScalingFactor, Double targetScalingFactor,
                                                                                           Vector paramVectorA, Vector paramVectorB)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();

            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPointInSource, sourceEvaluationCoordinate.X);
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPointInSource, sourceEvaluationCoordinate.Y);
            parameters.Add(CoordinateOperationParameters.Ordinate1OfEvaluationPointInTarget, targetEvaluationCoordinate.X);
            parameters.Add(CoordinateOperationParameters.Ordinate2OfEvaluationPointInTarget, targetEvaluationCoordinate.Y);
            parameters.Add(CoordinateOperationParameters.ScalingFactorForSourceCoordinateDifferences, sourceScalingFactor);
            parameters.Add(CoordinateOperationParameters.ScalingFactorForTargetCoordinateDifferences, targetScalingFactor);

            parameters.Add(CoordinateOperationParameters.A0, paramVectorA[0]);
            parameters.Add(CoordinateOperationParameters.B0, paramVectorB[0]);

            Int32 l = 1;
            for (Int32 j = 1; j <= degree; j++)
            {
                Int32 m = Convert.ToInt32(Calculator.Sum(1, j + 1)) - Convert.ToInt32(Calculator.Sum(1, j));
                for (Int32 k = 0; k < m; k++)
                {
                    FieldInfo fieldInfo = typeof(CoordinateOperationParameters).GetField("Au" + (j - k) + "v" + k, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
                    CoordinateOperationParameter parameter = fieldInfo.GetValue(null) as CoordinateOperationParameter;
                    parameters.Add(parameter, paramVectorA[l]);

                    fieldInfo = typeof(CoordinateOperationParameters).GetField("Bu" + (j - k) + "v" + k, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
                    parameter = fieldInfo.GetValue(null) as CoordinateOperationParameter;
                    parameters.Add(parameter, paramVectorB[l]);

                    l++;
                }
            }

            return parameters;
        }

        #endregion
    }
}
