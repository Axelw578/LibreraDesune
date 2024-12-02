using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LibreraDes.Services;
using LibreraDes.ViewModels;

namespace LibreraDes.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // Muestra la vista de login
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        // Procesa el login del usuario
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Login/Login.cshtml", model);
            }

            var usuario = await _loginService.LoginUsuario(model.Correo, model.Clave);

            if (usuario != null)
            {
                // Crear claims del usuario, incluyendo UsuarioId y la URL de la imagen de perfil
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),  // Claim para el UsuarioId
                    new Claim("URLFotoPerfil", usuario.URLFotoPerfil ?? "/img/default-user.png")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Mantener la sesión activa
                };

                // Inicia la sesión con las claims del usuario
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            // Si hay error, lo agrega al estado del modelo y vuelve a mostrar la vista de login
            ModelState.AddModelError("", "Credenciales incorrectas. Intente de nuevo.");
            return View("~/Views/Login/Login.cshtml", model);
        }

        // Cierra sesión del usuario
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
