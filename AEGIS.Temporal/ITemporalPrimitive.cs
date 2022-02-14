/// <copyright file="ITemporalPrimitive.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Defines a primitive temporal object.
    /// </summary>
    public interface ITemporalPrimitive : IEquatable<ITemporalPrimitive>
    {
        /// <summary>
        /// Gets the length of the <see cref="ITemporalPrimitive" />.
        /// </summary>
        /// <value>The length of the period.</value>
        Duration Length { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ITemporalPrimitive" /> instance is valid.
        /// </summary>
        /// <value><c>true</c> if the order represents a valid temporal context; otherwise, <c>false</c>.</value>
        Boolean IsValid { get; }

        /// <summary>
        /// Computes the distance to the other <see cref="ITemporalPrimitive" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="Duration" /> representing the distance to the other <see cref="ITemporalPrimitive" /> instance.</returns>
        /// <exception cref="System.InvalidOperationException">The instance is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">The other instance is null.</exception>
        /// <exception cref="System.ArgumentException">The other instance is not valid.</exception>
        Duration Distance(ITemporalPrimitive other);

        /// <summary>
        /// Determines the relative position compared to the other <see cref="ITemporalPrimitive" /> instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>The <see cref="RelativePosition" /> of this instance compared to the other.</returns>
        /// <exception cref="System.InvalidOperationException">The instance is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">The other instance is null.</exception>
        /// <exception cref="System.ArgumentException">The other instance is not valid.</exception>
        RelativePosition RelativePosition(ITemporalPrimitive other);
    }
}
