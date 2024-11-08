using Microsoft.Data.SqlClient;
using Xeptions;

namespace Sheenam.Api.Models.Foundations.Guests.Exceptions
{
    public class GuestOneDepencyException : Xeption
    {
        public GuestOneDepencyException(SqlException innerException)
                : base(message: "Guest dependency error occurred.", innerException)
        { }
    }
}
