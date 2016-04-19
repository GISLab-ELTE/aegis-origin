using System;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a type specifying the dimensions of a raster.
    /// </summary>
    public class RasterDimensions
    {
        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        /// <value>The number of spectral values contained in a row.</value>
        public Int32 NumberOfColumns { get; private set; }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        /// <value>The number of spectral values contained in a column.</value>
        public Int32 NumberOfRows { get; private set; }

        /// <summary>
        /// Gets the number of spectral bands.
        /// </summary>
        /// <value>The number of spectral bands contained in the raster.</value>
        public Int32 NumberOfBands { get; private set; }

        /// <summary>
        /// Gets the radiometric resolution of the bands in the raster.
        /// </summary>
        /// <value>The radiometric resolution of the bands in the raster.</value>
        public Int32 RadiometricResolution { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterDimensions" /> class.
        /// </summary>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than 64.
        /// </exception>
        public RasterDimensions(Int32 numberOfRows, Int32 numberOfColumns, Int32 numberOfBands, Int32 radiometricResolution)
        {
            if (numberOfBands < 0)
                throw new ArgumentOutOfRangeException("numberOfBands", "The number of bands is less than 1.");
            if (numberOfRows < 0)
                throw new ArgumentOutOfRangeException("numberOfRows", "The number of rows is less than 0.");
            if (numberOfColumns < 0)
                throw new ArgumentOutOfRangeException("numberOfColumns", "The number of columns is less than 0.");
            if (radiometricResolution < 1)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is less than 1.");
            if (radiometricResolution > 64)
                throw new ArgumentOutOfRangeException("radiometricResolution", "The radiometric resolution is greater than 64.");

            NumberOfBands = numberOfBands;
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;
            RadiometricResolution = radiometricResolution;
        }
    }
}
