//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public class GuestService : IGuestService
    {
        private readonly IstorageBroker storageBroker;
        private readonly IloggingBroker loggingBroker;

        public GuestService(
            IstorageBroker storageBroker,
            IloggingBroker loggingBroker)
            {
               this.storageBroker = storageBroker;
               this.loggingBroker = loggingBroker;
            }

        public async ValueTask<Guest> AddGuestAsync(Guest guest)
        {
            if (guest is null)
            {
                throw new GuestValidationException(new NullGuestException());
            }
            return await this.storageBroker.InsertGuestAsync(guest);
        }
    }
}
