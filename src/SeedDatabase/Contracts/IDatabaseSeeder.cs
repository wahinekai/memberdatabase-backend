// -----------------------------------------------------------------------
// <copyright file="IDatabaseSeeder.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.SeedDatabase.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// Contract for a database seeder
    /// </summary>
    public interface IDatabaseSeeder
    {
        /// <summary>
        /// Adds new data to the database.  Assumes an empty database before running.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SeedAsync();

        /// <summary>
        /// Clears the database
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ClearAsync();

        /// <summary>
        /// Gets all users from the database.
        /// </summary>
        /// <returns>All of the users.</returns>
        public Task<ICollection<AdminUser>> GetAllUsersAsync();
    }
}
