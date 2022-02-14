/// <copyright file="TestOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances.
    /// </summary>
    [OperationParameterCollection]
    public static class TestOperationParameters
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
                    _all = typeof(CommonOperationParameters).GetProperties().
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

        private static OperationParameter _testRequiredParameter;
        private static OperationParameter _testOptionalParameter;

        #endregion

        #region Public static properties

        /// <summary>
        /// Test parameter.
        /// </summary>
        public static OperationParameter TestRequiredParameter
        {
            get
            {
                return _testRequiredParameter ?? (_testRequiredParameter = OperationParameter.CreateRequiredParameter<Boolean>("AEGIS::000000", "Test parameter", String.Empty, null));
            }
        }

        /// <summary>
        /// Test parameter.
        /// </summary>
        public static OperationParameter TestOptionalParameter
        {
            get
            {
                return _testOptionalParameter ?? (_testOptionalParameter = OperationParameter.CreateOptionalParameter<Boolean>("AEGIS::000001", "Test parameter", String.Empty, null, true, new Predicate<Object>(value => (Boolean)value)));
            }
        }

        #endregion
    }
}
