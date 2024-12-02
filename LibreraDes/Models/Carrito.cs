using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreraDes.Models
{
    [Table("Carrito")]
    public class Carrito
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }  // Relación con Usuario

        [Required]
        public int LibroId { get; set; }
        public Libro Libro { get; set; }  // Relación con Libro

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        [Required]
        public DateTime FechaAgregado { get; set; } = DateTime.Now;  // Valor predeterminado


    }
}
