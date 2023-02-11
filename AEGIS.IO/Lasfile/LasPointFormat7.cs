// <copyright file="LasPointFormat7.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 7 geometry in Cartesian coordinate space.
    /// </summary>
    /// <author>Antal Tar</author>
    public class LasPointFormat7 : LasPointFormat6
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat7" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat7(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 7;
        }

        /// <summary>
        /// Gets or sets the normalised value of the red color channel associated with the point.
        /// </summary>
        public UInt16 RedChannel { get; set; }

        /// <summary>
        /// Gets or sets the normalised value of the green color channel associated with the point.
        /// </summary>
        public UInt16 GreenChannel { get; set; }

        /// <summary>
        /// Gets or sets the normalised value of the blue color channel associated with the point.
        /// </summary>
        public UInt16 BlueChannel { get; set; }
    }
}
