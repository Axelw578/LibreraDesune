namespace LibreraDes.Models
{
    public class Bitacora
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }  // Relación con Usuario

        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
