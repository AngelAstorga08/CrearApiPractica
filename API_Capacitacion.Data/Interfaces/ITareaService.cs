using API_Capacitacion.DTO.Tarea;
using API_Capacitacion.Model;


namespace API_Capacitacion.Data.Interfaces
{
    public interface ITareaService
    {
        public Task<TareaModel?> Create(CreateTareaDTO createTareaDTO);
        public Task<IEnumerable<TareaModel?>> FindAll();
        public Task<TareaModel?> FindOne(int tareaId);
        public Task<TareaModel?> Update(int tareaId, UpdateTareaDTO updateTareaDto);
        public Task<TareaModel?> Remove(int tareaId);
        public Task<TareaModel?> ChangeStatus(int taskId);

    }
}
