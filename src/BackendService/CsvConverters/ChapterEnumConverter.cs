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
            switch (text.ToLower())
            {
                case "virginia beach":
                    return Chapter.VirginiaBeach;
                case "san diego" or "sandiego" or "sd":
                    return Chapter.SanDiego;
                case "orange county" or "orangecounty" or "oc" or "orangecountylosangeles"
                    or "orange county/los angeles" or "ocla" or "la" or "losangeles" or "los angeles":
                    return Chapter.OrangeCountyLosAngeles;
                case "ventura" or "santabarbara" or "venturasantabarbara" or "ventura/santa barbara":
                    return Chapter.VenturaSantaBarbara;
                case "santacruz" or "santa cruz" or "sc" or "sf" or "santa cruz/san francisco"
                    or "santacruzsanfransisco" or "scsf" or "san fran" or "san francisco" or "sanfrancisco":
                    return Chapter.SantaCruzSanFrancisco;
                case "oregon":
                    return Chapter.Oregon;
                case "delnorteoregon" or "del norte oregon":
                    return Chapter.DelNorteOregon;
                case "washington" or "wa state":
                    return Chapter.Washington;
                case "hawaii":
                    return Chapter.Hawaii;
                case "maine" or "ne" or "new england" or "newengland":
                    return Chapter.NewEngland;
                case "new jersey" or "nj" or "newjersey":
                    return Chapter.NewJersey;
                case "staugustine" or "augustine" or "st augustine" or "st. augustine"
                    or "st.augustine" or "florida" or "staugustineflorida" or "st augustine florida"
                    or "augustineflorida" or "augustine florida" or "st. augustine florida"
                    or "st.augustineflorida" or "fl":
                    return Chapter.StAugustineFlorida;
                case "new york" or "ny" or "newyork" or "rockawaybeach" or "rockaway beach"
                    or "rockawaybeachnewyork" or "rockaway beach new york" or "rockaway"
                    or "rockawaynewyork" or "rockaway new york":
                    return Chapter.RockawayBeachNewYork;
            }

            return Chapter.WahineKaiInternational;
        }
    }
}
