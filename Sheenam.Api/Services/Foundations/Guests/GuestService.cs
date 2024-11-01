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

        public IQueryable<Guest> RetrieveAllGuests() =>
           TryCatch(() => this.storageBroker.SelectAllGuests());

        public async ValueTask<Guest> SelectGuestByIdAsync(Guid guestId)
        {
            IsInvalid(guestId);

            return await this.storageBroker.SelectGuestByIdAsync(guestId);
        }


        public ValueTask<Guest> RemoveGuestByIdAsync(Guid guestId) =>
            TryCatch(async () =>
            {
                ValidateCompanyId(guestId);

                Guest maybeGuest =
                    await this.storageBroker.SelectGuestByIdAsync(guestId);

                ValidateStorageGuestExists(maybeGuest, guestId);

                return await this.storageBroker.DeleteGuestAsync(maybeGuest);
            });

        public async ValueTask<Guest> GetGuestByIdAsync(Guid guestId)
        {
            IsInvalid(guestId);

            Guest guest = await this.storageBroker.SelectGuestByIdAsync(guestId);

            if (guest is null)
            {
                throw new GuestNotFoundException(guestId);
            }

            return guest;
        }


        public async ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId)
        {
            IsInvalid(guestId);

            Guest guest = await this.storageBroker.SelectGuestByIdAsync(guestId);

            if (guest is null)
            {
                throw new GuestNotFoundException(guestId);
            }

            return guest;
        }

      

    }
}
