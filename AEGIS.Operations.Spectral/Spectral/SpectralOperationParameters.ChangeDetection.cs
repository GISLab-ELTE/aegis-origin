// <copyright file="SpectralOperationParameters.ChangeDetection.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Management;
using System;

namespace ELTE.AEGIS.Operations.Spectral
{
    public static partial class SpectralOperationParameters
    {
        #region Private fields

        private static OperationParameter _changeDetectionCategoryIndex;
        private static OperationParameter _changeDetectionCategoryIndices;
        private static OperationParameter _differentialChangeDetection;
        private static OperationParameter _differentialAreaComputation;
        private static OperationParameter _lossDetection;

        #endregion

        #region Public properties

        /// <summary>
        /// Change detection category index.
        /// </summary>
        public static OperationParameter ChangeDetectionCategoryIndex
        {
            get
            {
                return _changeDetectionCategoryIndex ?? (_changeDetectionCategoryIndex =
                    OperationParameter.CreateOptionalParameter<Int32>("AEGIS::354905", "Change detection category index",
                                                                      "Specifies a category index for the change detection operation.",
                                                                      null, 0, Conditions.IsPositive()));
            }
        }

        /// <summary>
        /// Change detection category indices.
        /// </summary>
        public static OperationParameter ChangeDetectionCategoryIndices
        {
            get
            {
                return _changeDetectionCategoryIndices ?? (_changeDetectionCategoryIndices =
                    OperationParameter.CreateOptionalParameter<Int32[]>("AEGIS::354906", "Classification category indices",
                                                                        "Specifies category indices for the change detection operation.",
                                                                        null, new Int32[0]));
            }
        }

        /// <summary>
        /// Differential change detection.
        /// </summary>
        public static OperationParameter DifferentialChangeDetection
        {
            get
            {
                return _differentialChangeDetection ?? (_differentialChangeDetection =
                    OperationParameter.CreateOptionalParameter<Boolean>("AEGIS::354907", "Differential change detection",
                                                                        "Specifies to only compute change detection with respect to the altered areas of the image.",
                                                                        null, false));
            }
        }

        /// <summary>
        /// Differential area computation.
        /// </summary>
        public static OperationParameter DifferentialAreaComputation
        {
            get
            {
                return _differentialAreaComputation ?? (_differentialAreaComputation =
                    OperationParameter.CreateOptionalParameter<Boolean>("AEGIS::354908", "Differential area computation",
                                                                        "Specifies to compute the area change with respect to the size of the altered area.",
                                                                        null, false));
            }
        }

        /// <summary>
        /// Loss detection.
        /// </summary>
        public static OperationParameter LossDetection
        {
            get
            {
                return _lossDetection ?? (_lossDetection =
                    OperationParameter.CreateOptionalParameter<Boolean>("AEGIS::354909", "Loss detection",
                                                                        "Specifies to compute loss instead of growth in classification change detection.",
                                                                        null, false));
            }
        }

        #endregion
    }
}
