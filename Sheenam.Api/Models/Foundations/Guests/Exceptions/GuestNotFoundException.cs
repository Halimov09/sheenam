using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestNotFoundException : Xeption
    {
        public GuestNotFoundException(Guid guestId)
           : base($"Guest with id {guestId} was not found.") { }
    }
}
