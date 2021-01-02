// -----------------------------------------------------------------------
// <copyright file="IEntrypoint.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Contracts
{
    /// <summary>
    /// Interface for an application entrypoint
    /// </summary>
    public interface IEntrypoint
    {
        /// <summary>
        /// Entrypoint to the application
        /// </summary>
        public void Start();
    }
}
