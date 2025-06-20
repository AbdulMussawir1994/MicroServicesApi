using MicroServicesApi.Repository;
using MicroServicesApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService) => _userService = userService;

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] LoginViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginUser(model, cancellationToken);
            return result.Status ? Ok(result) : BadRequest(result);
        }

        [HttpPost("RegisterUser")]
        // [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUser(model, cancellationToken);
            return result.Status ? CreatedAtAction(nameof(RegisterUser), result) : BadRequest(result);
        }
    }
}
