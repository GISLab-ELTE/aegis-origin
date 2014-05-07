/// <copyright file="GridProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a grid projection.
    /// </summary>
    public abstract class GridProjection : CoordinateConversion<GeoCoordinate, String>
    {
        #region Protected fields

        protected readonly AreaOfUse _areaOfUse;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the operation is applicable.</value>
        public AreaOfUse AreaOfUse { get { return _areaOfUse; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GridProjection" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation.</param>
        /// <param name="name">The name of the operation.</param>
        /// <param name="method">The coordinate operation method.</param>
        /// <param name="areaOfUse">The area of use where the operation is applicable.</param>
        /// <exception cref="System.ArgumentNullException">The area of use is null.</exception>
        public GridProjection(String identifier, String name, CoordinateOperationMethod method, AreaOfUse areaOfUse)
            : base(identifier, name, method, null)
        {
            if (areaOfUse == null)
                throw new ArgumentNullException("areaOfUse", "The area of use is null.");

            _areaOfUse = areaOfUse;
        }

        #endregion
    }
}
