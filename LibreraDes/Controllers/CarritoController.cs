using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibreraDes.Servicios;
using LibreraDes.ViewModels;
using System.Security.Claims;

namespace LibreraDes.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ServicioCarrito _servicioCarrito;
        private readonly ServicioLibros _servicioLibros;
        private readonly ServicioVentas _servicioVentas;

        // Constructor
        public CarritoController(ServicioCarrito servicioCarrito, ServicioLibros servicioLibros, ServicioVentas servicioVentas)
        {
            _servicioCarrito = servicioCarrito;
            _servicioLibros = servicioLibros;
            _servicioVentas = servicioVentas;
            
        }



        // Método auxiliar para obtener el UsuarioId desde los claims
        private int ObtenerUsuarioIdDeClaims()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        // Acción para mostrar la vista del carrito del usuario
        public async Task<IActionResult> Index()
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            var carrito = await _servicioCarrito.ObtenerCarritoPorUsuario(usuarioId);
            var total = await _servicioCarrito.CalcularTotalCarrito(usuarioId);

            ViewBag.TotalCarrito = total;
            return View(carrito ?? new List<CarritoViewModel>());
        }

        // Acción para agregar un libro al carrito
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito(int libroId, int cantidad)
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            var resultado = await _servicioCarrito.AgregarAlCarrito(usuarioId, libroId, cantidad);

            if (resultado == "Libro añadido al carrito correctamente.")
            {
                var total = await _servicioCarrito.CalcularTotalCarrito(usuarioId); // Calcular el total después de añadir el libro
                return Json(new { success = true, message = resultado, totalCarrito = total });
            }
            return Json(new { success = false, message = resultado });


        }




        // Aumentar la cantidad de un libro en el carrito
        [HttpPost]
        public async Task<IActionResult> AumentarCantidad(int libroId)
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            await _servicioCarrito.AumentarCantidadEnCarrito(usuarioId, libroId);
            var total = await _servicioCarrito.CalcularTotalCarrito(usuarioId); // Calcular el total después de aumentar
            return Json(new { success = true, totalCarrito = total });
        }

        // Disminuir la cantidad de un libro en el carrito
        [HttpPost]
        public async Task<IActionResult> DisminuirCantidad(int libroId)
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            await _servicioCarrito.DisminuirCantidadEnCarrito(usuarioId, libroId);
            var total = await _servicioCarrito.CalcularTotalCarrito(usuarioId); // Calcular el total después de disminuir
            return Json(new { success = true, totalCarrito = total });
        }

        // Eliminar un libro del carrito
        [HttpPost]
        public async Task<IActionResult> EliminarDelCarrito(int libroId)
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            await _servicioCarrito.EliminarDelCarrito(usuarioId, libroId);
            var total = await _servicioCarrito.CalcularTotalCarrito(usuarioId); // Calcular el total después de eliminar
            return Json(new { success = true, totalCarrito = total });
        }


        // Actualizar la cantidad de un libro en el carrito (se unifica Aumentar/Disminuir en una sola acción)
        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int libroId, int cantidad)
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            await _servicioCarrito.ActualizarCantidadEnCarrito(usuarioId, libroId, cantidad);
            var total = await _servicioCarrito.CalcularTotalCarrito(usuarioId); // Calcular el total después de actualizar
            return Json(new { success = true, totalCarrito = total });
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarCompra([FromBody] List<int> librosSeleccionados)
        {
            try
            {
                // Obtener el usuario desde los claims
                int usuarioId = ObtenerUsuarioIdDeClaims();

                if (usuarioId <= 0)
                {
                    return Json(new { success = false, message = "Error: No se pudo obtener el ID del usuario." });
                }

                // Verificar si hay productos seleccionados en el carrito
                if (librosSeleccionados == null || !librosSeleccionados.Any())
                {
                    return Json(new { success = false, message = "Error: No se seleccionaron productos para la compra." });
                }

                // Registrar la venta solo de los libros seleccionados
                await _servicioVentas.RegistrarVentaDesdeCarrito(usuarioId, librosSeleccionados);

                // Actualizar el carrito eliminando solo los libros comprados
                await _servicioCarrito.EliminarLibrosSeleccionadosDelCarrito(usuarioId, librosSeleccionados);

                return Json(new { success = true, message = "Compra realizada con éxito." });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "No hay excepción interna.";
                return Json(new { success = false, message = "Error al procesar la compra: " + ex.Message + " | Detalles: " + innerException });
            }
        }




        // Obtener la cantidad de productos en el carrito
        [HttpGet]
        public async Task<IActionResult> ObtenerCantidadProductos()
        {
            int usuarioId = ObtenerUsuarioIdDeClaims();
            var cantidad = await _servicioCarrito.ObtenerCantidadProductos(usuarioId);
            return Json(new { cantidad });
        }
    }
}
