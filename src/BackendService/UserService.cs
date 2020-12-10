// -----------------------------------------------------------------------
// <copyright file="UserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO;
    using WahineKai.Backend.DTO.Contracts;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.DTO.Properties;
    using WahineKai.Backend.Service.Contracts;

    /// <inheritdoc/>
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public UserService(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            // Build cosmos configuration
            var cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(this.Configuration);

            this.userRepository = new CosmosUserRepository(cosmosConfiguration, loggerFactory);
        }

        /// <inheritdoc/>
        public async Task<User> GetMeAsync(string userEmail)
        {
            var user = await this.userRepository.GetUserByEmailAsync(userEmail);
            Ensure.IsTrue(() => user.Admin);

            return user;
        }

        /// <inheritdoc/>
        public async Task<ICollection<User>> GetAllAsync(string userEmail)
        {
            var user = await this.userRepository.GetUserByEmailAsync(userEmail);
            Ensure.IsTrue(() => user.Admin);

            var users = await this.userRepository.GetAllUsersAsync();
            Ensure.IsNotNullOrEmpty(() => users);

            return users;
        }

        /// <inheritdoc/>
        public async Task<User> CreateUserAsync(User user)
        {
            // input sanity checking
            user = Ensure.IsNotNull(() => user);
            user.Validate();

            // Get from repository
            var userFromRepository = await this.userRepository.CreateUserAsync(user);

            // Sanity check output
            userFromRepository = Ensure.IsNotNull(() => userFromRepository);
            userFromRepository.Validate();

            return userFromRepository;
        }
    }
}
