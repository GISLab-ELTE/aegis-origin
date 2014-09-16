/// <copyright file="TopOfAtmosphereReflectanceComputation.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Reflectance
{
    /// <summary>
    /// Represents an operation computing the Top of Atmosphere (ToA) reflectance of raster geometries.
    /// </summary>
    [OperationClass("AEGIS::213461", "Top of atmosphere reflectance computation")]
    public class TopOfAtmosphereReflectanceComputation : SpectralTransformation
    {
        #region Private fields

        private readonly Int32 _indexOfRedBand;
        private readonly Int32 _indexOfNearInfraredBand;
        private readonly Int32 _indexOfShortWaveInfraredBand;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TopOfAtmosphereReflectanceComputation" /> class.
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
        public TopOfAtmosphereReflectanceComputation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopOfAtmosphereReflectanceComputation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="result">The result.</param>
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
        public TopOfAtmosphereReflectanceComputation(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.TopOfAthmospehereReflectanceComputation, parameters)
        {
            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.IndexOfRedBand))
            {
                _indexOfRedBand = Convert.ToInt32(parameters[SpectralOperationParameters.IndexOfRedBand]);
            }
            else
            {
                _indexOfRedBand = 0;
            }

            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.IndexOfNearInfraredBand))
            {
                _indexOfNearInfraredBand = Convert.ToInt32(parameters[SpectralOperationParameters.IndexOfNearInfraredBand]);
            }
            else
            {
                _indexOfNearInfraredBand = 1;
            }

            if (parameters != null && parameters.ContainsKey(SpectralOperationParameters.IndexOfShortWavelengthInfraredBand))
            {
                _indexOfShortWaveInfraredBand = Convert.ToInt32(parameters[SpectralOperationParameters.IndexOfShortWavelengthInfraredBand]);
            }
            else
            {
                _indexOfShortWaveInfraredBand = 2;
            }


            Single n11 = 42.310691F, n19 = 1573, n21 = 0, n55 = 1043, n57 = 0, n59 = 236, n61 = 0;
            Int32 n62 = 106;

            n21 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n19 * Math.Cos(n11 * Constants.PI / 180)));
            n61 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n59 * Math.Cos(n11 * Constants.PI / 180)));
            n57 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n55 * Math.Cos(n11 * Constants.PI / 180)));
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            Double swir, nir, red;

            Single n11 = 42.310691F, n13 = 0, n16 = 1858, n19 = 1573, n21 = 0, n55 = 1043, n57 = 0, n59 = 236, n61 = 0;
            Int32 n62 = 106;

            n21 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n19 * Math.Cos(n11 * Constants.PI / 180)));
            n13 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n16 * Math.Cos(n11 * Constants.PI / 180)));

            n61 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n59 * Math.Cos(n11 * Constants.PI / 180)));
            n57 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n55 * Math.Cos(n11 * Constants.PI / 180)));

            switch (_source.Raster.Format)
            {
                case RasterFormat.Floating:
                    switch (bandIndex)
                    {
                        case 0:
                            swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                            nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            return (swir - nir) / (swir + nir);
                        case 1:
                            nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand);
                            red = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);
                            return (nir - red) / (nir + red);
                        case 2:
                            swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand);
                            red = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand);
                            return (swir - red) / (swir + red);
                    }
                    break;
                case RasterFormat.Integer:
                    switch (bandIndex)
                    {
                        case 0:
                            swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand) / 10.587413 * n61;
                            nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) / 1.721458 * n57;
                            return (swir - nir) / (swir + nir);
                        case 1:
                            nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) / 1.721458 * n57;
                            red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) / 2.702960 * n21;
                            return (nir - red) / (nir + red);
                        case 2:
                            swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand) / 10.587413 * n61;
                            red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) / 2.702960 * n21;
                            return (swir - red) / (swir + red);
                    }
                    break;
            }

            return 0;
        }

        /// <summary>
        /// Computes the specified floating spectral values.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <returns>The array containing the spectral values for each band at the specified index.</returns>
        protected override Double[] ComputeFloat(Int32 rowIndex, Int32 columnIndex)
        {
            Double swir, nir, red;

            Single n11 = 42.310691F, n13 = 0, n16 = 1858, n19 = 1573, n21 = 0, n55 = 1043, n57 = 0, n59 = 236, n61 = 0;
            Int32 n62 = 106;

            n21 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n19 * Math.Cos(n11 * Constants.PI / 180)));
            n13 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n16 * Math.Cos(n11 * Constants.PI / 180)));

            n61 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n59 * Math.Cos(n11 * Constants.PI / 180)));
            n57 = Convert.ToSingle(Constants.PI * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Constants.PI / 180) * (1 - 0.01673 * Math.Cos(0.9856 * (n62 - 4) * Math.PI / 180))) / (n55 * Math.Cos(n11 * Constants.PI / 180)));

            switch (_source.Raster.Format)
            {
                case RasterFormat.Floating:
                    swir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand) / 10.587413 * n61;
                    nir = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfNearInfraredBand) / 1.721458 * n57;
                    red = _source.Raster.GetFloatValue(rowIndex, columnIndex, _indexOfRedBand) / 2.702960 * n21;

                    return new Double[] { red, nir, swir };
                case RasterFormat.Integer:

                    swir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfShortWaveInfraredBand) / 10.587413 * n61;
                    nir = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfNearInfraredBand) / 1.721458 * n57;
                    red = _source.Raster.GetValue(rowIndex, columnIndex, _indexOfRedBand) / 2.702960 * n21;

                    return new Double[] { red, nir, swir };
            }
            return null;
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        protected override void PrepareResult()
        {
            _result = _source.Factory.CreateSpectralGeometry(_source,
                                                             PrepareRasterResult(RasterFormat.Floating,
                                                                                 3,
                                                                                 _source.Raster.NumberOfRows,
                                                                                 _source.Raster.NumberOfColumns,
                                                                                 Enumerable.Repeat(64, 3).ToArray(),
                                                                                 _source.Raster.Mapper),
                                                             _source.Presentation,
                                                             _source.Imaging);
        }

        #endregion

    }
}
