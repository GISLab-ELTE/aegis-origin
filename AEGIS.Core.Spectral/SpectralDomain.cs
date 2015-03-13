/// <copyright file="SpectralDomain.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents domains of the electromagnetic spectrum.
    /// </summary>
    [Flags]
    public enum SpectralDomain
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Radar.
        /// </summary>
        Radar = 1,

        /// <summary>
        /// Far infrared.
        /// </summary>
        FarInfrared = 2,

        /// <summary>
        /// Long-wavelength (thermal) infrared.
        /// </summary>
        LongWavelengthInfrared = 4,

        /// <summary>
        /// Mid-wavelength infrared.
        /// </summary>
        MidWavelengthInfrared = 8,

        /// <summary>
        /// Short-wavelength infrared.
        /// </summary>
        ShortWavelengthInfrared = 16,

        /// <summary>
        /// Middle infrared.
        /// </summary>
        MiddleInfrared = 28,

        /// <summary>
        /// Near infrared.
        /// </summary>
        NearInfrared = 32,

        /// <summary>
        /// Infrared.
        /// </summary>
        Infrared = 62,

        /// <summary>
        /// Red.
        /// </summary>
        Red = 64,

        /// <summary>
        /// Orange.
        /// </summary>
        Orange = 128,

        /// <summary>
        /// Yellow.
        /// </summary>
        Yellow = 256,

        /// <summary>
        /// Green.
        /// </summary>
        Green = 512,

        /// <summary>
        /// Blue.
        /// </summary>
        Blue = 1024,

        /// <summary>
        /// Violet.
        /// </summary>
        Violet = 2048,

        /// <summary>
        /// Visible.
        /// </summary>
        Visible = 4032,

        /// <summary>
        /// Ultraviolet.
        /// </summary>
        Ultraviolet = 4096
    }
}
