// -----------------------------------------------------------------------
// <copyright file="IUpdatable.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Contracts
{
    /// <summary>
    /// Interface indicating that an object implementing this interface can be validated to ensure correctness.
    /// </summary>
    /// <typeparam name="T">The type of the update parameter</typeparam>
    public interface IUpdatable<T>
    {
        /// <summary>
        /// Update the object with the new object
        /// </summary>
        /// <param name="objectToUpdateWith">The object to update with</param>
        public void Update(T objectToUpdateWith);
    }
}
