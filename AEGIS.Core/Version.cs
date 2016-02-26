/// <copyright file="Version.cs" company="Eötvös Loránd University (ELTE)">
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

using System;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a version number with major, minor and revision versions.
    /// </summary>
    public class Version : IComparable, IComparable<Version>, IEquatable<Version>
    {
        #region Private constant fields

        /// <summary>
        /// The default version of 1.0.0. This field is read-only.
        /// </summary>
        private const String DefaultVersionString = "1.0.0";

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        /// <value>The major component of the current version number.</value>
        public Int32 Major { get; private set; }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        /// <value>The minor component of the current version number.</value>
        public Int32 Minor { get; private set; }

        /// <summary>
        /// Gets the revision number.
        /// </summary>
        /// <value>The revision component of the current version number.</value>
        public Int32 Revision { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The major version number is less than 0.</exception>
        public Version(Int32 major) : this(major, 0, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The major version number is less than 0.
        /// or
        /// The minor version number is less than 0.
        /// </exception>
        public Version(Int32 major, Int32 minor) : this(minor, 0, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="revision">The revision number.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The major version number is less than 0.
        /// or
        /// The minor version number is less than 0.
        /// or
        /// The revision number is less than 0.
        /// </exception>
        public Version(Int32 major, Int32 minor, Int32 revision)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException("major", "The major version number is less than 0.");
            if (minor < 0)
                throw new ArgumentOutOfRangeException("minor", "The minor version number is less than 0.");
            if (revision < 0)
                throw new ArgumentOutOfRangeException("revision", "The revision number is less than 0.");

            Major = major;
            Minor = minor;
            Revision = revision;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the version is compatible with another.
        /// </summary>
        /// <param name="other">The other version.</param>
        /// <returns><c>true</c> if the two versions are compatible; otherwise <c>false</c>.</returns>
        public Boolean IsCompatible(Version other)
        {
            if (other == null)
                return false;

            return Major == other.Major;
        }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current version with another object.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="System.InvalidOperationException">The object is not the same type as this instance.</exception>
        public Int32 CompareTo(Object obj)
        {
            if (!(obj is Version))
                throw new InvalidOperationException("The object is not the same type as this instance.");

            return CompareTo(obj as Version);
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="other">A version to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the versions being compared.</returns>
        public Int32 CompareTo(Version other)
        {
            if (other == null)
                return 1;

            if (Major != other.Major)
                return (Major > other.Major) ? 1 : -1;
 
            if (Minor != other.Minor)
                return (Minor > other.Minor) ? 1 : -1;
 
            if (Revision != other.Revision)
                return (Revision > other.Revision) ? 1 : -1;
 
            return 0;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether the current version is equal to another version.
        /// </summary>
        /// <param name="other">The version to compare with the current version.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Version other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;

            return (Major == other.Major) || (Minor == other.Minor) || (Revision == other.Revision);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified object is equal to the current version.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;

            Version version = obj as Version;

            return (Major == version.Major) || (Minor == version.Minor) || (Revision == version.Revision);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Object" />.</returns>
        public override Int32 GetHashCode()
        {
            return (Major << 24 | Minor << 16 | Revision) ^ 961777723;
        }

        /// <summary>
        /// Returns a string that represents the current version.
        /// </summary>
        /// <returns>A string that represents the current version.</returns>
        public override String ToString()
        {
            return String.Concat(Major, ".", Minor, ".", Revision); 
        }

        #endregion

        #region Public static instances

        /// <summary>
        /// Gets the default version.
        /// </summary>
        /// <value>The default version of 1.0.0.</value>
        public static Version Default { get { return Version.Parse(DefaultVersionString); } }

        #endregion

        #region Public static methods

        /// <summary>
        /// Parses the specified version string.
        /// </summary>
        /// <param name="version">The version string.</param>
        /// <returns>The parsed version.</returns>
        /// <exception cref="System.ArgumentNullException">The version is null.</exception>
        /// <exception cref="System.ArgumentException">The version has no components or more than three components.</exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public static Version Parse(String version)
        {
            if (version == null)
                throw new ArgumentNullException("version", "The version is null.");

            String[] components = version.Split('.');

            if (components.Length < 1 || components.Length > 3)
                throw new ArgumentException("The version has no components or more than three components.", "version");

            Int32 major, minor = 0, revision = 0;
            if (!Int32.TryParse(components[0], out major) ||
                components.Length > 1 && !Int32.TryParse(components[1], out minor) ||
                components.Length > 2 && !Int32.TryParse(components[2], out revision))
            {
                throw new FormatException("One or more components of the version do not parse into an integer.");
            }

            if (major < 0 || minor < 0 || revision < 0)
                throw new ArgumentOutOfRangeException("One or more components of the version have a value of less than 0.");

            return new Version(major, minor, revision);
        }

        /// <summary>
        /// Tries to parse the specified version string.
        /// </summary>
        /// <param name="version">The version string.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if the version string was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String version, out Version result)
        {
            result = null;

            if (version == null)
                return false;

            String[] components = version.Split('.');

            if (components.Length < 1 || components.Length > 3)
                return false;

            Int32 major, minor = 0, revision = 0;
            if (!Int32.TryParse(components[0], out major) ||
                components.Length > 1 && !Int32.TryParse(components[1], out minor) ||
                components.Length > 2 && !Int32.TryParse(components[2], out revision))
            {
                return false;
            }

            if (major < 0 || minor < 0 || revision < 0)
                return false;

            result = new Version(major, minor, revision);

            return true;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Version" /> instances are equal.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;

            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Version" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return false;

            return !first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is less than the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is less than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return false;

            return (first.CompareTo(second) < 0);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is greater than the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is greater than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Version first, Version second)
        {
            return (second < first);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is smaller or equal to the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is smaller or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;

            return (first.CompareTo(second) <= 0);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is greater or equal to the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is greater or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Version first, Version second)
        {
            return (second <= first);
        }

        #endregion
    }
}

