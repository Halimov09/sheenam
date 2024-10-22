//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests.Exceptions;
using Xeptions;


namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionONAddIfGuestIsNullAndLOggitAsync()
        {
            //given
            Guest nullGuest = null;
            var nullGuestException = new NullGuestException();

            var expectedGuestValidationException =
                new GuestValidationException(nullGuestException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(nullGuest);

            //then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
            addGuestTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
            Times.Once);

            this.storageBrokerMock.Verify(broker => 
            broker.InsertGuestAsync(It.IsAny<Guest>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async  Task ShouldThrowValidationExceptionOnAddIfGuestIsInvalidAndLogitAsync(
            string invalidText)
        {
            //given
            var invalidGuest = new Guest()
            {
                FirstName = invalidText
            };
            
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(key: nameof(Guest.Id),
                values: "Id is Required");

            invalidGuestException.AddData(key: nameof(Guest.FirstName),
                values: "FirstName is Required");

            invalidGuestException.AddData(key: nameof(Guest.LastName),
                values: "LastName is Required");

            invalidGuestException.AddData(key: nameof(Guest.DateOfBirth),
                values: "DateOfBirth is Required");

            invalidGuestException.AddData(key: nameof(Guest.Email),
                values: "Values is Required");

            invalidGuestException.AddData(key: nameof(Guest.Address),
                values: "Address is Required");

            var ExpectedGuestValidationException = 
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> addGuestTask = 
                this.guestService.AddGuestAsync(invalidGuest);


            // then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                addGuestTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is<GuestValidationException>(actualException =>
                    actualException.Message == ExpectedGuestValidationException.Message)),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(It.IsAny<Guest>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfGenderIsInvalidAndLogItAsync()
        {
            //given
            Guest randomGuest = CreateRandomGuest();
            Guest invalidGuest = randomGuest;
            invalidGuest.Gender = GetTInvalidEnum<GenderType>();
            var invalidGuestException = new InvalidGuestException();

            invalidGuestException.AddData(
                key: nameof(Guest.Gender),
                values: "value is invalid");

            var expectedGuestValidationException =
                new GuestValidationException(invalidGuestException);

            //when
            ValueTask<Guest> addGuestTask =
                this.guestService.AddGuestAsync(invalidGuest);

            //then
            await Assert.ThrowsAsync<GuestValidationException>(() =>
                addGuestTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(It.IsAny<Guest>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
