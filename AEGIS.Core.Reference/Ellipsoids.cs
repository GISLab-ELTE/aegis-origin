// <copyright file="Ellipsoids.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="Ellipsoid" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(Ellipsoid))]
    public static class Ellipsoids
    {
        #region Query fields

        private static Ellipsoid[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="Ellipsoid" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="Ellipsoid" /> instances within the collection.</value>
        public static IList<Ellipsoid> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(Ellipsoids).GetProperties().
                                              Where(property => property.Name != "All").
                                              Select(property => property.GetValue(null, null) as Ellipsoid).
                                              ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="Ellipsoid" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="Ellipsoid" /> instances that match the specified identifier.</returns>
        public static IList<Ellipsoid> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="Ellipsoid" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="Ellipsoid" /> instances that match the specified name.</returns>
        public static IList<Ellipsoid> FromName(String name)
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

        private static Ellipsoid _airy1830;
        private static Ellipsoid _airy1849;
        private static Ellipsoid _ATS1977;
        private static Ellipsoid _australianNS;
        private static Ellipsoid _bessel1841;
        private static Ellipsoid _besselM;
        private static Ellipsoid _besselNamibia;
        private static Ellipsoid _CGCS2000;
        private static Ellipsoid _clarke1858;
        private static Ellipsoid _clarke1866;
        private static Ellipsoid _clarke1866AS;
        private static Ellipsoid _clarke1866M;
        private static Ellipsoid _clarke1880;
        private static Ellipsoid _clarke1880B;
        private static Ellipsoid _clarke1880IGN;
        private static Ellipsoid _clarke1880RGS;
        private static Ellipsoid _clarke1880A;
        private static Ellipsoid _clarke1922;
        private static Ellipsoid _danish1876;
        private static Ellipsoid _everest1830;
        private static Ellipsoid _everest1937;
        private static Ellipsoid _everest1962;
        private static Ellipsoid _everest1967;
        private static Ellipsoid _everest1969;
        private static Ellipsoid _everest1975;
        private static Ellipsoid _everest1830M;
        private static Ellipsoid _GEM10C;
        private static Ellipsoid _GRS1967;
        private static Ellipsoid _GRS1967M;
        private static Ellipsoid _GRS1980;
        private static Ellipsoid _GRS1980AS;
        private static Ellipsoid _helmert1906;
        private static Ellipsoid _hough1960;
        private static Ellipsoid _hughes1960;
        private static Ellipsoid _IAG1975;
        private static Ellipsoid _indonesianNS;
        private static Ellipsoid _international1924;
        private static Ellipsoid _international1924AS;
        private static Ellipsoid _international1967;
        private static Ellipsoid _krassowsky1940;
        private static Ellipsoid _NWL9D;
        private static Ellipsoid _OSU86F;
        private static Ellipsoid _OSU91A;
        private static Ellipsoid _plessis1817;
        private static Ellipsoid _PZ90;
        private static Ellipsoid _struve1860;
        private static Ellipsoid _warOffice;
        private static Ellipsoid _WGS1972;
        private static Ellipsoid _WGS1984;


        #endregion

        #region Public static properties

        /// <summary>
        /// Airy 1830.
        /// </summary>
        public static Ellipsoid Airy1830 { get { return _airy1830 ?? (_airy1830 = Ellipsoid.FromSemiMinorAxis("EPSG::7001", "Airy 1830", 6377563.396, 6356256.910)); } }

        /// <summary>
        /// Airy Modified 1849.
        /// </summary>
        public static Ellipsoid Airy1849 { get { return _airy1849 ?? (_airy1849 = Ellipsoid.FromSemiMinorAxis("EPSG::7002", "Airy Modified 1849", 6377340.189, 6356034.446)); } }

        /// <summary>
        /// Average Terrestrial System 1977.
        /// </summary>
        public static Ellipsoid ATS1977 { get { return _ATS1977 ?? (_ATS1977 = Ellipsoid.FromInverseFlattening("EPSG::7041", "Average Terrestrial System 1977", 6378135, 298.257)); } }

        /// <summary>
        /// Australian National Spheroid.
        /// </summary>
        public static Ellipsoid AustralianNS { get { return _australianNS ?? (_australianNS = Ellipsoid.FromInverseFlattening("EPSG::7003", "Australian National Spheroid", 6378160, 298.25)); } }

        /// <summary>
        /// Bessel 1841.
        /// </summary>
        public static Ellipsoid Bessel1841 { get { return _bessel1841 ?? (_bessel1841 = Ellipsoid.FromInverseFlattening("EPSG::7004", "Bessel 1841", 6377397.155, 299.1528128)); } }

        /// <summary>
        /// Bessel Modified.
        /// </summary>
        public static Ellipsoid BesselM { get { return _besselM ?? (_besselM = Ellipsoid.FromInverseFlattening("EPSG::7005", "Bessel Modified", 6377492.018, 299.1528128)); } }

        /// <summary>
        /// Bessel Namibia.
        /// </summary>
        public static Ellipsoid BesselNamibia { get { return _besselNamibia ?? (_besselNamibia = Ellipsoid.FromInverseFlattening("EPSG::7046", "Bessel Namibia", 6377483.865, 299.1528128)); } }

        /// <summary>
        /// China Geodetic Coordinate System 2000.
        /// </summary>
        public static Ellipsoid CGCS2000 { get { return _CGCS2000 ?? (_CGCS2000 = Ellipsoid.FromInverseFlattening("EPSG::1024", "China Geodetic Coordinate System 2000", 6378137, 298.257222101)); } }

        /// <summary>
        /// Clarke 1858.
        /// </summary>
        public static Ellipsoid Clarke1858 { get { return _clarke1858 ?? (_clarke1858 = Ellipsoid.FromSemiMinorAxis("EPSG::7007", "Clarke 1858", Length.FromClarkesFoot(20926348), Length.FromClarkesFoot(20855233))); } }

        /// <summary>
        /// Clarke 1866.
        /// </summary>
        public static Ellipsoid Clarke1866 { get { return _clarke1866 ?? (_clarke1866 = Ellipsoid.FromInverseFlattening("EPSG::7008", "Clarke 1866", 6378206.4, 294.9786982)); } }

        /// <summary>
        /// Clarke 1866 Authalic Sphere.
        /// </summary>
        public static Ellipsoid Clarke1866AS { get { return _clarke1866AS ?? (_clarke1866AS = Ellipsoid.FromSphere("EPSG::7052", "Clarke 1866 Authalic Sphere", 6370997)); } }

        /// <summary>
        /// Clarke 1866 Michigan.
        /// </summary>
        public static Ellipsoid Clarke1866M { get { return _clarke1866M ?? (_clarke1866M = Ellipsoid.FromSemiMinorAxis("EPSG::7009", "Clarke 1866 Michigan", Length.FromFoot(20926631.531), Length.FromFoot(20855688.674))); } }

        /// <summary>
        /// Clarke 1880 (Benoit).
        /// </summary>
        public static Ellipsoid Clarke1880B { get { return _clarke1880B ?? (_clarke1880B = Ellipsoid.FromSemiMinorAxis("EPSG::7010", "Clarke 1880 (Benoit)", 6378300.789, 6356566.435)); } }

        /// <summary>
        /// Clarke 1880 (IGN).
        /// </summary>
        public static Ellipsoid Clarke1880IGN { get { return _clarke1880IGN ?? (_clarke1880IGN = Ellipsoid.FromSemiMinorAxis("EPSG::7011", "Clarke 1880 (IGN)", 6378249.2, 6356515)); } }

        /// <summary>
        /// Clarke 1880 (RGS).
        /// </summary>
        public static Ellipsoid Clarke1880RGS { get { return _clarke1880RGS ?? (_clarke1880RGS = Ellipsoid.FromSemiMinorAxis("EPSG::7012", "Clarke 1880 (RGS)", 6378249.145, 6356583.8)); } }

        /// <summary>
        /// Clarke 1880.
        /// </summary>
        public static Ellipsoid Clarke1880 { get { return _clarke1880 ?? (_clarke1880 = Ellipsoid.FromSemiMinorAxis("EPSG::7034", "Clarke 1880", Length.FromClarkesFoot(20926202), Length.FromClarkesFoot(20854895))); } }

        /// <summary>
        /// Clarke 1880 (Arc).
        /// </summary>
        public static Ellipsoid Clarke1880A { get { return _clarke1880A ?? (_clarke1880A = Ellipsoid.FromInverseFlattening("EPSG::7013", "Clarke 1880 (Arc)", 6378249.145, 293.4663077)); } }

        /// <summary>
        /// Clarke 1922.
        /// </summary>
        public static Ellipsoid Clarke1922 { get { return _clarke1922 ?? (_clarke1922 = Ellipsoid.FromInverseFlattening("EPSG::7014", "Clarke 1922", 6378249.2, 293.46598)); } }

        /// <summary>
        /// Danish 1876.
        /// </summary>
        public static Ellipsoid Danish1876 { get { return _danish1876 ?? (_danish1876 = Ellipsoid.FromInverseFlattening("EPSG::7051", "Danish 1876", 6377019.27, 300)); } }

        /// <summary>
        /// Everest 1830.
        /// </summary>
        public static Ellipsoid Everest1830 { get { return _everest1830 ?? (_everest1830 = Ellipsoid.FromSemiMinorAxis("EPSG::7042", "Everest 1830", Length.FromFoot(20922931.8), Length.FromFoot(20853374.58))); } }

        /// <summary>
        /// Everest 1937.
        /// </summary>
        public static Ellipsoid Everest1937 { get { return _everest1937 ?? (_everest1937 = Ellipsoid.FromInverseFlattening("EPSG::7015", "Everest 1937", 6377276.345, 300.8017)); } }

        /// <summary>
        /// Everest 1962.
        /// </summary>
        public static Ellipsoid Everest1962 { get { return _everest1962 ?? (_everest1962 = Ellipsoid.FromInverseFlattening("EPSG::7044", "Everest 1962 ", 6377301.243, 300.8017255)); } }

        /// <summary>
        /// Everest 1967.
        /// </summary>
        public static Ellipsoid Everest1967 { get { return _everest1967 ?? (_everest1967 = Ellipsoid.FromInverseFlattening("EPSG::7016", "Everest 1967", 6377298.556, 300.8017)); } }

        /// <summary>
        /// Everest 1969.
        /// </summary>
        public static Ellipsoid Everest1969 { get { return _everest1969 ?? (_everest1969 = Ellipsoid.FromInverseFlattening("EPSG::7056", "Everest 1969", 6377295.664, 300.8017)); } }

        /// <summary>
        /// Everest 1975.
        /// </summary>
        public static Ellipsoid Everest1975 { get { return _everest1975 ?? (_everest1975 = Ellipsoid.FromInverseFlattening("EPSG::7045", "Everest 1962", 6377299.151, 300.8017255)); } }

        /// <summary>
        /// Everest 1830 Modified.
        /// </summary>
        public static Ellipsoid Everest1830M { get { return _everest1830M ?? (_everest1830M = Ellipsoid.FromInverseFlattening("EPSG::7018", "Everest 1830 Modified", 6377304.063, 300.8017)); } }

        /// <summary>
        /// GEM 10C.
        /// </summary>
        public static Ellipsoid GEM10C { get { return _GEM10C ?? (_GEM10C = Ellipsoid.FromInverseFlattening("EPSG::7031", "GEM 10C", 6378137, 298.257223563)); } }

        /// <summary>
        /// GRS 1967.
        /// </summary>
        public static Ellipsoid GRS1967 { get { return _GRS1967 ?? (_GRS1967 = Ellipsoid.FromInverseFlattening("EPSG::7036", "GRS 1967", 6378160, 298.247167427)); } }

        /// <summary>
        /// GRS 1967.
        /// </summary>
        public static Ellipsoid GRS1967M { get { return _GRS1967M ?? (_GRS1967M = Ellipsoid.FromInverseFlattening("EPSG::7050", "GRS 1967 Modified", 6378160, 298.25)); } }

        /// <summary>
        /// GRS 1980.
        /// </summary>
        public static Ellipsoid GRS1980 { get { return _GRS1980 ?? (_GRS1980 = Ellipsoid.FromInverseFlattening("EPSG::7019", "GRS 1980", 6378137.0, 298.257222101)); } }

        /// <summary>
        /// GRS 1980 Authalic Sphere.
        /// </summary>
        public static Ellipsoid GRS1980AS { get { return _GRS1980AS ?? (_GRS1980AS = Ellipsoid.FromSphere("EPSG::7048", "GRS 1980 Authalic Sphere", 6371007)); } }

        /// <summary>
        /// Helmert 1906.
        /// </summary>
        public static Ellipsoid Helmert1906 { get { return _helmert1906 ?? (_helmert1906 = Ellipsoid.FromInverseFlattening("EPSG::7020", "Helmert 1906", 6378200, 298.3)); } }

        /// <summary>
        /// Hough 1960.
        /// </summary>
        public static Ellipsoid Hough1960 { get { return _hough1960 ?? (_hough1960 = Ellipsoid.FromInverseFlattening("EPSG::7053", "Hough 1960", 6378270.0, 297)); } }

        /// <summary>
        /// Hughes 1980.
        /// </summary>
        public static Ellipsoid Hughes1960 { get { return _hughes1960 ?? (_hughes1960 = Ellipsoid.FromSemiMinorAxis("EPSG::7058", "Hughes 1980", 6378273, 6356889.449)); } }

        /// <summary>
        /// IAG 1975.
        /// </summary>
        public static Ellipsoid IAG1975 { get { return _IAG1975 ?? (_IAG1975 = Ellipsoid.FromInverseFlattening("EPSG::7049", "IAG 1975", 6378140, 298.257)); } }

        /// <summary>
        /// Indonesian National Spheroid.
        /// </summary>
        public static Ellipsoid IndonesianNS { get { return _indonesianNS ?? (_indonesianNS = Ellipsoid.FromInverseFlattening("EPSG::7021", "Indonesian National Spheroid", 6378160, 298.247)); } }

        /// <summary>
        /// International 1924.
        /// </summary>
        public static Ellipsoid International1924 { get { return _international1924 ?? (_international1924 = Ellipsoid.FromInverseFlattening("EPSG::7022", "International 1924", 6378388, 297)); } }

        /// <summary>
        /// International 1924 Authalic Sphere.
        /// </summary>
        public static Ellipsoid International1924AS { get { return _international1924AS ?? (_international1924AS = Ellipsoid.FromSphere("EPSG::7057", "International 1924 Authalic Sphere", 6371228)); } }

        /// <summary>
        /// International 1967.
        /// </summary>
        public static Ellipsoid International1967 { get { return _international1967 ?? (_international1967 = Ellipsoid.FromSemiMinorAxis("EPSG::7023", "International 1967", 6378157.5, 6356772.2)); } }

        /// <summary>
        /// Krassowsky 1940.
        /// </summary>
        public static Ellipsoid Krassowsky1940 { get { return _krassowsky1940 ?? (_krassowsky1940 = Ellipsoid.FromInverseFlattening("EPSG::7024", "Krassowsky 1940", 6378245.0, 298.3)); } }

        /// <summary>
        /// NWL 9D.
        /// </summary>
        public static Ellipsoid NWL9D { get { return _NWL9D ?? (_NWL9D = Ellipsoid.FromInverseFlattening("EPSG::7025", "NWL 9D", 6378145, 298.25)); } }

        /// <summary>
        /// OSU86F.
        /// </summary>
        public static Ellipsoid OSU86F { get { return _OSU86F ?? (_OSU86F = Ellipsoid.FromInverseFlattening("EPSG::7032", "OSU86F", 6378136.2, 298.257223563)); } }

        /// <summary>
        /// OSU91A.
        /// </summary>
        public static Ellipsoid OSU91A { get { return _OSU91A ?? (_OSU91A = Ellipsoid.FromInverseFlattening("EPSG::7033", "OSU91A", 6378136.3, 298.257223563)); } }

        /// <summary>
        /// Plessis 1817.
        /// </summary>
        public static Ellipsoid Plessis1817 { get { return _plessis1817 ?? (_plessis1817 = Ellipsoid.FromSemiMinorAxis("EPSG::7027", "Plessis 1817", 6376523, 6355863)); } }

        /// <summary>
        /// PZ-90.
        /// </summary>
        public static Ellipsoid PZ90 { get { return _PZ90 ?? (_PZ90 = Ellipsoid.FromInverseFlattening("EPSG::7054", "PZ-90", 6378136, 298.257839303)); } }

        /// <summary>
        /// Struve 1860.
        /// </summary>
        public static Ellipsoid Struve1860 { get { return _struve1860 ?? (_struve1860 = Ellipsoid.FromInverseFlattening("EPSG::7028", "Struve 1860", 6378298.3, 294.73)); } }

        /// <summary>
        /// War Office.
        /// </summary>
        public static Ellipsoid WarOffice { get { return _warOffice ?? (_warOffice = Ellipsoid.FromInverseFlattening("EPSG::7029", "War Office", 6378300, 296)); } }

        /// <summary>
        /// WGS 1972.
        /// </summary>
        public static Ellipsoid WGS1972 { get { return _WGS1972 ?? (_WGS1972 = Ellipsoid.FromInverseFlattening("EPSG::7043", "WGS 1972", 6378135, 298.26)); } }

        /// <summary>
        /// WGS 1984.
        /// </summary>
        public static Ellipsoid WGS1984 { get { return _WGS1984 ?? (_WGS1984 = Ellipsoid.FromSemiMinorAxis("EPSG::7030", "WGS 1984", 6378137, 6356752.314)); } }

        #endregion
    }
}
