// -----------------------------------------------------------------------
// <copyright file="UserBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Newtonsoft.Json;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Base class - includes requried information for all user types
    /// </summary>
    public abstract class UserBase : ModelBase, IValidatable
    {
        /// <summary>
        /// Container Id for this model
        /// </summary>
        public const string ContainerId = "Users";

        /// <summary>
        /// Partion key for this container
        /// </summary>
        public const string PartitionKey = "/id";

        /// <summary>
        /// Gets or sets user email address, required
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <inheritdoc/>
        public new void Validate()
        {
            // Validate base
            base.Validate();

            // Email is required
            this.Email = Ensure.IsNotNullOrWhitespace(() => this.Email);
        }

        /// <summary>
        /// Override of base ToString Method
        /// </summary>
        /// <returns>A printable string representing this document</returns>
        public override string ToString()
        {
            bool valid = true;
            try
            {
                this.Validate();
            }
            catch (Exception)
            {
                valid = false;
            }

            var stringBuilder = new StringBuilder(base.ToString());
            stringBuilder.AppendLine("User Model");
            stringBuilder.AppendLine("UserBase Section");
            stringBuilder.AppendLine($"Valid?: {valid}");
            stringBuilder.AppendLine($"id: {this.Id}");
            stringBuilder.AppendLine($"Email: {this.Email}");

            return stringBuilder.ToString();
        }
    }
}
