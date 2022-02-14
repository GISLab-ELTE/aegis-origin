/// <copyright file="EquidistantCylindricalProjection.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics.Integral;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents an Equidistant Cylindrical Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::1028", "Equidistant Cylindrical")]
    public class EquidistantCylindricalProjection : CoordinateProjection
    {
        #region Protected fields

        protected Double _latitudeOf1stStadardParallel; // projection params
        protected Double _longitudeOfNaturalOrigin;
        protected Double _falseEasting;
        protected Double _falseNorthing;
        protected Double _nu1; // projection constant

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EquidistantCylindricalProjection" /> class.
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
        /// </exception>
        public EquidistantCylindricalProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.EquidistantCylindricalProjection, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 76

            _latitudeOf1stStadardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _nu1 = _ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOf1stStadardParallel));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformEquidistantCylindricalProjection" /> class.
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
        protected EquidistantCylindricalProjection(String identifier, String name, CoordinateOperationMethod method, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, method, parameters, ellipsoid, areaOfUse)
        {
            // source: EPSG Guidance Note number 7, part 2, page 76

            _latitudeOf1stStadardParallel = ((Angle)_parameters[CoordinateOperationParameters.LatitudeOf1stStandardParallel]).BaseValue;
            _longitudeOfNaturalOrigin = ((Angle)_parameters[CoordinateOperationParameters.LongitudeOfNaturalOrigin]).BaseValue;
            _falseEasting = ((Length)_parameters[CoordinateOperationParameters.FalseEasting]).Value;
            _falseNorthing = ((Length)_parameters[CoordinateOperationParameters.FalseNorthing]).Value;

            _nu1 = _ellipsoid.SemiMajorAxis.Value / Math.Sqrt(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(_latitudeOf1stStadardParallel));
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
            // source: EPSG Guidance Note number 7, part 2, page 76

            Double easting, northing;

            if (_ellipsoid.IsSphere)
            {
                easting = _falseEasting + _ellipsoid.SemiMajorAxis.Value * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin) * Math.Cos(_latitudeOf1stStadardParallel);
                northing = _falseNorthing + _ellipsoid.SemiMajorAxis.Value * coordinate.Latitude.BaseValue;
            }
            else
            {
                Double latitude = coordinate.Latitude.BaseValue;
                Double m = _ellipsoid.SemiMajorAxis.Value * (1 - _ellipsoid.EccentricitySquare) * SimpsonMethod.Integrate(phi => Math.Pow(1 - _ellipsoid.EccentricitySquare * Calculator.Sin2(phi), -1.5), 0, latitude, 100);

                easting = _falseEasting + _nu1 * Math.Cos(_latitudeOf1stStadardParallel) * (coordinate.Longitude.BaseValue - _longitudeOfNaturalOrigin);
                northing = _falseNorthing + m;
            }

            return new Coordinate(easting, northing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 76

            Double phi, lambda;

            if (_ellipsoid.IsSphere)
            {
                phi = (coordinate.Y - _falseNorthing) / _ellipsoid.SemiMajorAxis.Value;
                lambda = _longitudeOfNaturalOrigin + (coordinate.X - _falseEasting) / _ellipsoid.SemiMajorAxis.Value / Math.Cos(_latitudeOf1stStadardParallel);
            }
            else
            {
                Double x = coordinate.X - _falseEasting;
                Double y = coordinate.Y - _falseNorthing;

                Double mu = y / (_ellipsoid.SemiMajorAxis.Value * (1
                    - 1d / 4 * _ellipsoid.EccentricitySquare
                    - 3d / 64 * Calculator.Pow(_ellipsoid.Eccentricity, 4)
                    - 5d / 256 * Calculator.Pow(_ellipsoid.Eccentricity, 6)
                    - 175d / 16384 * Calculator.Pow(_ellipsoid.Eccentricity, 8)
                    - 441d / 65536 * Calculator.Pow(_ellipsoid.Eccentricity, 10)
                    - 4851d / 1048576 * Calculator.Pow(_ellipsoid.Eccentricity, 12)
                    - 14157d / 4194304 * Calculator.Pow(_ellipsoid.Eccentricity, 14)));
                Double n = (1 - Math.Sqrt(1 - _ellipsoid.EccentricitySquare)) / (1 + Math.Sqrt(1 - _ellipsoid.EccentricitySquare));

                lambda = _longitudeOfNaturalOrigin + x / (_nu1 * Math.Cos(_latitudeOf1stStadardParallel));
                phi = mu
                    + (3d / 2 * n - 27d / 32 * Calculator.Pow(n, 3) + 269d / 512 * Calculator.Pow(n, 5) - 6607d / 24576 * Calculator.Pow(n, 7)) * Math.Sin(2 * mu)
                    + (21d / 16 * Calculator.Pow(n, 2) - 55d / 32 * Calculator.Pow(n, 4) + 6759d / 4096 * Calculator.Pow(n, 6)) * Math.Sin(4 * mu)
                    + (151d / 96 * Calculator.Pow(n, 3) - 417d / 128 * Calculator.Pow(n, 5) + 87963d / 20480 * Calculator.Pow(n, 7)) * Math.Sin(6 * mu)
                    + (1097d / 512 * Calculator.Pow(n, 4) - 15543d / 2560 * Calculator.Pow(n, 6)) * Math.Sin(8 * mu)
                    + (8011d / 2560 * Calculator.Pow(n, 5) - 69119d / 6144 * Calculator.Pow(n, 7)) * Math.Sin(10 * mu)
                    + (293393d / 61440 * Calculator.Pow(n, 6)) * Math.Sin(12 * mu)
                    + (6845701d / 860160 * Calculator.Pow(n, 7)) * Math.Sin(14 * mu);
            }

            return new GeoCoordinate(phi, lambda);
        }

        #endregion
    }
}
