using Microsoft.AspNetCore.Mvc;
using LibreraDes.Models;
using LibreraDes.Servicios;
using LibreraDes.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace LibreraDes.Controllers
{
    public class AutoresController : Controller
    {
        private readonly ServicioCategoriasAutores _servicioCategoriasAutores;

        public AutoresController(ServicioCategoriasAutores servicioCategoriasAutores)
        {
            _servicioCategoriasAutores = servicioCategoriasAutores;
        }

        // Acción para listar todos los autores
        public async Task<IActionResult> Index()
        {
            var (isSuccess, autores, errorMessage) = await _servicioCategoriasAutores.ObtenerTodosLosAutores();

            if (!isSuccess)
            {
                ViewBag.ErrorMessage = errorMessage;
                return View("Error");
            }

            var autorViewModels = autores.Select(autor => new AutorViewModel
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                FechaNacimiento = autor.FechaNacimiento
            }).ToList();

            return View(autorViewModels);
        }

        // Acción para mostrar la vista de creación de un nuevo autor
        public IActionResult Create()
        {
            return View();
        }

        // Acción para crear un nuevo autor (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var nuevoAutor = new Autor
                {
                    Nombre = viewModel.Nombre,
                    FechaNacimiento = viewModel.FechaNacimiento
                };

                var (isSuccess, errorMessage) = await _servicioCategoriasAutores.InsertarAutor(nuevoAutor);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Autor creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(viewModel);
        }

        // Acción para mostrar la vista de edición de un autor específico
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("El ID del autor no fue proporcionado.");
            }

            var (isSuccess, autor, errorMessage) = await _servicioCategoriasAutores.ObtenerAutorPorId(id.Value);

            if (!isSuccess)
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new AutorViewModel
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                FechaNacimiento = autor.FechaNacimiento
            };

            return View(viewModel);
        }

        // Acción para editar un autor existente (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AutorViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound("El ID del autor no coincide.");
            }

            if (ModelState.IsValid)
            {
                var autorActualizado = new Autor
                {
                    Id = viewModel.Id,
                    Nombre = viewModel.Nombre,
                    FechaNacimiento = viewModel.FechaNacimiento
                };

                var (isSuccess, errorMessage) = await _servicioCategoriasAutores.ModificarAutor(autorActualizado);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Autor actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View(viewModel);
        }

        // Acción para confirmar la eliminación de un autor
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("El ID del autor no fue proporcionado.");
            }

            var (isSuccess, autor, errorMessage) = await _servicioCategoriasAutores.ObtenerAutorPorId(id.Value);

            if (!isSuccess)
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new AutorViewModel
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                FechaNacimiento = autor.FechaNacimiento
            };

            return View(viewModel);
        }

        // Acción para eliminar un autor (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (isSuccess, errorMessage) = await _servicioCategoriasAutores.EliminarAutor(id);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Autor eliminado exitosamente.";
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
