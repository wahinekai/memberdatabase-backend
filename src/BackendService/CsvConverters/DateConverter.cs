// -----------------------------------------------------------------------
// <copyright file="DateConverter.cs" company="Wahine Kai">
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
    public class DateConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Converts a date type object from the CSV's text string
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="row">The row the data is on</param>
        /// <param name="memberMapData">A Helper object given by CsvHelper</param>
        /// <returns>The boolean value that corresponds to the string</returns>
        public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            try
            {
                return DateTime.Parse(text);
            }
            catch (Exception)
            {
            }

            // Normal parsing not possible, remaining options are YYYY or YYYYMM
            var numbersOnly = Regex.Replace(text, "[^0-9]", string.Empty);

            // If the regex removed non-number characters, we're in a fomat not supported - perhaps notes about an lifetime member
            if (numbersOnly.Length != text.Length)
            {
                return null;
            }

            // At this point, we are in one of three states - YYYYMM, YYYY, or unknown
            if (numbersOnly.Length == 4)
            {
                // YYYY
                try
                {
                    var year = int.Parse(numbersOnly);
                    return new DateTime(year, 1, 1);
                }
                catch (Exception)
                {
                }
            }
            else if (numbersOnly.Length == 6)
            {
                // YYYYMM
                try
                {
                    var year = int.Parse(numbersOnly.Substring(0, 4));
                    var month = int.Parse(numbersOnly.Substring(4, 2));
                    return new DateTime(year, month, 1);
                }
                catch (Exception)
                {
                }
            }

            // No string, unknown format, or parsing not possible
            return null;
        }
    }
}
