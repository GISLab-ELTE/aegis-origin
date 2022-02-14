/// <copyright file="CoordinateOperationMethod.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a coordinate operation method.
    /// </summary>
    public class CoordinateOperationMethod : IdentifiedObject
    {
        #region Private fields

        private readonly CoordinateOperationParameter[] _parameters;
        private readonly Boolean _isReversible;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a values indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get { return _isReversible; } }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The array containing the parameters of the method.</value>
        public IList<CoordinateOperationParameter> Parameters { get { return _parameters == null ? null : Array.AsReadOnly(_parameters); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="isReversible">Indicates whether the operation is reversible.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationMethod(String identifier, String name, Boolean isReversible, params CoordinateOperationParameter[] parameters)
            : this(identifier, name, null, null, isReversible, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="isReversible">Indicates whether the operation is reversible.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public CoordinateOperationMethod(String identifier, String name, String remarks, String[] aliases, Boolean isReversible, params CoordinateOperationParameter[] parameters)
            : base(identifier, name, remarks, aliases)
        {
            _isReversible = isReversible;
            _parameters = parameters;
        }

        #endregion
    }
}
