/// <copyright file="CoordinateProjection.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Management;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Provides factory methods for creating <see cref="CoordinateProjection" /> instances.
    /// </summary>
    [IdentifiedObjectFactory(typeof(CoordinateProjection))]
    public static class CoordinateProjectionFactory
    {
        #region Query fields

        private static Dictionary<CoordinateOperationMethod, Type> _operations;

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="CoordinateProjection" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The list containing the <see cref="CoordinateProjection" /> instances that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The identifier is empty.</exception>
        public static IList<CoordinateProjection> FromIdentifier(String identifier, Ellipsoid ellipsoid)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");           
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");
            if (String.IsNullOrEmpty(identifier))
                throw new ArgumentException("The identifier is empty.", "identifier");

            // identifier correction
            identifier = Regex.Escape(identifier);

            // query methods with the appropriate attribute
            MethodInfo[] methods = typeof(CoordinateProjectionFactory).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method =>
            {
                Object attribute = method.GetCustomAttributes(typeof(IdentifiedObjectFactoryMethodAttribute), false).FirstOrDefault();
                if (attribute == null)
                    return false;

                return Regex.IsMatch((attribute as IdentifiedObjectFactoryMethodAttribute).Identifier, identifier, RegexOptions.IgnoreCase);
            }).ToArray();

            List<CoordinateProjection> operations = new List<CoordinateProjection>();

            // invoke methods and gather return values
            foreach (MethodInfo method in methods)
            {
                operations.Add(typeof(CoordinateProjectionFactory).InvokeMember(method.Name, BindingFlags.Public | BindingFlags.Static, null, null, new Object[] { ellipsoid }) as CoordinateProjection);
            }

            return operations;
        }

        /// <summary>
        /// Returns all <see cref="CoordinateProjection" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The list containing the <see cref="CoordinateProjection" /> instances that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The name is empty.</exception>
        public static IList<CoordinateProjection> FromName(String name, Ellipsoid ellipsoid)
        {
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("The name is empty.", "name");

            // name correction
            name = Regex.Escape(name);

            // query methods with the appropriate attribute
            MethodInfo[] methods = typeof(CoordinateProjectionFactory).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(method =>
            {
                Object attribute = method.GetCustomAttributes(typeof(IdentifiedObjectFactoryMethodAttribute), false).FirstOrDefault();
                if (attribute == null)
                    return false;

                return Regex.IsMatch((attribute as IdentifiedObjectFactoryMethodAttribute).Name, name, RegexOptions.IgnoreCase);
            }).ToArray();

            List<CoordinateProjection> operations = new List<CoordinateProjection>();

            // invoke methods and gather return values
            foreach (MethodInfo method in methods)
            {
                operations.Add(typeof(CoordinateProjectionFactory).InvokeMember(method.Name, BindingFlags.Public | BindingFlags.Static, null, null, new Object[] { ellipsoid }) as CoordinateProjection);
            }

            return operations;
        }

        /// <summary>
        /// Returns all <see cref="CoordinateProjection" /> instances matching a specified method identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <returns>The list containing the <see cref="CoordinateProjection" /> instances that match the specified identifier.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The identifier is empty.</exception>
        public static IList<CoordinateProjection> FromMethodIdentifier(String identifier, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");
            if (String.IsNullOrEmpty(identifier))
                throw new ArgumentException("The identifier is empty.", "identifier");

            if (_operations == null)
                LoadOperations();

            List<CoordinateProjection> operations = new List<CoordinateProjection>();

            // identifier correction
            identifier = Regex.Escape(identifier);

            // query types with the specified identifier
            foreach (CoordinateOperationMethod method in _operations.Keys.Where(m => Regex.IsMatch(m.Identifier, identifier, RegexOptions.IgnoreCase)))
            {
                if (!_operations.ContainsKey(method))
                    continue;
                Boolean hasParameters = true;
                foreach (CoordinateOperationParameter parameter in method.Parameters)
                    if (!parameters.ContainsKey(parameter))
                        hasParameters = false;

                if (!hasParameters)
                    continue;

                try
                {
                    // create instances
                    operations.Add(Activator.CreateInstance(_operations[method], method.Identifier, method.Name, parameters, ellipsoid, areaOfUse) as CoordinateProjection);
                }   
                catch (ArgumentException) { }
            }

            return operations;
        }

        /// <summary>
        /// Returns all <see cref="CoordinateProjection" /> instances matching a specified method name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <returns>The list containing the <see cref="CoordinateProjection" /> instances that match the specified name.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The name is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The name is empty.</exception>
        public static IList<CoordinateProjection> FromMethodName(String name, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
        {
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("The name is empty.", "name");

            if (_operations == null)
                LoadOperations();

            // name correction
            name = Regex.Escape(name);

            List<CoordinateProjection> operations = new List<CoordinateProjection>();

            // query types with the specified name
            foreach (CoordinateOperationMethod method in _operations.Keys.Where(m => Regex.IsMatch(m.Name, name, RegexOptions.IgnoreCase) || m.Aliases != null && m.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))))
            {
                if (!_operations.ContainsKey(method))
                    continue;

                Boolean hasParameters = true;
                foreach (CoordinateOperationParameter parameter in method.Parameters)
                    if (!parameters.ContainsKey(parameter))
                        hasParameters = false;

                if (!hasParameters)
                    continue;

                try
                {
                    // create instances
                    operations.Add(Activator.CreateInstance(_operations[method], method.Identifier, method.Name, parameters, ellipsoid, areaOfUse) as CoordinateProjection);
                }   
                catch { }
            }

            return operations;
        }

        /// <summary>
        /// Returns the <see cref="CoordinateProjection" /> instance for the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <returns>The <see cref="CoordinateProjection" /> instance implementing the method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The method is null.
        /// or
        /// The ellipsoid is null.
        /// </exception>
        public static CoordinateProjection FromMethod(CoordinateOperationMethod method, IDictionary<CoordinateOperationParameter, Object> parameters, Ellipsoid ellipsoid, AreaOfUse areaOfUse)
        {
            if (method == null)
                throw new ArgumentNullException("method", "The method is null.");
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid", "The ellipsoid is null.");

            if (_operations == null)
                LoadOperations();

            if (!_operations.ContainsKey(method))
                return null;

            // create instance
            return Activator.CreateInstance(_operations[method], method.Identifier, method.Name, parameters, ellipsoid, areaOfUse) as CoordinateProjection;
        }

        /// <summary>
        /// Loads the operations.
        /// </summary>
        private static void LoadOperations()
        {            
            _operations = new Dictionary<CoordinateOperationMethod,Type>();

            // collect all coordinate projection types within the assembly that matches the specified name
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(CoordinateProjection))))
            {
                // query the attribute of the type
                Object attribute = type.GetCustomAttributes(typeof(CoordinateOperationMethodImplementationAttribute), false).FirstOrDefault();
                if (attribute == null)
                    continue;

                // query the method of the projection
                CoordinateOperationMethod method = CoordinateOperationMethods.FromIdentifier((attribute as CoordinateOperationMethodImplementationAttribute).Identifier).FirstOrDefault();
                if (method == null)
                    continue;

                _operations.Add(method, type);
            }
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// British National Grid.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::19916", "British National Grid", typeof(TransverseMercatorProjection))]
        public static CoordinateProjection BritishNationalGrid(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(49));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-2));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(400000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(-100000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996012717);

            return new TransverseMercatorProjection("EPSG::19916", "British National Grid", parameters, ellipsoid, AreasOfUse.GreatBritainMan);
        }

        /// <summary>
        /// Egységes országos vetületi rendszer.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::19931", "Egységes országos vetületi rendszer", typeof(HotineObliqueMercatorBProjection))]
        public static CoordinateProjection EOV(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfProjectionCentre, Angle.FromDegree(47.144393722));
            parameters.Add(CoordinateOperationParameters.LongitudeOfProjectionCentre, Angle.FromDegree(19.048571778));
            parameters.Add(CoordinateOperationParameters.AzimuthOfInitialLine, Angle.FromDegree(90));
            parameters.Add(CoordinateOperationParameters.AngleFromRectifiedToSkewGrid, Angle.FromDegree(90));
            parameters.Add(CoordinateOperationParameters.ScaleFactorOnInitialLine, 0.99993);
            parameters.Add(CoordinateOperationParameters.EastingAtProjectionCentre, Length.Convert(Length.FromMetre(650000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.NorthingAtProjectionCentre, Length.Convert(Length.FromMetre(200000), ellipsoid.SemiMajorAxis.Unit));

            return new HotineObliqueMercatorBProjection("EPSG::19931", "Egységes országos vetületi rendszer", parameters, ellipsoid, AreasOfUse.Hungary);
        }

        /// <summary>
        /// RD New.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::19914", "RD New", typeof(ObliqueStereographicProjection))]
        public static CoordinateProjection RDNew(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(52, 9, 22.178));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(5, 23, 15.5));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(155000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(463000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9999079);

            return new ObliqueStereographicProjection("EPSG::19914", "RD New", parameters, ellipsoid, AreasOfUse.NetherlandsOnshore);
        }

        /// <summary>
        /// SPCS83 Alabama West zone (meters).
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::10132", "SPCS83 Alabama West zone (meters)", typeof(TransverseMercatorProjection))]
        public static CoordinateProjection SPCS83AlabamaWestZone(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(30));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-87.5));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(600000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(0), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.999933333);

            return new TransverseMercatorProjection("EPSG::10132", "SPCS83 Alabama West zone (meters)", parameters, ellipsoid, AreasOfUse.USAAlabamaSPCSW);
        }

        /// <summary>
        /// Texas CS27 South Central zone.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::14204", "Texas CS27 South Central zone", typeof(LambertConicConformal2SPProjection))]
        public static CoordinateProjection TexasCS27SouthCentral(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfFalseOrigin, Angle.FromDegree(27, 50, 0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfFalseOrigin, Angle.FromDegree(-99, 0, 0));
            parameters.Add(CoordinateOperationParameters.LatitudeOf1stStandardParallel, Angle.FromDegree(28, 23, 0));
            parameters.Add(CoordinateOperationParameters.LatitudeOf2ndStandardParallel, Angle.FromDegree(30, 17, 0));
            parameters.Add(CoordinateOperationParameters.EastingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(2000000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.NorthingAtFalseOrigin, Length.Convert(Length.FromUSSurveyFoot(0), ellipsoid.SemiMajorAxis.Unit));

            return new LambertConicConformal2SPProjection("EPSG::14204", "Texas CS27 South Central zone", parameters, ellipsoid, AreasOfUse.USATexasSPCS27SC);
        }

        /// <summary>
        /// Trinidad Grid.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::19925", "Trinidad Grid", typeof(CassiniSoldnerProjection))]
        public static CoordinateProjection TrinidadGrid(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(10, 25, 30));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(-61, 20, 0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromClarkesLink(430000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromClarkesLink(325000), ellipsoid.SemiMajorAxis.Unit));

            return new CassiniSoldnerProjection("EPSG::19925", "Trinidad Grid", parameters, ellipsoid, AreasOfUse.TrinidadAndTobago);
        }

        /// <summary>
        /// Universal Polar Stereographic North.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::16061", "Universal Polar Stereographic North", typeof(PolarStereographicAProjection))]
        public static CoordinateProjection UniversalPolarStereographicNorth(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(90));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.994);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(2000000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(2000000), ellipsoid.SemiMajorAxis.Unit));

            return new PolarStereographicAProjection("EPSG::16061", "Universal Polar Stereographic North", parameters, ellipsoid, AreasOfUse.World_60NTo90N);
        }

        /// <summary>
        /// Universal Polar Stereographic South.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::16161", "Universal Polar Stereographic South", typeof(PolarStereographicAProjection))]
        public static CoordinateProjection UniversalPolarStereographicSouth(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(-90));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.994);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(2000000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(2000000), ellipsoid.SemiMajorAxis.Unit));

            return new PolarStereographicAProjection("EPSG::16161", "Universal Polar Stereographic South", parameters, ellipsoid, AreasOfUse.World_60STo90S);
        }

        /// <summary>
        /// UTM grid system (northern hemisphere).
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::16000", "UTM grid system (northern hemisphere)", typeof(TransverseMercatorZonedProjection))]
        public static CoordinateProjection UniversalTransverseMercatorGridSystemNorthHemisphere(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.InitialLongitude, Angle.FromDegree(180));
            parameters.Add(CoordinateOperationParameters.ZoneWidth, 6);
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(500000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(0), ellipsoid.SemiMajorAxis.Unit));

            return new TransverseMercatorZonedProjection("EPSG::16000", "UTM grid system (northern hemisphere)", parameters, ellipsoid, AreasOfUse.WorldNorthHemisphere_0NTo84N);
        }

        /// <summary>
        /// UTM grid system (south hemisphere).
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::16000", "UTM grid system (south hemisphere)", typeof(TransverseMercatorZonedProjection))]
        public static CoordinateProjection UniversalTransverseMercatorGridSystemSouthHemisphere(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.InitialLongitude, Angle.FromDegree(180));
            parameters.Add(CoordinateOperationParameters.ZoneWidth, 6);
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(500000), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(10000000), ellipsoid.SemiMajorAxis.Unit));

            return new TransverseMercatorZonedProjection("EPSG::16100", "UTM grid system (south hemisphere)", parameters, ellipsoid, AreasOfUse.WorldNorthHemisphere_0NTo84N);
        }

        /// <summary>
        /// UTM zone 10N.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::16010", "UTM zone 10N", typeof(TransverseMercatorProjection))]
        public static CoordinateProjection UniversalTransverseMercatorZone10North(Ellipsoid ellipsoid)
        { 
            return UniversalTransverseMercatorZone(ellipsoid, 10, EllipsoidHemisphere.North); 
        }

        /// <summary>
        /// UTM zone 10S.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::16110", "UTM zone 10S", typeof(TransverseMercatorProjection))]
        public static CoordinateProjection UniversalTransverseMercatorZone10South(Ellipsoid ellipsoid)
        { 
            return UniversalTransverseMercatorZone(ellipsoid, 10, EllipsoidHemisphere.South); 
        }

        /// <summary>
        /// World Mercator.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        [IdentifiedObjectFactoryMethod("EPSG::19883", "World Mercator", typeof(MercatorAProjection))]
        public static CoordinateProjection WorldMercator(Ellipsoid ellipsoid)
        {
            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromDegree(0));
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.Convert(Length.FromMetre(0), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.Convert(Length.FromMetre(0), ellipsoid.SemiMajorAxis.Unit));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 1.0);

            return new MercatorAProjection("EPSG::19883", "World Mercator", parameters, ellipsoid, AreasOfUse.World_80STo84N);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// UTM zone.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="zoneNumber">The zone number.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        /// <exception cref="System.ArgumentException">
        /// The zone number is not valid.;longitude
        /// or
        /// The hemisphere must be north or south.;hemisphere
        /// </exception>
        public static CoordinateProjection UniversalTransverseMercatorZone(Ellipsoid ellipsoid, Int32 zoneNumber, EllipsoidHemisphere hemisphere)
        {
            if (zoneNumber < 1 || zoneNumber > 60)
                throw new ArgumentException("The zone number is not valid.", "longitude");
            if (hemisphere == EllipsoidHemisphere.Equador)
                throw new ArgumentException("The hemisphere must be north or south.", "hemisphere");

            Double zoneWidth = Constants.PI / 30;
            Double zoneCenter = (zoneNumber - 1) * zoneWidth + zoneWidth / 2 - Constants.PI;

            String name = "UTM zone " + zoneNumber + (hemisphere == EllipsoidHemisphere.North ? "N" : "S");
            String identifier = "EPSG::" + (16000 + zoneNumber + (hemisphere == EllipsoidHemisphere.South ? 100 : 0));

            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromRadian(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromRadian(zoneCenter));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(500000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(10000000));

            return new TransverseMercatorProjection(identifier, name, parameters, ellipsoid, AreasOfUse.WorldZone(zoneNumber, hemisphere));
        }

        /// <summary>
        /// UTM zone.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns>The coordinate projection produced by the method.</returns>
        /// <exception cref="System.ArgumentException">
        /// The longitude is invalid.;longitude
        /// or
        /// The hemisphere must be north or south.;hemisphere
        /// </exception>
        public static CoordinateProjection UniversalTransverseMercatorZone(Ellipsoid ellipsoid, Angle longitude, EllipsoidHemisphere hemisphere)
        {
            if (longitude.BaseValue < -Constants.PI || longitude.BaseValue > Constants.PI)
                throw new ArgumentException("The longitude is invalid.", "longitude");
            if (hemisphere == EllipsoidHemisphere.Equador)
                throw new ArgumentException("The hemisphere must be north or south.", "hemisphere");

            Double zoneWidth = Constants.PI / 30;

            Double zoneCenter = Math.Floor(Math.Round(longitude.BaseValue / zoneWidth, 4)) * zoneWidth + zoneWidth / 2;
            Int32 zoneNumber = Convert.ToInt32(Math.Floor(Math.Round((longitude.BaseValue + Constants.PI) / zoneWidth, 4))) + 1;

            String name = "UTM zone " + zoneNumber + (hemisphere == EllipsoidHemisphere.North ? "N" : "S");
            String identifier = "EPSG::" + (16000 + zoneNumber + (hemisphere == EllipsoidHemisphere.South ? 100 : 0)); 

            Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
            parameters.Add(CoordinateOperationParameters.LatitudeOfNaturalOrigin, Angle.FromRadian(0));
            parameters.Add(CoordinateOperationParameters.LongitudeOfNaturalOrigin, Angle.FromRadian(zoneCenter));
            parameters.Add(CoordinateOperationParameters.ScaleFactorAtNaturalOrigin, 0.9996);
            parameters.Add(CoordinateOperationParameters.FalseEasting, Length.FromMetre(500000));
            parameters.Add(CoordinateOperationParameters.FalseNorthing, Length.FromMetre(10000000));

            return new TransverseMercatorProjection(identifier, name, parameters, ellipsoid, AreasOfUse.WorldZone(longitude, hemisphere));
        }

        #endregion
    }
}
