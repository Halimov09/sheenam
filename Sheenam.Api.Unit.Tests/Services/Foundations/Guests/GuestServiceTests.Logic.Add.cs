﻿//==================================================
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
        public async Task ShouldAddGuestAsync()
        {
            //Given
            Guest rundomGuest = CreateRandomGuest();
            Guest inputGuest = rundomGuest;
            Guest storageGuest = inputGuest;
            Guest expectedGuest = storageGuest.DeepClone();

            this.storageBrokerMock.Setup(broker => 
                broker.InsertGuestAsync(inputGuest))
                .ReturnsAsync(storageGuest);

            //When
            Guest actualGuest = await this.guestService.AddGuestAsync(inputGuest);

            //Then
            actualGuest.Should().BeEquivalentTo(expectedGuest);

            this.storageBrokerMock.Verify(broker => 
                broker.InsertGuestAsync(inputGuest), 
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        private Guest CreateRandomGuest()
        {
            throw new NotImplementedException();
        }
    }
}
