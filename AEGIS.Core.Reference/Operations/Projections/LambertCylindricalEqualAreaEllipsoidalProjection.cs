/// <copyright file="LambertCylindricalEqualAreaEllipsoidalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Szabó</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents the Lambert Cylindrical Equal Area (ellipsoidal case) projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9835", "Lambert Cylindrical Equal Area (ellipsoidal case)")]
    public class LambertCylindricalEqualAreaEllipsoidalProjection : CoordinateProjection
    {
        #region Protected fields

        /// <summary>
        /// The false easting parameter.
        /// </summary>
        protected Double _falseEasting;

        /// <summary>
        /// The false northing parameter.
        /// </summary>
        protected Double _falseNorthing;

        /// <summary>
        /// The latitude of the 1st standard parallel parameter.
        /// </summary>
        protected Double _latitudeOf1stStandardParallel;

        /// <summary>
        /// The latitude of natural origin parameter.
        /// </summary>
        protected Double _longitudeOfNaturalOrigin;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaEllipsoidalProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
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
        public LambertCylindricalEqualAreaEllipsoidalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : this(identifier, name, CoordinateOperationMethods.LambertCylindricalEqualAreaEllipsoidalProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaEllipsoidalProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
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
        protected LambertCylindricalEqualAreaEllipsoidalProjection(String identifier, String name, CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            _falseEasting = ((Length)(parameters[CoordinateOperationParameters.FalseEasting])).BaseValue;
            _falseNorthing = ((Length)(parameters[CoordinateOperationParameters.FalseNorthing])).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)(parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin])).BaseValue;
            _latitudeOf1stStandardParallel = ((Angle)parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(GeoCoordinate coordinate)
        {
            Double easting, northing;

            Double a = _ellipsoid.SemiMajorAxis.Value;
            Double e = _ellipsoid.Eccentricity;
            Double k_0 = ComputeK(_latitudeOf1stStandardParallel, e);
            Double q = ComputeQ(coordinate.Latitude.BaseValue, e);

            easting = _falseEasting + a * k_0 * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
            northing = _falseNorthing + a * q / (2 * k_0);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double phi = 0, lambda = 0;
            Double eccentricity = _ellipsoid.Eccentricity;
            Double k_0 = ComputeK(_latitudeOf1stStandardParallel, eccentricity);
            Double q_p = ComputeQ(Angle.FromDegree(90).BaseValue, eccentricity);
            Double beta = Math.Asin(2 * coordinate.Y * k_0 / (_ellipsoid.SemiMajorAxis.Value * q_p));

            lambda = _longitudeOfNaturalOrigin + coordinate.X / (_ellipsoid.SemiMajorAxis.Value * k_0);

            Double beta_c = Math.Atan(Math.Tan(beta) / Math.Cos(lambda - _longitudeOfNaturalOrigin));
            Double q_c = q_p * Math.Sin(beta_c);

            Double phi_c = 0;

            do
            {
                phi = phi_c;
                phi_c = phi_c +
                        Math.Pow(1 - Math.Pow(eccentricity, 2) * Math.Pow(Math.Sin(phi_c), 2), 2) / (2 * Math.Cos(phi_c)) *
                        (q_c / (1 - Math.Pow(eccentricity, 2)) -
                         Math.Sin(phi_c) / (1 - Math.Pow(eccentricity, 2) * Math.Pow(Math.Sin(phi_c), 2)) +
                         1 / (2 * eccentricity) *
                         Math.Log((1 - eccentricity * Math.Sin(phi_c)) / (1 + eccentricity * Math.Sin(phi_c))));
            } while (Math.Abs(phi_c - phi) > 0.00001);

            phi = phi_c;

            return new GeoCoordinate(phi, lambda);
        }

        /// <summary>
        /// Computes the K argument.
        /// </summary>
        /// <param name="latitudeOf1StStandardParallel">The latitude of the 1st standard parallel.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>The K argument.</returns>
        protected Double ComputeK(Double latitudeOf1StStandardParallel, Double eccentricity)
        {
            return Math.Cos(latitudeOf1StStandardParallel) / Math.Sqrt(1 - Math.Pow(eccentricity, 2) * Math.Pow(Math.Sin(latitudeOf1StStandardParallel), 2));
        }

        /// <summary>
        /// Computes the Q argument.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="eccentricity">The eccentricity.</param>
        /// <returns>The Q argument</returns>
        protected Double ComputeQ(Double latitude, Double eccentricity)
        {
            return (1 - Math.Pow(eccentricity, 2)) * (Math.Sin(latitude) / (1 - Math.Pow(eccentricity, 2) * Math.Pow(Math.Sin(latitude), 2)) - (1 / (2 * eccentricity)) * Math.Log((1 - eccentricity * Math.Sin(latitude)) / (1 + eccentricity * Math.Sin(latitude))));
        }

        #endregion
    }
}