// -----------------------------------------------------------------------
// <copyright file="SuggestAndAutocompleteReturn.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Host.Models
{
    using System.Collections.Generic;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// The return type from the suggest and autocomplete endpoint
    /// </summary>
    public class SuggestAndAutocompleteReturn : IValidatable
    {
        /// <summary>
        /// Gets the list of suggested users
        /// </summary>
        public IList<ReadByAllUser>? Suggestions { get; init; }

        /// <summary>
        /// Gets the top autocomplete suggestion
        /// </summary>
        public string? Autocomplete { get; init; }

        /// <inheritdoc/>
        public void Validate()
        {
            Ensure.IsNotNull(() => this.Suggestions);
            Ensure.IsNotNull(() => this.Autocomplete);
        }
    }
}
