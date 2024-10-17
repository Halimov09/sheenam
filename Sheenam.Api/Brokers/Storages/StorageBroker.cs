//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker: EFxceptionsContext, IstorageBroker
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<StorageBroker> logger;

        public StorageBroker(IConfiguration configuration, ILogger<StorageBroker> logger)
        {
            this.configuration = configuration;
            this.Database.Migrate();
            this.logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = 
                this.configuration.GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        public override void Dispose() {}
    }
}
