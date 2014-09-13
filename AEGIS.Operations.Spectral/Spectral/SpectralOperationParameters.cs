/// <copyright file="SpectralOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using ELTE.AEGIS.Operations.Spectral.Resampling;
using ELTE.AEGIS.Operations.Spectral.Resampling.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for spectral operations.
    /// </summary>
    [OperationParameterCollection]
    public static class SpectralOperationParameters
    {
        #region Query fields

        private static OperationParameter[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OperationParameter" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OperationParameter" /> instances within the collection.</value>
        public static IList<OperationParameter> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(SpectralOperationParameters).GetProperties().
                                                               Where(property => property.Name != "All").
                                                               Select(property => property.GetValue(null, null) as OperationParameter).
                                                               ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OperationParameter" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OperationParameter" /> instances that match the specified identifier.</returns>
        public static IList<OperationParameter> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList();
        }
        /// <summary>
        /// Returns all <see cref="OperationParameter" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OperationParameter" /> instances that match the specified name.</returns>
        public static IList<OperationParameter> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList();
        }

        #endregion

        #region Private static fields

        private static OperationParameter _bandIndex;
        private static OperationParameter _contrastEnhancementValue;
        private static OperationParameter _convertResultToRGB;
        private static OperationParameter _densitySlicingThresholds;
        private static OperationParameter _filterFactor;
        private static OperationParameter _filterKernel;
        private static OperationParameter _filterOffset;
        private static OperationParameter _filterSize;
        private static OperationParameter _filterWeight;
        private static OperationParameter _gammaValue;
        private static OperationParameter _gaussianStandardDeviation;
        private static OperationParameter _histogramMatchFunction;
        private static OperationParameter _histogramMatchValues;
        private static OperationParameter _indexOfBlueBand;
        private static OperationParameter _indexOfFarInfraredBand;
        private static OperationParameter _indexOfGreenBand;
        private static OperationParameter _indexOfInfraredBand;
        private static OperationParameter _indexOfLongWavelengthInfraredBand;
        private static OperationParameter _indexOfMiddleWavelengthInfraredBand;
        private static OperationParameter _indexOfNearInfraredBand;
        private static OperationParameter _indexOfOrangeBand;
        private static OperationParameter _indexOfRedBand;
        private static OperationParameter _indexOfShortWavelengthInfraredBand;
        private static OperationParameter _indexOfUltravioletBand;
        private static OperationParameter _indexOfVioletBand;
        private static OperationParameter _indexOfVisibleBand;
        private static OperationParameter _indexOfYellowBand;
        private static OperationParameter _lowerThresholdBoundary;
        private static OperationParameter _numberOfRows;
        private static OperationParameter _numberOfColumns;
        private static OperationParameter _spectralFactor;
        private static OperationParameter _spectralOffset;
        private static OperationParameter _spectralResamplingStrategy;
        private static OperationParameter _spectralResamplingType;
        private static OperationParameter _spectralSelectorFunction;
        private static OperationParameter _upperThresholdBoundary;

        #endregion

        #region Public static properties

        /// <summary>
        /// Band index.
        /// </summary>
        public static OperationParameter BandIndex
        {
            get
            {
                return _bandIndex ?? (_bandIndex =
                    OperationParameter.CreateOptionalParameter<Int32>("AEGIS::223001", "Band index",
                                                                      "The zero-based index of the band the operation should be executed on.", null,
                                                                      Int32.MaxValue,
                                                                      Conditions.IsNotNegative())
                    );
            }
        }

        /// <summary>
        /// Convert result to RGB.
        /// </summary>
        public static OperationParameter ConvertResultToRGB
        {
            get
            {
                return _convertResultToRGB ?? (_convertResultToRGB =
                     OperationParameter.CreateOptionalParameter<Boolean>("AEGIS::000000", "Convert result to RGB",
                                                                         "", null, false)
                     );
            }
        }

        /// <summary>
        /// Contrast enhancement value.
        /// </summary>
        public static OperationParameter DensitySlicingThresholds
        {
            get
            {
                return _densitySlicingThresholds ?? (_densitySlicingThresholds =
                    OperationParameter.CreateOptionalParameter<Double[]>("AEGIS::223128", "Density slicing thresholds",
                                                                         "The array of threshold values used for dencity slicing.", null, 
                                                                         (Double[])null)
                    );
            }
        }

        /// <summary>
        /// Contrast enhancement value.
        /// </summary>
        public static OperationParameter ContrastEnhancementValue
        {
            get
            {
                return _contrastEnhancementValue ?? (_contrastEnhancementValue =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223120", "Contrast enhancement value",
                                                                       "The contrast enhancement value used by the basic contrast operator.", null, 
                                                                       0)
                    );
            }
        }

        /// <summary>
        /// Filter factor.
        /// </summary>
        public static OperationParameter FilterFactor
        {
            get 
            {
                return _filterFactor ?? (_filterFactor =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223201", "Filter factor",
                                                                       "The factor used by the filter operation to divide the result.", null, 
                                                                       1)
                    );
            }
        }

        /// <summary>
        /// Filter kernel.
        /// </summary>
        public static OperationParameter FilterKernel
        {
            get
            {
                return _filterKernel ?? (_filterKernel =
                    OperationParameter.CreateRequiredParameter<Matrix>("AEGIS::223200", "Filter kernel",
                                                                       "The odd sized matrix used by filters for multipling neightbour values.", null,
                                                                       value => (value is Matrix) && (value as Matrix).NumberOfColumns == (value as Matrix).NumberOfRows && (value as Matrix).NumberOfRows % 2 == 1)
                    );
            }
        }

        /// <summary>
        /// Filter offset.
        /// </summary>
        public static OperationParameter FilterOffset
        {
            get
            {
                return _filterOffset ?? (_filterOffset =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223202", "Filter offset",
                                                                       "The offset used by the filter operation to add to the result.", null,
                                                                       0)
                    );
            }
        }

        /// <summary>
        /// Radius of the filter.
        /// </summary>
        public static OperationParameter FilterRadius
        {
            get
            {
                return _filterSize ?? (_filterSize =
                    OperationParameter.CreateOptionalParameter<Int32>("AEGIS::223205", "Radius of the filter",
                                                                      "The radius of the filter determining the number of neighbouring pixels to be convoluted by the filter. The radius must be a positive number.", null, 
                                                                      1,
                                                                      Conditions.IsPositive())
                    );
            }
        }

        /// <summary>
        /// Filter weight.
        /// </summary>
        public static OperationParameter FilterWeight
        {
            get
            {
                return _filterWeight ?? (_filterWeight =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223204", "Filter weight",
                                                                       "The weight of the central value in the filter.", null, 
                                                                       1)
                    );
            }
        }

        /// <summary>
        /// Gamma value.
        /// </summary>
        public static OperationParameter GammaValue
        {
            get
            {
                return _gammaValue ?? (_gammaValue =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::223204", "Gamma value",
                                                                       "The gamma value used for gamma correction.", null)
                );
            }
        }

        /// <summary>
        /// Gaussian standard deviation.
        /// </summary>
        public static OperationParameter GaussianStandardDeviation
        {
            get
            {
                return _gaussianStandardDeviation ?? (_gaussianStandardDeviation =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223104", "Gaussian standard deviation",
                                                                       "The standard deviation value for the Gaussian blur filter.", null, 1)
                );
            }
        }

        /// <summary>
        /// Histogram match function.
        /// </summary>
        public static OperationParameter HistogramMatchFunction
        {
            get
            {
                return _histogramMatchFunction ?? (_histogramMatchFunction =
                    OperationParameter.CreateRequiredParameter<Func<Int32, Double>>("AEGIS::223271", "Histogram match function",
                                                                                    "The function that is matched against the current raster histogram.", null)
                    );
            }
        }

        /// <summary>
        /// Histogram match values.
        /// </summary>
        public static OperationParameter HistogramMatchValues
        {
            get
            {
                return _histogramMatchValues ?? (_histogramMatchValues =
                    OperationParameter.CreateRequiredParameter<IList<Int32>>("AEGIS::223124", "Histogram match values.",
                                                                             "The histrogram values which are matched against the current raster historgam.", null)
                    );
            }
        }

        /// <summary>
        /// Index of blue band.
        /// </summary>
        public static OperationParameter IndexOfBlueBand
        {
            get
            {
                return _indexOfBlueBand ?? (_indexOfBlueBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223013", "Index of blue band",
                                                                       "The zero-based index of the blue spectral band within the raster.", null, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of far infrared (FIR) band.
        /// </summary>
        public static OperationParameter IndexOfFarInfraredBand
        {
            get
            {
                return _indexOfFarInfraredBand ?? (_indexOfFarInfraredBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223035", "Index of far infrared (FIR) band",
                                                                       "The zero-based index of the far infrared (FIR) spectral band within the raster.", null, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of green band.
        /// </summary>
        public static OperationParameter IndexOfGreenBand
        {
            get
            {
                return _indexOfGreenBand ?? (_indexOfGreenBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223012", "Index of green band",
                                                                       "The zero-based index of the green spectral band within the raster.", null, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of infrared (IR) band.
        /// </summary>
        public static OperationParameter IndexOfInfraredBand
        {
            get
            {
                return _indexOfInfraredBand ?? (_indexOfInfraredBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223030", "Index of infrared (IR) band",
                                                                       "The zero-based index of the infrared (IR) spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of Long-wavelength infrared (LWIR) band.
        /// </summary>
        public static OperationParameter IndexOfLongWavelengthInfraredBand
        {
            get
            {
                return _indexOfLongWavelengthInfraredBand ?? (_indexOfLongWavelengthInfraredBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223034", "Index of Long-wavelength infrared (LWIR) band",
                                                                       "The zero-based index of the Long-wavelength infrared (LWIR) spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of Middle-wavelength infrared (MWIR) band.
        /// </summary>
        public static OperationParameter IndexOfMiddleWavelengthInfraredBand
        {
            get
            {
                return _indexOfMiddleWavelengthInfraredBand ?? (_indexOfMiddleWavelengthInfraredBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223033", "Index of Middle-wavelength infrared (MWIR) band",
                                                                       "The zero-based index of the Middle-wavelength infrared (MWIR) spectral band within the raster.", null, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of near infrared (NIR) band.
        /// </summary>
        public static OperationParameter IndexOfNearInfraredBand
        {
            get
            {
                return _indexOfNearInfraredBand ?? (_indexOfNearInfraredBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223031", "Index of near infrared (NIR) band",
                                                                       "The zero-based index of the near infrared (NIR) spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of orange band.
        /// </summary>
        public static OperationParameter IndexOfOrangeBand
        {
            get
            {
                return _indexOfOrangeBand ?? (_indexOfOrangeBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223014", "Index of orange band",
                                                                       "The zero-based index of the orange spectral band within the raster.", null, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of red band.
        /// </summary>
        public static OperationParameter IndexOfRedBand
        {
            get
            {
                return _indexOfRedBand ?? (_indexOfRedBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223011", "Index of red band",
                                                                       "The zero-based index of the red spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of Short-wavelength infrared (SWIR) band.
        /// </summary>
        public static OperationParameter IndexOfShortWavelengthInfraredBand
        {
            get
            {
                return _indexOfShortWavelengthInfraredBand ?? (_indexOfShortWavelengthInfraredBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223032", "Index of Short-wavelength infrared (SWIR) band",
                                                                       "The zero-based index of the Short-wavelength infrared (SWIR) spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of ultraviolet (UV) band.
        /// </summary>
        public static OperationParameter IndexOfUltravioletBand
        {
            get
            {
                return _indexOfUltravioletBand ?? (_indexOfUltravioletBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223020", "Index of ultraviolet (UV) band",
                                                                       "The zero-based index of the ultraviolet (UV) spectral band within the raster.", null, 
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of violet band.
        /// </summary>
        public static OperationParameter IndexOfVioletBand
        {
            get
            {
                return _indexOfVioletBand ?? (_indexOfVioletBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223016", "Index of violet band",
                                                                       "The zero-based index of the violet spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of visible band.
        /// </summary>
        public static OperationParameter IndexOfVisibleBand
        {
            get
            {
                return _indexOfVisibleBand ?? (_indexOfVisibleBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223010", "Index of visible band",
                                                                       "The zero-based index of the visible spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of yellow band.
        /// </summary>
        public static OperationParameter IndexOfYellowBand
        {
            get
            {
                return _indexOfYellowBand ?? (_indexOfYellowBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::223015", "Index of yellow band",
                                                                       "The zero-based index of the yellow spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );

            }
        }

        /// <summary>
        /// Lower threshold boundary.
        /// </summary>
        public static OperationParameter LowerThresholdBoundary
        {
            get
            {
                return _lowerThresholdBoundary ?? (_lowerThresholdBoundary =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS:223101", "Lower threshold boundary",
                                                                       "The lower threshold boundary value for creating a monochrome image.", null)
                );

            }
        }

        /// <summary>
        /// Number of rows.
        /// </summary>
        public static OperationParameter NumberOfRows
        {
            get
            {
                return _numberOfRows ?? (_numberOfRows =
                    OperationParameter.CreateRequiredParameter<Int32>("AEGIS::223015", "Number of rows",
                                                                      "The number of rows in the resulting image.", null,
                                                                      Conditions.IsPositive())
                );

            }
        }

        /// <summary>
        /// Number of columns.
        /// </summary>
        public static OperationParameter NumberOfColumns
        {
            get
            {
                return _numberOfColumns ?? (_numberOfColumns =
                    OperationParameter.CreateRequiredParameter<Int32>("AEGIS::223016", "Number of columns",
                                                                      "The number of columns in the resulting image.", null,
                                                                      Conditions.IsPositive())
                );

            }
        }

        /// <summary>
        /// Spectral factor.
        /// </summary>
        public static OperationParameter SpectralFactor
        {
            get
            {
                return _spectralFactor ?? (_spectralFactor =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:223108", "Spectral factor",
                                                                       "A factor by which all spectral values are multiplied.", null,
                                                                       1)
                );

            }
        }

        /// <summary>
        /// Spectral offset.
        /// </summary>
        public static OperationParameter SpectralOffset
        {
            get
            {
                return _spectralOffset ?? (_spectralOffset =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:223107", "Spectral offset",
                                                                       "An offset by which the spectral values are changed.", null, 
                                                                       0)
                );

            }
        }

        /// <summary>
        /// Spectral selector function.
        /// </summary>
        public static OperationParameter SpectralResamplingStrategy
        {
            get
            {
                return _spectralResamplingStrategy ?? (_spectralResamplingStrategy =
                    OperationParameter.CreateOptionalParameter<SpectralResamplingStrategy>("AEGIS::223382", "Spectral resampling strategy",
                                                                                         "A strategy that is applied suring spectral resampling.", null,
                                                                                         (SpectralResamplingStrategy)null)
                );
            }
        }

        /// <summary>
        /// Spectral selector function.
        /// </summary>
        public static OperationParameter SpectralResamplingType
        {
            get
            {
                return _spectralResamplingType ?? (_spectralResamplingType =
                    OperationParameter.CreateOptionalParameter<SpectralResamplingType>("AEGIS::223381", "Spectral resampling type",
                                                                                       "The type of resampling to be used. The default reampling type is the Lanczos method.", null, 
                                                                                       Resampling.SpectralResamplingType.Lanczos)
                );
            }
        }

        /// <summary>
        /// Spectral selector function.
        /// </summary>
        public static OperationParameter SpectralSelectorFunction
        {
            get
            {
                return _spectralSelectorFunction ?? (_spectralSelectorFunction =
                    OperationParameter.CreateRequiredParameter<Func<IRaster, Int32, Int32, Int32, Boolean>>("AEGIS::223100", "Spectral selector function",
                                                                                                            "A function deciding whether a raster value meets a specified criteria.", null)
                );
            }
        }

        /// <summary>
        /// Upper threshold boundary.
        /// </summary>
        public static OperationParameter UpperThresholdBoundary
        {
            get
            {
                return _upperThresholdBoundary ?? (_upperThresholdBoundary =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:223102", "Upper threshold boundary",
                                                                       "The upper threshold boundary for creating a monochrome image.", null, 
                                                                       Double.PositiveInfinity)
                );

            }
        }

        #endregion
    }
}
