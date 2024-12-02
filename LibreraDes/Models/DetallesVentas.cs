namespace LibreraDes.Models
{
    public class DetallesVentas
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public Venta Venta { get; set; }

        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }


    }
}