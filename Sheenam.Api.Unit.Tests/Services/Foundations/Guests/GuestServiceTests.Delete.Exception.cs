using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Models.Foundations.Guests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace Sheenam.Api.Unit.Tests.Services.Foundations.Guests
{
    public partial class GuestServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedStorageGuestException =
                new FailedStorageGuestException(sqlException);

            var expectedCompanyDependencyException =
                new GuestDependencyException(failedStorageGuestException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someId))
                    .Throws(sqlException);

            // when
            ValueTask<Guest> removeCompanyTask =
                 this.guestService.RemoveGuestByIdAsync(someId);

            GuestDependencyException actualCompanyDependencyException =
                await Assert.ThrowsAsync<GuestDependencyException>(
                    removeCompanyTask.AsTask);

            // then
            actualCompanyDependencyException.Should().BeEquivalentTo(expectedCompanyDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCompanyDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCompanyId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCompanyServiceException = 
                new FailedGuestServiceException(serviceException);

            var expectedCompanyServiceException =
                new GuestServiceException(failedCompanyServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGuestByIdAsync(someCompanyId))
                    .Throws(serviceException);

            // when
            ValueTask<Guest> removeCompanyByIdTask =
                this.guestService.RemoveGuestByIdAsync(someCompanyId);

            GuestServiceException actualCompanyServiceException =
                await Assert.ThrowsAsync<GuestServiceException>(
                    removeCompanyByIdTask.AsTask);

            // then
            actualCompanyServiceException.Should().BeEquivalentTo(expectedCompanyServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGuestByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCompanyServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
