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
using BC = BCrypt.Net.BCrypt;

namespace API_Capacitacion.Data.Services
{
    public class UserService : IUserService
    {
        public PostgresqlConfiguration _connection { get; set; }
        public UserService(PostgresqlConfiguration connection) => _connection = connection;
        public NpgsqlConnection CreateConnection() => new(_connection.Connection);

        #region Create
        public async Task<UserModel?> Create(CreateUserDto createUserDto)
        {
            using NpgsqlConnection database = CreateConnection();
            string sqlQuery = "SELECT * FROM fun_user_create(" +
                "p_nombres := @name," +
                "p_usuario := @user," +
                "p_contrasena := @password)";

            string hashedPassword = BC.EnhancedHashPassword(createUserDto.Contrasena);

            try
            {
                await database.OpenAsync();
                IEnumerable<UserModel> user = await database.QueryAsync<UserModel>(
                    sqlQuery,
                    param: new { 
                        name = createUserDto.Nombres,
                        user = createUserDto.Usuario,
                        password = hashedPassword
                    });
                await database.CloseAsync();

                return user.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region FindAll
        public async Task<IEnumerable<UserModel?>> FindAll()
        {
            using NpgsqlConnection database = CreateConnection();
            string sqlQuery = "SELECT * FROM view_usuario";

            Dictionary<int, List<TareaModel>> userTasks = [];

            try 
            {
                await database.OpenAsync();
                IEnumerable<UserModel> result = await database.QueryAsync<UserModel, TareaModel, UserModel>(
                sqlQuery,
                map: (user, task) => 
                {
                    //List<TareaModel> currentTasks = userTasks[user.IdUsuario] ?? [];
                    //if (currentTasks.Count == 0) currentTasks = [task];
                    //else currentTasks.Add(task);

                    //userTasks[user.IdUsuario] = currentTasks;
                    List<TareaModel> currentTasks = [];
                    userTasks.TryGetValue(user.IdUsuario, out currentTasks);

                    currentTasks ??= [];

                    if (currentTasks.Count == 0 && task != null)
                    {
                        currentTasks = [task];
                    }
                    else if (currentTasks.Count > 0 && task != null)
                    {                       
                        currentTasks.Add(task); // O(n)
                    }

                    userTasks[user.IdUsuario] = currentTasks; // O(n)
                    return user;
                },
                splitOn: "idTarea");
                await database.CloseAsync();
                IEnumerable<UserModel> usersList = result.Distinct().ToList().Select(user => {
                    user.Tareas = userTasks[user.IdUsuario];
                    return user;
                });
                    return usersList;
            }
            catch(Exception ex) 
            {
                return [];
            }
        }
        #endregion

        #region FindOne
        public async Task<UserModel?> FindOne(int userId)
        {
            using NpgsqlConnection database = CreateConnection();
            string sqlQuery = "SELECT * FROM view_usuario WHERE idUsuario = @idUser";

            Dictionary<int, List<TareaModel>> userTasks = [];

            try
            {
                await database.OpenAsync();
                IEnumerable<UserModel> result = await database.QueryAsync<UserModel, TareaModel, UserModel>(
                    sqlQuery,
                    param: new { idUser = userId},
                    map: (users, task) => {

                        List<TareaModel> currentTasks = [];
                        userTasks.TryGetValue(users.IdUsuario, out currentTasks);

                        currentTasks ??= [];

                        if (currentTasks.Count == 0 && task != null)
                        {
                            currentTasks = [task];
                        }
                        else if (currentTasks.Count > 0 && task != null)
                        {
                            currentTasks.Add(task);   
                        }

                        userTasks[users.IdUsuario] = currentTasks;

                        return users;
                    },
                    splitOn: "idTarea");
                await database.CloseAsync();

                IEnumerable<UserModel?> usersList = result.Distinct().ToList().Select(user => {
                    user.Tareas = userTasks[user.IdUsuario];
                    return user;
                });
                return usersList.FirstOrDefault();
            }
            catch (Exception EX)
            {
                return null;
            }
        }
        #endregion

        #region Update
        public async Task<UserModel?> Update(int id, UpdateUsuarioDto updateUserDto)
        {
            using NpgsqlConnection database = CreateConnection();
            string sqlQuery = "SELECT * FROM fun_user_update(" +
                "p_idUsuario := @userId," +
                "p_nombres := @name," +
                "p_usuario := @user," +
                "p_contrasena := @password)";

            try
            {
                await database.OpenAsync();
                UserModel? result = await database.QueryFirstOrDefaultAsync<UserModel>(
                    sqlQuery,
                    param: new {
                        userId = id,
                        name = updateUserDto.Nombre,
                        user = updateUserDto.Usuario,
                        password = updateUserDto.Contrasena                    
                    }
                    );
                await database.CloseAsync();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Remove
        public async Task<UserModel?> Remove(int Id)
        {
            string sqlQuery = "SELECT * FROM fun_user_remove(p_idUsuario := @userId);";
            using NpgsqlConnection database = CreateConnection();

            Dictionary<int, List<TareaModel>> usersTasks = [];

            try
            {
                await database.OpenAsync();
                IEnumerable<UserModel> result = await database.QueryAsync<UserModel, TareaModel, UserModel>(
                    sqlQuery,
                    param: new
                    {
                        userId = Id
                    },
                    map: (users, task) =>
                    {

                        List<TareaModel> currentTask = [];
                        usersTasks.TryGetValue(users.IdUsuario, out currentTask);

                        currentTask ??= [];

                        if (currentTask.Count == 0 && task != null)
                        {
                            currentTask = [task];
                        }
                        else if (currentTask.Count > 0 && task != null)
                        {
                            currentTask.Add(task);
                        }

                        usersTasks[users.IdUsuario] = currentTask;

                        return users;
                    },
                    splitOn: "idTarea");

                await database.CloseAsync();

                IEnumerable<UserModel> user = result.Distinct().ToList().Select(users => {
                    users.Tareas = usersTasks[users.IdUsuario];
                    return users;
                });

                return user.FirstOrDefault();
            }
             catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
