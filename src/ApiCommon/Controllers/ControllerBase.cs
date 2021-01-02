// -----------------------------------------------------------------------
// <copyright file="ControllerBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Api.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;

    /// <summary>
    /// Base class for all controllers
    /// </summary>
    [ApiController]
    [Route("api/v1/[Controller]/[Action]")]
    [Route("api/v1/[Controller]/[Action]/{userId:guid}")]
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory given by ASP.NET</param>
        /// <param name="configuration">Global configuration given by ASP.NET</param>
        protected ControllerBase(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            loggerFactory = Ensure.IsNotNull(() => loggerFactory);
            this.Logger = loggerFactory.CreateLogger<ControllerBase>();

            this.Configuration = Ensure.IsNotNull(() => configuration);

            this.Logger.LogTrace("Construction of ControllerBase complete");
        }

        /// <summary>
        /// Gets configuration given by .NET
        /// </summary>
        protected IConfiguration Configuration { get; init; }

        /// <summary>
        /// Gets logger given by .NET
        /// </summary>
        protected ILogger Logger { get; init; }

        /// <summary>
        /// Gets the user email from the HTTP Context
        /// </summary>
        /// <returns>The user's email</returns>
        protected string GetUserEmailFromContext()
        {
            this.Logger.LogDebug("Checking HTTP Request context to get Email");

            // Get claim from HTTP context
            var emailsClaim = this.HttpContext.User.Claims.Single(claim => claim.Type.Equals("emails"));
            Ensure.IsNotNull(() => emailsClaim);

            // Get email from claim
            var email = emailsClaim.Value;
            Ensure.IsNotNullOrWhitespace(() => email);

            this.Logger.LogTrace($"Got email {email} from the HTTP request context");

            return email;
        }
    }
}
