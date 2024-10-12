//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public class GuestService : IGuestService
    {
        private readonly IstorageBroker storageBroker;

        public GuestService(IstorageBroker storageBroker) => this.storageBroker = storageBroker;

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
            this.storageBroker.InsertGuestAsync(guest);
    }
}
