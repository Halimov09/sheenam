﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Services.Foundations.Guests;
using Microsoft.AspNetCore.OData.Query;

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

        [HttpGet("{guestId}")]
        public async Task<IActionResult> GetGuestByIdAsync(Guid guestId)
        {
            try
            {
                Guest guest = await this.guestService.GetGuestByIdAsync(guestId);

                if (guest == null)
                {
                    return NotFound(); 
                }

                return Ok(guest); 
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return StatusCode(500, guestDependencyException.Message);
            }
            catch (GuestServiceException guestServiceException)
            {
                return StatusCode(500, guestServiceException.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }


        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Guest>> GetAllGuests()
        {
            try
            {
                IQueryable<Guest> allCompanies = this.guestService.RetrieveAllGuests();

                return Ok(allCompanies);
            }
            catch (GuestDependencyException locationDependencyException)
            {
                return InternalServerError(locationDependencyException.InnerException);
            }
            catch (GuestServiceException locationServiceException)
            {
                return InternalServerError(locationServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Guest>> PutCompanyAsync(Guest guest)
        {
            try
            {
                Guest modifiedCompany = await this.guestService.ModifyGuestAsync(guest);

                return Ok(modifiedCompany);
            }
            catch (GuestValidationException guestValidationException)
                  when (guestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
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

        [HttpDelete]
        public async ValueTask<ActionResult<Guest>> DeleteCompanyByIdAsync(Guid id)
        {
            try
            {
                Guest deletedCompany = await this.guestService.RemoveGuestByIdAsync(id);

                return Ok(deletedCompany);
            }
            catch (GuestValidationException guestValidationException)
                when (guestValidationException.InnerException is GuestNotFoundException)
            {
                return NotFound(guestValidationException.InnerException);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when (guestDependencyValidationException.InnerException is LockedGuestException)
            {
                return Locked(guestDependencyValidationException.InnerException);
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

    }
}
