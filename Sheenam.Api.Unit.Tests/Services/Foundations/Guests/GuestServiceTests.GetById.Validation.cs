//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Moq;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Models.Foundations.Guests;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidClientId = Guid.Empty;
            var invalidClientException = new InvalidGuestException();

            invalidClientException.AddData(
                key: nameof(Guest.Id),
                values: "Id is required");

            var expectedClientValidationException =
                new GuestValidationException(invalidClientException);

            // when
            ValueTask<Guest> retrieveClientById =
                this.guestService.RetrieveGuestByIdAsync(invalidClientId);

            GuestValidationException actualClientValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(retrieveClientById.AsTask);

            // then
            actualClientValidationException.Should().BeEquivalentTo(expectedClientValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClientValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfClientNotFoundAndLogItAsync()
        {
            // given
            Guid someclientId = Guid.NewGuid();
            Guest noClient = null;

            var notFoundClientException =
                new NotFoundGuestException(someclientId);

            var expetedClientValidationException =
                new GuestValidationException(notFoundClientException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(
                    It.IsAny<Guid>())).ReturnsAsync(noClient);

            // when
            ValueTask<Guest> retriveByIdClientTask =
                this.guestService.RetrieveGuestByIdAsync(someclientId);

            var actualClientValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    retriveByIdClientTask.AsTask);

            // then
            actualClientValidationException.Should().BeEquivalentTo(expetedClientValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(someclientId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expetedClientValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
