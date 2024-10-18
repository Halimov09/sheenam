//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using AutoFixture;
using FluentAssertions;
using FluentAssertions.Primitives;
using Moq;
using Sheenam.Api.Brokers.Loggings;
using Sheenam.Api.Brokers.Storages;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;
using System.Linq.Expressions;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        private readonly Mock<IstorageBroker> storageBrokerMock;
        private readonly Mock<IloggingBroker> loggingBrokerMock;
        private readonly IGuestService guestService;

        public GuestServiceTests()
        {
            this.storageBrokerMock = new Mock<IstorageBroker>();
            this.loggingBrokerMock = new Mock<IloggingBroker>();

            this.guestService = new GuestService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Guest CreateRandomGuest() =>
            CreateGuestFiller(date: GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)
        {
            return actualException => 
               actualException.Message == expectedException.Message
               && actualException.InnerException.Message == expectedException.InnerException.Message
               && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Filler<Guest> CreateGuestFiller(DateTimeOffset date)
        {
            var filler = new Filler<Guest>();

            filler.Setup()
                .OnType<DateTimeOffset>()
                .Use(date);

            return filler;
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
