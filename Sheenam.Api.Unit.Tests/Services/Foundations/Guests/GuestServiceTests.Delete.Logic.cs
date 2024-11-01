//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldRemoveCompanyByIdAsync()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();
            Guid inputGuestId = randomGuestId;
            Guest randomGuest = CreateRandomGuest();
            Guest storageGueest = randomGuest;
            Guest expectedInputGuest = storageGueest;
            Guest deletedGuest = expectedInputGuest;
            Guest expectedGuest = deletedGuest.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(inputGuestId))
                    .ReturnsAsync(storageGueest);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteGuestAsync(expectedInputGuest))
                    .ReturnsAsync(deletedGuest);

            // when

            Guest actualGuest = await this.guestService
                .RemoveGuestByIdAsync(inputGuestId);

            // then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(inputGuestId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGuestAsync(expectedInputGuest), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
