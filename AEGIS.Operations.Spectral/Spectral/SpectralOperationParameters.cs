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

using ELTE.AEGIS.Algorithms.Distances;
using ELTE.AEGIS.Algorithms.Resampling;
using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Management;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for spectral operations.
    /// </summary>
    [OperationParameterCollection]
    public static partial class SpectralOperationParameters
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

        private static OperationParameter _bandIndices;
        private static OperationParameter _bandIndex;
        private static OperationParameter _gammaValue;
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
        private static OperationParameter _numberOfColumns;
        private static OperationParameter _numberOfIterations;
        private static OperationParameter _numberOfRows;
        private static OperationParameter _rasterResamplingAlgorithm;
        private static OperationParameter _rasterResamplingAlgorithmType;
        private static OperationParameter _segmentCollection;
        private static OperationParameter _sourceColumnCount;
        private static OperationParameter _sourceColumnOffset;
        private static OperationParameter _sourceRowCount;
        private static OperationParameter _sourceRowOffset;
        private static OperationParameter _spectralDistance;
        private static OperationParameter _spectralDistanceType;
        private static OperationParameter _spectralFactor;
        private static OperationParameter _spectralOffset;

        #endregion

        #region Public static properties

        /// <summary>
        /// Band indices.
        /// </summary>
        public static OperationParameter BandIndices
        {
            get
            {
                return _bandIndices ?? (_bandIndices =
                    OperationParameter.CreateOptionalParameter<Int32[]>("AEGIS::223002", "Band indices",
                                                                        "The array of zero-based index of the band the operation should be executed on.", null,
                                                                        (Int32[])null)
                    );
            }
        }

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
        /// Gamma value.
        /// </summary>
        public static OperationParameter GammaValue
        {
            get
            {
                return _gammaValue ?? (_gammaValue =
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::223104", "Gamma value",
                                                                       "The gamma value used for gamma correction.", null)
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
                    OperationParameter.CreateRequiredParameter<Func<Int32, Double>>("AEGIS::223123", "Histogram match function",
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
        /// Number of iterations.
        /// </summary>
        public static OperationParameter NumberOfIterations
        {
            get
            {
                return _numberOfIterations ?? (_numberOfIterations =
                    OperationParameter.CreateRequiredParameter<Int32>("AEGIS::223009", "Number of iterations",
                                                                      "The number of iterations for an iterative algorithm.", null)
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
        /// Raster resampling algorithm.
        /// </summary>
        public static OperationParameter RasterResamplingAlgorithm
        {
            get
            {
                return _rasterResamplingAlgorithm ?? (_rasterResamplingAlgorithm =
                    OperationParameter.CreateOptionalParameter<RasterResamplingAlgorithm>("AEGIS::223382", "Raster resampling algorithm",
                                                                                          "The algroithm that performs the resampling of the raster.", null,
                                                                                          (RasterResamplingAlgorithm)null)
                );
            }
        }

        /// <summary>
        /// Raster resampling algorithm type.
        /// </summary>
        public static OperationParameter RasterResamplingAlgorithmType
        {
            get
            {
                return _rasterResamplingAlgorithmType ?? (_rasterResamplingAlgorithmType =
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::223383", "Raster resampling algorithm type",
                                                                     "The type of the algorithm that performs the resampling of the raster.", null,
                                                                     typeof(BilinearResamplingAlgorithm),
                                                                     Conditions.Inherits<RasterResamplingAlgorithm>())
                );
            }
        }

        /// <summary>
        /// Segment collection.
        /// </summary>
        public static OperationParameter SegmentCollection
        {
            get
            {
                return _segmentCollection ?? (_segmentCollection =
                    OperationParameter.CreateOptionalParameter<SegmentCollection>("AEGIS::213060", "Segment collection", "An enumerable collection of segments.", null)
                );
            }
        }

        /// <summary>
        /// Source column count.
        /// </summary>
        public static OperationParameter SourceColumnCount
        {
            get
            {
                return _sourceColumnCount ?? (_sourceColumnCount =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223484", "Source column count", "The number of columns taken from the source image.", null)
                );
            }
        }

        /// <summary>
        /// Source column offset.
        /// </summary>
        public static OperationParameter SourceColumnOffset
        {
            get
            {
                return _sourceColumnOffset ?? (_sourceColumnOffset =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223482", "Source column offset", "The offset of columns within the source image.", null, 0.0)
                );
            }
        }

        /// <summary>
        /// Source row count.
        /// </summary>
        public static OperationParameter SourceRowCount
        {
            get
            {
                return _sourceRowCount ?? (_sourceRowCount =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223483", "Source row count", "The number of rows taken from the source image.", null)
                );
            }
        }

        /// <summary>
        /// Source row offset.
        /// </summary>
        public static OperationParameter SourceRowOffset
        {
            get
            {
                return _sourceRowOffset ?? (_sourceRowOffset =
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::223481", "Source row offset", "The offset of rows within the source image.", null, 0.0)
                );
            }
        }

        /// <summary>
        /// Spectral distance algorithm.
        /// </summary>
        public static OperationParameter SpectralDistanceAlgorithm
        {
            get
            {
                return _spectralDistance ?? (_spectralDistance =
                    OperationParameter.CreateOptionalParameter<SpectralDistance>("AEGIS::223412", "Spectral distance algorithm",
                                                                                 "The algorithm used for determining the distance of spectral values.", null, (SpectralDistance)null)
                );
            }
        }

        /// <summary>
        /// Spectral distance type.
        /// </summary>
        public static OperationParameter SpectralDistanceType
        {
            get
            {
                return _spectralDistanceType ?? (_spectralDistanceType =
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::223413", "Spectral distance type",
                                                                     "The type used for determining the distance of spectral values.", null, (Type)null)
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

        #endregion
    }
}
