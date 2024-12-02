namespace LibreraDes.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        // Relación con Autor
        public int AutorId { get; set; }
        public Autor Autor { get; set; }

        // Relación con Categoría
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public decimal Precio { get; set; }
        public string URLImagen { get; set; }

        // Campo recién agregado para la sinopsis
        public string Sinopsis { get; set; }

        // Relaciones con Inventario, Carrito y DetallesVentas
        public List<Inventario> Inventarios { get; set; }
        public List<Carrito> Carritos { get; set; }
        public List<DetallesVentas> DetallesVentas { get; set; }
    }
}
