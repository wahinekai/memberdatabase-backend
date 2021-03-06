﻿// -----------------------------------------------------------------------
// <copyright file="BooleanConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using System;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class BooleanConverter : DefaultTypeConverter
    {
        /// <summary>
        /// Converts a boolean type object from the CSV's text string
        /// </summary>
        /// <param name="text">The text to convert</param>
        /// <param name="row">The row the data is on</param>
        /// <param name="memberMapData">A Helper object given by CsvHelper</param>
        /// <returns>The boolean value that corresponds to the string</returns>
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var splitters = new char[] { ' ', '-' };
            var options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var stringArray = text.Split(splitters, options);

            if (stringArray.Length > 0)
            {
                var yesno = stringArray[0].ToLower();
                return yesno == "yes" || yesno == "true";
            }

            return false;
        }
    }
}
