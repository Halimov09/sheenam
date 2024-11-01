using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class NotFoundGuestException : Xeption
    {
        public NotFoundGuestException(Guid guestId)
         : base($"Guest with id {guestId} was not found.") 
        { }
    }
}
