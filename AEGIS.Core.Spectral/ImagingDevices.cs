/// <copyright file="ImagingDevices.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a collection of known <see cref="ImagingDevice"/> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(ImagingDevice))]
    public static class ImagingDevices
    {
        #region Query fields

        /// <summary>
        /// The array of all spectral imaging device instances within the collection.
        /// </summary>
        private static ImagingDevice[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="ImagingDevice"/> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="ImagingDevice"/> instances within the collection.</value>
        public static IList<ImagingDevice> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(ImagingDevices).GetProperties().
                                                          Where(property => property.Name != "All").
                                                          Select(property => property.GetValue(null, null) as ImagingDevice).
                                                          ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="ImagingDevice"/> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="ImagingDevice"/> instances that match the specified identifier.</returns>
        public static IList<ImagingDevice> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="ImagingDevice"/> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="ImagingDevice"/> instances that match the specified name.</returns>
        public static IList<ImagingDevice> FromName(String name)
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

        private static ImagingDevice _SPOT4HRVIR;
        private static ImagingDevice _SPOT5HRG;
        private static ImagingDevice _Landsat7ETMPLUS;
        private static ImagingDevice _Landsat8OLITIRS;

        #endregion

        #region Public static properties

        /// <summary>
        /// SPOT 4 HRVIR.
        /// </summary>
        public static ImagingDevice SPOT4HRVIR
        {
            get
            {
                if (_SPOT4HRVIR == null)
                    _SPOT4HRVIR = new ImagingDevice("AEGIS::135058", "SPOT", 4, "HRVIR",
                                                    "SPOT-4 is the fourth satellite in the SPOT series of CNES (Space Agency of France), placed into orbit by an Ariane launcher.", null, null,
                                                    Length.FromKilometre(822), TimeSpan.FromDays(26), Length.FromKilometre(60), 
                                                    new ImagingDeviceBand(0, "Pancromatic band (M)", Length.FromMetre(10), 8, SpectralDomain.Visible, new SpectralRange(0.61e-6, 0.68e-6)),
                                                    new ImagingDeviceBand(1, "Multispectral green band (XS1)", Length.FromMetre(20), 8, SpectralDomain.Green, new SpectralRange(0.5e-6, 0.59e-6)),
                                                    new ImagingDeviceBand(2, "Multispectral red band (XS2)", Length.FromMetre(20), 8, SpectralDomain.Red, new SpectralRange(0.61e-6, 0.68e-6)),
                                                    new ImagingDeviceBand(3, "Multispectral NIR band (XS3)", Length.FromMetre(20), 8, SpectralDomain.NearInfrared, new SpectralRange(0.79e-6, 0.89e-6)),
                                                    new ImagingDeviceBand(4, "Multispectral MIR band (SWIR)", Length.FromMetre(20), 8, SpectralDomain.ShortWavelengthInfrared, new SpectralRange(1.58e-6, 1.75e-6)));
                return _SPOT4HRVIR;
            }
        }

        /// <summary>
        /// SPOT 5 HRG.
        /// </summary>
        public static ImagingDevice SPOT5HRG
        {
            get
            {
                if (_SPOT5HRG == null)
                    _SPOT5HRG = new ImagingDevice("AEGIS::135060", "SPOT", 5, "HRG (High Resolution Geometric)",
                                                   "SPOT-5 is the fifth satellite in the SPOT series of CNES (Space Agency of France), placed into orbit by an Ariane launcher.", null, null,
                                                   Length.FromKilometre(832), TimeSpan.FromDays(26), Length.FromKilometre(60),
                                                   new ImagingDeviceBand(0, "Pancromatic band (M)", Length.FromMetre(5), 8, SpectralDomain.Visible, new SpectralRange(0.49e-6, 0.69e-6)),
                                                   new ImagingDeviceBand(1, "Multispectral green band (XS1)", Length.FromMetre(10), 8, SpectralDomain.Green, new SpectralRange(0.5e-6, 0.59e-6)),
                                                   new ImagingDeviceBand(2, "Multispectral red band (XS2)", Length.FromMetre(10), 8, SpectralDomain.Red, new SpectralRange(0.61e-6, 0.68e-6)),
                                                   new ImagingDeviceBand(3, "Multispectral NIR band (XS3)", Length.FromMetre(10), 8, SpectralDomain.NearInfrared, new SpectralRange(0.79e-6, 0.89e-6)),
                                                   new ImagingDeviceBand(4, "Multispectral MIR band (SWIR)", Length.FromMetre(20), 8, SpectralDomain.ShortWavelengthInfrared, new SpectralRange(1.58e-6, 1.75e-6)));
                return _SPOT5HRG;
            }
        }

        /// <summary>
        /// Landsat 7 ETM+.
        /// </summary>
        public static ImagingDevice Landsat7ETMPLUS
        {
            get
            {
                if (_Landsat7ETMPLUS == null)
                    _Landsat7ETMPLUS = new ImagingDevice("", "Landsat", 7, "ETM+ (Enhanced Thematic Mapper Plus (ETM+))",
                                                   "Landsat-7 is the seventh satellite in the Landsat series of U.S. Geological Survey (USGS) and NASA, placed into orbit by DELTA II. launcher.", null, null,
                                                   Length.FromKilometre(705), TimeSpan.FromDays(16), Length.FromKilometre(185),
                                                   new ImagingDeviceBand(0, "Multispectral blue band (BAND 1)", Length.FromMetre(30), 8, SpectralDomain.Blue, new SpectralRange(0.45e-6, 0.515e-6)),
                                                   new ImagingDeviceBand(1, "Multispectral green band (BAND 2)", Length.FromMetre(30), 8, SpectralDomain.Green, new SpectralRange(0.525e-6, 0.605e-6)),
                                                   new ImagingDeviceBand(2, "Multispectral red band (BAND 3)", Length.FromMetre(30), 8, SpectralDomain.Red, new SpectralRange(0.63e-6, 0.69e-6)),
                                                   new ImagingDeviceBand(3, "Multispectral NIR band (BAND 4)", Length.FromMetre(30), 8, SpectralDomain.NearInfrared, new SpectralRange(0.75e-6, 0.90e-6)),
                                                   new ImagingDeviceBand(4, "Multispectral SWIR band (BAND 5)", Length.FromMetre(30), 8, SpectralDomain.ShortWavelengthInfrared, new SpectralRange(1.55e-6, 1.75e-6)),
                                                   new ImagingDeviceBand(5, "Thermal IR band (BAND 6)", Length.FromMetre(60), 8, SpectralDomain.LongWavelengthInfrared, new SpectralRange(10.4e-6, 12.5e-6)),
                                                   new ImagingDeviceBand(6, "Multispectral SWIR band (BAND 7)", Length.FromMetre(30), 8, SpectralDomain.ShortWavelengthInfrared, new SpectralRange(2.09e-6, 2.35e-6)),
                                                   new ImagingDeviceBand(7, "Pancromatic band (BAND 8)", Length.FromMetre(15), 8, SpectralDomain.Visible, new SpectralRange(0.52e-6, 0.9e-6)));
                return _Landsat7ETMPLUS;
            }
        }

        /// <summary>
        /// Landsat 8 OLI/TIRS.
        /// </summary>
        public static ImagingDevice Landsat8OLITIRS
        {
            get
            {
                if (_Landsat8OLITIRS == null)
                    _Landsat8OLITIRS = new ImagingDevice("", "Landsat", 8, "OLI/TIRS (Operational Land Imager (OLI), Thermal Infrared Sensor (TIRS))",
                                                   "Landsat-8 is the eighth satellite in the Landsat series of U.S. Geological Survey (USGS) and NASA, placed into orbit by Atlas V-401 launcher.", null, null,
                                                   Length.FromKilometre(705), TimeSpan.FromDays(16), Length.FromKilometre(185),
                                                   new ImagingDeviceBand(0, "Multispectral violet-deep blue band, coastal aerosol (BAND 1)", Length.FromMetre(30), 16, SpectralDomain.Blue, new SpectralRange(0.43e-6, 0.45e-6)),
                                                   new ImagingDeviceBand(1, "Multispectral blue band (BAND 2)", Length.FromMetre(30), 16, SpectralDomain.Blue, new SpectralRange(0.45e-6, 0.51e-6)),
                                                   new ImagingDeviceBand(2, "Multispectral green band (BAND 3)", Length.FromMetre(30), 16, SpectralDomain.Green, new SpectralRange(0.53e-6, 0.59e-6)),
                                                   new ImagingDeviceBand(3, "Multispectral red band (BAND 4)", Length.FromMetre(30), 16, SpectralDomain.Red, new SpectralRange(0.64e-6, 0.67e-6)),
                                                   new ImagingDeviceBand(4, "Multispectral NIR band (BAND 5)", Length.FromMetre(30), 16, SpectralDomain.NearInfrared, new SpectralRange(0.85e-6, 0.88e-6)),
                                                   new ImagingDeviceBand(5, "Multispectral SWIR band (BAND 6)", Length.FromMetre(30), 16, SpectralDomain.ShortWavelengthInfrared, new SpectralRange(1.57e-6, 1.65e-6)),
                                                   new ImagingDeviceBand(6, "Multispectral SWIR band (BAND 7)", Length.FromMetre(30), 16, SpectralDomain.ShortWavelengthInfrared, new SpectralRange(2.11e-6, 2.29e-6)),
                                                   new ImagingDeviceBand(7, "Pancromatic band (BAND 8)", Length.FromMetre(15), 16, SpectralDomain.Visible, new SpectralRange(0.50e-6, 0.68e-6)),
                                                   new ImagingDeviceBand(8, "Multispectral band, cirrus clouds (BAND 9)", Length.FromMetre(30), 16, SpectralDomain.NearInfrared, new SpectralRange(1.36e-6, 1.38e-6)),
                                                   new ImagingDeviceBand(9, "Thermal IR band (BAND 10)", Length.FromMetre(30), 16, SpectralDomain.LongWavelengthInfrared, new SpectralRange(10.60e-6, 11.19e-6)),
                                                   new ImagingDeviceBand(10, "Thermal IR band (BAND 11)", Length.FromMetre(30), 16, SpectralDomain.LongWavelengthInfrared, new SpectralRange(11.50e-6, 12.51e-6)));
                return _Landsat8OLITIRS;
            }
        }

        

        #endregion
    }
}
