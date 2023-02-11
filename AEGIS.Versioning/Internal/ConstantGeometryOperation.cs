// <copyright file="ConstantGeometryOperation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Management;

namespace ELTE.AEGIS.Versioning.Internal
{
    /// <summary>
    /// Defines an operation producing a constant geometry.
    /// </summary>
    /// <author>Máté Cserép</author>
    [OperationMethodImplementation("AEGIS::Internal::1", "Constant geometry production", "1.0.0")]
    internal sealed class ConstantGeometryOperation : Operation<IGeometry, IGeometry>
    {
        #region Private static fields

        /// <summary>
        /// Stores the geometry factory operation method.
        /// </summary>
        private static OperationMethod _operationMethod;

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets the geometry factory operation method.
        /// </summary>
        public static OperationMethod OperationMethod
        {
            get
            {
                return _operationMethod ?? (_operationMethod = OperationMethod.CreateMethod<IGeometry, IGeometry>(
                    "AEGIS::Internal::1", "Constant geometry production",
                    "An operation which produces a constant geometry object.",
                    false, GeometryModel.SpatialOrSpatioTemporal, ExecutionMode.InPlace, null));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantGeometryOperation" /> class.
        /// </summary>
        /// <param name="result">The geometry object to produce.</param>
        public ConstantGeometryOperation(IGeometry result)
            : base(result, result, OperationMethod, null)
        {
        }

        #endregion

        #region Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            // Left blank on purpose.
        }

        #endregion
    }
}
