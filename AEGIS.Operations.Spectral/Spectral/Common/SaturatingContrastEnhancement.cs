using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    /// Represents an inversion transformation.
    /// </summary>
    [OperationMethodImplementation("AEGIS::213130", "Saturating contrast enhancement")]
    public class SaturatingContrastEnhancement : PerBandSpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The array of offset values.
        /// </summary>
        private Int32[] _offset;

        /// <summary>
        /// The array of factor values.
        /// </summary>
        private Double[] _factor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaturatingContrastEnhancement" /> class.
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
        public SaturatingContrastEnhancement(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaturatingContrastEnhancement" /> class.
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
        public SaturatingContrastEnhancement(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.SaturatingContrastEnhancement, parameters)
        {
            if (_sourceBandIndex >= 0)
            {
                Int32 minIntensity = Int32.MaxValue, maxIntensity = 0;
                for (Int32 intensity = 0; intensity < _source.Raster.HistogramValues[_sourceBandIndex].Count; intensity++)
                {
                    if (_source.Raster.HistogramValues[_sourceBandIndex][intensity] > 0)
                    {
                        if (minIntensity > intensity)
                            minIntensity = intensity;
                        maxIntensity = intensity;
                    } 
                }

                _offset = new Int32[] { -minIntensity };
                _factor = new Double[] { RasterAlgorithms.RadiometricResolutionMax(_source.Raster.RadiometricResolutions[_sourceBandIndex]) / (maxIntensity - minIntensity) };
            }
            else
            {
                _offset = new Int32[_source.Raster.NumberOfBands];
                _factor = new Double[_source.Raster.NumberOfBands];

                for (Int32 bandIndex = 0; bandIndex < _source.Raster.NumberOfBands; bandIndex++)
                {
                    Int32 minIntensity = Int32.MaxValue, maxIntensity = 0;
                    for (Int32 intensity = 0; intensity < _source.Raster.HistogramValues[bandIndex].Count; intensity++)
                    {
                        if (_source.Raster.HistogramValues[bandIndex][intensity] > 0)
                        {
                            if (minIntensity > intensity)
                                minIntensity = intensity;
                            maxIntensity = intensity;
                        }
                    }

                    _offset[bandIndex] = -minIntensity;
                    _factor[bandIndex] = (Double)RasterAlgorithms.RadiometricResolutionMax(_source.Raster.RadiometricResolutions[bandIndex]) / (maxIntensity - minIntensity);
                }
            }
        }

        #endregion

        #region Protected RasterTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return RasterAlgorithms.Restrict(_source.Raster.GetValue(rowIndex, columnIndex, bandIndex) * _factor[bandIndex] + _offset[bandIndex], _source.Raster.RadiometricResolutions[bandIndex]);
        }

        /// <summary>
        /// Computes the specified floating spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override Double ComputeFloat(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            return _source.Raster.GetFloatValue(rowIndex, columnIndex, bandIndex) * _factor[bandIndex] + _offset[bandIndex];
        }

        #endregion
    }
}
