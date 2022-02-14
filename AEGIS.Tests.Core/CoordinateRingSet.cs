/// <copyright file="CoorindateRingSet.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests
{
    /// <summary>
    /// Defines a set of <see cref="CoordinateRing"/>, enabling the unordered equality comparison of two sets of coordinate rings.
    /// </summary>
    /// <remarks>This type purely serves testing purposes.</remarks>
    internal class CoordinateRingSet : IEquatable<CoordinateRingSet>
        // The IEnumerable interface in not implemented on purpose, because it would override the usage of IEquatable in NUnit.
    {
        #region Private fields

        /// <summary>
        /// The collection of <see cref="CoordinateRing"/>.
        /// </summary>
        private readonly IList<CoordinateRing> _collection;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the collection of <see cref="CoordinateRing"/>.
        /// </summary>
        /// <value>The collection.</value>
        public IList<CoordinateRing> Collection
        {
            get { return _collection; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateRingSet"/> class.
        /// </summary>
        /// <param name="collection">The collection of <see cref="CoordinateRing"/> to use.</param>
        public CoordinateRingSet(IList<CoordinateRing> collection)
        {
            _collection = collection;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// <code>true</code> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <code>false</code>.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public Boolean Equals(CoordinateRingSet other)
        {
            if (Collection.Count != other.Collection.Count)
                return false;

            List<CoordinateRing> otherCollection = new List<CoordinateRing>(other.Collection);

            foreach (CoordinateRing ring in Collection)
                if (!otherCollection.Remove(ring))
                    return false;

            return true;
        }

        #endregion
    }

    /// <summary>
    /// Defines extension methods for the <see cref="CoordinateRing"/> and <see cref="CoordinateRingSet"/> classes.
    /// </summary>
    internal static class CoordinateRingSetExtensions
    {
        #region Public extension methods

        /// <summary>
        /// Converts a coordinate list into a <see cref="CoordinateRing"/>.
        /// </summary>
        /// <param name="value">The input coordinates.</param>
        /// <returns>The coordinate ring.</returns>
        public static CoordinateRing ToRing(this IEnumerable<Coordinate> value)
        {
            return new CoordinateRing(value);
        }

        /// <summary>
        /// Converts a coordinate list into a <see cref="CoordinateRingSet"/>.
        /// </summary>
        /// <param name="value">The input coordinates.</param>
        /// <returns>The coordinate ring set.</returns>
        public static CoordinateRingSet ToRingSet(this IEnumerable<Coordinate> value)
        {
            return new CoordinateRingSet(new[] { new CoordinateRing(value) });
        }

        /// <summary>
        /// Converts a sequence of coordinate lists into a sequence of <see cref="CoordinateRingSet"/>.
        /// </summary>
        /// <param name="value">The input coordinates.</param>
        /// <returns>The coordinate ring set.</returns>
        public static CoordinateRingSet ToRingSet(this IEnumerable<IEnumerable<Coordinate>> value)
        {
            return new CoordinateRingSet(value.Select(ring => new CoordinateRing(ring)).ToList());
        }

        /// <summary>
        /// Converts a <see cref="CoordinateRing"/> into a <see cref="CoordinateRingSet"/>.
        /// </summary>
        /// <param name="value">The input coordinate ring.</param>
        /// <returns>The coordinate ring set.</returns>
        public static CoordinateRingSet ToRingSet(this CoordinateRing value)
        {
            return new CoordinateRingSet(new[] { value });
        }

        /// <summary>
        /// Converts a list of <see cref="CoordinateRing"/> into a <see cref="CoordinateRingSet"/>.
        /// </summary>
        /// <param name="value">The input coordinate rings.</param>
        /// <returns>The coordinate ring set.</returns>
        public static CoordinateRingSet ToRingSet(this IList<CoordinateRing> value)
        {
            return new CoordinateRingSet(value);
        }

        #endregion
    }
}