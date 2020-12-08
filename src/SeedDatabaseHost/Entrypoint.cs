// -----------------------------------------------------------------------
// <copyright file="Entrypoint.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.SeedDatabase.Host
{
    using WahineKai.Backend.Common.Contracts;

    /// <summary>
    /// Entrypoint for SeedDatabaseHost console application
    /// </summary>
    public sealed class Entrypoint : IEntrypoint
    {
        /// <summary>
        /// Main method entrypoint
        /// </summary>
        public static void Main()
        {
            var program = new Entrypoint();
            program.Start();
        }

        /// <inheritdoc/>
        public void Start()
        {
            var seeder = new DatabaseSeeder();
            seeder.Clear();
            seeder.Seed();
        }
    }
}
