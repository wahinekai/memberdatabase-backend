﻿// -----------------------------------------------------------------------
// <copyright file="LevelEnumConverter.cs" company="Wahine Kai">
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
    public class LevelEnumConverter : DefaultTypeConverter
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
                case "Beginner":
                    return Level.Beginner;
                case "Beg":
                    return Level.Beginner;
                case "Intermediate":
                    return Level.Intermediate;
                case "Int":
                    return Level.Intermediate;
                case "Advanced":
                    return Level.Advanced;
                case "Adv":
                    return Level.Advanced;
                case "Expert":
                    return Level.Expert;
                case "Exp":
                    return Level.Expert;
            }

            return null;
        }
    }
}
