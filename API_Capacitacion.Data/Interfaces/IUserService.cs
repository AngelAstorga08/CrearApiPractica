using API_Capacitacion.DTO.User;
using API_Capacitacion.Model;

namespace API_Capacitacion.Data.Interfaces
{
    public interface IUserService
    {
        public Task<UserModel?> Create(CreateUserDto createUserDto);
        public Task<IEnumerable<UserModel?>> FindAll();
        public Task<UserModel?> FindOne(int userId);
        public Task<UserModel?> Update(int userId, UpdateUsuarioDto updateUserDto);
        public Task<UserModel?> Remove(int userId);
    }
}
