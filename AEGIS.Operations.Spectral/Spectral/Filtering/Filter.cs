/// <copyright file="Filter.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Operations.Spectral.Filtering
{
    /// <summary>
    /// Represents a raster filter.
    /// </summary>
    /// <remarks>
    /// The filter consists of a kernel (also known as convolution matrix or mask), a factor and an offset scalar. Depending on the element values, a kernel can cause a wide range of effects.
    /// </remarks>
    public class Filter
    {
        #region Private fields

        /// <summary>
        /// The factor of the filter.
        /// </summary>
        protected Double _factor;

        /// <summary>
        /// The kernel of the filter.
        /// </summary>
        protected Matrix _kernel;

        /// <summary>
        /// The offset of the filter.
        /// </summary>
        protected Double _offset;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the radius of the kernel.
        /// </summary>
        /// <value>The radius of the filter.</value>
        public Int32 Radius { get { return _kernel.NumberOfColumns / 2; } }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>The kernel.</value>
        public Matrix Kernel { get { return _kernel; } }

        /// <summary>
        /// Gets the factor.
        /// </summary>
        /// <value>The factor.</value>
        public Double Factor { get { return _factor; } }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public Double Offset { get { return _offset; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <exception cref="System.ArgumentNullException">The kernel is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The kernel is not rectangular.
        /// or
        /// The kernel is not odd size.
        /// </exception>
        public Filter(Matrix kernel)
            : this(kernel, 1, 0)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="offset">The offset.</param>
        /// <exception cref="System.ArgumentNullException">The kernel is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The kernel is not rectangular.
        /// or
        /// The kernel is not odd size.
        /// </exception>
        public Filter(Matrix kernel, Double factor, Double offset)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel", "The kernel is null.");
            if (kernel.NumberOfColumns != kernel.NumberOfRows)
                throw new ArgumentException("The kernel is not rectangular.", "kernel");
            if (kernel.NumberOfColumns % 2 == 0)
                throw new ArgumentException("The kernel is even size.", "kernel");

            _kernel = kernel;
            _factor = factor;
            _offset = offset;
        }

        #endregion
    }
}
