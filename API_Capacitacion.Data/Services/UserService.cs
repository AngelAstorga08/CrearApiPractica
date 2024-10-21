using API_Capacitacion.Data.Interfaces;
using API_Capacitacion.DTO.User;
using API_Capacitacion.Model;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Capacitacion.Data.Services
{
    public class UserService : IUserService
    {
        public PostgresqlConfiguration _connection { get; set; }
        public UserService(PostgresqlConfiguration connection) => _connection = connection;
        public NpgsqlConnection CreateConnection() => new(_connection.Connection);

        #region Create
        public Task<UserModel?> Create(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FindAll
        public async Task<IEnumerable<UserModel?>> FindAll()
        {
            using NpgsqlConnection database = CreateConnection();
            string sqlQuery = "SELECT * FROM view_usuario";

            try 
            {
                await database.OpenAsync();
                IEnumerable<UserModel> users = await database.QueryAsync<UserModel>(sqlQuery);
                await database.CloseAsync();
                return users;
            }
            catch(Exception ex) 
            {
                return [];
            }
        }
        #endregion

        #region FindOne
        public Task<UserModel?> FindOne(int userId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public Task<UserModel?> Update(int id, UpdateUsuarioDto createUserDto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public Task<UserModel?> Remove(int userId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
