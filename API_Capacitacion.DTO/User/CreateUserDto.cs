using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Capacitacion.DTO.User
{
    public class CreateUserDto
    {
        public string Nombres { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }
}
