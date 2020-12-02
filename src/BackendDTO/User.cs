// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Wahine Kai">
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
    public record User
    {
        /// <summary>
        /// Gets user first name
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// Gets user email address
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// Ensure this object is a valid user record
        /// </summary>
        public void Validate()
        {
            // Required parameters
            Ensure.IsNotNullOrWhitespace(() => this.FirstName);
            Ensure.IsNotNullOrWhitespace(() => this.Email);
        }
    }
}
