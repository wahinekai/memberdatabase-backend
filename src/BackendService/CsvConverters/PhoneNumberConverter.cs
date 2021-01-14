// -----------------------------------------------------------------------
// <copyright file="PhoneNumberConverter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.CsvConverters
{
    using System.Text.RegularExpressions;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;

    /// <summary>
    /// Helper for boolean conversions
    /// </summary>
    public class PhoneNumberConverter : DefaultTypeConverter
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
            // Regex to remove all characters that are not + or number
            var phoneNumber = Regex.Replace(text, "[^+0-9]", string.Empty);

            // If phone numbe exists, return it, else return null
            if (phoneNumber.Length > 0)
            {
                // If phone number exists and isn't internationalized, default it to the US
                if (phoneNumber[0] != '+')
                {
                    phoneNumber = $"+1{phoneNumber}";
                }

                return phoneNumber;
            }

            return null;
        }
    }
}
