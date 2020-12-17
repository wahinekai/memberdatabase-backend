// -----------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Host.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.Service;
    using WahineKai.Backend.Service.Contracts;

    /// <summary>
    /// User controller class
    /// </summary>
    public sealed class UsersController : ControllerBase
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
        [ActionName("IsAdmin/Me")]
        public async Task<bool> AmIAdminAsync()
        {
            this.Logger.LogDebug("Getting the user associated with this request");
            var user = await this.userService.GetByEmailAsync(this.GetUserEmailFromContext());
            return user.Admin ?? false;
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
