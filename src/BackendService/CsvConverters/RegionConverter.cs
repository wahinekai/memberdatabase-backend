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
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using StatesAndProvinces;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class RegionConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Transfers what could be a full name, abbreviation, or alternate abbreviation to a full region name
        /// </summary>
        /// <param name="text">The input string</param>
        /// <returns>The name of the region specified (null if not possible)</returns>
        public static string? NameOrAbbreviationToString(string text)
        {
            var usStates = StatesAndProvinces.Factory.Make(CountrySelection.UnitedStates);
            var canadianProvinces = StatesAndProvinces.Factory.Make(CountrySelection.Canada);
            var regions = usStates.Concat(canadianProvinces);

            foreach (var region in regions)
            {
                if (text == region.Name || text == region.Abbreviation || text == region.AlternateAbbreviation)
                {
                    return region.Name;
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
