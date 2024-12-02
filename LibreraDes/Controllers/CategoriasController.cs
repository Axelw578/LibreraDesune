using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibreraDes.ViewModels;
using LibreraDes.Servicios;
using LibreraDes.Models;

namespace LibreraDes.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ServicioCategoriasAutores _servicioCategoriasAutores;

        public CategoriasController(ServicioCategoriasAutores servicioCategoriasAutores)
        {
            _servicioCategoriasAutores = servicioCategoriasAutores;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            var result = await _servicioCategoriasAutores.ObtenerTodasLasCategorias();
            if (!result.isSuccess)
            {
                ModelState.AddModelError("", result.errorMessage);
                return View(new List<CategoriaViewModel>());
            }

            var viewModel = result.categorias.Select(c => new CategoriaViewModel
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            }).ToList();

            return View(viewModel);
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nuevaCategoria = new Categoria
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion
                };
                var result = await _servicioCategoriasAutores.InsertarCategoria(nuevaCategoria);
                if (result.isSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, errorMessage = result.errorMessage });
            }
            return Json(new { success = false, errorMessage = "Modelo inválido." });
        }

        // POST: Categorias/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] CategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categoriaExistente = await _servicioCategoriasAutores.ObtenerCategoriaPorId(id);
                if (!categoriaExistente.isSuccess)
                {
                    return Json(new { success = false, errorMessage = categoriaExistente.errorMessage });
                }

                var categoria = categoriaExistente.categoria;
                categoria.Nombre = model.Nombre;
                categoria.Descripcion = model.Descripcion;

                var result = await _servicioCategoriasAutores.ModificarCategoria(categoria);
                if (result.isSuccess)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, errorMessage = result.errorMessage });
            }
            return Json(new { success = false, errorMessage = "Modelo inválido." });
        }

        // POST: Categorias/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _servicioCategoriasAutores.EliminarCategoria(id);
            if (result.isSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, errorMessage = result.errorMessage });
        }
    }
}
