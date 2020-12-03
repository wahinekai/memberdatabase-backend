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
    public class UserService<TILoggerType> : IUserService<TILoggerType>
    {
        private readonly ILogger<TILoggerType> logger;
        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService{TILoggerType}"/> class.
        /// </summary>
        /// <param name="logger">Logger for this call stack</param>
        /// <param name="settings">Application settings</param>
        public UserService(ILogger<TILoggerType> logger, Settings settings)
        {
            this.settings = Ensure.IsNotNull(() => settings);
            this.logger = Ensure.IsNotNull(() => logger);
        }

        /// <inheritdoc/>
        public User Get()
        {
            var user = new UserWithToken
            {
                Token = "TOKEN",
                FirstName = "Cameron",
                Email = "test@test.com",
            };
            user.SettingsAwareValidation(this.settings);
            return user;
        }
    }
}
