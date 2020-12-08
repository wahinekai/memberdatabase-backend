// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Class defining the behavior of the global configuration settings in use by our application
    /// </summary>
    public class Settings
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="configuration">ASP.NET Configuration</param>
        public Settings(IConfiguration configuration)
        {
            this.configuration = Ensure.IsNotNull(() => configuration);
        }
    }
}
