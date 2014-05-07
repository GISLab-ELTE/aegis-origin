/// <copyright file="AxisDirection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Defines directions for coordinate system axis.
    /// </summary>
    /// <remarks>
    /// The direction of positive increase in the coordinate value for a coordinate system axis.
    /// </remarks>
    public enum AxisDirection
    {
        /// <summary>
        /// Other.
        /// </summary>
        Other,

        /// <summary>
        /// North.
        /// </summary>
        North, 

        /// <summary>
        /// North, north east.
        /// </summary>
        NorthNorthEast, 

        /// <summary>
        /// North east.
        /// </summary>
        NorthEast,

        /// <summary>
        /// East, north east.
        /// </summary>
        EastNorthEast, 

        /// <summary>
        /// East.
        /// </summary>
        East, 

        /// <summary>
        /// East, south east.
        /// </summary>
        EastSouthEast,

        /// <summary>
        /// South east.
        /// </summary>
        SouthEast, 

        /// <summary>
        /// South, south east.
        /// </summary>
        SouthSouthEast, 

        /// <summary>
        /// South.
        /// </summary>
        South, 

        /// <summary>
        /// South, south west.
        /// </summary>
        SouthSouthWest, 

        /// <summary>
        /// South west.
        /// </summary>
        SouthWest,

        /// <summary>
        /// West, south west.
        /// </summary>
        WestSouthWest, 

        /// <summary>
        /// West.
        /// </summary>
        West, 

        /// <summary>
        /// West, north west.
        /// </summary>
        WestNorthWest,

        /// <summary>
        /// North west.
        /// </summary>
        NorthWest, 

        /// <summary>
        /// North, north west.
        /// </summary>
        NorthNorthWest,

        /// <summary>
        /// Up.
        /// </summary>
        Up, 

        /// <summary>
        /// Down.
        /// </summary>
        Down,

        /// <summary>
        /// Geocentric X.
        /// </summary>
        GeocentricX,

        /// <summary>
        /// Geocentric Y.
        /// </summary>
        GeocentricY,

        /// <summary>
        /// Geocentric Z.
        /// </summary>
        GeocentricZ,

        /// <summary>
        /// Future.
        /// </summary>
        Future, 

        /// <summary>
        /// Past.
        /// </summary>
        Past,

        /// <summary>
        /// Column positive.
        /// </summary>
        ColumnPositive,
 
        /// <summary>
        /// Column negative.
        /// </summary>
        ColumnNegative, 

        /// <summary>
        /// Row positive.
        /// </summary>
        RowPositive, 

        /// <summary>
        /// Row negative.
        /// </summary>
        RowNegative,

        /// <summary>
        /// Display right.
        /// </summary>
        DisplayRight,

        /// <summary>
        /// Display left.
        /// </summary>
        DisplayLeft,

        /// <summary>
        /// Display up.
        /// </summary>
        DisplayUp,

        /// <summary>
        /// Display down.
        /// </summary>
        DisplayDown
    }
}
