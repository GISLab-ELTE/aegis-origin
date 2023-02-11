// <copyright file="VerticalDatum.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Defines a vertical datum.
    /// </summary>
    public class VerticalDatum : Datum
    {
        #region Private fields

        private readonly AreaOfUse _areaOfUse;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the geodetic datum is applicable.</value>
        public AreaOfUse AreaOfUse { get { return _areaOfUse; } }

        /// <summary>
        /// Gets the subtype of the vertical datum.
        /// </summary>
        /// <value>The subtype of the vertical datum.</value>
        public VerticalDatumType Type 
        { 
            get 
            {
                if (Identifier.Substring(0, 4) == "EPSG")
                {
                    UInt16 code = UInt16.Parse(Identifier.Substring(5, 9));
                    if (code >= 5000 && code < 5099)
                        return VerticalDatumType.Ellipsoidal;
                    else if (code >= 5100 && code < 5899)
                        return VerticalDatumType.Orthometric;
                    else
                        return VerticalDatumType.Unknown;
                }
                else
                    return VerticalDatumType.Unknown;
            } 
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalDatum" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="anchorPoint">The anchor point.</param>
        /// <param name="realizationEpoch">The realization epoch.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <exception cref="System.ArgumentNullException">The ellipsoid is null.</exception>
        public VerticalDatum(String identifier, String name, String anchorPoint, String realizationEpoch, AreaOfUse areaOfUse) : base(identifier, name, anchorPoint, realizationEpoch) 
        {
            _areaOfUse = areaOfUse;
        }

        #endregion
    }
}
