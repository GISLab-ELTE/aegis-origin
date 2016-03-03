/// <copyright file="SpectralOperationParameters.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2016 Roberto Giachetta. Licensed under the
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
        private static OperationParameter _bandName;
        private static OperationParameter _bandNames;
        private static OperationParameter _gammaValue;
        private static OperationParameter _histogramMatchFunction;
        private static OperationParameter _histogramMatchValues;
        private static OperationParameter _indexOf445nmBand;
        private static OperationParameter _indexOf500nmBand;
        private static OperationParameter _indexOf510nmBand;
        private static OperationParameter _indexOf531nmBand;
        private static OperationParameter _indexOf550nmBand;
        private static OperationParameter _indexOf570nmBand;
        private static OperationParameter _indexOf680nmBand;
        private static OperationParameter _indexOf700nmBand;
        private static OperationParameter _indexOf705nmBand;
        private static OperationParameter _indexOf715nmBand;
        private static OperationParameter _indexOf720nmBand;
        private static OperationParameter _indexOf726nmBand;
        private static OperationParameter _indexOf734nmBand;
        private static OperationParameter _indexOf740nmBand;
        private static OperationParameter _indexOf747nmBand;
        private static OperationParameter _indexOf750nmBand;
        private static OperationParameter _indexOf800nmBand;
        private static OperationParameter _indexOf819nmBand;
        private static OperationParameter _indexOf900nmBand;
        private static OperationParameter _indexOf970nmBand;
        private static OperationParameter _indexOf1510nmBand;
        private static OperationParameter _indexOf1599nmBand;
        private static OperationParameter _indexOf1649nmBand;
        private static OperationParameter _indexOf1680nmBand;
        private static OperationParameter _indexOf1754nmBand;
        private static OperationParameter _indexOf2000nmBand;
        private static OperationParameter _indexOf2100nmBand;
        private static OperationParameter _indexOf2200nmBand;
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
        private static OperationParameter _indicesOfBandsBetween500nm600nm;
        private static OperationParameter _numberOfColumns;
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
        private static OperationParameter _tileNumberHorizontally;
        private static OperationParameter _tileNumberVertically;

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
                    OperationParameter.CreateOptionalParameter<IEnumerable<Int32>>("AEGIS::350102", "Band indices",
                                                                                   "The collection of zero-based indices of the bands the operation should be executed on.", null,
                                                                                   (IEnumerable<Int32>)null)
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
                    OperationParameter.CreateOptionalParameter<Int32>("AEGIS::350101", "Band index",
                                                                      "The zero-based index of the band the operation should be executed on.", null,
                                                                      Int32.MaxValue,
                                                                      Conditions.IsNotNegative())
                    );
            }
        }

        /// <summary>
        /// Band name.
        /// </summary>
        public static OperationParameter BandName
        {
            get
            {
                return _bandName ?? (_bandName =
                    OperationParameter.CreateOptionalParameter<String>("AEGIS::350108", "Band name",
                                                                       "The  name of the band the operation should be executed on.", null,
                                                                       (String)null)
                    );
            }
        }

        /// <summary>
        /// Band names.
        /// </summary>
        public static OperationParameter BandNames
        {
            get
            {
                return _bandNames ?? (_bandNames =
                    OperationParameter.CreateOptionalParameter<IEnumerable<String>>("AEGIS::350109", "Band names",
                                                                                    "The collection of band names the operation should be executed on.", null,
                                                                                    (IEnumerable<String>)null)
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
                    OperationParameter.CreateRequiredParameter<Double>("AEGIS::350204", "Gamma value",
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
                    OperationParameter.CreateRequiredParameter<Func<Int32, Double>>("AEGIS::354123", "Histogram match function",
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
                    OperationParameter.CreateRequiredParameter<IList<Int32>>("AEGIS::354124", "Histogram match values.",
                                                                             "The histogram values which are matched against the current raster histogram.", null)
                    );
            }
        }

        /// <summary>
        /// Index of the 445nm band.
        /// </summary>
        public static OperationParameter IndexOf445nmBand
        {
            get
            {
                return _indexOf445nmBand ?? (_indexOf445nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351445", "Index of the 445nm band",
                                                                       "The zero-based index of the 445nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 500nm band.
        /// </summary>
        public static OperationParameter IndexOf500nmBand
        {
            get
            {
                return _indexOf500nmBand ?? (_indexOf500nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351500", "Index of the 500nm band",
                                                                       "The zero-based index of the 500nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 510nm band.
        /// </summary>
        public static OperationParameter IndexOf510nmBand
        {
            get
            {
                return _indexOf510nmBand ?? (_indexOf510nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351510", "Index of the 510nm band",
                                                                       "The zero-based index of the 510nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 531nm band.
        /// </summary>
        public static OperationParameter IndexOf531nmBand
        {
            get
            {
                return _indexOf531nmBand ?? (_indexOf531nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351531", "Index of the 531nm band",
                                                                       "The zero-based index of the 531nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 550nm band.
        /// </summary>
        public static OperationParameter IndexOf550nmBand
        {
            get
            {
                return _indexOf550nmBand ?? (_indexOf550nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351550", "Index of the 550nm band",
                                                                       "The zero-based index of the 550nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 570nm band.
        /// </summary>
        public static OperationParameter IndexOf570nmBand
        {
            get
            {
                return _indexOf570nmBand ?? (_indexOf570nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351570", "Index of the 570nm band",
                                                                       "The zero-based index of the 570nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 680nm band.
        /// </summary>
        public static OperationParameter IndexOf680nmBand
        {
            get
            {
                return _indexOf680nmBand ?? (_indexOf680nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351680", "Index of the 680nm band",
                                                                       "The zero-based index of the 680nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 700nm band.
        /// </summary>
        public static OperationParameter IndexOf700nmBand
        {
            get
            {
                return _indexOf700nmBand ?? (_indexOf700nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351700", "Index of the 700nm band",
                                                                       "The zero-based index of the 700nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 705nm band.
        /// </summary>
        public static OperationParameter IndexOf705nmBand
        {
            get
            {
                return _indexOf705nmBand ?? (_indexOf705nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351705", "Index of the 705nm band",
                                                                       "The zero-based index of the 705nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 715nm band.
        /// </summary>
        public static OperationParameter IndexOf715nmBand
        {
            get
            {
                return _indexOf715nmBand ?? (_indexOf715nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351715", "Index of the 715nm band",
                                                                       "The zero-based index of the 715nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 720nm band.
        /// </summary>
        public static OperationParameter IndexOf720nmBand
        {
            get
            {
                return _indexOf720nmBand ?? (_indexOf720nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351720", "Index of the 720nm band",
                                                                       "The zero-based index of the 720nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 726nm band.
        /// </summary>
        public static OperationParameter IndexOf726nmBand
        {
            get
            {
                return _indexOf726nmBand ?? (_indexOf726nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351726", "Index of the 726nm band",
                                                                       "The zero-based index of the 726nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 734nm band.
        /// </summary>
        public static OperationParameter IndexOf734nmBand
        {
            get
            {
                return _indexOf734nmBand ?? (_indexOf734nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351734", "Index of the 734nm band",
                                                                       "The zero-based index of the 734nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 740nm band.
        /// </summary>
        public static OperationParameter IndexOf740nmBand
        {
            get
            {
                return _indexOf740nmBand ?? (_indexOf740nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351740", "Index of the 740nm band",
                                                                       "The zero-based index of the 740nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 747nm band.
        /// </summary>
        public static OperationParameter IndexOf747nmBand
        {
            get
            {
                return _indexOf747nmBand ?? (_indexOf747nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351747", "Index of the 747nm band",
                                                                       "The zero-based index of the 747nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 750nm band.
        /// </summary>
        public static OperationParameter IndexOf750nmBand
        {
            get
            {
                return _indexOf750nmBand ?? (_indexOf750nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351750", "Index of the 750nm band",
                                                                       "The zero-based index of the 750nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 800nm band.
        /// </summary>
        public static OperationParameter IndexOf800nmBand
        {
            get
            {
                return _indexOf800nmBand ?? (_indexOf800nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351800", "Index of the 800nm band",
                                                                       "The zero-based index of the 800nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 819nm band.
        /// </summary>
        public static OperationParameter IndexOf819nmBand
        {
            get
            {
                return _indexOf819nmBand ?? (_indexOf819nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351819", "Index of the 819nm band",
                                                                       "The zero-based index of the 819nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 900nm band.
        /// </summary>
        public static OperationParameter IndexOf900nmBand
        {
            get
            {
                return _indexOf900nmBand ?? (_indexOf900nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351900", "Index of the 900nm band",
                                                                       "The zero-based index of the 900nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 970nm band.
        /// </summary>
        public static OperationParameter IndexOf970nmBand
        {
            get
            {
                return _indexOf970nmBand ?? (_indexOf970nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351970", "Index of the 970nm band",
                                                                       "The zero-based index of the 970nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 1510nm band.
        /// </summary>
        public static OperationParameter IndexOf1510nmBand
        {
            get
            {
                return _indexOf1510nmBand ?? (_indexOf1510nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::352510", "Index of the 1510nm band",
                                                                       "The zero-based index of the 1510nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 1599nm band.
        /// </summary>
        public static OperationParameter IndexOf1599nmBand
        {
            get
            {
                return _indexOf1599nmBand ?? (_indexOf1599nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::352599", "Index of the 1599nm band",
                                                                       "The zero-based index of the 1599nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 1649nm band.
        /// </summary>
        public static OperationParameter IndexOf1649nmBand
        {
            get
            {
                return _indexOf1649nmBand ?? (_indexOf1649nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::352649", "Index of the 1649nm band",
                                                                       "The zero-based index of the 1649nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 1680nm band.
        /// </summary>
        public static OperationParameter IndexOf1680nmBand
        {
            get
            {
                return _indexOf1680nmBand ?? (_indexOf1680nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::352680", "Index of the 1680nm band",
                                                                       "The zero-based index of the 1680nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 1754nm band.
        /// </summary>
        public static OperationParameter IndexOf1754nmBand
        {
            get
            {
                return _indexOf1754nmBand ?? (_indexOf1754nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::353754", "Index of the 1754nm band",
                                                                       "The zero-based index of the 1754nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 2000nm band.
        /// </summary>
        public static OperationParameter IndexOf2000nmBand
        {
            get
            {
                return _indexOf2000nmBand ?? (_indexOf2000nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::353000", "Index of the 2000nm band",
                                                                       "The zero-based index of the 2000nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 2100nm band.
        /// </summary>
        public static OperationParameter IndexOf2100nmBand
        {
            get
            {
                return _indexOf2100nmBand ?? (_indexOf2100nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::353100", "Index of the 2100nm band",
                                                                       "The zero-based index of the 2100nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );
            }
        }

        /// <summary>
        /// Index of the 2200nm band.
        /// </summary>
        public static OperationParameter IndexOf2200nmBand
        {
            get
            {
                return _indexOf2200nmBand ?? (_indexOf2200nmBand =
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::353200", "Index of the 2200nm band",
                                                                       "The zero-based index of the 2200nm band within the raster.", null,
                                                                       Conditions.IsNotNegative())
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351023", "Index of blue band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351015", "Index of far infrared (FIR) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351022", "Index of green band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351010", "Index of infrared (IR) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351014", "Index of Long-wavelength infrared (LWIR) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351013", "Index of Middle-wavelength infrared (MWIR) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351011", "Index of near infrared (NIR) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351024", "Index of orange band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351021", "Index of red band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351012", "Index of Short-wavelength infrared (SWIR) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351030", "Index of ultraviolet (UV) band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351026", "Index of violet band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351020", "Index of visible band",
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
                    OperationParameter.CreateOptionalParameter<UInt32>("AEGIS::351025", "Index of yellow band",
                                                                       "The zero-based index of the yellow spectral band within the raster.", null,
                                                                       Conditions.IsNotNegative())
                );

            }
        }

        /// <summary>
        /// Indices of bands between 500nm and 600nm.
        /// </summary>
        public static OperationParameter IndicesOfBandsBetween500nm600nm
        {
            get
            {
                return _indicesOfBandsBetween500nm600nm ?? (_indicesOfBandsBetween500nm600nm =
                    OperationParameter.CreateOptionalParameter<UInt32[]>("AEGIS::355600", "Indices of bands between 500nm and 600nm",
                                                                         "The zero-based indices of all bands with wavelength between 500nm and 600nm.", null,
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
                    OperationParameter.CreateRequiredParameter<Int32>("AEGIS::350106", "Number of columns",
                                                                      "The number of columns in the resulting image.", null,
                                                                      Conditions.IsPositive())
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
                    OperationParameter.CreateRequiredParameter<Int32>("AEGIS::350107", "Number of rows",
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
                    OperationParameter.CreateOptionalParameter<RasterResamplingAlgorithm>("AEGIS::350382", "Raster resampling algorithm",
                                                                                          "The algorithm that performs the resampling of the raster.", null,
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
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::350383", "Raster resampling algorithm type",
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
                    OperationParameter.CreateOptionalParameter<SegmentCollection>("AEGIS::354060", "Segment collection", "An enumerable collection of segments.", null)
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::350361", "Source column count", "The number of columns taken from the source image.", null)
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::350363", "Source column offset", "The offset of columns within the source image.", null, 0.0)
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::350362", "Source row count", "The number of rows taken from the source image.", null)
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS::350364", "Source row offset", "The offset of rows within the source image.", null, 0.0)
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
                    OperationParameter.CreateOptionalParameter<SpectralDistance>("AEGIS::350212", "Spectral distance algorithm",
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
                    OperationParameter.CreateOptionalParameter<Type>("AEGIS::350213", "Spectral distance type",
                                                                     "The type used for determining the distance of spectral values.", null, (Type)null, 
                                                                     Conditions.Inherits<SpectralDistance>())
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:350208", "Spectral factor",
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
                    OperationParameter.CreateOptionalParameter<Double>("AEGIS:350207", "Spectral offset",
                                                                       "An offset by which the spectral values are changed.", null, 
                                                                       0)
                );

            }
        }

        /// <summary>
        /// Tile number horizontally.
        /// </summary>
        public static OperationParameter TileNumberHorizontally
        {
            get
            {
                return _tileNumberHorizontally ?? (_tileNumberHorizontally =
                    OperationParameter.CreateOptionalParameter<Int32>("", "Tile number horizontally",
                                                                       "The number of tiles of the raster besides each other horizontally.",
                                                                       null, 8, Conditions.IsPositive())
                );
            }
        }

        /// <summary>
        /// Tile number vertically.
        /// </summary>
        public static OperationParameter TileNumberVertically
        {
            get
            {
                return _tileNumberVertically ?? (_tileNumberVertically =
                    OperationParameter.CreateOptionalParameter<Int32>("", "Tile number vertically",
                                                                       "The number of tiles of the raster besides each other vertically.",
                                                                       null, 8, Conditions.IsPositive())
                );
            }
        }


        #endregion
    }
}
