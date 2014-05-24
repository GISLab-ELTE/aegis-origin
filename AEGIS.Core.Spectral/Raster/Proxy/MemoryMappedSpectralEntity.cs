using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;

namespace ELTE.AEGIS.Raster.Proxy
{
    class MemoryMappedSpectralEntity: ISpectralEntity
    {
            #region Private fields

            private readonly Int32 _spectralResolution;
            private readonly Int32 _numberOfColumns;
            private readonly Int32 _numberOfRows;
            private readonly Int32[] _radiometricResolutions;
            private readonly RasterRepresentation _representation;

            #endregion

            #region ISpectralEntity properties

            /// <summary>
            /// Gets the number of columns.
            /// </summary>
            /// <value>The number of spectral values contained in a row.</value>
            public Int32 NumberOfColumns { get { return _numberOfColumns; } }

            /// <summary>
            /// Gets the number of rows.
            /// </summary>
            /// <value>The number of spectral values contained in a column.</value>
            public Int32 NumberOfRows { get { return _numberOfRows; } }

            /// <summary>
            /// Gets the spectral resolution of the dataset.
            /// </summary>
            /// <value>The number of spectral bands contained in the dataset.</value>
            public Int32 SpectralResolution { get { return _spectralResolution; } }

            /// <summary>
            /// Gets the radiometric resolutions of the bands in the raster.
            /// </summary>
            /// <value>The list containing the radiometric resolution of each band in the raster.</value>
            public IList<Int32> RadiometricResolutions { get { return _radiometricResolutions; } }

            /// <summary>
            /// Gets a value indicating whether the dataset is readable.
            /// </summary>
            /// <value><c>true</c> if the dataset is readable; otherwise, <c>false</c>.</value>
            public Boolean IsReadable { get { return true; } }

            /// <summary>
            /// Gets a value indicating whether the dataset is writable.
            /// </summary>
            /// <value><c>true</c> if the dataset is writable; otherwise, <c>false</c>.</value>
            public Boolean IsWritable { get { return true; } }

            /// <summary>
            /// Gets the representation of the dataset.
            /// </summary>
            /// <value>The representation of the dataset.</value>
            public RasterRepresentation Representation { get { return _representation; } }

            /// <summary>
            /// Gets the supported read/write orders.
            /// </summary>
            /// <value>The list of supported read/write orders.</value>
            public IList<SpectralDataOrder> SupportedOrders { get { return Array.AsReadOnly(_supportedOrders); } }

            #endregion

            #region Constructor

            public MemoryMappedSpectralEntity(Int32 rowNumber, Int32 columnNumber, String path)
            {
                memoryMappedFile = MemoryMappedFile.CreateNew("first", (rowNumber+1)*(columnNumber+1));
            }

            #endregion

            #region ISpectralEntity methods for reading integer value

            public UInt32 ReadValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
            {
                return 0;
            }
            
            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return null;
            }

            public UInt32[] ReadValueSequence(Int32 startIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }

            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return null;
            }

            public UInt32[] ReadValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }


            #endregion

            #region ISpectralEntity methods for reading float value

            public Double ReadFloatValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex) 
            {
                return 0;
            }

            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues)
            {
                return null;
            }

            public Double[] ReadFloatValueSequence(Int32 startIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }

            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues)
            {
                return null;
            }

            public Double[] ReadFloatValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Int32 numberOfValues, SpectralDataOrder readOrder)
            {
                return null;
            }

            #endregion

            #region ISpectralEntity methods for writing integer values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the dataset.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32 spectralValue)
            {

            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues)
            {

            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, UInt32[] spectralValues, SpectralDataOrder writeOrder)
            {

            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues)
            {

            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, UInt32[] spectralValues, SpectralDataOrder writeOrder)
            {

            }

            #endregion

            #region ISpectralEntity methods for writing integer values (explicit)

            /// <summary>
            /// Writes the specified spectral value to the dataset.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="bandIndex">The band index.</param>
            /// <param name="spectralValue">The spectral value.</param>
            /// <returns>The spectral value at the specified index.</returns>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValue(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double spectralValue)
            {

            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, Double[] spectralValues)
            {

            }

            /// <summary>
            /// Writes the specified spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The row index.</param>
            /// <param name="columnIndex">The column index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 startIndex, Double[] spectralValues, SpectralDataOrder writeOrder)
            {

            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the default order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues)
            {

            }

            /// <summary>
            /// Writes a sequence of spectral values to the dataset in the specified order.
            /// </summary>
            /// <param name="rowIndex">The starting row index.</param>
            /// <param name="columnIndex">The starting column index.</param>
            /// <param name="bandIndex">The starting band index.</param>
            /// <param name="spectralValues">The spectral values.</param>
            /// <param name="writeOrder">The writing order.</param>
            /// <exception cref="System.NotSupportedException">The dataset is not writable.</exception>
            void ISpectralEntity.WriteValueSequence(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex, Double[] spectralValues, SpectralDataOrder writeOrder)
            {

            }

            #endregion

            private static SpectralDataOrder[] _supportedOrders;

            private MemoryMappedFile memoryMappedFile;
        

    }
}
