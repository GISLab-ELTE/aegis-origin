/// <copyright file="BonneSouthOrientatedProjection.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Krisztián Fodor</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Bonne South Orientated projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9828", "Bonne South Orientated")]
    public class BonneSouthOrientatedProjection : BonneProjection
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BonneSouthOrientatedProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        public BonneSouthOrientatedProjection(String identifier, String name, Dictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.BonneSouthOrientated, parameters, ellipsoid, areaOfUse)
        {
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
            // source: EPSG Guidance Note number 7, part 2, page 79

            Double phi = coordinate.Latitude.BaseValue;
            Double lambda = coordinate.Longitude.BaseValue;

            Double m = Computem(phi);
            Double m0 = Computem(_latitudeOfNaturalOrigin);
            Double M = ComputeM(phi);
            Double M0 = ComputeM(_latitudeOfNaturalOrigin);

            Double rho = _ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) + M0 - M;
            Double T = _ellipsoid.SemiMajorAxis.Value * m * (lambda - _longitudeOfNaturalOrigin) / rho;

            Double westing = _falseEasting - (rho * Math.Sin(T));
            Double southing = _falseNorthing - (_ellipsoid.SemiMajorAxis.Value * m0 / Math.Sin(_latitudeOfNaturalOrigin) - rho * Math.Cos(T));

            return new Coordinate(westing, southing);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(Coordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 80

            Double x = _falseEasting - coordinate.X;
            Double y = _falseNorthing - coordinate.Y;

            return ComputeReverseInternal(x, y);
        }

        #endregion
    }
}
