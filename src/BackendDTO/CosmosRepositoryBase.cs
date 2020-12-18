// -----------------------------------------------------------------------
// <copyright file="CosmosRepositoryBase.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.Backend.DTO
{
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using WahineKai.Backend.Common;
    using WahineKai.Backend.DTO.Properties;

    /// <summary>
    /// Base class for repositories interfacing with Cosmos DB
    /// </summary>
    public abstract class CosmosRepositoryBase : RepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosRepositoryBase"/> class.
        /// </summary>
        /// <param name="cosmosConfiguration">Configuration to create connection with an Azure Cosmos DB Database</param>
        /// <param name="loggerFactory">Logger factory to create a logger</param>
        public CosmosRepositoryBase(CosmosConfiguration cosmosConfiguration, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.Logger.LogTrace("Construction of Cosmos Repository Base starting");

            this.CosmosConfiguration = Ensure.IsNotNull(() => cosmosConfiguration);
            this.CosmosConfiguration.Validate();

            var jsonSerializationSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            jsonSerializationSettings.Converters.Add(new StringEnumConverter());

            var cosmosOptions = new CosmosClientOptions() { Serializer = new CosmosJsonSerializer(jsonSerializationSettings) };

            this.CosmosClient = new CosmosClient(this.CosmosConfiguration.EndpointUrl, this.CosmosConfiguration.PrimaryKey, cosmosOptions);

            this.Logger.LogTrace("Construction of Cosmos Repository Base complete");
        }

        /// <summary>
        /// Gets Cosmos DB Configuration
        /// </summary>
        protected CosmosConfiguration CosmosConfiguration { get; }

        /// <summary>
        /// Gets client for interacting with Cosmos DB
        /// </summary>
        protected CosmosClient CosmosClient { get; }
    }
}
