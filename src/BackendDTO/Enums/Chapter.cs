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
    /// Chapters of Wahine Kai
    /// </summary>
    public enum Chapter
    {
        /// <summary>
        /// San Diego chapter of Wahine Kai
        /// </summary>
        [EnumMember(Value = "San Diego")]
        SanDiego,

        /// <summary>
        /// Orange County & Los Angeles chapter
        /// </summary>
        [EnumMember(Value = "Orange County/Los Angeles")]
        OrangeCountyLosAngeles,

        /// <summary>
        /// Ventura chapter
        /// </summary>
        Ventura,

        /// <summary>
        /// Santa Cruz/San Francisco chapter
        /// </summary>
        [EnumMember(Value = "Santa Cruz/San Francisco")]
        SantaCruzSanFrancisco,

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
        /// Maine chapter
        /// </summary>
        Maine,
    }
}
