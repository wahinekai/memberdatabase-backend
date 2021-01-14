// -----------------------------------------------------------------------
// <copyright file="CityFromTownConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using System;
    using System.Text;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class CityFromTownConverter : DefaultTypeConverter
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
            var splitters = new string[] { " ", ", " };
            var options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var stringArray = text.Split(splitters, options);

            // If <= 2 entries, then we can't guarantee a match
            if (stringArray.Length > 2)
            {
                var stringBuilder = new StringBuilder(stringArray[0]);

                // Ignore last two entries - state & ZIP
                // Start at second entry - first seeded up there, allow a space beforehand
                for (int i = 1; i < stringArray.Length - 2; i++)
                {
                    stringBuilder.Append(' ');
                    stringBuilder.Append(stringArray[i]);
                }

                return stringBuilder.ToString();
            }

            return null;
        }
    }
}
