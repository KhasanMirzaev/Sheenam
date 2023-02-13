//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free to Use To Find Comfort and Peace
//=================================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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

                throw CreateAndLogCriticalDependencyException(failedGuestStorageException);
            }
            catch(DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistGuestException = 
                    new AlreadyExistGuestException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistGuestException);
            }
            //catch(Exception serviceException)
            //{
            //    var failedGuestServiceException =
            //        new FailedGuestServiceException(serviceException);

            //    throw CreateAndLogServiceException(failedGuestServiceException);
            //}
        }

        private GuestValidationException CreateAndLogValidationException(Xeption xeption)
        {
            var guestValidationException =
                    new GuestValidationException(xeption);

            this.loggingBroker.LogError(guestValidationException);

            return guestValidationException;
        }

        private GuestDependencyException CreateAndLogCriticalDependencyException(Xeption xeption)
        {
            var guestDependencyException =
                new GuestDependencyException(xeption);

            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }

        private GuestDependencyValidationException CreateAndLogDependencyValidationException(Xeption xeption)
        {
            var guestDependencyValidationException =
                new GuestDependencyValidationException(xeption);

            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }

        //private GuestServiceException CreateAndLogServiceException(Xeption xeption)
        //{
        //    var guestServiceException = new GuestServiceException(xeption);

        //    this.loggingBroker.LogError(guestServiceException);

        //    return guestServiceException;
        //}
    }
}
