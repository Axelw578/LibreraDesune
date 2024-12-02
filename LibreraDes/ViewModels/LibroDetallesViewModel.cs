namespace LibreraDes.ViewModels
{
    public class LibroDetallesViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string AutorNombre { get; set; } // Nombre del Autor
        public string CategoriaNombre { get; set; } // Nombre de la Categoría
        public decimal Precio { get; set; }
        public string URLImagen { get; set; }
        public string DescripcionCategoria { get; set; }
        public DateTime FechaNacimientoAutor { get; set; }
        public int CantidadDisponible { get; set; } // Cantidad en inventario
        public string Sinopsis { get; set; } // Nueva propiedad para la sinopsis del libro
    }
}
