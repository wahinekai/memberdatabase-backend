// -----------------------------------------------------------------------
// <copyright file="IValidatable.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Contracts
{
    /// <summary>
    /// Interface indicating that an object implementing this interface can be validated to ensure correctness.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Ensure that this object is in a valid state
        /// </summary>
        public void Validate();
    }
}
