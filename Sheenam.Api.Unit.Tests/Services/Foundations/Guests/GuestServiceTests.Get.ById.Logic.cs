//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveGuestByIdLogicAsync()
        {
            // given
            Guid inputGuestId = Guid.NewGuid();
            Guest randomGuest = CreateRandomGuest();
            Guest expectedGuest = randomGuest;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(inputGuestId))
                    .ReturnsAsync(expectedGuest);

            // when
            Guest actualGuest =
                await this.guestService.RetrieveGuestByIdAsync(inputGuestId);

            // then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(inputGuestId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
