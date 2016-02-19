/// <copyright file="IMultiCurve.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines behavior for multicurve geometries.
    /// </summary>
    /// <typeparam name="T">The type of the curve.</typeparam>
    public interface IMultiCurve<out T> : IGeometryCollection<T> where T : ICurve
    {
        /// <summary>
        /// Gets a value indicating whether the multicurve is closed.
        /// </summary>
        /// <value><c>true</c> if all curves within the multicure are closed; otherwise, <c>false</c>.</value>
        Boolean IsClosed { get; }
        /// <summary>
        /// Gets the length of the multicurve.
        /// </summary>
        /// <value>The <see cref="System.Double" /> instance indicating the length of the multicurve.</value>
        Double Length { get; }
    }
}
