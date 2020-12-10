// -----------------------------------------------------------------------
// <copyright file="UsersController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Host.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
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
            this.userService = new UserService(loggerFactory, this.Configuration);
        }

        /// <summary>
        /// Get the user's profile who sent the request
        /// </summary>
        /// <returns>The user's profile</returns>
        [HttpGet]
        [ActionName("Me")]
        public async Task<User> GetMeAsync()
        {
            var user = await this.userService.GetMeAsync(this.GetUserEmailFromContext());
            return user;
        }

        /// <summary>
        /// Get all users from database
        /// </summary>
        /// <returns>A collection of all users' profiles</returns>
        [HttpGet]
        [ActionName("All")]
        public async Task<ICollection<User>> GetAllAsync()
        {
            var users = await this.userService.GetAllAsync(this.GetUserEmailFromContext());
            return users;
        }

        /// <summary>
        /// Create a user with parameters given from body
        /// </summary>
        /// <param name="user">User given in body</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            // Input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            // Use service to add to repository
            var userFromService = await this.userService.CreateUserAsync(user);

            // Validate userFromService
            userFromService = Ensure.IsNotNull(() => userFromService);
            userFromService.Validate();

            // Return createdAtAction
            return this.CreatedAtAction("Create", userFromService);
        }
    }
}
