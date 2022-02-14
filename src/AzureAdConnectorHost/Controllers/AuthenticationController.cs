﻿// -----------------------------------------------------------------------
// <copyright file="AuthenticationController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using WahineKai.Common;
    using WahineKai.MemberDatabase.AzureAdConnector.Service;
    using WahineKai.MemberDatabase.AzureAdConnector.Service.Contracts;

    /// <summary>
    /// Controller for API Connector
    /// </summary>
    public class AuthenticationController : Common.Api.Controllers.ControllerBase
    {
        private readonly IAzureAdConnectorService apiConnectorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="configuration">Application configuration</param>
        public AuthenticationController(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Beginning construction of AuthenticationController");

            this.apiConnectorService = new AzureAdConnectorService(configuration, loggerFactory);

            this.Logger.LogTrace("Construction of AuthenticationController complete");
        }

        /// <summary>
        /// Validates whether a user is allowed to sign in/sign up
        /// </summary>
        /// <param name="body">The body of the request</param>
        /// <returns>An OK Result if allowed, a BadRequestObject if not</returns>
        [HttpPost]
        [ActionName("ValidateEmail")]
        public async Task<IActionResult> ValidateSignupSignInAsync([FromBody] Body body)
        {
            // If input data is null, show block page
            try
            {
                body = Ensure.IsNotNull(() => body);

                foreach (var identity in body.Identities)
                {
                    if (identity.SignInType?.StartsWith("emailAddress") ?? false)
                    {
                        var canSignUp = await this.apiConnectorService.CanSignUpFromEmail(identity.IssuerAssignedId);

                        if (!canSignUp)
                        {
                            return new OkObjectResult(new ResponseContent("ShowBlockPage", $"Email {identity.IssuerAssignedId} is not registered as an administrator in the system.  Contact cathy@wahinekai.org for help."));
                        }

                        break;
                    }
                }

                // Input validation passed successfully, return `Allow` response.
                return new OkObjectResult(new ResponseContent());
            }
            catch (Exception)
            {
                return new OkObjectResult(new ResponseContent("ShowBlockPage", "There was a problem with your request."));
            }
        }
    }
}
