using System.ComponentModel.DataAnnotations;

namespace LibreraDes.ViewModels
{
    public class UsuarioRegistroViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria.")]
        [Compare("Clave", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarClave { get; set; }

        public string URLFotoPerfil { get; set; }
    }
}
