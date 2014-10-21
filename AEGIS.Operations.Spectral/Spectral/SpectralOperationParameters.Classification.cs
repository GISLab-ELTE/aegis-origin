using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Operations.Spectral
{
    /// <summary>
    /// Represents a collection of known <see cref="OperationParameter" /> instances for spectral operations.
    /// </summary>
    public static partial class SpectralOperationParameters
    {
        #region Private static fields

        private static OperationParameter _densitySlicingThresholds;
        private static OperationParameter _lowerThresholdBoundary;
        private static OperationParameter _spectralSelectorFunction;
        private static OperationParameter _upperThresholdBoundary;

        #endregion

        #region Public static properties


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
