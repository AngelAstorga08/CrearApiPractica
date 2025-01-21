using API_Capacitacion.Data.Interfaces;
using API_Capacitacion.DTO.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_Capacitacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _service;
        public AuthController(IAuthService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return Ok(await _service.Login(loginDto));
        }
    }
}
