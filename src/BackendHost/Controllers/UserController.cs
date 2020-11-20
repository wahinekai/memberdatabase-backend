// <copyright file="UserController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// </copyright>

namespace WahineKai.Backend.Host.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// User controller class.
    /// </summary>
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">Logger given by ASP.NET.</param>
        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Sample get method to setup backend.
        /// </summary>
        /// <returns>A sample user.</returns>
        [HttpGet("user")]
        public User Get()
        {
            this.logger.LogInformation("In GET method.");
            return new User
            {
                Email = "test@test.com",
                Password = "password",
            };
        }
    }
}
