//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public interface IGuestService
    {
        ValueTask<Guest> AddGuestAsync(Guest guest);
        ValueTask<Guest> RemoveGuestByIdAsync(Guid guestId);
        ValueTask<Guest> DeleteGuestAsync(Guid guestId);
        ValueTask<Guest> RetrieveGuestByIdAsync(Guid guestId);
        ValueTask<IEnumerable<Guest>> RetrieveAllGuestsAsync();
        ValueTask<Guest> UpdateGuestAsync(Guid guestId, Guest guest);

    }
}
