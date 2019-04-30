/// <copyright file="HadoopUsernameAuthentication.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.IO.Storage.Authentication
{
    /// <summary>
    /// Represents a Hadoop file system authentication based on user name.
    /// </summary>
    public class HadoopUsernameAuthentication : IHadoopFileSystemAuthentication
    {
        #region IFileSystemAuthentication properties

        /// <summary>
        /// Gets the type of the authentication.
        /// </summary>
        /// <value>The type of the authentication.</value>
        public FileSystemAuthenticationType AutenticationType { get { return FileSystemAuthenticationType.UserCredentials; } }

        #endregion

        #region IHadoopFileSystemAuthentication properties

        /// <summary>
        /// Gets the request of the authentication.
        /// </summary>
        /// <value>The request form of the authentication.</value>
        public String Request { get { return "user.name=" + Username; } }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The Hadoop username used for executing operations.</value>
        public String Username { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HadoopUsernameAuthentication" /> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <exception cref="System.ArgumentNullException">The username is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The username is empty.
        /// or
        /// The username is in an invalid format.
        /// </exception>
        public HadoopUsernameAuthentication(String username) 
        {
            if (username == null)
                throw new ArgumentNullException("username", "The username is null.");
            if (String.IsNullOrEmpty(username))
                throw new ArgumentException("The username is empty.", "username");

            Username = username;
        }

        #endregion
    }
}
