using System;
using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Pagos
    {
        [Key]
        public int id_pago { get; set; }
        [Required]
        public int id_caja { get; set; }
        [Required]
        public decimal monto { get; set; }
        public decimal ingresos { get; set; }
        public decimal egresos { get; set; }
        public DateTime fecha { get; set; }

        public int idVenta { get; set; }
        public Pagos()
        {
        }
    }
}
