// <copyright file="IGeometryBufferOperator.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Defines generalized spatial buffering operations on <see cref="IGeometry" /> instances.
    /// </summary>
    public interface IGeometryBufferOperator : IDisposable
    {
        /// <summary>
        /// Computes the buffer of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <returns>The buffer of the specified <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        IGeometry Buffer(IGeometry geometry, Double distance);
    }
}
