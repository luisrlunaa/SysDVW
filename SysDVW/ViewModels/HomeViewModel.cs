using SysDVW.Models.Entities;
using System.Collections.Generic;


namespace SysDVW.ViewModels
{
    public class HomeViewModel
    {
        public List<Producto> productlist { get; set; }
        public List<Venta> Saletlist { get; set; }
        public int salescount { get; set; }
        public int ProductCount { get; set; }
        public decimal Productgain { get; set; }
        public decimal salesgain { get; set; }

        public HomeViewModel()
        {
            productlist = new List<Producto>();
            Saletlist = new List<Venta>();
        }
    }
}
