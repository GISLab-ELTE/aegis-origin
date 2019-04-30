/// <copyright file="UsernamePasswordFileSystemAuthentication.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
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
using System.Security;

namespace ELTE.AEGIS.IO.Storage.Authentication
{
    /// <summary>
    /// Represents a file system authentication by username and password.
    /// </summary>
    public class UsernamePasswordFileSystemAuthentication : IFileSystemAuthentication
    {
        #region Public properties

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public String Username { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The secure string containing the password.</value>
        public SecureString Password { get; private set; }

        #endregion

        #region IFileSystemAuthentication properties

        /// <summary>
        /// Gets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public FileSystemAuthenticationType AutenticationType { get { return FileSystemAuthenticationType.UserCredentials; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePasswordFileSystemAuthentication" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.ArgumentNullException">The username is null.</exception>
        /// <exception cref="System.ArgumentException">The username is empty, or consists only of whitespace characters.</exception>
        public UsernamePasswordFileSystemAuthentication(String username, String password)
        {
            if (username == null)
                throw new ArgumentNullException("username", "The username is null.");
            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentException("The username is empty, or consists only of whitespace characters.", "username");

            Username = username;
            Password = new SecureString();

            foreach (Char passwordChar in password)
                Password.AppendChar(passwordChar);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernamePasswordFileSystemAuthentication" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.ArgumentNullException">The username is null.</exception>
        /// <exception cref="System.ArgumentException">The username is empty, or consists only of whitespace characters.</exception>
        public UsernamePasswordFileSystemAuthentication(String username, SecureString password)
        {
            if (username == null)
                throw new ArgumentNullException("username", "The username is null.");
            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentException("The username is empty, or consists only of whitespace characters.", "username");

            Username = username;
            Password = password;
        }

        #endregion
    }
}
