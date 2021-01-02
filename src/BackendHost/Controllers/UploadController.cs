// -----------------------------------------------------------------------
// <copyright file="UploadController.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Host.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Backend.Service;
    using WahineKai.Backend.Service.Contracts;

    /// <summary>
    /// Controller for upload endpoints
    /// </summary>
    public class UploadController : ApiCommon.Controllers.ControllerBase
    {
        private readonly IUploadService uploadService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadController"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory given by ASP.NET</param>
        /// <param name="configuration">Global configuration given by ASP.NET</param>
        public UploadController(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of Upload Controller beginning");

            this.uploadService = new UploadService(loggerFactory, this.Configuration);

            this.Logger.LogTrace("Construction of Upload Controller complete");
        }

        /// <summary>
        /// Endpoint for uploading a profile photo from the body of a request
        /// </summary>
        /// <returns>The URL to the uploaded profile photo</returns>
        [HttpPost]
        [ActionName("ProfilePhoto")]
        public async Task<string> UploadProfilePhotoAsync()
        {
            var pictureStream = this.Request.Body;
            var url = await this.uploadService.UploadProfilePhotoAsync(pictureStream, this.GetUserEmailFromContext());
            return url;
        }
    }
}
