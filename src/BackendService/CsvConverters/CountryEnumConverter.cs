// -----------------------------------------------------------------------
// <copyright file="CountryEnumConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class CountryEnumConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Converts a boolean type object from the CSV's text string
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="row">The row the data is on</param>
        /// <param name="memberMapData">A Helper object given by CsvHelper</param>
        /// <returns>The boolean value that corresponds to the string</returns>
        public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            switch (text)
            {
                case "United States":
                    return Country.UnitedStates;
                case "United States of America":
                    return Country.UnitedStates;
                case "America":
                    return Country.UnitedStates;
                case "US":
                    return Country.UnitedStates;
                case "USA":
                    return Country.UnitedStates;
                case "Canada":
                    return Country.Canada;
                case "CA":
                    return Country.Canada;
            }

            return null;
        }
    }
}
