using System.Collections.Generic;
using LibreraDes.Models;

namespace LibreraDes.ViewModels
{
    public class LibroUsuarioViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string AutorNombre { get; set; }
        public string CategoriaNombre { get; set; }
        public decimal Precio { get; set; }
        public string URLImagen { get; set; }
        public string Sinopsis { get; set; }
    }

    public class LibrosUsuarioViewModel
    {
        public List<LibroUsuarioViewModel> Libros { get; set; } = new List<LibroUsuarioViewModel>();
    }
}
