/// <copyright file="ShapeType.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.IO.Shapefile
{
    /// <summary>
    /// Defines the types of shapes.
    /// </summary>
    enum ShapeType
    {
        /// <summary>
        /// Null.
        /// </summary>
        Null = 0,

        /// <summary>
        /// Point.
        /// </summary>
        Point = 1,

        /// <summary>
        /// Poly line.
        /// </summary>
        PolyLine = 3,

        /// <summary>
        /// Polygon.
        /// </summary>
        Polygon = 5,

        /// <summary>
        /// Multi point.
        /// </summary>
        MultiPoint = 8,

        /// <summary>
        /// 3D pont.
        /// </summary>
        PointZ = 11,

        /// <summary>
        /// 3D poly line.
        /// </summary>
        PolyLineZ = 13,

        /// <summary>
        /// 3D polygon.
        /// </summary>
        PolygonZ = 15,

        /// <summary>
        /// 3D multi point.
        /// </summary>
        MultiPointZ = 18,

        /// <summary>
        /// Point with measure.
        /// </summary>
        PointM = 21,

        /// <summary>
        /// Poly line with measure.
        /// </summary>
        PolyLineM = 23,

        /// <summary>
        /// Polygon with measure.
        /// </summary>
        PolygonM = 25,

        /// <summary>
        /// Multi point with measure.
        /// </summary>
        MultiPointM = 28,

        /// <summary>
        /// Multi patch.
        /// </summary>
        MultiPatch = 31
    }
}
