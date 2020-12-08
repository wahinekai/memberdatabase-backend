// -----------------------------------------------------------------------
// <copyright file="DatabaseSeeder.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.SeedDatabase
{
    using System;

    /// <summary>
    /// Project for seeding the development database with data
    /// </summary>
    public sealed class DatabaseSeeder
    {
        /// <summary>
        /// Adds new data to the database.  Assumes an empty database before running.
        /// </summary>
        public void Seed()
        {
            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// Clears the database
        /// </summary>
        public void Clear()
        {
            Console.WriteLine("Clearing database");
        }
    }
}
