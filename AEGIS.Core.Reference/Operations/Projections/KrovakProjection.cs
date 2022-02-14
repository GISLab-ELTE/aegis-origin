/// <copyright file="KrovakProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Krovak Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::9819", "Krovak Projection")]
    public class KrovakProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfProjectionCentre; // projection params
        protected Double _longitudeOfOrigin;
        protected Double _coLatitudeOfConeAxis;
        protected Double _latitudeOfPseudoStandardParallel;
        protected Double _scaleFactorOnPseudoStandardParallel;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _A; // projection constants
        protected Double _B;
        protected Double _gammaO;
        protected Double _tO;
        protected Double _n;
        protected Double _rO;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakProjection" /> class.
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
        public KrovakProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.KrovakProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 25

            _latitudeOfProjectionCentre = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfProjectionCentre]).BaseValue;
            _longitudeOfOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfOrigin]).BaseValue;
            _coLatitudeOfConeAxis = ((Angle)_parameters[CoordinateOperationParameters.CoLatitudeOfConeAxis]).BaseValue;
            _latitudeOfPseudoStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfPseudoStandardParallel]).BaseValue;
            _scaleFactorOnPseudoStandardParallel = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel]);
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).BaseValue;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).BaseValue;

            _A = (_ellipsoid.SemiMajorAxis.BaseValue * Math.Pow((1 - _ellipsoid.EccentricitySquare), 0.5)) / (1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfProjectionCentre));
            _B = Math.Pow(1 + ((_ellipsoid.EccentricitySquare * Calculator.Cos4(_latitudeOfProjectionCentre)) / (1 - _ellipsoid.EccentricitySquare)), 0.5);
            _gammaO = Math.Asin(Math.Sin(_latitudeOfProjectionCentre) / _B);
            _tO = Math.Tan((Math.PI / 4) + (_gammaO / 2)) * (Math.Pow((1 + _ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)) / (1 - _ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)), _ellipsoid.Eccentricity * _B / 2)) / Math.Pow(Math.Tan(Math.PI / 4 + _latitudeOfProjectionCentre / 2), _B);
            _n = Math.Sin(_latitudeOfPseudoStandardParallel);
            _rO = (_scaleFactorOnPseudoStandardParallel * _A) / (Math.Tan(_latitudeOfPseudoStandardParallel));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KrovakProjection" /> class.
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
        protected KrovakProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 25

            _latitudeOfProjectionCentre = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfProjectionCentre]).BaseValue;
            _longitudeOfOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfOrigin]).BaseValue;
            _coLatitudeOfConeAxis = ((Angle)_parameters[CoordinateOperationParameters.CoLatitudeOfConeAxis]).BaseValue;
            _latitudeOfPseudoStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfPseudoStandardParallel]).BaseValue;
            _scaleFactorOnPseudoStandardParallel = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorOnPseudoStandardParallel]);
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).BaseValue;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).BaseValue;

            _A = (_ellipsoid.SemiMajorAxis.BaseValue * Math.Pow((1 - _ellipsoid.EccentricitySquare), 0.5)) / (1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfProjectionCentre));
            _B = Math.Pow(1 + ((_ellipsoid.EccentricitySquare * Calculator.Cos4(_latitudeOfProjectionCentre)) / (1 - _ellipsoid.EccentricitySquare)), 0.5);
            _gammaO = Math.Asin(Math.Sin(_latitudeOfProjectionCentre) / _B);
            _tO = Math.Tan((Math.PI / 4) + (_gammaO / 2)) * (Math.Pow((1 + _ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)) / (1 - _ellipsoid.Eccentricity * Math.Sin(_latitudeOfProjectionCentre)), _ellipsoid.Eccentricity * _B / 2)) / Math.Pow(Math.Tan(Math.PI / 4 + _latitudeOfProjectionCentre / 2), _B);
            _n = Math.Sin(_latitudeOfPseudoStandardParallel);
            _rO = (_scaleFactorOnPseudoStandardParallel * _A) / (Math.Tan(_latitudeOfPseudoStandardParallel));
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
            // source: EPSG Guidance Note number 7, part 2, page 25

            Double u = 2 * (Math.Atan(_tO * Math.Pow(Math.Tan((coordinate.Latitude.BaseValue / 2) + (Math.PI / 4)), _B) / Math.Pow(((1 + _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 - _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue))), _ellipsoid.Eccentricity * _B / 2)) - Math.PI / 4);
            Double v = _B * (_longitudeOfOrigin - coordinate.Longitude.BaseValue);
            Double t = Math.Asin(Math.Cos(_coLatitudeOfConeAxis) * Math.Sin(u) + Math.Sin(_coLatitudeOfConeAxis) * Math.Cos(u) * Math.Cos(v));
            Double d = Math.Asin(Math.Cos(u) * Math.Sin(v) / Math.Cos(t));
            Double theta = _n * d;
            Double r = _rO * Math.Pow(Math.Tan((Math.PI / 4) + (_latitudeOfPseudoStandardParallel / 2)), _n) / Math.Pow(Math.Tan(t/2 + Math.PI / 4), _n);
            Double xP = r * Math.Cos(theta);
            Double yP = r * Math.Sin(theta);

            return new Coordinate(yP + _falseEasting, xP + _falseNorthing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 25

            Double xP = coordinate.Y - _falseNorthing;
            Double yP = coordinate.X - _falseEasting;
            Double r = Math.Pow(Math.Pow(xP, 2) + Math.Pow(yP, 2), 0.5);
            Double teta = Math.Atan(yP / xP);
            Double d = teta / Math.Sin(_latitudeOfPseudoStandardParallel);
            Double t = 2 * (Math.Atan(Math.Pow(_rO / r, 1/_n) * Math.Tan(Math.PI / 4 + _latitudeOfPseudoStandardParallel / 2)) - Math.PI / 4);
            Double u = Math.Asin(Math.Cos(_coLatitudeOfConeAxis) * Math.Sin(t) - Math.Sin(_coLatitudeOfConeAxis) * Math.Cos(t) * Math.Cos(d));
            Double v = Math.Asin(Math.Cos(t) * Math.Sin(d) / Math.Cos(u));

            Double lambda = _longitudeOfOrigin - (v / _B);

            Double phiJ;
            Double phiJ1 = u;

            for (Int32 i = 0; i < 3; i++)
            {
                phiJ = 2 * (Math.Atan(Math.Pow(_tO, -1 / _B) * Math.Pow(Math.Tan(u / 2 + Math.PI / 4), 1 / _B) * Math.Pow((1 + _ellipsoid.Eccentricity * Math.Sin(phiJ1)) / (1 - _ellipsoid.Eccentricity * Math.Sin(phiJ1)), _ellipsoid.Eccentricity / 2)) - Math.PI / 4);
                phiJ1 = phiJ;
            }

            return new GeoCoordinate(phiJ1, lambda);
        }

        #endregion
    }
}