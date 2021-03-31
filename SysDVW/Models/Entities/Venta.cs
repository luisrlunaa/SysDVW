using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysDVW.Models.Entities
{
    public class Venta
    {
        [Key]
        public int IdVenta { get; set; }
        public int? IdCliente { get; set; }
        public int Serie { get; set; }
        public string NroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaVenta { get; set; }
        public decimal Total { get; set; }
        [Required]
        public int IdEmpleado { get; set; }
        public decimal Restante { get; set; }
        public string TipoFactura { get; set; }
        public string NombreCliente { get; set; }
        public bool borrado { get; set; } = false;
        public Venta()
        {
        }
    }
}
