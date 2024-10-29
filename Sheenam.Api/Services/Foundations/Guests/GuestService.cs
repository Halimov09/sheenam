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

        public async ValueTask<IEnumerable<Guest>> RetrieveAllGuestsAsync()
        {
            return await this.storageBroker.SelectAllGuestsAsync();
        }

        public async ValueTask<Guest> UpdateGuestAsync(Guid guestId, Guest guest)
        {
            Guest existingGuest = await this.storageBroker.SelectGuestByIdAsync(guestId);
            if (existingGuest == null)
            {
                throw new GuestNotFoundException(guestId);
            }

            existingGuest.FirstName = guest.FirstName; 
            existingGuest.LastName = guest.LastName; 
            existingGuest.PhoneNumber = guest.PhoneNumber;
            existingGuest.DateOfBirth = guest.DateOfBirth;
            existingGuest.Address = guest.Address;
            existingGuest.Email = guest.Email;
            existingGuest.Gender = guest.Gender;

            return await this.storageBroker.UpdateGuestAsync(existingGuest);
        }

    }
}
