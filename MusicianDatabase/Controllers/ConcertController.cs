using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertController : ControllerBase
    {
        private readonly IConcertService _concertService;

        public ConcertController(IConcertService concertService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _concertService = concertService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _concertService.GetList());

        [HttpGet("{concertId}")]
        public async Task<IActionResult> Get(int concertId) => Ok(await _concertService.GetById(concertId));

        [HttpPost]
        public async Task<IActionResult> Create(ConcertCreateDto concertDto) => Ok(await _concertService.CreateConcert(concertDto));

        [HttpPost("QuickCreate")]
        public async Task<IActionResult> QuickCreate(ConcertQuickCreateDto concertQCDto) => Ok(await _concertService.QuickCreateConcert(concertQCDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ConcertUpdateDto concertUpdateDto) => Ok(await _concertService.UpdateConcert(id, concertUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _concertService.DeleteConcert(id));
    }
}
