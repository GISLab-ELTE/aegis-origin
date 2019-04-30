/// <copyright file="TestOperation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Tests.Operations
{
    /// <summary>
    /// Represents an operation used for testing.
    /// </summary>
    [OperationMethodImplementation("AEGIS::000000", "Test Operation Method")]
    public class TestOperation : IOperation<Object, Object>
    {
        #region IOperation properties

        /// <summary>
        /// Gets the method associated with the operation.
        /// </summary>
        /// <value>The associated operation method.</value>
        public OperationMethod Method
        {
            get { return TestOperationMethods.TestMethodWithoutParameter; }
        }

        /// <summary>
        /// Gets the parameters of the operation.
        /// </summary>
        /// <value>The parameters of the operation stored as key/value pairs.</value>
        public IDictionary<OperationParameter, Object> Parameters
        {
            get { return new Dictionary<OperationParameter, Object>(); }
        }

        /// <summary>
        /// Gets a value indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get { return false; } }

        /// <summary>
        /// Gets the operation state.
        /// </summary>
        /// <value>The current state of the operation.</value>
        public OperationState State { get { return OperationState.Initialized; } }

        /// <summary>
        /// Gets the source of the operation.
        /// </summary>
        /// <value>The source of the operation.</value>
        public Object Source { get { return null; } }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result of the operation.</value>
        public Object Result { get { return null; } }

        #endregion

        #region IOperation methods

        /// <summary>
        /// Executes the operation.
        /// </summary>
        public void Execute() { }

        /// <summary>
        /// Returns the reverse operation.
        /// </summary>
        /// <returns>The reverse operation.</returns>
        public IOperation<object, object> GetReverseOperation() { return null; }

        #endregion
    }
}
