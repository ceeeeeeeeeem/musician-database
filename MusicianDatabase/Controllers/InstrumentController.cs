using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;

        public InstrumentController(IInstrumentService instrumentService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _instrumentService = instrumentService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _instrumentService.GetList());

        [HttpGet("{instrumentId}")]
        public async Task<IActionResult> Get(int instrumentId) => Ok(await _instrumentService.GetById(instrumentId));

        [HttpGet("GetPlayers")]
        public async Task<IActionResult> GetInstrumentalists(int instrumentId) => Ok(await _instrumentService.GetInstrumentalists(instrumentId));

        [HttpPost]
        public async Task<IActionResult> Create(InstrumentCUDto instrumentDto) => Ok(await _instrumentService.CreateInstrument(instrumentDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InstrumentCUDto instrumentUpdateDto) => Ok(await _instrumentService.UpdateInstrument(id, instrumentUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _instrumentService.DeleteInstrument(id));
    }
}
