/// <copyright file="IGeometryOperatorFactory.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior of factories producing geometry operators.
    /// </summary>
    public interface IGeometryOperatorFactory : IFactory
    {
        /// <summary>
        /// Gets the measure operator.
        /// </summary>
        /// <value>The measure operator.</value>
        IGeometryMeasureOperator Measure { get; }

        /// <summary>
        /// Gets the overlay operator.
        /// </summary>
        /// <value>The overlay operator.</value>
        IGeometryOverlayOperator Overlay { get; }

        /// <summary>
        /// Gets the relate operator.
        /// </summary>
        /// <value>The relate operator.</value>
        IGeometryRelateOperator Relate { get; }
    }
}
