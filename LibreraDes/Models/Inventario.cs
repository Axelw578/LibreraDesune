using System.ComponentModel.DataAnnotations.Schema;

namespace LibreraDes.Models
{
    [Table("Inventario")]  // Especifica el nombre exacto de la tabla en la base de datos
    public class Inventario
    {
        public int Id { get; set; }
        public int LibroId { get; set; }

        // Relación con la clase Libro
        public Libro Libro { get; set; }

        public int CantidadDisponible { get; set; }
    }
}
