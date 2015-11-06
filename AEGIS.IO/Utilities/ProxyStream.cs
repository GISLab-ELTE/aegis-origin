using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELTE.AEGIS.IO.Utilities
{
    /// <summary>
    /// Works as a proxy for an underlying stream.
    /// </summary>
    public class ProxyStream : Stream
    {
        #region Constant fields

        /// <summary>
        /// The size of the storage units used for cashing.
        /// </summary>
        private const Int32 StorageSize = 10000; // the size of the byte arrays used (10 kB)

        #endregion

        #region Private types

        /// <summary>
        /// Represents the possible access types of the stream.
        /// </summary>
        private enum StreamAccessType
        {
            Readable,
            Writable,
            Undefined
        };

        #endregion

        #region Private fields

        /// <summary>
        /// The underlying stream.
        /// </summary>
        private Stream _underlyingStream;

        /// <summary>
        /// Defines whether the stream can be read/ written multiple times or not. 
        /// </summary>
        private Boolean _singleUse;

        /// <summary>
        /// Defines whether the instances has been disposed or not. 
        /// </summary>
        private Boolean _disposed;

        /// <summary>
        /// Defines whether proxy mode is forced.
        /// </summary>
        private Boolean _forced;

        /// <summary>
        /// The current position in the stream.
        /// </summary>
        private Int64 _currentPosition;

        /// <summary>
        /// The maximum position reached in the stream.
        /// </summary>
        private Int64 _maximumPosition;

        /// <summary>
        /// The position in the stream where Flush occured.
        /// </summary>
        private Int64 _flushPosition;

        /// <summary>
        /// The index of the storage unit where Flush occured.
        /// </summary>
        private Int64 _flushIndex;

        /// <summary>
        /// The size of the bit flag arrays.
        /// </summary>
        private Int32 _bitFlagSize;

        /// <summary>
        /// The access type of the stream.
        /// </summary>
        private StreamAccessType _accessType;

        /// <summary>
        /// The bit flag arrays.
        /// </summary>
        private Dictionary<Int64, Byte[]> _bitFlagArrays;

        /// <summary>
        /// The data containing arrays.
        /// </summary>
        private Dictionary<Int64, Byte[]> _byteArrays;

        /// <summary>
        /// A value indicating whether to dispose the underlying stream.
        /// </summary>
        private Boolean _disposeUnderlyingStream;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyStream" /> class.
        /// </summary>
        /// <param name="underlyingStream">The underlying stream.</param>
        /// <param name="singleUse">Defines whether the stream can be read/ written multiple times or not.</param>
        /// <param name="forced">A value indicating whether proxy mode is forced.</param>
        /// <param name="disposeUnderlyingStream">A value indicating whether to dispose the underlysing stream.</param>
        /// <exception cref="System.ArgumentNullException">The stream is null.</exception>
        /// <exception cref="System.NotSupportedException">The stream does not support reading and writing.</exception>
        public ProxyStream(Stream underlyingStream, Boolean singleUse = true, Boolean forced = false, Boolean disposeUnderlyingStream = true)
        {
            if (underlyingStream == null)
                throw new ArgumentNullException("stream", "The stream is null.");

            if (!underlyingStream.CanRead && !underlyingStream.CanWrite)
                throw new NotSupportedException("The stream does not support reading and writing.");

            _underlyingStream = underlyingStream;
            _singleUse = singleUse;
            _disposed = false;
            _forced = forced;
            _currentPosition = 0;
            _maximumPosition = 0;
            _flushPosition = 0;
            _flushIndex = -1;
            _bitFlagSize = Convert.ToInt32(Math.Ceiling((Double)(StorageSize) / 8.0));

            if (_underlyingStream.CanWrite && _underlyingStream.CanRead)
                _accessType = StreamAccessType.Undefined;
            else if (_underlyingStream.CanWrite && !_underlyingStream.CanRead)
                _accessType = StreamAccessType.Writable;
            else if (!_underlyingStream.CanWrite && _underlyingStream.CanRead)
                _accessType = StreamAccessType.Readable;

            _byteArrays = new Dictionary<Int64, Byte[]>();

            _bitFlagArrays = new Dictionary<Int64, Byte[]>();

            _disposeUnderlyingStream = disposeUnderlyingStream;
        }

        #endregion

        #region Stream properties

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override Int64 Position
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                if (_currentPosition != value)
                {
                    Seek(_currentPosition, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override Int64 Length
        {
            get { return _underlyingStream.Length; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override Boolean CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override Boolean CanWrite
        {
            get { return (_accessType == StreamAccessType.Writable || _accessType == StreamAccessType.Undefined) ? true : false; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override Boolean CanRead
        {
            get { return (_accessType == StreamAccessType.Readable || _accessType == StreamAccessType.Undefined) ? true : false; }
        }

        #endregion

        #region Stream methods

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">Method was called after the stream was closed.</exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            if (_disposed)
                throw new ObjectDisposedException("Method was called after the stream was closed.");

            if (!_forced && _underlyingStream.CanSeek)
                return _underlyingStream.Seek(offset, origin);

            switch (_accessType)
            {
                case StreamAccessType.Readable:
                    switch (origin)
                    {
                        case SeekOrigin.Begin:
                            ReadFromUnderlyingStream(offset, 0);
                            break;
                        case SeekOrigin.Current:
                            ReadFromUnderlyingStream(offset, _currentPosition);
                            break;
                        case SeekOrigin.End:
                            ReadFromUnderlyingStream(offset, _underlyingStream.Length);
                            break;
                    }
                    break;
                case StreamAccessType.Writable:
                case StreamAccessType.Undefined:
                    switch (origin)
                    {
                        case SeekOrigin.Begin:
                            _currentPosition = offset;
                            break;
                        case SeekOrigin.Current:
                            _currentPosition += offset;
                            break;
                        case SeekOrigin.End:
                            _currentPosition = offset + _underlyingStream.Length;
                            break;
                    }
                    break;
            }

            return _currentPosition;
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream, stores them in the cache and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// </returns>
        /// <exception cref="System.ObjectDisposedException">Method was called after the stream was closed.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer is null.</exception>
        /// <exception cref="System.ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The offset or count is negative.</exception>
        /// <exception cref="System.NotSupportedException">The stream does not support reading.</exception>
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            if (_disposed)
                throw new ObjectDisposedException("Method was called after the stream was closed.");

            if (!_forced && _underlyingStream.CanSeek && _underlyingStream.CanRead)
                return _underlyingStream.Read(buffer, offset, count);

            if (buffer == null)
                throw new ArgumentNullException("The buffer is null.");

            if (offset + count > buffer.Length)
                throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException("The offset or count is negative.");

            if (_accessType == StreamAccessType.Undefined)
                _accessType = StreamAccessType.Readable;

            if (_accessType != StreamAccessType.Readable)
                throw new NotSupportedException("The stream does not support reading.");

            if (_currentPosition >= _underlyingStream.Length)
                return 0;

            Int32 bytesToRead = (_currentPosition + count > _underlyingStream.Length) ? (Int32)(_underlyingStream.Length - _currentPosition) : count;

            ReadFromUnderlyingStream(_currentPosition, bytesToRead);
            _currentPosition -= bytesToRead;

            if (_singleUse)
                CheckIfSet(bytesToRead);

            HashSet<Int64> indexesToCheck = new HashSet<Int64>();

            for (Int32 bufferIndex = 0; bufferIndex < bytesToRead; bufferIndex++)
            {
                Int64 index = _currentPosition / StorageSize;
                buffer[bufferIndex + offset] = _byteArrays[index][_currentPosition % StorageSize];

                indexesToCheck.Add(index);

                if (_singleUse)
                    SetBitFlag(index);

                _currentPosition++;
            }

            if (_singleUse)
                RemoveCachedData(indexesToCheck);

            return bytesToRead;
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.ObjectDisposedException">Method was called after the stream was closed.</exception>
        /// <exception cref="System.ArgumentNullException">The buffer is null</exception>
        /// <exception cref="System.ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The offset or count is negative.</exception>
        /// <exception cref="System.NotSupportedException">The stream does not support writing.</exception>
        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            if (_disposed)
                throw new ObjectDisposedException("Method was called after the stream was closed.");

            if (!_forced && _underlyingStream.CanSeek)
            {
                _underlyingStream.Write(buffer, offset, count);
                return;
            }

            if (buffer == null)
                throw new ArgumentNullException("The buffer is null");

            if (offset + count > buffer.Length)
                throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException("The offset or count is negative.");

            if (_accessType == StreamAccessType.Undefined)
                _accessType = StreamAccessType.Writable;

            if (_accessType != StreamAccessType.Writable)
                throw new NotSupportedException("The stream does not support writing.");

            CheckIfSet(count);

            for (Int32 bufferIndex = 0; bufferIndex < count; bufferIndex++)
            {
                Int64 index = _currentPosition / StorageSize;

                if (!_byteArrays.ContainsKey(index))
                    _byteArrays.Add(index, new Byte[StorageSize]);

                _byteArrays[index][_currentPosition % StorageSize] = buffer[bufferIndex + offset];

                SetBitFlag(index);

                _currentPosition++;
            }
            if (_singleUse)
                RemoveCachedData(null);

            if (_currentPosition > _maximumPosition)
                _maximumPosition = _currentPosition;
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="System.ObjectDisposedException">Method was called after the stream was closed.</exception>
        /// <exception cref="System.InvalidOperationException">The stream is not writable or does not support seeking.</exception>
        public override void SetLength(Int64 value)
        {
            if (_disposed)
                throw new ObjectDisposedException("Method was called after the stream was closed.");

            if ((_accessType == StreamAccessType.Writable || _accessType == StreamAccessType.Undefined) && _underlyingStream.CanSeek)
            {
                _accessType = StreamAccessType.Writable;
                _underlyingStream.SetLength(value);
            }
            else
                throw new InvalidOperationException("The stream is not writable or does not support seeking.");
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying stream.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The stream is not writable.</exception>
        public override void Flush()
        {
            if (_accessType == StreamAccessType.Writable || _accessType == StreamAccessType.Undefined)
            {
                _accessType = StreamAccessType.Writable;
                FlushIntoUnderlyingStream();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the necessary bytes from the underlying stream and stores them.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="position">The position depending on the seek origin.</param>
        /// <exception cref="System.IO.IOException">Error occured during stream reading.</exception>
        private void ReadFromUnderlyingStream(Int64 offset, Int64 position)
        {
            _currentPosition = offset + position;

            if (_currentPosition > _maximumPosition)
            {
                Int64 numberOfBytes = _currentPosition - _maximumPosition;

                Byte[] bytesToCopy = new Byte[numberOfBytes];
                try
                {
                    _underlyingStream.Read(bytesToCopy, 0, (Int32)numberOfBytes);
                }
                catch (Exception ex)
                {
                    throw new IOException("Error occured during stream reading.", ex);
                }

                for (Int64 byteNumber = 0; byteNumber < numberOfBytes; byteNumber++)
                {
                    Int64 index = _maximumPosition / StorageSize;
                    if (!_byteArrays.ContainsKey(index))
                        _byteArrays.Add(index, new Byte[StorageSize]);

                    Array.Copy(bytesToCopy, byteNumber, _byteArrays[index], (Int32)(_maximumPosition % StorageSize), 1);
                    _maximumPosition++;
                }
            }
        }

        /// <summary>
        /// Sets the bit flag for the read data.
        /// </summary>
        /// <param name="index">The index of the data.</param>
        private void SetBitFlag(Int64 index)
        {
            if (!_bitFlagArrays.ContainsKey(index))
                _bitFlagArrays.Add(index, new Byte[_bitFlagSize]);

            Byte actualByte = _bitFlagArrays[index][(_currentPosition % StorageSize) / 8];
            _bitFlagArrays[index][(_currentPosition % StorageSize) / 8] = (Byte)(actualByte | (1 << (7 - (Int32)(_currentPosition % 8))));
        }

        /// <summary>
        /// Checks if the data has already been read of written.
        /// </summary>
        /// <param name="count">The number of bytes.</param>
        /// <exception cref="System.InvalidOperationException">Since caching is used, reading or writing elements multiple times is forbidden.</exception>
        private void CheckIfSet(Int64 count)
        {
            Int64 actualPositionInStream = _currentPosition;
            for (Int32 i = 0; i < count; i++)
            {
                Int64 index = actualPositionInStream / StorageSize;
                if ((_accessType == StreamAccessType.Readable && ((_bitFlagArrays.ContainsKey(index) &&
                        (_bitFlagArrays[index][(actualPositionInStream % StorageSize) / 8] & (1 << (7 - (Int32)(actualPositionInStream % 8)))) != 0) ||
                            !_byteArrays.ContainsKey(index))) ||
                    (_accessType == StreamAccessType.Writable && (_singleUse && (_bitFlagArrays.ContainsKey(index) &&
                        (_bitFlagArrays[index][(actualPositionInStream % StorageSize) / 8] & (1 << (7 - (Int32)(actualPositionInStream % 8)))) != 0) ||
                             _flushIndex > index || _flushPosition > actualPositionInStream)))
                {
                    throw new InvalidOperationException("Since caching is used, reading or writing elements multiple times is forbidden.");
                }
                actualPositionInStream++;
            }
        }

        /// <summary>
        /// Writes the stored data in the underlying stream in case of Flush or Dispose.
        /// </summary>
        private void FlushIntoUnderlyingStream()
        {
            // Flush until the current position or the last position where data has been cached
            Int64 position = 0;
            if (_maximumPosition > _currentPosition)
                position = _maximumPosition;
            else
                position = _currentPosition;

            Int32 numberOfUnitsToWrite = Convert.ToInt32(Math.Ceiling((Double)position / StorageSize));
            for (Int64 index = _flushIndex + 1; index < numberOfUnitsToWrite; index++)
            {
                try
                {
                    Int32 count = (Int32)((index + 1) * StorageSize < position ? StorageSize : position - index * StorageSize);
                    Int32 offset = 0;

                    // if a Flush occured previously, the bytes at the beginning of the last byte array have to be skipped
                    if (index == _flushPosition / StorageSize)
                    {
                        count -= (Int32)(_flushPosition % StorageSize);
                        offset = (Int32)(_flushPosition % StorageSize);
                    }

                    if (count < 0)
                        count = 0;

                    if (_byteArrays.ContainsKey(index))
                    {
                        _underlyingStream.Write(_byteArrays[index], offset, count);

                        // only the fully used byte arrays have to be removed
                        if (count == StorageSize)
                        {
                            _byteArrays.Remove(index);
                            _bitFlagArrays.Remove(index);
                        }
                    }
                    else
                        _underlyingStream.Write(new Byte[StorageSize], offset, count);
                }
                catch (Exception ex)
                {
                    throw new IOException("Error occured during writing into the stream.", ex);
                }
            }
            _flushIndex = position / StorageSize - 1;
            _flushPosition = position;
        }

        /// <summary>
        /// Removes the unnecessary data from the cache if needed.
        /// </summary>
        /// <param name="keysToCheck">The keys to check.</param>
        private void RemoveCachedData(HashSet<Int64> keysToCheck)
        {
            if (_accessType == StreamAccessType.Readable)
            {
                foreach (Int64 key in keysToCheck)
                {
                    if (Array.TrueForAll(_bitFlagArrays[key], value => value == 255))
                    {
                        _bitFlagArrays.Remove(key);
                        _byteArrays.Remove(key);
                    }
                }
            }
            else if (_accessType == StreamAccessType.Writable)
            {
                List<Int64> keysToRemove = new List<Int64>();
                keysToRemove = SelectKeysToRemove();
                WriteCachedDataIntoUnderlyingStream(keysToRemove);
            }
        }

        /// <summary>
        /// Selects the keys that have to be removed from the data container.
        /// </summary>
        /// <returns>The list of keys.</returns>
        private List<Int64> SelectKeysToRemove()
        {
            List<Int64> keysToRemove = new List<Int64>();
            List<Int64> sortedKeys = _byteArrays.Keys.ToList();
            sortedKeys.Sort();

            foreach (Int64 index in sortedKeys)
            {
                if (_flushPosition % StorageSize != 0 && index == _flushIndex + 1)
                {
                    Boolean trueForAll = true;
                    for (Int64 bitIndex = _flushPosition % StorageSize; bitIndex < StorageSize; bitIndex++)
                    {
                        if ((_bitFlagArrays[index][bitIndex / 8] & (1 << (7 - (Int32)bitIndex % 8))) == 0)
                        {
                            trueForAll = false;
                            break;
                        }
                    }
                    if (trueForAll)
                        keysToRemove.Add(index);
                    else
                        break;
                }
                else
                {
                    if (Array.TrueForAll(_bitFlagArrays[index], value => value == 255))
                        keysToRemove.Add(index);
                    else
                        break;
                }
            }
            return keysToRemove;
        }

        /// <summary>
        /// Writes the cached data into underlying stream.
        /// </summary>
        /// <param name="keysToRemove">The keys to remove.</param>
        private void WriteCachedDataIntoUnderlyingStream(List<Int64> keysToRemove)
        {
            foreach (Int64 index in keysToRemove)
            {
                if (index == _flushIndex + 1)
                    _underlyingStream.Write(_byteArrays[index], (Int32)(_flushPosition % StorageSize), StorageSize - (Int32)(_flushPosition % StorageSize));
                else
                    _underlyingStream.Write(_byteArrays[index], 0, StorageSize);

                _byteArrays.Remove(index);
                _bitFlagArrays.Remove(index);
            }

            if (keysToRemove.Count != 0)
                _flushIndex = keysToRemove.Max();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected override void Dispose(Boolean disposing)
        {
            _disposed = true;

            if (disposing)
            {
                if (_accessType == StreamAccessType.Writable)
                    FlushIntoUnderlyingStream();

                if (_disposeUnderlyingStream)
                    _underlyingStream.Dispose();
            }
        }

        #endregion
    }
}

