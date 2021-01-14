// -----------------------------------------------------------------------
// <copyright file="ImportReturn.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.Models
{
    using System.Collections.Generic;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// Return type from ImportCsv
    /// </summary>
    public class ImportReturn
    {
        /// <summary>
        /// Gets or sets collection of users that were imported
        /// </summary>
        public ICollection<AdminUser> ImportedUsers { get; set; } = new List<AdminUser>();

        /// <summary>
        /// Gets or sets collection of users that weren't imported because they were invalid
        /// </summary>
        public ICollection<AdminUser> InvalidUsers { get; set; } = new List<AdminUser>();

        /// <summary>
        /// Gets or sets collection of users that weren't imported because they are duplicates
        /// </summary>
        public ICollection<AdminUser> DuplicateUsers { get; set; } = new List<AdminUser>();
    }
}
