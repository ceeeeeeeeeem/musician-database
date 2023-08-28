using Microsoft.AspNetCore.Mvc;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            // Should I set up the configuration so it doesn't generate as "this.context"?
            _roleService = roleService;
        }

        // Doesn't return roles.
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _roleService.GetList());
        
        [HttpGet("Details")]
        public async Task<IActionResult> GetDetailedList() => Ok(await _roleService.GetDetailedList());

        [HttpGet("{roleId}")]
        public async Task<IActionResult> Get(int roleId) => Ok(await _roleService.GetById(roleId));

        [HttpPost]
        public async Task<IActionResult> Create(RoleCUDto roleDto) => Ok(await _roleService.CreateRole(roleDto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleCUDto roleUpdateDto) => Ok(await _roleService.UpdateRole(id, roleUpdateDto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _roleService.DeleteRole(id));
    }
}
