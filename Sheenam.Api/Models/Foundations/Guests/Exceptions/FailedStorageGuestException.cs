//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class FailedStorageGuestException : Xeption
    {
        public FailedStorageGuestException(Exception innerExcepption)
            : base(message: "Failed guest storage error occured, contact support", 
                  innerExcepption)
        { }
    }
}
