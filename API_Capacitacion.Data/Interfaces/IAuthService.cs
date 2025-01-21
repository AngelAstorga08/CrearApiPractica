using API_Capacitacion.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Capacitacion.Data.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> Login(LoginDto loginDto);
    }
}
