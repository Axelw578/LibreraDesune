using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace LibreraDes.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }  // Relación con Usuario

        [DataMember]
        [DataType(DataType.DateTime)]

        public DateTime FechaVenta { get; set; }
        public decimal Total { get; set; }

        // Relación: Una venta puede tener muchos detalles
        public List<DetallesVentas> DetallesVentas { get; set; }
    }
}
