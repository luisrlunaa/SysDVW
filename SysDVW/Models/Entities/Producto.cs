using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysDVW.Models.Entities
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        [Required]
        public int Stock { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal? Pmax { get; set; }
        public decimal? Pmin { get; set; }
        public string TipoGoma { get; set; }
        [NotMapped]
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal itbis { get; set; }
        public DateTime FechaModificacion { get; set; }
        [NotMapped]
        public int cantidad { get; set; }
        [NotMapped]
        public string classbg { get; set; }
        [NotMapped]
        public string Categoria { get; set; }

        public Producto()
        {
        }
    }
}
