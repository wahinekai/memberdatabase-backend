// -----------------------------------------------------------------------
// <copyright file="RequestLoggingMiddleware.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Common.Api.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Middleware for logging requests
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next function to be called in the chain</param>
        /// <param name="loggerFactory">Factory to creat loggers</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }

        /// <summary>
        /// Log the request information, then perform the next function
        /// </summary>
        /// <param name="context">The HTTP context of the request</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            finally
            {
                this.logger.LogInformation($"Request {context.Request?.Method} {context.Request?.Path.Value} => {context.Response?.StatusCode}.\n");
            }
        }
    }
}
