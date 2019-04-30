/// <copyright file="SpectralStatistics.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
    /// Defines statistics computes for spectral data.
    /// </summary>
    [Flags]
    public enum SpectralStatistics
    {
        /// <summary>
        /// Indicates the computation of no statistics.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates the computation of variance.
        /// </summary>
        Variance = 1,

        /// <summary>
        /// Indicates the computation of covariance.
        /// </summary>
        Covariance = 2,

        /// <summary>
        /// Indicates the computation of comoment.
        /// </summary>
        Comoment = 4,

        /// <summary>
        /// Indicates the computation of all statistics.
        /// </summary>
        All = 7
    }
}
