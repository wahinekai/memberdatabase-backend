// -----------------------------------------------------------------------
// <copyright file="ReadByAllWriteByUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO.Models
{
    using System;
    using System.Linq;
    using StatesAndProvinces;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.Common.Contracts;
    using WahineKai.Backend.DTO.Enums;

    /// <summary>
    /// User type with widest permissions (read by all, write by user, all by admin).
    /// </summary>
    public class ReadByAllWriteByUser : UserBase, IValidatable
    {
        /// <summary>
        /// Gets or sets user first name, required
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets user last name, not required (in the case where people don't have/don't want to share last names)
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets a member's facebook name, not required
        /// </summary>
        public string? FacebookName { get; set; }

        /// <summary>
        /// Gets or sets the user's city, not required
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the user's state or province, not required.  Must belong to states in supported countries in settings.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the user's country, not required.  Must belong to set of supported countries in settings.
        /// </summary>
        public Country? Country { get; set; }

        /// <summary>
        /// Gets or sets the user's occupation, not required
        /// </summary>
        public string? Occupation { get; set; }

        /// <summary>
        /// Gets or sets surfer level
        /// </summary>
        public Level? Level { get; set; }

        /// <summary>
        /// Gets or sets URL of a user's profile photo
        /// </summary>
        public string? PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets a user's biography
        /// </summary>
        public string? Biography { get; set; }

        /// <inheritdoc/>
        public new void Validate()
        {
            // Validate base
            base.Validate();

            // User must have a name
            this.FirstName = Ensure.IsNotNullOrWhitespace(() => this.FirstName);

            // Country & state validation
            if (this.Region != null)
            {
                // Country cannot be null if Region is
                Ensure.IsNotNull(() => this.Country);

                #pragma warning disable CS8629 // Nullable value checked for above

                // Cast country to CountrySelection to check for states
                var regions = Factory.Make((CountrySelection)this.Country);

                #pragma warning restore CS8629

                // Map to region names
                var regionNames = regions.Select(region => region.Name);

                // Check to see if region name contains the requested region
                if (!regionNames.Contains(this.Region))
                {
                    throw new ArgumentException($"{this.Region} is not a supported region");
                }
            }
        }
    }
}
