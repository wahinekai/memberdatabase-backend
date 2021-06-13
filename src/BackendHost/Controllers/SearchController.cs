// -----------------------------------------------------------------------
// <copyright file="SearchController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Host.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.MemberDatabase.Backend.Host.Models;
    using WahineKai.MemberDatabase.Backend.Service;
    using WahineKai.MemberDatabase.Backend.Service.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// Search Controller
    /// </summary>
    public class SearchController : Common.Api.Controllers.ControllerBase
    {
        private readonly ISearchService searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory given by ASP.NET</param>
        /// <param name="configuration">Global configuration given by ASP.NET</param>
        public SearchController(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of Search Controller beginning");

            this.searchService = new SearchService(loggerFactory, this.Configuration);

            this.Logger.LogTrace("Construction of Search Controller complete");
        }

        /// <summary>
        /// Gets users from the database that match the query string
        /// </summary>
        /// <param name="query">The query string to check against the database</param>
        /// <returns>A Collection of users</returns>
        [HttpGet]
        [ActionName("Query")]
        public async Task<ICollection<ReadByAllUser>> SearchAsync([FromQuery] string query)
        {
            this.Logger.LogDebug("Getting the user associated with this request");
            var users = await this.searchService.SearchAsync(this.GetUserEmailFromContext(), query);
            return users;
        }

        /// <summary>
        /// Returns a Complex Object Indicating the top five suggestions
        /// from the given partial query and the top suggested autocompletion
        /// of the query
        /// </summary>
        /// <param name="partialQuery">The partial query string to check against the database</param>
        /// <returns>A Collection of users and the top object</returns>
        [HttpGet]
        [ActionName("SuggestAndAutocomplete")]
        public async Task<SuggestAndAutocompleteReturn> SuggestAndAutocompleteAsync([FromQuery] string partialQuery)
        {
            this.Logger.LogDebug("Getting the user associated with this request");
            var suggestions = await this.searchService.SuggestAsync(this.GetUserEmailFromContext(), partialQuery);
            var autocomplete = await this.searchService.AutocompleteAsync(this.GetUserEmailFromContext(), partialQuery);

            var returnObject = new SuggestAndAutocompleteReturn
            {
                Suggestions = suggestions,
                Autocomplete = autocomplete,
            };

            returnObject.Validate();

            return returnObject;
        }
    }
}
