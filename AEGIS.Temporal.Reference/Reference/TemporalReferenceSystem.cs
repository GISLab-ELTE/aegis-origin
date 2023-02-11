// <copyright file="TemporalReferenceSystem.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

using ELTE.AEGIS.Reference;
using System;

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a temporal reference system.
    /// </summary>
    public abstract class TemporalReferenceSystem : ReferenceSystem
    {
        #region ReferenceSystem properties

        /// <summary>
        /// Gets the dimension of the reference system.
        /// </summary>
        /// <value>The dimension of the reference system.</value>
        public override Int32 Dimension { get { return 1; }  }

        /// <summary>
        /// Gets the type of the reference system.
        /// </summary>
        /// <value>The type of the reference system.</value>
        public override ReferenceSystemType Type { get { return ReferenceSystemType.Temporal; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporalReferenceSystem" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="scope">The scope.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected TemporalReferenceSystem(String identifier, String name, String remarks, String[] aliases, String scope)
            : base(identifier, name, remarks, aliases, scope) 
        {
        }

        #endregion
    }
}
