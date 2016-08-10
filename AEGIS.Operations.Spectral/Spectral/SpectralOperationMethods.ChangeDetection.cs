using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.Operations.Spectral
{
    public static partial class SpectralOperationMethods
    {
        #region Private fields

        private static OperationMethod _classifiedImageChangeDetection;
        private static OperationMethod _classifiedAreaChangeDetection;
        private static OperationMethod _classificationCategoryChangeDetection;

        #endregion

        #region Public properties

        /// <summary>
        /// Change detection of classified images.
        /// </summary>
        public static OperationMethod ClassifiedImageChangeDetection
        {
            get
            {
                return _classifiedImageChangeDetection ?? (_classifiedImageChangeDetection =
                    SpectralOperationMethod.CreateSpectralTransformation("AEGIS::253502", "Change detection of classified images",
                                                                         "Change detection of categories in a classified image compared to a reference image.",
                                                                         null, "1.0", false, SpectralOperationDomain.Local,
                                                                         SpectralOperationParameters.ClassificationReferenceGeometry));
            }
        }

        /// <summary>
        /// Change detection of classification areas.
        /// </summary>
        public static OperationMethod ClassificationAreaChangeDetection
        {
            get
            {
                return _classifiedAreaChangeDetection ?? (_classifiedAreaChangeDetection =
                    OperationMethod.CreateMethod<ISpectralGeometry, Double[]>("AEGIS::253503", "Change detection of classification areas",
                                                                              "Change detection of the area of categories in a classified image compared to a reference image",
                                                                              null, "1.0", false, GeometryModel.Any, ExecutionMode.Any,
                                                                              SpectralOperationParameters.ClassificationReferenceGeometry,
                                                                              SpectralOperationParameters.DifferentialChangeDetection));

            }
        }

        /// <summary>
        /// Change detection of classification categories.
        /// </summary>
        public static OperationMethod ClassificationCategoryChangeDetection
        {
            get
            {
                return _classificationCategoryChangeDetection ?? (_classificationCategoryChangeDetection =
                    OperationMethod.CreateMethod<ISpectralGeometry, Double[,]>("AEGIS::253504", "Change detection of classification categories",
                                                                               "Change detection of the pixel count of each category in a classified image compared to a reference image",
                                                                               null, "1.0", false, GeometryModel.Any, ExecutionMode.Any,
                                                                               SpectralOperationParameters.ClassificationReferenceGeometry));
            }
        }

        #endregion
    }
}
