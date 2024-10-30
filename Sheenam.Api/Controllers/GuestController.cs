//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : RESTFulController
    {
        private readonly IGuestService guestService;

        public GuestController(IGuestService guestService)
        {
            this.guestService = guestService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
        {
            try
            {
                Guest postedGuest = await this.guestService.AddGuestAsync(guest);

                return Created(postedGuest);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
            {
                return BadRequest(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        [HttpDelete("{guestId}")]
        public async ValueTask<ActionResult<Guest>> DeleteGuestAsync(Guid guestId)
        {
            try
            {
                Guest deletedGuest = await this.guestService.RemoveGuestByIdAsync(guestId);
                return Ok(deletedGuest);
            }
            catch (GuestNotFoundException guestNotFoundException)
            {
                return NotFound(guestNotFoundException.Message);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpGet("{guestId}")]
        public async Task<ActionResult<Guest>> GetGuestByIdAsync(Guid guestId)
        {
            try
            {
                Guest guest = await this.guestService.RetrieveGuestByIdAsync(guestId);

                if (guest is null)
                {
                    return NotFound();
                }

                return Ok(guest); 
            }
            catch (GuestNotFoundException guestNotFoundException)
            {
                return NotFound(guestNotFoundException.Message);
            }
        }

        [HttpGet]
        public async ValueTask<ActionResult<IEnumerable<Guest>>> GetAllGuestsAsync()
        {
            try
            {
                IEnumerable<Guest> guests = await this.guestService.RetrieveAllGuestsAsync();
                return Ok(guests);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        [HttpPut("{guestId}")]
        public async ValueTask<ActionResult<Guest>> UpdateGuestAsync(Guid guestId, Guest guest)
        {
            try
            {
                Guest updatedGuest = await this.guestService.UpdateGuestAsync(guestId, guest);
                return Ok(updatedGuest);
            }
            catch (GuestNotFoundException guestNotFoundException)
            {
                return NotFound(guestNotFoundException.Message);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

    }
}
