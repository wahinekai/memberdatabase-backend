// -----------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service.Contracts
{
    using WahineKai.Backend.DTO;

    /// <summary>
    /// Service for interaction with users
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get a test user
        /// </summary>
        /// <param name="authenticatedUserEmail">E-mail address of the authenticated user</param>
        /// <returns>A hardcoded test user</returns>
        public User Get(string authenticatedUserEmail);
    }
}
