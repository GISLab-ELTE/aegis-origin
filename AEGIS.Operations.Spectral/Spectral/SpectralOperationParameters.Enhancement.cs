
using ELTE.AEGIS.Management;
/// <copyright file="SpertalOperationParameters.Enhancement.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Numerics;
using System;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="SpectralOperationParameter" /> instances.
    /// </summary>
    public static partial class SpectralOperationParameters
    {
        #region Private static fields

        private static OperationParameter _morphologicalStructuringElement;

        #endregion

        #region Public static properties

        /// <summary>
        /// Morphological structuring element.
        /// </summary>
        public static OperationParameter MorphologicalStructuringElement
        {
            get
            {
                return _morphologicalStructuringElement ?? (_morphologicalStructuringElement =
                    OperationParameter.CreateOptionalParameter<Matrix>("AEGIS::350591", "Morphological structuring element",
                                                                      "The structuring element (matrix) used for mathematical morphology operations.", null,
                                                                      new Matrix (new Double[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } })));
            }
        }

        #endregion
    }
}
