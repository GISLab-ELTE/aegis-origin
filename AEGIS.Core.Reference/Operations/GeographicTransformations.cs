/// <copyright file="GeographicTransformations.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a collection of known <see cref="GeographicTransformation" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(GeographicTransformation))]
    public class GeographicTransformations
    {
        #region Query fields

        private static GeographicTransformation[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeographicTransformation" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeographicTransformation" /> instances within the collection.</value>
        public static IList<GeographicTransformation> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(GeographicTransformations).GetProperties().
                                                             Where(property => property.Name != "All").
                                                             Select(property => property.GetValue(null, null) as GeographicTransformation).
                                                             ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeographicTransformation" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A read-only list containing the <see cref="GeographicTransformation" /> instances that match the specified identifier.</returns>
        public static IList<GeographicTransformation> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeographicTransformation" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A read-only list containing the <see cref="GeographicTransformation" /> instances that match the specified name.</returns>
        public static IList<GeographicTransformation> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeographicTransformation" /> instances matching a specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>A read-only list containing the <see cref="GeographicTransformation" /> instances that match the specified method.</returns>
        public static IList<GeographicTransformation> FromMethod(CoordinateOperationMethod method)
        {
            if (method == null)
                return null;

            return All.Where(obj => obj.Method.Equals(method)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeographicTransformation" /> instances transforming between the specified reference systems.
        /// </summary>
        /// <param name="source">The source reference system.</param>
        /// <param name="target">The target reference system.</param>
        /// <returns>A read-only list containing the <see cref="GeographicTransformation" /> instances transforming between the specified reference systems.</returns>
        public static IList<GeographicTransformation> FromReferenceSystems(GeographicCoordinateReferenceSystem source, GeographicCoordinateReferenceSystem target)
        {
            if (source == null || target == null)
                return null;

            return All.Where(referenceSystem => referenceSystem.Source.Equals(source) && referenceSystem.Target.Equals(target)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static GeographicTransformation _HD72_WGS84_V1;
        private static GeographicTransformation _HD72_WGS84_V2;
        private static GeographicTransformation _HD72_WGS84_V3;
        private static GeographicTransformation _HD72_WGS84_V4;

        #endregion

        #region Public static properties

        /// <summary>
        /// HD72 to WGS 84 (1).
        /// </summary>
        public static GeographicTransformation HD72_WGS84_V1
        {
            get
            {
                if (_HD72_WGS84_V1 == null)
                {
                    Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
                    parameters.Add(CoordinateOperationParameters.XAxisTranslation, Length.FromMetre(56));
                    parameters.Add(CoordinateOperationParameters.YAxisTranslation, Length.FromMetre(-75.77));
                    parameters.Add(CoordinateOperationParameters.ZAxisTranslation, Length.FromMetre(-15.31));
                    parameters.Add(CoordinateOperationParameters.XAxisRotation, Angle.FromArcSecond(0.37));
                    parameters.Add(CoordinateOperationParameters.YAxisRotation, Angle.FromArcSecond(0.2));
                    parameters.Add(CoordinateOperationParameters.ZAxisRotation, Angle.FromArcSecond(0.21));
                    parameters.Add(CoordinateOperationParameters.ScaleDifference, 1.01);

                    _HD72_WGS84_V1 = new CompoundGeographic2DTransformation("EPSG::1830", "HD72 to WGS 84 (1)",
                                                                            CoordinateOperationMethods.CoordinateFrameRotationGeographic2DDomain,
                                                                            parameters,
                                                                            Geographic2DCoordinateReferenceSystems.HD72,
                                                                            Geographic2DCoordinateReferenceSystems.WGS84,
                                                                            AreasOfUse.Hungary);
                }

                return _HD72_WGS84_V1;
            }
        }

        /// <summary>
        /// HD72 to WGS 84 (2).
        /// </summary>
        public static GeographicTransformation HD72_WGS84_V2
        {
            get
            {
                if (_HD72_WGS84_V2 == null)
                {
                    Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
                    parameters.Add(CoordinateOperationParameters.XAxisTranslation, Length.FromMetre(57.01));
                    parameters.Add(CoordinateOperationParameters.YAxisTranslation, Length.FromMetre(-69.97));
                    parameters.Add(CoordinateOperationParameters.ZAxisTranslation, Length.FromMetre(-9.29));

                    _HD72_WGS84_V2 = new CompoundGeographic2DTransformation("EPSG::1831", "HD72 to WGS 84 (2)",
                                                                            CoordinateOperationMethods.GeocentricTranslationGeographic2DDomain,
                                                                            parameters,
                                                                            Geographic2DCoordinateReferenceSystems.HD72,
                                                                            Geographic2DCoordinateReferenceSystems.WGS84,
                                                                            AreasOfUse.Hungary);
                }

                return _HD72_WGS84_V2;
            }
        }

        /// <summary>
        /// HD72 to WGS 84 (2).
        /// </summary>
        public static GeographicTransformation HD72_WGS84_V3
        {
            get
            {
                if (_HD72_WGS84_V3 == null)
                {
                    Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
                    parameters.Add(CoordinateOperationParameters.XAxisTranslation, Length.FromMetre(52.684));
                    parameters.Add(CoordinateOperationParameters.YAxisTranslation, Length.FromMetre(-71.194));
                    parameters.Add(CoordinateOperationParameters.ZAxisTranslation, Length.FromMetre(-13.975));
                    parameters.Add(CoordinateOperationParameters.XAxisRotation, Angle.FromArcSecond(0.312));
                    parameters.Add(CoordinateOperationParameters.YAxisRotation, Angle.FromArcSecond(0.1063));
                    parameters.Add(CoordinateOperationParameters.ZAxisRotation, Angle.FromArcSecond(0.3729));
                    parameters.Add(CoordinateOperationParameters.ScaleDifference, 1.0191);

                    _HD72_WGS84_V3 = new CompoundGeographic2DTransformation("EPSG::1448", "HD72 to WGS 84 (3)",
                                                                            CoordinateOperationMethods.CoordinateFrameRotationGeographic2DDomain,
                                                                            parameters,
                                                                            Geographic2DCoordinateReferenceSystems.HD72,
                                                                            Geographic2DCoordinateReferenceSystems.WGS84,
                                                                            AreasOfUse.Hungary);
                }

                return _HD72_WGS84_V3;
            }
        }

        /// <summary>
        /// HD72 to WGS 84 (4).
        /// </summary>
        public static GeographicTransformation HD72_WGS84_V4
        {
            get
            {
                if (_HD72_WGS84_V4 == null)
                {
                    Dictionary<CoordinateOperationParameter, Object> parameters = new Dictionary<CoordinateOperationParameter, Object>();
                    parameters.Add(CoordinateOperationParameters.XAxisTranslation, Length.FromMetre(52.17));
                    parameters.Add(CoordinateOperationParameters.YAxisTranslation, Length.FromMetre(-71.82));
                    parameters.Add(CoordinateOperationParameters.ZAxisTranslation, Length.FromMetre(-14.9));

                    _HD72_WGS84_V4 = new CompoundGeographic2DTransformation("EPSG::1242", "HD72 to WGS 84 (4)",
                                                                            CoordinateOperationMethods.GeocentricTranslationGeographic2DDomain,
                                                                            parameters,
                                                                            Geographic2DCoordinateReferenceSystems.HD72,
                                                                            Geographic2DCoordinateReferenceSystems.WGS84,
                                                                            AreasOfUse.Hungary);
                }

                return _HD72_WGS84_V4;
            }
        }

        #endregion
    }
}
