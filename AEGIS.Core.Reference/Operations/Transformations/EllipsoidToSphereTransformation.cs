/// <copyright file="EllipsoidToSphereTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents an ellipsoid to sphere transformation
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::735001", "Ellipsoid to sphere transformation")]
    public class EllipsoidToSphereTransformation : CoordinateTransformation<GeoCoordinate>
    {
        #region Protected fields

        protected readonly Ellipsoid _ellipsoid;
        protected Double _latitudeOfNaturalOrigin; // transformation params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _radiusOfConformalSphere;
        protected Double _conformalLatitudeOfNaturalOrigin; // ellipsoid params
        protected Double _conformalLongitudeOfNaturalOrigin;
        protected Double _n; // transformation constants
        protected Double _c;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid model of Earth.</value>
        public Ellipsoid Ellipsoid { get { return _ellipsoid; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipsoidToSphereTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameters which are not specified.
        /// or
        /// The ellipsoid is null.
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
        public EllipsoidToSphereTransformation(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid)
            : base(identifier, name, CoordinateOperationMethods.EllipsoidToSphereTransformation, parameters)
        {
            if (ellipsoid.IsSphere)
                return;

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;

            _n = Math.Sqrt(1 + (_ellipsoid.EccentricitySquare * Calculator.Cos4(_latitudeOfNaturalOrigin)) / (1 - _ellipsoid.EccentricitySquare));

            Double s1 = (1 + Math.Sin(_latitudeOfNaturalOrigin)) / (1 - Math.Sin(_latitudeOfNaturalOrigin));
            Double s2 = (1 - _ellipsoid.Eccentricity * Math.Sin(_latitudeOfNaturalOrigin)) / (1 + _ellipsoid.Eccentricity * Math.Sin(_latitudeOfNaturalOrigin));
            Double w1 = Math.Pow(s1 * Math.Pow(s2, _ellipsoid.Eccentricity), _n);
            Double sinXi1 = (w1 - 1) / (w1 + 1);

            _radiusOfConformalSphere = _ellipsoid.RadiusOfConformalSphere(_latitudeOfNaturalOrigin);
            _c = (_n + Math.Sin(_latitudeOfNaturalOrigin)) * (1 - sinXi1) / ((_n - Math.Sin(_latitudeOfNaturalOrigin)) * (1 + sinXi1));

            Double w2 = _c * w1;

            _conformalLatitudeOfNaturalOrigin = Math.Asin((w2 - 1) / (w2 + 1));
            _conformalLongitudeOfNaturalOrigin = _longitudeOfNaturalOrigin;
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate coordinate)
        {
            if (_ellipsoid.IsSphere)
                return coordinate;

            Double conformalLongitude = _n * (coordinate.Longitude.BaseValue - _conformalLongitudeOfNaturalOrigin) + _conformalLongitudeOfNaturalOrigin;

            Double sA = (1 + Math.Sin(coordinate.Latitude.BaseValue)) / (1 - Math.Sin(coordinate.Latitude.BaseValue));
            Double sB = (1 - _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue));
            Double w = _c * Math.Pow(sA * Math.Pow(sB, _ellipsoid.Eccentricity), _n);

            Double conformalLatitude = Math.Asin((w - 1) / (w + 1));

            return new GeoCoordinate(conformalLatitude, conformalLongitude);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate coordinate)
        {
            if (_ellipsoid.IsSphere)
                return coordinate;

            Double longitude = (coordinate.Longitude.BaseValue - _conformalLongitudeOfNaturalOrigin) / _n + _conformalLongitudeOfNaturalOrigin;

            Double psi1 = Math.Log((1 + Math.Sin(coordinate.Latitude.BaseValue)) / (_c * (1 - Math.Sin(coordinate.Latitude.BaseValue)))) / _n / 2;
            Double latitude1 = 2 * Math.Atan(Math.Exp(psi1)) - Constants.PI / 2;
            Double latitude2, psi;

            for (Int32 k = 0; k < 100; k++)
            {
                psi = Math.Log((Math.Tan(latitude1 / 2 + Constants.PI / 4)) * Math.Pow((1 - _ellipsoid.Eccentricity * Math.Sin(latitude1)) / (1 + _ellipsoid.Eccentricity * Math.Sin(latitude1)), _ellipsoid.Eccentricity / 2));
                latitude2 = latitude1 - (psi - psi1) * Math.Cos(latitude1) * (1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(latitude1) / (1 - _ellipsoid.EccentricitySquare));
                if (Math.Abs(latitude1 - latitude2) <= 1E-4)
                    break;

                latitude1 = latitude2;
            }
            Double latitude = latitude1;

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion
    }
}
