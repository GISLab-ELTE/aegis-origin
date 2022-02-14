/// <copyright file="GnomonicProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Péter Rónai</author>

using System;
using System.Collections.Generic;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Management;

namespace ELTE.AEGIS.Reference.Operations.Projections
{
    /// <summary>
    /// Represents a Gnomonic Projection.
    /// </summary>
    [CoordinateOperationMethodImplementation("AEGIS::735137", "Gnomonic Projection")]
    public class GnomonicProjection : CoordinateProjection
    {
        #region Private fields

        private readonly Double _falseEasting;
        private readonly Double _falseNorthing;
        private readonly Double _latitudeOfCentre;
        private readonly Double _longitudeOfCentre;
        private readonly Double _sphereRadius;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="GnomonicProjection" />.
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
        public GnomonicProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.GnomonicProjection, parameters, ellipsoid, areaOfUse)
        {
            _latitudeOfCentre = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfProjectionCentre]).BaseValue;
            _longitudeOfCentre = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfProjectionCentre]).BaseValue;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _sphereRadius = ellipsoid.SemiMajorAxis.Value;
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">the coordinate to be forward transformed</param>
        /// <returns>the transformed coordinate</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double latitude = coordinate.Latitude.BaseValue;
            Double longitude = coordinate.Longitude.BaseValue;
            Double longitudeDelta = GetLongitudeDelta(longitude);
            Double angularDistanceFromProjectionCentre = GetAngularDistanceFromProjectionCentre(latitude, longitude);
            Double x = _sphereRadius / angularDistanceFromProjectionCentre * (Math.Cos(latitude) * Math.Sin(longitudeDelta));
            Double y = _sphereRadius / angularDistanceFromProjectionCentre * (Math.Cos(_latitudeOfCentre) * Math.Sin(latitude) - Math.Sin(_latitudeOfCentre) * Math.Cos(latitude) * Math.Cos(longitudeDelta));
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
            Double rho = Math.Sqrt(Calculator.Square(x) + Calculator.Square(y));

            if (rho == 0)
            {
                // this is a corner case as defined by the working manual:
                // when rho == 0, then the rest of the equations are indeterminate, but longitude, and latitude are exactly on the centre of the projection
                return new GeoCoordinate(_latitudeOfCentre, _longitudeOfCentre);
            }

            Double c = Math.Atan(rho / _sphereRadius);
            Double latitude = Math.Asin(Math.Cos(c) * Math.Sin(_latitudeOfCentre) + ((y * Math.Sin(c) * Math.Cos(_latitudeOfCentre)) / rho));
            Double longitude = _longitudeOfCentre + (Math.Atan(x * Math.Sin(c) / (rho * Math.Cos(_latitudeOfCentre) * Math.Cos(c) - y * Math.Sin(_latitudeOfCentre) * Math.Sin(c))));
            return new GeoCoordinate(latitude, longitude);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the angular distance of a given point on the Earth's surface from the projection center.
        /// </summary>
        /// <remarks>
        /// This simplified calculation is specific to the Gnomonic Projection (e.g. it doesn't take the Spheroid into account).
        /// </remarks>
        /// <param name="latitude">Latitude of the geographic coordinate (in radians).</param>
        /// <param name="longitude">Longitude of the geographic coordinate (in radians).</param>
        /// <returns>The angular distance from the projection center (in radians).</returns>
        private Double GetAngularDistanceFromProjectionCentre(Double latitude, Double longitude)
        {
            return Math.Sin(_latitudeOfCentre) * Math.Sin(latitude) + Math.Cos(_latitudeOfCentre) * Math.Cos(latitude) * Math.Cos(GetLongitudeDelta(longitude));
        }

        /// <summary>
        /// Returns the longitude delta.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The longitude delta.</returns>
        private Double GetLongitudeDelta(Double longitude)
        {
            Angle threshold = Angle.FromDegree(180);
            Angle diff = Angle.FromRadian(longitude) - Angle.FromRadian(_longitudeOfCentre);
            Angle correction = Angle.FromDegree(diff > threshold ? -360 : (diff < -threshold ? 360 : 0));
            return (diff + correction).BaseValue;
        }

        #endregion
    }
}
