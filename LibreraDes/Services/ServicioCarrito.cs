using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibreraDes.Models;
using LibreraDes.Data;
using Microsoft.EntityFrameworkCore;
using LibreraDes.ViewModels;

namespace LibreraDes.Servicios
{
    public class ServicioCarrito
    {
        private readonly LibreriaDbContext _context;
        private readonly ServicioInventario _servicioInventario;

        public ServicioCarrito(LibreriaDbContext context, ServicioInventario servicioInventario)
        {
            _context = context;
            _servicioInventario = servicioInventario;
        }

        // Vaciar solo los productos específicos del carrito después de la compra
        public async Task EliminarProductosCompradosDelCarrito(int usuarioId, List<int> librosCompradosIds)
        {
            var productosComprados = await _context.Carritos
                .Where(c => c.UsuarioId == usuarioId && librosCompradosIds.Contains(c.LibroId))
                .ToListAsync();

            if (productosComprados.Any())
            {
                _context.Carritos.RemoveRange(productosComprados);
                await _context.SaveChangesAsync();
            }
        }


        // elimina solo los libros comprados:
        public async Task EliminarLibrosSeleccionadosDelCarrito(int usuarioId, List<int> librosSeleccionados)
        {
            var itemsAEliminar = await _context.Carritos
                                               .Where(c => c.UsuarioId == usuarioId && librosSeleccionados.Contains(c.LibroId))
                                               .ToListAsync();

            if (itemsAEliminar.Any())
            {
                _context.Carritos.RemoveRange(itemsAEliminar);
                await _context.SaveChangesAsync();
            }
        }


        // Actualizar la cantidad de un libro en el carrito
        public async Task ActualizarCantidadEnCarrito(int usuarioId, int libroId, int nuevaCantidad)
        {
            var itemCarrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.LibroId == libroId);

            if (itemCarrito != null)
            {
                if (nuevaCantidad > 0)
                {
                    itemCarrito.Cantidad = nuevaCantidad;
                    _context.Carritos.Update(itemCarrito);
                }
                else
                {
                    _context.Carritos.Remove(itemCarrito); // Eliminar si la cantidad es 0
                }

                await _context.SaveChangesAsync();
            }
        }


        // Agregar libro al carrito
        public async Task<string> AgregarAlCarrito(int usuarioId, int libroId, int cantidad)
        {
            var inventario = await _servicioInventario.ObtenerInventarioPorLibro(libroId);

            if (inventario == null)
            {
                return "Error: El libro no está disponible en el inventario.";
            }

            if (inventario.CantidadDisponible < cantidad)
            {
                return "Error: No hay suficiente inventario disponible.";
            }

            var itemCarrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.LibroId == libroId);

            if (itemCarrito != null)
            {
                itemCarrito.Cantidad += cantidad;

                if (itemCarrito.Cantidad > inventario.CantidadDisponible)
                {
                    itemCarrito.Cantidad = inventario.CantidadDisponible;
                    return "Alerta: La cantidad solicitada excedía el inventario disponible. Se ajustó la cantidad en el carrito.";
                }

                _context.Carritos.Update(itemCarrito);
            }
            else
            {
                var nuevoItem = new Carrito
                {
                    UsuarioId = usuarioId,
                    LibroId = libroId,
                    Cantidad = cantidad
                };

                _context.Carritos.Add(nuevoItem);
            }

            await _context.SaveChangesAsync();
            return "Libro añadido al carrito correctamente.";
        }

        // Obtener la cantidad total de productos en el carrito
        public async Task<int> ObtenerCantidadProductos(int usuarioId)
        {
            return await _context.Carritos
                                 .Where(c => c.UsuarioId == usuarioId)
                                 .SumAsync(c => c.Cantidad);
        }

        // Obtener el carrito del usuario con información adicional
        public async Task<List<CarritoViewModel>> ObtenerCarritoPorUsuario(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Libro)
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => new CarritoViewModel
                {
                    LibroId = c.LibroId,
                    TituloLibro = c.Libro.Titulo,
                    PrecioLibro = c.Libro.Precio,
                    URLImagenLibro = c.Libro.URLImagen,
                    Cantidad = c.Cantidad,
                    Subtotal = c.Libro.Precio * c.Cantidad
                }).ToListAsync();

            return carrito;
        }

        // Calcular el total del carrito
        public async Task<decimal> CalcularTotalCarrito(int usuarioId)
        {
            var carrito = await ObtenerCarritoPorUsuario(usuarioId);
            return carrito.Sum(c => c.Subtotal);
        }

        // Aumentar la cantidad de un libro en el carrito
        public async Task AumentarCantidadEnCarrito(int usuarioId, int libroId)
        {
            var itemCarrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.LibroId == libroId);

            if (itemCarrito != null)
            {
                itemCarrito.Cantidad++;
                _context.Carritos.Update(itemCarrito);
                await _context.SaveChangesAsync();
            }
        }

        // Disminuir la cantidad de un libro en el carrito (y eliminarlo si llega a 0)
        public async Task DisminuirCantidadEnCarrito(int usuarioId, int libroId)
        {
            var itemCarrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.LibroId == libroId);

            if (itemCarrito != null)
            {
                if (itemCarrito.Cantidad > 1)
                {
                    itemCarrito.Cantidad--;
                    _context.Carritos.Update(itemCarrito);
                }
                else
                {
                    _context.Carritos.Remove(itemCarrito);
                }

                await _context.SaveChangesAsync();
            }
        }

        // Eliminar un libro del carrito
        public async Task EliminarDelCarrito(int usuarioId, int libroId)
        {
            var itemCarrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.LibroId == libroId);

            if (itemCarrito != null)
            {
                _context.Carritos.Remove(itemCarrito);
                await _context.SaveChangesAsync();
            }
        }

        // Vaciar el carrito después de la compra
        public async Task VaciarCarrito(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();
            _context.Carritos.RemoveRange(carrito);
            await _context.SaveChangesAsync();
        }

        // Registrar detalles de ventas al comprar
        public async Task RegistrarDetallesVentas(int usuarioId, int ventaId)
        {
            var carrito = await ObtenerCarritoPorUsuario(usuarioId);
            var detallesVentas = carrito.Select(c => new DetallesVentas
            {
                VentaId = ventaId,
                LibroId = c.LibroId,
                Cantidad = c.Cantidad,
                PrecioUnitario = c.PrecioLibro
            }).ToList();

            _context.DetallesVentas.AddRange(detallesVentas);
            await _context.SaveChangesAsync();
        }

        // Método para validar si el carrito tiene productos seleccionados
        public async Task<bool> CarritoTieneProductos(int usuarioId)
        {
            return await _context.Carritos.AnyAsync(c => c.UsuarioId == usuarioId);
        }
    }


}
