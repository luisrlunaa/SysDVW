using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class DetalleVenta
    {
        [Key]
        public int IdDetalleVenta { get; set; }
        [Required]
        public int IdProducto { get; set; }
        [Required]
        public int IdVenta { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Igv { get; set; }
        public decimal SubTotal { get; set; }
        public string detalles_P { get; set; }

        public DetalleVenta()
        {
        }
    }
}
