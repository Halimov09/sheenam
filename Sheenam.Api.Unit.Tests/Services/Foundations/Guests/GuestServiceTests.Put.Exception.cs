//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given 
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid companyId = someGuest.Id;
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedStorageGuestException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(companyId))
                    .ThrowsAsync(sqlException);

            // when 
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            GuestDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(
                    modifyGuestTask.AsTask);

            // then
            actualCompanyDependencyException.Should()
                .BeEquivalentTo(expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(companyId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given 
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid GuestId = someGuest.Id;
            var databaseUpdateException = new DbUpdateException();

            var failedStorageGuestException =
                new FailedGuestServiceException(databaseUpdateException);

            var expectedGuestDependencyException =
                new GuestServiceException(failedStorageGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(GuestId))
                    .ThrowsAsync(databaseUpdateException);

            // when 
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            GuestServiceException actualguestDependencyException =
                await Assert.ThrowsAsync<GuestServiceException>(modifyGuestTask.AsTask);

            // then
            actualguestDependencyException.Should()
                .BeEquivalentTo(expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(GuestId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task
            ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given 
            Guest randomGuest = CreateRandomGuest();
            Guest someGuest = randomGuest;
            Guid GuestId = someGuest.Id;
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGuestException =
                new FailedGuestServiceException(databaseUpdateConcurrencyException);

            var expectedGuestDependencyException =
                new GuestServiceException(lockedGuestException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectGuestByIdAsync(GuestId)).ThrowsAsync(databaseUpdateConcurrencyException);

            // when 
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(someGuest);

            GuestServiceException actualGuestDependencyException =
                await Assert.ThrowsAsync<GuestServiceException>(modifyGuestTask.AsTask);

            // then
            actualGuestDependencyException.Should()
                .BeEquivalentTo(expectedGuestDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(GuestId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))));

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
