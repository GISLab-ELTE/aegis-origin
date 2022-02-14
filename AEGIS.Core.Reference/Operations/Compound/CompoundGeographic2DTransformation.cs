/// <copyright file="CompoundGeographic2DTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a compound coordinate transformation in the geographic 2D domain.
    /// </summary>
    public class CompoundGeographic2DTransformation : GeographicTransformation
    {
        #region Protected fields

        private readonly Geographic3DTo2DConversion _3DTo2DConversion;
        private readonly GeographicToGeocentricConversion _sourceConversion;
        private readonly GeographicToGeocentricConversion _targetConversion;
        private readonly GeocentricTransformation _transformation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcatenatedGeographic2DTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="source">The source geographic coordinate reference system.</param>
        /// <param name="target">The target geographic coordinate reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentException">The specified method does not contain 5 submethods.</exception>
        public CompoundGeographic2DTransformation(String identifier, String name, CompoundCoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target, AreaOfUse areaOfUse)
            : base(identifier, name, method, null, source, target, areaOfUse)
        {
            if (method.Methods.Count != 5)
                throw new ArgumentException("The specified method does not contain 5 submethods.", "method");

            _3DTo2DConversion = new Geographic3DTo2DConversion();
            _sourceConversion = new GeographicToGeocentricConversion((source.Datum as GeodeticDatum).Ellipsoid);
            _transformation = GeocentricTransformationFactory.FromMethod(method.Methods[2], parameters);
            _targetConversion = new GeographicToGeocentricConversion((target.Datum as GeodeticDatum).Ellipsoid);
        }

        #endregion

        #region Protected Operations methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate coordinate)
        {
            coordinate = _3DTo2DConversion.Reverse(coordinate);
            Coordinate geocentricCoordinate = _sourceConversion.Forward(coordinate);
            geocentricCoordinate = _transformation.Forward(geocentricCoordinate);
            coordinate = _targetConversion.Reverse(geocentricCoordinate);
            coordinate = _3DTo2DConversion.Forward(coordinate);
            return coordinate;
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate coordinate)
        {
            coordinate = _3DTo2DConversion.Forward(coordinate);
            Coordinate geocentricCoordinate = _targetConversion.Forward(coordinate);
            geocentricCoordinate = _transformation.Reverse(geocentricCoordinate);
            coordinate = _sourceConversion.Reverse(geocentricCoordinate);
            coordinate = _3DTo2DConversion.Reverse(coordinate);
            return coordinate;
        }

        #endregion
    }
}
