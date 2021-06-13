// -----------------------------------------------------------------------
// <copyright file="ISearchService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// Interface describing the API of the search service
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Gets users matching a specific query after checking that user email given is an administrator
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <param name="query">The search query to give the user repository</param>
        /// <returns>A Collection of users</returns>
        public Task<IList<ReadByAllUser>> SearchAsync(string userEmail, string query);

        /// <summary>
        /// Suggests the top 5 users matching a specific partial query
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <param name="partialQuery">The partial search query to give the user repository</param>
        /// <returns>A Collection of users</returns>
        public Task<IList<ReadByAllUser>> SuggestAsync(string userEmail, string partialQuery);

        /// <summary>
        /// Suggests the top autocompletion for a specific partial query
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <param name="partialQuery">The partial search query to give the user repository</param>
        /// <returns>The top autocompletion</returns>
        public Task<string> AutocompleteAsync(string userEmail, string partialQuery);
    }
}
