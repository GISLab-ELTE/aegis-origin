/// <copyright file="CompoundReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
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
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a compound reference system.
    /// </summary>
    public class CompoundReferenceSystem : ReferenceSystem
    {
        #region Private fields

        private readonly ReferenceSystem[] _components;
        private readonly AreaOfUse _areaOfUse;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the area of use.
        /// </summary>
        /// <value>The area of use where the reference system is applicable.</value>
        public AreaOfUse AreaOfUse { get { return _areaOfUse; } }

        /// <summary>
        /// Gets the compontents of the reference system.
        /// </summary>
        /// <value>The components of the compound reference system.</value>
        public IList<ReferenceSystem> Components { get { return Array.AsReadOnly(_components); } }

        #endregion

        #region ReferenceSystem properties

        /// <summary>
        /// Gets the number of dimensions.
        /// </summary>
        /// <value>The number of dimensions.</value>
        public override Int32 Dimension { get { return _components.Sum(referenceSystem => referenceSystem.Dimension); } }

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Compound; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope of the reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="components">The components of the reference system.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// No components are specified.
        /// </exception>
        public CompoundReferenceSystem(String identifier, String name, AreaOfUse areaOfUse, params ReferenceSystem[] components)
            : this(identifier, name, null, null, null, areaOfUse, components)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope of the reference system.</param>
        /// <param name="areaOfUse">The area of use.</param>
        /// <param name="components">The components of the reference system.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// No components are specified.
        /// </exception>
        public CompoundReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope, AreaOfUse areaOfUse, params ReferenceSystem[] components)
            : base(identifier, name, remarks, aliases, scope)
        {
            if (components == null)
                throw new ArgumentNullException("components", "No components are specified.");

            _components = components;
            _areaOfUse = areaOfUse;
        }

        #endregion
    }
}
