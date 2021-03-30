using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SysDVW.Models;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System;
using System.Linq;

namespace SysDVW.Pages.Sales
{
    public class sales_listModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public SalesListViewModel SalesListView { get; set; }
        public SalesViewModel SalesView { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public sales_listModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            SalesView = new SalesViewModel();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("SalesDetail");
            var saleslist = _httpContextAccessor.HttpContext.Session.Get<SalesListViewModel>(ConstactSession.SalesList);
            if (saleslist == null)
            {
                var listsales = _context.Ventas.AsNoTracking().ToList();
                var _groupbyResults = listsales.GroupBy(x => x.IdVenta).SelectMany(x => x).ToList();
                var filterresult = _groupbyResults.Where(p => p.IdVenta >= 0 && p.borrado == false).ToList();

                if (filterresult.Count() > 0)
                {
                    SalesListView.SalesList = filterresult;
                    foreach (var item in filterresult)
                    {
                        if (item.IdCliente != null || item.IdCliente > 0)
                        {
                            var cliente = _context.Clientes.FirstOrDefault(x => x.IdCliente == item.IdCliente);
                            item.NombreCliente = cliente.Nombres + " " + cliente.Apellidos;
                        }

                        SalesListView.allmoneysales = item.Total + SalesListView.allmoneysales;
                        if (item.FechaVenta == DateTime.Today)
                        {
                            SalesListView.allmoneysalestoday = item.Total + SalesListView.allmoneysalestoday;
                        }
                    }
                    _httpContextAccessor.HttpContext.Session.Get<SalesListViewModel>(ConstactSession.SalesList);
                    _httpContextAccessor.HttpContext.Session.Set<SalesListViewModel>(ConstactSession.SalesList, SalesListView);
                }
            }
            else
            {
                SalesListView = saleslist;
            }
            return Page();
        }

        public IActionResult OnGetDeleteSales(int ID)
        {
            SalesListView = _httpContextAccessor.HttpContext.Session.Get<SalesListViewModel>(ConstactSession.SalesList);
            if (SalesListView != null)
            {
                foreach (var item in SalesListView.SalesList)
                {
                    if (item.IdVenta == ID)
                    {
                        item.borrado = true;
                        var save = _context.Ventas.Update(item);
                        if (save != null)
                        {
                            var pago = _context.Pago.FirstOrDefault(x => x.idVenta == item.IdVenta);
                            var saved = _context.Pago.Remove(pago);
                            if (saved != null)
                            {
                                _context.SaveChanges();

                                var detailsales = _context.DetalleVentas.Where(x => x.IdVenta == item.IdVenta).ToList();
                                if (detailsales != null)
                                {
                                    foreach (var itemdetails in detailsales)
                                    {
                                        var product = _context.Productos.FirstOrDefault(x => x.IdProducto == itemdetails.IdProducto);
                                        if (product != null)
                                        {
                                            product.cantidad = product.cantidad + itemdetails.Cantidad;
                                            var sav = _context.Productos.Update(product);
                                            if (sav != null)
                                            {
                                                _context.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        SalesListView.SalesList.Remove(item);
                        _httpContextAccessor.HttpContext.Session.Set<SalesListViewModel>(ConstactSession.SalesList, SalesListView);
                        return OnGet();
                    }
                }
            }
            return OnGet();
        }

        public IActionResult OnGetReturnSales(int ID)
        {
            SalesListView = _httpContextAccessor.HttpContext.Session.Get<SalesListViewModel>(ConstactSession.SalesList);
            if (SalesListView != null)
            {
                foreach (var item in SalesListView.SalesList)
                {
                    if (item.IdVenta == ID)
                    {
                        item.borrado = true;
                        var save = _context.Ventas.Update(item);
                        if (save != null)
                        {
                            var pago = _context.Pago.FirstOrDefault(x => x.idVenta == item.IdVenta);
                            if (pago != null)
                            {
                                var caja = _context.Cajas.FirstOrDefault(x => x.id_caja == pago.id_caja);
                                if (caja != null)
                                {
                                    if (item.TipoFactura.ToLower() == "debito")
                                    {
                                        caja.montoactual = caja.montoactual - item.Total;
                                        if (caja.fecha != DateTime.Today)
                                        {
                                            var cuadre = _context.Cuadre.FirstOrDefault(x => x.id == pago.id_caja);
                                            if (cuadre != null)
                                            {
                                                cuadre.monto = cuadre.monto - item.Total;
                                                var update = _context.Cuadre.Update(cuadre);
                                                if (update != null)
                                                {
                                                    _context.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        caja.deuda = caja.deuda - item.Total;
                                    }

                                    var up = _context.Cajas.Update(caja);
                                    if (up != null)
                                    {
                                        _context.SaveChanges();
                                    }
                                }

                                var saved = _context.Pago.Remove(pago);
                                if (saved != null)
                                {
                                    _context.SaveChanges();

                                    var detailsales = _context.DetalleVentas.Where(x => x.IdVenta == item.IdVenta).ToList();
                                    if (detailsales != null)
                                    {
                                        foreach (var itemdetails in detailsales)
                                        {
                                            var product = _context.Productos.FirstOrDefault(x => x.IdProducto == itemdetails.IdProducto);
                                            if (product != null)
                                            {
                                                product.cantidad = product.cantidad + itemdetails.Cantidad;
                                                var sav = _context.Productos.Update(product);
                                                if (sav != null)
                                                {
                                                    _context.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        SalesListView.SalesList.Remove(item);
                        _httpContextAccessor.HttpContext.Session.Set<SalesListViewModel>(ConstactSession.SalesList, SalesListView);
                        return OnGet();
                    }
                }
            }
            return OnGet();
        }

        public IActionResult OnGetDetailSales(int ID)
        {
            SalesListView = _httpContextAccessor.HttpContext.Session.Get<SalesListViewModel>(ConstactSession.SalesList);
            if (SalesListView != null)
            {
                foreach (var item in SalesListView.SalesList)
                {
                    if (item.IdVenta == ID)
                    {
                        if (item.IdCliente != null || item.IdCliente > 0)
                        {
                            var cliente = _context.Clientes.FirstOrDefault(x => x.IdCliente == item.IdCliente);
                            item.NombreCliente = cliente.Nombres + " " + cliente.Apellidos;
                            if (cliente.DNI.Trim().Length < 10)
                            {
                                SalesView.DocClient = "Sin Documento de Identidad";
                            }
                            else
                            {
                                SalesView.DocClient = cliente.DNI;
                            }
                        }
                        else
                        {
                            SalesView.DocClient = "Sin Documento de Identidad";
                        }

                        SalesView.Sales = item;

                        var salesdetail = _context.DetalleVentas.Where(x => x.IdVenta == item.IdVenta).ToList();
                        if (salesdetail != null)
                        {
                            SalesView.DetailSalesList = salesdetail;

                            if (SalesView.DetailSalesList.Count() > 0)
                            {
                                foreach (var item1 in SalesView.DetailSalesList)
                                {
                                    SalesView.subtotal += item1.PrecioUnitario;
                                    SalesView.total += item1.SubTotal;
                                    SalesView.ITBIS += item1.Igv;
                                }
                                SalesView.Sales.Total = SalesView.total;
                            }
                        }

                        _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.SalesDetail, SalesView);
                        return RedirectToPage("/Sales/SalesDetails");
                    }
                }
            }
            return OnGet();
        }
    }
}
