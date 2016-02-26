/// <copyright file="LambertConicConformal2SPProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Conformal Projection (with 2 Standard Parallels).
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9802", "Lambert Conic Conformal (2SP)")]
    public class LambertConicConformal2SPProjection : LambertConicConformalProjection
    {
        #region Protected fields

        protected Double _latitudeOfFalseOrigin; // projection params
        protected Double _longitudeOfFalseOrigin;
        protected Double _latitudeOf1stStandardParallel;
        protected Double _latitudeOf2ndStandardParallel;
        protected Double _eastingAtFalseOrigin;
        protected Double _northingAtFalseOrigin;
        protected Double _rF; // projection constant
        protected Double _n;
        protected Double _f;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
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
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        public LambertConicConformal2SPProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.LambertConicConformal2SPProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 18

            _latitudeOfFalseOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfFalseOrigin]).BaseValue;
            _longitudeOfFalseOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfFalseOrigin]).BaseValue;
            _latitudeOf1stStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
            _latitudeOf2ndStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf2ndStandardParallel]).BaseValue;
            _eastingAtFalseOrigin = ((Length)_parameters[CoordinateOperationParameters.EastingAtFalseOrigin]).Value;
            _northingAtFalseOrigin = ((Length)_parameters[CoordinateOperationParameters.NorthingAtFalseOrigin]).Value;

            Double m1 = Math.Cos(_latitudeOf1stStandardParallel) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOf1stStandardParallel));
            Double m2 = Math.Cos(_latitudeOf2ndStandardParallel) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOf2ndStandardParallel));
            Double t1 = ComputeTValue(_latitudeOf1stStandardParallel);
            Double t2 = ComputeTValue(_latitudeOf2ndStandardParallel);
            Double tF = ComputeTValue(_latitudeOfFalseOrigin);
            _n = (Math.Log(m1) - Math.Log(m2)) / (Math.Log(t1) - Math.Log(t2));
            _f = m1 / (_n * Math.Pow(t1, _n));
            _rF = _ellipsoid.SemiMajorAxis.Value * _f * Math.Pow(tF, _n);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal2SPProjection" /> class.
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
        /// The parameter is not a double precision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        protected LambertConicConformal2SPProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 18

            _latitudeOfFalseOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfFalseOrigin]).BaseValue;
            _longitudeOfFalseOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfFalseOrigin]).BaseValue;
            _latitudeOf1stStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
            _latitudeOf2ndStandardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf2ndStandardParallel]).BaseValue;
            _eastingAtFalseOrigin = ((Length)_parameters[CoordinateOperationParameters.EastingAtFalseOrigin]).Value;
            _northingAtFalseOrigin = ((Length)_parameters[CoordinateOperationParameters.NorthingAtFalseOrigin]).Value;

            Double m1 = Math.Cos(_latitudeOf1stStandardParallel) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOf1stStandardParallel));
            Double m2 = Math.Cos(_latitudeOf2ndStandardParallel) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOf2ndStandardParallel));
            Double t1 = ComputeTValue(_latitudeOf1stStandardParallel);
            Double t2 = ComputeTValue(_latitudeOf2ndStandardParallel);
            Double tF = ComputeTValue(_latitudeOfFalseOrigin);
            _n = (Math.Log(m1) - Math.Log(m2)) / (Math.Log(t1) - Math.Log(t2));
            _f = m1 / (_n * Math.Pow(t1, _n));
            _rF = _ellipsoid.SemiMajorAxis.Value * _f * Math.Pow(tF, _n);
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
            // source: EPSG Guidance Note number 7, part 2, page 18

            Double t = ComputeTValue(coordinate.Latitude.BaseValue);
            Double r = _ellipsoid.SemiMajorAxis.Value * _f * Math.Pow(t, _n);
            Double theta = _n * (coordinate.Longitude.BaseValue - _longitudeOfFalseOrigin);
            Double easting, northing;

            ComputeCoordinate(r, theta, out easting, out northing);

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 18

            Double theta = Math.Atan((coordinate.X - _eastingAtFalseOrigin) / (_rF - (coordinate.Y - _northingAtFalseOrigin)));
            Double r = Math.Sign(_n) * Math.Sqrt(Calculator.Square(coordinate.X - _eastingAtFalseOrigin) + Calculator.Square(_rF - (coordinate.Y - _northingAtFalseOrigin)));
            Double t = Math.Pow(r / (_ellipsoid.SemiMajorAxis.Value * _f), 1 / _n);

            Double phi = ComputeLatitude(t);
            Double lambda = ComputeLongitude(theta); 

            return new GeoCoordinate(phi, lambda);
        }

        #endregion

        #region Protected utility methods

        /// <summary>
        /// Compute the coordinate easting and northing based on the R and Theta values.
        /// </summary>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        protected virtual void ComputeCoordinate(Double r, Double theta, out Double easting, out Double northing)
        {
            easting = _eastingAtFalseOrigin + r * Math.Sin(theta);
            northing = _northingAtFalseOrigin + _rF - r * Math.Cos(theta);
        }

        /// <summary>
        /// Computes the longitude.
        /// </summary>
        /// <param name="theta">The Theta value.</param>
        /// <returns>The longitude.</returns>
        protected virtual Double ComputeLongitude(Double theta)
        {
            return theta / _n + _longitudeOfFalseOrigin;
        }

        #endregion
    }
}
