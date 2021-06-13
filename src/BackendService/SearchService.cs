// -----------------------------------------------------------------------
// <copyright file="SearchService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.Common.Api.Services;
    using WahineKai.MemberDatabase.Backend.Service.Contracts;
    using WahineKai.MemberDatabase.Dto;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Implementation of ISearchService
    /// </summary>
    public class SearchService : ServiceBase, ISearchService
    {
        private readonly IUserRepository<ReadByAllUser> userRepository;
        private readonly ISearchRepository searchRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public SearchService(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of Search Service beginning");

            // Build configurations
            var cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(this.Configuration);
            var searchConfiguration = AzureSearchConfiguration.BuildFromConfiguration(this.Configuration);

            this.userRepository = new CosmosUserRepository<ReadByAllUser>(cosmosConfiguration, loggerFactory);
            this.searchRepository = new AzureSearchRepository(searchConfiguration, loggerFactory);

            this.Logger.LogTrace("Construction of Search Service complete");
        }

        /// <inheritdoc/>
        public async Task<IList<ReadByAllUser>> SearchAsync(string userEmail, string query)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);
            query = Ensure.IsNotNullOrWhitespace(() => query);

            await this.EnsureCallingUserPermissionsAsync(userEmail);

            this.Logger.LogDebug($"Getting users matching query \"{query}\" from repository");

            var idList = await this.searchRepository.SearchAsync(query);
            this.Logger.LogDebug($"Search returned {idList.Count}");

            var allUsers = await this.userRepository.GetAllUsersAsync();

            var users = await this.userRepository.GetUsersByIdCollectionAsync(idList);

            this.Logger.LogTrace($"Got {users.Count} users from the user repository");

            return users;
        }

        /// <inheritdoc/>
        public async Task<IList<ReadByAllUser>> SuggestAsync(string userEmail, string partialQuery)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);
            partialQuery = Ensure.IsNotNullOrWhitespace(() => partialQuery);

            await this.EnsureCallingUserPermissionsAsync(userEmail);

            this.Logger.LogDebug($"Suggesting users matching query \"{partialQuery}\" from repository");

            var idList = await this.searchRepository.SuggestAsync(partialQuery);
            this.Logger.LogDebug($"Search returned {idList.Count}");

            var allUsers = await this.userRepository.GetAllUsersAsync();

            var users = await this.userRepository.GetUsersByIdCollectionAsync(idList);

            this.Logger.LogTrace($"Got {users.Count} users from the user repository");

            return users;
        }

        /// <inheritdoc/>
        public async Task<string> AutocompleteAsync(string userEmail, string partialQuery)
        {
            // Sanity check input
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);
            partialQuery = Ensure.IsNotNullOrWhitespace(() => partialQuery);

            await this.EnsureCallingUserPermissionsAsync(userEmail);

            this.Logger.LogDebug($"Autocompeting query \"{partialQuery}\"");

            var fullQuery = await this.searchRepository.AutoCompleteAsync(partialQuery);

            return fullQuery.Substring(partialQuery.Length);
        }
    }
}
