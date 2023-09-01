using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _artistService.GetList());

        [HttpGet("{artistId}")]
        public async Task<IActionResult> Get(int artistId) => Ok(await _artistService.GetById(artistId));

        [HttpGet("GetRolesById")]
        public async Task<IActionResult> GetRolesById(int id) => Ok(await _artistService.GetRolesById(id));

        [HttpGet("GetArtistsWithoutBands")]
        public async Task<IActionResult> GetArtistsWithoutBands() => Ok(await _artistService.GetArtistsWithoutBands());

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ArtistCUDto artistDto) => Ok(await _artistService.CreateArtist(artistDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ArtistCUDto artistUpdateDto) => Ok(await _artistService.UpdateArtist(id, artistUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _artistService.DeleteArtist(id));
    }
}
