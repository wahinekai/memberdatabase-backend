﻿// -----------------------------------------------------------------------
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
            switch (text.ToLower())
            {
                case "united states" or "united states of america" or "america" or "us" or "usa":
                    return Country.UnitedStates;
                case "canada" or "ca":
                    return Country.Canada;
            }

            return null;
        }
    }
}
