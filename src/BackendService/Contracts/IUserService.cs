// -----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using WahineKai.MemberDatabase.Backend.Service.Models;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// Service for interaction with users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all users after checking that the user email given is an administrator
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <returns>A Collection of users</returns>
        public Task<ICollection<AdminUser>> GetAllUsersAsync(string userEmail);

        /// <summary>
        /// Gets all users after checking that the user email given is an administrator
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <returns>A Collection of users</returns>
        public Task<ICollection<AdminUser>> GetAllActiveUsersAsync(string userEmail);

        /// <summary>
        /// Get the profile of the a user
        /// </summary>
        /// <param name="userEmail">E-mail address of the user to get</param>
        /// <param name="callingUserEmail">The email address of the user calling this service.  Defaults to the user to get.</param>
        /// <returns>A validated user</returns>
        public Task<AdminUser> GetByEmailAsync(string userEmail, string? callingUserEmail = null);

        /// <summary>
        /// Get the profile of the a user
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <param name="callingUserEmail">The email address of the user calling this service</param>
        /// <returns>A validated user</returns>
        public Task<AdminUser> GetByIdAsync(Guid id, string? callingUserEmail);

        /// <summary>
        /// Creates a user in the chosen repository
        /// </summary>
        /// <param name="user">The user to add to the repository</param>
        /// <param name="callingUserEmail">E-mail address of the calling user</param>
        /// <returns>The created user in the repository</returns>
        public Task<AdminUser> CreateAsync(AdminUser user, string callingUserEmail);

        /// <summary>
        /// Replace the user with making the request with the new user information
        /// </summary>
        /// <param name="userEmail">The email of the authenticated user making this request</param>
        /// <param name="updatedUser">The updated user to replace it with</param>
        /// <param name="callingUserEmail">E-mail address of the calling user, defaults to the email to replace</param>
        /// <returns>The updated user</returns>
        public Task<AdminUser> ReplaceByEmailAsync(string userEmail, AdminUser updatedUser, string? callingUserEmail = null);

        /// <summary>
        /// Replace the user with the specified id with the new user
        /// </summary>
        /// <param name="id">The id of the user to replace</param>
        /// <param name="updatedUser">The updated user to replace it with</param>
        /// <param name="callingUserEmail">E-mail address of the calling user</param>
        /// <returns>The updated user</returns>
        public Task<AdminUser> ReplaceByIdAsync(Guid id, AdminUser updatedUser, string callingUserEmail);

        /// <summary>
        /// Delete the user with the specified id
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        /// <param name="callingUserEmail">E-mail address of the calling user</param>
        /// <returns>A <see cref="Task" /></returns>
        public Task DeleteByIdAsync(Guid id, string callingUserEmail);

        /// <summary>
        /// Upload users from a CSV stream
        /// </summary>
        /// <param name="usersStream">The stream (in CSV format) of users to upload</param>
        /// <param name="callingUserEmail">The user making the request</param>
        /// <returns>TBD</returns>
        public Task<ImportReturn> UploadUsersFromCsvAsync(Stream usersStream, string callingUserEmail);
    }
}