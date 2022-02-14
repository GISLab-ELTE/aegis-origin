/// <copyright file="ReferenceOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spatial
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for reference operations.
    /// </summary>
    [OperationParameterCollection]
    public static class ReferenceOperationParameters
    {
        #region Query fields

        private static OperationParameter[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationParameter" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationParameter" /> instances within the collection.</value>
        public static IList<OperationParameter> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(ReferenceOperationParameters).GetProperties().
                                                       Where(property => property.Name != "All").
                                                       Select(property => property.GetValue(null, null) as OperationParameter).
                                                       ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationParameter" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationParameter" /> instances that match the specified identifier.</returns>
        public static IList<OperationParameter> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }

        /// <summary>
        /// Returns all <see cref="OperationParameter" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationParameter" /> instances that match the specified name.</returns>
        public static IList<OperationParameter> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static fields

        private static OperationParameter _targetReferenceSystem;

        #endregion

        #region Public static methods

        /// <summary>
        /// Target reference system.
        /// </summary>
        public static OperationParameter TargetReferenceSystem
        {
            get
            {
                return _targetReferenceSystem ?? (_targetReferenceSystem =
                    OperationParameter.CreateRequiredParameter<IReferenceSystem>(
                        "AEGIS::305001", "Target reference system", 
                        "The reference system in which the geometry should be translated.", null
                    ));
            }
        }
        
        #endregion
    }
}
