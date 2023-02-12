//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free to Use To Find Comfort and Peace
//=================================================

using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate ValueTask<Guest> ReturningGuestFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }
            catch (NullGuestException nullGuestException)
            {
                throw CreateAndLogValidationException(nullGuestException);
            }
            catch (InvalidGuestException invalidGuestException)
            {
                throw CreateAndLogValidationException(invalidGuestException);
            }
            catch(SqlException sqlException)
            {
                var failedGuestStorageException = new FailedGuestStorageException(sqlException);

                throw CreateAndLogCriticalException(failedGuestStorageException);
            }
        }

        private GuestValidationException CreateAndLogValidationException(Xeption xeption)
        {
            var guestValidationException =
                    new GuestValidationException(xeption);

            this.loggingBroker.LogError(guestValidationException);

            return guestValidationException;
        }

        private GuestDependencyException CreateAndLogCriticalException(Xeption xeption)
        {
            GuestDependencyException guestDependencyException =
                new GuestDependencyException(xeption);

            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }
    }
}
