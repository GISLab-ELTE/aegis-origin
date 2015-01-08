///<copyright file="IGeometryConvexHullOperator.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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
    /// Defines generalized spatial convex hull computation operations on <see cref="IGeometry" /> instances.
    /// </summary>
    public interface IGeometryConvexHullOperator : IDisposable
    {
        /// <summary>
        /// Computes the convex hull of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The convex hull of the <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        IGeometry ConvexHull(IGeometry geometry);
    }
}
