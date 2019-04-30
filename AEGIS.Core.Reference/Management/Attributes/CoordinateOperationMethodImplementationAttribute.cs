/// <copyright file="IdentifiedObjectInstanceAttribute.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS.Management
{
    /// <summary>
    /// Indicates that the type implements a coordinate operation method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CoordinateOperationMethodImplementationAttribute : IdentifiedObjectInstanceAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateOperationMethodImplementationAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// </exception>
        public CoordinateOperationMethodImplementationAttribute(String identifier, String name) 
            : base(identifier, name)
        { }

        #endregion
    }
}
