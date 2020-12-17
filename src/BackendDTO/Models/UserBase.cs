// -----------------------------------------------------------------------
// <copyright file="UserBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.Common.Contracts;

    /// <summary>
    /// Base class - includes requried information for all user types
    /// </summary>
    public abstract class UserBase : IValidatable
    {
        /// <summary>
        /// Container Id for this model
        /// </summary>
        public const string ContainerId = "Users";

        /// <summary>
        /// Partion key for this container
        /// </summary>
        public const string PartitionKey = "/Email";

        /// <summary>
        /// Gets Azure Cosmos DB id for this user
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// Gets user email address, required
        /// </summary>
        [EmailAddress]
        public string? Email { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            // Email is primary key, required, can't be changed
            Ensure.IsNotNullOrWhitespace(() => this.Email);
        }
    }
}
