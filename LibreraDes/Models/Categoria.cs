﻿namespace LibreraDes.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Relación: Una categoría puede tener muchos libros
        public List<Libro> Libros { get; set; }
    }
}
