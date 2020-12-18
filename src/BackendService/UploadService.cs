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
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO;
    using WahineKai.Backend.DTO.Contracts;
    using WahineKai.Backend.DTO.Models;
    using WahineKai.Backend.DTO.Properties;
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

            // Get calling user and ensure user has requisite permissions to perform task
            var user = await this.GetCallingUser(userEmail);
            this.EnsureCallingUserPermissions(user);

            return await this.UploadProfilePhotoAsync(pictureStream, user.Id);
        }

        /// <inheritdoc/>
        public async Task<string> UploadProfilePhotoAsync(Stream pictureStream, Guid id, string userEmail)
        {
            // Sanity check inputs
            userEmail = Ensure.IsNotNullOrWhitespace(() => userEmail);

            // Get calling user and ensure user has requisite permissions to perform task
            await this.EnsureCallingUserPermissionsAsync(userEmail);

            return await this.UploadProfilePhotoAsync(pictureStream, id);
        }

        /// <summary>
        /// Uploads a picture stream to the database as a profile photo
        /// </summary>
        /// <param name="pictureStream">The stream to upload</param>
        /// <param name="id">The Id of the user to upload this as their profile picture</param>
        /// <returns>The url of the photo</returns>
        private async Task<string> UploadProfilePhotoAsync(Stream pictureStream, Guid id)
        {
            // Sanity check input arguments
            pictureStream = Ensure.IsNotNull(() => pictureStream);
            id = Ensure.IsNotNull(() => id);

            // File name is combination of user id and profile photo
            var fileName = $"{id}-profile-photo";

            // Add picture to repository and return URL
            var url = await this.uploadRepository.UploadAsync(fileName, pictureStream);

            // Sanity check and return output
            url = Ensure.IsNotNullOrWhitespace(() => url);
            return url;
        }
    }
}
