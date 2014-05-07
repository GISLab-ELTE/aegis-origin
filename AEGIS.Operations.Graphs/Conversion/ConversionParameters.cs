/// <copyright file="ConversionParameters.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations.Conversion
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for conversion operations.
    /// </summary>
    [IdentifiedObjectCollection(typeof(OperationParameter))]
    public static class ConversionParameters
    {
        #region Query fields

        private static OperationMethod[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationMethod" /> instances in the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationMethod" /> instances in the collection.</value>
        public static IList<OperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(ConversionParameters).GetProperties().
                                                        Where(property => property.Name != "All").
                                                        Select(property => property.GetValue(null, null) as OperationMethod).
                                                        ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified identifier.</returns>
        public static IList<OperationMethod> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="OperationMethod" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationMethod" /> instances that match the specified name.</returns>
        public static IList<OperationMethod> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static fields

        private static OperationParameter _bidirectionalConversion;
        private static OperationParameter _geometryDimesion;

        #endregion

        #region Public static properties

        /// <summary>
        /// Bidirectional conversion.
        /// </summary>
        public static OperationParameter BidirectionalConversion
        {
            get
            {
                return _bidirectionalConversion ?? (_bidirectionalConversion =
                    OperationParameter.CreateOptionalParameter<Boolean>(
                        "AEGIS::222101", "Bidirectional conversion",
                        "A value indication whether the conversion should keep the line directions of the original geometry, or should the lines be directed in both ways.", null,
                        true
                    ));
            }
        }

        /// <summary>
        /// Geometry dimension.
        /// </summary>
        public static OperationParameter GeometryDimension
        {
            get
            {
                return _geometryDimesion ?? (_geometryDimesion =
                    OperationParameter.CreateOptionalParameter<Int32>(
                        "222160", "Geometry dimension", 
                        "A value indicating the dimension limit of the resulting geometry. The value must be between 0 and 2.", null, 
                        2,
                        Conditions.IsBetween(0, 2)
                    ));
            }
        }

        #endregion
    }
}
