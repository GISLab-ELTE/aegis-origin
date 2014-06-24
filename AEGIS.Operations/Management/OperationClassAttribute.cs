/// <copyright file="OperationClassAttribute.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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

namespace ELTE.AEGIS.Operations.Management
{
    /// <summary>
    /// Idicates that the specified type is an operation implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class OperationClassAttribute : IdentifiedObjectInstanceAttribute
    {
        #region Private fields

        /// <summary>
        /// The version of the operation.
        /// </summary>
        private readonly Version _version;

        /// <summary>
        /// The type of the certificate.
        /// </summary>
        private readonly Type _certificateType;

        /// <summary>
        /// The certificate initialized from the type.
        /// </summary>
        private OperationCertificate _certificate;

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
        /// Initializes a new instance of the <see cref="OperationClassAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// </exception>
        public OperationClassAttribute(String identifier, String name)
            : base(identifier, name)
        {
            _version = Version.Default;
            _certificateType = typeof(OperationCertificate);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationClassAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// or
        /// The version is null.
        /// </exception>
        public OperationClassAttribute(String identifier, String name, String version)
            : base(identifier, name)
        {
            _version = Version.Parse(version);
            _certificateType = typeof(OperationCertificate);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationClassAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="certificateType">The type of the certificate.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// or
        /// The version is null.
        /// or
        /// The certificate type is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The specified type is not an operation certificate.
        /// or
        /// The version has no components or more than three components.
        /// </exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public OperationClassAttribute(String identifier, String name, String version, Type certificateType)
            : base(identifier, name)
        {
            if (certificateType == null)
                throw new ArgumentNullException("certificateType", "The certificate type is null.");

            if (!certificateType.Equals(typeof(OperationCertificate)) && !certificateType.IsSubclassOf(typeof(OperationCertificate)))
                throw new ArgumentException("certificateType", "The specified type is not an operation certificate.");

            _version = Version.Parse(version);
            _certificateType = certificateType;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the certificate of the operation.
        /// </summary>
        /// <returns>The certificate of the operation.</returns>
        public OperationCertificate GetCertificate() 
        {
            return _certificate ?? (_certificate = Activator.CreateInstance(_certificateType) as OperationCertificate);
        }

        #endregion
    }
}
