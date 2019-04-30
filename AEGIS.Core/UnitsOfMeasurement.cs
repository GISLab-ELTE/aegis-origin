///<copyright file="UnitsOfMeasurement.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a collection of known <see cref="UnitOfMeasurement" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(UnitOfMeasurement))]
    public static class UnitsOfMeasurement
    {
        #region Query fields

        /// <summary>
        /// The array of all unit of measurement instances within the collection.
        /// </summary>
        private static UnitOfMeasurement[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="UnitOfMeasurement" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="UnitOfMeasurement" /> instances within the collection.</value>
        public static IList<UnitOfMeasurement> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(UnitsOfMeasurement).GetProperties().
                                                      Where(property => property.Name != "All").
                                                      Select(property => property.GetValue(null, null) as UnitOfMeasurement).
                                                      ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="UnitOfMeasurement" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="UnitOfMeasurement" /> instances that match the specified identifier.</returns>
        public static IList<UnitOfMeasurement> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="UnitOfMeasurement" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="UnitOfMeasurement" /> instances that match the specified name.</returns>
        public static IList<UnitOfMeasurement> FromName(String name)
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

        private static UnitOfMeasurement _arcMinute;
        private static UnitOfMeasurement _arcSecond;
        private static UnitOfMeasurement _bin;
        private static UnitOfMeasurement _britishChainBenoit1895A;
        private static UnitOfMeasurement _britishChainBenoit1895B;
        private static UnitOfMeasurement _britishChainSears1922;
        private static UnitOfMeasurement _britishChainSears1922T;
        private static UnitOfMeasurement _britishFoot1865;
        private static UnitOfMeasurement _britishFoot1936;
        private static UnitOfMeasurement _britishFootBenoit1895A;
        private static UnitOfMeasurement _britishFootBenoit1895B;
        private static UnitOfMeasurement _britishFootSears1922;
        private static UnitOfMeasurement _britishFootSears1922T;
        private static UnitOfMeasurement _britishLinkBenoit1895A;
        private static UnitOfMeasurement _britishLinkBenoit1895B;
        private static UnitOfMeasurement _britishLinkSears1922;
        private static UnitOfMeasurement _britishLinkSears1922T;
        private static UnitOfMeasurement _britishYardBenoit1895A;
        private static UnitOfMeasurement _britishYardBenoit1895B;
        private static UnitOfMeasurement _britishYardSears1922;        
        private static UnitOfMeasurement _britishYardSears1922T;
        private static UnitOfMeasurement _centesimalMinute;
        private static UnitOfMeasurement _centesimalSecond;
        private static UnitOfMeasurement _chain;
        private static UnitOfMeasurement _clarkesChain;
        private static UnitOfMeasurement _clarkesFoot;
        private static UnitOfMeasurement _clarkesLink;
        private static UnitOfMeasurement _clarkesYard;
        private static UnitOfMeasurement _coefficient;
        private static UnitOfMeasurement _degree;
        private static UnitOfMeasurement _degreeHemishere;
        private static UnitOfMeasurement _degreeMinute;
        private static UnitOfMeasurement _degreeMinuteHemishere;
        private static UnitOfMeasurement _degreeMinuteSecond;
        private static UnitOfMeasurement _degreeMinuteSecondHemishere;
        private static UnitOfMeasurement _degreeSupplierDefined;
        private static UnitOfMeasurement _fathom;
        private static UnitOfMeasurement _foot;
        private static UnitOfMeasurement _germanLegalMetre;
        private static UnitOfMeasurement _goldCoastFoot;
        private static UnitOfMeasurement _gon;
        private static UnitOfMeasurement _grad;
        private static UnitOfMeasurement _hemisphereDegree;
        private static UnitOfMeasurement _hemisphereDegreeMinute;
        private static UnitOfMeasurement _hemisphereDegreeMinuteSecond;
        private static UnitOfMeasurement _indianFoot;
        private static UnitOfMeasurement _indianFoot1937;
        private static UnitOfMeasurement _indianFoot1962;
        private static UnitOfMeasurement _indianFoot1975;
        private static UnitOfMeasurement _indianYard;
        private static UnitOfMeasurement _indianYard1937;
        private static UnitOfMeasurement _indianYard1962;
        private static UnitOfMeasurement _indianYard1975;
        private static UnitOfMeasurement _kilometre;
        private static UnitOfMeasurement _link;
        private static UnitOfMeasurement _metre;
        private static UnitOfMeasurement _microRadian;
        private static UnitOfMeasurement _mil6400;
        private static UnitOfMeasurement _nauticalMile;
        private static UnitOfMeasurement _partsPerMillion;
        private static UnitOfMeasurement _radian;
        private static UnitOfMeasurement _sexagesimalDM;
        private static UnitOfMeasurement _sexagesimalDMS;
        private static UnitOfMeasurement _sexagesimalDMSs;
        private static UnitOfMeasurement _statuteMile;
        private static UnitOfMeasurement _unity;
        private static UnitOfMeasurement _usSurveyChain;
        private static UnitOfMeasurement _usSurveyFoot;
        private static UnitOfMeasurement _usSurveyLink;
        private static UnitOfMeasurement _usSurveyMile;
        private static UnitOfMeasurement _yard;

        #endregion

        #region Angle

        /// <summary>
        /// Arc-minute.
        /// </summary>
        public static UnitOfMeasurement ArcMinute
        {
            get
            {
                return _arcMinute ?? (_arcMinute = new UnitOfMeasurement("EPSG::9103", "arc-minute",
                                                                         null, new String[] {"min", "mina"},
                                                                         "'", Constants.PI / 10800, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Arc-second.
        /// </summary>
        public static UnitOfMeasurement ArcSecond
        {
            get
            {
                return _arcSecond ?? (_arcSecond = new UnitOfMeasurement("EPSG::9104", "arc-second",
                                                                         null, new String[] {"sec", "seca"},
                                                                         "\"", Constants.PI / 648000, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Centesimal minute.
        /// </summary>
        public static UnitOfMeasurement CentesimalMinute
        {
            get
            {
                return _centesimalMinute ?? (_centesimalMinute = new UnitOfMeasurement("EPSG::9113", "centesimal minute",
                                                                                       "/100 of a grad and gon = ((pi/200) / 100) radians.",
                                                                                       new String[] {"c", "cgr"},
                                                                                       "c", Constants.PI / 20000,
                                                                                       UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Centesimal second.
        /// </summary>
        public static UnitOfMeasurement CentesimalSecond
        {
            get
            {
                return _centesimalSecond ?? (_centesimalSecond = new UnitOfMeasurement("EPSG::9113", "centesimal second",
                                                                                       "1/100 of a centesimal minute or 1/10,000th of a grad and gon = ((pi/200) / 10000) radians.",
                                                                                       new String[] {"cc", "ccgr"},
                                                                                       "cc", Constants.PI / 2000000,
                                                                                       UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree.
        /// </summary>
        public static UnitOfMeasurement Degree
        {
            get
            {
                return _degree ?? (_degree = new UnitOfMeasurement("EPSG::9102", "degree",
                                                                   "= pi/180 radians", new String[] {"deg", "dega"},
                                                                   "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeHemisphere
        {
            get
            {
                return _degreeHemishere ?? (_degreeHemishere = new UnitOfMeasurement("EPSG::9116", "degree hemisphere",
                                                                                     "Degree representation. Format: degrees (real, any precision) - hemisphere abbreviation (single character N S E or W). Convert to degrees using algorithm.",
                                                                                     new String[] {"degH", "dega"},
                                                                                     "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree minute.
        /// </summary>
        public static UnitOfMeasurement DegreeMinute
        {
            get
            {
                return _degreeMinute ?? (_degreeMinute = new UnitOfMeasurement("EPSG::9115", "degree minute",
                                                                               "Degree representation. Format: signed degrees (integer) - arc-minutes (real, any precision). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.",
                                                                               new String[] {"DM", "dega"},
                                                                               "°'", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree minute hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteHemisphere
        {
            get
            {
                return _degreeMinuteHemishere ??
                       (_degreeMinuteHemishere = new UnitOfMeasurement("EPSG::9118", "degree minute hemisphere",
                                                                       "Degree representation. Format: degrees (integer) - arc-minutes (real, any precision) - hemisphere abbreviation (single character N S E or W). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.",
                                                                       new String[] {"DMH", "dega"},
                                                                       "°'", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree minute second.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteSecond
        {
            get
            {
                return _degreeMinuteSecond ?? (_degreeMinuteSecond = new UnitOfMeasurement("EPSG::9107", "degree minute second",
                                                                                           "Degree representation. Format: signed degrees (integer) - arc-minutes (integer) - arc-seconds (real, any precision). Different symbol sets are in use as field separators, for example º ' \". Convert to degrees using algorithm.",
                                                                                           new String[] {"DMS", "dega"},
                                                                                           "°'\"", Constants.PI / 180,
                                                                                           UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree minute second hemisphere.
        /// </summary>
        public static UnitOfMeasurement DegreeMinuteSecondHemisphere
        {
            get
            {
                return _degreeMinuteSecondHemishere ??
                       (_degreeMinuteSecondHemishere = new UnitOfMeasurement("EPSG::9108", "degree minute second hemisphere",
                                                                             "Degree representation. Format: degrees (integer) - arc-minutes (integer) - arc-seconds (real) - hemisphere abbreviation (single character N S E or W). Different symbol sets are in use as field separators for example º ' \". Convert to deg using algorithm.",
                                                                             new String[] {"DMSH", "dega"},
                                                                             "°'\"", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Degree (supplier to define representation).
        /// </summary>
        public static UnitOfMeasurement DegreeSupplierDefined
        {
            get
            {
                return _degreeSupplierDefined ??
                       (_degreeSupplierDefined = new UnitOfMeasurement("EPSG::9122", "degree (supplier to define representation)",
                                                                       "= pi/180 radians. The degree representation (e.g. decimal, DMSH, etc.) must be clarified by suppliers of data associated with this code.",
                                                                       new String[] {"deg", "dega"},
                                                                       "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Gon.
        /// </summary>
        public static UnitOfMeasurement Gon
        {
            get
            {
                return _gon ?? (_gon = new UnitOfMeasurement("EPSG::9106", "gon",
                                                             "=pi/200 radians", new String[] {"g"},
                                                             "g", Constants.PI / 200, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Grad.
        /// </summary>
        public static UnitOfMeasurement Grad
        {
            get
            {
                return _grad ?? (_grad = new UnitOfMeasurement("EPSG::9105", "grad",
                                                               "=pi/200 radians", new String[] {"gr"},
                                                               "gr", Constants.PI / 200, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Hemisphere degree.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegree
        {
            get
            {
                return _hemisphereDegree ?? (_hemisphereDegree = new UnitOfMeasurement("EPSG::9117", "hemisphere degree",
                                                                                       "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (real, any precision). Convert to degrees using algorithm.",
                                                                                       new String[] {"Hdeg", "dega"},
                                                                                       "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Hemisphere degree minute.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegreeMinute
        {
            get
            {
                return _hemisphereDegreeMinute ??
                       (_hemisphereDegreeMinute = new UnitOfMeasurement("EPSG::9119", "hemisphere degree minute",
                                                                        "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (integer) - arc-minutes (real, any precision). Different symbol sets are in use as field separators, for example º '. Convert to degrees using algorithm.",
                                                                        new String[] {"HDM", "dega"},
                                                                        "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Hemisphere degree minute second.
        /// </summary>
        public static UnitOfMeasurement HemisphereDegreeMinuteSecond
        {
            get
            {
                return _hemisphereDegreeMinuteSecond ??
                       (_hemisphereDegreeMinuteSecond = new UnitOfMeasurement("EPSG::9120", "hemisphere degree minute second",
                                                                              "Degree representation. Format: hemisphere abbreviation (single character N S E or W) - degrees (integer) - arc-minutes (integer) - arc-seconds (real). Different symbol sets are in use as field separators for example º ' \". Convert to deg using algorithm.",
                                                                              new String[] {"HDMS", "dega"},
                                                                              "°'|\"", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Microradian.
        /// </summary>
        public static UnitOfMeasurement MicroRadian
        {
            get
            {
                return _microRadian ?? (_microRadian = new UnitOfMeasurement("EPSG::9109", "microradian",
                                                                             "rad * 10E-6", new String[] {"µrad", "urad"},
                                                                             "µrad", 0.000001, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Mil_6400.
        /// </summary>
        public static UnitOfMeasurement Mil6400
        {
            get
            {
                return _mil6400 ?? (_mil6400 = new UnitOfMeasurement("EPSG::9114", "mil_6400",
                                                                     "Angle subtended by 1/6400 part of a circle. Approximates to 1/1000th radian. Note that other approximations (notably 1/6300 circle and 1/6000 circle) also exist.",
                                                                     new String[] {"mil", "mila"},
                                                                     "mil", 0.0009817477042468094, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Radian.
        /// </summary>
        public static UnitOfMeasurement Radian
        {
            get
            {
                return _radian ?? (_radian = new UnitOfMeasurement("EPSG::9101", "radian",
                                                                   String.Empty, new String[] {"rad"},
                                                                   "rad", 1, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Sexagesimal DM.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDM
        {
            get
            {
                return _sexagesimalDM ?? (_sexagesimalDM = new UnitOfMeasurement("EPSG::9111", "sexagesimal DM",
                                                                                 "Pseudo unit. Format: sign - degrees - decimal point - integer minutes (two digits) - fraction of minutes (any precision). Must include leading zero in integer minutes. Must exclude decimal point for minutes. Convert to deg using algorithm.",
                                                                                 new String[] {"DDD.MMm", "dega"},
                                                                                 "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Sexagesimal DMS.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDMS
        {
            get
            {
                return _sexagesimalDMS ?? (_sexagesimalDMS = new UnitOfMeasurement("EPSG::9110", "sexagesimal DMS",
                                                                                   "Pseudo unit. Format: signed degrees - period - minutes (2 digits) - integer seconds (2 digits) - fraction of seconds (any precision). Must include leading zero in minutes and seconds and exclude decimal point for seconds. Convert to degree using formula.",
                                                                                   new String[]
                                                                                       {"DDD.MMSSsss", "dega", "sexagesimal degree"},
                                                                                   "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }

        /// <summary>
        /// Sexagesimal DMS.s.
        /// </summary>
        public static UnitOfMeasurement SexagesimalDMSs
        {
            get
            {
                return _sexagesimalDMSs ?? (_sexagesimalDMSs = new UnitOfMeasurement("EPSG::9121", "sexagesimal DM",
                                                                                     "Pseudo unit. Format: signed degrees - minutes (two digits) - seconds (real, any precision). Must include leading zero in minutes and seconds where value is under 10 and include decimal separator for seconds. Convert to degree using algorithm.",
                                                                                     new String[] {"DDDMMSS.sss", "dega"},
                                                                                     "°", Constants.PI / 180, UnitQuantityType.Angle));
            }
        }


        #endregion

        #region Length

        /// <summary>
        /// British chain (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishChainBenoit1895A
        {
            get
            {
                return _britishChainBenoit1895A ??
                       (_britishChainBenoit1895A = new UnitOfMeasurement("EPSG::9052", "British chain (Benoit 1895 A)",
                                                                         "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.",
                                                                         new String[] {"chain", "chBnA"},
                                                                         "ch", 20.1167824, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British chain (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishChainBenoit1895B
        {
            get
            {
                return _britishChainBenoit1895B ??
                       (_britishChainBenoit1895B = new UnitOfMeasurement("EPSG::9062", "British chain (Benoit 1895 B)",
                                                                         "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.",
                                                                         new String[] {"chain", "chBnB"},
                                                                         "ch", 20.11678249437587, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British chain (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishChainSears1922
        {
            get
            {
                return _britishChainSears1922 ??
                       (_britishChainSears1922 = new UnitOfMeasurement("EPSG::9042", "British chain (Sears 1922)",
                                                                       "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.",
                                                                       new String[] {"chain", "chSe"},
                                                                       "ch", 20.116765121552632, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British chain (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishChainSears1922T
        {
            get
            {
                return _britishChainSears1922T ??
                       (_britishChainSears1922T = new UnitOfMeasurement("EPSG::9301", "British chain (Sears 1922 truncated)",
                                                                        "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 1 chSe(T) = 22 ydSe(T). Used in metrication of Malaya RSO grid.",
                                                                        new String[] {"chain", "chSe(T)"},
                                                                        "ch", 20.116756, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British foot (1865).
        /// </summary>
        public static UnitOfMeasurement BritishFoot1865
        {
            get
            {
                return _britishFoot1865 ?? (_britishFoot1865 = new UnitOfMeasurement("EPSG::9070", "British foot (1865)",
                                                                                     "Uses Clark's estimate of 1853-1865 British foot-metre ratio of 0.9144025 metres per yard. Used in 1962 and 1975 estimates of Indian foot.",
                                                                                     new String[] {"foot", "ftBr(65)"},
                                                                                     "ft", 0.30480083333333335, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British foot (1936).
        /// </summary>
        public static UnitOfMeasurement BritishFoot1936
        {
            get
            {
                return _britishFoot1936 ?? (_britishFoot1936 = new UnitOfMeasurement("EPSG::9095", "British foot (1936)",
                                                                                     "For the 1936 re-triangulation OSGB defines the relationship of 10 feet of 1796 to the International metre through the logarithmic relationship (10^0.48401603 exactly). 1 ft = 0.3048007491…m. Also used for metric conversions in Ireland.",
                                                                                     new String[] {"foot", "ftBr(36)"},
                                                                                     "ft", 0.3048007491, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British foot (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishFootBenoit1895A
        {
            get
            {
                return _britishFootBenoit1895A ??
                       (_britishFootBenoit1895A = new UnitOfMeasurement("EPSG::9051", "British foot (Benoit 1895 A)",
                                                                        "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.",
                                                                        new String[] {"foot", "ftBnA"},
                                                                        "ft", 0.3047997333333333, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British foot (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishFootBenoit1895B
        {
            get
            {
                return _britishFootBenoit1895B ??
                       (_britishFootBenoit1895B = new UnitOfMeasurement("EPSG::9061", "British foot (Benoit 1895 B)",
                                                                        "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.",
                                                                        new String[] {"foot", "ftBnB"},
                                                                        "ft", 0.30479973476327077, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British foot (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishFootSears1922
        {
            get
            {
                return _britishFootSears1922 ??
                       (_britishFootSears1922 = new UnitOfMeasurement("EPSG::9041", "British foot (Sears 1922)",
                                                                      "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.",
                                                                      new String[] {"foot", "ftSe"},
                                                                      "ft", 0.3047994715386762, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British foot (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishFootSears1922T
        {
            get
            {
                return _britishFootSears1922T ??
                       (_britishFootSears1922T = new UnitOfMeasurement("EPSG::9300", "British foot (Sears 1922 truncated)",
                                                                       "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ",
                                                                       new String[] {"foot", "ftSe(T)"},
                                                                       "ft", 0.3047994715386762, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British link (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishLinkBenoit1895A
        {
            get
            {
                return _britishLinkBenoit1895A ??
                       (_britishLinkBenoit1895A = new UnitOfMeasurement("EPSG::9053", "British link (Benoit 1895 A)",
                                                                        "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.",
                                                                        new String[] {"link", "ftBnA"},
                                                                        "l", 0.201167824, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British link (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishLinkBenoit1895B
        {
            get
            {
                return _britishLinkBenoit1895B ??
                       (_britishLinkBenoit1895B = new UnitOfMeasurement("EPSG::9063", "British link (Benoit 1895 B)",
                                                                        "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.",
                                                                        new String[] {"link", "ftBnB"},
                                                                        "l", 0.2011678249437587, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British link (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishLinkSears1922
        {
            get
            {
                return _britishLinkSears1922 ??
                       (_britishLinkSears1922 = new UnitOfMeasurement("EPSG::9043", "British link (Sears 1922)",
                                                                      "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.",
                                                                      new String[] {"link", "ftSe"},
                                                                      "l", 0.2011676512155263, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British link (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishLinkSears1922T
        {
            get
            {
                return _britishLinkSears1922T ??
                       (_britishLinkSears1922T = new UnitOfMeasurement("EPSG::9302", "British link (Sears 1922 truncated)",
                                                                       "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ",
                                                                       new String[] {"link", "ftSe(T)"},
                                                                       "l", 0.20116756, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British yard (Benoit 1895 A).
        /// </summary>
        public static UnitOfMeasurement BritishYardBenoit1895A
        {
            get
            {
                return _britishYardBenoit1895A ??
                       (_britishYardBenoit1895A = new UnitOfMeasurement("EPSG::9050", "British yard (Benoit 1895 A)",
                                                                        "Uses Benoit's 1895 British yard-metre ratio as given by Clark as 0.9143992 metres per yard. Used for deriving metric size of ellipsoid in Palestine.",
                                                                        new String[] {"yard", "ftBnA"},
                                                                        "yd", 0.9143992, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British yard (Benoit 1895 B).
        /// </summary>
        public static UnitOfMeasurement BritishYardBenoit1895B
        {
            get
            {
                return _britishYardBenoit1895B ??
                       (_britishYardBenoit1895B = new UnitOfMeasurement("EPSG::9060", "British yard (Benoit 1895 B)",
                                                                        "Uses Benoit's 1895 British yard-metre ratio as given by Bomford as 39.370113 inches per metre. Used in West Malaysian mapping.",
                                                                        new String[] {"yard", "ftBnB"},
                                                                        "yd", 0.9143992042898124, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British yard (Sears 1922).
        /// </summary>
        public static UnitOfMeasurement BritishYardSears1922
        {
            get
            {
                return _britishYardSears1922 ??
                       (_britishYardSears1922 = new UnitOfMeasurement("EPSG::9040", "British yard (Sears 1922)",
                                                                      "Uses Sear's 1922 British yard-metre ratio as given by Bomford as 39.370147 inches per metre. Used in East Malaysian and older New Zealand mapping.",
                                                                      new String[] {"yard", "ftSe"},
                                                                      "yd", 0.9143984146160287, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// British yard (Sears 1922 truncated).
        /// </summary>
        public static UnitOfMeasurement BritishYardSears1922T
        {
            get
            {
                return _britishYardSears1922T ??
                       (_britishYardSears1922T = new UnitOfMeasurement("EPSG::9099", "British yard (Sears 1922 truncated)",
                                                                       "Uses Sear's 1922 British yard-metre ratio (UoM code 9040) truncated to 6 significant figures; this truncated ratio (0.914398, UoM code 9099) then converted to other imperial units. 3 ftSe(T) = 1 ydSe(T). ",
                                                                       new String[] {"yard", "ftSe(T)"},
                                                                       "yd", 0.914398, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Chain.
        /// </summary>
        public static UnitOfMeasurement Chain
        {
            get
            {
                return _chain ?? (_chain = new UnitOfMeasurement("EPSG::9097", "chain",
                                                                 "=22 international yards or 66 international feet.",
                                                                 new String[] {"international chain", "ch"},
                                                                 "ch", 20.1168, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Clarke's chain.
        /// </summary>
        public static UnitOfMeasurement ClarkesChain
        {
            get
            {
                return _clarkesChain ?? (_clarkesChain = new UnitOfMeasurement("EPSG::9038", "Clarke's chain",
                                                                               "=22 Clarke's yards. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.",
                                                                               new String[] {"chCla"},
                                                                               "ch", 20.1166195164, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Clarke's foot.
        /// </summary>
        public static UnitOfMeasurement ClarkesFoot
        {
            get
            {
                return _clarkesFoot ?? (_clarkesFoot = new UnitOfMeasurement("EPSG::9005", "Clarke's foot",
                                                                             "Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.",
                                                                             new String[] {"South African geodetic foot", "ftCla"},
                                                                             "ft", 0.3047972654, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Clarke's link.
        /// </summary>
        public static UnitOfMeasurement ClarkesLink
        {
            get
            {
                return _clarkesLink ?? (_clarkesLink = new UnitOfMeasurement("EPSG::9039", "Clarke's link",
                                                                             "=1/100 Clarke's chain. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.",
                                                                             new String[] {"link (Clarke's ratio)", "lkCla"},
                                                                             "l", 0.201166195164, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Clarke's yard.
        /// </summary>
        public static UnitOfMeasurement ClarkesYard
        {
            get
            {
                return _clarkesYard ?? (_clarkesYard = new UnitOfMeasurement("EPSG::9037", "Clarke's yard",
                                                                             "=3 Clarke's feet. Assumes Clarke's 1865 ratio of 1 British foot = 0.3047972654 French legal metres applies to the international metre. Used in older Australian, southern African & British West Indian mapping.",
                                                                             new String[] {"ydCla"},
                                                                             "yd", 0.9143917962, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Fathom.
        /// </summary>
        public static UnitOfMeasurement Fathom
        {
            get
            {
                return _fathom ?? (_fathom = new UnitOfMeasurement("EPSG::9014", "fathom",
                                                                   String.Empty, new String[] {"f"},
                                                                   "f", 1.8288, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Foot.
        /// </summary>
        public static UnitOfMeasurement Foot
        {
            get
            {
                return _foot ?? (_foot = new UnitOfMeasurement("EPSG::9002", "foot",
                                                               String.Empty, new String[] {"ft"},
                                                               "ft", 0.3048, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// German legal metre.
        /// </summary>
        public static UnitOfMeasurement GermanLegalMetre
        {
            get
            {
                return _germanLegalMetre ?? (_germanLegalMetre = new UnitOfMeasurement("EPSG::9031", "German legal metre",
                                                                                       "Used in Namibia.", new String[] {"GLM", "mGer"},
                                                                                       "m", 1.0000135965, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Gold Coast foot.
        /// </summary>
        public static UnitOfMeasurement GoldCoastFoot
        {
            get
            {
                return _goldCoastFoot ?? (_goldCoastFoot = new UnitOfMeasurement("EPSG::9094", "Gold Coast foot",
                                                                                 "Used in Ghana and some adjacent parts of British west Africa prior to metrication, except for the metrication of projection defining parameters when British foot (Sears 1922) used.",
                                                                                 new String[] {"foot", "ftGC"},
                                                                                 "ft", 0.3047997101815088, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian foot.
        /// </summary>
        public static UnitOfMeasurement IndianFoot
        {
            get
            {
                return _indianFoot ?? (_indianFoot = new UnitOfMeasurement("EPSG::9080", "Indian foot",
                                                                           "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (= 3 British feet) taken to be J.S.Clark's 1865 value of 0.9144025 metres.",
                                                                           new String[] {"Indian geodetic foot", "ftInd"},
                                                                           "ft", 0.30479951024814694, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian foot (1937).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1937
        {
            get
            {
                return _indianFoot1937 ?? (_indianFoot1937 = new UnitOfMeasurement("EPSG::9081", "Indian foot (1937)",
                                                                                   "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British foot taken to be 1895 Benoit value of 12/39.370113m. Rounded to 8 decimal places as 0.30479841. Used from Bangladesh to Vietnam. Previously used in India and Pakistan but superseded.",
                                                                                   new String[] {"Indian geodetic foot", "ftInd(37)"},
                                                                                   "ft", 0.30479841, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian foot (1962).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1962
        {
            get
            {
                return _indianFoot1962 ?? (_indianFoot1962 = new UnitOfMeasurement("EPSG::9082", "Indian foot (1962)",
                                                                                   "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures with a small error as 1 Ind ft=0.3047996m. Used in Pakistan since metrication.",
                                                                                   new String[] {"ftInd(62)"},
                                                                                   "ft", 0.3047996, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian foot (1975).
        /// </summary>
        public static UnitOfMeasurement IndianFoot1975
        {
            get
            {
                return _indianFoot1975 ?? (_indianFoot1975 = new UnitOfMeasurement("EPSG::9083", "Indian foot (1975)",
                                                                                   "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures as 1 Ind ft=0.3047995m. Used in India since metrication.",
                                                                                   new String[] {"ftInd(75)"},
                                                                                   "ft", 0.3047995, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian yard.
        /// </summary>
        public static UnitOfMeasurement IndianYard
        {
            get
            {
                return _indianYard ?? (_indianYard = new UnitOfMeasurement("EPSG::9084", "Indian yard",
                                                                           "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (= 3 British feet) taken to be J.S.Clark's 1865 value of 0.9144025 metres.",
                                                                           new String[] {"yard", "ydInd"},
                                                                           "yd", 0.9143985307444408, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian yard (1937).
        /// </summary>
        public static UnitOfMeasurement IndianYard1937
        {
            get
            {
                return _indianYard1937 ?? (_indianYard1937 = new UnitOfMeasurement("EPSG::9085", "Indian yard (1937)",
                                                                                   "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be 1895 Benoit value of 12/39.370113m. Rounded to 8 decimal places as 0.30479841. Used from Bangladesh to Vietnam. Previously used in India and Pakistan but superseded.",
                                                                                   new String[] {"yard", "ydInd(37)"},
                                                                                   "yd", 0.9143952, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian yard (1962).
        /// </summary>
        public static UnitOfMeasurement IndianYard1962
        {
            get
            {
                return _indianYard1962 ?? (_indianYard1962 = new UnitOfMeasurement("EPSG::9086", "Indian yard (1962)",
                                                                                   "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures with a small error as 1 Ind ft=0.3047996m. Used in Pakistan since metrication.",
                                                                                   new String[] {"ydInd(62)"},
                                                                                   "yd", 0.9143988, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Indian yard (1975).
        /// </summary>
        public static UnitOfMeasurement IndianYard1975
        {
            get
            {
                return _indianYard1975 ?? (_indianYard1975 = new UnitOfMeasurement("EPSG::9087", "Indian yard (1975)",
                                                                                   "Indian Foot = 0.99999566 British feet (A.R.Clarke 1865). British yard (3 feet) taken to be J.S. Clark's 1865 value of 0.9144025m. Rounded to 7 significant figures as 1 Ind ft=0.3047995m. Used in India since metrication.",
                                                                                   new String[] {"ydInd(75)"},
                                                                                   "yd", 0.9143985, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Kilometre.
        /// </summary>
        public static UnitOfMeasurement Kilometre
        {
            get
            {
                return _kilometre ?? (_kilometre = new UnitOfMeasurement("EPSG::9036", "kilometre",
                                                                         String.Empty, new String[] {"kilometer", "km"},
                                                                         "km", 1000, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Link.
        /// </summary>
        public static UnitOfMeasurement Link
        {
            get
            {
                return _link ?? (_link = new UnitOfMeasurement("EPSG::9098", "link",
                                                               "=1/100 international chain", new String[] {"international link", "lk"},
                                                               "lk", 0.201168, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Metre.
        /// </summary>
        public static UnitOfMeasurement Metre
        {
            get
            {
                return _metre ?? (_metre = new UnitOfMeasurement("EPSG::9001", "metre",
                                                                 String.Empty, new String[] {"meter", "International metre", "m"},
                                                                 "m", 1, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Nautical mile.
        /// </summary>
        public static UnitOfMeasurement NauticalMile
        {
            get
            {
                return _nauticalMile ?? (_nauticalMile = new UnitOfMeasurement("EPSG::9030", "nautical mile",
                                                                               String.Empty, new String[] {"NM", "nautmi"},
                                                                               "NM", 1852, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Statute mile.
        /// </summary>
        public static UnitOfMeasurement StatuteMile
        {
            get
            {
                return _statuteMile ?? (_statuteMile = new UnitOfMeasurement("EPSG::9093", "Statute mile",
                                                                             "=5280 feet", new String[] {"mi"},
                                                                             "M", 1609.344, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// US survey chain.
        /// </summary>
        public static UnitOfMeasurement USSurveyChain
        {
            get
            {
                return _usSurveyChain ?? (_usSurveyChain = new UnitOfMeasurement("EPSG::9033", "US survey chain",
                                                                                 "Used in USA primarily for public lands cadastral work.",
                                                                                 new String[] {"chUS"},
                                                                                 "ch", 20.11684023368047, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// US survey foot.
        /// </summary>
        public static UnitOfMeasurement USSurveyFoot
        {
            get
            {
                return _usSurveyFoot ?? (_usSurveyFoot = new UnitOfMeasurement("EPSG::9003", "US survey foot",
                                                                               "Used in USA.", new String[] {"American foot", "ftUS"},
                                                                               "ft", 0.30480060960121924, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// US survey link.
        /// </summary>
        public static UnitOfMeasurement USSurveyLink
        {
            get
            {
                return _usSurveyLink ?? (_usSurveyLink = new UnitOfMeasurement("EPSG::9034", "US survey link",
                                                                               "Used in USA primarily for public lands cadastral work.",
                                                                               new String[] {"lkUS"},
                                                                               "lk", 0.2011684023368047, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// US survey mile.
        /// </summary>
        public static UnitOfMeasurement USSurveyMile
        {
            get
            {
                return _usSurveyMile ?? (_usSurveyMile = new UnitOfMeasurement("EPSG::9035", "US survey mile",
                                                                               "Used in USA primarily for public lands cadastral work.",
                                                                               new String[] {"miUS"},
                                                                               "M", 1609.3472186944375, UnitQuantityType.Length));
            }
        }

        /// <summary>
        /// Yard.
        /// </summary>
        public static UnitOfMeasurement Yard
        {
            get
            {
                return _yard ?? (_yard = new UnitOfMeasurement("EPSG::9096", "yard",
                                                               "=3 international feet.", new String[] {"international yard", "yd"},
                                                               "yd", 0.9144, UnitQuantityType.Length));
            }
        }

        #endregion

        #region Scale

        /// <summary>
        /// Bin.
        /// </summary>
        public static UnitOfMeasurement Bin
        {
            get
            {
                return _bin ?? (_bin = new UnitOfMeasurement("EPSG::1024", "bin",
                                                             "One of the dimensions of a seismic bin. Its size (in units of the bin grid's base coordinate reference system) is defined through an associated coordinate operation.",
                                                             null,
                                                             "bin", 1, UnitQuantityType.Scale));
            }
        }

        /// <summary>
        /// Coefficient.
        /// </summary>
        public static UnitOfMeasurement Coefficient
        {
            get
            {
                return _coefficient ?? (_coefficient = new UnitOfMeasurement("EPSG::9203", "coefficient",
                                                                             "Used when parameters are coefficients. They inherently take the units which depend upon the term to which the coefficient applies.",
                                                                             null,
                                                                             String.Empty, 1, UnitQuantityType.Scale));
            }
        }

        /// <summary>
        /// Parts per million.
        /// </summary>
        public static UnitOfMeasurement PartsPerMillion
        {
            get
            {
                return _partsPerMillion ?? (_partsPerMillion = new UnitOfMeasurement("EPSG::9202", "parts per million",
                                                                                     String.Empty, 0.000001, UnitQuantityType.Scale));
            }
        }

        /// <summary>
        /// Unity.
        /// </summary>
        public static UnitOfMeasurement Unity
        {
            get
            {
                return _unity ?? (_unity = new UnitOfMeasurement("EPSG::9201", "unity",
                                                                 String.Empty, 1, UnitQuantityType.Scale));
            }
        }

        #endregion     
    }
}
