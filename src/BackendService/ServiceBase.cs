// -----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Common;

    /// <summary>
    /// Base class for all services
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public ServiceBase(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this.Configuration = Ensure.IsNotNull(() => configuration);

            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.Logger = loggerFactory.CreateLogger<ServiceBase>();
        }

        /// <summary>
        /// Gets logger for console logging
        /// </summary>
        protected ILogger Logger { get; init; }

        /// <summary>
        /// Gets global configuration
        /// </summary>
        protected IConfiguration Configuration { get; init; }
    }
}
