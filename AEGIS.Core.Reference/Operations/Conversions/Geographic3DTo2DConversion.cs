/// <copyright file="Geographic3DTo2DConversion.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a geographic 3D to 2D conversion.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("EPSG::9659", "Geographic3D to 2D conversion")]
    public class Geographic3DTo2DConversion : CoordinateConversion<GeoCoordinate, GeoCoordinate>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Geographic3DTo2DConversion" /> class.
        /// </summary>
        public Geographic3DTo2DConversion()
            : base("EPSG::9659", "Geographic3D to 2D conversion", CoordinateOperationMethods.Geographic3DTo2DConversion, null)
        {
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeForward(GeoCoordinate location)
        {
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(GeoCoordinate location)
        {
            return location;
        }

        #endregion
    }
}
