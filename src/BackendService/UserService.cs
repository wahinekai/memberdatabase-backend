// -----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service
{
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO;
    using WahineKai.Backend.DTO.Extensions;
    using WahineKai.Backend.Service.Contracts;

    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly ILogger logger;
        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="settings">Application settings</param>
        public UserService(ILoggerFactory loggerFactory, Settings settings)
        {
            this.settings = Ensure.IsNotNull(() => settings);
            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.logger = loggerFactory.CreateLogger<UserService>();
        }

        /// <inheritdoc/>
        public User Get()
        {
            var user = new User
            {
                FirstName = "Cameron",
                Email = "test@test.com",
            };
            user.SettingsAwareValidation(this.settings);
            return user;
        }
    }
}
