///<copyright file="GeometryMeasure.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents a type peforming spatial measurement operations on <see cref="IGeometry"/> instances.
    /// </summary>
    public static class GeometryMeasure
    {
        #region Public static (extension) methods

        /// <summary>
        /// Computes the distance between the specified <see cref="IGeometry" /> instances.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="otherGeometry">The other geometry.</param>
        /// <returns>The distance between the <see cref="IGeometry" /> instances.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The geometry is null.
        /// or
        /// The other geometry is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry types.</exception>
        public static Double Distance(this IGeometry geometry, IGeometry otherGeometry)
        {
            using (IGeometryMeasureOperator op = GetOperator(geometry))
            {
                return op.Distance(geometry, otherGeometry);
            }
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Returns the measure operator for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The measure operator of the specified geometry, if any; otherwise, the default measure operator.</returns>
        public static IGeometryMeasureOperator GetOperator(IGeometry geometry)
        { 
            IGeometryOperatorFactory factory = geometry.Factory.GetFactory<IGeometryOperatorFactory>();

            if (factory == null)
                return Factory.DefaultInstance<IGeometryOperatorFactory>().Measure;

            return factory.Measure;
        }

        #endregion
    }
}
