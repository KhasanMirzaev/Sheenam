//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free to Use To Find Comfort and Peace
//=================================================

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestsController : Controller
    {
        private readonly IGuestService guestService;

        public GuestsController(IGuestService guestService)
        {
            this.guestService = guestService;
        }

        [HttpPost]
        public async Task<IActionResult> PostGuest(Guest guest)
        {
            return Ok(await this.guestService.AddGuestAsync(guest));
        }
    }
}
