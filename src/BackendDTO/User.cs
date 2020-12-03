// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO
{
    using System.ComponentModel.DataAnnotations;
    using WahineKai.Backend.Common;

    /// <summary>
    /// Model of a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets user first name
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// Gets user last name, not required (in the case where people don't have/don't want to share last names
        /// </summary>
        public string? LastName { get; init; }

        /// <summary>
        /// Gets a value indicating whether a member is active, required, defaults to false
        /// </summary>
        public bool Active { get; init; } = false;

        /// <summary>
        /// Gets a member's facebook name, not required
        /// </summary>
        public string? FacebookName { get; init; }

        /// <summary>
        /// Gets a member's PayPal Name, not required
        /// </summary>
        public string? PayPalName { get; init; }

        /// <summary>
        /// Gets user email address
        /// </summary>
        [EmailAddress]
        public string? Email { get; init; }

        /// <summary>
        /// Gets user phone number, not required
        /// </summary>
        [Phone]
        public string? PhoneNumber { get; init; }

        /// <summary>
        /// Gets the user's street address, not required
        /// </summary>
        public string? StreetAddress { get; init; }

        /// <summary>
        /// Gets the user's city, not required
        /// </summary>
        public string? City { get; init; }

        /// <summary>
        /// Gets the user's state or province, not required
        /// </summary>
        public string? Region { get; init; }

        /// <summary>
        /// Gets the user's country, not required
        /// </summary>
        public string? Country { get; init; }

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
