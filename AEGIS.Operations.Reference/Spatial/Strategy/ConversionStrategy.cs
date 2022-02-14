/// <copyright file="ConversionStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Reference;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Represents a conversion strategy.
    /// </summary>
    /// <remarks>
    /// A conversion strategy is composed of two conversions (converting to geographic, and from geographic coordinates).
    /// </remarks>
    public class ConversionStrategy : ITransformationStrategy
    {
        #region Private fields

        private ITransformationStrategy<Coordinate, GeoCoordinate> _conversionToGeographic;
        private ITransformationStrategy<GeoCoordinate, Coordinate> _conversionFromGeographic;

        #endregion

        #region ITransformationStrategy properties

        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        public ReferenceSystem SourceReferenceSystem { get { return _conversionToGeographic.SourceReferenceSystem; } }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        public ReferenceSystem TargetReferenceSystem { get { return _conversionFromGeographic.TargetReferenceSystem; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionStrategy" /> class.
        /// </summary>
        /// <param name="conversionToGeographic">The conversion to geographic coordinate.</param>
        /// <param name="conversionFromGeographic">The conversion from geographic coordinate.</param>
        public ConversionStrategy(ITransformationStrategy<Coordinate, GeoCoordinate> conversionToGeographic, ITransformationStrategy<GeoCoordinate, Coordinate> conversionFromGeographic)
        {
            _conversionFromGeographic = conversionFromGeographic;
            _conversionToGeographic = conversionToGeographic;
        }

        #endregion

        #region ITransformationStrategy methods

        /// Transforms the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        public Coordinate Transform(Coordinate coordinate)
        {
            return _conversionFromGeographic.Transform(_conversionToGeographic.Transform(coordinate));
        }

        #endregion
    }
}
