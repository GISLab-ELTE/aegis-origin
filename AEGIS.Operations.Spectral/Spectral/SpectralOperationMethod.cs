// <copyright file="SpectralOperationMethod.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a spectral operation method.
    /// </summary>
    public class SpectralOperationMethod : OperationMethod
    {
        #region Private fields

        /// <summary>
        /// The supported raster formats.
        /// </summary>
        private RasterFormat[] _supportedFormats;

        /// <summary>
        /// The spectral domain of the operation.
        /// </summary>
        private SpectralOperationDomain _spectralDomain;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the supported raster formats.
        /// </summary>
        /// <value>The read-only list containing the supported raster formats.</value>
        public IList<RasterFormat> SupportedFormats { get { return Array.AsReadOnly(_supportedFormats); } }

        /// <summary>
        /// Gets the spectral domain of the operation.
        /// </summary>
        /// <value>The spectral domain of the operation.</value>
        public SpectralOperationDomain SpectralDomain { get { return _spectralDomain; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectralOperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="spectralDomain">The spectral domain of the operation.</param>
        /// <param name="sourceType">The source type of the method.</param>
        /// <param name="resultType">The result type of the method.</param>
        /// <param name="supportedModels">The supported geometry models.</param>
        /// <param name="supportedFormats">The supported raster formats.</param>
        /// <param name="supportedModes">The supported execution modes of the method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public SpectralOperationMethod(String identifier, String name, String remarks, String[] aliases, Version version,
                                       Boolean isReversible, SpectralOperationDomain spectralDomain,
                                       Type sourceType, Type resultType, GeometryModel supportedModels,
                                       RasterFormat supportedFormats,                                       
                                       ExecutionMode supportedModes,
                                       params OperationParameter[] parameters)
            : base(identifier, name, remarks, aliases, version, isReversible, sourceType, resultType, supportedModels, supportedModes, parameters)
        {
            _supportedFormats = ExtractRasterFormats(supportedFormats);
            _spectralDomain = spectralDomain;
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a general <see cref="SpectralOperationMethod" /> instance for spectral transformations.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="spectralDomain">The spectral domain of the operation.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <returns>The <see cref="SpectralOperationMethod" /> instance produced by the method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The version is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The version has no components or more than three components.</exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public static SpectralOperationMethod CreateSpectralTransformation(String identifier, String name, String remarks, String[] aliases, String version, 
                                                                           Boolean isReversible, SpectralOperationDomain spectralDomain,
                                                                           params OperationParameter[] parameters)
        { 
            return new SpectralOperationMethod(identifier, name, remarks, aliases, Version.Parse(version),
                                               isReversible, spectralDomain,
                                               typeof(ISpectralGeometry),
                                               typeof(ISpectralGeometry),
                                               GeometryModel.Any,
                                               RasterFormat.Floating | RasterFormat.Integer,
                                               ExecutionMode.Any,
                                               parameters);
        }

        /// <summary>
        /// Creates a general <see cref="SpectralOperationMethod" /> instance for spectral transformations.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="spectralDomain">The spectral domain of the operation.</param>
        /// <param name="supportedExecutions">The supported execution modes of the method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <returns>The <see cref="SpectralOperationMethod" /> instance produced by the method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The version is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The version has no components or more than three components.</exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public static SpectralOperationMethod CreateSpectralTransformation(String identifier, String name, String remarks, String[] aliases, String version,
                                                                           Boolean isReversible, SpectralOperationDomain spectralDomain,
                                                                           ExecutionMode supportedExecutions,
                                                                           params OperationParameter[] parameters)
        {
            return new SpectralOperationMethod(identifier, name, remarks, aliases, Version.Parse(version),
                                               isReversible, spectralDomain,
                                               typeof(ISpectralGeometry),
                                               typeof(ISpectralGeometry),
                                               GeometryModel.Any,
                                               RasterFormat.Integer | RasterFormat.Floating, 
                                               supportedExecutions,
                                               parameters);
        }

        /// <summary>
        /// Creates a general <see cref="SpectralOperationMethod" /> instance for spectral transformations.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="spectralDomain">The spectral domain of the operation.</param>
        /// <param name="supportedFormats">The supported raster formats of the method.</param>
        /// <param name="parameters">The parameters of the operation.</param>
        /// <returns>The <see cref="SpectralOperationMethod" /> instance produced by the method.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public static SpectralOperationMethod CreateSpectralTransformation(String identifier, String name, String remarks, String[] aliases, String version,
                                                                           Boolean isReversible, SpectralOperationDomain spectralDomain,
                                                                           RasterFormat supportedFormats,
                                                                           params OperationParameter[] parameters)
        {
            return new SpectralOperationMethod(identifier, name, remarks, aliases, Version.Parse(version),
                                               isReversible, spectralDomain,
                                               typeof(ISpectralGeometry),
                                               typeof(ISpectralGeometry),
                                               GeometryModel.Any,
                                               supportedFormats,
                                               ExecutionMode.Any,
                                               parameters);
        }

        #endregion

        #region Protected static methods

        /// <summary>
        /// Extracts the raster format values.
        /// </summary>
        /// <param name="models">The raster format values.</param>
        /// <returns>The array of extracted enumeration values.</returns>
        protected static RasterFormat[] ExtractRasterFormats(RasterFormat values)
        {
            Array enumValues = Enum.GetValues(typeof(RasterFormat));

            return Enumerable.Range(0, enumValues.Length).Where(value => (Convert.ToInt32(values) & Calculator.Pow(2, value)) != 0).Select(value => (RasterFormat)Calculator.Pow(2, value)).ToArray();
        }

        #endregion
    }
}
