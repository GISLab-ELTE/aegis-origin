/// <copyright file="MetadataCollection.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ELTE.AEGIS.Metadata
{
    /// <summary>
    /// Represents a metadata collection.
    /// </summary>
    public class MetadataCollection : Dictionary<String, Object>, IMetadataCollection
    {
        #region Private fields

        private readonly IMetadataFactory _factory;

        #endregion

        #region IFactoryProduct properties

        public IMetadataFactory Factory { get { return _factory; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        public MetadataCollection() : this((IMetadataFactory)null) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="MetadataCollection" /> can contain.</param>
        public MetadataCollection(Int32 capacity) : this(capacity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{String, Object}" /> whose elements are copied to the new <see cref="MetadataCollection" />.</param>
        public MetadataCollection(IDictionary<String, Object> source) : this(source, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IMetadataCollection" /> whose elements are copied to the new <see cref="MetadataCollection" />.</param>
        public MetadataCollection(IMetadataCollection source)
            : this(source, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="factory">The factory of the collection.</param>
        public MetadataCollection(IMetadataFactory factory) : base() 
        {
            _factory = factory ?? AEGIS.Factory.DefaultInstance<MetadataFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="MetadataCollection" /> can contain.</param>
        /// <param name="factory">The factory of the collection.</param>
        public MetadataCollection(Int32 capacity, IMetadataFactory factory) : base(capacity) 
        {
            _factory = factory ?? AEGIS.Factory.DefaultInstance<MetadataFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IDictionary{String, Object}" /> whose elements are copied to the new <see cref="MetadataCollection" />.</param>
        /// <param name="factory">The factory of the collection.</param>
        public MetadataCollection(IDictionary<String, Object> source, IMetadataFactory factory)
            : base(source) 
        {
            _factory = factory ?? AEGIS.Factory.DefaultInstance<MetadataFactory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataCollection" /> class.
        /// </summary>
        /// <param name="source">The <see cref="IMetadataCollection" /> whose elements are copied to the new <see cref="MetadataCollection" />.</param>
        /// <param name="factory">The factory of the collection.</param>
        public MetadataCollection(IMetadataCollection source, IMetadataFactory factory)
            : base(source)
        {
            _factory = factory ?? AEGIS.Factory.DefaultInstance<MetadataFactory>();
        }

        #endregion

        #region ICloneable methods

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Object Clone()
        {
            return new MetadataCollection(this, _factory);
        }

        #endregion
    }
}
