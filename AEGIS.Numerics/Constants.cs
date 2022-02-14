/// <copyright file="Constants.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Numerics
{
    /// <summary>
    /// Defines mathematical constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The Euler number.
        /// </summary>
        public const Double E = 2.7182818284590452353602874713526624977572470937000;

        /// <summary>
        /// The Pi number.
        /// </summary>
        public const Double PI = 3.1415926535897932384626433832795028841971693993751;

        /// <summary>
        /// The speed of light (m/s).
        /// </summary>
        public const Double SpeedOfLight = 299792458;
        
        /// <summary>
        /// The value to convert degree to radian.
        /// </summary>
        public const Double DegreeToRadian = PI / 180;

        /// <summary>
        /// The value to convert radian to degree.
        /// </summary>
        public const Double RadianToDegree = 180 / PI;
    }
}
