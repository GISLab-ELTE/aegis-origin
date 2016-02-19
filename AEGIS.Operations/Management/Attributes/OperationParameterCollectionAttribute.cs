/// <copyright file="OperationParameterCollectionAttribute.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;

namespace ELTE.AEGIS.Operations.Management
{
    /// <summary>
    /// Indicates that the type is a collection of known identified object instances.
    /// </summary>
    /// <remarks>
    /// The supported type is a static class with properties representing the <see cref="OperationParameter" /> instances of a specified subclass.
    /// The class must also support a query property for all known instances and methods for identifier and name queries.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class OperationParameterCollectionAttribute : IdentifiedObjectCollectionAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationParameterCollectionAttribute"/> class.
        /// </summary>
        public OperationParameterCollectionAttribute() : base(typeof(OperationParameter)) { }

        #endregion
    }
}
