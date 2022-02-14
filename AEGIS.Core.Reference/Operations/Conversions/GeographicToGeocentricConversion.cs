/// <copyright file="GeographicToGeocentricConversion.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a geographic to geocentric conversion.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9602", "Geographic/geocentric conversion")]
    public class GeographicToGeocentricConversion : CoordinateConversion<GeoCoordinate, Coordinate>
    {
        #region Protected fields

        protected readonly Ellipsoid _ellipsoid;

        #endregion Protected fields

        #region Public properties

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid used by the operation.</value>
        public Ellipsoid Ellipsoid { get { return _ellipsoid; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicToGeocentricConversion" /> class.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public GeographicToGeocentricConversion(Ellipsoid ellipsoid)
            : base(CoordinateOperationMethods.GeographicToGeocentricConversion.Identifier, CoordinateOperationMethods.GeographicToGeocentricConversion.Name, CoordinateOperationMethods.GeographicToGeocentricConversion, null)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");

            _ellipsoid = ellipsoid;
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
            // source: EPSG Guidance Note number 7, part 2, page 89

            Double lambda = coordinate.Longitude.BaseValue;
            Double phi = coordinate.Latitude.BaseValue;
            Double h = coordinate.Height.BaseValue;
            Double nu = _ellipsoid.RadiusOfPrimeVerticalCurvature(phi);
            Double x = (nu + h) * Math.Cos(phi) * Math.Cos(lambda);
            Double y = (nu + h) * Math.Cos(phi) * Math.Sin(lambda);
            Double z = ((1 - _ellipsoid.EccentricitySquare) * nu + h) * Math.Sin(phi);

            return new Coordinate(x, y, z);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 89
            // Bowring's formula

            Double p = Math.Sqrt(Calculator.Square(coordinate.X) + Calculator.Square(coordinate.Y));
            Double q = Math.Atan(coordinate.Z * _ellipsoid.SemiMajorAxis.Value / p / _ellipsoid.SemiMinorAxis.Value);
            Double phi = Math.Atan((coordinate.Z + Calculator.Square(_ellipsoid.SecondEccentricity) * _ellipsoid.SemiMinorAxis.Value * Calculator.Sin3(q)) / (p - _ellipsoid.EccentricitySquare * _ellipsoid.SemiMajorAxis.Value * Calculator.Cos3(q)));
            Double nu = _ellipsoid.RadiusOfPrimeVerticalCurvature(phi);
            Double lambda = Math.Atan(coordinate.Y / coordinate.X);
            Double height = coordinate.X * Calculator.Sec(lambda) * Calculator.Sec(phi) - nu;

            return new GeoCoordinate(phi, lambda, height);
        }

        #endregion
    }
}
