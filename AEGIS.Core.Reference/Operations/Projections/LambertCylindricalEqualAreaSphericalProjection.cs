/// <copyright file="LambertCylindricalEqualAreaSphericalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamás Szabó</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents the Lambert Cylindrical Equal Area (spherical case) projection.
    /// </summary>
    [IdentifiedObjectInstance("EPSG::9834", "Lambert Cylindrical Equal Area (spherical case)")]
    public class LambertCylindricalEqualAreaSphericalProjection : LambertCylindricalEqualAreaEllipsoidalProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertCylindricalEqualAreaSphericalProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The method requires parameteres which are not specified.
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
        /// or
        /// The ellipsoid is not spherical.
        /// </exception>
        public LambertCylindricalEqualAreaSphericalProjection(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.LambertCylindricalEqualAreaSphericalProjection, parameters, ellipsoid, areaOfUse)
        {
            if (!_ellipsoid.IsSphere)
                throw new ArgumentException("The ellipsoid is not spherical.", "ellipsoid");
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
            Double easting = _falseEasting +
                             _ellipsoid.SemiMajorAxis.Value * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin) *
                             Math.Cos(_latitudeOf1stStandardParallel);
            Double northing = _falseNorthing +
                              _ellipsoid.SemiMajorAxis.Value * Math.Sin(coordinate.Latitude.BaseValue) /
                              Math.Cos(_latitudeOf1stStandardParallel);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            Double phi = Math.Asin((coordinate.Y / _ellipsoid.SemiMajorAxis.Value) * Math.Cos(_latitudeOf1stStandardParallel));
            Double lambda = coordinate.X / (_ellipsoid.SemiMajorAxis.Value * Math.Cos(_latitudeOf1stStandardParallel)) +
                            _longitudeOfNaturalOrigin;

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}