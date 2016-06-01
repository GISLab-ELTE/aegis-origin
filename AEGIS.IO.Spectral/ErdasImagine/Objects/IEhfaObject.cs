/// <copyright file="IEhfaObject.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Tamas Nagy</author>

using System.IO;
using ELTE.AEGIS.IO.ErdasImagine.Types;

namespace ELTE.AEGIS.IO.ErdasImagine.Objects
{
    /// <summary>
    /// Defines behavior of objects in the HFA file structure.
    /// </summary>
    internal interface IEhfaObject
    {
        /// <summary>
        /// Gets the type of the <see cref="IEhfaObject" />.
        /// </summary>
        /// <value>The type of the <see cref="IEhfaObject" />.</value>
        IEhfaObjectType Type { get; }

        /// <summary>
        /// Reads the contents of the <see cref="IEhfaObject"/> from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.ArgumentException">The stream is invalid.</exception>
        void Read(Stream stream);
    }
}
