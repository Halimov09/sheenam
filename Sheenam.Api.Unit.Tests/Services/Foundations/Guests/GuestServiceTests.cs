//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using FluentAssertions;
using Moq;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        private readonly Mock<IstorageBroker> storageBrokerMock;
        private readonly IGuestService guestService;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IstorageBroker>();

            this.guestService =
                new GuestService(storageBroker:this.storageBrokerMock.Object);
        }

        [Fact]
        public async Task ShoulAddGuestAsync()
        {
            // Arrange
            Guest randomGuest = new Guest
            {
                Id = Guid.NewGuid(),
                FirstName = "Jhon",
                LastName = "kroos",
                DateOfBirth = DateTime.Now,
                Address = "street05",
                Email = "ofweivrw@gmail.com",
                PhoneNumber = "1234567890",
                Gender = GenderType.Male,
            };

            this.storageBrokerMock.Setup(broker => 
            broker.InsertGuestAsync(randomGuest))
                .ReturnsAsync(randomGuest);

            //Act
            Guest actual = await this.guestService.AddGuestAsync(randomGuest);

            //Assert
            actual.Should().BeEquivalentTo(randomGuest);
        }
    }
}
