/// <copyright file="FactoryMethodAttribute.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Management
{
    /// <summary>
    /// Indicates that the method is a factory method for producing <see cref="IndentifiedObject" /> instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IdentifiedObjectFactoryMethodAttribute : Attribute
    {
        #region Public properties

        /// <summary>
        /// Gets the identifier of the instance.
        /// </summary>
        /// <value>The identifier of the instance.</value>
        public String Identifier { get; private set; }

        /// <summary>
        /// Gets the name of the instance.
        /// </summary>
        /// <value>The name of the instance.</value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the type of the instance.
        /// </summary>
        /// <value>The type of the instance.</value>
        public Type InstanceType { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObjectFactoryMethodAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// </exception>
        public IdentifiedObjectFactoryMethodAttribute(String identifier, String name)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            Identifier = identifier;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifiedObjectFactoryMethodAttribute" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The name is null.
        /// </exception>
        public IdentifiedObjectFactoryMethodAttribute(String identifier, String name, Type instanceType)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");
            if (name == null)
                throw new ArgumentNullException("name", "The name is null.");

            Identifier = identifier;
            Name = name;
            InstanceType = instanceType;
        }

        #endregion
    }
}
