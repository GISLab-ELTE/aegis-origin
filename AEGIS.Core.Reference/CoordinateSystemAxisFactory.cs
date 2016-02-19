/// <copyright file="CoordinateSystemAxisFactory.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Reflection;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Provides factory methods for creating <see cref="CoordinateSystemAxis" /> instances.
    /// </summary>    
    [IdentifiedObjectFactory(typeof(CoordinateSystemAxis))]
    public class CoordinateSystemAxisFactory
    {
        #region Query methods

        /// <summary>
        /// Returns all <see cref="CoordinateSystemAxis" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The list containing the <see cref="CoordinateSystemAxis" /> instances that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The unit os measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The identifier is empty.</exception>
        public static IList<CoordinateSystemAxis> FromIdentifier(String identifier, UnitOfMeasurement unit)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");
            if (unit == null)
                throw new ArgumentNullException("unit", "The unit os measurement is null.");
            if (String.IsNullOrEmpty(identifier))
                throw new ArgumentException("The identifier is empty.", "identifier");

            // identifier correction
            identifier = Regex.Escape(identifier);

            // query methods with the appropriate attribute
            MethodInfo[] methods = typeof(CoordinateSystemAxisFactory).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method =>
            {
                Object attribute = method.GetCustomAttributes(typeof(IdentifiedObjectFactoryMethodAttribute), false).FirstOrDefault();
                if (attribute == null)
                    return false;

                return Regex.IsMatch((attribute as IdentifiedObjectFactoryMethodAttribute).Identifier, identifier, RegexOptions.IgnoreCase);
            }).ToArray();

            List<CoordinateSystemAxis> operations = new List<CoordinateSystemAxis>();

            // invoke methods and gather return values
            foreach (MethodInfo method in methods)
            {
                operations.Add(typeof(CoordinateSystemAxisFactory).InvokeMember(method.Name, BindingFlags.Public | BindingFlags.Static, null, null, new Object[] { unit }) as CoordinateSystemAxis);
            }

            return operations;
        }

        /// <summary>
        /// Returns all <see cref="CoordinateSystemAxis" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The list containing the <see cref="CoordinateSystemAxis" /> instances that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The unit os measurement is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The name is empty.</exception>
        public static IList<CoordinateSystemAxis> FromName(String name, UnitOfMeasurement unit)
        {
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");
            if (unit == null)
                throw new ArgumentNullException("unit", "The unit os measurement is null.");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("The name is empty.", "name");

            // name correction
            name = Regex.Escape(name);

            // query methods with the appropriate attribute
            MethodInfo[] methods = typeof(CoordinateSystemAxisFactory).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method =>
            {
                Object attribute = method.GetCustomAttributes(typeof(IdentifiedObjectFactoryMethodAttribute), false).FirstOrDefault();
                if (attribute == null)
                    return false;

                return Regex.IsMatch((attribute as IdentifiedObjectFactoryMethodAttribute).Name, name, RegexOptions.IgnoreCase);
            }).ToArray();

            List<CoordinateSystemAxis> operations = new List<CoordinateSystemAxis>();

            // invoke methods and gather return values
            foreach (MethodInfo method in methods)
            {
                operations.Add(typeof(CoordinateSystemAxisFactory).InvokeMember(method.Name, BindingFlags.Public | BindingFlags.Static, null, null, new Object[] { unit }) as CoordinateSystemAxis);
            }

            return operations;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a new coordinate system axis for geodetic latitude.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GeodeticLatitude(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9901", "Geodetic latitude", AxisDirection.North, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for geodetic longitude.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GeodeticLongitude(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9902", "Geodetic longitude", AxisDirection.East, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for ellipsoidal height.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis EllipsoidalHeight(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9903", "Ellipsoidal height", AxisDirection.Up, unit); }
        /// <summary>
        /// Creates a new coordinate system axis for gravity related height.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GravityRelatedHeight(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9904", "Gravity-related height", AxisDirection.Up, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for gravity related depth.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GravityRelatedDepth(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9905", "Gravity-related depth", AxisDirection.Down, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for easting.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis Easting(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9906", "Easting", AxisDirection.East, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for northing.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis Northing(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9907", "Northing", AxisDirection.North, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for westing.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis Westing(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9908", "Westing", AxisDirection.West, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for southing.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis Southing(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9909", "Southing", AxisDirection.South, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for geocentic X direction.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GeocentricX(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9910", "Geocentric X", AxisDirection.GeocentricX, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for geocentic Y direction.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GeocentricY(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9911", "Geocentric Y", AxisDirection.GeocentricY, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for geocentic Z direction.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GeocentricZ(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9912", "Geocentric Z", AxisDirection.GeocentricZ, unit); }
        /// <summary>
        /// Creates a new coordinate system axis for local height.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis LocalHeight(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9916", "Local height", AxisDirection.Up, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for local depth.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis LocalDepth(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9917", "Local depth", AxisDirection.Down, unit); }

        /// <summary>
        /// Creates a new coordinate system axis for geocentric radius.
        /// </summary>
        /// <param name="unit">The unit of measurement.</param>
        /// <returns>The coordinate system axis with the specified unit of measurement.</returns>
        public static CoordinateSystemAxis GeocentricRadius(UnitOfMeasurement unit) { return new CoordinateSystemAxis("EPSG::9928", "Geocentric radius", AxisDirection.Up, unit); }

        #endregion
    }
}
