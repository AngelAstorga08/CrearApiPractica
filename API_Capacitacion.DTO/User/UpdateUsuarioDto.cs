using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Capacitacion.DTO.User
{
    internal class UpdateUsuarioDto
    {
        public string Nombres { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }

    }
}
