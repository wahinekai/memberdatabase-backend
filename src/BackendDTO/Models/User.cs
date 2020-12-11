// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using StatesAndProvinces;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO.Enums;

    /// <summary>
    /// Model of a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Container Id for this model
        /// </summary>
        public const string ContainerId = "Users";

        /// <summary>
        /// Partion key for this container
        /// </summary>
        public const string PartitionKey = "/Email";

        /// <summary>
        /// Gets Azure Cosmos DB id for this user
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual Guid Id { get; init; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets a value indicating whether a user is an admin user
        /// </summary>
        public bool? Admin { get; set; }

        /// <summary>
        /// Gets or sets user first name, required
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets user last name, not required (in the case where people don't have/don't want to share last names)
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a member is active, required, defaults to false
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or sets a member's facebook name, not required
        /// </summary>
        public string? FacebookName { get; set; }

        /// <summary>
        /// Gets or sets a member's PayPal Name, not required
        /// </summary>
        public string? PayPalName { get; set; }

        /// <summary>
        /// Gets user email address, required
        /// </summary>
        [EmailAddress]
        public string? Email { get; init; }

        /// <summary>
        /// Gets or sets user phone number, not required
        /// </summary>
        [Phone]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's street address, not required
        /// </summary>
        public string? StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's city, not required
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the user's state or province, not required.  Must belong to states in supported countries in settings.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the user's country, not required.  Must belong to set of supported countries in settings.
        /// </summary>
        public Country? Country { get; set; }

        /// <summary>
        /// Gets or sets the user's occupation, not required
        /// </summary>
        public string? Occupation { get; set; }

        /// <summary>
        /// Gets or sets the user's chapter, required.  Must belong to set of supported chapters in settings
        /// </summary>
        public Chapter? Chapter { get; set; }

        /// <summary>
        /// Gets or sets the user's birth date - not required.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Gets the age of the user
        /// </summary>
        public int? Age { get => this.CalculateAge(); }

        /// <summary>
        /// Gets or sets surfer level
        /// </summary>
        public Level? Level { get; set; }

        /// <summary>
        /// Gets or sets the date a member started surfing
        /// </summary>
        public DateTime? StartedSurfing { get; set; }

        /// <summary>
        /// Gets or sets a member's boards
        /// </summary>
        public ICollection<string> Boards { get; set; } = new Collection<string>();

        /// <summary>
        /// Gets or sets URL of a user's profile photo
        /// </summary>
        public string? PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets a user's biography
        /// </summary>
        public string? Biography { get; set; }

        /// <summary>
        /// Gets or sets the date the user joined the Wahine Kais, required
        /// </summary>
        public DateTime? JoinedDate { get; set; }

        /// <summary>
        /// Gets or sets the date the user needs to renew their membership
        /// </summary>
        public DateTime? RenewalDate { get; set; }

        /// <summary>
        /// Gets or sets the date the user terminated their membership with the Wahine Kais
        /// </summary>
        public DateTime? TerminatedDate { get; set; }

        /// <summary>
        /// Gets or sets the leadership position of the user.  Null means no leadership position
        /// </summary>
        public Position? Position { get; set; }

        /// <summary>
        /// Gets or sets the date the user started their particular position
        /// </summary>
        public DateTime? DateStartedPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in the facebook chapter group
        /// </summary>
        public EnteredStatus? EnteredInFacebookChapter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in facebook WKI
        /// </summary>
        public EnteredStatus? EnteredInFacebookWki { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user needs a new member bag
        /// </summary>
        public bool? NeedsNewMemberBag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a user has won a surfboard
        /// </summary>
        public bool? WonSurfboard { get; set; }

        /// <summary>
        /// Gets or sets the date a user has won a surfboard - null if the user hasn't won
        /// </summary>
        public DateTime? DateSurfboardWon { get; set; }

        /// <summary>
        /// Clone the old user, and replace it with the parameters of the updated user
        /// </summary>
        /// <param name="oldUser">READONLY old user to update</param>
        /// <param name="updatedUser">READONLY updated user to replace the parameters of the old user</param>
        /// <returns>A new user with the parameters of the old user replaced by the new user.</returns>
        public static User Replace(User oldUser, User updatedUser)
        {
            // Create a deep copy of oldUser
            var replacedUser = oldUser.Clone();

            // Update updatable parameters
            replacedUser.Admin = updatedUser.Admin ?? oldUser.Admin;
            replacedUser.FirstName = updatedUser.FirstName ?? oldUser.FirstName;
            replacedUser.LastName = updatedUser.LastName ?? oldUser.LastName;
            replacedUser.Active = updatedUser.Active ?? oldUser.Active;
            replacedUser.FacebookName = updatedUser.FacebookName ?? oldUser.FacebookName;
            replacedUser.PayPalName = updatedUser.PayPalName ?? oldUser.PayPalName;
            replacedUser.PhoneNumber = updatedUser.PhoneNumber ?? oldUser.PhoneNumber;
            replacedUser.StreetAddress = updatedUser.StreetAddress ?? oldUser.StreetAddress;
            replacedUser.City = updatedUser.City ?? oldUser.City;
            replacedUser.Region = updatedUser.Region ?? oldUser.Region;
            replacedUser.Country = updatedUser.Country ?? oldUser.Country;
            replacedUser.Occupation = updatedUser.Occupation ?? oldUser.Occupation;
            replacedUser.Chapter = updatedUser.Chapter ?? oldUser.Chapter;
            replacedUser.Birthdate = updatedUser.Birthdate ?? oldUser.Birthdate;
            replacedUser.Level = updatedUser.Level ?? oldUser.Level;
            replacedUser.StartedSurfing = updatedUser.StartedSurfing ?? oldUser.StartedSurfing;
            replacedUser.Boards = updatedUser.Boards ?? oldUser.Boards;
            replacedUser.PhotoUrl = updatedUser.PhotoUrl ?? oldUser.PhotoUrl;
            replacedUser.Biography = updatedUser.Biography ?? oldUser.Biography;
            replacedUser.JoinedDate = updatedUser.JoinedDate ?? oldUser.JoinedDate;
            replacedUser.RenewalDate = updatedUser.RenewalDate ?? oldUser.RenewalDate;
            replacedUser.TerminatedDate = updatedUser.TerminatedDate ?? oldUser.TerminatedDate;
            replacedUser.Position = updatedUser.Position ?? oldUser.Position;
            replacedUser.DateStartedPosition = updatedUser.DateStartedPosition ?? oldUser.DateStartedPosition;
            replacedUser.EnteredInFacebookChapter = updatedUser.EnteredInFacebookChapter ?? oldUser.EnteredInFacebookChapter;
            replacedUser.EnteredInFacebookWki = updatedUser.EnteredInFacebookWki ?? oldUser.EnteredInFacebookWki;
            replacedUser.NeedsNewMemberBag = updatedUser.NeedsNewMemberBag ?? oldUser.NeedsNewMemberBag;
            replacedUser.WonSurfboard = updatedUser.WonSurfboard ?? oldUser.WonSurfboard;
            replacedUser.DateSurfboardWon = updatedUser.DateSurfboardWon ?? oldUser.DateSurfboardWon;

            // Validate and return replaced user
            replacedUser.Validate();
            return replacedUser;
        }

        /// <summary>
        /// Ensure this object is a valid user record
        /// </summary>
        public void Validate()
        {
            // User must have a name
            this.FirstName = Ensure.IsNotNullOrWhitespace(() => this.FirstName);

            // Email is primary key, required, can't be changed
            Ensure.IsNotNullOrWhitespace(() => this.Email);

            // Every user belongs to a chapter
            this.Chapter = Ensure.IsNotNull(() => this.Chapter);

            // Every user must have a joined date
            this.JoinedDate = Ensure.IsNotNull(() => this.JoinedDate);

            // User must be either terminated or have a renewal date
            if (this.TerminatedDate == null)
            {
                Ensure.IsNotNull(() => this.RenewalDate);
            }

            // If user has a leadership position, they must also have a date started
            if (this.Position != null)
            {
                Ensure.IsNotNull(() => this.DateStartedPosition);
            }

            // If user has won a surfboard, must have the date won
            if (this.WonSurfboard == true)
            {
                Ensure.IsNotNull(() => this.DateSurfboardWon);
            }

            // Country & state validation
            if (this.Region != null)
            {
                // Country cannot be null if Region is
                Ensure.IsNotNull(() => this.Country);

                #pragma warning disable CS8629 // Nullable value checked for above

                // Cast country to CountrySelection to check for states
                var regions = Factory.Make((CountrySelection)this.Country);

                #pragma warning restore CS8629

                // Map to region names
                var regionNames = regions.Select(region => region.Name);

                // Check to see if region name contains the requested region
                if (!regionNames.Contains(this.Region))
                {
                    throw new ArgumentException($"{this.Region} is not a supported region");
                }
            }
        }

        /// <summary>
        /// Creates a deep copy of the object
        /// </summary>
        /// <returns>The new user cloned from this user</returns>
        public User Clone()
        {
            return new User
            {
                Id = this.Id,
                Admin = this.Admin,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Active = this.Active,
                FacebookName = this.FacebookName,
                PayPalName = this.PayPalName,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                StreetAddress = this.StreetAddress,
                City = this.City,
                Region = this.Region,
                Country = this.Country,
                Occupation = this.Occupation,
                Chapter = this.Chapter,
                Birthdate = this.Birthdate,
                Level = this.Level,
                StartedSurfing = this.StartedSurfing,
                Boards = this.Boards,
                PhotoUrl = this.PhotoUrl,
                Biography = this.Biography,
                JoinedDate = this.JoinedDate,
                RenewalDate = this.RenewalDate,
                TerminatedDate = this.TerminatedDate,
                Position = this.Position,
                DateStartedPosition = this.DateStartedPosition,
                EnteredInFacebookChapter = this.EnteredInFacebookChapter,
                EnteredInFacebookWki = this.EnteredInFacebookWki,
                NeedsNewMemberBag = this.NeedsNewMemberBag,
                WonSurfboard = this.WonSurfboard,
                DateSurfboardWon = this.DateSurfboardWon,
            };
        }

        /// <summary>
        /// Calculates the age of the user from the birthdate
        /// </summary>
        /// <returns>The age in years of the user</returns>
        private int? CalculateAge()
        {
            // Returns null if no birthdate given
            if (this.Birthdate == null)
            {
                return null;
            }

            var age = DateTime.Today.Year - this.Birthdate.Value.Year;

            // Account for leap years
            if (this.Birthdate?.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
