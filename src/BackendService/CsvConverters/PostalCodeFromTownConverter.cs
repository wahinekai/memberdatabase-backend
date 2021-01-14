// -----------------------------------------------------------------------
// <copyright file="PostalCodeFromTownConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using System;
    using System.Text.RegularExpressions;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class PostalCodeFromTownConverter : DefaultTypeConverter
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
            // Regex to remove all characters that are not - or number
            // Assumes there aren't any numbers in city or town
            var stringPostCode = Regex.Replace(text, "[^0-9]", string.Empty);
            try
            {
                return int.Parse(stringPostCode);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
