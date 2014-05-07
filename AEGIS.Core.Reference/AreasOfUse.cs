/// <copyright file="AreasOfUse.cs" company="Eötvös Loránd University (ELTE)">
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
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of the known <see cref="AreaOfUse" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(AreaOfUse))]
    public static class AreasOfUse
    {
        #region Query fields

        private static AreaOfUse[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="AreaOfUse" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="AreaOfUse" /> instances within the collection.</value>
        public static IList<AreaOfUse> All
        {
            get
            {
                if (_all == null)
                {
                    List<AreaOfUse> areasOfUse = typeof(AreasOfUse).GetProperties().
                                                      Where(property => property.Name != "All").
                                                      Select(property => property.GetValue(null, null) as AreaOfUse).
                                                      ToList();

                    for (Double longitude = -180; longitude < 180; longitude += 6)
                    {
                        areasOfUse.Add(AreasOfUse.WorldZone(Angle.FromDegree(longitude), EllipsoidHemisphere.South));
                    }
                    for (Double longitude = -180; longitude < 180; longitude += 6)
                    {
                        areasOfUse.Add(AreasOfUse.WorldZone(Angle.FromDegree(longitude), EllipsoidHemisphere.North));
                    }
                    _all = areasOfUse.OrderBy(x => x.Name).ToArray();
                }
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="AreaOfUse" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="AreaOfUse" /> instances that match the specified identifier.</returns>
        public static IList<AreaOfUse> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="AreaOfUse" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="AreaOfUse" /> instances that match the specified name.</returns>
        public static IList<AreaOfUse> FromName(String name)
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

        private static AreaOfUse _algeriaN32;
        private static AreaOfUse _europeEVRF2000;
        private static AreaOfUse _europeETRS89;
        private static AreaOfUse _germanyThuringen;
        private static AreaOfUse _greatBritainMan;
        private static AreaOfUse _greeceOnshore;
        private static AreaOfUse _hungary;
        private static AreaOfUse _netherlandsOnshore;
        private static AreaOfUse _newZealandOnshoreNearShore;
        private static AreaOfUse _northAmericaNAD27;
        private static AreaOfUse _northAmericaNAD83;
        private static AreaOfUse _tunisia;
        private static AreaOfUse _trinidadAndTobago;
        private static AreaOfUse _USAAlabamaSPCSW;
        private static AreaOfUse _USACONUSAlaskaPRVI;
        private static AreaOfUse _USACONUSEastAlaskaOnshore;
        private static AreaOfUse _USACONUSOnshore;
        private static AreaOfUse _USATexasSPCS27SC;
        private static AreaOfUse _world;
        private static AreaOfUse _world_60NTo90N;
        private static AreaOfUse _world_60STo90S;
        private static AreaOfUse _world_80STo84N;
        private static AreaOfUse _worldByCountry;
        private static AreaOfUse _worldNorthHemisphere_0NTo84N;
        private static AreaOfUse _worldSouthHemisphere_0NTo80S;

        #endregion

        #region Public static properties

        /// <summary>
        /// Algeria - north of 32°N.
        /// </summary>
        public static AreaOfUse AlgeriaN32
        {
            get
            {
                return _algeriaN32 ?? (_algeriaN32 = 
                    AreaOfUse.FromDegrees("EPSG::1365", "Algeria - north of 32°N", "Algeria - onshore north of 32°N.", null,
                                          -2.94, 90.8, 37.14, 32));
            }
        }

        /// <summary>
        /// Europe - EVRF2000.
        /// </summary>
        public static AreaOfUse EuropeEVRF2000
        {
            get
            {
                 return _europeEVRF2000 ?? (_europeEVRF2000 = 
                     AreaOfUse.FromDegrees("EPSG::1299", "Europe - EVRF2000", "Europe - onshore - Andorra; Austria; Belgium; Bosnia and Herzegovina; Croatia; Czech Republic; Denmark; Estonia; Finland; France - mainland; Germany; Hungary; Italy - mainland and Sicily; Latvia; Liechtenstein; Lithuania; Luxembourg; Netherlands; Norway; Poland; Portugal; Romania; San Marino; Slovakia; Slovenia; Spain - mainland; Sweden; Switzerland; United Kingdom (UK) - Great Britain mainland; Vatican City State.", null,
                                           -9.55, 31.58, 71.2, 35.96));
            }
        }

        /// <summary>
        /// Europe - ETRS89.
        /// </summary>
        public static AreaOfUse EuropeETRS89
        {
            get
            {
                return _europeETRS89 ?? (_europeETRS89 = 
                    AreaOfUse.FromDegrees("EPSG::1298", "Europe - ETRS89", "Europe - onshore and offshore: Albania; Andorra; Austria; Belgium; Bosnia and Herzegovina; Bulgaria; Croatia; Cyprus; Czech Republic; Denmark; Estonia; Faroe Islands; Finland; France; Germany; Gibraltar; Greece; Hungary; Ireland; Italy; Latvia; Liechtenstein; Lithuania; Luxembourg; Macedonia; Malta; Monaco; Montenegro; Netherlands; Norway including Svalbard and Jan Mayen; Poland; Portugal; Romania; San Marino; Serbia; Slovakia; Slovenia; Spain; Sweden; Switzerland; United Kingdom (UK) including Channel Islands and Isle of Man; Vatican City State.", null,
                                          -16.1, 39.64, 84.16, 32.88));
            }
        }

        /// <summary>
        /// Germany - Thuringen.
        /// </summary>
        public static AreaOfUse GermanyThuringen
        {
            get
            {
                return _germanyThuringen ?? (_germanyThuringen = 
                    AreaOfUse.FromDegrees("EPSG::2544", "Germany - Thuringen", "Germany - Thuringen", null,
                                          9.92, 12.56, 51.64, 50.2));
            }
        }

        /// <summary>
        /// Greece - onshore.
        /// </summary>
        public static AreaOfUse GreeceOnshore
        {
            get
            {
                return _greeceOnshore ?? (_greeceOnshore = 
                    AreaOfUse.FromDegrees("EPSG::3254", "Greece - onshore", "Greece - onshore", null,
                                          19.58, 28.3, 41.75, 34.88));
            }
        }

        /// <summary>
        /// UK - Great Britain; Isle of Man.
        /// </summary>
        public static AreaOfUse GreatBritainMan
        {
            get
            {
                return _greatBritainMan ?? (_greatBritainMan = 
                    AreaOfUse.FromDegrees("EPSG::1264", "UK - Great Britain; Isle of Man", "United Kingdom (UK) - Great Britain - England and Wales onshore, Scotland onshore and Western Isles nearshore; Isle of Man onshore.", null,
                                          -8.73, 1.83, 60.89, 49.81));
            }
        }

        /// <summary>
        /// Hungary.
        /// </summary>
        public static AreaOfUse Hungary
        {
            get
            {
                return _hungary ?? (_hungary = 
                    AreaOfUse.FromDegrees("EPSG::1119", "Hungary", "Hungary", null, 
                                          16.11, 22.89, 48.58, 45.75));
            }
        }

        /// <summary>
        /// Netherlands - onshore.
        /// </summary>
        public static AreaOfUse NetherlandsOnshore
        {
            get
            {
                return _netherlandsOnshore ?? (_netherlandsOnshore = 
                    AreaOfUse.FromDegrees("EPSG::1275", "Netherlands - onshore", "Netherlands - onshore, including Waddenzee, Dutch Wadden Islands and 12-mile offshore coastal zone.", null,
                                          3.21, 7.21, 53.69, 50.75));
            }
        }

        /// <summary>
        /// New Zealand - onshore and nearshore.
        /// </summary>
        public static AreaOfUse NewZealandOnshoreNearShore
        {
            get
            {
                return _newZealandOnshoreNearShore ?? (_newZealandOnshoreNearShore = 
                    AreaOfUse.FromDegrees("EPSG::3285 ", "New Zealand - onshore and nearshore", "New Zealand - North Island, South Island, Stewart Island - onshore and nearshore.", null,
                                          165.87, 179.26, -33.9, -47.64));
            }
        }

        /// <summary>
        /// North America - NAD27.
        /// </summary>
        public static AreaOfUse NorthAmericaNAD27
        {
            get
            {
                return _northAmericaNAD27 ?? (_northAmericaNAD27 = 
                    AreaOfUse.FromDegrees("EPSG::1349", "North America - NAD27", "North and central America: Antigua and Barbuda. Bahamas. Belize. British Virgin Islands. Canada - Alberta; British Columbia; Manitoba; New Brunswick; Newfoundland and Labrador; Northwest Territories; Nova Scotia; Nunavut; Ontario; Prince Edward Island; Quebec; Saskatchewan; Yukon. Cuba. El Salvador. Guatemala. Honduras. Puerto Rico. Mexico. Nicaragua. United States (USA) - Alabama; Alaska; Arizona; Arkansas; California; Colorado; Connecticut; Delaware; Florida; Georgia; Idaho; Illinois; Indiana; Iowa; Kansas; Kentucky; Louisiana; Maine; Maryland; Massachusetts; Michigan; Minnesota; Mississippi; Missouri; Montana; Nebraska; Nevada; New Hampshire; New Jersey; New Mexico; New York; North Carolina; North Dakota; Ohio; Oklahoma; Oregon; Pennsylvania; Rhode Island; South Carolina; South Dakota; Tennessee; Texas; Utah; Vermont; Virginia; Washington; West Virginia; Wisconsin; Wyoming. US Virgin Islands. Usage shall be onshore only except that onshore and offshore shall apply to Canada east coast (New Brunswick; Newfoundland and Labrador; Prince Edward Island; Quebec). Cuba. Mexico (Gulf of Mexico and Caribbean coasts only). USA Alaska. USA Gulf of Mexico (Alabama; Florida; Louisiana; Mississippi; Texas). USA East Coast. Bahamas onshore plus offshore over internal continental shelf only.", "Area crosses 180-degree meridian.",
                                          167.65, -47.74, 83.16, 7.98));
            }
        }

        /// <summary>
        /// North America - NAD83.
        /// </summary>
        public static AreaOfUse NorthAmericaNAD83
        {
            get
            {
                return _northAmericaNAD83 ?? (_northAmericaNAD83 = 
                    AreaOfUse.FromDegrees("EPSG::1350", "North America - NAD83", "North America - onshore and offshore: Canada - Alberta; British Columbia; Manitoba; New Brunswick; Newfoundland and Labrador; Northwest Territories; Nova Scotia; Nunavut; Ontario; Prince Edward Island; Quebec; Saskatchewan; Yukon. Puerto Rico. United States (USA) - Alabama; Alaska; Arizona; Arkansas; California; Colorado; Connecticut; Delaware; Florida; Georgia; Hawaii; Idaho; Illinois; Indiana; Iowa; Kansas; Kentucky; Louisiana; Maine; Maryland; Massachusetts; Michigan; Minnesota; Mississippi; Missouri; Montana; Nebraska; Nevada; New Hampshire; New Jersey; New Mexico; New York; North Carolina; North Dakota; Ohio; Oklahoma; Oregon; Pennsylvania; Rhode Island; South Carolina; South Dakota; Tennessee; Texas; Utah; Vermont; Virginia; Washington; West Virginia; Wisconsin; Wyoming. US Virgin Islands. British Virgin Islands.", "Area crosses 180-degree meridian.",
                                          167.65, -47.74, 86.45, 14.93));
            }
        }

        /// <summary>
        /// Trinidad and Tobago - Trinidad.
        /// </summary>
        public static AreaOfUse TrinidadAndTobago
        {
            get
            {
                return _trinidadAndTobago ?? (_trinidadAndTobago = 
                    AreaOfUse.FromDegrees("EPSG::1339", "Trinidad and Tobago - Trinidad", "Trinidad and Tobago - Trinidad - onshore and offshore.", null,
                                          -62.08, -60, 11.5, 9.83));
            }
        }

        /// <summary>
        /// Tunisia.
        /// </summary>
        public static AreaOfUse Tunisia
        {
            get
            {
                return _tunisia ?? (_tunisia = 
                    AreaOfUse.FromDegrees("EPSG::1236", "Tunisia", "Tunisia - onshore and offshore.", null, 
                                          7.49, 13.66, 38.4, 30.23));
            }
        }

        /// <summary>
        /// USA - Alabama - SPCS - W.
        /// </summary>
        public static AreaOfUse USAAlabamaSPCSW
        {
            get
            {
                return _USAAlabamaSPCSW ?? (_USAAlabamaSPCSW =
                    AreaOfUse.FromDegrees("EPSG::2155", "USA - Alabama - SPCS - W", "United States (USA) - Alabama west of approximately 86°37'W - counties Autauga; Baldwin; Bibb; Blount; Butler; Chilton; Choctaw; Clarke; Colbert; Conecuh; Cullman; Dallas; Escambia; Fayette; Franklin; Greene; Hale; Jefferson; Lamar; Lauderdale; Lawrence; Limestone; Lowndes; Marengo; Marion; Mobile; Monroe; Morgan; Perry; Pickens; Shelby; Sumter; Tuscaloosa; Walker; Washington; Wilcox; Winston.", null,
                                          -88.47, -86.3, 35.02, 30.14));
            }
        }

        /// <summary>
        /// USA - CONUS and Alaska; PRVI.
        /// </summary>
        public static AreaOfUse USACONUSAlaskaPRVI
        {
            get
            {
                return _USACONUSAlaskaPRVI ?? (_USACONUSAlaskaPRVI = 
                    AreaOfUse.FromDegrees("EPSG::1511", "USA - CONUS and Alaska; PRVI", "Puerto Rico - onshore and offshore. United States (USA) onshore and offshore - Alabama; Alaska; Arizona; Arkansas; California; Colorado; Connecticut; Delaware; Florida; Georgia; Idaho; Illinois; Indiana; Iowa; Kansas; Kentucky; Louisiana; Maine; Maryland; Massachusetts; Michigan; Minnesota; Mississippi; Missouri; Montana; Nebraska; Nevada; New Hampshire; New Jersey; New Mexico; New York; North Carolina; North Dakota; Ohio; Oklahoma; Oregon; Pennsylvania; Rhode Island; South Carolina; South Dakota; Tennessee; Texas; Utah; Vermont; Virginia; Washington; West Virginia; Wisconsin; Wyoming. US Virgin Islands - onshore and offshore.", "Area crosses 180-degree meridian.",
                                          167.65, -63.89, 74.71, 14.93));
            }
        }

        /// <summary>
        /// USA - CONUS and east Alaska - onshore.
        /// </summary>
        public static AreaOfUse USACONUSEastAlaskaOnshore
        {
            get
            {
                return _USACONUSEastAlaskaOnshore ?? (_USACONUSEastAlaskaOnshore = 
                    AreaOfUse.FromDegrees("EPSG::3664", "USA - CONUS and east Alaska - onshore", "United States (USA) - CONUS and east Alaska - onshore - Alabama; Alaska east of 152ºW; Arizona; Arkansas; California; Colorado; Connecticut; Delaware; Florida; Georgia; Idaho; Illinois; Indiana; Iowa; Kansas; Kentucky; Louisiana; Maine; Maryland; Massachusetts; Michigan; Minnesota; Mississippi; Missouri; Montana; Nebraska; Nevada; New Hampshire; New Jersey; New Mexico; New York; North Carolina; North Dakota; Ohio; Oklahoma; Oregon; Pennsylvania; Rhode Island; South Carolina; South Dakota; Tennessee; Texas; Utah; Vermont; Virginia; Washington; West Virginia; Wisconsin; Wyoming.", null,
                                          -152, -66.92, 70.62, 24.41));
            }
        }

        /// <summary>
        /// USA - CONUS - onshore.
        /// </summary>
        public static AreaOfUse USACONUSOnshore
        {
            get
            {
                return _USACONUSOnshore ?? (_USACONUSOnshore = 
                    AreaOfUse.FromDegrees("EPSG::1323", "USA - CONUS - onshore", "United States (USA) - onshore - Alabama; Arizona; Arkansas; California; Colorado; Connecticut; Delaware; Florida; Georgia; Idaho; Illinois; Indiana; Iowa; Kansas; Kentucky; Louisiana; Maine; Maryland; Massachusetts; Michigan; Minnesota; Mississippi; Missouri; Montana; Nebraska; Nevada; New Hampshire; New Jersey; New Mexico; New York; North Carolina; North Dakota; Ohio; Oklahoma; Oregon; Pennsylvania; Rhode Island; South Carolina; South Dakota; Tennessee; Texas; Utah; Vermont; Virginia; Washington; West Virginia; Wisconsin; Wyoming.", null,
                                          -124.79, -66.92, 49.38, 24.41));
            }
        }

        /// <summary>
        /// USA - Texas - SPCS27 - SC.
        /// </summary>
        public static AreaOfUse USATexasSPCS27SC
        {
            get
            {
                return _USATexasSPCS27SC ?? (_USATexasSPCS27SC = 
                    AreaOfUse.FromDegrees("EPSG::2256", "USA - Texas - SPCS27 - SC", "United States (USA) - Texas - counties of Aransas; Atascosa; Austin; Bandera; Bee; Bexar; Brazoria; Brewster; Caldwell; Calhoun; Chambers; Colorado; Comal; De Witt; Dimmit; Edwards; Fayette; Fort Bend; Frio; Galveston; Goliad; Gonzales; Guadalupe; Harris; Hays; Jackson; Jefferson; Karnes; Kendall; Kerr; Kinney; La Salle; Lavaca; Live Oak; Matagorda; Maverick; McMullen; Medina; Presidio; Real; Refugio; Terrell; Uvalde; Val Verde; Victoria; Waller; Wharton; Wilson; Zavala. Gulf of Mexico outer continental shelf (GoM OCS) protraction areas: Matagorda Island; Brazos; Galveston; High Island, Sabine Pass (TX).", null,
                                          9.92, 12.56, 51.64, 50.2));
            }
        }

        /// <summary>
        /// World.
        /// </summary>
        public static AreaOfUse World
        {
            get
            {
                return _world ?? (_world = 
                    AreaOfUse.FromDegrees("EPSG::1262", "World", "World", null, 
                                          -180, 180, 90, -90));
            }
        }     
   
        /// <summary>
        /// World - N hemisphere - north of 60°N.
        /// </summary>
        public static AreaOfUse World_60NTo90N
        {
            get
            {
                return _world_60NTo90N ?? (_world_60NTo90N = 
                    AreaOfUse.FromDegrees("EPSG::1996", "World - N hemisphere - north of 60°N", "Northern hemisphere - north of 60°N onshore and offshore, including Arctic.", null,
                                          -180, 180, 90, 60));
            }
        }

        /// <summary>
        /// World - S hemisphere - south of 60°S.
        /// </summary>
        public static AreaOfUse World_60STo90S
        {
            get
            {
                return _world_60STo90S ?? (_world_60STo90S = 
                    AreaOfUse.FromDegrees("EPSG::1997", "World - S hemisphere - south of 60°S", "Southern hemisphere - south of 60°S onshore and offshore - Antarctica.", null, 
                                          -180, 180, -60, -90));
            }
        }

        /// <summary>
        /// World - between 80°S and 84°N.
        /// </summary>
        public static AreaOfUse World_80STo84N
        {
            get
            {
                return _world_80STo84N ?? (_world_80STo84N = 
                    AreaOfUse.FromDegrees("EPSG::3391", "World - between 80°S and 84°N", "World between 80°S and 84°N.", null, 
                                          -180, 180, 84, -80));
            }
        }  
      
        /// <summary>
        /// World (by country).
        /// </summary>
        public static AreaOfUse WorldByCountry
        {
            get
            {
                return _worldByCountry ?? (_worldByCountry =
                    AreaOfUse.FromDegrees("EPSG::2830", "World", "World: Afghanistan, Albania, Algeria, American Samoa, Andorra, Angola, Anguilla, Antarctica, Antigua and Barbuda, Argentina, Armenia, Aruba, Australia, Austria, Azerbaijan, Bahamas, Bahrain, Bangladesh, Barbados, Belgium, Belgium, Belize, Benin, Bermuda, Bhutan, Bolivia, Bonaire, Saint Eustasius and Saba, Bosnia and Herzegovina, Botswana, Bouvet Island, Brazil, British Indian Ocean Territory, British Virgin Islands, Brunei Darussalam, Bulgaria, Burkina Faso, Burundi, Cambodia, Cameroon, Canada, Cape Verde, Cayman Islands, Central African Republic, Chad, Chile, China, Christmas Island, Cocos (Keeling) Islands, Comoros, Congo, Cook Islands, Costa Rica, Côte d'Ivoire (Ivory Coast), Croatia, Cuba, Curacao, Cyprus, Czech Republic, Denmark, Djibouti, Dominica, Dominican Republic, East Timor, Ecuador, Egypt, El Salvador, Equatorial Guinea, Eritrea, Estonia, Ethiopia, Falkland Islands (Malvinas), Faroe Islands, Fiji, Finland, France, French Guiana, French Polynesia, French Southern Territories, Gabon, Gambia, Georgia, Germany, Ghana, Gibraltar, Greece, Greenland, Grenada, Guadeloupe, Guam, Guatemala, Guinea, Guinea-Bissau, Guyana, Haiti, Heard Island and McDonald Islands, Holy See (Vatican City State), Honduras, China - Hong Kong, Hungary, Iceland, India, Indonesia, Islamic Republic of Iran, Iraq, Ireland, Israel, Italy, Jamaica, Japan, Jordan, Kazakhstan, Kenya, Kiribati, Democratic People's Republic of Korea (North Korea), Republic of Korea (South Korea), Kuwait, Kyrgyzstan, Lao People's Democratic Republic (Laos), Latvia, Lebanon, Lesotho, Liberia, Libyan Arab Jamahiriya, Liechtenstein, Lithuania, Luxembourg, China - Macao, The Former Yugoslav Republic of Macedonia, Madagascar, Malawi, Malaysia, Maldives, Mali, Malta, Marshall Islands, Martinique, Mauritania, Mauritius, Mayotte, Mexico, Federated States of Micronesia, Monaco, Mongolia, Montenegro, Montserrat, Morocco, Mozambique, Myanmar (Burma), Namibia, Nauru, Nepal, Netherlands, New Caledonia, New Zealand, Nicaragua, Niger, Nigeria, Niue, Norfolk Island, Northern Mariana Islands, Norway, Oman, Pakistan, Palau, Panama, Papua New Guinea (PNG), Paraguay, Peru, Philippines, Pitcairn, Poland, Portugal, Puerto Rico, Qatar, Reunion, Romania, Russian Federation, Rwanda, Saint Kitts and Nevis, Saint Helena, Ascension and Tristan da Cunha, Saint Lucia, Saint Pierre and Miquelon, Saint Vincent and the Grenadines, Samoa, San Marino, Sao Tome and Principe, Saudi Arabia, Senegal, Serbia, Seychelles, Sierra Leone, Singapore, Slovakia (Slovak Republic), Slovenia, Sint Maarten, Solomon Islands, Somalia, South Africa, South Georgia and the South Sandwich Islands, South Sudan, Spain, Sri Lanka, Sudan, Suriname, Svalbard and Jan Mayen, Swaziland, Sweden, Switzerland, Syrian Arab Republic, Taiwan, Tajikistan, United Republic of Tanzania, Thailand, The Democratic Republic of the Congo (Zaire), Togo, Tokelau, Tonga, Trinidad and Tobago, Tunisia, Turkey, Turkmenistan, Turks and Caicos Islands, Tuvalu, Uganda, Ukraine, United Arab Emirates (UAE), United Kingdom (UK), United States (USA), United States Minor Outlying Islands, Uruguay, Uzbekistan, Vanuatu, Venezuela, Vietnam, US Virgin Islands, Wallis and Futuna, Western Sahara, Yemen, Zambia, Zimbabwe.", null,
                                          -180, 180, 90, -90));
            }
        }

        /// <summary>
        /// World - N hemisphere - 0°N to 84°N.
        /// </summary>
        public static AreaOfUse WorldNorthHemisphere_0NTo84N
        {
            get
            {
                return _worldNorthHemisphere_0NTo84N ?? (_worldNorthHemisphere_0NTo84N =
                    AreaOfUse.FromDegrees("EPSG::1998", "World - N hemisphere - 0°N to 84°N", "Northern hemisphere between equator and 84°N, onshore and offshore.", null,
                                          -180, 180, 84, 0));
            }
        }

        /// <summary>
        /// World - S hemisphere - 0°N to 80°S.
        /// </summary>
        public static AreaOfUse WorlSouthHemisphere_0NTo80S
        {
            get
            {
                return _worldSouthHemisphere_0NTo80S ?? (_worldSouthHemisphere_0NTo80S =
                    AreaOfUse.FromDegrees("EPSG::1999", "World - S hemisphere - 0°N to 80°S", "Southern hemisphere between equator and 80°S, onshore and offshore.", null,
                                          -180, 180, 0, -80));
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Retrieves a world zone.
        /// </summary>
        /// <param name="zoneNumber">The zone number.</param>
        /// <param name="hemisphere">The ellpisoid hemisphere.</param>
        /// <returns>The world zone for the specified number and hemisphere</returns>
        /// <exception cref="System.ArgumentException">
        /// The zone number is not valid.;zoneNumber
        /// or
        /// The hemisphere is not south or north.hemisphere
        /// </exception>
        public static AreaOfUse WorldZone(Int32 zoneNumber, EllipsoidHemisphere hemisphere)
        {
            if (zoneNumber < 1 || zoneNumber > 60)
                throw new ArgumentException("The zone number is not valid.", "zoneNumber");
            if (hemisphere == EllipsoidHemisphere.Equador)
                throw new ArgumentException("The hemisphere is not south or north.", "hemisphere");

            Double zoneWidth = Constants.PI / 30;
            Double zoneCenter = (zoneNumber - 1) * zoneWidth + zoneWidth / 2;

            Double zoneWest = Math.Round(zoneCenter * Constants.RadianToDegree) - 3;
            Double zoneEast = Math.Round(zoneCenter * Constants.RadianToDegree) + 3;
            Double zoneNorth = (hemisphere == EllipsoidHemisphere.North ? 84 : 0);
            Double zoneSouth = (hemisphere == EllipsoidHemisphere.North ? 0 : -80);

            String identifier = "EPSG::" + (1872 + (hemisphere == EllipsoidHemisphere.North ? zoneNumber * 2 - 1 : zoneNumber * 2));
            String name = "World - " + (hemisphere == EllipsoidHemisphere.North ? "N" : "S") +
                          " hemisphere - " + Math.Abs(zoneWest) +
                          "°" + (zoneWest < 0 ? "W" : "E") + " to " +
                          Math.Abs(zoneEast) +
                          "°" + (zoneEast < 0 ? "W" : "E");

            return new AreaOfUse(identifier, name, Angle.FromDegree(zoneWest), Angle.FromDegree(zoneEast), Angle.FromDegree(zoneNorth), Angle.FromDegree(zoneSouth));
        }
        /// <summary>
        /// Retrieves a world zone.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="hemisphere">The ellpisoid hemisphere.</param>
        /// <returns>The world zone for the specified number and hemisphere</returns>
        /// <exception cref="System.ArgumentException">
        /// The longitude is not valid.;longitude
        /// or
        /// The hemisphere is not south or north.hemisphere
        /// </exception>
        public static AreaOfUse WorldZone(Angle longitude, EllipsoidHemisphere hemisphere)
        {
            if (longitude.BaseValue < -Constants.PI || longitude.BaseValue > Constants.PI)
                throw new ArgumentException("The longitude is not valid.", "longitude");
            if (hemisphere == EllipsoidHemisphere.Equador)
                throw new ArgumentException("The menisphere must be north or south.", "hemisphere");

            Double zoneWidth = Constants.PI / 30;

            Double zoneCenter = Math.Floor(Math.Round(longitude.BaseValue / zoneWidth, 4)) * zoneWidth + zoneWidth / 2;
            Int32 zoneNumber = Convert.ToInt32(Math.Floor(Math.Round((longitude.BaseValue + Constants.PI) / zoneWidth, 4))) + 1;

            Double zoneWest = Math.Round(zoneCenter * Constants.RadianToDegree) - 3;
            Double zoneEast = Math.Round(zoneCenter * Constants.RadianToDegree) + 3;
            Double zoneNorth = (hemisphere == EllipsoidHemisphere.North ? 84 : 0);
            Double zoneSouth = (hemisphere == EllipsoidHemisphere.North ? 0 : -80);

            String identifier = "EPSG::" + (1872 + (hemisphere == EllipsoidHemisphere.North ? zoneNumber * 2 - 1 : zoneNumber * 2));
            String name = "World - " + (hemisphere == EllipsoidHemisphere.North ? "N" : "S") + 
                          " hemisphere - " + Math.Abs(zoneWest) + 
                          "°" + (zoneWest < 0 ? "W" : "E") + " to " +  
                          Math.Abs(zoneEast) +
                          "°" + (zoneEast < 0 ? "W" : "E");

            return new AreaOfUse(identifier, name, Angle.FromDegree(zoneWest), Angle.FromDegree(zoneEast), Angle.FromDegree(zoneNorth), Angle.FromDegree(zoneSouth));
        }

        #endregion        
    }
}
