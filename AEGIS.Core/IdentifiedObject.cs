﻿/// <copyright file="IdentifiedObject.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents an identified object.
    /// </summary>
    [Serializable]
    public abstract class IdentifiedObject : IEquatable<IdentifiedObject>
    {
        #region Protected fields

        protected readonly String _identifier;
        protected String _name;
        protected String _remarks;
        protected String[] _aliases;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>An identifier which references elsewhere the object's defining information.</value>
        public String Identifier { get { return _identifier; } }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <value>The authority responsible for the object if provided; otherwise, <c>Empty</c>.</value>
        public String Authority { get { return _identifier.Contains("::") ? _identifier.Substring(0, _identifier.IndexOf("::")) : String.Empty; } }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code by which the object is identified in the authority's domain if provided; otherwise, <c>0</c>.</value>
        public Int32 Code 
        { 
            get 
            { 
                String codeString = _identifier.Contains("::") ? _identifier.Substring(_identifier.LastIndexOf("::")) : _identifier;
                Int32 code;

                return (Int32.TryParse(codeString, out code)) ? code : 0;
            } 
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The primary name by which this object is identified.</value>
        public String Name { get { return _name ?? String.Empty; } }

        /// <summary>
        /// Gets the remarks.
        /// </summary>
        /// <value>Comments on or information about this object, including data source information.</value>
        public String Remarks { get { return _remarks; } }

        /// <summary>
        /// Gets the aliasas that can also be used for naming purposes.
        /// </summary>
        /// <value>An alternative names by which this object is identified.</value>
        public String[] Aliases { get { return _aliases; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.IdentifiedObject" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected IdentifiedObject(String identifier, String name)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");

            _identifier = identifier;
            _name = name;
            _remarks = null;
            _aliases = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ELTE.AEGIS.IdentifiedObject" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="remarks">The remarks.</param>
        /// <param name="aliases">The aliases.</param>
        /// <exception cref="System.ArgumentNullException">The identifier is null.</exception>
        protected IdentifiedObject(String identifier, String name, String remarks, String[] aliases)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier", "The identifier is null.");

            _identifier = identifier;
            _name = name;
            _remarks = remarks;
            _aliases = aliases;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Determines whether the specified <see cref="IdentifiedObject" /> is equal to the current <see cref="IdentifiedObject" />.
        /// </summary>
        /// <param name="another">The <see cref="IdentifiedObject" /> to compare with the current <see cref="IdentifiedObject" />.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="IdentifiedObject" /> is equal to the current <see cref="IdentifiedObject" />; otherwise, <c>false</c>.
        /// </returns>
        public virtual Boolean Equals(IdentifiedObject another)
        {
            if (ReferenceEquals(null, another)) return false;
            if (ReferenceEquals(this, another)) return true;

            return _identifier.Equals(another._identifier);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified <see cref="Object" /> is equal to the current <see cref="IdentifiedObject" />.
        /// </summary>
        /// <param name="obj">The <see cref="Object" /> to compare with the current <see cref="IdentifiedObject" />.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Object" /> is equal to the current <see cref="IdentifiedObject" />; otherwise, <c>false</c>.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return (obj is IdentifiedObject && _identifier.Equals((obj as IdentifiedObject)._identifier) && (Name.Equals((obj as IdentifiedObject).Name)));
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="IdentifiedObject" />.
        /// </returns>
        public override Int32 GetHashCode()
        {
            return _identifier.GetHashCode() ^ Name.GetHashCode() ^ 925409699;
        }

        /// <summary>
        /// Returns a <see cref="String" /> that represents the current <see cref="IdentifiedObject" />.
        /// </summary>
        /// <returns>A <see cref="String" /> that contains both identifier and name.</returns>
        public override String ToString()
        {
            return "[" + _identifier + "] " + Name;
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Generates a random identifier.
        /// </summary>
        /// <returns>A random identifier that is not garanteed to be unique.</returns>
        public static String GenerateRandomIdentifier()
        {
            return "Random::" + new Random().Next(1000000, 9999999); 
        }

        #endregion
    }
}
