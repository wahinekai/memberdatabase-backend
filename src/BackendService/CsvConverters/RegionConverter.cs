// -----------------------------------------------------------------------
// <copyright file="RegionConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using System;
    using System.Linq;
    using CountryData.Standard;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class RegionConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Country Helper from CountryData.Standard for Country and State Validation
        /// </summary>
        protected static readonly CountryHelper CountryHelper = new CountryHelper();

        /// <summary>
        /// Transfers what could be a full name, abbreviation, or alternate abbreviation to a full region name
        /// </summary>
        /// <param name="text">The input string</param>
        /// <returns>The name of the region specified (null if not possible)</returns>
        public static string? NameOrAbbreviationToString(string text)
        {
            foreach (var country in CountryHelper.GetCountryData())
            {
                foreach (var region in country.Regions)
                {
                    if (text.ToLower() == region.Name.ToLower() || text.ToLower() == region.ShortCode.ToLower())
                    {
                        return region.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converts a boolean type object from the CSV's text string
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="row">The row the data is on</param>
        /// <param name="memberMapData">A Helper object given by CsvHelper</param>
        /// <returns>The boolean value that corresponds to the string</returns>
        public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return NameOrAbbreviationToString(text);
        }
    }
}
