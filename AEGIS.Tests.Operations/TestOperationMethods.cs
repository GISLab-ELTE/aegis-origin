/// <copyright file="TestOperationMethods.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Represents a collection of known <see cref="OperationMethod" /> instances.
    /// </summary>
    [OperationMethodCollection]
    public static class TestOperationMethods
    {
        #region Query fields

        private static OperationMethod[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationMethod" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationMethod" /> instances within the collection.</value>
        public static IList<OperationMethod> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(TestOperationMethods).GetProperties().
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

        #region Private static instances

        private static OperationMethod _testMethodWithoutParameter;
        private static OperationMethod _testMethodWithParameter;

        #endregion

        #region Public static properties

        /// <summary>
        /// Test Operation Method.
        /// </summary>
        public static OperationMethod TestMethodWithoutParameter
        {
            get
            {
                return _testMethodWithoutParameter ?? (_testMethodWithoutParameter =
                    OperationMethod.CreateMethod<Object, Object>("AEGIS::000000", "Test Operation Method",
                                                                 String.Empty, null, "1.0.0",
                                                                 false, GeometryModel.None, ExecutionMode.InPlace));
            }
        }

        /// <summary>
        /// Test Operation Method.
        /// </summary>
        public static OperationMethod TestMethodWithParameter
        {
            get
            {
                return _testMethodWithParameter ?? (_testMethodWithParameter =
                    OperationMethod.CreateMethod<Object, Object>("AEGIS::000001", "Test Operation Method",
                                                                 String.Empty, null, "1.0.0",
                                                                 false, GeometryModel.None, ExecutionMode.InPlace,
                                                                 TestOperationParameters.TestRequiredParameter, 
                                                                 TestOperationParameters.TestOptionalParameter));
            }
        }

        #endregion
    }
}
