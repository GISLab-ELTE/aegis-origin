// <copyright file="RelativePosition.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal
{
    /// <summary>
    /// Identifies the relative temporal relationships.
    /// </summary>
    /// <remarks>
    /// Relative positioning is defined according to James F. Allen. 1983. Maintaining knowledge about 
    /// temporal intervals. Commun. ACM 26, 11 (November 1983), 832-843.
    /// Besides noted positions and undefined value is available for indeterminate situations.
    /// </remarks>
    public enum RelativePosition
    {
        /// <summary>
        /// The position is before the position of the other.
        /// </summary>
        Before,

        /// <summary>
        /// The position is after the position of the other.
        /// </summary>
        After,

        /// <summary>
        /// The position begins at the position of the other.
        /// </summary>
        Begins,

        /// <summary>
        /// The position ends at the position of the other.
        /// </summary>
        Ends,

        /// <summary>
        /// The position is during the position of the other.
        /// </summary>
        During,

        /// <summary>
        /// The position is equal to the position of the other.
        /// </summary>
        Equals,

        /// <summary>
        /// The position contains the position of the other.
        /// </summary>
        Contains,

        /// <summary>
        /// The position overlaps the position of the other.
        /// </summary>
        Overlaps,

        /// <summary>
        /// The position meets the position of the other.
        /// </summary>
        Meets,

        /// <summary>
        /// The position is overlapped by the position of the other.
        /// </summary>
        OverlappedBy,

        /// <summary>
        /// The position is met by the position of the other.
        /// </summary>
        MetBy,

        /// <summary>
        /// The position of the other begins at the position of the current instance.
        /// </summary>
        BegunBy,

        /// <summary>
        /// The position of the other ends at the position of the current instance.
        /// </summary>
        EndedBy,

        /// <summary>
        /// The relative position is undefined.
        /// </summary>
        Undefined
    }
}
