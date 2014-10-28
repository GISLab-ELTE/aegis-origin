/// <copyright file="MilitaryGridProjection.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a Military Grid Projection.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::735202", "Military grid projection")]
    public class MilitaryGridProjection : GridProjection
    {
        #region Protected fields

        protected static List<Char> _gridSigns;
        protected static List<Char> _rowSigns;
        protected static List<Char> _columnSigns;
        protected static List<Char> _polarRowSigns;
        protected static List<Char> _polarColumnSigns;
        protected const Double _zoneWidth = 6 * Constants.DegreeToRadian;
        protected const Double _zoneHeight = 8 * Constants.DegreeToRadian;
        protected const Int32 _rowHeight = 100000;
        protected const Int32 _columnWidth = 100000;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MilitaryGridProjection" /> class.
        /// </summary>
        public MilitaryGridProjection()
            : base(CoordinateOperationMethods.MilitaryGridProjection.Identifier, CoordinateOperationMethods.MilitaryGridProjection.Name, CoordinateOperationMethods.MilitaryGridProjection, AreasOfUse.World)
        {
            if (_gridSigns == null)
                _gridSigns = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            if (_rowSigns == null)
                _rowSigns = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V' };
            if (_columnSigns == null)
                _columnSigns = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            if (_polarRowSigns == null)
                _polarRowSigns = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            if (_polarColumnSigns == null)
                _polarColumnSigns = new List<Char> { 'A', 'B', 'C', 'F', 'G', 'H', 'J', 'K', 'L', 'P', 'Q', 'R', 'S', 'T', 'U', 'X', 'Y', 'Z' };
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
            // source: http://en.wikipedia.org/wiki/MGRS

            Int32 xZoneNumber, yZoneNumber, columnNumber, rowNumber, columnMeters, rowMeters;
            Char yZoneChar, rowChar, columnChar;

            if (coordinate.Latitude > Angle.FromDegree(84)) // north pole
            {
                // apply projection
                Coordinate projectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_UPSN_EN.Projection.Forward(coordinate);

                // compute row and column (for coordinates)
                columnNumber = Convert.ToInt32(Math.Floor(projectedCoordinate.X) / _columnWidth);
                rowNumber = Convert.ToInt32(Math.Floor(projectedCoordinate.Y) / _rowHeight);

                // compute metre precision
                columnMeters = Convert.ToInt32(Math.Floor(projectedCoordinate.X) % _columnWidth);
                rowMeters = Convert.ToInt32(Math.Floor(projectedCoordinate.Y) % _rowHeight);

                // compute string
                yZoneChar = (coordinate.Longitude.BaseValue >= 0) ? 'Z' : 'Y';
                columnChar = _polarColumnSigns[(columnNumber - 20 + _polarColumnSigns.Count) % _polarColumnSigns.Count];
                rowChar = _polarRowSigns[rowNumber - 13];

                return yZoneChar.ToString() + columnChar + rowChar + columnMeters.ToString("D5") + rowMeters.ToString("D5");
            }
            else if (coordinate.Latitude < Angle.FromDegree(-80)) // south pole
            {
                // apply projection
                Coordinate projectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_UPSS_EN.Projection.Forward(coordinate);

                // compute row and column (for coordinates)
                columnNumber = Convert.ToInt32(Math.Floor(projectedCoordinate.X) / _columnWidth);
                rowNumber = Convert.ToInt32(Math.Floor(projectedCoordinate.Y) / _rowHeight);

                // compute metre precision
                columnMeters = Convert.ToInt32(Math.Floor(projectedCoordinate.X) % _columnWidth);
                rowMeters = Convert.ToInt32(Math.Floor(projectedCoordinate.Y) % _rowHeight);

                // compute string
                yZoneChar = (coordinate.Longitude.BaseValue >= 0) ? 'Z' : 'Y';
                columnChar = _polarColumnSigns[(columnNumber - 20 + _polarColumnSigns.Count) % _polarColumnSigns.Count];
                rowChar = _polarRowSigns[rowNumber - 13];

                return yZoneChar.ToString() + columnChar + rowChar + columnMeters.ToString("D5") + rowMeters.ToString("D5"); 
            }
            else
            {
                // apply projection to the specific zone
                CoordinateProjection projection = CoordinateProjectionFactory.UniversalTransverseMercatorZone(Ellipsoids.WGS1984, coordinate.Longitude, coordinate.Latitude.BaseValue >= 0 ? EllipsoidHemisphere.North : EllipsoidHemisphere.South);
                Coordinate projectedCoordinate = projection.Forward(coordinate);

                // compute the zone (from the original coordinates)
                xZoneNumber = Convert.ToInt32(Math.Floor(Math.Round((coordinate.Longitude.BaseValue + Constants.PI) / _zoneWidth, 4))) % 60 + 1;
                yZoneNumber = Convert.ToInt32(Math.Floor(Math.Round((coordinate.Latitude.BaseValue + Constants.PI / 2 - 2 * Constants.DegreeToRadian) / _zoneHeight, 4))) + 1;

                // compute row and column (for coordinates)
                columnNumber = (xZoneNumber - 1) * 8 + Convert.ToInt32(Math.Floor(Math.Round(projectedCoordinate.X, 4) / _columnWidth)) - 1;
                rowNumber = Convert.ToInt32(Math.Floor(Math.Round(projectedCoordinate.Y, 4) / _rowHeight));
                if (xZoneNumber % 2 == 0)
                    rowNumber += 5;

                // compute metre precision
                columnMeters = Convert.ToInt32(Math.Round(projectedCoordinate.X)) % _columnWidth;
                rowMeters = Convert.ToInt32(Math.Round(projectedCoordinate.Y)) % _rowHeight;

                // compute string
                yZoneChar = _gridSigns[yZoneNumber];
                columnChar = _columnSigns[columnNumber % _columnSigns.Count];
                rowChar = _rowSigns[rowNumber % _rowSigns.Count];

                return xZoneNumber.ToString() + yZoneChar + columnChar + rowChar + columnMeters.ToString("D5") + rowMeters.ToString("D5"); 
            }
        }

        /// <summary>
        /// Computes the reverse transformation.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        protected override GeoCoordinate ComputeReverse(String coordinate)
        {
            // source: http://en.wikipedia.org/wiki/MGRS

            if (coordinate == null)
                throw new ArgumentNullException("coordinate", "The coordinate is null.");

            if (!Regex.IsMatch(coordinate, "^[0-9]{1,2}[A-Z]{1,3}[0-9]{2,10}"))
                throw new ArgumentException("The coordinate is not a valid MGRS string.", "coordinate");

            Int32 xZoneNumber = 0, yZoneNumber = 0, columnNumber = 0, rowNumber = 0, columnMeters = 0, rowMeters = 0, precision = 0;

            // compute the main zone
            if (Regex.IsMatch(coordinate, "^[0-9]{1,2}[A-Z]{1}"))
            {
                if (Char.IsDigit(coordinate[1]))
                {
                    xZoneNumber = Int32.Parse(coordinate.Substring(0, 2));
                    yZoneNumber = _gridSigns.IndexOf(coordinate[2]);
                }
                else
                {
                    xZoneNumber = Int32.Parse(coordinate.Substring(0, 1));
                    yZoneNumber = _gridSigns.IndexOf(coordinate[1]);
                }
            }

            // compute the latitude and longitue for the main zone
            Double longitudeBase = (xZoneNumber - 1) * _zoneWidth - Constants.PI; 
            Double latitudeBase = (yZoneNumber - 2) * _zoneHeight - 80 * Constants.DegreeToRadian;

            // compute the subzone
            if (Regex.IsMatch(coordinate, "^[0-9]{1,2}[A-Z]{3}"))
            {
                if (xZoneNumber >= 10)
                {
                    columnNumber = _columnSigns.IndexOf(coordinate[3]); // compute the relative row and column
                    rowNumber = _rowSigns.IndexOf(coordinate[4]);
                }
                else
                {
                    columnNumber = _columnSigns.IndexOf(coordinate[2]);
                    rowNumber = _rowSigns.IndexOf(coordinate[3]);
                }
            }

            // compute metre precision
            if (Regex.IsMatch(coordinate, "^[0-9]{1,2}[A-Z]{1,3}[0-9]{2,10}"))
            {
                if (xZoneNumber >= 10)
                {
                    if ((coordinate.Length - 5) % 2 != 0)
                        throw new ArgumentException("The coordinate is not a valid MGRS string.", "coordinate");

                    precision = (coordinate.Length - 5) / 2;

                    columnMeters = Int32.Parse(coordinate.Substring(5, precision)) * Convert.ToInt32(Calculator.Pow(10, 5 - precision));
                    rowMeters = Int32.Parse(coordinate.Substring(5 + precision, precision)) * Convert.ToInt32(Calculator.Pow(10, 5 - precision));
                }
                else
                {
                    if ((coordinate.Length - 4) % 2 != 0)
                        throw new ArgumentException("The coordinate is not a valid MGRS string.", "coordinate");

                    precision = (coordinate.Length - 4) / 2;

                    columnMeters = Int32.Parse(coordinate.Substring(4, precision)) * Convert.ToInt32(Calculator.Pow(10, 5 - precision));
                    rowMeters = Int32.Parse(coordinate.Substring(4 + precision, precision)) * Convert.ToInt32(Calculator.Pow(10, 5 - precision));
                }
            }

            CoordinateProjection projection = CoordinateProjectionFactory.UniversalTransverseMercatorZone(Ellipsoids.WGS1984, xZoneNumber, latitudeBase >= 0 ? EllipsoidHemisphere.North : EllipsoidHemisphere.South);
            Coordinate coordinateBase = projection.Forward(new GeoCoordinate(latitudeBase, longitudeBase));

            // row number correction
            if (xZoneNumber % 2 == 0)
                rowNumber = (rowNumber + _rowSigns.Count - 5) % _rowSigns.Count;

            Int32 rowNumberBase = Convert.ToInt32(Math.Floor(coordinateBase.Y / _rowHeight));

            // compute the number inside the zone
            if (rowNumberBase / _rowSigns.Count * _rowSigns.Count + rowNumber < rowNumberBase) 
                rowNumber += (rowNumberBase / _rowSigns.Count + 1) * _rowSigns.Count;
            else
                rowNumber += rowNumberBase / _rowSigns.Count * _rowSigns.Count;

            if (rowNumber >= 190) // handle overflow
                rowNumber -= _rowSigns.Count;

            // compute metre precision
            Double coordinateX = (columnNumber % 8 + 1) * _columnWidth + columnMeters; 
            Double coordinateY = rowNumber * _rowHeight + rowMeters;

            // apply projection
            return projection.Reverse(new Coordinate(coordinateX, coordinateY));
        }

        #endregion
    }
}
