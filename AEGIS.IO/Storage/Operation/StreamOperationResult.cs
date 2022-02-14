/// <copyright file="StreamOperationResult.cs" company="Eötvös Loránd University (ELTE)">
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

using System.IO;

namespace ELTE.AEGIS.IO.Storage.Operation
{
    /// <summary>
    /// Represent a file system result containing a stream.
    /// </summary>
    public class StreamOperationResult : FileSystemOperationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamOperationResult"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StreamOperationResult(Stream stream) : base(FileSystemOperationResultCode.Completed)
        {
            Stream = stream;
        }

        /// <summary>
        /// Gets the result stream.
        /// </summary>
        /// <value>The result stream.</value>
        public Stream Stream { get; private set; }
    }
}
