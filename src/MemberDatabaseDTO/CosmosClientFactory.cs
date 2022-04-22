// -----------------------------------------------------------------------
// <copyright file="CosmosClientFactory.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto
{
    using Microsoft.Azure.Cosmos;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using WahineKai.MemberDatabase.Dto.Contracts;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public sealed class CosmosClientFactory : ICosmosClientFactory
    {
        /// <summary>
        /// The options to be used when creating clients in this factory.
        /// </summary>
        private readonly CosmosClientOptions cosmosOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosClientFactory"/> class.
        /// </summary>
        public CosmosClientFactory()
        {
            // Create custom JSON serializer for enums
            var jsonSerializationSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            jsonSerializationSettings.Converters.Add(new StringEnumConverter());

            // Create cosmos client
            this.cosmosOptions = new CosmosClientOptions() { Serializer = new CosmosJsonSerializer(jsonSerializationSettings) };
        }

        /// <inheritdoc/>
        public CosmosClient GetCosmosClient(string connectionString)
            => new (connectionString, this.cosmosOptions);
    }
}
