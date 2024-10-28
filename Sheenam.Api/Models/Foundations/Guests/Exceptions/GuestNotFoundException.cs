namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestNotFoundException : Exception
    {
        public GuestNotFoundException(Guid guestId)
           : base($"Guest with id {guestId} was not found.") { }
    }
}
