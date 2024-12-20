﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheenam.Api.Models.Foundations.Guests;
using System.Collections.Generic;

namespace Sheenam.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests { get; set; }

        public async ValueTask<Guest> InsertGuestAsync(Guest guest)
        {
            using var broker = new StorageBroker(this.configuration, this.logger);

            EntityEntry<Guest> guestEntityEntry =
                await broker.Guests.AddAsync(guest);

            await broker.SaveChangesAsync();

            return guestEntityEntry.Entity;
        }

        public async ValueTask<Guest> DeleteGuestAsync(Guest guest) =>
            await DeleteAsync(guest);

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId) =>
           await SelectAsync<Guest>(guestId);

        public async ValueTask<Guest> UpdateGuestAsync(Guest guest) =>
            await UpdateAsync(guest);

        public IQueryable<Guest> SelectAllGuests() =>
            SelectAll<Guest>();


    }
}
