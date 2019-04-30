/// <copyright file="IdentifiedObjectFactoryAttribute.cs" company="Eötvös Loránd University (ELTE)">
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
    /// Indicates that the type is a factory for identified object instances.
    /// </summary>
    /// <remarks>
    /// The supported type is a static class with methods producing the <see cref="T:ELTE.AEGIS.IdentifiedObject" /> instances of a specified subclass.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class IdentifiedObjectFactoryAttribute : Attribute
    {
        #region Public properties

        /// <summary>
        /// Gets the type of the instances.
        /// </summary>
        /// <value>The base type for all instances produced in the class.</value>
        public Type Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObjectFactoryAttribute" /> class.
        /// </summary>
        /// <param name="type">The base type for all instances in the collection.</param>
        /// <exception cref="System.ArgumentNullException">The types is null.</exception>
        /// <exception cref="System.ArgumentException">The type is not a subclass of <see cref="IdentifiedObject"/>.</exception>
        public IdentifiedObjectFactoryAttribute(Type type) 
        {
            if (type == null)
                throw new ArgumentNullException("type", "The type is null.");
            if (!type.IsSubclassOf(typeof(IdentifiedObject)))
                throw new ArgumentException("The type is not a subclass of IdentifiedObject.", "type");

            Type = type;
        }

        #endregion
    }
}
