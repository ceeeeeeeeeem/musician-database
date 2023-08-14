using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleInstrumentController : ControllerBase
    {
        private readonly IRoleInstrumentService _roleInstrumentService;

        public RoleInstrumentController(IRoleInstrumentService roleInstrumentService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _roleInstrumentService = roleInstrumentService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _roleInstrumentService.GetList());

        [HttpGet("{roleId}/{instrumentId}")]
        public async Task<IActionResult> Get(int roleId, int instrumentId) => Ok(await _roleInstrumentService.GetById(roleId, instrumentId));

        [HttpPost]
        public async Task<IActionResult> Create(RoleInstrumentCreateDto roleInstrumentDto) => Ok(await _roleInstrumentService.CreateRoleInstrument(roleInstrumentDto));

        [HttpPut("{roleId}/{instrumentId}")]
        public async Task<IActionResult> Update(int roleId, int instrumentId, RoleInstrumentUpdateDto roleInstrumentUpdateDto) => Ok(await _roleInstrumentService.UpdateRoleInstrument(roleId, instrumentId, roleInstrumentUpdateDto));

        [HttpDelete("{roleId}/{instrumentId}")]
        public async Task<IActionResult> Delete(int roleId, int instrumentId) => Ok(await _roleInstrumentService.DeleteRoleInstrument(roleId, instrumentId));
    }
}
