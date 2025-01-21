using API_Capacitacion.Data.Interfaces;
using API_Capacitacion.DTO.Tarea;
using API_Capacitacion.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api_Capacitacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        ITareaService _service;
        public TareaController(ITareaService service)=>_service = service;
 

        // GET: api/<TareaController>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            IEnumerable<TareaModel?> listaTarea = await _service.FindAll();
            if (listaTarea.Count() == 0) return NotFound(); 
            return Ok(listaTarea);
        }

        // GET api/<TareaController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            TareaModel? tarea = await _service.FindOne(id);
            if (tarea == null) return NotFound();
            return Ok(tarea);
        }

        // POST api/<TareaController>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTareaDTO createTareaDTO)
        {
            TareaModel? task = await _service.Create(createTareaDTO);
            if(task == null) return NotFound();
            return Ok(task);
        }

        // PUT api/<TareaController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateTareaDTO updateTareaDTO)
        {
            TareaModel? task = await _service.Update(id, updateTareaDTO);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPut("Tooglestatus/{TaskId}")]
        public async Task<IActionResult> ToggleStatus(int TaskId)
        {
            TareaModel? task = await _service.ChangeStatus(TaskId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // DELETE api/<TareaController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            TareaModel? task = await _service.Remove(id);
            if (task == null) return NotFound();
            return Ok(task);
        }
    }
}
