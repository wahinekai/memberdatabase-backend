// -----------------------------------------------------------------------
// <copyright file="ReadByAllUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO.Models
{
    using System;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.Common.Contracts;
    using WahineKai.Backend.DTO.Enums;

    /// <summary>
    /// Extension of users that can be read by all
    /// </summary>
    public class ReadByAllUser : ReadByAllWriteByUser, IValidatable
    {
        /// <summary>
        /// Gets or sets the leadership position of the user.  Null means no leadership position
        /// </summary>
        public Position? Position { get; set; }

        /// <summary>
        /// Gets or sets the date the user started their particular position
        /// </summary>
        public DateTime? DateStartedPosition { get; set; }

        /// <summary>
        /// Gets or sets the user's chapter, required.  Must belong to set of supported chapters in settings
        /// </summary>
        public Chapter? Chapter { get; set; }

        /// <inheritdoc/>
        public new void Validate()
        {
            // Validate base
            base.Validate();

            // Every user belongs to a chapter
            this.Chapter = Ensure.IsNotNull(() => this.Chapter);
        }
    }
}
