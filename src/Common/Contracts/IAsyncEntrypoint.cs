// -----------------------------------------------------------------------
// <copyright file="IAsyncEntrypoint.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for an application entrypoint
    /// </summary>
    public interface IAsyncEntrypoint
    {
        /// <summary>
        /// Async entrypoint to the application
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task StartAsync();
    }
}
