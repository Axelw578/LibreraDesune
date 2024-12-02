using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LibreraDes.Servicios;
using LibreraDes.ViewModels;
using System.Collections.Generic;
using LibreraDes.Models;
using System.Diagnostics;

namespace LibreraDes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ServicioLibros _servicioLibros;
        private readonly ServicioInventario _servicioInventario;
        private readonly ServicioCarrito _servicioCarrito;

        public HomeController(ILogger<HomeController> logger, ServicioLibros servicioLibros, ServicioInventario servicioInventario, ServicioCarrito servicioCarrito)
        {
            _logger = logger;
            _servicioLibros = servicioLibros;
            _servicioInventario = servicioInventario;
            _servicioCarrito = servicioCarrito;
        }
        // Acción para mostrar todos los libros en el menú principal
        public async Task<IActionResult> Index(int? id)
        {
            var libros = await _servicioLibros.ObtenerTodosLosLibros();
            var librosViewModel = new List<LibroDetallesViewModel>();

            foreach (var libro in libros)
            {
                var inventario = await _servicioInventario.ObtenerInventarioPorLibro(libro.Id);
                librosViewModel.Add(new LibroDetallesViewModel
                {
                    Id = libro.Id,
                    Titulo = libro.Titulo,
                    AutorNombre = libro.Autor.Nombre,
                    CategoriaNombre = libro.Categoria.Nombre,
                    Precio = libro.Precio,
                    URLImagen = libro.URLImagen,
                    DescripcionCategoria = libro.Categoria.Descripcion,
                    FechaNacimientoAutor = libro.Autor.FechaNacimiento,
                    CantidadDisponible = inventario?.CantidadDisponible ?? 0,
                    Sinopsis = libro.Sinopsis // Nuevo campo añadido
                });
            }

            // Si se proporciona un id, obtenemos el libro específico
            LibroDetallesViewModel libroSeleccionado = null;
            if (id.HasValue)
            {
                var libro = await _servicioLibros.ObtenerLibroPorId(id.Value);
                if (libro != null)
                {
                    var inventario = await _servicioInventario.ObtenerInventarioPorLibro(libro.Id);
                    libroSeleccionado = new LibroDetallesViewModel
                    {
                        Id = libro.Id,
                        Titulo = libro.Titulo,
                        AutorNombre = libro.Autor.Nombre,
                        CategoriaNombre = libro.Categoria.Nombre,
                        Precio = libro.Precio,
                        URLImagen = libro.URLImagen,
                        DescripcionCategoria = libro.Categoria.Descripcion,
                        FechaNacimientoAutor = libro.Autor.FechaNacimiento,
                        CantidadDisponible = inventario?.CantidadDisponible ?? 0,
                        Sinopsis = libro.Sinopsis // Nuevo campo añadido
                    };
                }
            }

            // Pasamos tanto la lista de libros como el libro seleccionado a la vista
            ViewBag.LibroSeleccionado = libroSeleccionado;
            return View(librosViewModel); // Devuelve la vista con todos los libros
        }


        [HttpPost]
public async Task<IActionResult> AgregarAlCarrito(int libroId, int cantidad)
{
    // Obtener el usuarioId desde los claims (usuario autenticado)
    var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

    if (claim == null)
    {
        // Si no se encuentra el usuario autenticado, redirigir al login
        TempData["Error"] = "Debes iniciar sesión para agregar productos al carrito.";
        return RedirectToAction("Index");
    }

    // Convertir el valor del claim a int (asumiendo que usuarioId es un entero)
    int usuarioId = int.Parse(claim.Value);

    // Llamar al servicio de carrito para agregar el libro
    var resultado = await _servicioCarrito.AgregarAlCarrito(usuarioId, libroId, cantidad);

    if (resultado.StartsWith("Error"))
    {
        // Mostrar mensaje de error si no hay suficiente inventario o hay algún otro problema
        TempData["Error"] = resultado;
    }
    else
    {
        TempData["Mensaje"] = resultado;
    }

    // Redirigir de vuelta a la vista principal
    return RedirectToAction("Index");
}



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
