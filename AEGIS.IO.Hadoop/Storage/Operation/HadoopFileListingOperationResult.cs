// <copyright file="HadoopFileListingOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represents a Hadoop file system operation result containing a file status list.
    /// </summary>
    public class HadoopFileListingOperationResult : HadoopFileSystemOperationResult
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the status list.
        /// </summary>
        /// <value>The status list.</value>
        public IList<HadoopFileStatusOperationResult> StatusList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperationResult"/> class.
        /// </summary>
        public HadoopFileListingOperationResult() 
        {
            StatusList = new HadoopFileStatusOperationResult[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopFileListingOperationResult" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="statusList">The list of file status results.</param>
        public HadoopFileListingOperationResult(String request, IList<HadoopFileStatusOperationResult> statusList)
            :base(request)
        {
            if (statusList == null)
                StatusList = new HadoopFileStatusOperationResult[0];
            else
                StatusList = statusList; 
        }

        #endregion
    }
}
