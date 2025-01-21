using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Capacitacion.Model
{
    public class UserModel
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public List<TareaModel> Tareas { get; set; } = [];

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is UserModel user) return user.IdUsuario == IdUsuario;
            return false;
        }
        public override int GetHashCode()
        {
            return IdUsuario.GetHashCode();
        }

    }
}
