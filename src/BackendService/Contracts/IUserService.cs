// -----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WahineKai.Backend.DTO.Models;

    /// <summary>
    /// Service for interaction with users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get the profile of the a user
        /// </summary>
        /// <param name="userEmail">E-mail address of the user to get</param>
        /// <param name="callingUserEmail">The email address of the user calling this service.  Defaults to the user to get.</param>
        /// <returns>A validated user</returns>
        public Task<User> GetByEmailAsync(string userEmail, string? callingUserEmail = null);

        /// <summary>
        /// Get the profile of the a user
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <param name="callingUserEmail">The email address of the user calling this service</param>
        /// <returns>A validated user</returns>
        public Task<User> GetByIdAsync(Guid id, string? callingUserEmail);

        /// <summary>
        /// Creates a user in the chosen repository
        /// </summary>
        /// <param name="user">The user to add to the repository</param>
        /// <param name="callingUserEmail">E-mail address of the calling user</param>
        /// <returns>The created user in the repository</returns>
        public Task<User> CreateAsync(User user, string callingUserEmail);

        /// <summary>
        /// Replace the user with making the request with the new user information
        /// </summary>
        /// <param name="userEmail">The email of the authenticated user making this request</param>
        /// <param name="updatedUser">The updated user to replace it with</param>
        /// <param name="callingUserEmail">E-mail address of the calling user, defaults to the email to replace</param>
        /// <returns>The updated user</returns>
        public Task<User> ReplaceByEmailAsync(string userEmail, User updatedUser, string? callingUserEmail = null);

        /// <summary>
        /// Replace the user with the specified id with the new user
        /// </summary>
        /// <param name="id">The id of the user to replace</param>
        /// <param name="updatedUser">The updated user to replace it with</param>
        /// <param name="callingUserEmail">E-mail address of the calling user</param>
        /// <returns>The updated user</returns>
        public Task<User> ReplaceByIdAsync(Guid id, User updatedUser, string callingUserEmail);
    }
}
