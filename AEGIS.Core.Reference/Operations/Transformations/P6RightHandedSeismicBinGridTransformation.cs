/// <copyright file="P6RightHandedSeismicBinGridTransformation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Reference.Operations
{
    /// <summary>
    /// Represents a P6 right handed seismic bin grid transformation.
    /// </summary>
    [CoordinateOperationMethodImplementationAttribute("AEGIS::9666", "P6 (I = J+90°) seismic bin grid transformation")]
    public class P6RightHandedSeismicBinGridTransformation : P6SeismicBinGridTransformation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="P6RightHandedSeismicBinGridTransformation" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The parameter is not an angular value as required by the method.
        /// or
        /// The parameter is not a length value as required by the method.
        /// or
        /// The parameter is not a double precision floating-point number as required by the method.
        /// </exception>
        public P6RightHandedSeismicBinGridTransformation(String identifier, String name, IDictionary<CoordinateOperationParameter, Object> parameters)
            : base(identifier, name, CoordinateOperationMethods.P6RightHandedSeismicBinGridTransformation, parameters)
        {
            _orientation = Orientation.RightHanded;
        }

        #endregion
    }
}
