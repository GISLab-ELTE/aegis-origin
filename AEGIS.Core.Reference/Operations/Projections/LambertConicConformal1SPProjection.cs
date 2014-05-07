/// <copyright file="LambertConicConformal1SPProjection.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Lambert Conic Conformal Projection (with 1 Standard Parallel).
    /// </summary>
    [IdentifiedObjectInstance("EPSG::9801", "Lambert Conic Conformal (1SP)")]
    public class LambertConicConformal1SPProjection : LambertConicConformalProjection
    {
        #region Protected fields

        protected Double _latitudeOfNaturalOrigin; // projection params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _scaleFactorAtNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _r0; // projection constant
        protected Double _n;
        protected Double _f;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPProjection" /> class.
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
        public LambertConicConformal1SPProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.LambertConicConformal1SPProjection, parameters, ellipsoid, areaOfUse)
        {
            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            // source: EPSG Guidance Note number 7, part 2, page 18

            Double m0 = Math.Cos(_latitudeOfNaturalOrigin) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfNaturalOrigin));
            Double t0 = ComputeTValue(_latitudeOfNaturalOrigin);
            _n = Math.Sin(_latitudeOfNaturalOrigin);
            _f = m0 / (_n * Math.Pow(t0, _n));
            _r0 = _ellipsoid.SemiMajorAxis.Value * _f * Math.Pow(t0, _n) * _scaleFactorAtNaturalOrigin;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LambertConicConformal1SPProjection" /> class.
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
        /// The parameter is not a double percision floating-point number as required by the method.
        /// or
        /// The parameter does not have the same measurement unit as the ellipsoid.
        /// </exception>
        protected LambertConicConformal1SPProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            _latitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOfNaturalOrigin]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _scaleFactorAtNaturalOrigin = Convert.ToDouble(_parameters[CoordinateOperationParameters.ScaleFactorAtNaturalOrigin]);
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            // source: EPSG Guidance Note number 7, part 2, page 18

            Double m0 = Math.Cos(_latitudeOfNaturalOrigin) / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOfNaturalOrigin));
            Double t0 = ComputeTValue(_latitudeOfNaturalOrigin);
            _n = Math.Sin(_latitudeOfNaturalOrigin);
            _f = m0 / (_n * Math.Pow(t0, _n));
            _r0 = _ellipsoid.SemiMajorAxis.Value * _f * Math.Pow(t0, _n) * _scaleFactorAtNaturalOrigin;
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
            // source: EPSG Guidance Note number 7, part 2, page 19

            Double t = ComputeTValue(coordinate.Latitude.BaseValue);
            Double r = _ellipsoid.SemiMajorAxis.Value * _f * Math.Pow(t, _n) * _scaleFactorAtNaturalOrigin;
            Double theta = _n * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
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
            // source: EPSG Guidance Note number 7, part 2, page 19

            Double r, theta;
            ComputeParams(coordinate.X, coordinate.Y, out r, out theta);

            Double t = Math.Pow(r / (_ellipsoid.SemiMajorAxis.Value * _scaleFactorAtNaturalOrigin * _f), 1 / _n);

            Double phi = ComputeLatitude(t);
            Double lambda = theta / _n + _longitudeOfNaturalOrigin;

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
            easting = _falseEasting + r * Math.Sin(theta);
            northing = _falseNorthing + _r0 - r * Math.Cos(theta);
        }

        /// <summary>
        /// Compute the transformations parameters R and Theta based on the easting and northing values.
        /// </summary>
        /// <param name="easting">The easting of the coordinate.</param>
        /// <param name="northing">The northing of the coordinate.</param>
        /// <param name="r">The R value.</param>
        /// <param name="theta">The Theta value.</param>
        protected virtual void ComputeParams(Double easting, Double northing, out Double r, out Double theta)
        {
            theta = Math.Atan((easting - _falseEasting) / (_r0 - (northing - _falseNorthing)));
            r = Math.Sign(_n) * Math.Sqrt(Calculator.Square(easting - _falseEasting) + Calculator.Square(_r0 - (northing - _falseNorthing)));
        }

        #endregion
    }
}
