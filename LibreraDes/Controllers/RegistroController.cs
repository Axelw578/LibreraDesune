using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LibreraDes.Services;
using LibreraDes.Models;
using LibreraDes.ViewModels;

namespace LibreraDes.Controllers
{
    public class RegistroController : Controller
    {
        private readonly RegistroService _registroService;

        public RegistroController(RegistroService registroService)
        {
            _registroService = registroService;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            // Inicializa el modelo para la vista
            return View(new UsuarioRegistroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegistroViewModel model)
        {
            // Verifica si el modelo es válido
            if (!ModelState.IsValid)
            {
                return View(model); // Retorna el modelo con errores
            }

            // Asigna la URL de la imagen de perfil o una imagen por defecto
            var urlFotoPerfil = string.IsNullOrEmpty(model.URLFotoPerfil) ? "/img/default-user.png" : model.URLFotoPerfil;

            // Crea un nuevo usuario a partir del modelo
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = model.NombreUsuario,
                Correo = model.Correo,
                Clave = model.Clave,
                URLFotoPerfil = urlFotoPerfil
            };

            try
            {
                // Intenta registrar el nuevo usuario
                var resultado = await _registroService.RegistrarUsuario(nuevoUsuario);

                // Verifica el resultado del registro
                if (resultado.Contains("registrado con éxito"))
                {
                    TempData["RegistroExitoso"] = resultado;
                    return RedirectToAction("Login", "Cuenta"); // Redirige al controlador de Login
                }

                // Agrega un mensaje de error específico del servicio al modelo
                ModelState.AddModelError("", resultado);
            }
            catch (Exception ex)
            {
                // Manejo de excepción: añade el mensaje de error al modelo
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
            }

            // Retorna la vista con el modelo en caso de error
            return View(model);
        }
    }
}
