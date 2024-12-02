using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LibreraDes.Services;

namespace LibreraDes.Controllers
{
    public class RecuperarCuentaController : Controller
    {
        private readonly RecuperarCuentaService _recuperarCuentaService;

        public RecuperarCuentaController(RecuperarCuentaService recuperarCuentaService)
        {
            _recuperarCuentaService = recuperarCuentaService;
        }

        [HttpGet]
        public IActionResult RecuperarCuenta()
        {
            return View("~/Views/RecuperarCuenta/RecuperarCuenta.cshtml");  // Ruta explícita a la vista
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarCuenta(string correo)
        {
            var resultado = await _recuperarCuentaService.RecuperarCuenta(correo);

            if (resultado.Contains("enviado"))
            {
                TempData["RecuperarExitoso"] = resultado;
                return RedirectToAction("Login", "Login");
            }

            ModelState.AddModelError("", resultado);
            return View("~/Views/RecuperarCuenta/RecuperarCuenta.cshtml");  // Ruta explícita a la vista
        }
    }
}
