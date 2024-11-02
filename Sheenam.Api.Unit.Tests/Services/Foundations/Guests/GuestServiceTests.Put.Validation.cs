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
        public async Task ShouldThrowValidationExceptionOnModifyIfCompanyIsNullAndLogItAsync()
        {
            // given
            Guest nullGuest = null;
            var nullGuestException = new NullGuestException();

            var expectedGuestValidationException =
                new GuestValidationException(nullGuestException);

            // when
            ValueTask<Guest> modifyGuestTask = this.guestService.ModifyGuestAsync(nullGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(modifyGuestTask.AsTask);

            // then
            actualGuestValidationException.Should().BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfCompanyIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            var invalidGuest = new Guest
            {
                LastName = invalidString
            };

            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
            key: nameof(Guest.Id),
                values: "Id is required");

            invalidGuestException.AddData(
                key: nameof(Guest.LastName),
                values: "Text is Require");

            invalidGuestException.AddData(
                key: nameof(Guest.DateOfBirth),
                values: "Date is Require");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(invalidGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    modifyGuestTask.AsTask);

            // then
            actualGuestValidationException.Should().BeEquivalentTo(expectedGuestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCompanyDoesNotExistAndLogItAsync()
        {
            // given
            int negativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTimeOffset();
            Guest randomCompany = CreateRandomGuest(dateTime);
            Guest nonExistGuest = randomCompany;
            nonExistGuest.DateOfBirth = dateTime.AddMinutes(negativeMinutes);
            Guest nullGuest = null;

            var notFoundGuestException = new GuestNotFoundException(nonExistGuest.Id);

            var expectedGuestValidationException =
                new GuestValidationException(notFoundGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(nonExistGuest.Id))
                    .ReturnsAsync(nullGuest);

            // when
            ValueTask<Guest> modifyGuestTask =
                this.guestService.ModifyGuestAsync(nonExistGuest);

            GuestValidationException actualGuestValidationException =
                await Assert.ThrowsAsync<GuestValidationException>(
                    modifyGuestTask.AsTask);

            // then
            actualGuestValidationException.Should().BeEquivalentTo(expectedGuestValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(nonExistGuest.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGuestValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        
    }
}
