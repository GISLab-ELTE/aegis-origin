/// <copyright file="SpectralRange.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a range in the electromagnetic spectrum.
    /// </summary>
    public class SpectralRange : IEquatable<SpectralRange>
    {
        #region Private fields

        /// <summary>
        /// The minimum wavelength (in meters).
        /// </summary>
        private readonly Double _wavelengthMinimum;

        /// <summary>
        /// The maximum wavelength (in meters).
        /// </summary>
        private readonly Double _wavelengthMaximum;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralRange" /> class.
        /// </summary>
        /// <param name="wavelengthBegin">The lower bound of the wavelength (in metre).</param>
        /// <param name="wavelengthEnd">The upper bound of the wavelength (in metre).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The lower bound of the wavelength is less than or equal to 0.
        /// or
        /// The lower bound of the wavelength if greater than the upper bound.
        /// </exception>
        public SpectralRange(Double wavelengthBegin, Double wavelengthEnd)
        {
            if (wavelengthBegin <= 0)
                throw new ArgumentOutOfRangeException("The lower bound of the wavelength is less than or equal to 0.", nameof(wavelengthBegin));
            if (wavelengthEnd < wavelengthBegin)
                throw new ArgumentOutOfRangeException("The lower bound of the wavelength if greater than the upper bound.", nameof(wavelengthEnd));

            _wavelengthMinimum = wavelengthBegin;
            _wavelengthMaximum = wavelengthEnd;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the lower bound of the wavelength (in meters).
        /// </summary>
        public Double WavelengthMinimum { get { return _wavelengthMinimum; } }

        /// <summary>
        /// Gets the upper bound of the wavelength (in meters).
        /// </summary>
        public Double WavelengthMaximum { get { return _wavelengthMaximum; } }

        /// <summary>
        /// Gets the lower bound of the frequency (in hertz).
        /// </summary>
        public Double FrequencyMinimum
        {
            get
            {
                return Constants.SpeedOfLight / _wavelengthMaximum;
            }
        }

        /// <summary>
        /// Gets the upper bound of the frequency (in hertz).
        /// </summary>
        public Double FrequencyMaximum
        {
            get
            {
                return Constants.SpeedOfLight / _wavelengthMinimum;
            }
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether this instance and a specified other <see cref="SpectralRange" /> are equal.
        /// </summary>
        /// <param name="obj">Another <see cref="SpectralRange" /> to compare to.</param>
        /// <returns><c>true</c> if <paramref name="another" /> and this instance represent the same value; otherwise, <c>false</c>.</returns>
        public Boolean Equals(SpectralRange another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return _wavelengthMinimum == another._wavelengthMinimum && _wavelengthMaximum == another._wavelengthMaximum;
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (obj is SpectralRange && _wavelengthMinimum == (obj as SpectralRange)._wavelengthMinimum && _wavelengthMaximum == (obj as SpectralRange)._wavelengthMaximum);
        }
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override Int32 GetHashCode()
        {
            return (_wavelengthMinimum.GetHashCode() >> 2) ^ _wavelengthMaximum.GetHashCode() ^ 897318403;
        }
        /// <summary>
        /// Returns the <see cref="System.String" /> equivalent of the instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> containing the coordinates in all dimensions.</returns>
        public override String ToString()
        {
            return "[" + _wavelengthMinimum + "m, " + _wavelengthMaximum + "m]";
        }

        #endregion
    }
}
