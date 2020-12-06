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
    public class UserService : ServiceBase, IUserService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="settings">Application settings</param>
        public UserService(ILoggerFactory loggerFactory, Settings settings)
            : base(loggerFactory, settings)
        {
        }

        /// <inheritdoc/>
        public User Get(string authenticatedUserEmail)
        {
            var user = new User
            {
                FirstName = "Cameron",
                Email = authenticatedUserEmail,
            };
            user.SettingsAwareValidation(this.Settings);
            return user;
        }
    }
}
