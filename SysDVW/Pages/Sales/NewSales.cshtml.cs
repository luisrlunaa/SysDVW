using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rotativa.AspNetCore;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Services.Print;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System;
using System.Linq;

namespace SysDVW.Pages.Sales
{
    public class NewSalesModel : PageModel
    {
        [BindProperty]
        public Producto Product { get; set; }
        [BindProperty(SupportsGet = true)]
        public SalesViewModel SalesView { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPrintFuntions _printFuntions;

        public NewSalesModel(InvSysContext context, IHttpContextAccessor httpContextAccessor, IPrintFuntions printFuntions)
        {
            Product = new Producto();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _printFuntions = printFuntions;
        }

        public IActionResult OnGet()
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            if (SalesView is null)
            {
                SalesView = new SalesViewModel();
                SalesView.TypePrint = "Pequena";
                SalesView.pkPrint = true;
                SalesView.Pay.id_caja = _context.Cajas.OrderByDescending(x => x.id_caja).FirstOrDefault().id_caja + 1;
                SalesView.Disable = "disabled";
                SalesView.paybutton = "disabled";
                SalesView.Sales.IdVenta = _context.Ventas.OrderByDescending(x => x.IdVenta).FirstOrDefault().IdVenta + 1;

                var UserLogger = _httpContextAccessor.HttpContext.Session.Get<UsuarioTable>(ConstactSession.userLogger);
                if (UserLogger != null)
                {
                    SalesView.Sales.IdEmpleado = UserLogger.IdEmpleado;
                }

                SalesView.Sales.FechaVenta = DateTime.Now;

                var clientList = _context.Clientes.Where(x => x.DNI != null).ToList();
                if (clientList.Count() > 0)
                {
                    SalesView.ClientList = clientList;
                }

                var productList = _context.Productos.ToList();
                if (productList.Count() > 0)
                {
                    SalesView.ProductList = productList;
                }

                var listNCF = _context.Ncfs.Where(x => x.secuenciaIni > 0 && x.secuenciaIni < x.secuenciaF).ToList();
                var listComprobante = _context.Comprobante.Where(z => z.Activo == true);
                foreach (var item in listNCF)
                {
                    foreach (var item1 in listComprobante)
                    {
                        if (item.id_ncf == item1.id_comprobante)
                        {
                            SalesView.ncfList.Add(item);
                        }
                    }
                }

                SalesView.typeticketList = _context.Tipo_Facturas.ToList();
            }

            SalesView.subtotal = 0;
            SalesView.total = 0;
            SalesView.ITBIS = 0;
            SalesView.Sales.Total = 0;

            if (SalesView.ClientName != null)
            {
                SalesView.Sales.NombreCliente = SalesView.ClientName;
            }

            if (SalesView.IdClient > 0)
            {
                SalesView.Sales.IdCliente = SalesView.IdClient;
            }

            if (SalesView.DetailSalesList.Count() > 0)
            {
                foreach (var item in SalesView.DetailSalesList)
                {
                    SalesView.subtotal += item.PrecioUnitario;
                    SalesView.total += item.SubTotal;
                    SalesView.ITBIS += item.Igv;
                }
                SalesView.Sales.Total = SalesView.total;
            }

            _httpContextAccessor.HttpContext.Session.Set(ConstactSession.Sales, SalesView);
            return Page();
        }

        //seleccionar comprobante
        public IActionResult OnGetSelectncf(int nfc)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            var ncf = _context.Ncfs.FirstOrDefault(x => x.id_ncf == nfc);
            SalesView.Sales.NroDocumento = ncf.prefijo + ncf.secuenciaIni.ToString("00000000");
            SalesView.Sales.Serie = ncf.id_ncf;
            SalesView.Sales.TipoDocumento = ncf.descripcion_ncf;
            _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
            return OnGet();
        }

