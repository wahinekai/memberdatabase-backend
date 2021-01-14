// -----------------------------------------------------------------------
// <copyright file="StringArrayConverter.cs" company="Wahine Kai">
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

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class StringArrayConverter : DefaultTypeConverter
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
            // Split string into strings
            var splitters = new string[] { "also enjoy", "but have also surfed in", ";", ",", "&", "-", "and", "to", "and a", "and now have a", ", and", "or" };
            var options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var substrings = text.Split(splitters, options).ToList();

            // Remove invalid entries
            substrings.RemoveAll(element => element == "NA");
            substrings.RemoveAll(element => element == "up");

            return substrings;
        }
    }
}
