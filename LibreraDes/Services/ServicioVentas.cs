using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibreraDes.Models;
using LibreraDes.Data;
using Microsoft.EntityFrameworkCore;

namespace LibreraDes.Servicios
{
    public class ServicioVentas
    {
        private readonly LibreriaDbContext _context;
        private readonly ServicioInventario _servicioInventario;
        private readonly ServicioCarrito _servicioCarrito; // Inyectar el servicio de carrito

        public ServicioVentas(LibreriaDbContext context, ServicioInventario servicioInventario, ServicioCarrito servicioCarrito)
        {
            _context = context;
            _servicioInventario = servicioInventario;
            _servicioCarrito = servicioCarrito; // Guardar la referencia al servicio de carrito
        }

        // Obtener libros comprados por un usuario
        public List<Libro> ObtenerLibrosCompradosPorUsuario(int usuarioId)
        {
            var ventas = _context.Ventas
                                 .Include(v => v.DetallesVentas)
                                 .ThenInclude(dv => dv.Libro)
                                 .Where(v => v.UsuarioId == usuarioId)
                                 .ToList();

            var librosComprados = ventas.SelectMany(v => v.DetallesVentas)
                                        .Select(dv => dv.Libro)
                                        .ToList();

            return librosComprados;
        }

        // Registrar una nueva venta y actualizar el inventario
        public async Task RegistrarVenta(Venta nuevaVenta)
        {
            // Insertar la venta
            _context.Ventas.Add(nuevaVenta);
            await _context.SaveChangesAsync();

            // Actualizar el inventario de cada libro vendido
            foreach (var detalle in nuevaVenta.DetallesVentas)
            {
                await _servicioInventario.ActualizarInventarioTrasCompra(detalle.LibroId, detalle.Cantidad);
            }
        }

        // Registrar una nueva venta a partir del carrito
        public async Task RegistrarVentaDesdeCarrito(int usuarioId, List<int> librosSeleccionados)
        {
            // Obtener solo los libros seleccionados del carrito
            var carritoSeleccionado = await _context.Carritos
                                                    .Include(c => c.Libro)
                                                    .Where(c => c.UsuarioId == usuarioId && librosSeleccionados.Contains(c.LibroId))
                                                    .ToListAsync();

            if (!carritoSeleccionado.Any())
            {
                throw new Exception("El carrito no contiene los libros seleccionados.");
            }

            // Calcular el total de la venta
            decimal total = carritoSeleccionado.Sum(c => c.Libro.Precio * c.Cantidad);

            // Crear una nueva venta
            var nuevaVenta = new Venta
            {
                UsuarioId = usuarioId,
                Total = total,
                DetallesVentas = carritoSeleccionado.Select(c => new DetallesVentas
                {
                    LibroId = c.LibroId,
                    Cantidad = c.Cantidad,
                    PrecioUnitario = c.Libro.Precio
                }).ToList()
            };

            // Registrar la venta
            await RegistrarVenta(nuevaVenta);
        }


        // Obtener detalles de una venta específica
        public async Task<Venta> ObtenerVentaPorId(int ventaId)
        {
            return await _context.Ventas
                .Include(v => v.DetallesVentas)
                .ThenInclude(dv => dv.Libro)
                .FirstOrDefaultAsync(v => v.Id == ventaId);
        }
    }
}
