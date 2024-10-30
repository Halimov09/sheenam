//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldUpdateGuestAsync()
        {
            // Arrange
            Guid randomGuestId = Guid.NewGuid();
            Guest randomGuest = CreateRandomGuest();
            randomGuest.Id = randomGuestId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(randomGuestId))
                    .ReturnsAsync(randomGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(randomGuest))
                    .ReturnsAsync(randomGuest);

            // Act
            Guest actualGuest = await this.guestService.UpdateGuestAsync(randomGuestId, randomGuest);

            // Assert
            actualGuest.Should().BeEquivalentTo(randomGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(randomGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(randomGuest),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowExceptionOnUpdateIfGuestNotFound()
        {
            // Arrange
            Guid randomGuestId = Guid.NewGuid();
            Guest randomGuest = CreateRandomGuest();
            randomGuest.Id = randomGuestId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(randomGuestId))
                    .ReturnsAsync(null as Guest);

            // Act & Assert
            await Assert.ThrowsAsync<GuestNotFoundException>(async () =>
                await this.guestService.UpdateGuestAsync(randomGuestId, randomGuest));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(randomGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(It.IsAny<Guest>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
