/// <copyright file="SinusoidalProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
/// <author>Péter Rónai</author>

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations.Projections
{
    /// <summary>
    /// Represents a Sinusoidal Projection.
    /// Source: Map Projections - A Working Manual by John P. Snyder (1987) pages 247-248
    /// </summary>
    [CoordinateOperationMethodImplementation("ESRI::53008", "Sinusoidal Projection")]
    public class SinusoidalProjection : CoordinateProjection
    {
        #region Private fields

        private readonly Double _falseEasting;
        private readonly Double _falseNorthing;
        private readonly Double _longitudeOfNaturalOrigin;
        private readonly Double _sphereRadius;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SinusoidalProjection" />.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use, where the operation is applicable.</param>
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
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public SinusoidalProjection(string identifier, string name, IDictionary<CoordinateOperationParameter, object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse) : 
            base(identifier, name, CoordinateOperationMethods.SinusoidalProjection, parameters, ellipsoid, areaOfUse)
        {
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _sphereRadius = ellipsoid.SemiMajorAxis.BaseValue;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be forward transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;
            Double ellipsoidCorrection = _ellipsoid.IsSphere ? 1 : Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Square(Math.Sin(latitude)));
            Double x = _sphereRadius * (ComputeLongitudeDelta(longitude)) * Math.Cos(latitude) / ellipsoidCorrection;
            Double y = _ellipsoid.IsSphere ? _sphereRadius * latitude : ComputeYForEllipsoid(latitude);
            return new Coordinate(_falseEasting + x, _falseNorthing + y);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be reverse transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double x = coordinate.X - _falseEasting;
            Double y = coordinate.Y - _falseNorthing;
            if (_ellipsoid.IsSphere)
            {
                return ComputeReverseSphere(x, y);
            }
            else
            {
                return ComputeReverseEllipsoid(x, y);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the reverse sphere coordinate.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The resulting geo-coordinate.</returns>
        private GeoCoordinate ComputeReverseSphere(Double x, Double y)
        {
            Double latitude = y / _sphereRadius;
            Double longitude;
            if (latitude == Math.PI / 2 || -latitude == Math.PI / 2)
            {
                longitude = _longitudeOfNaturalOrigin;
            }
            else
            {
                longitude = _longitudeOfNaturalOrigin + x / (_sphereRadius * Math.Cos(latitude));
            }
            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Computes the reverse ellipsoid coordinate.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The resulting geo-coordinate.</returns>
        private GeoCoordinate ComputeReverseEllipsoid(Double x, Double y)
        {
            Double modifiedEccentricity = ComputeModifiedEccentricity();
            Double mu = ComputeMu(y);
            Double latitude = mu +
                (3 * modifiedEccentricity / 2 - 27 * Math.Pow(modifiedEccentricity, 3) / 32) * Math.Sin(2 * mu) +
                (21 * Calculator.Square(modifiedEccentricity) / 16 - 55 * Math.Pow(modifiedEccentricity, 4) / 32) * Math.Sin(4 * mu) +
                (151 * Math.Pow(modifiedEccentricity, 3) / 96) * Math.Sin(6 * mu) +
                (1097 * Math.Pow(modifiedEccentricity, 4) / 512) * Math.Sin(8 * mu);
            Double longitude = _longitudeOfNaturalOrigin +
                x * Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Square(Math.Sin(latitude))) / (_sphereRadius * Math.Cos(latitude));
            return new GeoCoordinate(latitude, longitude);
        }

        /// <summary>
        /// Computes the longitude delta.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The longitude delta.</returns>
        private Double ComputeLongitudeDelta(Double longitude)
        {
            Angle threshold = Angle.FromDegree(180);
            Angle diff = Angle.FromRadian(longitude) - Angle.FromRadian(_longitudeOfNaturalOrigin);
            Angle correction = Angle.FromDegree(diff > threshold ? -360 : (diff < -threshold ? 360 : 0));
            return (diff + correction).BaseValue;
        }

        /// <summary>
        /// Computes the Y coordinate for ellipsoid.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <returns>The Y coordinate.</returns>
        private Double ComputeYForEllipsoid(Double latitude)
        {
            Double e2 = _ellipsoid.EccentricitySquare;
            Double e4 = Calculator.Square(_ellipsoid.EccentricitySquare);
            Double e6 = Math.Pow(_ellipsoid.EccentricitySquare, 3);
            return _sphereRadius * (
                (1 - e2 / 4 - 3 * e4 / 64 - 5 * e6 / 256) * latitude
                - (3 * e2 / 8 + 3 * e4 / 32 + 45 * e6 / 1024) * Math.Sin(2 * latitude)
                + (15 * e4 / 256 + 45 * e6 / 1024) * Math.Sin(4 * latitude)
                - (35 * e6 / 3072) * Math.Sin(6 * latitude));
        }

        /// <summary>
        /// Computes the modified eccentricity.
        /// </summary>
        /// <returns>The modified eccentricity.</returns>
        private Double ComputeModifiedEccentricity()
        {
            return (1 - Math.Sqrt(1 - _ellipsoid.EccentricitySquare)) / (1 + Math.Sqrt(1 - _ellipsoid.EccentricitySquare));
        }

        /// <summary>
        /// Computes the Mu parameter.
        /// </summary>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The Mu parameter.</returns>
        private Double ComputeMu(Double y)
        {
            Double e2 = _ellipsoid.EccentricitySquare;
            Double e4 = Calculator.Square(_ellipsoid.EccentricitySquare);
            Double e6 = Math.Pow(_ellipsoid.EccentricitySquare, 3);
            return y / (_sphereRadius * (1 - e2 / 4 - 3 * e4 / 64 - 5 * e6 / 256));
        }

        #endregion

    }
}
