using LibreraDes.Data;
using LibreraDes.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibreraDes.Services
{
    public class RecuperarCuentaService
    {
        private readonly LibreriaDbContext _context;

        public RecuperarCuentaService(LibreriaDbContext context)
        {
            _context = context;
        }

        public async Task<string> RecuperarCuenta(string correo)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

                if (usuario != null)
                {
                    // Aquí debes agregar la lógica para enviar el correo de recuperación
                    return "Correo de recuperación enviado.";
                }

                return "Correo no registrado.";
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return $"Error durante la recuperación de cuenta: {ex.Message}";
            }
        }
    }
}
