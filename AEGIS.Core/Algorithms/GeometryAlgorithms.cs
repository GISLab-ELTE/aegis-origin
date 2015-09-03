/// <copyright file="GeometryAlgorithms.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Linq;
using ELTE.AEGIS.Geometry;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Contains general algorithms for geometries.
    /// </summary>
    public static class GeometryAlgorithms
    {
        #region Validation

        /// <summary>
        /// Validates the specified geometry or returns <c>null</c> for invalid geometries.
        /// </summary>
        /// <remarks>
        /// The algorithm fixes the issues produced by the application of <see cref="PrecisionModel"/>.
        /// </remarks>
        /// <param name="geometry">The geometry.</param>
        /// <returns>A valid geometry or <c>null</c>.</returns>
        public static IGeometry Validate(IGeometry geometry)
        {
            if (geometry is IPolygon)
                return Validate(geometry as IPolygon);
            else if (geometry is IGeometryCollection<IGeometry>)
            {
                IGeometryFactory factory = geometry.Factory;
                var collection = new GeometryList<IGeometry>(factory, null);
                foreach (IGeometry subGeometry in (IGeometryCollection<IGeometry>) geometry)
                {
                    collection.Add(Validate(subGeometry));
                }
                while (collection.Remove(null)) ;
                if (collection.Count == 0)
                    collection = null;
                return collection;
            }
            else
                return geometry.IsValid ? geometry : null;
        }

        /// <summary>
        /// Validates the specified polygon or returns <c>null</c> for invalid polygons.
        /// The algorithm may produce a polygon, a line or a point beside the <c>null</c> value.
        /// </summary>
        /// <remarks>
        /// The algorithm fixes the issues produced by the application of <see cref="PrecisionModel"/>.
        /// </remarks>
        /// <param name="polygon">The polygon.</param>
        /// <returns>A valid geometry or <c>null</c>.</returns>
        private static IGeometry Validate(IPolygon polygon)
        {
            if (polygon.IsValid)
                return polygon;

            for (Int32 i = polygon.Shell.Count - 1; i > 0; --i)
                if (polygon.Shell.Coordinates[i] == polygon.Shell.Coordinates[i - 1])
                {
                    try
                    {
                        polygon.Shell.RemoveAt(i);
                    }
                    catch (InvalidOperationException) // A linear ring must contain at least two coordinates.
                    {
                        Coordinate coordinate = polygon.Shell.Coordinates[0];
                        return new Point(coordinate.X, coordinate.Y, coordinate.Z, polygon.Factory, null);
                    }
                }

            if (polygon.Shell.Count == 2)
            {
                Coordinate coordinate = polygon.Shell.Coordinates[0];
                return new Point(coordinate.X, coordinate.Y, coordinate.Z, polygon.Factory, null);
            }

            if (polygon.Shell.Count <= 3)
            {
                return new Line(polygon.Shell.Coordinates[0], polygon.Shell.Coordinates[1], polygon.Factory, null);
            }

            for (Int32 j = polygon.HoleCount - 1; j > 0; --j)
            {
                ILinearRing hole = polygon.Holes[j];
                for (Int32 i = hole.Count - 1; i > 0; --i)
                    if (hole.Coordinates[i] == hole.Coordinates[i - 1])
                    {
                        try
                        {
                            hole.RemoveAt(i);
                        }
                        catch (InvalidOperationException) // A linear ring must contain at least two coordinates.
                        {
                            break;
                        }
                    }

                if (hole.Count <= 3)
                    polygon.Holes.RemoveAt(j);
            }

            return polygon.IsValid ? polygon : null;
        }

        #endregion
    }
}
