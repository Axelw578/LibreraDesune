using Microsoft.AspNetCore.Mvc;
using LibreraDes.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibreraDes.Controllers
{
    public class PruebaConexionController : Controller
    {
        private readonly LibreriaDbContext _context;

        public PruebaConexionController(LibreriaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> VerificarConexion()
        {
            try
            {
                // Prueba de conexión básica
                using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();  // Intentar abrir la conexión
                return Content("Conexión exitosa a la base de datos");
            }
            catch (Exception ex)
            {
                // Mostrar el mensaje de error si no se puede conectar
                return Content($"Error al conectar a la base de datos: {ex.Message}");
            }
        }
    }
}
