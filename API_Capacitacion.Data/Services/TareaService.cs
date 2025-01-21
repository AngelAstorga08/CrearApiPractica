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
                        tarea.Usuarioo = user;
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

        public async Task<IEnumerable<TareaModel?>> FindAll()
        {
            using NpgsqlConnection dataBase = CreateConnection();
            string sqlQuery = "SELECT * FROM view_tarea";
            try
            {                  
                await dataBase.OpenAsync();
                IEnumerable<TareaModel> listaTarea = await dataBase.QueryAsync<TareaModel, UserModel, TareaModel>(
                    sqlQuery,
                    map: (tarea, user) => {
                        tarea.Usuarioo = user;
                        return tarea;
                    },
                    splitOn: "usuarioId");
                await dataBase.CloseAsync();
                return listaTarea;

            }
            catch (Exception ex)
            {
                return [];
            }
        }

        public async Task<TareaModel?> FindOne(int userId)
        {
            using NpgsqlConnection dataBase = CreateConnection();
            string sqlQuery = "SELECT * FROM view_tarea WHERE idtarea = @idTask;";
            try
            {
                await dataBase.OpenAsync();
                var tarea = await dataBase.QueryAsync<TareaModel, UserModel, TareaModel>(
                    sqlQuery,
                    param: new {
                    idTask = userId},
                    map: (tarea, user) => {
                        tarea.Usuarioo = user;
                        return tarea;
                    },
                    splitOn: "usuarioId");
                await dataBase.CloseAsync();
                return tarea.FirstOrDefault();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TareaModel?> Update(int tareaId, UpdateTareaDTO updateTareaDto)
        {
            using NpgsqlConnection dataBase = CreateConnection();
            string sqlQuery = "SELECT * FROM fun_task_update(" +
                "p_idtarea:= @idTask," +
                "p_tarea := @task," +
                "p_descripcion := @description);";
            try
            {
                await dataBase.OpenAsync();
                IEnumerable<TareaModel> result = await dataBase.QueryAsync<TareaModel, UserModel, TareaModel>(
                    sqlQuery, param: new
                    {
                        idTask = tareaId,
                        task = updateTareaDto.Tarea,
                        description = updateTareaDto.Descripcion
                    },
                    map: (tarea, user) => {
                        tarea.Usuarioo = user;
                        return tarea;
                    },
                    splitOn: "usuarioId"
                    );
                await dataBase.CloseAsync();
                return result.FirstOrDefault();
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public async Task<TareaModel?> Remove(int userId)
        {
            using NpgsqlConnection dataBase = CreateConnection();
            string sqlQuery = "SELECT * FROM fun_task_remove(p_idTarea := @idTask);";
            try
            {
                await dataBase.OpenAsync();
                var tarea = await dataBase.QueryAsync<TareaModel, UserModel, TareaModel>(
                    sqlQuery,
                    param: new
                    {
                        idTask = userId                       
                    },
                    map: (tarea, user) => {
                        tarea.Usuarioo = user;
                        return tarea;
                    },
                    splitOn: "usuarioId");
                await dataBase.CloseAsync();
                return tarea.FirstOrDefault();

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<TareaModel?> ChangeStatus(int taskId)
        {
            using NpgsqlConnection dataBase = CreateConnection();
            string sqlQuery = "SELECT * FROM fun_task_togglestatus(p_idTarea := @idTask);";
            try
            {
                await dataBase.OpenAsync();
                var tarea = await dataBase.QueryAsync<TareaModel, UserModel, TareaModel>(
                    sqlQuery,
                    param: new
                    {
                        idTask = taskId
                    },
                    map: (tarea, user) => {
                        tarea.Usuarioo = user;
                        return tarea;
                    },
                    splitOn: "usuarioId");
                await dataBase.CloseAsync();
                return tarea.FirstOrDefault();

            }
            catch (Exception ex)
            {
                return null;
            }

        }


        #endregion
    }
}
