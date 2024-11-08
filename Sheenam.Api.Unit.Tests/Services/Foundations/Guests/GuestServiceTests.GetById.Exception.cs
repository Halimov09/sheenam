//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedClientStorageException =
                new FailedStorageGuestException(sqlException);

            var expectedClientDependencyException =
                new GuestDependencyException(failedClientStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when 
            ValueTask<Guest> retrieveClientById =
                this.guestService.RetrieveGuestByIdAsync(someId);

            GuestDependencyException actualClientDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(
                    retrieveClientById.AsTask);

            // then
            actualClientDependencyException.Should()
                .BeEquivalentTo(expectedClientDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedClientDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            Exception serverException = new Exception();

            var failedClientServiceException =
                new FailedGuestServiceException(serverException);

            var expectedClientServiceException =
                new GuestServiceException(failedClientServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serverException);

            // when
            ValueTask<Guest> retrieveClientById =
                this.guestService.RetrieveGuestByIdAsync(someId);

            GuestServiceException actualClientServiceException =
                await Assert.ThrowsAsync<GuestServiceException>(
                    retrieveClientById.AsTask);

            // then
            actualClientServiceException.Should().BeEquivalentTo(expectedClientServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
