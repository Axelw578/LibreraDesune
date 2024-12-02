namespace LibreraDes.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }

        // Relacion: Un autor puede tener muchos libros
        public List<Libro> Libros { get; set; }
    }

}
