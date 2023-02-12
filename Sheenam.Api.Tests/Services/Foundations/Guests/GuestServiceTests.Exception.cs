//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free to Use To Find Comfort and Peace
//=================================================

using Microsoft.Data.SqlClient;
using Moq;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;
using Xunit;

namespace Sheenam.Api.Tests.Unit.Services.Foundations.Guests
{
    public partial class GuestServiceTests : Xeption
    {
        [Fact]
        public async Task ShouldThrowCriticalExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guest someGuest = CreateRandomGuest();
            SqlException sqlException = GetSqlError();
            var failedGuestStorageException = new FailedGuestStorageException(sqlException);

            var expectedGuestDependencyException =
                new GuestDependencyException(failedGuestStorageException);

            //bazaga borib kelishini kutmay sqlException yuborilishi mock qilindi(sohtalashtirdik)
            this.storageBrokerMock.Setup(broker =>
                broker.InsertGuestAsync(someGuest)).ThrowsAsync(sqlException);

            //when
            ValueTask<Guest> addGuestTask = 
                this.guestService.AddGuestAsync(someGuest);

            //then
            // kutib tur: GuestDependencyException tipidagi xatolikni kelishini qachonki, biz //whenda
                //saqlab qo'ygan addGuestTask bajarilganda
            await Assert.ThrowsAsync<GuestDependencyException>(() =>
                addGuestTask.AsTask());
            
            this.storageBrokerMock.Verify(broker =>
                broker.InsertGuestAsync(someGuest),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker => 
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGuestDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
