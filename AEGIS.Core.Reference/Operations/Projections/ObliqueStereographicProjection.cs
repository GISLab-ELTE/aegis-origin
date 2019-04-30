/// <copyright file="ObliqueStereographicProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents an Oblique Stereographic Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9809", "Oblique Stereographic")]
    public class ObliqueStereographicProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // transformation params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _scaleFactorAtNaturalOrigin;
        protected Double _radiusOfConformalSphere;
        protected Double _conformalLatitudeOfNaturalOrigin; // equivalent sphere params
        protected Double _conformalLongitudeOfNaturalOrigin;
        protected Double _n; // projection constants
        protected Double _c;
        protected Double _g;
        protected Double _h;

        #endregion Protected fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObliqueStereographicProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
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
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// or
        /// The latitude of natural origin is a pole.
        /// </exception>
        public ObliqueStereographicProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.ObliqueStereographicProjection, parameters, ellipsoid, areaOfUse)
        {
            if (Math.Abs(((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue - Constants.PI) <= Calculator.Tolerance)
                throw new ArgumentException("The latitude of natural origin is a pole.", "parameters");

            // source: EPSG Guidance Note number 7, part 2, page 62

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;
            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);

            if (!_ellipsoid.IsSphere) // in case of ellipsoid values have to projected to the sphere
            {
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
            else // in case of sphere values need to be copied
            {
                _radiusOfConformalSphere = _ellipsoid.SemiMajorAxis.Value;

                _conformalLatitudeOfNaturalOrigin = _latitudeOfNaturalOrigin;
                _conformalLongitudeOfNaturalOrigin = _longitudeOfNaturalOrigin;
            }

            _g = 2 * _radiusOfConformalSphere * _scaleFactorAtNaturalOrigin * Math.Tan(Constants.PI / 4 - _conformalLatitudeOfNaturalOrigin / 2);
            _h = 4 * _radiusOfConformalSphere * _scaleFactorAtNaturalOrigin * Math.Tan(_conformalLatitudeOfNaturalOrigin) + _g;
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
            // source: EPSG Guidance Note number 7, part 2, page 62

            Double conformalLongitude, conformalLatitude;
            if (!_ellipsoid.IsSphere)
            {                
                conformalLongitude = _n * (coordinate.Longitude.BaseValue - _conformalLongitudeOfNaturalOrigin) + _conformalLongitudeOfNaturalOrigin;

                Double A = (1 + Math.Sin(coordinate.Latitude.BaseValue)) / (1 - Math.Sin(coordinate.Latitude.BaseValue));
                Double B = (1 - _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue)) / (1 + _ellipsoid.Eccentricity * Math.Sin(coordinate.Latitude.BaseValue));
                Double w = _c * Math.Pow(A * Math.Pow(B, _ellipsoid.Eccentricity), _n);

                conformalLatitude = Math.Asin((w - 1) / (w + 1));
            }
            else
            {
                conformalLatitude = coordinate.Latitude.BaseValue;
                conformalLongitude = coordinate.Longitude.BaseValue;
            }

            Double b = 1 + Math.Sin(conformalLatitude) * Math.Sin(_conformalLatitudeOfNaturalOrigin) + Math.Cos(conformalLatitude) * Math.Cos(_conformalLatitudeOfNaturalOrigin) * Math.Cos(conformalLongitude - _conformalLongitudeOfNaturalOrigin);
            Double easting = _falseEasting + 2 * _radiusOfConformalSphere * _scaleFactorAtNaturalOrigin * Math.Cos(conformalLatitude) * Math.Sin(conformalLongitude - _conformalLongitudeOfNaturalOrigin) / b;
            Double northing = _falseNorthing + 2 * _radiusOfConformalSphere * _scaleFactorAtNaturalOrigin * (Math.Sin(conformalLatitude) * Math.Cos(_conformalLatitudeOfNaturalOrigin) - Math.Cos(conformalLatitude) * Math.Sin(_conformalLatitudeOfNaturalOrigin) * Math.Cos(conformalLongitude - _conformalLongitudeOfNaturalOrigin)) / b;

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 63

            Double i = Math.Atan((coordinate.X - _falseEasting) / (_h + coordinate.Y - _falseNorthing));
            Double j = Math.Atan((coordinate.X - _falseEasting) / (_g - coordinate.Y + _falseNorthing)) - i;

            Double conformalLongitude = j + 2 * i + _conformalLongitudeOfNaturalOrigin;
            Double conformalLatitude = _conformalLatitudeOfNaturalOrigin + 2 * Math.Tanh((coordinate.Y - _falseNorthing - (coordinate.X - _falseEasting) * Math.Tan(j / 2)) / (2 * _radiusOfConformalSphere * _scaleFactorAtNaturalOrigin));

            Double latitude, longitude;
            if (!_ellipsoid.IsSphere)
            {
                longitude = (conformalLongitude - _conformalLongitudeOfNaturalOrigin) / _n + _conformalLongitudeOfNaturalOrigin;

                Double psi1 = Math.Log((1 + Math.Sin(conformalLatitude)) / (_c * (1 - Math.Sin(conformalLatitude)))) / _n / 2;
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
                latitude = latitude1;
            }
            else
            {
                longitude = conformalLongitude;
                latitude = conformalLatitude;
            }

            return new GeoCoordinate(latitude, longitude);
        }

        #endregion
    }
}
