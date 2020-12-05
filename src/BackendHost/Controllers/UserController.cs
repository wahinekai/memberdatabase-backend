// -----------------------------------------------------------------------
// <copyright file="UserController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Host.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO;
    using WahineKai.Backend.Service;
    using WahineKai.Backend.Service.Contracts;

    /// <summary>
    /// User controller class
    /// </summary>
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<UserController> logger;
        private readonly Settings settings;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory given by ASP.NET</param>
        /// <param name="configuration">Global configuration given by ASP.NET</param>
        public UserController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.logger = loggerFactory.CreateLogger<UserController>();

            this.configuration = Ensure.IsNotNull(() => configuration);

            this.settings = new Settings(this.configuration);

            this.userService = new UserService(loggerFactory, this.settings);
        }

        /// <summary>
        /// Sample get method to setup backend
        /// </summary>
        /// <returns>A sample user</returns>
        [HttpGet("/user")]
        public User Get()
        {
            return this.userService.Get();
        }
    }
}
