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
        public async Task ShouldModifyCompanyAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Guest randomGuest = CreateRandomGuest(randomDate);
            Guest inputGuest = randomGuest;
            Guest storageGuest = inputGuest.DeepClone();
            Guest updatedGuest = inputGuest;
            Guest expectedGuest = updatedGuest.DeepClone();
            Guid companyId = inputGuest.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(companyId))
                    .ReturnsAsync(storageGuest);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGuestAsync(inputGuest))
                    .ReturnsAsync(updatedGuest);

            // when
            Guest actualGuest =
               await this.guestService.ModifyGuestAsync(inputGuest);

            // then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(companyId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGuestAsync(inputGuest), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
