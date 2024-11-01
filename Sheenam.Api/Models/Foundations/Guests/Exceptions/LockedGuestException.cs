using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class LockedGuestException : Xeption
    {
        public LockedGuestException(Exception innerException)
            : base(message: "Company is locked, try again later.", innerException)
        { }
    }
}
