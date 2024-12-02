using System.Threading.Tasks;
using LibreraDes.Models;
using LibreraDes.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibreraDes.Servicios
{
    public class ServicioInventario
    {
        private readonly LibreriaDbContext _context;

        public ServicioInventario(LibreriaDbContext context)
        {
            _context = context;
        }

        // Agregar libros al inventario
        public async Task AgregarInventario(int libroId, int cantidad)
        {
            // Verificar si ya existe un registro de inventario para el libro
            var inventario = await ObtenerInventarioPorLibro(libroId);

            if (inventario != null)
            {
                // Si existe, simplemente aumentamos la cantidad disponible
                inventario.CantidadDisponible += cantidad;
                _context.Inventarios.Update(inventario);
            }
            else
            {
                // Si no existe, creamos un nuevo registro de inventario
                var nuevoInventario = new Inventario
                {
                    LibroId = libroId,
                    CantidadDisponible = cantidad
                };
                _context.Inventarios.Add(nuevoInventario);
            }

            await _context.SaveChangesAsync();
        }

        // Restaurar inventario (por ejemplo, al devolver un libro o agregar stock)
        public async Task RestaurarInventario(int libroId, int cantidadRestaurada)
        {
            var inventario = await ObtenerInventarioPorLibro(libroId);
            if (inventario != null)
            {
                // Aumentamos la cantidad disponible en el inventario
                inventario.CantidadDisponible += cantidadRestaurada;
                _context.Inventarios.Update(inventario);
                await _context.SaveChangesAsync();
            }
        }


        // Obtener el inventario de un libro específico
        public async Task<Inventario> ObtenerInventarioPorLibro(int libroId)
        {
            return await _context.Inventarios
                .Include(i => i.Libro)
                .FirstOrDefaultAsync(i => i.LibroId == libroId);
        }

        // Actualizar la cantidad de inventario tras una compra
        public async Task ActualizarInventarioTrasCompra(int libroId, int cantidadComprada)
        {
            var inventario = await ObtenerInventarioPorLibro(libroId);
            if (inventario != null)
            {
                inventario.CantidadDisponible -= cantidadComprada;
                _context.Inventarios.Update(inventario);
                await _context.SaveChangesAsync();
            }
        }

        // Mostrar la disponibilidad de inventario (número de copias o "sin existencias")
        public string ObtenerDisponibilidadDeInventario(int libroId)
        {
            var inventario = _context.Inventarios.FirstOrDefault(i => i.LibroId == libroId);
            if (inventario != null)
            {
                return inventario.CantidadDisponible > 0
                    ? inventario.CantidadDisponible.ToString()
                    : "Sin existencias";
            }
            return "Sin existencias";
        }
    }



}
