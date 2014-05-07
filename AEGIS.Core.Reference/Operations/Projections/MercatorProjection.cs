/// <copyright file="MercatorProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a general Mercator projection.
    /// </summary>
    public abstract class MercatorProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _longitudeOfNaturalOrigin; // projection params
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double[] _inverseParams;
        protected Double _scaleFactorAtNaturalOrigin;
        protected Double _ellipsoidRadius; // projection constant

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MercatorProjection" /> class.
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
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double percision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        protected MercatorProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 34

            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _ellipsoidRadius = _ellipsoid.SemiMajorAxis.Value;

            _inverseParams = new Double[] 
            {
                (_ellipsoid.EccentricitySquare / 2 + 5 * Calculator.Pow(_ellipsoid.Eccentricity, 4) / 24 + Calculator.Pow(_ellipsoid.Eccentricity, 6) / 12 + 13 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 360),
                (7 * Calculator.Pow(_ellipsoid.Eccentricity, 4) / 48 + 29 * Calculator.Pow(_ellipsoid.Eccentricity, 6) / 240 + 811 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 11520),
                (7 * Calculator.Pow(_ellipsoid.Eccentricity, 6) / 120 + 81 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 1120),
                4279 * Calculator.Pow(_ellipsoid.Eccentricity, 8) / 161280
            };
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
            // source: EPSG Guidance Note number 7, part 2, page 34

            Double easting, northing;

            if (coordinate.Latitude.BaseValue > 3.07)
                throw new ArgumentException("The coordinate is not within the range of the transformation.", "coordinate");

            if (_ellipsoid.IsSphere)
            {
                easting = _falseEasting + _ellipsoidRadius * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                northing = _falseNorthing + _ellipsoidRadius * Math.Log(Math.Tan(Constants.PI / 4 + coordinate.Latitude.BaseValue / 2));
            }
            else
            {
                easting = _falseEasting + _ellipsoidRadius * _scaleFactorAtNaturalOrigin * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                northing = _falseNorthing + _ellipsoidRadius * _scaleFactorAtNaturalOrigin * Math.Log(Math.Tan(Constants.PI / 4 + coordinate.Latitude.BaseValue / 2) * Math.Pow((1 - _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)), _ellipsoid.Eccentricity / 2));
            }

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 34

            Double phi, lambda;

            if (_ellipsoid.IsSphere)
            {
                phi = Constants.PI / 2 - 2 * Math.Atan(Math.Exp((_falseNorthing - coordinate.Y) / _ellipsoidRadius));
                lambda = (coordinate.X - _falseEasting) / _ellipsoidRadius + _longitudeOfNaturalOrigin;
            }
            else
            {
                Double t = Math.Pow(Math.E, (_falseNorthing - coordinate.Y) / (_ellipsoidRadius * _scaleFactorAtNaturalOrigin));
                Double xi = Constants.PI / 2 - 2 * Math.Atan(t);

                lambda = (coordinate.X - _falseEasting) / (_ellipsoidRadius * _scaleFactorAtNaturalOrigin) + _longitudeOfNaturalOrigin;
                phi = xi + _inverseParams[0] * Math.Sin(2 * xi)
                         + _inverseParams[1] * Math.Sin(4 * xi)
                         + _inverseParams[2] * Math.Sin(6 * xi)
                         + _inverseParams[3] * Math.Sin(8 * xi);
            }
            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}
