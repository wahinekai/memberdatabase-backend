// -----------------------------------------------------------------------
// <copyright file="UserExtensions.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO.Extensions
{
    using System;
    using System.Linq;
    using StatesAndProvinces;
    using WahineKai.Backend.Common;

    /// <summary>
    /// Extensions for the User Model
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Extension to the User validation method that is settings-aware.  This is a superset of user.Validate() - there is no need to call both
        /// </summary>
        /// <param name="user">The user to validate</param>
        /// <param name="settings">Global application settings used to validate the user</param>
        public static void SettingsAwareValidation(this User user, Settings settings)
        {
            // Do normal validation on user first
            user.Validate();

            // Country & state validation
            if (user.Region != null && user.Country != null)
            {
                // Check to see if country is a supported country
                if (!settings.SupportedCountries.Contains(user.Country))
                {
                    throw new ArgumentException($"{user.Country} is not a supported country");
                }

                // Setup state checker - at the moment, I have to hand-do supported countries
                var regions = user.Country switch
                {
                    "United States" => Ensure.IsNotNullOrEmpty(() => Factory.Make(CountrySelection.UnitedStates)),
                    "Canada" => Ensure.IsNotNullOrEmpty(() => Factory.Make(CountrySelection.Canada)),
                    _ => throw new ArgumentException($"{user.Country} is not a supported country"),
                };

                // Map to region names
                var regionNames = regions.Select(region => region.Name);

                // Check to see if region name contains the requested region
                if (!regionNames.Contains(user.Region))
                {
                    throw new ArgumentException($"{user.Region} is not a supported region");
                }
            }
        }
    }
}
