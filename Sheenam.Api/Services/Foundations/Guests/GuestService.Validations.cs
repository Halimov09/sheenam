﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use Comfort and Peace
//==================================================

using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private void ValidateGuestOnAdd(Guest guest)
        {
            ValidateGuestNotNull(guest);

            Validate(
                (Rule: IsInvalid(guest.Id), Parameter: nameof(guest.Id)),
                (Rule: IsInvalid(guest.FirstName), Parameter: nameof(guest.FirstName)),
                (Rule: IsInvalid(guest.LastName), Parameter: nameof(guest.LastName)),
                (Rule: IsInvalid(guest.DateOfBirth), Parameter: nameof(guest.DateOfBirth)),
                (Rule: IsInvalid(guest.Email), Parameter: nameof(guest.Email)),
                (Rule: IsInvalid(guest.Address), Parameter: nameof(guest.Address)),
                (Rule: IsInvalid(guest.Gender), Parametr: nameof(guest.Gender)));
        }

        private void ValidateGuestOnModify(Guest guest)
        {
            ValidateGuestNotNull(guest);

            Validate(
                (Rule: IsInvalid(guest.Id), Parameter: nameof(guest.Id)),
                (Rule: IsInvalid(guest.LastName), Parameter: nameof(guest.LastName)),
                (Rule: IsInvalid(guest.DateOfBirth), Parameter: nameof(guest.DateOfBirth)));
        }


        private void ValidateGuestNotNull(Guest guest)
        {
            if (guest is null)
            {
                throw new NullGuestException();
            }
        }

        private void ValidateStorageGuestExists(Guest maybeGuest, Guid guestId)
        {
            if (maybeGuest is null)
            {
                throw new GuestNotFoundException(guestId);
            }
        }


        private void ValidateAgainstStorageOnModify(Guest inputGuest, Guest storageGuest)
        {
            ValidateStorageGuestExists(storageGuest, inputGuest.Id);

            Validate(
                (Rule: IsNotSame(
                    firstDate: inputGuest.DateOfBirth,
                    secondDate: storageGuest.DateOfBirth,
                    secondDateName: nameof(Guest.DateOfBirth)),
                    Parameter: nameof(Guest.DateOfBirth)));
        }


        private static dynamic IsNotSame(
           DateTimeOffset firstDate,
           DateTimeOffset secondDate,
           string secondDateName) => new
           {
               Condition = firstDate != secondDate,
               Message = $"Date is not same as {secondDateName}"
           };


        private static void ValidateCompanyId(Guid guestId) =>
            Validate((Rule: IsInvalid(guestId), Parameter: nameof(Guest.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is Require"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is Require"
        };

        private static dynamic IsInvalid(GenderType gender) => new
        {
            Condition = Enum.IsDefined(gender) is false,
            Message = "value is invalid"
        };

        private static void Validate(params (dynamic Rule, string Parametr)[] validations)
        {
            var invalidGuestException = new InvalidGuestException();

            foreach ((dynamic rule, string parametr) in validations)
            {
                if (rule.Condition)
                {
                    invalidGuestException.UpsertDataList(
                      key: parametr,
                     value: rule.Message);
                }
            }

            invalidGuestException.ThrowIfContainsErrors();
        }
    }
}
