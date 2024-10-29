//==================================================
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

        public async ValueTask<Guest> DeleteGuestAsync(Guid guestId)
        {
            Guest guest = await this.SelectGuestByIdAsync(guestId);

            if (guest != null)
            {
                this.Guests.Remove(guest);
                await this.SaveChangesAsync();
            }

            return guest;
        }

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId)
        {
            // Mehmonni ma'lumotlar bazasidan olish
            return await this.Guests.FindAsync(guestId);
        }

        public async ValueTask<IEnumerable<Guest>> SelectAllGuestsAsync()
        {
            return await this.Guests.ToListAsync();
        }

    }
}
