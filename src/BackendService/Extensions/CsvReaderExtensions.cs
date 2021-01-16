// -----------------------------------------------------------------------
// <copyright file="CsvReaderExtensions.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service.Extensions
{
    using System;
    using System.Collections.Generic;
    using CsvHelper;
    using WahineKai.MemberDatabase.Backend.Service.CsvConverters;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// Project-specific extensions for CSV Reader
    /// </summary>
    public static class CsvReaderExtensions
    {
        /// <summary>
        /// Configures CSV Reader for use in this project
        /// </summary>
        /// <param name="csvReader">CSV Reader to configure</param>
        /// <returns>The Csv Reader</returns>
        public static CsvReader Configure(this CsvReader csvReader)
        {
            // Boolean handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<bool>();
            csvReader.Configuration.TypeConverterCache.AddConverter<bool>(new BooleanConverter());

            // DateTime handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<DateTime>();
            csvReader.Configuration.TypeConverterCache.AddConverter<DateTime>(new DateConverter());

            // string[] handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<ICollection<string>>();
            csvReader.Configuration.TypeConverterCache.AddConverter<ICollection<string>>(new StringArrayConverter());

            // EnteredStatus handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<EnteredStatus>();
            csvReader.Configuration.TypeConverterCache.AddConverter<EnteredStatus>(new EnteredStatusEnumConverter());

            // MemberStatus handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<MemberStatus>();
            csvReader.Configuration.TypeConverterCache.AddConverter<MemberStatus>(new MemberStatusEnumConverter());

            // Level handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<Level>();
            csvReader.Configuration.TypeConverterCache.AddConverter<Level>(new LevelEnumConverter());

            // Chapter handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<Chapter>();
            csvReader.Configuration.TypeConverterCache.AddConverter<Chapter>(new ChapterEnumConverter());

            // Country handling
            csvReader.Configuration.TypeConverterCache.RemoveConverter<Country>();
            csvReader.Configuration.TypeConverterCache.AddConverter<Country>(new CountryEnumConverter());

            // Add class map
            csvReader.Configuration.RegisterClassMap<AdminUserCsvMap>();

            return csvReader;
        }
    }
}
