﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IstorageBroker
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<StorageBroker> logger;

        public StorageBroker(IConfiguration configuration, ILogger<StorageBroker> logger)
        {
            this.configuration = configuration;
            this.Database.Migrate();
            this.logger = logger;
        }

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString =
                this.configuration.GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        public async ValueTask<T> SelectAsync<T>(params object[] objectIds) where T : class
        {
            var broker = new StorageBroker(this.configuration);

            return await broker.FindAsync<T>(objectIds);
        }

        public async ValueTask<T> DeleteAsync<T>(T @object)
        {
            var broker = new StorageBroker(this.configuration);
            broker.Entry(@object).State = EntityState.Deleted;
            await broker.SaveChangesAsync();

            return @object;
        }


        public override void Dispose() { }

        public ValueTask<Guest> DeleteGuestAsync(Guest guest)
        {
            throw new NotImplementedException();
        }
    }
}
