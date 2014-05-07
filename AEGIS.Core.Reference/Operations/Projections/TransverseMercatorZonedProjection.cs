/// <copyright file="TransverseMercatorZonedProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Zoned Transverse Mercator Projection.
    /// </summary>
    [IdentifiedObjectInstance("EPSG::9824", "Transverse Mercator Zoned Grid System")]    
    public class TransverseMercatorZonedProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // projection params
        protected Double _initialLongitude;
        protected Double _zoneWidth;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _scaleFactorAtNaturalOrigin;
        protected Double _M0; // projection constant
        protected Double _e2; // ellipsoid eccentricities
        protected Double _e4;
        protected Double _e6;
        protected Double _e8;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TransverseMercatorZonedProjection" /> class.
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
        /// The parameter is not a double percision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public TransverseMercatorZonedProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.TransverseMercatorZonedProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 47

            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _initialLongitude = ((Angle)_parameters[CoordinateOperationParameters.InitialLongitude]).BaseValue;
            _zoneWidth = ((Angle)_parameters[CoordinateOperationParameters.ZoneWidth]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;
            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);

            _e2 = Calculator.Pow(_ellipsoid.Eccentricity, 2);
            _e4 = Calculator.Pow(_ellipsoid.Eccentricity, 4);
            _e6 = Calculator.Pow(_ellipsoid.Eccentricity, 6);
            _e8 = Calculator.Pow(_ellipsoid.Eccentricity, 8);
            _M0 = ComputeM(_latitudeOfNaturalOrigin);
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
            // source: EPSG Guidance Note number 7, part 2, page 52

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue > 0 ? coordinate.Longitude.BaseValue : coordinate.Longitude.BaseValue + Constants.PI;

            Int32 zoneNumber = Convert.ToInt32(Math.Floor((lambda + _initialLongitude + _zoneWidth) / _zoneWidth));
            Double longitudeOfNaturalOrigin = zoneNumber * _zoneWidth - _initialLongitude + _zoneWidth / 2;

            // source: EPSG Guidance Note number 7, part 2, page 48

            Double t = Calculator.Tan4(phi);
            Double c = _e2 * Calculator.Cos2(phi) / (1 - _e2);
            Double a = (lambda - longitudeOfNaturalOrigin) * Math.Cos(phi);
            Double m = ComputeM(phi);

            Double easting = zoneNumber * 1E6 + _falseEasting + _scaleFactorAtNaturalOrigin * _ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * (a + (1 - t + c) * Calculator.Pow(a, 3) / 6 + (5 - 18 * t + Calculator.Square(t) + 72 * c - 58 * Calculator.Square(_ellipsoid.SecondEccentricity)) * Calculator.Pow(a, 5) / 120);
            Double northing = _falseNorthing + _scaleFactorAtNaturalOrigin * (m - _M0 + _ellipsoid.RadiusOfPrimeVerticalCurvature(phi) * Math.Tan(phi) * (Calculator.Square(a) / 2 + (5 - t + 9 * c + 4 * Calculator.Square(c)) * Calculator.Pow(a, 4) / 24 + (61 - 58 * t + Calculator.Square(t) + 600 * c - 330 * Calculator.Square(_ellipsoid.SecondEccentricity)) * Calculator.Pow(a, 6) / 720));

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 48

            Int32 zoneNumber = Convert.ToInt32(Math.Floor(coordinate.X - _falseEasting) / 1E6);

            Double m1 = _M0 + (coordinate.Y - _falseNorthing);
            Double nu1 = m1 / (_ellipsoid.SemiMajorAxis.BaseValue * (1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256));
            Double e1 = (1 - Math.Sqrt(1 - _e2)) / (1 + Math.Sqrt(1 - _e2));
            Double phi1 = nu1 + (3 * e1 / 2 - 27 * Calculator.Pow(e1, 3) / 32) * Math.Sin(2 * nu1) +
                                (21 * Calculator.Pow(e1, 2) / 16 - 55 * Calculator.Pow(e1, 4) / 32) * Math.Sin(4 * nu1) +
                                (151 * Calculator.Pow(e1, 3) / 96) * Math.Sin(6 * nu1) + (1097 * Calculator.Pow(e1, 4) / 512) * Math.Sin(8 * nu1);
            Double d = (coordinate.X - (_falseEasting + zoneNumber * 1E6)) / (_ellipsoid.RadiusOfMeridianCurvature(phi1) * _scaleFactorAtNaturalOrigin);
            Double t1 = Calculator.Tan2(phi1);
            Double c1 = Calculator.Square(_ellipsoid.SecondEccentricity) * Calculator.Cos2(phi1);

            // source: EPSG Guidance Note number 7, part 2, page 52

            Double phi = phi1 - (_ellipsoid.RadiusOfPrimeVerticalCurvature(phi1) * Math.Tan(phi1) / _ellipsoid.RadiusOfMeridianCurvature(phi1)) * (Calculator.Square(d) / 2 - (1 + 3 * t1) * Calculator.Pow(d, 4) / 24) +
                                (61 + 90 * t1 + 298 * c1 + 45 * Calculator.Square(t1) - 252 * Calculator.Square(_ellipsoid.SecondEccentricity) - 3 * Calculator.Square(c1) * Calculator.Pow(d, 6) / 720);
            Double lambda = (coordinate.X - (_falseEasting + zoneNumber * 1E6)) / (_ellipsoid.RadiusOfMeridianCurvature(phi1) * _scaleFactorAtNaturalOrigin);

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the M value.
        /// </summary>
        /// <param name="latitude">The latitude (expressed in radian).</param>
        /// <returns>The M Value.</returns>
        protected Double ComputeM(Double latitude)
        {
            return _ellipsoid.SemiMajorAxis.BaseValue * ((1 - _e2 / 4 - 3 * _e4 / 64 - 5 * _e6 / 256) * latitude -
                                                         (3 * _e2 / 8 + 3 * _e4 / 32 + 45 * _e6 / 1024) * Math.Sin(2 * latitude) +
                                                         (15 * _e4 / 256 + 45 * _e6 / 1024) * Math.Sin(4 * latitude) -
                                                         35 * _e6 / 3072 * Math.Sin(6 * latitude));
        }

        #endregion
    }
}
