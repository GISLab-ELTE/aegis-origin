/// <copyright file="OperationMethod.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
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
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Represents an operation method.
    /// </summary>
    [Serializable]
    public class OperationMethod : IdentifiedObject, IEquatable<OperationMethod>
    {
        #region Private fields

        /// <summary>
        /// The version of the method.
        /// </summary>
        private readonly Version _version;

        /// <summary>
        /// A value indicating whther the operation is reversible.
        /// </summary>
        private readonly Boolean _isReversible;

        /// <summary>
        /// The type of the source.
        /// </summary>
        private readonly Type _sourceType;

        /// <summary>
        /// The type of the result.
        /// </summary>
        private readonly Type _resultType;

        /// <summary>
        /// The supported geometry models.
        /// </summary>
        private readonly GeometryModel[] _supportedModels;

        /// <summary>
        /// The supported execution domains.
        /// </summary>
        private readonly ExecutionDomain[] _supportedDomains;

        /// <summary>
        /// The supported execution modes.
        /// </summary>
        private readonly ExecutionMode[] _supportedModes;

        /// <summary>
        /// The parameters of the method.
        /// </summary>
        private readonly OperationParameter[] _parameters;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the version of the method.
        /// </summary>
        /// <value>The version of the method.</value>
        public Version Version { get { return _version; } }

        /// <summary>
        /// Gets a values indicating whether the operation is reversible.
        /// </summary>
        /// <value><c>true</c> if the operation is reversible; otherwise, <c>false</c>.</value>
        public Boolean IsReversible { get { return _isReversible; } }

        /// <summary>
        /// Gets the source type of the method.
        /// </summary>
        /// <value>The source type of the method.</value>
        public Type SourceType { get { return _sourceType; } }

        /// <summary>
        /// Gets the result type of the method.
        /// </summary>
        /// <value>The result type of the method.</value>
        public Type ResultType { get { return _resultType; } }

        /// <summary>
        /// Gets or sets the geometry models supported by the method.
        /// </summary>
        /// <value>The read-only list containing the geometry models supported by the method.</value>
        public IList<GeometryModel> SupportedModels { get { return Array.AsReadOnly(_supportedModels); } }

        /// <summary>
        /// Gets or sets the execution modes supported by the method.
        /// </summary>
        /// <value>The read-only list containing the execution modes supported by the method.</value>
        public IList<ExecutionMode> SupportedModes { get { return Array.AsReadOnly(_supportedModes); } }

        /// <summary>
        /// Gets or sets the execution domains supported by the method.
        /// </summary>
        /// <value>The read-only list containing the execution domains supported by the method.</value>
        public IList<ExecutionDomain> SupportedDomains { get { return Array.AsReadOnly(_supportedDomains); } }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The read-only list containing the parameters of the method.</value>
        public IList<OperationParameter> Parameters { get { return Array.AsReadOnly(_parameters); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationMethod" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="sourceType">The source type of the method.</param>
        /// <param name="resultType">The result type of the method.</param>
        /// <param name="supportedModels">The supported models of the method.</param>
        /// <param name="supportedModes">The supported execution modes of the method.</param>
        /// <param name="supportedDomains">The supported execution domains of the method.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The source type is null.
        /// or
        /// The target type is null.
        /// </exception>
        public OperationMethod(String identifier, String name, String remarks, String[] aliases, Version version,
                               Boolean isReversible, 
                               Type sourceType, Type resultType, GeometryModel supportedModels, 
                               ExecutionMode supportedModes, ExecutionDomain supportedDomains,
                               params OperationParameter[] parameters)
            : base(identifier, name, remarks, aliases)
        {
            if (sourceType == null)
                throw new ArgumentNullException("sourceType", "The source type is null.");
            if (resultType == null)
                throw new ArgumentNullException("targetType", "The target type is null.");

            _version = version;
            _isReversible = isReversible;
            _sourceType = sourceType;
            _resultType = resultType;
            _supportedModels = ExtractGeometryModels(supportedModels);
            _supportedModes = ExtractExecutionModes(supportedModes);
            _supportedDomains = ExtractExecutionDomains(supportedDomains);
            _parameters = parameters;            
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Determines whether the specified <see cref="OperationMethod" /> is equal to the current <see cref="OperationMethod" />.
        /// </summary>
        /// <param name="another">The <see cref="OperationMethod" /> to compare with the current <see cref="OperationMethod" />.</param>
        /// <returns><c>true</c> if the specified <see cref="OperationMethod" /> is equal to the current <see cref="OperationMethod" />; otherwise, <c>false</c>.</returns>
        public Boolean Equals(OperationMethod another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return Identifier.Equals(another.Identifier) && _version.Equals(another._version);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to the current <see cref="System.Object" />.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with the current <see cref="System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to the current <see cref="System.Object" />; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is OperationMethod)) return false;

            return Identifier.Equals((obj as OperationMethod).Identifier) && _version.Equals((obj as OperationMethod)._version);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="OperationMethod" />.</returns>
        public override Int32 GetHashCode()
        {
            return Identifier.GetHashCode() ^ _version.GetHashCode() ^ 920419823;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the current <see cref="OperationMethod" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that contains both identifier and name.</returns>
        public override String ToString()
        {
            return "[" + Identifier + "] " + Identifier + " (V" + _version + ")";
        }

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a new instance of the <see cref="OperationMethod" /> class.
        /// </summary>
        /// <typeparam name="SourceType">The type of the source.</typeparam>
        /// <typeparam name="ResultType">The type of the result.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="supportedModels">The supported models of the method.</param>
        /// <param name="supportedModes">The supported execution modes of the method.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <returns>The <see cref="OperationMethod"/> instance produced by the method.</returns>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        public static OperationMethod CreateMethod<SourceType, ResultType>(String identifier, String name, String remarks,
                                                                           Boolean isReversible,
                                                                           GeometryModel supportedModels, ExecutionMode supportedModes,
                                                                           params OperationParameter[] parameters)
        {
            return new OperationMethod(identifier, name, remarks, null, Version.Default,
                                       isReversible, typeof(SourceType), typeof(ResultType),
                                       supportedModels, supportedModes, ExecutionDomain.Local | ExecutionDomain.Remote, parameters);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OperationMethod" /> class.
        /// </summary>
        /// <typeparam name="SourceType">The type of the source.</typeparam>
        /// <typeparam name="ResultType">The type of the result.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <param name="version">The version.</param>
        /// <param name="isReversible">Indicates whether the method is reversible.</param>
        /// <param name="supportedModels">The supported models of the method.</param>
        /// <param name="supportedModes">The supported execution modes of the method.</param>
        /// <param name="supportedDomains">The supported execution domains of the method.</param>
        /// <param name="parameters">The parameters of the method.</param>
        /// <returns>The <see cref="OperationMethod"/> instance produced by the method.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The identifier is null.
        /// or
        /// The version is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">The version has no components or more than three components.</exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public static OperationMethod CreateMethod<SourceType, ResultType>(String identifier, String name, String remarks, String[] aliases, String version,
                                                                           Boolean isReversible,
                                                                           GeometryModel supportedModels,
                                                                           ExecutionMode supportedModes, ExecutionDomain supportedDomains,
                                                                           params OperationParameter[] parameters)
        {
            return new OperationMethod(identifier, name, remarks, aliases, Version.Parse(version),
                                       isReversible, typeof(SourceType), typeof(ResultType),
                                       supportedModels, supportedModes, supportedDomains, parameters);
        }

        #endregion

        #region Protected static methods

        /// <summary>
        /// Extracts the geometry model values.
        /// </summary>
        /// <param name="models">The geometry model values.</param>
        /// <returns>The array of extracted enumeration values.</returns>
        protected static GeometryModel[] ExtractGeometryModels(GeometryModel values)
        {
            Array enumValues = Enum.GetValues(typeof(GeometryModel));

            return Enumerable.Range(0, enumValues.Length).Where(value => (Convert.ToInt32(values) & Calculator.Pow(2, value)) != 0).Select(value => (GeometryModel)Calculator.Pow(2, value)).ToArray();
        }

        /// <summary>
        /// Extracts the execution mode values.
        /// </summary>
        /// <param name="models">The execution mode values.</param>
        /// <returns>The array of extracted enumeration values.</returns>
        protected static ExecutionMode[] ExtractExecutionModes(ExecutionMode values)
        {
            Array enumValues = Enum.GetValues(typeof(ExecutionMode));

            return Enumerable.Range(0, enumValues.Length).Where(value => (Convert.ToInt32(values) & Calculator.Pow(2, value)) != 0).Select(value => (ExecutionMode)Calculator.Pow(2, value)).ToArray();
        }

        /// <summary>
        /// Extracts the execution domain values.
        /// </summary>
        /// <param name="models">The execution domain values.</param>
        /// <returns>The array of extracted enumeration values.</returns>
        protected static ExecutionDomain[] ExtractExecutionDomains(ExecutionDomain values)
        {
            Array enumValues = Enum.GetValues(typeof(ExecutionDomain));

            return Enumerable.Range(0, enumValues.Length).Where(value => (Convert.ToInt32(values) & Calculator.Pow(2, value)) != 0).Select(value => (ExecutionDomain)Calculator.Pow(2, value)).ToArray();
        }

        #endregion
    }
}
