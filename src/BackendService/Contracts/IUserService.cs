// -----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WahineKai.Backend.DTO.Models;

    /// <summary>
    /// Service for interaction with users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get the profile of the authenticated user
        /// </summary>
        /// <param name="userEmail">E-mail address of the authenticated user</param>
        /// <returns>A hardcoded test user</returns>
        public Task<User> GetMeAsync(string userEmail);

        /// <summary>
        /// Get all users in the database
        /// </summary>
        /// <param name="userEmail">E-mail address of the authenticated user</param>
        /// <returns>A hardcoded test user</returns>
        public Task<ICollection<User>> GetAllAsync(string userEmail);

        /// <summary>
        /// Creates a user in the chosen repository
        /// </summary>
        /// <param name="user">The user to add to the repository</param>
        /// <returns>The created user in the repository</returns>
        public Task<User> CreateUserAsync(User user);
    }
}
