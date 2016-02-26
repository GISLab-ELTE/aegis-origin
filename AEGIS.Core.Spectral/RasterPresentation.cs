/// <copyright file="RasterPresentation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type containing information for presentation of raster images.
    /// </summary>
    public class RasterPresentation
    {
        #region Private fields

        /// <summary>
        /// The array of color space bands.
        /// </summary>
        private RasterColorSpaceBand[] _bands;

        /// <summary>
        /// The color map used in pseudo-color and density slicing modes.
        /// </summary>
        private IDictionary<Int32, UInt32[]> _colorMap;

        #endregion

        #region Public properties

        /// <summary>
        /// The presentation model.
        /// </summary>
        /// <value>The presentation model of the raster.</value>
        public RasterPresentationModel Model { get; private set; } 

        /// <summary>
        /// The color map.
        /// </summary>
        /// <value>The read-only color map used in pseudo-color and density slicing modes.</value>
        public IDictionary<Int32, UInt32[]> ColorMap { get { return _colorMap == null ? null : _colorMap.IsReadOnly ? _colorMap : _colorMap.AsReadOnly(); } }
        
        /// <summary>
        /// The color space.
        /// </summary>
        /// <value>The primary color space used for presentation.</value>
        public RasterColorSpace ColorSpace { get; private set; }

        /// <summary>
        /// The bands of the color space.
        /// </summary>
        /// <value>The read-only list of color space bands.</value>
        public IList<RasterColorSpaceBand> Bands { get { return Array.AsReadOnly(_bands); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterPresentation" /> class.
        /// </summary>
        /// <param name="model">The presentation model.</param>
        /// <param name="colorSpace">The color space.</param>
        /// <param name="bands">The bands.</param>
        /// <exception cref="System.ArgumentException">Pseudo-color and density slicing models must define a color map.</exception>
        public RasterPresentation(RasterPresentationModel model, RasterColorSpace colorSpace, params RasterColorSpaceBand[] bands)
        {
            if (model == RasterPresentationModel.DensitySlicing || model == RasterPresentationModel.PseudoColor)
                throw new ArgumentException("Pseudo-color and density slicing models must define a color map.", "model");

            Model = model;
            ColorSpace = colorSpace;
            _bands = bands.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterPresentation" /> class.
        /// </summary>
        /// <param name="model">The presentation model.</param>
        /// <param name="colorMap">The color map.</param>
        /// <exception cref="System.ArgumentException">Only pseudo-color and density slicing models may define a color map.</exception>
        /// <exception cref="System.ArgumentNullException">The color map is null.</exception>
        public RasterPresentation(RasterPresentationModel model, IDictionary<Int32, UInt32[]> colorMap)
        {
            if (model != RasterPresentationModel.DensitySlicing && model != RasterPresentationModel.PseudoColor)
                throw new ArgumentException("Only pseudo-color and density slicing models can define a color map.", "model");
            if (colorMap == null)
                throw new ArgumentNullException("colorMap", "The color map is null.");

            Model = model;
            _colorMap = colorMap;
            _bands = new RasterColorSpaceBand[1] { RasterColorSpaceBand.Value };
        }

        

        #endregion

        #region Public static factory methods

        /// <summary>
        /// Creates a true color presentation.
        /// </summary>
        /// <returns>A true color raster presentation using RGB color space with red, green, blue band order.</returns>
        public static RasterPresentation CreateTrueColorPresentation()
        {
            return new RasterPresentation(RasterPresentationModel.TrueColor, RasterColorSpace.RGB, RasterColorSpaceBand.Red, RasterColorSpaceBand.Green, RasterColorSpaceBand.Blue);
        }

        /// <summary>
        /// Creates a true color presentation.
        /// </summary>
        /// <param name="indexOfRedBand">The zero based index of the red band.</param>
        /// <param name="indexOfGreenBand">The zero based index of the green band.</param>
        /// <param name="indexOfBlueBand">The zero based index of the blue band.</param>
        /// <returns>A true color raster presentation using RGB color space with the specified band order.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index of the red band is less than 0.
        /// or
        /// The index of the green band is less than 0.
        /// or
        /// The index of the blue band is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The red and green bands are specified for the same index.
        /// or
        /// The red and blue bands are specified for the same index.
        /// or
        /// The green and blue bands are specified for the same index.
        /// </exception>
        public static RasterPresentation CreateTrueColorPresentation(Int32 indexOfRedBand, Int32 indexOfGreenBand, Int32 indexOfBlueBand)
        {
            if (indexOfRedBand < 0)
                throw new ArgumentOutOfRangeException("indexOfRedBand", "The index of the red band is less than 0.");
            if (indexOfGreenBand < 0)
                throw new ArgumentOutOfRangeException("indexOfGreenBand", "The index of the green band is less than 0.");
            if (indexOfBlueBand < 0)
                throw new ArgumentOutOfRangeException("indexOfBlueBand", "The index of the blue band is less than 0.");
            if (indexOfGreenBand == indexOfRedBand)
                throw new ArgumentException("The red and green bands are specified for the same index.", "indexOfGreenBand");
            if (indexOfBlueBand == indexOfRedBand)
                throw new ArgumentException("The red and blue bands are specified for the same index.", "indexOfBlueBand");
            if (indexOfBlueBand == indexOfGreenBand)
                throw new ArgumentException("The green and blue bands are specified for the same index.", "indexOfBlueBand");

            RasterColorSpaceBand[] bands = new RasterColorSpaceBand[Calculator.Max(indexOfRedBand, indexOfGreenBand, indexOfBlueBand) + 1];

            bands[indexOfRedBand] = RasterColorSpaceBand.Red;
            bands[indexOfGreenBand] = RasterColorSpaceBand.Green;
            bands[indexOfBlueBand] = RasterColorSpaceBand.Blue;

            return new RasterPresentation(RasterPresentationModel.TrueColor, RasterColorSpace.RGB, bands);
        }

        /// <summary>
        /// Creates a true color presentation.
        /// </summary>
        /// <param name="colorSpace">The color space.</param>
        /// <returns>A true color raster representing the specified color space.</returns>
        /// <exception cref="System.ArgumentException">The specified color space is not supported.</exception>
        public static RasterPresentation CreateTrueColorPresentation(RasterColorSpace colorSpace)
        {
            RasterColorSpaceBand[] bands;

            switch (colorSpace)
            { 
                case RasterColorSpace.CIELab:
                    bands = new RasterColorSpaceBand[] { RasterColorSpaceBand.Lightness, RasterColorSpaceBand.A, RasterColorSpaceBand.B };
                    break;
                case RasterColorSpace.CMYK:
                    bands = new RasterColorSpaceBand[] { RasterColorSpaceBand.Cyan, RasterColorSpaceBand.Magenta, RasterColorSpaceBand.Yellow, RasterColorSpaceBand.Black };
                    break;
                case RasterColorSpace.HSL:
                    bands = new RasterColorSpaceBand[] { RasterColorSpaceBand.Hue, RasterColorSpaceBand.Saturation, RasterColorSpaceBand.Lightness };
                    break;
                case RasterColorSpace.HSV:
                    bands = new RasterColorSpaceBand[] { RasterColorSpaceBand.Hue, RasterColorSpaceBand.Saturation, RasterColorSpaceBand.Value };
                    break;
                case RasterColorSpace.RGB:
                    bands = new RasterColorSpaceBand[] { RasterColorSpaceBand.Red, RasterColorSpaceBand.Green, RasterColorSpaceBand.Blue };
                    break;
                case RasterColorSpace.YCbCr:
                    bands = new RasterColorSpaceBand[] { RasterColorSpaceBand.Luma, RasterColorSpaceBand.BlueDifferenceChroma, RasterColorSpaceBand.RedDifferenceChroma };
                    break;
                default:
                    throw new ArgumentException("The specified color space is not supported.", "colorSpace");
            }
            return new RasterPresentation(RasterPresentationModel.TrueColor, colorSpace, bands);
        }

        /// <summary>
        /// Creates a false color presentation using three bands.
        /// </summary>
        /// <param name="indexOfRedBand">The zero based index of the red band.</param>
        /// <param name="indexOfGreenBand">The zero based index of the green band.</param>
        /// <param name="indexOfBlueBand">The zero based index of the blue band.</param>
        /// <returns>A false color raster presentation using RGB color space with the specified band order.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The index of the red band is less than 0.
        /// or
        /// The index of the green band is less than 0.
        /// or
        /// The index of the blue band is less than 0.
        /// </exception>
        public static RasterPresentation CreateFalseColorPresentation(Int32 indexOfRedBand, Int32 indexOfGreenBand, Int32 indexOfBlueBand)
        {
            if (indexOfRedBand < 0)
                throw new ArgumentOutOfRangeException("indexOfRedBand", "The index of the red band is less than 0.");
            if (indexOfGreenBand < 0)
                throw new ArgumentOutOfRangeException("indexOfGreenBand", "The index of the green band is less than 0.");
            if (indexOfBlueBand < 0)
                throw new ArgumentOutOfRangeException("indexOfBlueBand", "The index of the blue band is less than 0.");

            RasterColorSpaceBand[] bands = new RasterColorSpaceBand[Calculator.Max(indexOfRedBand, indexOfGreenBand, indexOfBlueBand) + 1];

            bands[indexOfRedBand] = RasterColorSpaceBand.Red;
            bands[indexOfGreenBand] = RasterColorSpaceBand.Green;
            bands[indexOfBlueBand] = RasterColorSpaceBand.Blue;

            return new RasterPresentation(RasterPresentationModel.FalseColor, RasterColorSpace.RGB, bands);
        }

        /// <summary>
        /// Creates a grayscale presentation using a single band.
        /// </summary>
        /// <returns>A grayscale presentation using a single band.</returns>
        public static RasterPresentation CreateGrayscalePresentation()
        {
            return new RasterPresentation(RasterPresentationModel.Grayscale, RasterColorSpace.None);
        }

        /// <summary>
        /// Creates an inverted grayscale presentation using a single band.
        /// </summary>
        /// <returns>An inverted grayscale presentation using a single band.</returns>
        public static RasterPresentation CreateInvertedGrayscalePresentation()
        {
            return new RasterPresentation(RasterPresentationModel.InvertedGrayscale, RasterColorSpace.None);
        }

        /// <summary>
        /// Creates a transparency presentation using a single band.
        /// </summary>
        /// <returns>A transparency presentation using a single band.</returns>
        public static RasterPresentation CreateTransparencyPresentation()
        {
            return new RasterPresentation(RasterPresentationModel.Transparency, RasterColorSpace.None);
        }

        /// <summary>
        /// Creates a pseudo-color presentation using a color map.
        /// </summary>
        /// <returns>A pseudo-color presentation using a color map.</returns>
        /// <exception cref="System.ArgumentNullException">The color map is null.</exception>
        public static RasterPresentation CreatePresudoColorPresentation(IDictionary<Int32, UInt32[]> colorMap)
        {
            return new RasterPresentation(RasterPresentationModel.PseudoColor, colorMap);
        }

        /// <summary>
        /// Creates a density slicing presentation using a color map.
        /// </summary>
        /// <returns>A density slicing presentation using a color map.</returns>
        /// <exception cref="System.ArgumentNullException">The color map is null.</exception>
        public static RasterPresentation CreateDencitySlicingPresentation(IDictionary<Int32, UInt32[]> colorMap)
        {
            return new RasterPresentation(RasterPresentationModel.DensitySlicing, colorMap);
        }

        #endregion
    }
}
