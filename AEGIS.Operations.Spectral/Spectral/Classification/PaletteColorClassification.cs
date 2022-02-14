/// <copyright file="PaletteColorClassification.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Classification
{
    /// <summary>
    /// Represents an operation performing classification using a color palette.
    /// </summary>
    [OperationMethodImplementation("AEGIS::253401", "Palette color classification")]
    public class PaletteColorClassification : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The color palette.
        /// </summary>
        private UInt32[][] _palette;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteColorClassification"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public PaletteColorClassification(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteColorClassification"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public PaletteColorClassification(ISpectralGeometry source, ISpectralGeometry target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.PaletteColorClassification, parameters)
        {
            _palette = ResolveParameter<UInt16[][]>(SpectralOperationParameters.ColorPalette).Select(color => color.Cast<UInt32>().ToArray()).ToArray();
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            SetResultProperties(RasterFormat.Integer, 3, 8, RasterPresentation.CreateTrueColorPresentation(0, 1, 2));

            return base.PrepareResult();
        }
        
        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>
        /// The spectral value at the specified index.
        /// </returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            UInt32 value = Source.Raster.GetValue(rowIndex, columnIndex, 0);

            if (value >= _palette.Length)
                return 0;

            return _palette[value][bandIndex];
        }

        /// <summary>
        /// Computes the specified spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override UInt32[] Compute(Int32 rowIndex, Int32 columnIndex)
        {
            UInt32 value = Source.Raster.GetValue(rowIndex, columnIndex, 0);

            if (value >= _palette.Length)
                return Enumerable.Repeat((UInt32)0, _palette[0].Length).ToArray();

            return _palette[value];
        }

        #endregion 
    }
}
