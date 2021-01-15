// -----------------------------------------------------------------------
// <copyright file="MemberStatusEnumConverter.cs" company="Wahine Kai">
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
    public class MemberStatusEnumConverter : DefaultTypeConverter
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
                case "pending" or "unconfirmed":
                    return MemberStatus.Pending;
                case "non-paying" or "honorary" or "board" or "activenonpaying" or "active: non-paying":
                    return MemberStatus.ActiveNonPaying;
                case "lifetime" or "lifetimemember" or "lifetime member":
                    return MemberStatus.LifetimeMember;
                case "terminated" or "inactive":
                    return MemberStatus.Terminated;
            }

            // Default in import
            return MemberStatus.ActivePaying;
        }
    }
}
