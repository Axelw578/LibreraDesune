using System.Threading.Tasks;
using LibreraDes.Data;  // Asegúrate de tener acceso al DbContext y modelos
using LibreraDes.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreraDes.Services
{
    public class RegistroService
    {
        private readonly LibreriaDbContext _context;

        public RegistroService(LibreriaDbContext context)
        {
            _context = context;
        }

        // Método para registrar un nuevo usuario
        public async Task<string> RegistrarUsuario(Usuario nuevoUsuario)
        {
            try
            {
                // Validación de existencia de correo
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == nuevoUsuario.Correo);

                if (usuarioExistente != null)
                {
                    return "El correo ya está registrado.";
                }

                // Registro del nuevo usuario en la base de datos
                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return "Usuario registrado con éxito.";
            }
            catch (DbUpdateException ex)
            {
                // Manejo de errores específicos de la base de datos
                return $"Error al registrar el usuario: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Manejo de cualquier otro tipo de error
                return $"Error inesperado: {ex.Message}";
            }
        }
    }
}
