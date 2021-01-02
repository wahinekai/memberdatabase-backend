// -----------------------------------------------------------------------
// <copyright file="UploadService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common.Api.Services;
    using WahineKai.Common;
    using WahineKai.DTO;
    using WahineKai.DTO.Contracts;
    using WahineKai.DTO.Properties;
    using WahineKai.Backend.Service.Contracts;

    /// <summary>
    /// Service for uploading to an upload repository
    /// </summary>
    public class UploadService : ServiceBase, IUploadService
    {
        private readonly IUploadRepository uploadRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadService"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logger factory for this service</param>
        /// <param name="configuration">Application configuration</param>
        public UploadService(ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(loggerFactory, configuration)
        {
            this.Logger.LogTrace("Construction of Upload Service beginning");

            // Build cosmos configuration
            var azureBlobConfiguration = AzureBlobConfiguration.BuildFromConfiguration(this.Configuration);

            var profilePictureContainerName = this.Configuration["AzureBlob:ProfilePhotosContainer"];
            Ensure.IsNotNullOrWhitespace(() => profilePictureContainerName);

            this.uploadRepository = new AzureBlobUploadRepository(azureBlobConfiguration, profilePictureContainerName, loggerFactory);

            this.Logger.LogTrace("Construction of Upload Service complete");
        }

        /// <inheritdoc/>
        public async Task<string> UploadProfilePhotoAsync(Stream pictureStream, string userEmail)
        {
            // Sanity check inputs
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);
            pictureStream = Ensure.IsNotNull(() => pictureStream);

            // Get calling user and ensure user has requisite permissions to perform task
            await this.EnsureCallingUserPermissionsAsync(userEmail);

            // Add picture to repository and return URL - file name is random GUID
            var url = await this.uploadRepository.UploadAsync(Guid.NewGuid().ToString(), pictureStream);

            // Sanity check and return output
            url = Ensure.IsNotNullOrWhitespace(() => url);
            return url;
        }
    }
}
