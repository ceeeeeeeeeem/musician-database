using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _venueService = venueService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _venueService.GetList());

        [HttpGet("{venueId}")]
        public async Task<IActionResult> Get(int venueId) => Ok(await _venueService.GetById(venueId));

        [HttpGet("GetAvailable")]
        public async Task<IActionResult> GetAvailable(DateTime date) => Ok(await _venueService.GetAvailable(date));

        [HttpPost]
        public async Task<IActionResult> Create(VenueCUDto venueDto) => Ok(await _venueService.CreateVenue(venueDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VenueCUDto venueUpdateDto) => Ok(await _venueService.UpdateVenue(id, venueUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _venueService.DeleteVenue(id));
    }
}
