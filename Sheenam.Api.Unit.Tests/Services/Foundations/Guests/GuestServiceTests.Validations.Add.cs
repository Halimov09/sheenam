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

            //its code not long timely code its need put down to rubbish
            this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is<Xeption>(exception =>
            exception is NullGuestException)), Times.Once);

            // its code must-have change to correct code
            //this.loggingBrokerMock.Verify(broker =>
            //broker.LogError(It.Is(SameExceptionAs(expectedGuestValidationException))),
            //Times.Once);

            this.storageBrokerMock.Verify(broker => 
            broker.InsertGuestAsync(It.IsAny<Guest>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
