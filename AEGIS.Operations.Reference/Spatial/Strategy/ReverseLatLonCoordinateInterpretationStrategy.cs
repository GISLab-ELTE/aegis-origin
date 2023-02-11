// <copyright file="ReverseLatLonCoordinateInterpretationStrategy.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using System;
using ELTE.AEGIS.Reference;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Represents a reverse coordinate interpretation for (latitude, longitude) representation.
    /// </summary>
    public class ReverseLatLonCoordinateInterpretationStrategy : ITransformationStrategy<Coordinate, GeoCoordinate>
    {
        #region Private fields

        private readonly CoordinateReferenceSystem _referenceSystem;
        private readonly UnitOfMeasurement _latitudeUnit;
        private readonly UnitOfMeasurement _longitudeUnit;

        #endregion

        #region ITransformationStrategy properties

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem SourceReferenceSystem { get { return _referenceSystem; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem TargetReferenceSystem { get { return _referenceSystem; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseLatLonCoordinateInterpretationStrategy" /> class.
        /// </summary>
        /// <param name="referenceSystem">The reference system.</param>
        /// <exception cref="System.ArgumentNullException">referenceSystem;The reference system is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The coordinate system of the reference system does not match the conversion.;referenceSystem
        /// or
        /// The coordinate system of the reference system does not match the conversion.;referenceSystem
        /// </exception>
        public ReverseLatLonCoordinateInterpretationStrategy(CoordinateReferenceSystem referenceSystem)
        {
            if (referenceSystem == null)
                throw new ArgumentNullException("referenceSystem", "The reference system is null.");
            if (referenceSystem.CoordinateSystem.Dimension != 2)
                throw new ArgumentException("The coordinate system of the reference system does not match the conversion.", "referenceSystem");
            if (!referenceSystem.CoordinateSystem.GetAxis(0).Name.Equals("Geodetic latitude") ||
                !referenceSystem.CoordinateSystem.GetAxis(1).Name.Equals("Geodetic longitude"))
                throw new ArgumentException("The coordinate system of the reference system does not match the conversion.", "referenceSystem");

            _referenceSystem = referenceSystem;
            _latitudeUnit = _referenceSystem.CoordinateSystem.GetAxis(0).Unit;
            _longitudeUnit = _referenceSystem.CoordinateSystem.GetAxis(1).Unit;
        }

        #endregion

        #region ICoordinateTransformationStrategy methods

        /// <summary>
        /// Transforms the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public GeoCoordinate Transform(Coordinate coordinate)
        {
            return new GeoCoordinate(new Angle(coordinate.X, _latitudeUnit),
                                     new Angle(coordinate.Y, _longitudeUnit));

        }

        #endregion
    }
}
