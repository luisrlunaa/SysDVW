using SysDVW.Models.Entities;
using System.Collections.Generic;

namespace SysDVW.ViewModels
{
    public class PrintViewModel
    {
        public List<DetalleVenta> DetailSalesList { get; set; }
        public Venta Sales { get; set; }
        public bool ReImpresion { get; set; }
        public string DocClient { get; set; }
    }
}
