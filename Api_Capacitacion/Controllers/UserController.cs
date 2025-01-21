using API_Capacitacion.Data.Interfaces;
using API_Capacitacion.DTO.User;
using API_Capacitacion.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_Capacitacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        public UserController(IUserService service) => _service = service;
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            IEnumerable<UserModel?> users = await _service.FindAll();
            return Ok(users);
        }


        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            UserModel? user = await _service.FindOne(id);
            if(user == null) return NotFound();
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDto createUserDto)
        {
            UserModel? user = await _service.Create(createUserDto);
            if (user == null)  return NotFound();
            return Ok(user);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUsuarioDto updateUsuarioDto)
        {
            UserModel? user = await _service.Update(id, updateUsuarioDto);
            if (user == null) return NotFound();
            return Ok(user); 
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            UserModel? user = await _service.Remove(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
