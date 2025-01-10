//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class FailedGuestServiceException : Xeption
    {
        private Exception exeption;

        public FailedGuestServiceException(Exception exeption)
        {
            this.exeption = exeption;
        }

        public FailedGuestServiceException(string v, Exception innerException)
            :base(message: "Failed guest service error occured, contact support",
                 innerException)
        {}
    }
}
