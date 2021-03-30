using SysDVW.Models.Entities;
using System.Collections.Generic;

namespace SysDVW.ViewModels
{
    public class SalesListViewModel
    {
        public List<Venta> SalesList { get; set; }
        public decimal allmoneysales { get; set; }
        public decimal allmoneysalestoday { get; set; }
        public decimal allmoneysgain { get; set; }
        public bool ReImpresion { get; set; }
    }
}
