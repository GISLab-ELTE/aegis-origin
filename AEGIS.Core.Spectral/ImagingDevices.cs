/// <copyright file="SpectralImagingDevices.cs" company="Eötvös Loránd University (ELTE)">
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

        private static ImagingDevice _SPOT5HRG1;

        #endregion

        #region Public static properties

        /// <summary>
        /// SPOT-5 HRG-1.
        /// </summary>
        public static ImagingDevice SPOT5HRG1
        {
            get
            {
                if (_SPOT5HRG1 == null)
                    _SPOT5HRG1 = new ImagingDevice("AEGIS::135065", "SPOT5", "HRG1", 
                                                   "SPOT-5 is the fifth satellite in the SPOT series of CNES (Space Agency of France), placed into orbit by an Ariane launcher.", 
                                                   Length.FromKilometre(832), TimeSpan.FromDays(26), 
                                                   new ImagingDeviceBand(1, "Pancromatic band", Length.FromMetre(10), Length.FromKilometre(60), 8, new SpectralRange(0.49, 0.69)),
                                                   new ImagingDeviceBand(2, "Multispectral green band", Length.FromMetre(5), Length.FromKilometre(60), 8, new SpectralRange(0.5, 0.59)),
                                                   new ImagingDeviceBand(3, "Multispectral red band", Length.FromMetre(10), Length.FromKilometre(60), 8, new SpectralRange(0.61, 0.68)),
                                                   new ImagingDeviceBand(4, "Multispectral NIR band", Length.FromMetre(10), Length.FromKilometre(60), 8, new SpectralRange(0.79, 0.89)),
                                                   new ImagingDeviceBand(5, "Multispectral MIR band", Length.FromMetre(20), Length.FromKilometre(60), 8, new SpectralRange(1.58, 1.75)));
                return _SPOT5HRG1;
            }
        }

        #endregion
    }
}
