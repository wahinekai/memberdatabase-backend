// -----------------------------------------------------------------------
// <copyright file="UserWithToken.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO
{
    using WahineKai.Backend.Common;

    /// <summary>
    /// Model of a user.
    /// </summary>
    public class UserWithToken : User
    {
        /// <summary>
        /// Gets user authentication token
        /// </summary>
        public string? Token { get; init; }

        /// <summary>
        /// Ensure this object is a valid user record
        /// </summary>
        public new void Validate()
        {
            // Validate base class
            base.Validate();

            // Validate required parameters
            Ensure.IsNotNullOrWhitespace(() => this.Token);
        }
    }
}
