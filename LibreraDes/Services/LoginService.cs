using LibreraDes.Data;
using LibreraDes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LibreraDes.Services
{
    public class LoginService
    {
        private readonly LibreriaDbContext _context;

        public LoginService(LibreriaDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> LoginUsuario(string correo, string clave)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == correo && u.Clave == clave);

                if (usuario != null)
                {
                    // Si se encuentra el usuario, lo retorna para poder usarlo en el controlador
                    return usuario;
                }

                // Si no se encuentra, retorna null para indicar credenciales incorrectas
                return null;
            }
            catch (Exception ex)
            {
                // Manejo de errores (aquí puedes loguear el error o manejarlo según lo necesites)
                throw new Exception($"Error durante el login: {ex.Message}");
            }
        }
    }
}
