//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate ValueTask<Guest> ReturningGuestFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }
            catch (NullGuestException nullGuestException)
            {
                throw CreateGuestValidationException(nullGuestException);
            }
            catch (InvalidGuestException invalidGuestException)
            {
                throw CreateGuestValidationException(invalidGuestException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageGuestException = new FailedStorageGuestException(sqlException);

                throw CreateAndLogCriticalDepenDency(failedStorageGuestException);
            }
        }

        private GuestValidationException CreateGuestValidationException(Xeption exception)
        {
            var guestValidationException =
                    new GuestValidationException(exception);

            this.loggingBroker.LogError(guestValidationException);

            return guestValidationException;
        }

        private GuestDependencyException CreateAndLogCriticalDepenDency(Xeption xeption)
            {
            var guestDependencyException = new GuestDependencyException(xeption);
            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }
    }
}
