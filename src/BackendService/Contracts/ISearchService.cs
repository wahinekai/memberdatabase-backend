// -----------------------------------------------------------------------
// <copyright file="ISearchService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WahineKai.Backend.DTO.Models;

    /// <summary>
    /// Interface describing the API of the search service
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Gets all users after checking that the user email given is an administrator
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <returns>A Collection of users</returns>
        public Task<ICollection<ReadByAllUser>> GetAllUsersAsync(string userEmail);

        /// <summary>
        /// Gets users matching a specific query after checking that user email given is an administrator
        /// </summary>
        /// <param name="userEmail">Email of the calling user</param>
        /// <param name="query">The search query to give the user repository</param>
        /// <returns>A Collection of users</returns>
        public Task<ICollection<ReadByAllUser>> GetByQueryAsync(string userEmail, string query);
    }
}
