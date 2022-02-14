/// <copyright file="SpectralRanges.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a collections of known <see cref="SpectralRange" /> instances.
    /// </summary>
    public static class SpectralRanges
    {
        #region Private static fields

        private static SpectralRange _farInfrared;
        private static SpectralRange _longWavelengthInfrared;
        private static SpectralRange _midWavelengthInfrared;
        private static SpectralRange _shortWavelengthInfrared;
        private static SpectralRange _nearInfrared;
        private static SpectralRange _infrared;
        private static SpectralRange _red;
        private static SpectralRange _orange;
        private static SpectralRange _yellow;
        private static SpectralRange _green;
        private static SpectralRange _blue;
        private static SpectralRange _violet;
        private static SpectralRange _visible;
        private static SpectralRange _ultraviolet;

        #endregion

        #region Public static properties

        /// <summary>
        /// Far infrared.
        /// </summary>
        public static SpectralRange FarInfrared
        {
            get
            {
                return _farInfrared ?? (_farInfrared = new SpectralRange(15e-6, 1000e-6));
            }
        }

        /// <summary>
        /// Long-wavelength infrared.
        /// </summary>
        public static SpectralRange LongWavelengthInfrared
        {
            get
            {
                return _longWavelengthInfrared ?? (_longWavelengthInfrared = new SpectralRange(8e-6, 15e-6));
            }
        }

        /// <summary>
        /// Mid-wavelength infrared.
        /// </summary>
        public static SpectralRange ShortWavelengthInfrared
        {
            get
            {
                return _midWavelengthInfrared ?? (_midWavelengthInfrared = new SpectralRange(3e-6, 8e-6));
            }
        }

        /// <summary>
        /// Short-wavelength infrared.
        /// </summary>
        public static SpectralRange MiddleWavelengthInfrared
        {
            get
            {
                return _shortWavelengthInfrared ?? (_shortWavelengthInfrared = new SpectralRange(1.4e-6, 3e-6));
            }
        }

        /// <summary>
        /// Near infrared.
        /// </summary>
        public static SpectralRange NearInfrared
        {
            get
            {
                return _nearInfrared ?? (_nearInfrared = new SpectralRange(0.75e-6, 1.4e-6));
            }
        }

        /// <summary>
        /// Infrared.
        /// </summary>
        public static SpectralRange Infrared
        {
            get
            {
                return _infrared ?? (_infrared = new SpectralRange(0.75e-6, 1000e-6));
            }
        }

        /// <summary>
        /// Red.
        /// </summary>
        public static SpectralRange Red
        {
            get
            {
                return _red ?? (_red = new SpectralRange(62e-6, 75e-6));
            }
        }

        /// <summary>
        /// Orange.
        /// </summary>
        public static SpectralRange Orange
        {
            get
            {
                return _orange ?? (_orange = new SpectralRange(59e-8, 62e-8));
            }
        }

        /// <summary>
        /// Yellow.
        /// </summary>
        public static SpectralRange Yellow
        {
            get
            {
                return _yellow ?? (_yellow = new SpectralRange(57e-8, 59e-8));
            }
        }

        /// <summary>
        /// Green.
        /// </summary>
        public static SpectralRange Green
        {
            get
            {
                return _green ?? (_green = new SpectralRange(495e-9, 57e-8));
            }
        }

        /// <summary>
        /// Blue.
        /// </summary>
        public static SpectralRange Blue
        {
            get
            {
                return _blue ?? (_blue = new SpectralRange(45e-8, 495e-9));
            }
        }

        /// <summary>
        /// Violet.
        /// </summary>
        public static SpectralRange Violet
        {
            get
            {
                return _violet ?? (_violet = new SpectralRange(38e-8, 45e-8));
            }
        }

        /// <summary>
        /// Visible.
        /// </summary>
        public static SpectralRange Visible
        {
            get
            {
                return _visible ?? (_visible = new SpectralRange(38e-8, 75e-8));
            }
        }

        /// <summary>
        /// Ultraviolet.
        /// </summary>
        public static SpectralRange Ultraviolet
        {
            get
            {
                return _ultraviolet ?? (_ultraviolet = new SpectralRange(1e-8, 38e-8));
            }
        }

        #endregion
    }
}
