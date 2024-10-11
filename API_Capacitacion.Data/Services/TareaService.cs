using API_Capacitacion.Data.Interfaces;
using API_Capacitacion.DTO.Tarea;
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
    public class TareaService : ITareaService
    {
        private PostgresqlConfiguration _connection;
        public TareaService(PostgresqlConfiguration connection) => _connection = connection;
        private NpgsqlConnection CreateConnection() => new(_connection.Connection);
        #region Create
        public async Task<TareaModel?> Create(CreateTareaDTO createTareaDTO) 
        {
            using NpgsqlConnection dataBase = CreateConnection();
            string sqlQuery = "SELECT * FROM fun_task_create(" +
                "p_tarea:= @task," +
                "p_descripcion := @descripcion," +
                "p_idUsuario := @userID);";

            try 
            {
                await dataBase.OpenAsync();
                var result = await dataBase.QueryAsync<TareaModel, UserModel, TareaModel>(
                    sqlQuery, param: new {
                    task = createTareaDTO.Tarea,
                    descripcion = createTareaDTO.Descripcion,
                    userID = createTareaDTO.IdUsuario
                    },
                    map: (tarea,user) => {
                        tarea.Usuario = user;
                        return tarea;
                        },
                    splitOn: "usuarioId"
                    );
                await dataBase.CloseAsync();
                return result.FirstOrDefault();
            }
            catch(Exception Ex) 
            {
                return null;
            }
        }
        #endregion
    }
}
