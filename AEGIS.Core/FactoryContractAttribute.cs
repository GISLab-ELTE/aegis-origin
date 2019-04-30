/// <copyright file="FactoryContractAttribute.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Indicates that the interface is a factory contract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class FactoryContractAttribute : Attribute
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the default factory behavior.
        /// </summary>
        /// <value>The type of the default behavior for the factory.</value>
        public Type DefaultBehavior { get; set; }

        /// <summary>
        /// Gets or sets the factory product.
        /// </summary>
        /// <value>The type of the product of the factory.</value>
        public Type Product { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryContractAttribute"/> class.
        /// </summary>
        public FactoryContractAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryContractAttribute" /> class.
        /// </summary>
        /// <param name="productType">The product type.</param>
        public FactoryContractAttribute(Type productType)
        {
            Product = productType;
        }

        #endregion
    }
}
