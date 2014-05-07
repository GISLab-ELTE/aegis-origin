/// <copyright file="Meridians.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="Meridian" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(Meridian))]
    public static class Meridians
    {
        #region Query all

        private static Meridian[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="Meridian" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="Meridian" /> instances within the collection.</value>
        public static IList<Meridian> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(Meridians).GetProperties().
                                             Where(property => property.Name != "All").
                                             Select(property => property.GetValue(null, null) as Meridian).
                                             ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="Meridian" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="Meridian" /> instances that match the specified identifier.</returns>
        public static IList<Meridian> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="Meridian" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="Meridian" /> instances that match the specified name.</returns>
        public static IList<Meridian> FromName(String name)
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

        private static Meridian _Greenwich;
        private static Meridian _Lisbon;
        private static Meridian _Paris;
        private static Meridian _Bogota;
        private static Meridian _Madrid;
        private static Meridian _Rome;
        private static Meridian _Bern;
        private static Meridian _Jakarta;
        private static Meridian _Ferro;
        private static Meridian _Brussels;
        private static Meridian _Stockholm;
        private static Meridian _Athens;
        private static Meridian _Oslo;
        private static Meridian _Budapest;
        private static Meridian _Marosvasarhely;

        #endregion

        #region Public static properties

        /// <summary>
        /// Greenwich.
        /// </summary>
        public static Meridian Greenwich
        {
            get
            {
                if (_Greenwich == null)
                    _Greenwich = Meridian.FromDegrees("EPSG::8901", "Greenwich", 0);
                return _Greenwich;
            }
        }

        /// <summary>
        /// Lisbon.
        /// </summary>
        public static Meridian Lisbon
        {
            get
            {
                if (_Lisbon == null)
                    _Lisbon = Meridian.FromDegrees("EPSG::8902", "Lisbon", -9.131906111);
                return _Lisbon;
            }
        }

        /// <summary>
        /// Paris.
        /// </summary>
        public static Meridian Paris
        {
            get
            {
                if (_Paris == null)
                    _Paris = Meridian.FromDegrees("EPSG::8903", "Paris", 2.337229167);
                return _Paris;
            }
        }

        /// <summary>
        /// Bogota.
        /// </summary>
        public static Meridian Bogota
        {
            get
            {
                if (_Bogota == null)
                    _Bogota = Meridian.FromDegrees("EPSG::8904", "Bogota", -74.08091667);
                return _Bogota;
            }
        }

        /// <summary>
        /// Madrid.
        /// </summary>
        public static Meridian Madrid
        {
            get
            {
                if (_Madrid == null)
                    _Madrid = Meridian.FromDegrees("EPSG::8905", "Madrid", -3.687938889);
                return _Madrid;
            }
        }

        /// <summary>
        /// Rome.
        /// </summary>
        public static Meridian Rome
        {
            get
            {
                if (_Rome == null)
                    _Rome = Meridian.FromDegrees("EPSG::8906", "Rome", 12.45233333);
                return _Rome;
            }
        }

        /// <summary>
        /// Bern.
        /// </summary>
        public static Meridian Bern
        {
            get
            {
                if (_Bern == null)
                    _Bern = Meridian.FromDegrees("EPSG::8907", "Bern", 7.439583333);
                return _Bern;
            }
        }

        /// <summary>
        /// Jakarta.
        /// </summary>
        public static Meridian Jakarta
        {
            get
            {
                if (_Jakarta == null)
                    _Jakarta = Meridian.FromDegrees("EPSG::8908", "Jakarta", 106.8077194);
                return _Jakarta;
            }
        }

        /// <summary>
        /// Ferro.
        /// </summary>
        public static Meridian Ferro
        {
            get
            {
                if (_Ferro == null)
                    _Ferro = Meridian.FromDegrees("EPSG::8909", "Ferro", -17.66666667);
                return _Ferro;
            }
        }

        /// <summary>
        /// Brussels.
        /// </summary>
        public static Meridian Brussels
        {
            get
            {
                if (_Brussels == null)
                    _Brussels = Meridian.FromDegrees("EPSG::8910", "Brussels", 4.367975);
                return _Brussels;
            }
        }

        /// <summary>
        /// Stockholm.
        /// </summary>
        public static Meridian Stockholm
        {
            get
            {
                if (_Stockholm == null)
                    _Stockholm = Meridian.FromDegrees("EPSG::8911", "Stockholm", 18.05827778);
                return _Stockholm;
            }
        }

        /// <summary>
        /// Athens.
        /// </summary>
        public static Meridian Athens
        {
            get
            {
                if (_Athens == null)
                    _Athens = Meridian.FromDegrees("EPSG::8912", "Athens", 23.7163375);
                return _Athens;
            }
        }

        /// <summary>
        /// Oslo.
        /// </summary>
        public static Meridian Oslo
        {
            get
            {
                if (_Oslo == null)
                    _Oslo = Meridian.FromDegrees("EPSG::8913", "Oslo", 10.72291667);
                return _Oslo;
            }
        }        

        /// <summary>
        /// Budapest (Gellérthegy).
        /// </summary>
        public static Meridian Budapest
        {
            get
            {
                if (_Budapest == null)
                    _Budapest = Meridian.FromDegrees("AEGIS::718920", "Budapest (Gellérthegy)", 19.047353);
                return _Budapest;
            }
        }

        /// <summary>
        /// Marosvásárhely (Kesztej-hegy).
        /// </summary>
        public static Meridian Marosvasarhely
        {
            get
            {
                if (_Marosvasarhely == null)
                    _Marosvasarhely = Meridian.FromDegrees("AEGIS::718921", "Marosvásárhely (Kesztej-hegy)", 24.3923055);
                return _Marosvasarhely;
            }
        }

        #endregion
    }
}
