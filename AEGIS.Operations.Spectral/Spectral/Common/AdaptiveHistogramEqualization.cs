/// <copyright file="AdaptiveHistogramEqualization.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dóra Papp</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using ELTE.AEGIS.Raster;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    /// Represent an operation performing adaptive histogram equalization.
    /// </summary>
    /// <remarks>
    /// Adaptive histogram equalization (AHE) is a computer image processing technique used to improve contrast in images. 
    /// It differs from ordinary histogram equalization in the respect that the adaptive method computes several histograms, each 
    /// corresponding to a distinct section of the image, and uses them to redistribute the lightness values of the image. 
    /// Contrast Limited AHE(CLAHE) differs from ordinary adaptive histogram equalization in its contrast limiting.
    /// CLAHE was developed to prevent the overamplification of noise that adaptive histogram equalization can give rise to.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::250218", "Adaptive histogram equalization")]
    public class AdaptiveHistogramEqualization : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// Represent the data of a tile of the source raster that is needed for performing the adaptive histogram equalization.
        /// </summary>
        private class TileHistogramEqualizationParameters
        {
            /// <summary>
            /// The cumulative distribution values for a tile's histogram of each band.
            /// </summary>
            public Double[][] CumulativeDistributionValues { get; set; }

            /// <summary>
            /// The cumulative distribution value for a tile's histogram minimum of each band.
            /// </summary>
            public Double[] CumulativeDistributionMinimums { get; set; }

            /// <summary>
            /// The cumulative distribution value for a tile's histogram maximum of each band.
            /// </summary>
            public Double[] CumulativeDistributionMaximums { get; set; }

            /// <summary>
            /// The radiometric exponent value of each band of a tile.
            /// </summary>
            public Double[] RadiometricResolutionExponents { get; set; }    
        }

        /// <summary>
        /// The data of all tiles of the raster.
        /// </summary>
        private TileHistogramEqualizationParameters[,] _allTilesParameters;

        /// <summary>
        /// The number of tiles of the raster besides each other vertically.
        /// </summary>
        private Int32 _tileNumberVertically;

        /// <summary>
        /// The number of tiles of the raster besides each other horizontally.
        /// </summary>
        private Int32 _tileNumberHorizontally;

        /// <summary>
        /// The number of pixel rows of a tile that is not in the last tile row (tiles in the last tile row contain more pixel rows).
        /// </summary>
        private Int32 _numberOfPixelRowsOfNormalTile;

        /// <summary>
        /// The number of pixel columns of a tile that is not in the last tile column (tiles in the last tile column contain more pixel columns).
        /// </summary>
        private Int32 _numberOfPixelColumnsOfNormalTile;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveHistogramEqualization" /> class.
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
        /// </exception>
        public AdaptiveHistogramEqualization(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.AdaptiveHistogramEqualization, parameters)
        {
            _tileNumberVertically = ResolveParameter<Int32>(SpectralOperationParameters.TileNumberVertically);
            _tileNumberHorizontally = ResolveParameter<Int32>(SpectralOperationParameters.TileNumberHorizontally);
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            _allTilesParameters = new TileHistogramEqualizationParameters[_tileNumberVertically, _tileNumberHorizontally];
            _numberOfPixelRowsOfNormalTile = Source.Raster.NumberOfRows / _tileNumberVertically;
            _numberOfPixelColumnsOfNormalTile = Source.Raster.NumberOfColumns / _tileNumberHorizontally;

            ComputeParametersOfTiles();

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
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            // determining which four tiles are the closest to the actual pixel

            // upper left tile's tile-row index in the array of tiles
            Int32 upperLeftTileRowIndex = (_numberOfPixelRowsOfNormalTile % 2 == 0) ? (rowIndex + _numberOfPixelRowsOfNormalTile / 2 - 1) / _numberOfPixelRowsOfNormalTile - 1 : (rowIndex + _numberOfPixelRowsOfNormalTile / 2) / _numberOfPixelRowsOfNormalTile - 1;

            // upper left tile's tile-column index in the array of tiles
            Int32 upperLeftTileColumnIndex = (_numberOfPixelColumnsOfNormalTile % 2 == 0) ? (columnIndex + _numberOfPixelColumnsOfNormalTile / 2 - 1) / _numberOfPixelColumnsOfNormalTile - 1 : (columnIndex + _numberOfPixelColumnsOfNormalTile / 2) / _numberOfPixelColumnsOfNormalTile - 1;

            Int32 upperRightTileRowIndex = upperLeftTileRowIndex; // upper right tile's tile-row index in the array of tiles
            Int32 upperRightTileColumnIndex = upperLeftTileColumnIndex + 1; // upper right tile's tile-column index in the array of tiles

            Int32 lowerLeftTileRowIndex = upperLeftTileRowIndex + 1; // lower left tile's tile-row index in the array of tiles
            Int32 lowerLeftTileColumnIndex = upperLeftTileColumnIndex; // lower left tile's tile-column index in the array of tiles

            Int32 lowerRightTileRowIndex = upperLeftTileRowIndex + 1; // lower right tile's tile-row index in the array of tiles
            Int32 lowerRightTileColumnIndex = upperLeftTileColumnIndex + 1; // lower right tile's tile-column index in the array of tiles


            // the four values to be bilinearly interpolated, computed using the four used tiles' transformation formula
            // for the corresponding tile's (not adaptive!) histogram equalization
            Double upperLeftValue = 0;
            Double upperRightValue = 0;
            Double lowerLeftValue = 0;
            Double lowerRightValue = 0;

            if (!(upperLeftTileRowIndex == -1 || upperLeftTileColumnIndex == -1))
            {
                upperLeftValue = PixelValueTransformation(Source.Raster.GetValue(rowIndex, columnIndex, bandIndex), bandIndex, _allTilesParameters[upperLeftTileRowIndex, upperLeftTileColumnIndex]);
            }

            if (!(upperRightTileRowIndex == -1 || upperRightTileColumnIndex == _tileNumberHorizontally))
            {
                upperRightValue = PixelValueTransformation(Source.Raster.GetValue(rowIndex, columnIndex, bandIndex), bandIndex, _allTilesParameters[upperRightTileRowIndex, upperRightTileColumnIndex]);
            }

            if (!(lowerLeftTileRowIndex == _tileNumberVertically || lowerLeftTileColumnIndex == -1))
            {
                lowerLeftValue = PixelValueTransformation(Source.Raster.GetValue(rowIndex, columnIndex, bandIndex), bandIndex, _allTilesParameters[lowerLeftTileRowIndex, lowerLeftTileColumnIndex]);
            }

            if (!(lowerRightTileRowIndex == _tileNumberVertically || lowerRightTileColumnIndex == _tileNumberHorizontally))
            {
                lowerRightValue = PixelValueTransformation(Source.Raster.GetValue(rowIndex, columnIndex, bandIndex), bandIndex, _allTilesParameters[lowerRightTileRowIndex, lowerRightTileColumnIndex]);
            }


            // the weights of the upper tiles and the left tiles
            Double upperWeight = ((lowerLeftTileRowIndex * _numberOfPixelRowsOfNormalTile + _numberOfPixelRowsOfNormalTile / 2) - rowIndex) / (Double)((lowerLeftTileRowIndex * _numberOfPixelRowsOfNormalTile + _numberOfPixelRowsOfNormalTile / 2) - (upperLeftTileRowIndex * _numberOfPixelRowsOfNormalTile + _numberOfPixelRowsOfNormalTile / 2));

            Double leftWeight = ((upperRightTileColumnIndex * _numberOfPixelColumnsOfNormalTile + _numberOfPixelColumnsOfNormalTile / 2) - columnIndex) / (Double)((upperRightTileColumnIndex * _numberOfPixelColumnsOfNormalTile + _numberOfPixelColumnsOfNormalTile / 2) - (upperLeftTileColumnIndex * _numberOfPixelColumnsOfNormalTile + _numberOfPixelColumnsOfNormalTile / 2));


            Double value;

            if (upperLeftTileRowIndex == -1)
            {
                if (upperLeftTileColumnIndex == -1)
                    value = lowerRightValue;

                else if (upperLeftTileColumnIndex >= 0 && upperLeftTileColumnIndex <= (_tileNumberHorizontally - 2))
                    value = LinearInterpolation(lowerLeftValue, lowerRightValue, leftWeight);

                else
                    value = lowerLeftValue;
            }

            else if (upperLeftTileRowIndex >= 0 && upperLeftTileRowIndex <= (_tileNumberVertically - 2))
            {
                if (upperLeftTileColumnIndex == -1)
                    value = LinearInterpolation(upperRightValue, lowerRightValue, upperWeight);

                else if (upperLeftTileColumnIndex >= 0 && upperLeftTileColumnIndex <= (_tileNumberHorizontally - 2))
                    value = BilinearInterpolation(upperLeftValue, lowerLeftValue, upperRightValue, lowerRightValue, upperWeight, leftWeight);

                else
                    value = LinearInterpolation(upperLeftValue, lowerLeftValue, upperWeight);
            }

            else
            {
                if (upperLeftTileColumnIndex == -1)
                    value = upperRightValue;

                else if (upperLeftTileColumnIndex >= 0 && upperLeftTileColumnIndex <= (_tileNumberHorizontally - 2))
                    value = LinearInterpolation(upperLeftValue, upperRightValue, leftWeight);

                else
                    value = upperLeftValue;
            }

            return RasterAlgorithms.Restrict(value, Source.Raster.RadiometricResolution);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the parameters of the tiles.
        /// </summary>
        private void ComputeParametersOfTiles()
        {
            for (Int32 i = 0; i < _tileNumberVertically; ++i)
            {
                Int32 numberOfRowsInTile = (i < _tileNumberVertically - 1) ?
                    _numberOfPixelRowsOfNormalTile :
                    _numberOfPixelRowsOfNormalTile + Source.Raster.NumberOfRows % _tileNumberVertically;

                for (Int32 j = 0; j < _tileNumberHorizontally; ++j)
                {
                    Int32 numberOfColumnsInTile = (j < _tileNumberHorizontally - 1) ? _numberOfPixelColumnsOfNormalTile : _numberOfPixelColumnsOfNormalTile + Source.Raster.NumberOfColumns % _tileNumberHorizontally;

                    MaskedRaster tile = new MaskedRaster(null, Source.Raster, i * _numberOfPixelRowsOfNormalTile, j * _numberOfPixelColumnsOfNormalTile, numberOfRowsInTile, numberOfColumnsInTile);

                    _allTilesParameters[i, j] = new TileHistogramEqualizationParameters();
                    _allTilesParameters[i, j].CumulativeDistributionValues = new Double[Source.Raster.NumberOfBands][];
                    _allTilesParameters[i, j].CumulativeDistributionMinimums = new Double[Source.Raster.NumberOfBands];
                    _allTilesParameters[i, j].CumulativeDistributionMaximums = new Double[Source.Raster.NumberOfBands];
                    _allTilesParameters[i, j].RadiometricResolutionExponents = new Double[Source.Raster.NumberOfBands];

                    for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                    {
                        ComputeParametersOfActualTile(bandIndex, i, j, tile, _allTilesParameters[i, j]);
                    }
                }
            }
        }

        /// <summary>
        /// Computes the parameters of the specified band of the actual tile.
        /// </summary>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="tileRowIndex">The row index.</param>
        /// <param name="tileColumnIndex">The column index.</param>
        /// <param name="actualTile">The actual tile.</param>
        private void ComputeParametersOfActualTile(Int32 bandIndex, Int32 tileRowIndex, Int32 tileColumnIndex, MaskedRaster actualTile, TileHistogramEqualizationParameters actualTileParameters)
        {
            IReadOnlyList<Int32> histogram = actualTile.HistogramValues[bandIndex];

            actualTileParameters.CumulativeDistributionValues[bandIndex] = new Double[histogram.Count];

            // setting values
            actualTileParameters.CumulativeDistributionValues[bandIndex][0] = Convert.ToDouble(histogram[0]) / (actualTile.NumberOfRows * actualTile.NumberOfColumns);
            for (Int32 k = 1; k < histogram.Count; k++)
            {
                actualTileParameters.CumulativeDistributionValues[bandIndex][k] = actualTileParameters.CumulativeDistributionValues[bandIndex][k - 1] + Convert.ToDouble(histogram[k]) / (actualTile.NumberOfRows * actualTile.NumberOfColumns);
            }

            // setting minimum
            Int32 minIndex = 0;
            for (Int32 k = 0; k < histogram.Count; k++)
            {
                if (histogram[k] != 0)
                {
                    minIndex = k;
                    break;
                }
            }

            actualTileParameters.CumulativeDistributionMinimums[bandIndex] = actualTileParameters.CumulativeDistributionValues[bandIndex][minIndex];

            // setting maximum
            Int32 maxIndex = histogram.Count - 1;
            for (Int32 k = histogram.Count - 1; k >= 0; k--)
            {
                if (histogram[k] != 0)
                {
                    maxIndex = k;
                    break;
                }
            }

            actualTileParameters.CumulativeDistributionMaximums[bandIndex] = actualTileParameters.CumulativeDistributionValues[bandIndex][maxIndex];

            // exponent
            actualTileParameters.RadiometricResolutionExponents[bandIndex] = Calculator.Pow(2, actualTile.RadiometricResolution);
        }

        /// <summary>
        /// Computes the transformed pixel value of a specified band based on the transformation function of a specified tile.
        /// </summary>
        /// <param name="value">The value of the specified pixel of the specified band.</param>
        /// <param name="bandIndex">The band index.</param>
        /// <param name="tileParameters">The actual tile's data.</param>
        private Double PixelValueTransformation(UInt32 value, Int32 bandIndex, TileHistogramEqualizationParameters tileParameters)
        {
            return (tileParameters.CumulativeDistributionValues[bandIndex][value] - tileParameters.CumulativeDistributionMinimums[bandIndex]) / ((tileParameters.CumulativeDistributionMaximums[bandIndex] - tileParameters.CumulativeDistributionMinimums[bandIndex])) * tileParameters.RadiometricResolutionExponents[bandIndex];
        }

        /// <summary>
        /// Linearly interpolates the two specified value using the specified weight.
        /// </summary>
        /// <param name="a1">The first value.</param>
        /// <param name="a2">The second value.</param>
        /// <param name="weightOfFirstParameter">The weight of the first parameter.</param>
        private Double LinearInterpolation(Double a1, Double a2, Double weightOfFirstParameter)
        {
            return (weightOfFirstParameter * a1 + (1 - weightOfFirstParameter) * a2);
        }

        /// <summary>
        /// Bilinearly interpolates the four specified value using the specified weights.
        /// </summary>
        /// <param name="a1">The first value, which has to be linearly interpolated with "a2" in the first step.</param>
        /// <param name="a2">The second value, which has to be linearly interpolated with "a1" in the first step.</param>
        /// <param name="b1">The third value, which has to be linearly interpolated with "b2" in the second step.</param>
        /// <param name="b2">The fourth value, which has to be linearly interpolated with "b1" in the second step.</param>
        /// <param name="weightOfFirstParameterInTheFirstDimension">The weight of the first parameters in the first dimension, so in the first two linear interpolations.</param>
        /// <param name="weightOfFirstParameterInTheSecondDimension">The weight of the first parameter in the second dimension, so when linearly interpolating the two values computed in the first two linear interpolation steps.</param>
        private Double BilinearInterpolation(Double a1, Double a2, Double b1, Double b2, Double weightOfFirstParameterInTheFirstDimension, Double weightOfFirstParameterInTheSecondDimension)
        {
            return LinearInterpolation(LinearInterpolation(a1, a2, weightOfFirstParameterInTheFirstDimension), LinearInterpolation(b1, b2, weightOfFirstParameterInTheFirstDimension), weightOfFirstParameterInTheSecondDimension);
        }

        #endregion
    }
}
