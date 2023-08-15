using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IBandService _bandService;

        public BandController(IBandService bandService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _bandService = bandService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _bandService.GetList());

        [HttpGet("{bandId}")]
        public async Task<IActionResult> Get(int bandId) => Ok(await _bandService.GetById(bandId));

        [HttpGet("GetAvailable")]
        public async Task<IActionResult> GetAvailable(string genre, DateTime date) => Ok(await _bandService.GetAvailable(genre, date));

        [HttpGet("GetMembers")]
        public async Task<IActionResult> GetMembers(int id) => Ok(await _bandService.GetMembers(id));

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember(int artistId, int bandId, int instrumentId, string? roleDescription = null) => Ok(await _bandService.AddMember(artistId, bandId, instrumentId, roleDescription));
        [HttpDelete("RemoveMember")]
        public async Task<IActionResult> RemoveMember(int artistId, int bandId) => Ok(await _bandService.RemoveMember(artistId,bandId));

        [HttpPost]
        public async Task<IActionResult> Create(BandCreateDto bandDto) => Ok(await _bandService.CreateBand(bandDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BandUpdateDto bandUpdateDto) => Ok(await _bandService.UpdateBand(id, bandUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _bandService.DeleteBand(id));
    }
}
