// -----------------------------------------------------------------------
// <copyright file="AdminUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using WahineKai.Common.Contracts;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// Model of a user with all fields - to be worked on by admin
    /// </summary>
    public class AdminUserCSV : ReadByAllUser, IValidatable
    {
        /// <summary>
        /// Gets or sets a value indicating whether a user is an admin user
        /// </summary>
        public bool Admin { get; set; } = false;

        /// <summary>
        /// Gets or sets a member's PayPal Name, not required
        /// </summary>
        public string? PayPalName { get; set; }

        /// <summary>
        /// Gets or sets user phone number, not required
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's street address, not required
        /// </summary>
        public string? StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's birth date - not required.
        /// </summary>
        public string? Birthdate { get; set; }

        /// <summary>
        /// Gets the age of the user
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the membership status of the user.
        /// </summary>
        public MemberStatus Status { get; set; } = MemberStatus.Pending;

        /// <summary>
        /// Gets or sets the date the user joined the Wahine Kais, required
        /// </summary>
        public string? JoinedDate { get; set; }

        /// <summary>
        /// Gets or sets the date the user needs to renew their membership
        /// </summary>
        public string? RenewalDate { get; set; }

        /// <summary>
        /// Gets or sets the date the user terminated their membership with the Wahine Kais
        /// </summary>
        public string? TerminatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in the facebook chapter group
        /// </summary>
        public EnteredStatus EnteredInFacebookChapter { get; set; } = EnteredStatus.Entered;

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in facebook WKI
        /// </summary>
        public EnteredStatus EnteredInFacebookWki { get; set; } = EnteredStatus.Entered;

        /// <summary>
        /// Gets or sets a value indicating whether a user needs a new member bag
        /// </summary>
        public bool NeedsNewMemberBag { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a user has won a surfboard
        /// </summary>
        public bool WonSurfboard { get; set; } = false;

        /// <summary>
        /// Gets or sets the date a user has won a surfboard - null if the user hasn't won
        /// </summary>
        public string? DateSurfboardWon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a member has opted out of social media
        /// </summary>
        public bool SocialMediaOptOut { get; set; } = false;

        public string Boards { get; set; }
        public string SurfSpots { get; set; }
        public string Positions { get; set; }

        /// <summary>
        /// Gets or sets TimeStamp Property from Cosmos DB
        /// </summary>
        [JsonProperty(PropertyName = "_ts")]
        public long? TimeStamp { get; }

        public static AdminUserCSV ConvertUserToCSV(AdminUser user)
        {
            // Create a deep copy of oldUser
            var csvUser = new AdminUserCSV();

            // Update updatable parameters
            csvUser.Admin = user.Admin;
            csvUser.Age = user.Age;
            csvUser.Email = user.Email;
            csvUser.FirstName = user.FirstName;
            csvUser.LastName = user.LastName;
            csvUser.FacebookName = user.FacebookName;
            csvUser.PayPalName = user.PayPalName;
            csvUser.PhoneNumber = user.PhoneNumber;
            csvUser.StreetAddress = user.StreetAddress;
            csvUser.City = user.City;
            csvUser.Region = user.Region;
            csvUser.Country = user.Country;
            csvUser.Occupation = user.Occupation;
            csvUser.Chapter = user.Chapter;
            csvUser.Birthdate = csvUser.ConvertDate(user.Birthdate);
            csvUser.Level = user.Level;
            csvUser.StartedSurfing = user.StartedSurfing;
            csvUser.Boards = string.Join(":", user.Boards);
            csvUser.SurfSpots = string.Join(":", user.SurfSpots);
            csvUser.PhotoUrl = user.PhotoUrl;
            csvUser.Biography = user.Biography;
            csvUser.Status = user.Status;
            csvUser.JoinedDate = csvUser.ConvertDate(user.JoinedDate);
            csvUser.RenewalDate = csvUser.ConvertDate(user.RenewalDate);
            csvUser.TerminatedDate = csvUser.ConvertDate(user.TerminatedDate);
            csvUser.Positions = string.Join(":", user.Positions);
            csvUser.EnteredInFacebookChapter = user.EnteredInFacebookChapter;
            csvUser.EnteredInFacebookWki = user.EnteredInFacebookWki;
            csvUser.NeedsNewMemberBag = user.NeedsNewMemberBag;
            csvUser.WonSurfboard = user.WonSurfboard;
            csvUser.DateSurfboardWon = csvUser.ConvertDate(user.DateSurfboardWon);
            csvUser.PostalCode = user.PostalCode;
            csvUser.SocialMediaOptOut = user.SocialMediaOptOut;


            return csvUser;
        }

        private string ConvertDate(Nullable<DateTime> date)
        {
            DateTime? myDate = date;
            string datestring;
            if (myDate.HasValue)
            {
                datestring = myDate.Value.ToString("MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo);
            }
            else
            {
                datestring = string.Empty;
            }
            return datestring;
        }



    }
}
