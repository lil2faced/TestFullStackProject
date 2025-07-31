using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTO.UserAPI;
using WebAPI.Core.Interfaces;

namespace WebAPI.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiRoleController : ControllerBase
    {
        private readonly IUserApiRoleService _userApiRoleService;
        public ApiRoleController(IUserApiRoleService apiRoleService)
        {
            _userApiRoleService = apiRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOAPIRole>>> GetAllRoles()
        {
            var cts = new CancellationTokenSource();
            var res = await _userApiRoleService.GetAllRoles(cts.Token);

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(DTOAPIRole role)
        {
            var cts = new CancellationTokenSource();
            await _userApiRoleService.SetRole(role, cts.Token);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditRole(string roleToEdit, DTOAPIRole role)
        {
            var cts = new CancellationTokenSource();
            await _userApiRoleService.EditRole(roleToEdit, role, cts.Token);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string role)
        {
            var cts = new CancellationTokenSource();
            await _userApiRoleService.DeleteRole(role, cts.Token);

            return Ok();
        }
    }
}
