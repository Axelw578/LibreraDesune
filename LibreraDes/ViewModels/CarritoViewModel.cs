namespace LibreraDes.ViewModels
{
    // ViewModel para mostrar información del carrito
    public class CarritoViewModel
    {
        public int LibroId { get; set; }
        public string TituloLibro { get; set; }
        public string URLImagenLibro { get; set; }
        public decimal PrecioLibro { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; } // Precio * Cantidad
    }
}
