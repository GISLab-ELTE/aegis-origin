/// <copyright file="GeocentricTranslation.cs" company="Eötvös Loránd University (ELTE)">
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
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a geocentric translation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::1031", "Geocentric translations (geocentric domain)")]
    public class GeocentricTranslation : GeocentricTransformation
    {
        #region Protected fields

        protected Double _xAxisTranslation;
        protected Double _yAxisTranslation;
        protected Double _zAxisTranslation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocentricTranslation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">The method requires parameteres which are not specified.</exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double percision floating-point number as required by the method.
        /// </exception>
        public GeocentricTranslation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, CoordinateOperationMethods.GeocentricTranslationGeocentricDomain, parameters)
        {
            _xAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.XAxisTranslation]).BaseValue;
            _yAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.YAxisTranslation]).BaseValue;
            _zAxisTranslation = ((Length)_parameters[CoordinateOperationParameters.ZAxisTranslation]).BaseValue;
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeForward(Coordinate coordinate)
        {
            return new Coordinate(coordinate.X + _xAxisTranslation, coordinate.Y + _yAxisTranslation, coordinate.Z + _zAxisTranslation);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override Coordinate ComputeReverse(Coordinate coordinate)
        {
            return new Coordinate(coordinate.X - _xAxisTranslation, coordinate.Y - _yAxisTranslation, coordinate.Z - _zAxisTranslation);
        }

        #endregion
    }
}
