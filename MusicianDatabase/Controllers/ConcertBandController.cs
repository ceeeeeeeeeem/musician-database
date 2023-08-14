﻿using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcertBandController : ControllerBase
    {
        private readonly IConcertBandService _concertBandService;

        public ConcertBandController(IConcertBandService concertBandService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _concertBandService = concertBandService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _concertBandService.GetList());

        [HttpGet("{concertId}/{bandId}")]
        public async Task<IActionResult> Get(int concertId, int bandId) => Ok(await _concertBandService.GetById(concertId, bandId));

        [HttpPost]
        public async Task<IActionResult> Create(ConcertBandCreateDto concertBandDto) => Ok(await _concertBandService.CreateConcertBand(concertBandDto));

        [HttpPut("{concertId}/{bandId}")]
        public async Task<IActionResult> Update(int concertId, int bandId, ConcertBandUpdateDto concertBandUpdateDto) => Ok(await _concertBandService.UpdateConcertBand(concertId, bandId, concertBandUpdateDto));

        [HttpDelete("{concertId}/{bandId}")]
        public async Task<IActionResult> Delete(int concertId, int bandId) => Ok(await _concertBandService.DeleteConcertBand(concertId, bandId));
    }
}
