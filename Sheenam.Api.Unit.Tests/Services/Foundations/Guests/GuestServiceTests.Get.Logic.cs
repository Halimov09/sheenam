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
        public void ShouldRetrieveAllCompanies()
        {
            //given
            IQueryable<Guest> randomGuests = CreateRandomGuests();
            IQueryable<Guest> storageGuests = randomGuests;
            IQueryable<Guest> expectedGuests = storageGuests.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGuests()).Returns(storageGuests);

            //when
            IQueryable<Guest> actualCompanies =
                this.guestService.RetrieveAllGuests();

            //then
            actualCompanies.Should().BeEquivalentTo(expectedGuests);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGuests(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
