// <copyright file="Meridian.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a meridian.
    /// </summary>
    public class Meridian : IdentifiedObject
    {
        #region Private fields

        private readonly Angle _longitude;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the longitude angle of the meridian.
        /// </summary>
        /// <value>The longitude angle of the meridian.</value>
        public Angle Longitude { get { return _longitude; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Meridian" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude (in radians).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public Meridian(String identifier, String name, Double longitude)
            : base(identifier, name)
        {
            _longitude = Angle.FromRadian(longitude);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Meridian" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public Meridian(String identifier, String name, Angle longitude) 
            : base(identifier, name)
        {
            _longitude = longitude;
        }

        #endregion

        #region Static factory methods

        /// <summary>
        /// Creates a meridian from the longitude specified in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude (in degrees).</param>
        /// <returns>The meridian based on the specified longitude.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">Longitude value must be between -180 and 180 degrees.</exception>
        public static Meridian FromDegrees(String identifier, String name, Double longitude)
        {
            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude value must be between -180 and 180 degrees.", "longitude");

            return new Meridian(identifier, name, Angle.FromDegree(longitude));
        }

        /// <summary>
        /// Creates a meridian from the longitude specified in radians.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The meridian based on the specified longitude.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        /// <exception cref="System.ArgumentException">Longitude value must be between -PI and +PI radians.</exception>
        public static Meridian FromRadian(String identifier, String name, Double longitude)
        {
            if (longitude < -Constants.PI || longitude > Constants.PI)
                throw new ArgumentException("Longitude value must be between -PI and +PI radians.", "longitude");

            return new Meridian(identifier, name, Angle.FromRadian(longitude));
        }

        #endregion
    }
}
