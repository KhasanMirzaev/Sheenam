//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free to Use Comfort and Peace
//=================================================namespace Sheenam.Api.Brokers.Storages

using Microsoft.EntityFrameworkCore;
using Sheenam.Api.Models.Foundations.Guests;

namespace Sheenam.Api.Brokers.Storages

{
    public partial class StorageBroker
    {
        public DbSet<Guest> Guests {get; set; }
    }
}