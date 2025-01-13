//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate ValueTask<Guest> ReturningGuestFunction();
        private delegate IQueryable<Guest> ReturningGuestsFunction();

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
            catch (GuestNotFoundException guestNotFoundException)
            {
                var guestValidationException = new GuestValidationException(guestNotFoundException);

                this.loggingBroker.LogError(guestValidationException);
                throw guestValidationException;
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistGuestException = new AlreadyExistGuestException(duplicateKeyException);

                throw CreateAndAlreadyExistGuestExcption(alreadyExistGuestException);
            }
            catch (Exception exeption)
            {
                var failedGuestServiceException = new FailedGuestServiceException(exeption);

                throw CreateAndLogGuestServiceException(failedGuestServiceException);
            }


        }

        private IQueryable<Guest> TryCatch(ReturningGuestsFunction
            returningCompaniesFunction)
        {
            try
            {
                return returningCompaniesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedGuestServiceException =
                    new FailedGuestServiceException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedGuestServiceException);
            }
            catch (Exception exception)
            {
                var failedGuestServiceException = new FailedGuestServiceException(exception);

                throw CreateAndLogGuestServiceException(failedGuestServiceException);
            }
        }

        private GuestDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var guestDependencyException = new GuestDependencyException(exception);
            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
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

        private GuestDependencyValidationException CreateAndAlreadyExistGuestExcption(Xeption xeption)
        {
            var guestDependencyValidationException = new GuestDependencyValidationException(xeption);
            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }

        private GuestServiceException CreateAndLogGuestServiceException(Xeption xeption)
        {
            var guestServiceException = new GuestServiceException(xeption);

            this.loggingBroker.LogError(guestServiceException);

            return guestServiceException;
        }
    }
}
