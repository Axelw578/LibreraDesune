using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LibreraDes.Servicios;
using LibreraDes.ViewModels;
using System.Collections.Generic;
using LibreraDes.Models;

namespace LibreraDes.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ServicioLibros _servicioLibros;
        private readonly ServicioInventario _servicioInventario;
        private readonly ServicioCategoriasAutores _servicioCategoriasAutores;

        public LibrosController(ServicioLibros servicioLibros, ServicioInventario servicioInventario, ServicioCategoriasAutores servicioCategoriasAutores)
        {
            _servicioLibros = servicioLibros;
            _servicioInventario = servicioInventario;
            _servicioCategoriasAutores = servicioCategoriasAutores;
        }

        // Acción para mostrar todos los libros en el menú principal
        public async Task<IActionResult> Index(int? id)
        {
            var libros = await _servicioLibros.ObtenerTodosLosLibros();
            var librosViewModel = new List<LibroViewModel>();

            foreach (var libro in libros)
            {
                librosViewModel.Add(new LibroViewModel
                {
                    Id = libro.Id,
                    Titulo = libro.Titulo,
                    Sinopsis = libro.Sinopsis,
                    Precio = libro.Precio,
                    URLImagen = libro.URLImagen,
                    AutorId = libro.AutorId,
                    CategoriaId = libro.CategoriaId
                });
            }

            ViewBag.LibroSeleccionado = id.HasValue ? librosViewModel.Find(l => l.Id == id.Value) : null;
            return View(librosViewModel);
        }

        // Acción para obtener categorías en formato JSON
        [HttpGet]
        public async Task<IActionResult> ObtenerCategorias()
        {
            var (success, categorias, error) = await _servicioCategoriasAutores.ObtenerTodasLasCategorias();
            if (!success)
            {
                return Json(new { success = false, message = error });
            }
            return Json(new { success = true, categorias });
        }

        // Acción para obtener autores en formato JSON
        [HttpGet]
        public async Task<IActionResult> ObtenerAutores()
        {
            var (success, autores, error) = await _servicioCategoriasAutores.ObtenerTodosLosAutores();
            if (!success)
            {
                return Json(new { success = false, message = error });
            }
            return Json(new { success = true, autores });
        }

        // Acción para crear un nuevo libro (GET)
        public IActionResult Crear()
        {
            return View(new LibroViewModel());
        }

        // Acción para crear un nuevo libro (POST)
        [HttpPost]
        public async Task<IActionResult> Crear(LibroViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var nuevoLibro = new Libro
                {
                    Titulo = modelo.Titulo,
                    Sinopsis = modelo.Sinopsis,
                    Precio = modelo.Precio,
                    URLImagen = modelo.URLImagen,
                    AutorId = modelo.AutorId,
                    CategoriaId = modelo.CategoriaId
                };

                await _servicioLibros.CrearLibro(nuevoLibro);
                return Json(new { success = true, message = "Libro creado exitosamente." });
            }
            return Json(new { success = false, message = "Error en los datos del libro." });
        }

        // Acción para editar un libro existente (GET)
        public async Task<IActionResult> Editar(int id)
        {
            var libro = await _servicioLibros.ObtenerLibroPorId(id);
            if (libro == null) return NotFound();

            var libroViewModel = new LibroViewModel
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Sinopsis = libro.Sinopsis,
                Precio = libro.Precio,
                URLImagen = libro.URLImagen,
                AutorId = libro.AutorId,
                CategoriaId = libro.CategoriaId
            };

            // Obtener listas de categorías y autores
            var (successCategorias, categorias, errorCategorias) = await _servicioCategoriasAutores.ObtenerTodasLasCategorias();
            var (successAutores, autores, errorAutores) = await _servicioCategoriasAutores.ObtenerTodosLosAutores();

            if (successCategorias && successAutores)
            {
                ViewBag.Categorias = categorias;
                ViewBag.Autores = autores;
            }
            else
            {
                // Manejar el error si es necesario
                ViewBag.Error = "No se pudieron cargar las categorías o autores.";
            }

            return View(libroViewModel);
        }

        // Acción para editar un libro existente (POST)
        [HttpPost]
        public async Task<IActionResult> Editar(int id, LibroViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var libroActualizado = await _servicioLibros.ObtenerLibroPorId(id);
                if (libroActualizado == null)
                {
                    return Json(new { success = false, message = "Libro no encontrado." });
                }

                libroActualizado.Titulo = modelo.Titulo;
                libroActualizado.Sinopsis = modelo.Sinopsis;
                libroActualizado.Precio = modelo.Precio;
                libroActualizado.URLImagen = modelo.URLImagen;
                libroActualizado.AutorId = modelo.AutorId;
                libroActualizado.CategoriaId = modelo.CategoriaId;

                await _servicioLibros.ActualizarLibro(libroActualizado);
                return Json(new { success = true, message = "Libro editado exitosamente." });
            }

            return Json(new { success = false, message = "Error en los datos del libro." });
        }

        // Acción para eliminar un libro
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var libro = await _servicioLibros.ObtenerLibroPorId(id);
            if (libro == null)
            {
                return Json(new { success = false, message = "Libro no encontrado." });
            }

            await _servicioLibros.EliminarLibro(id);
            return Json(new { success = true, message = "Libro eliminado exitosamente." });
        }
    }
}
