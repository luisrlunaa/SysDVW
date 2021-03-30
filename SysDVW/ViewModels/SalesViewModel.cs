using SysDVW.Models.Entities;
using System.Collections.Generic;

namespace SysDVW.ViewModels
{
    public class SalesViewModel
    {
        public List<Tipo_factura> typeticketList { get; set; }
        public List<ncf> ncfList { get; set; }
        public List<Cliente> ClientList { get; set; }
        public List<Producto> ProductList { get; set; }
        public List<DetalleVenta> DetailSalesList { get; set; }
        public Venta Sales { get; set; }
        public Pagos Pay { get; set; }
        public Producto Product { get; set; }
        public string ClientName { get; set; }
        public string DocClient { get; set; }
        public int IdClient { get; set; }
        public decimal total { get; set; }
        public decimal subtotal { get; set; }
        public decimal ITBIS { get; set; }
        public bool clientwithoutdoc { get; set; }
        public string Disable { get; set; }
        public string paybutton { get; set; }
        public string TypePrint { get; set; }
        public bool pkPrint { get; set; }
        public bool gdPrint { get; set; }
        public SalesViewModel()
        {
            Pay = new Pagos();
            Sales = new Venta();
            Product = new Producto();
            ClientList = new List<Cliente>();
            ProductList = new List<Producto>();
            DetailSalesList = new List<DetalleVenta>();
            ncfList = new List<ncf>();
            typeticketList = new List<Tipo_factura>();
        }
    }
}
