//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService : IGuestService
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

        public ValueTask<Guest> AddGuestAsync(Guest guest) =>
            TryCatch(async () =>
            {
                ValidateGuestOnAdd(guest);
                return await this.storageBroker.InsertGuestAsync(guest);
                throw new System.Exception("Fake Exception");
            });

        public ValueTask<Guest> DeleteGuestAsync(Guid guestId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Guest> RemoveGuestByIdAsync(Guid guestId)
        {
            IsInvalid(guestId);

            Guest guestToRemove = await this.storageBroker.SelectGuestByIdAsync(guestId);

            if (guestToRemove == null)
            {
                throw new GuestNotFoundException(guestId);
            }

            return await this.storageBroker.DeleteGuestAsync(guestId);
        }
    }
}
