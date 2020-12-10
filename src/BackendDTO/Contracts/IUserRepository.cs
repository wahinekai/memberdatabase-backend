// -----------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WahineKai.Backend.DTO.Models;

    /// <summary>
    /// Interface for actions connecting Users with User store
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get the user given by the email from the database
        /// </summary>
        /// <param name="email">An email of a user</param>
        /// <returns>A validated user</returns>
        public Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Gets all users from the repository
        /// </summary>
        /// <returns>A collection of users</returns>
        public Task<ICollection<User>> GetAllUsersAsync();

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">The user to add to the database</param>
        /// <returns>The user added to the database</returns>
        public Task<User> CreateUserAsync(User user);
    }
}
