/// <copyright file="KrovakModifiedProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
    /// Represents a Krovak Modified Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::1042", "Krovak Modified Projection")]
    public class KrovakModifiedProjection : KrovakProjection
    {
        #region Protected members

        protected Double _ordinate1OfEvaluationPoint; // projection params
        protected Double _ordinate2OfEvaluationPoint;
        protected Double _C1;
        protected Double _C2;
        protected Double _C3;
        protected Double _C4;
        protected Double _C5;
        protected Double _C6;
        protected Double _C7;
        protected Double _C8;
        protected Double _C9;
        protected Double _C10;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakModifiedProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameters which are not specified.
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
        public KrovakModifiedProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.KrovakModifiedProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 29

            _ordinate1OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate1OfEvaluationPoint]).BaseValue;
            _ordinate2OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate2OfEvaluationPoint]).BaseValue;
            _C1 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C1]);
            _C2 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C2]);
            _C3 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C3]);
            _C4 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C4]);
            _C5 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C5]);
            _C6 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C6]);
            _C7 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C7]);
            _C8 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C8]);
            _C9 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C9]);
            _C10 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C10]);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakModifiedProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
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
        protected KrovakModifiedProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 29

            _ordinate1OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate1OfEvaluationPoint]).BaseValue;
            _ordinate2OfEvaluationPoint = ((Length)_parameters[CoordinateOperationParameters.Ordinate2OfEvaluationPoint]).BaseValue;
            _C1 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C1]);
            _C2 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C2]);
            _C3 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C3]);
            _C4 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C4]);
            _C5 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C5]);
            _C6 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C6]);
            _C7 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C7]);
            _C8 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C8]);
            _C9 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C9]);
            _C10 = Convert.ToDouble(_parameters[CoordinateOperationParameters.C10]);
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 29

            Coordinate baseCoord = base.ComputeForward(coordinate);

            Double xP = baseCoord.Y - _falseNorthing;
            Double yP = baseCoord.X - _falseEasting;

            Double xR = xP - _ordinate1OfEvaluationPoint;
            Double yR = yP - _ordinate2OfEvaluationPoint;

            Double dX = _C1 + _C3 * xR - _C4 * yR - 2 * _C6 * xR * yR + _C5 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C7 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) - _C8 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) + 4 * _C9 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C10 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));
            Double dY = _C2 + _C3 * yR + _C4 * xR - 2 * _C5 * xR * yR + _C6 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C8 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) + _C7 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) - 4 * _C10 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C9 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));

            return new Coordinate(yP - dY + _falseEasting, xP - dX + _falseNorthing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 29

            Double xR = (coordinate.Y - _falseNorthing) - _ordinate1OfEvaluationPoint;
            Double yR = (coordinate.X - _falseEasting) - _ordinate2OfEvaluationPoint;

            Double dX = _C1 + _C3 * xR - _C4 * yR - 2 * _C6 * xR * yR + _C5 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C7 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) - _C8 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) + 4 * _C9 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C10 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));
            Double dY = _C2 + _C3 * yR + _C4 * xR - 2 * _C5 * xR * yR + _C6 * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C8 * xR * (Math.Pow(xR, 2) - 3 * Math.Pow(yR, 2)) + _C7 * yR * (3 * Math.Pow(xR, 2) - Math.Pow(yR, 2)) - 4 * _C10 * xR * yR * (Math.Pow(xR, 2) - Math.Pow(yR, 2)) + _C9 * (Math.Pow(xR, 4) + Math.Pow(yR, 4) - 6 * Math.Pow(xR, 2) * Math.Pow(yR, 2));

            return base.ComputeReverse(new Coordinate(coordinate.X + dY, coordinate.Y + dX));
        }

        #endregion
    }
}
