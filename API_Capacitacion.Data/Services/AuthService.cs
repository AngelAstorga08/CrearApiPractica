using API_Capacitacion.Data.Interfaces;
using API_Capacitacion.DTO.Auth;
using API_Capacitacion.Model;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace API_Capacitacion.Data.Services
{
    public class AuthService : IAuthService
    {
        private PostgresqlConfiguration _configuration;

        public AuthService (PostgresqlConfiguration configuration)
        {
            _configuration = configuration;
        }
        public NpgsqlConnection CreateConnection() => new(_configuration.Connection);

        #region Login
        public async Task<bool> Login (LoginDto loginDto)
        {
            string sqlQuery = "select * from view_usuario where usuario = @userName";
            using NpgsqlConnection database = CreateConnection();

            try
            {
                await database.OpenAsync();

                UserModel? user = await database.QueryFirstOrDefaultAsync<UserModel>(
                    sqlQuery,
                    param: new
                    {
                        userName = loginDto.UserName
                    });


                await database.CloseAsync();

                if (user == null) return false;

                bool passwordMatching = BC.EnhancedVerify(loginDto.Password, user.Contrasena);

                return passwordMatching;

            }
            catch(Exception ex) 
            {
                return false;
            }
        }
        #endregion
    }
}
