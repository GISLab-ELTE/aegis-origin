/// <copyright file="LasPointFormat5.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Antal Tar</author>

namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 5 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat5 : LasPointFormat3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat5" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat5(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 5;
        }

        /// <summary>
        /// Gets or sets the record index of the wave packet.
        /// </summary>
        public Byte WavePacketDescriptorIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes to the wave packet.
        /// </summary>
        public UInt64 WavePacketDataOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of the wave packet in bytes.
        /// </summary>
        public UInt32 WavePacketSize { get; set; }

        /// <summary>
        /// Gets or sets the offset in picoseconds to the location that the associated return pulse was detected.
        /// </summary>
        public Single ReturnPointWaveformLocation { get; set; }

        /// <summary>
        /// Gets or sets the X(t) parameter.
        /// </summary>
        public Single Xt { get; set; }

        /// <summary>
        /// Gets or sets the Y(t) parameter.
        /// </summary>
        public Single Yt { get; set; }

        /// <summary>
        /// Gets or sets the Z(t) parameter.
        /// </summary>
        public Single Zt { get; set; }
    }
}
