/// <copyright file="MolodenskyTransformation.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a Molodensky Transformation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9604", "Molodensky")]
    public class MolodenskyTransformation : GeographicTransformation
    {
        #region Protected fields

        protected readonly Ellipsoid _ellipsoid;
        protected Double _xAxisTranslation;
        protected Double _yAxisTranslation;
        protected Double _zAxisTranslation;
        protected Double _semiMajorAxisLengthDifference;
        protected Double _flatteningDifference;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the ellipsoid.
        /// </summary>
        /// <value>The ellipsoid model of Earth.</value>
        public Ellipsoid Ellipsoid { get { return _ellipsoid; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MolodenskyTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <param name="source">The source coordinate reference system.</param>
        /// <param name="target">The target coordinate reference system.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameteres which are not specified.
        /// or
        /// The source coordinate reference system is null.
        /// or
        /// The target coordinate reference system is null.
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
        protected MolodenskyTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters, CoordinateReferenceSystem source, CoordinateReferenceSystem target, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
            : base(identifier, name, CoordinateOperationMethods.MolodenskyTransformation, parameters, source, target, areaOfUse)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");

            _ellipsoid = ellipsoid;
            _xAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.XAxisTranslation]).BaseValue;
            _yAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.YAxisTranslation]).BaseValue;
            _zAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.ZAxisTranslation]).BaseValue;
            _semiMajorAxisLengthDifference = ((Length)_parameters[CoordinateOperationParameters.SemiMajorAxisLengthDifference]).BaseValue;
            _flatteningDifference = Convert.ToDouble(_parameters[CoordinateOperationParameters.FlatteningDifference]);
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate coordinate)
        {
            // source: EPSG Guidance Note number 7, part 2, page 130

            Double d = _ellipsoid.SemiMajorAxis.BaseValue * _flatteningDifference + _ellipsoid.Flattening * _semiMajorAxisLengthDifference;
            Double deltaPhi = (-_xAxisTranslation * Math.Sin(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue) - _yAxisTranslation * Math.Sin(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue) + _zAxisTranslation * Math.Cos(coordinate.Latitude.BaseValue) + d) / Ellipsoid.RadiusOfMeridianCurvature(coordinate.Latitude.BaseValue);
            Double deltaLambda = (-_xAxisTranslation * Math.Sin(coordinate.Longitude.BaseValue) + _yAxisTranslation * Math.Cos(coordinate.Longitude.BaseValue)) / Ellipsoid.RadiusOfPrimeVerticalCurvature(coordinate.Latitude.BaseValue) / Math.Cos(coordinate.Latitude.BaseValue);
            Double deltaH = _xAxisTranslation * Math.Cos(coordinate.Latitude.BaseValue) * Math.Cos(coordinate.Longitude.BaseValue) + _yAxisTranslation * Math.Cos(coordinate.Latitude.BaseValue) * Math.Sin(coordinate.Longitude.BaseValue) + d * Calculator.Sin2(coordinate.Latitude.BaseValue) - _semiMajorAxisLengthDifference;

            return new GeoCoordinate(coordinate.Latitude.BaseValue + deltaPhi, coordinate.Longitude.BaseValue + deltaLambda, coordinate.Height.BaseValue + deltaH);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate coordinate)
        {
            throw new NotSupportedException("Coordinate operation is not reversible.");
        }

        #endregion
    }
}
