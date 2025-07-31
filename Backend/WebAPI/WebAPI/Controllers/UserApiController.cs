using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.DTO.UserAPI;
using WebAPI.Core.Interfaces;

namespace WebAPI.WebAPI.Controllers
{
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserApiService _userApiService;
        public UserApiController(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<ActionResult<string>> Login([FromBody] DTOUserAPILogin user)
        {
            var cts = new CancellationTokenSource();
            var jwt = await _userApiService.Login(user, cts.Token);

            Response.Cookies.Append("auth-token", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(12)
            });

            return Ok(jwt);
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<ActionResult> Register([FromBody] DTOUserAPIRegistration user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cts = new CancellationTokenSource();
            await _userApiService.Register(user, cts.Token);

            return Ok();
        }
    }
}
