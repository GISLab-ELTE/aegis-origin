/// <copyright file="ITransformationStrategy.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Reference;

namespace ELTE.AEGIS.Operations.Spatial.Strategy
{
    /// <summary>
    /// Defines behavior of transformation strategies.
    /// </summary>
    public interface ITransformationStrategy : ITransformationStrategy<Coordinate, Coordinate>
    {     
    }

    /// <summary>
    /// Defines general behavior of transformation strategies.
    /// </summary>
    public interface ITransformationStrategy<SourceType, ResultType>
    {
        /// <summary>
        /// Gets the source reference system.
        /// </summary>
        /// <value>The source reference system.</value>
        ReferenceSystem SourceReferenceSystem { get; }

        /// <summary>
        /// Gets the target reference system.
        /// </summary>
        /// <value>The target reference system.</value>
        ReferenceSystem TargetReferenceSystem { get; }

        /// Transforms the specified coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The transformed coordinate.</returns>
        ResultType Transform(SourceType coordinate);
    }
}
