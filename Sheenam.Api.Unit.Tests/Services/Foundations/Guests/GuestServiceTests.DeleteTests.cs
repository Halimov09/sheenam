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
        public async Task ShouldDeleteGuestById()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();
            Guest storageGuest = CreateRandomGuest();
            storageGuest.Id = randomGuestId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(randomGuestId))
                    .ReturnsAsync(storageGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteGuestAsync(randomGuestId))
                    .ReturnsAsync(storageGuest);

            // when
            Guest actualGuest = await this.guestService.RemoveGuestByIdAsync(randomGuestId);

            // then
            actualGuest.Should().BeEquivalentTo(storageGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(randomGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(randomGuestId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowExceptionOnDeleteIfGuestNotFound()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(randomGuestId))
                    .ReturnsAsync(null as Guest);

            // when & then
            await Assert.ThrowsAsync<GuestNotFoundException>(async () =>
                await this.guestService.RemoveGuestByIdAsync(randomGuestId));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(randomGuestId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
