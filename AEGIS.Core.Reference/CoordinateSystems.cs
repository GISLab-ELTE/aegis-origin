/// <copyright file="CoordinateSystems.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="CoordinateSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(CoordinateSystem))]
    public static class CoordinateSystems
    {
        #region Query fields

        private static CoordinateSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="CoordinateSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="CoordinateSystem" /> instances within the collection.</value>
        public static IList<CoordinateSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(CoordinateSystems).GetProperties().
                                                     Where(property => property.Name != "All").
                                                     Select(property => property.GetValue(null, null) as CoordinateSystem).
                                                     ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="CoordinateSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="CoordinateSystem" /> instances that match the specified identifier.</returns>
        public static IList<CoordinateSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="CoordinateSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="CoordinateSystem" /> instances that match the specified name.</returns>
        public static IList<CoordinateSystem> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static CoordinateSystem _cartesianENM;
        private static CoordinateSystem _cartesianGeocentric;
        private static CoordinateSystem _ellipsoidalLatLonD;
        private static CoordinateSystem _ellipsoidalLatLonHiDM;
        private static CoordinateSystem _ellipsoidalLonLatD;
        private static CoordinateSystem _sphericalLatLonHiDM;
        private static CoordinateSystem _verticalDepthM;
        private static CoordinateSystem _verticalHeightM;

        #endregion

        #region Public static methods

        /// <summary>
        /// Cartesian 2D CS. Axes: easting, northing (E,N). Orientations: east, north. UoM: m. 
        /// </summary>
        public static CoordinateSystem CartesianENM
        {
            get
            {
                return _cartesianENM ?? (_cartesianENM = 
                    new CoordinateSystem("EPSG::4400", CoordinateSystemType.Cartesian,
                                         CoordinateSystemAxisFactory.Easting(UnitsOfMeasurement.Metre),
                                         CoordinateSystemAxisFactory.Northing(UnitsOfMeasurement.Metre)));
            }
        }

        /// <summary>
        /// Cartesian CS (Earth centred, earth fixed, right handed 3D coordinate system, consisting of 3 orthogonal axes with X and Y axes in the equatorial plane, positive Z-axis parallel to mean earth rotation axis and pointing towards North Pole. UoM: m.). 
        /// </summary>
        public static CoordinateSystem CartesianGeocentric
        {
            get
            {
                return _cartesianGeocentric ?? (_cartesianGeocentric =
                    new CoordinateSystem("EPSG::6500", "Cartesian CS (Earth centred, earth fixed, right handed 3D coordinate system, consisting of 3 orthogonal axes with X and Y axes in the equatorial plane, positive Z-axis parallel to mean earth rotation axis and pointing towards North Pole. UoM: m.)", CoordinateSystemType.Cartesian,
                                         CoordinateSystemAxisFactory.GeocentricX(UnitsOfMeasurement.Metre),
                                         CoordinateSystemAxisFactory.GeocentricY(UnitsOfMeasurement.Metre),
                                         CoordinateSystemAxisFactory.GeocentricZ(UnitsOfMeasurement.Metre)));
            }
        }

        /// <summary>
        /// Spherical 3D CS. Axes: latitude, longitude, radius. Orientations: north, east, up. UoM: degrees, degrees, metres. 
        /// </summary>
        public static CoordinateSystem SphericalLatLonHiDM
        {
            get
            {
                return _sphericalLatLonHiDM ?? (_sphericalLatLonHiDM = 
                    new CoordinateSystem("EPSG:::6404", CoordinateSystemType.Spherical,
                                         CoordinateSystemAxisFactory.GeodeticLatitude(UnitsOfMeasurement.Degree),
                                         CoordinateSystemAxisFactory.GeodeticLongitude(UnitsOfMeasurement.Degree),
                                         CoordinateSystemAxisFactory.GeocentricRadius(UnitsOfMeasurement.Metre)));
            }
        }

        /// <summary>
        /// Ellipsoidal 2D CS. Axes: latitude, longitude. Orientations: north, east. UoM: degree.
        /// </summary>
        public static CoordinateSystem EllipsoidalLatLonD
        {
            get
            {
                return _ellipsoidalLatLonD ?? (_ellipsoidalLatLonD = 
                    new CoordinateSystem("EPSG::6422", CoordinateSystemType.Ellipsoidal,
                                         CoordinateSystemAxisFactory.GeodeticLatitude(UnitsOfMeasurement.Degree),
                                         CoordinateSystemAxisFactory.GeodeticLongitude(UnitsOfMeasurement.Degree)));
            }
        }

        /// <summary>
        /// Ellipsoidal 3D CS. Axes: latitude, longitude, ellipsoidal height. Orientations: north, east, up. UoM: degree, degree, metre. 
        /// </summary>
        public static CoordinateSystem EllipsoidalLatLonHiDM
        {
            get
            {
                return _ellipsoidalLatLonHiDM ?? (_ellipsoidalLatLonHiDM = 
                    new CoordinateSystem("EPSG::6423", CoordinateSystemType.Ellipsoidal,
                                         CoordinateSystemAxisFactory.GeodeticLatitude(UnitsOfMeasurement.Degree),
                                         CoordinateSystemAxisFactory.GeodeticLongitude(UnitsOfMeasurement.Degree), 
                                         CoordinateSystemAxisFactory.EllipsoidalHeight(UnitsOfMeasurement.Metre)));
            }
        }

        /// <summary>
        /// Ellipsoidal 2D CS. Axes: longitude, latitude. Orientations: east, north. UoM: degree.
        /// </summary>
        public static CoordinateSystem EllipsoidalLonLatD
        {
            get
            {
                return _ellipsoidalLonLatD ?? (_ellipsoidalLonLatD = 
                    new CoordinateSystem("EPSG::6424", CoordinateSystemType.Ellipsoidal,
                                         CoordinateSystemAxisFactory.GeodeticLongitude(UnitsOfMeasurement.Degree),
                                         CoordinateSystemAxisFactory.GeodeticLatitude(UnitsOfMeasurement.Degree)));
            }
        }

        /// <summary>
        /// Vertical CS. Axis: depth (D). Orientation: down. UoM: m. 
        /// </summary>
        public static CoordinateSystem VerticalDepthM
        {
            get
            {
                return _verticalDepthM ?? (_verticalDepthM = 
                    new CoordinateSystem("EPSG::6498", CoordinateSystemType.Vertical, 
                                         CoordinateSystemAxisFactory.GravityRelatedDepth(UnitsOfMeasurement.Metre)));
            }
        }

        /// <summary>
        /// Vertical CS. Axis: height (H). Orientation: up. UoM: m. 
        /// </summary>
        public static CoordinateSystem VerticalHeightM
        {
            get
            {
                return _verticalHeightM ?? (_verticalHeightM = 
                    new CoordinateSystem("EPSG::6499", CoordinateSystemType.Vertical, 
                                         CoordinateSystemAxisFactory.GravityRelatedHeight(UnitsOfMeasurement.Metre)));
            }
        }


        #endregion
    }
}
