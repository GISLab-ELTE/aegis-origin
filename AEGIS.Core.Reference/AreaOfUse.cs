/// <copyright file="AreaOfUse.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using ELTE.AEGIS.Numerics;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents an area of use.
    /// </summary>
    public class AreaOfUse : IdentifiedObject
    {
        #region Private fields

        private readonly Angle _west;
        private readonly Angle _east;
        private readonly Angle _north;
        private readonly Angle _south;
        private readonly String _description;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the west boundary.
        /// </summary>
        /// <value>The angle of the west boundary.</value>
        public Angle West { get { return _west; } }

        /// <summary>
        /// Gets the east boundary.
        /// </summary>
        /// <value>The angle of the east boundary.</value>
        public Angle East { get { return _east; } }

        /// <summary>
        /// Gets the north boundary.
        /// </summary>
        /// <value>The angle of the north boundary.</value>
        public Angle North { get { return _north; } }

        /// <summary>
        /// Gets the south boundary.
        /// </summary>
        /// <value>The angle of the south boundary.</value>
        public Angle South { get { return _south; } }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description { get { return _description; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="west">The west boundary (in radians).</param>
        /// <param name="east">The east boundary (in radians).</param>
        /// <param name="north">The north boundary (in radians).</param>
        /// <param name="south">The south boundary (in radians).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, Double west, Double east, Double north, Double south)
            : this(identifier, name, null, null, null, west, east, north, south)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="south">The south boundary.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, Angle west, Angle east, Angle north, Angle south)
            : this(identifier, name, null, null, null, west, east, north, south)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="description">The description.</param>
        /// <param name="west">The west boundary (in radians).</param>
        /// <param name="east">The east boundary (in radians).</param>
        /// <param name="north">The north boundary (in radians).</param>
        /// <param name="south">The south boundary (in radians).</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, String remarks, String[] aliases, String description, Double west, Double east, Double north, Double south)
            : base(identifier, name, remarks, aliases)
        {
            _west = Angle.FromRadian(west);
            _east = Angle.FromRadian(east);
            _north = Angle.FromRadian(north);
            _south = Angle.FromRadian(south);
            _description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfUse" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="description">The description.</param>
        /// <param name="west">The west boundary.</param>
        /// <param name="east">The east boundary.</param>
        /// <param name="north">The north boundary.</param>
        /// <param name="south">The south boundary.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public AreaOfUse(String identifier, String name, String remarks, String[] aliases, String description, Angle west, Angle east, Angle north, Angle south)
            : base(identifier, name, remarks, aliases)
        {
            _west = west;
            _east = east;
            _north = north;
            _south = south;
            _description = description;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the area contains a geographic coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the coordinate is within the area; otherwise, <c>false</c>.</returns>
        public Boolean Contains(GeoCoordinate coordinate)
        {
            return (West <= East && West <= coordinate.Latitude && coordinate.Latitude <= East ||
                    West > East && (West - Angle.FromRadian(2 * Constants.PI) <= coordinate.Latitude && coordinate.Latitude <= East ||
                                    West <= coordinate.Latitude && coordinate.Latitude <= East + Angle.FromRadian(2 * Constants.PI)));
        }

        #endregion

        #region Static factory methods

        /// <summary>
        /// Creates an area of use from boundaries given in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="west">The west boundary (in degrees).</param>
        /// <param name="east">The east boundary (in degrees).</param>
        /// <param name="north">The north boundary (in degrees).</param>
        /// <param name="south">The south boundary (in degrees).</param>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromDegrees(String identifier, String name, Double west, Double east, Double north, Double south)
        {
            return new AreaOfUse(identifier, name, Angle.FromDegree(west), Angle.FromDegree(east), Angle.FromDegree(north), Angle.FromDegree(south));
        }

        /// <summary>
        /// Creates an area of use from boundaries given in degrees.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="west">The west boundary (in degrees).</param>
        /// <param name="east">The east boundary (in degrees).</param>
        /// <param name="north">The north boundary (in degrees).</param>
        /// <param name="south">The south boundary (in degrees).</param>
        /// <returns>The created area of use.</returns>
        public static AreaOfUse FromDegrees(String identifier, String name, String description, String remarks, Double west, Double east, Double north, Double south)
        {
            return new AreaOfUse(identifier, name, Angle.FromDegree(west), Angle.FromDegree(east), Angle.FromDegree(north), Angle.FromDegree(south));
        }

        #endregion
    }
}
