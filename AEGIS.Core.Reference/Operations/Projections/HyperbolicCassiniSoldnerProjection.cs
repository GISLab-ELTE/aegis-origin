/// <copyright file="HyperbolicCassiniSoldnerProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Hyperbolic Cassini-Soldner projection.
    /// </summary>
    [IdentifiedObjectInstance("EPSG::9833", "Hyperbolic Cassini-Soldner")]
    public class HyperbolicCassiniSoldnerProjection : CassiniSoldnerProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperbolicCassiniSoldnerProjection" /> class.
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
        public HyperbolicCassiniSoldnerProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.HyperbolicCassiniSoldnerProjection, parameters, ellipsoid, areaOfUse)
        {
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Computes the northing.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="x">The X value.</param>
        /// <returns>The northing.</returns>
        protected override Double ComputeNorthing(GeoCoordinate coordinate, Double x)
        {
            // source: EPSG Guidance Note number 7, part 2, page 42

            return _falseNorthing + x - (Calculator.Pow(x, 3) / (6 * _ellipsoid.RadiusOfMeridianCurvature(coordinate.Latitude.BaseValue) * _ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude.BaseValue)));
        }

        /// <summary>
        /// Computes the M1 parameter.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The M1 value.</returns>
        protected override Double ComputeM1(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 42

            Double phi1 = _latitudeOfNaturalOrigin + (coordinate.Y - _falseNorthing) / 315320;
            Double ro = _ellipsoid.RadiusOfMeridianCurvature(phi1);
            Double v1 = _ellipsoid.RadiusOfPrimeVerticalCurvature(phi1);
            Double q_ = Calculator.Pow((coordinate.Y - _falseNorthing), 3) / (6 * ro * v1);
            Double q = Calculator.Pow((coordinate.Y - _falseNorthing + q_), 3) / (6 * ro * v1);

            return _M0 + (coordinate.Y - _falseNorthing) + q;
        }

        #endregion
    }
}
