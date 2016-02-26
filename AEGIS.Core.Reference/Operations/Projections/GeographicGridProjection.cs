/// <copyright file="EquidistantCylindricalSphericalProjection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Geographic Reference Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::735201", "Geographic grid projection")]
    public class GeographicGridProjection : GridProjection
    {
        #region Protected static fields

        protected static List<Char> _gridSigns;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicGridProjection" /> class.
        /// </summary>
        public GeographicGridProjection()
            : base(CoordinateOperationMethods.GeographicGridProjection.Identifier, CoordinateOperationMethods.GeographicGridProjection.Name, CoordinateOperationMethods.GeographicGridProjection, AreasOfUse.World)
        { 
            if (_gridSigns == null)
                _gridSigns = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        }

        #endregion

        #region Protected operation methods

        /// <summary>
        /// Computes the forward transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override String ComputeForward(GeoCoordinate coordinate)
        {
            // source: http://en.wikipedia.org/wiki/Georef

            if (!coordinate.IsValid)
                throw new ArgumentException("The coordinate is not valid.");

            // translation
            Double latitude = coordinate.Latitude.GetValue(UnitsOfMeasurement.Degree) + 90;
            Double longitude = coordinate.Longitude.GetValue(UnitsOfMeasurement.Degree) + 180;

            // 15°
            Int32 zone1Column = Convert.ToInt32(Math.Floor(longitude / 15));
            Int32 zone1Row = Convert.ToInt32(Math.Floor(latitude / 15));

            // 1°
            Int32 zone2Column = Convert.ToInt32(Math.Floor(longitude % 15));
            Int32 zone2Row = Convert.ToInt32(Math.Floor(latitude % 15));

            // minutes
            Int32 minutesEasting = Convert.ToInt32(Math.Floor((longitude - Math.Floor(longitude)) * 6000));
            Int32 minutesNorthing = Convert.ToInt32(Math.Floor((latitude - Math.Floor(latitude)) * 6000));

            StringBuilder builder = new StringBuilder(12);
            builder.Append(_gridSigns[zone1Column]);
            builder.Append(_gridSigns[zone1Row]);
            builder.Append(_gridSigns[zone2Column]);
            builder.Append(_gridSigns[zone2Row]);
            builder.Append(minutesEasting.ToString("D4"));
            builder.Append(minutesNorthing.ToString("D4"));
            return builder.ToString();
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(String coordinate)
        {
            // source: http://en.wikipedia.org/wiki/Georef
            
            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "The coordinate is null.");

            Double latitude = 0, longitude = 0;

            switch (coordinate.Length)
            { 
                case 2:
                    // 15° and translation
                    longitude += _gridSigns.IndexOf(coordinate[0]) * 15 - 180;
                    latitude += _gridSigns.IndexOf(coordinate[1]) * 15 - 90;
                    break;
                case 4:
                    // 1°
                    longitude += _gridSigns.IndexOf(coordinate[2]);
                    latitude += _gridSigns.IndexOf(coordinate[3]);
                    goto case 2;
                case 8:
                    // minutes
                    longitude = Double.Parse(coordinate.Substring(4, 2)) / 6000;
                    latitude = Double.Parse(coordinate.Substring(6, 2)) / 6000;
                    goto case 4;
                case 10:
                    // one tenth of minutes
                    longitude = Double.Parse(coordinate.Substring(4, 3)) / 6000;
                    latitude = Double.Parse(coordinate.Substring(7, 3)) / 6000;
                    goto case 4;
                case 12:
                    // one hundredth of minutes
                    longitude = Double.Parse(coordinate.Substring(4, 4)) / 6000;
                    latitude = Double.Parse(coordinate.Substring(8, 4)) / 6000;
                    goto case 4;
                default:
                    throw new ArgumentException("The coordinate is not a valid Georef string.", "coordinate");
            }

            return new GeoCoordinate(Angle.FromDegree(latitude), Angle.FromDegree(longitude));
        }

        #endregion
    }
}
