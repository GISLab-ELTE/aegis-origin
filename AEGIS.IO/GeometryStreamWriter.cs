/// <copyright file="GeometryStreamWriter.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.IO.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ELTE.AEGIS.IO
{
    /// <summary>
    /// Represents a base type for stream based geometry writing.
    /// </summary>
    public abstract class GeometryStreamWriter : IDisposable
    {
        #region Private fields

        /// <summary>
        /// Defines an empty collection of parameters. This field is read-only.
        /// </summary>
        private readonly static Dictionary<GeometryStreamParameter, Object> EmptyParameters = new Dictionary<GeometryStreamParameter, Object>();

        private readonly GeometryStreamFormat _format;
        private readonly IDictionary<GeometryStreamParameter, Object> _parameters;
        private readonly Uri _path;
        private Boolean _disposed;

        #endregion

        #region Protected fields

        protected readonly Stream _baseStream;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the format of the geometry stream.
        /// </summary>
        /// <value>The format of the geometry stream.</value>
        public GeometryStreamFormat Format { get { return _format; } }

        /// <summary>
        /// Gets the parameters of the reader.
        /// </summary>
        /// <value>The parameters of the reader stored as key/value pairs.</value>
        public IDictionary<GeometryStreamParameter, Object> Parameters { get { return new ReadOnlyDictionary<GeometryStreamParameter, Object>(_parameters != null ? _parameters : EmptyParameters); } }

        /// <summary>
        /// Gets the path of the data.
        /// </summary>
        /// <value>The full path of the data.</value>
        public Uri Path { get { return _path; } }
        
        /// <summary>
        /// Gets the undelying stream.
        /// </summary>
        /// <value>The underlying stream.</value>
        public Stream BaseStream { get { return _baseStream; } }

        #endregion

        #region Constructors and destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream writing.
        /// </exception>
        protected GeometryStreamWriter(String path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)            
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (format == null)
                throw new ArgumentNullException("format", "The format is null.");
            if (parameters == null && format.Parameters != null && format.Parameters.Length > 0)
                throw new ArgumentNullException("parameters", "The format requires parameters which are not specified.");
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("The path is empty.", "path");
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out _path))
                throw new ArgumentException("The path is invalid.", "path");

            if (parameters != null && format.Parameters != null)
            {
                foreach (GeometryStreamParameter parameter in format.Parameters)
                {
                    // check parameter existence
                    if (!parameter.IsOptional && (!parameters.ContainsKey(parameter) || parameters[parameter] == null))
                        throw new ArgumentException("The parameters do not contain a required parameter value (" + parameter.Name + ").", "parameters");

                    if (parameters.ContainsKey(parameter))
                    {
                        // check parameter type
                        if (!(parameter.Type.GetInterfaces().Contains(typeof(IConvertible)) && parameters[parameter] is IConvertible) &&
                            !parameter.Type.Equals(parameters[parameter].GetType()) &&
                            !parameters[parameter].GetType().IsSubclassOf(parameter.Type) &&
                            !parameters[parameter].GetType().GetInterfaces().Contains(parameter.Type))
                            throw new ArgumentException("The type of a parameter value (" + parameter.Name + ") does not match the type specified by the method.", "parameters");

                        // check parameter value
                        if (!parameter.IsValid(parameters[parameter]))
                            throw new ArgumentException("The parameter value (" + parameter.Name + ") does not satisfy the conditions of the parameter.", "parameters");
                    }
                }
            }

            _format = format;
            _parameters = parameters;
            _disposed = false;

            try
            {
                _baseStream = FileSystem.GetFileSystemForPath(_path).CreateFile(_path.AbsolutePath, true);
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream opening.", ex);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The path is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The path is empty.
        /// or
        /// The path is invalid.
        /// or
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// Exception occured during stream opening.
        /// or
        /// Exception occured during stream writing.
        /// </exception>
        protected GeometryStreamWriter(Uri path, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
        {
            if (path == null)
                throw new ArgumentNullException("path", "The path is null.");
            if (format == null)
                throw new ArgumentNullException("format", "The format is null.");
            if (parameters == null && format.Parameters != null && format.Parameters.Length > 0)
                throw new ArgumentNullException("parameters", "The format requires parameters which are not specified.");
            if (String.IsNullOrEmpty(path.AbsolutePath))
                throw new ArgumentException("The path is empty.", "path");

            if (parameters != null && format.Parameters != null)
            {
                foreach (GeometryStreamParameter parameter in format.Parameters)
                {
                    // check parameter existence
                    if (!parameter.IsOptional && (!parameters.ContainsKey(parameter) || parameters[parameter] == null))
                        throw new ArgumentException("The parameters do not contain a required parameter value (" + parameter.Name + ").", "parameters");

                    if (parameters.ContainsKey(parameter))
                    {
                        // check parameter type
                        if (!(parameter.Type.GetInterfaces().Contains(typeof(IConvertible)) && parameters[parameter] is IConvertible) &&
                            !parameter.Type.Equals(parameters[parameter].GetType()) &&
                            !parameters[parameter].GetType().IsSubclassOf(parameter.Type) &&
                            !parameters[parameter].GetType().GetInterfaces().Contains(parameter.Type))
                            throw new ArgumentException("The type of a parameter value (" + parameter.Name + ") does not match the type specified by the method.", "parameters");

                        // check parameter value
                        if (!parameter.IsValid(parameters[parameter]))
                            throw new ArgumentException("The parameter value (" + parameter.Name + ") does not satisfy the conditions of the parameter.", "parameters");
                    }
                }
            }

            _format = format;
            _parameters = parameters;
            _path = path;
            _disposed = false;

            try
            {
                _baseStream = FileSystem.GetFileSystemForPath(_path).CreateFile(_path.AbsolutePath, true);
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream opening.", ex);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometryStreamWriter" /> class.
        /// </summary>
        /// <param name="path">The file path to be written.</param>
        /// <param name="format">The format.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The stream is null.
        /// or
        /// The format is null.
        /// or
        /// The format requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter value does not match the type specified by the format.
        /// </exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream writing.</exception>
        protected GeometryStreamWriter(Stream stream, GeometryStreamFormat format, IDictionary<GeometryStreamParameter, Object> parameters)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "The stream is null.");
            if (format == null)
                throw new ArgumentNullException("format", "The format is null.");
            if (parameters == null && format.Parameters != null && format.Parameters.Length > 0)
                throw new ArgumentNullException("parameters", "The format requires parameters which are not specified.");

            if (parameters != null && format.Parameters != null)
            {
                foreach (GeometryStreamParameter parameter in format.Parameters)
                {
                    // check parameter existence
                    if (!parameter.IsOptional && (!parameters.ContainsKey(parameter) || parameters[parameter] == null))
                        throw new ArgumentException("The parameters do not contain a required parameter value (" + parameter.Name + ").", "parameters");

                    if (parameters.ContainsKey(parameter))
                    {
                        // check parameter type
                        if (!(parameter.Type.GetInterfaces().Contains(typeof(IConvertible)) && parameters[parameter] is IConvertible) &&
                            !parameter.Type.Equals(parameters[parameter].GetType()) &&
                            !parameters[parameter].GetType().IsSubclassOf(parameter.Type) &&
                            !parameters[parameter].GetType().GetInterfaces().Contains(parameter.Type))
                            throw new ArgumentException("The type of a parameter value (" + parameter.Name + ") does not match the type specified by the method.", "parameters");

                        // check parameter value
                        if (!parameter.IsValid(parameters[parameter]))
                            throw new ArgumentException("The parameter value (" + parameter.Name + ") does not satisfy the conditions of the parameter.", "parameters");
                    }
                }
            }

            _format = format;
            _parameters = parameters;
            _baseStream = stream;
            _disposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GeometryStreamWriter" /> class.
        /// </summary>
        ~GeometryStreamWriter()
        {
            Dispose(false);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry to be written.</param>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The geometry is not supported by the format.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream writing.</exception>
        public void Write(IGeometry geometry)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");
            if (_format.SupportedGeometries.Contains(geometry.GetType()))
                throw new ArgumentException("The geometry is not supported by the format.", "geometry");

            try
            {
                ApplyWriteGeometry(geometry);
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream writing.", ex);
            }
        }
        /// <summary>
        /// Writes the specified geometries.
        /// </summary>
        /// <param name="geometries">The geometries to be written.</param>
        /// <exception cref="System.ObjectDisposedException">Object is disposed.</exception>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">One or more of the geometries is not supported by the format.</exception>
        /// <exception cref="System.IO.IOException">Exception occured during stream writing.</exception>
        public void Write(IEnumerable<IGeometry> geometries)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (geometries == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            try
            {
                foreach (IGeometry geometry in geometries)
                {
                    if (geometry == null)
                        continue;
                    if (_format.SupportedGeometries.Contains(geometry.GetType()))
                        throw new ArgumentException("One or more of the geometries is not supported by the format.", "geometries");

                    ApplyWriteGeometry(geometry);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Exception occured during stream writing.", ex);
            }
        }

        /// <summary>
        /// Closes the writer and the underlying stream, and releases any system resources associated with the writer.
        /// </summary>
        public virtual void Close()
        {
            Dispose();
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Apply the write operation for the specified geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        protected abstract void ApplyWriteGeometry(IGeometry geometry);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether disposing is performed on the object.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            _disposed = true;

            if (disposing)
            {
                _baseStream.Dispose();
                if (_parameters != null)
                    _parameters.Clear();
            }
        }

        #endregion
    }
}
