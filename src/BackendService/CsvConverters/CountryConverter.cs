// -----------------------------------------------------------------------
// <copyright file="CountryConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using CountryData.Standard;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class CountryConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Country Helper from CountryData.Standard for Country and State Validation
        /// </summary>
        protected static readonly CountryHelper CountryHelper = new CountryHelper();

        /// <summary>
        /// Converts a boolean type object from the CSV's text string
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="row">The row the data is on</param>
        /// <param name="memberMapData">A Helper object given by CsvHelper</param>
        /// <returns>The boolean value that corresponds to the string</returns>
        public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            foreach (var country in CountryHelper.GetCountryData())
            {
                if (text.ToLower() == country.CountryName.ToLower() || text.ToLower() == country.CountryShortCode.ToLower())
                {
                    return country.CountryName;
                }
            }

            return null;
        }
    }
}