        //seleccionar metodo de impresion
        public IActionResult OnGetSelecttypePrint(string print)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            SalesView.TypePrint = print;
            _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);

            return OnGet();
        }

        //seleccionar tipo de factura
        public IActionResult OnGetSelecttypeticket(int id)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            var factura = _context.Tipo_Facturas.FirstOrDefault(x => x.id == id);
            SalesView.Sales.TipoFactura = factura.Descripcion;
            _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
            return OnGet();
        }

        //seleccionar producto
        public IActionResult OnGetSelectProduct(int ID)
        {
            SalesView.Disable = "";
            var producto = _context.Productos.FirstOrDefault(x => x.IdProducto == ID);
            if (producto != null)
            {
                SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
                SalesView.Product = producto;
                SalesView.Disable = "enabled";
                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
            }

            return OnGet();
        }

        //Editar producto
        public IActionResult OnGetEditProduct(int id, int cant, decimal prec, decimal igv)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            var _product = _context.Productos.FirstOrDefault(x => x.IdProducto == id);
            _product.cantidad = cant;
            _product.PrecioVenta = prec;
            _product.itbis = igv;
            SalesView.Product = _product;

            foreach (var item in SalesView.DetailSalesList)
            {
                if (item.IdProducto == _product.IdProducto)
                {
                    SalesView.DetailSalesList.Remove(item);
                    _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                    return OnGet();
                }
            }
            return OnGet();
        }

        //Eliminar producto
        public IActionResult OnGetDeleteProduct(int ID)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            foreach (var item in SalesView.DetailSalesList)
            {
                if (item.IdProducto == ID)
                {
                    SalesView.DetailSalesList.Remove(item);
                    _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                    return OnGet();
                }
            }
            return OnGet();
        }

        //seleccionar cliente
        public IActionResult OnGetAddClient(string ID = null, string nombre = null)
        {
            if (SalesView.ClientName is null)
            {
                SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
                var client = _context.Clientes.FirstOrDefault(x => x.DNI == ID);
                if (client != null)
                {
                    SalesView.ClientName = client.Nombres + " " + client.Apellidos;
                    SalesView.DocClient = client.DNI;
                    SalesView.IdClient = client.IdCliente;

                }
                else
                {
                    SalesView.DocClient = ID;
                    SalesView.ClientName = nombre;
                    SalesView.clientwithoutdoc = true;
                }
                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
            }
            return OnGet();
        }

        //Agregar producto al detalle de venta
        public IActionResult OnPostAddProducttoDetails(Producto product)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            if (product.cantidad == 0)
            {
                ModelState.AddModelError("CustomError", "El campo Cantidad es obligatorio");
                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                return OnGet();
            }

            if (product.cantidad > product.Stock)
            {
                ModelState.AddModelError("CustomError", "Ha excedido la cantidad existente de este Producto");
                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                return OnGet();
            }

            var producto = new Producto();
            var detalle = new DetalleVenta();
            SalesView.Disable = "disabled";
            SalesView.paybutton = "";
            SalesView.Product = producto;
            if (SalesView.DetailSalesList.Count() == 0)
            {
                detalle.IdVenta = SalesView.Sales.IdVenta;
                detalle.IdDetalleVenta = _context.DetalleVentas.OrderByDescending(x => x.IdDetalleVenta).FirstOrDefault().IdDetalleVenta + 1;
                detalle.IdProducto = product.IdProducto;
                detalle.PrecioUnitario = product.PrecioVenta;
                detalle.detalles_P = product.Nombre;
                detalle.Cantidad += product.cantidad;
                detalle.Igv = product.itbis;
                detalle.SubTotal = detalle.Cantidad * (detalle.PrecioUnitario + detalle.Igv);
                SalesView.DetailSalesList.Add(detalle);

                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                return OnGet();
            }
            else
            {
                detalle.IdVenta = SalesView.Sales.IdVenta;
                detalle.IdDetalleVenta = detalle.IdDetalleVenta + 1;
                detalle.IdProducto = product.IdProducto;
                detalle.PrecioUnitario = product.PrecioVenta;
                detalle.Cantidad = product.cantidad;
                detalle.detalles_P = product.Nombre;
                detalle.Igv = product.itbis;
                detalle.SubTotal = detalle.Cantidad * (detalle.PrecioUnitario + detalle.Igv);
                SalesView.DetailSalesList.Add(detalle);

                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                return OnGet();
            }
        }


        //Realizar venta
        public IActionResult OnPostSaler(SalesViewModel salesView)
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.Sales);
            if (SalesView.DetailSalesList.Count() == 0)
            {
                ModelState.AddModelError("CustomError", "Debe seleccionar al menos un producto para realizar una venta");
                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                return OnGet();
            }
            salesView.Pay.ingresos = salesView.Pay.ingresos - salesView.Pay.egresos;

            ///Pago
            SalesView.Pay.id_caja = SalesView.Pay.id_caja;
            SalesView.Pay.id_pago = _context.Pago.OrderByDescending(x => x.id_pago).FirstOrDefault().id_pago + 1;
            SalesView.Pay.ingresos = salesView.Pay.ingresos;
            SalesView.Pay.egresos = salesView.Pay.egresos;
            SalesView.Pay.monto = SalesView.Sales.Total;
            SalesView.Pay.fecha = SalesView.Sales.FechaVenta;
            SalesView.Pay.idVenta = SalesView.Sales.IdVenta;

            if (SalesView.Sales.TipoFactura.ToLower() != "credito" && SalesView.Pay.ingresos != SalesView.Sales.Total)
            {
                ModelState.AddModelError("CustomError", "Las Facturas Tipo Debito No Aceptan Deudas");
                _httpContextAccessor.HttpContext.Session.Set<SalesViewModel>(ConstactSession.Sales, SalesView);
                return OnGet();
            }

            if (SalesView.Sales.NroDocumento is null)
            {
                SalesView.Sales.NroDocumento = "Sin NCF";
            }

            if (SalesView.Sales.TipoDocumento is null)
            {
                SalesView.Sales.TipoDocumento = "Ningún Tipo de Comprobante";
            }

            ///Ventas
            var venta = new Venta();
            venta.IdVenta = SalesView.Sales.IdVenta;
            venta.IdCliente = SalesView.Sales.IdCliente;
            venta.Serie = SalesView.Sales.Serie;
            venta.NroDocumento = SalesView.Sales.NroDocumento;
            venta.TipoDocumento = SalesView.Sales.TipoDocumento;
            venta.FechaVenta = SalesView.Sales.FechaVenta;
            venta.Total = SalesView.Sales.Total;
            venta.IdEmpleado = SalesView.Sales.IdEmpleado;
            venta.NombreCliente = SalesView.Sales.NombreCliente;
            venta.TipoFactura = SalesView.Sales.TipoFactura;
            if (venta.TipoFactura.ToLower() == "credito")
            {
                if (SalesView.Pay.ingresos > 0)
                {
                    venta.Restante = venta.Total - SalesView.Pay.ingresos;
                }
                else
                {
                    venta.Restante = venta.Total;
                }
            }

            var savedd = _context.Ventas.Add(venta);
            if (savedd != null)
            {
                _context.SaveChanges();

                ///DetalleVentas
                foreach (var itemdetails in SalesView.DetailSalesList)
                {
                    var _product = _context.Productos.FirstOrDefault(x => x.IdProducto == itemdetails.IdProducto);
                    if (_product != null)
                    {
                        _product.cantidad = _product.Stock - itemdetails.Cantidad;
                        var _updateproduct = _context.Productos.Update(_product);
                        if (_updateproduct != null)
                        {
                            _context.SaveChanges();
                        }
                    }

                    var detalle = new DetalleVenta();
                    detalle.IdVenta = SalesView.Sales.IdVenta;
                    detalle.IdDetalleVenta = itemdetails.IdDetalleVenta;
                    detalle.IdProducto = itemdetails.IdProducto;
                    detalle.PrecioUnitario = itemdetails.PrecioUnitario;
                    detalle.Cantidad = itemdetails.Cantidad;
                    detalle.detalles_P = itemdetails.detalles_P;
                    detalle.Igv = itemdetails.Igv;
                    detalle.SubTotal = itemdetails.SubTotal;

                    var saved = _context.DetalleVentas.Add(detalle);
                    if (saved != null)
                    {
                        _context.SaveChanges();
                    }
                }

                ///Actualizando comprobantes
                var ncf = _context.Ncfs.FirstOrDefault(x => x.id_ncf == SalesView.Sales.Serie);
                if (ncf != null)
                {
                    ncf.secuenciaIni = ncf.secuenciaIni + 1;
                    var _updatencf = _context.Ncfs.Update(ncf);
                    if (_updatencf != null)
                    {
                        var Comprobante = _context.Comprobante.FirstOrDefault(x => x.id_comprobante == SalesView.Sales.Serie);
                        Comprobante.secuenciai = Comprobante.secuenciai + 1;
                        if (Comprobante.secuenciai == Comprobante.secuenciaf || Comprobante.fecha_final <= DateTime.Today)
                        {
                            Comprobante.Activo = false;
                        }
                        var _updateComp = _context.Comprobante.Update(Comprobante);
                        if (_updateComp != null)
                        {
                            var ncfGenerados = new NCFGenerados();
                            ncfGenerados.id_secuencia = SalesView.Sales.Serie;
                            ncfGenerados.secuenciaNCF = SalesView.Sales.NroDocumento;
                            ncfGenerados.fecha = SalesView.Sales.FechaVenta;

                            var save = _context.nCFGenerados.Add(ncfGenerados);
                            if (save != null)
                            {
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }

            ///Realizando pago
            var savepay = _context.Pago.Add(SalesView.Pay);
            if (savepay != null)
            {
                _context.SaveChanges();
            }

            //llenando modelo de impresion
            var PrintView = new PrintViewModel();
            PrintView.Sales = SalesView.Sales;
            PrintView.DetailSalesList = SalesView.DetailSalesList;
            _httpContextAccessor.HttpContext.Session.Set<PrintViewModel>(ConstactSession.Print, PrintView);

            //Seleccionando metodo de Impresion
            if (SalesView.TypePrint == "Pequena" && SalesView.pkPrint)
            {
                _printFuntions.tickFuntion();
                OnGetClean("");
            }
            else
            {
                OnGetClean("");
            }

            return RedirectToPage("/Sales/sales-list");
        }

        public IActionResult GeneratePDF()
        {
            var PrintView = new PrintViewModel();
            PrintView.Sales = SalesView.Sales;
            PrintView.DetailSalesList = SalesView.DetailSalesList;
            _httpContextAccessor.HttpContext.Session.Set<PrintViewModel>(ConstactSession.Print, PrintView);

            return new ViewAsPdf("GeneratePDF", PrintView)
            {
                FileName = PrintView.Sales.TipoDocumento + PrintView.Sales.FechaVenta,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        //limpiar ventana
        public IActionResult OnGetClean(string id)
        {
            if (id == "cleaning")
            {
                HttpContext.Session.Remove("Sales");
            }
            else
            {
                HttpContext.Session.Remove("Sales");
                HttpContext.Session.Remove("HomeData");
                HttpContext.Session.Remove("SalesDetail");
                HttpContext.Session.Remove("SalesList");
                HttpContext.Session.Remove("Print");
            }
            return OnGet();
        }
    }
}
