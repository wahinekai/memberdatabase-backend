﻿// -----------------------------------------------------------------------
// <copyright file="Chapter.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Chapters of Wahine Kai add new ones here
    /// </summary>
    public enum Chapter
    {
        /// <summary>
        /// Corpus Christi chapter of Wahine Kai
        /// </summary>
        [EnumMember(Value = "Corpus Christi")]
        CorpusChristi,

        /// <summary>
        /// Virginia Beach chapter of Wahine Kai
        /// </summary>
        [EnumMember(Value = "Virginia Beach")]
        VirginiaBeach,

        /// <summary>
        /// San Diego chapter of Wahine Kai
        /// </summary>
        [EnumMember(Value = "San Diego")]
        SanDiego,

        /// <summary>
        /// Orange County and Los Angeles chapter
        /// </summary>
        [EnumMember(Value = "Orange County/Los Angeles")]
        OrangeCountyLosAngeles,

        /// <summary>
        /// Ventura chapter
        /// </summary>
        [EnumMember(Value = "Ventura/Santa Barbara")]
        VenturaSantaBarbara,

        /// <summary>
        /// Santa Cruz/San Francisco chapter
        /// </summary>
        [EnumMember(Value = "Santa Cruz/San Francisco")]
        SantaCruzSanFrancisco,

        /// <summary>
        /// Del Norte Oregon chapter
        /// </summary>
        [EnumMember(Value = "Del Norte Oregon")]
        DelNorteOregon,

        /// <summary>
        /// Oregon chapter
        /// </summary>
        Oregon,

        /// <summary>
        /// Washington (state) chapter
        /// </summary>
        Washington,

        /// <summary>
        /// Hawaii chapter
        /// </summary>
        Hawaii,

        /// <summary>
        /// New England chapter
        /// </summary>
        [EnumMember(Value = "New England")]
        NewEngland,

        /// <summary>
        /// New Jersey chapter
        /// </summary>
        [EnumMember(Value = "New Jersey")]
        NewJersey,

        /// <summary>
        /// St. Augustine Florida chapter
        /// </summary>
        [EnumMember(Value = "St. Augustine Florida")]
        StAugustineFlorida,

        /// <summary>
        /// Rockaway Beach New York chapter
        /// </summary>
        [EnumMember(Value = "Rockaway Beach New York")]
        RockawayBeachNewYork,

        /// <summary>
        /// Wahine Kai International member, no chapter
        /// </summary>
        [EnumMember(Value = "Wahine Kai International")]
        WahineKaiInternational,
    }
}
