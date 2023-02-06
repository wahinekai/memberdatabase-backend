// -----------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Host.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.Backend.Service;
    using WahineKai.MemberDatabase.Backend.Service.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// User controller class
    /// </summary>
    public sealed class UsersController : Common.Api.Controllers.ControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory given by ASP.NET</param>
        /// <param name="configuration">Global configuration given by ASP.NET</param>
        public UsersController(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of Users Controller beginning");

            this.userService = new UserService(loggerFactory, this.Configuration);

            this.Logger.LogTrace("Construction of Users Controller complete");
        }

        /// <summary>
        /// Returns a CSV file of all users
        /// </summary>
        /// <returns>A CSV file</returns>
        [HttpGet]
        [ActionName("AllUsers.csv")]
        [Produces("text/csv")]
        public async Task<ICollection<AdminUserCSV>> DownloadCsvAsync()
        {
            this.Logger.LogDebug("Getting all users in a CSV");
            var users = await this.userService.GetAllUsersAsync(this.GetUserEmailFromContext());
            List<AdminUserCSV> csvList = new List<AdminUserCSV>();
            foreach (var user in users)
            {
                csvList.Add(AdminUserCSV.ConvertUserToCSV(user));
            }
                
            return csvList;
        }

        /// <summary>
        /// Imports CSV file
        /// </summary>
        /// <returns>Import Return</returns>
        [HttpPost]
        [ActionName("Import/Csv")]
        public async Task<IActionResult> CsvImportAsync()
        {
            var importReturn = await this.userService.UploadUsersFromCsvAsync(this.Request.Body, this.GetUserEmailFromContext());
            return this.CreatedAtAction("Import/Csv", importReturn);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>All users</returns>
        [HttpGet]
        [ActionName("All")]
        public async Task<ICollection<AdminUser>> GetAllUsersAsync()
        {
            this.Logger.LogDebug("Getting all users");
            var users = await this.userService.GetAllUsersAsync(this.GetUserEmailFromContext());
            return users;
        }

        /// <summary>
        /// Get the user's profile who sent the request
        /// </summary>
        /// <returns>The user's profile</returns>
        [HttpGet]
        [ActionName("Me")]
        public async Task<AdminUser> GetMeAsync()
        {
            this.Logger.LogDebug("Getting the user associated with this request");
            var user = await this.userService.GetByEmailAsync(this.GetUserEmailFromContext());
            return user;
        }

        /// <summary>
        /// Gets whether the user sending this request is an admin user
        /// </summary>
        /// <returns>Whether the user sending this request is an admin user</returns>
        [HttpGet]
        [ActionName("UserId/Me")]
        public async Task<Guid> GetMyUserIdAsync()
        {
            this.Logger.LogDebug("Getting the user associated with this request");
            var user = await this.userService.GetByEmailAsync(this.GetUserEmailFromContext());
            return user.Id;
        }

        /// <summary>
        /// Gets whether the user sending this request is an admin user
        /// </summary>
        /// <returns>Whether the user sending this request is an admin user</returns>
        [HttpGet]
        [ActionName("IsAdmin/Me")]
        public async Task<bool> AmIAdminAsync()
        {
            this.Logger.LogDebug("Getting the user associated with this request");
            var user = await this.userService.GetByEmailAsync(this.GetUserEmailFromContext());
            return user.Admin;
        }

        /// <summary>
        /// Gets a user by userId
        /// </summary>
        /// <param name="userId">The userId of the user to get</param>
        /// <returns>A valuserIdated user</returns>
        [HttpGet]
        [ActionName("Id")]
        public async Task<AdminUser> GetByIdAsync(Guid userId)
        {
            this.Logger.LogDebug($"Getting the user with userId {userId}");
            var user = await this.userService.GetByIdAsync(userId, this.GetUserEmailFromContext());
            return user;
        }

        /// <summary>
        /// Deletes a user by userId
        /// </summary>
        /// <param name="userId">The userId of the user to delete</param>
        /// <returns>A Task</returns>
        [HttpDelete]
        [ActionName("Id")]
        public async Task<IActionResult> DeleteByIdAsync(Guid userId)
        {
            this.Logger.LogDebug($"Deleting the user with userId {userId}");
            await this.userService.DeleteByIdAsync(userId, this.GetUserEmailFromContext());

            return this.NoContent();
        }

        /// <summary>
        /// Create a user with parameters given from body
        /// </summary>
        /// <param name="user">User given in body</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] AdminUser user)
        {
            this.Logger.LogDebug("Creating a user");

            // Input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            // Use service to add to repository
            var userFromService = await this.userService.CreateAsync(user, this.GetUserEmailFromContext());
            return this.CreatedAtAction("Create", userFromService);
        }

        /// <summary>
        /// Replaces parameters in the calling user's profile
        /// </summary>
        /// <param name="updatedUser">The updated user to replace with</param>
        /// <returns>The newly updated user from the database</returns>
        [HttpPut]
        [ActionName("Me")]
        public async Task<AdminUser> ReplaceMe([FromBody] AdminUser updatedUser)
        {
            this.Logger.LogDebug("Replacing a user");

            // User service to replace in repository
            var userFromService = await this.userService.ReplaceByEmailAsync(this.GetUserEmailFromContext(), updatedUser);
            return userFromService;
        }

        /// <summary>
        /// Replaces parameters in a user's profile
        /// </summary>
        /// <param name="userId">The userId of the user to replace</param>
        /// <param name="updatedUser">The updated user to replace with</param>
        /// <returns>The newly updated user from the database</returns>
        [HttpPut]
        [ActionName("Id")]
        public async Task<AdminUser> ReplaceByIdAsync(Guid userId, [FromBody] AdminUser updatedUser)
        {
            this.Logger.LogDebug($"Getting the user with userId {userId}");
            var user = await this.userService.ReplaceByIdAsync(userId, updatedUser, this.GetUserEmailFromContext());
            return user;
        }
    }
}
