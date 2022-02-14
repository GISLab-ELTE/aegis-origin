/// <copyright file="OperationMethodImplementationAttribute.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;

namespace ELTE.AEGIS.Operations.Management
{
    /// <summary>
    /// Indicates that the specified type implements an operation method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OperationMethodImplementationAttribute : IdentifiedObjectInstanceAttribute
    {
        #region Private fields

        /// <summary>
        /// The version of the operation.
        /// </summary>
        private readonly Version _version;

        /// <summary>
        /// The type of the credential.
        /// </summary>
        private readonly Type _credentialType;

        /// <summary>
        /// The credential initialized from the type.
        /// </summary>
        private OperationCredential _credential;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the version of the operation.
        /// </summary>
        /// <value>The version of the operation.</value>
        public Version Version { get { return _version; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMethodImplementationAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation method.</param>
        /// <param name="name">The name of the operation method.</param>
        /// <param name="version">The version of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// </exception>
        public OperationMethodImplementationAttribute(String identifier, String name)
            : base(identifier, name)
        {
            _version = Version.Default;
            _credentialType = typeof(OperationCredential);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMethodImplementationAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation method.</param>
        /// <param name="name">The name of the operation method.</param>
        /// <param name="version">The version of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// or
        /// The version is null.
        /// </exception>
        public OperationMethodImplementationAttribute(String identifier, String name, String version)
            : base(identifier, name)
        {
            _version = Version.Parse(version);
            _credentialType = typeof(OperationCredential);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMethodImplementationAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier of the operation method.</param>
        /// <param name="name">The name of the operation method.</param>
        /// <param name="version">The version of the operation.</param>
        /// <param name="credentialType">The type of the operation credential.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// or
        /// The version is null.
        /// or
        /// The credential type is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified type is not an operation credential.
        /// or
        /// The version has no components or more than three components.
        /// </exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public OperationMethodImplementationAttribute(String identifier, String name, String version, Type credentialType)
            : base(identifier, name)
        {
            if (credentialType == null)
                throw new ArgumentNullException("credentialType", "The credential type is null.");

            if (!credentialType.Equals(typeof(OperationCredential)) && !credentialType.IsSubclassOf(typeof(OperationCredential)))
                throw new ArgumentException("credentialType", "The credential type is not an operation credential.");

            _version = Version.Parse(version);
            _credentialType = credentialType;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the credential of the operation.
        /// </summary>
        /// <returns>The credential of the operation.</returns>
        public OperationCredential GetCredential() 
        {
            return _credential ?? (_credential = Activator.CreateInstance(_credentialType) as OperationCredential);
        }

        #endregion
    }
}
