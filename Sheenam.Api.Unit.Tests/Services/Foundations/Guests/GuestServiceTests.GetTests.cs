using FluentAssertions;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveGuestByIdAsync()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();
            Guest storageGuest = CreateRandomGuest();
            storageGuest.Id = randomGuestId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(randomGuestId))
                    .ReturnsAsync(storageGuest);

            // when
            Guest actualGuest = await this.guestService.GetGuestByIdAsync(randomGuestId);

            // then
            actualGuest.Should().BeEquivalentTo(storageGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(randomGuestId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowExceptionOnGetIfGuestNotFound()
        {
            // given
            Guid randomGuestId = Guid.NewGuid();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(randomGuestId))
                    .ReturnsAsync(null as Guest);

            // then
            await Assert.ThrowsAsync<GuestNotFoundException>(async () =>
                await this.guestService.GetGuestByIdAsync(randomGuestId));

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(randomGuestId),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
