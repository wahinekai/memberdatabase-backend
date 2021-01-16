// -----------------------------------------------------------------------
// <copyright file="AdminUserCsvMap.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Backend.Service
{
    using CsvHelper.Configuration;
    using WahineKai.MemberDatabase.Backend.Service.CsvConverters;
    using WahineKai.MemberDatabase.Dto.Enums;
    using WahineKai.MemberDatabase.Dto.Models;

    /// <summary>
    /// Map from a CSV file to an Admin User
    /// </summary>
    public sealed class AdminUserCsvMap : ClassMap<AdminUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserCsvMap"/> class.
        /// Maps a CSV record to an admin user
        /// </summary>
        public AdminUserCsvMap()
        {
            // Not mapping
            // id - done by default object creation
            // positions - too complicated, won't fit well in CSV anyway

            // Constant fields
            // For safety (in case some CSV uses that for something else), admin is always false
            this.Map(member => member.Admin).Ignore().Constant(false);

            // Easy fields - map to field directly
            this.Map(member => member.Email);

            // String fields with multiple names
            this.Map(member => member.FirstName).Name("FirstName", "First Name");
            this.Map(member => member.LastName).Name("LastName", "Last Name").Optional();
            this.Map(member => member.FacebookName).Name("FacebookName", "Facebook Name", "FB Name").Optional();
            this.Map(member => member.StreetAddress).Name("StreetAddress", "Street Address", "Address").Optional();
            this.Map(member => member.PayPalName).Name("PayPalName", "Pay Pal Name").Optional();
            this.Map(member => member.Occupation).Name("Occupation", "Profession").Optional();
            this.Map(member => member.PhotoUrl).Name("PhotoUrl", "Photo Url", "Photo URL").Optional();
            this.Map(member => member.Biography).Name("Bio", "Biography").Optional();
            this.Map(member => member.Boards).Name("Boards", "Board(s)").Optional();
            this.Map(member => member.Level).Name("Level").Optional();
            this.Map(member => member.Birthdate).Name("Birthdate", "Birthday", "bday").Optional();

            this.Map(member => member.Status)
                .Name("Status", "Member Status")
                .Optional()
                .Default(MemberStatus.ActivePaying);

            this.Map(member => member.NeedsNewMemberBag)
                .Name("NeedsNewMemberBag", "Needs New Member Bag", "Needs A New Member Bag", "Needs A New Member Bag?")
                .Optional()
                .Default(false);

            this.Map(member => member.SocialMediaOptOut)
                .Name("SocialMediaOptOut", "Opt Out of Social Media?", "Opt Out of Social Media")
                .Optional()
                .Default(false);

            this.Map(member => member.WonSurfboard)
                .Name("WonSurfboard", "Won A Surfboard?", "Won A Surfboard", "Won Surfboard?", "Won Surfboard")
                .Optional()
                .Default(false);

            this.Map(member => member.SurfSpots)
                .Name("SurfSpots", "Where", "Surf Spots", "Favorite Surf Spots")
                .Optional();

            this.Map(member => member.EnteredInFacebookChapter)
                .Name("EnteredInLocalChapter", "LocalChapter", "Local Chapter", "Entered in Local Facebook Chapter?", "Entered in Local Chapter")
                .Optional();

            this.Map(member => member.EnteredInFacebookWki)
                .Name("EnteredInFacebookWki", "FacebookWki", "Facebook Wki", "Entered in Facebook Wki", "Entered in Facebook Wki?", "Entered in Facebook WKI", "Entered in Facebook WKI?", "WKI", "Entered in WKI")
                .Optional();

            // TODO: Update to new chapters, WKI should be default - but shouldn't need a default
            this.Map(member => member.Chapter)
                .Name("Chapter")
                .Optional()
                .Default(Chapter.OrangeCountyLosAngeles);

            this.Map(member => member.StartedSurfing)
                .Name("Started Surfing", "StartedSurfing", "Started Surfing Date")
                .Optional();

            this.Map(member => member.DateSurfboardWon)
                .Name("Surfboard Won Date", "SurfboardWonDate", "Date Surfboard Won", "DateSurfboardWon")
                .Optional();

            this.Map(member => member.TerminatedDate)
                .Name("Terminated Date", "TerminatedDate", "Terminated")
                .Optional();

            this.Map(member => member.JoinedDate)
                .Name("Joined Date", "JoinedDate", "Joined");

            this.Map(member => member.RenewalDate)
                .Name("Renewal Date", "RenewalDate", "Renewal")
                .Optional();

            this.Map(member => member.Country)
                .Name("Country")
                .Optional()
                .Default(Country.UnitedStates);

            // Phone number needs to go into our database - so we need a specific converter type
            this.Map(member => member.PhoneNumber)
                .Name("PhoneNumber", "Phone Number")
                .Optional()
                .TypeConverter<PhoneNumberConverter>();

            // Region is complicated in spreadsheet
            // Option 1 - region from town
            // this.Map(member => member.City)
            //   .Name("Town")
            //   .Optional()
            //   .TypeConverter<CityFromTownConverter>();

            // this.Map(member => member.Region)
            //   .Name("Town")
            //   .Optional()
            //   .TypeConverter<RegionFromTownConverter>();

            // this.Map(member => member.PostalCode)
            //   .Name("Town")
            //   .Optional()
            //   .TypeConverter<PostalCodeFromTownConverter>();

            // Option 2 - Region in all seperate containers
            this.Map(member => member.City).Name("City").Optional();

            this.Map(member => member.Region)
             .Name("Region", "State", "Province", "State or Province")
             .Optional()
             .TypeConverter<RegionConverter>();

            this.Map(member => member.PostalCode)
              .Name("PostalCode", "Postal Code", "Post Code", "ZIP Code")
              .Optional()
              .TypeConverter<PostalCodeConverter>();
        }
    }
}
