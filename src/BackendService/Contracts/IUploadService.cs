// -----------------------------------------------------------------------
// <copyright file="IUploadService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.Service.Contracts
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Contracts for a upload service
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Uploads the file stream (interpreted as a user profile photo) to the user's profile photo section in the repository
        /// </summary>
        /// <param name="pictureStream">The file stream to send</param>
        /// <param name="userEmail">The email of the user making this request</param>
        /// <returns>The URL of the uploaded photo</returns>
        public Task<string> UploadProfilePhotoAsync(Stream pictureStream, string userEmail);

        /// <summary>
        /// Uploads the file stream (interpreted as a user profile photo) to the user's profile photo section in the repository
        /// </summary>
        /// <param name="pictureStream">The file stream to send</param>
        /// <param name="id">The id of the user to upload this photo as their profile photo</param>
        /// <param name="userEmail">The email of the user making this request</param>
        /// <returns>The URL of the uploaded photo</returns>
        public Task<string> UploadProfilePhotoAsync(Stream pictureStream, Guid id, string userEmail);
    }
}
