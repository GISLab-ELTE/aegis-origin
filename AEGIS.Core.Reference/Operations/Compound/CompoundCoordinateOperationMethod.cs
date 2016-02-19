/// <copyright file="CompoundCoordinateOperationMethod.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
using System.Linq;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a compound coordinate operation method.
    /// </summary>
    public class CompoundCoordinateOperationMethod : CoordinateOperationMethod
    {
        #region Protected fields

        protected CoordinateOperationMethod[] _methods;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the submethods of the concatenated coordinate operation.
        /// </summary>
        /// <value>A read-only list of the submethods this operation.</value>
        public IList<CoordinateOperationMethod> Methods { get { return Array.AsReadOnly(_methods); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcatenatedCoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="isReversible">The is reversible.</param>
        /// <param name="methods">The submethods of the operation.</param>
        /// <exception cref="System.ArgumentNullException">No methods are specified.</exception>
        public CompoundCoordinateOperationMethod(String identifier, String name, Boolean isReversible, params CoordinateOperationMethod[] methods)
            : base(identifier, name, isReversible, null)
        {
            if (methods == null || methods.Length == 0)
                throw new ArgumentNullException("methods", "No submethods are specified.");

            _methods = methods;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcatenatedCoordinateOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="isReversible">The is reversible.</param>
        /// <param name="methods">The submethods of the operation.</param>
        /// <exception cref="System.ArgumentNullException">No methods are specified.</exception>
        public CompoundCoordinateOperationMethod(String identifier, String name, String remarks, String[] aliases, Boolean isReversible, params CoordinateOperationMethod[] methods)
            : base(identifier, name, remarks, aliases, isReversible, null)
        {
            if (methods == null || methods.Length == 0)
                throw new ArgumentNullException("methods", "No submethods are specified.");

            _methods = methods;
        }

        #endregion
    }
}
