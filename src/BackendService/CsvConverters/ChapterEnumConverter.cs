// -----------------------------------------------------------------------
// <copyright file="ChapterEnumConverter.cs" company="Wahine Kai">
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
    public class ChapterEnumConverter : DefaultTypeConverter
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
                case "San Diego":
                    return Chapter.SanDiego;
                case "SanDiego":
                    return Chapter.SanDiego;
                case "SD":
                    return Chapter.SanDiego;
                case "Orange County":
                    return Chapter.OrangeCountyLosAngeles;
                case "OrangeCounty":
                    return Chapter.OrangeCountyLosAngeles;
                case "OC":
                    return Chapter.OrangeCountyLosAngeles;
                case "OrangeCountyLosAngeles":
                    return Chapter.OrangeCountyLosAngeles;
                case "Orange County/Los Angeles":
                    return Chapter.OrangeCountyLosAngeles;
                case "OCLA":
                    return Chapter.OrangeCountyLosAngeles;
                case "LA":
                    return Chapter.OrangeCountyLosAngeles;
                case "LosAngeles":
                    return Chapter.OrangeCountyLosAngeles;
                case "Los Angeles":
                    return Chapter.OrangeCountyLosAngeles;
                case "Ventura":
                    return Chapter.Ventura;
                case "SantaCruz":
                    return Chapter.SantaCruzSanFrancisco;
                case "Santa Cruz":
                    return Chapter.SantaCruzSanFrancisco;
                case "SC":
                    return Chapter.SantaCruzSanFrancisco;
                case "Santa Cruz/San Francisco":
                    return Chapter.SantaCruzSanFrancisco;
                case "SantaCruzSanFrancisco":
                    return Chapter.SantaCruzSanFrancisco;
                case "SCSF":
                    return Chapter.SantaCruzSanFrancisco;
                case "SanFrancisco":
                    return Chapter.SantaCruzSanFrancisco;
                case "San Francisco":
                    return Chapter.SantaCruzSanFrancisco;
                case "Oregon":
                    return Chapter.Oregon;
                case "Washington":
                    return Chapter.Washington;
                case "Hawaii":
                    return Chapter.Hawaii;
                case "Maine":
                    return Chapter.Maine;
            }

            return null;
        }
    }
}
