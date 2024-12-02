namespace LibreraDes.ViewModels
{
    public class LibroViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public decimal Precio { get; set; }
        public string URLImagen { get; set; }
        public int AutorId { get; set; } // Para asociar el libro con un autor sin modificar la entidad Autor
        public int CategoriaId { get; set; } // Para asociar el libro con una categoría sin modificar la entidad Categoria
    }
}
