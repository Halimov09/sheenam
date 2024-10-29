﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages
{
    public partial interface IstorageBroker
    {
        ValueTask<Guest> InsertGuestAsync(Guest guest);
        ValueTask<Guest> SelectGuestByIdAsync(Guid guestId);
        ValueTask<Guest> DeleteGuestAsync(Guid guestId);
        ValueTask<IEnumerable<Guest>> SelectAllGuestsAsync();
    }
}
