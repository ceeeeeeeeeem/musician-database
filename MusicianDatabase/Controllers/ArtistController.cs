using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _artistService = artistService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _artistService.GetList());

        [HttpGet("{artistId}")]
        public async Task<IActionResult> Get(int artistId) => Ok(await _artistService.GetById(artistId));

        [HttpGet("GetRolesById")]
        public async Task<IActionResult> GetRolesById(int id) => Ok(await _artistService.GetRolesById(id));


        [HttpPost]
        public async Task<IActionResult> Create(ArtistCreateDto artistDto) => Ok(await _artistService.CreateArtist(artistDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ArtistUpdateDto artistUpdateDto) => Ok(await _artistService.UpdateArtist(id, artistUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _artistService.DeleteArtist(id));
    }
}
