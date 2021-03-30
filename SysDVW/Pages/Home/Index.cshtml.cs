using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System;
using System.Linq;

namespace SysDVW.Pages.Home
{
    public class IndexModel : PageModel
    {
        public HomeViewModel homeViewModel { get; set; }
        public ProductViewModel ProductListView { get; set; }
        public SalesListViewModel SalesListView { get; set; }

        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IndexModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            ProductListView = new ProductViewModel();
            SalesListView = new SalesListViewModel();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
            homeViewModel = _httpContextAccessor.HttpContext.Session.Get<HomeViewModel>(ConstactSession.HomeData);
            if (homeViewModel is null)
            {
                homeViewModel = new HomeViewModel();
                products();
                sales();

                var listproduct = ProductListView.ProductList.OrderByDescending(x => x.FechaModificacion).Take(4).ToList();
                if (listproduct != null)
                {
                    homeViewModel.productlist = listproduct;
                }

                var listsales = SalesListView.SalesList.OrderByDescending(x => x.FechaVenta).Take(5).ToList();
                if (listsales != null)
                {
                    homeViewModel.Saletlist = listsales;
                }

                _httpContextAccessor.HttpContext.Session.Set<HomeViewModel>(ConstactSession.HomeData, homeViewModel);
            }
        }

        public void DataProduct()
        {
            var listcategory = _context.categorias.ToList();
            if (listcategory.Count() > 0)
            {
                ProductListView.categoryList = listcategory;
            }

            var listtypeyanta = _context.tipoGOma.ToList();
            if (listtypeyanta.Count() > 0)
            {
                ProductListView.typeyantaList = listtypeyanta;
            }

            var listtypejobs = _context.Tipo_trabajo.ToList();
            if (listtypejobs.Count() > 0)
            {
                ProductListView.typejobList = listtypejobs;
            }
        }
        public void products()
        {
            var productlist = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            if (productlist == null)
            {
                var listproduct = _context.Productos.Where(p => p.IdProducto > 0).ToList();
                DataProduct();
                if (listproduct.Count() > 0)
                {
                    foreach (var item in listproduct)
                    {
                        foreach (var category in ProductListView.categoryList)
                        {
                            if (item.IdCategoria == category.IdCategoria)
                            {
                                item.Categoria = category.Descripcion;
                            }

                            if (item.Stock >= 0 && item.Stock <= 5)
                            {
                                item.classbg = "bg-danger";
                            }
                            if (item.Stock > 5 && item.Stock <= 15)
                            {
                                item.classbg = "bg-warning";
                            }
                            if (item.Stock > 15 && item.Stock <= 20)
                            {
                                item.classbg = "bg-primary";
                            }
                            if (item.Stock > 20)
                            {
                                item.classbg = "bg-success";
                            }
                        }
                        ProductListView.allmoneysgain = ProductListView.allmoneysgain + (item.PrecioVenta - item.PrecioCompra);
                    }

                    ProductListView.ProductList = listproduct;
                }

                _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
                _httpContextAccessor.HttpContext.Session.Set<ProductViewModel>(ConstactSession.ProductList, ProductListView);
            }
            else
            {
                ProductListView = productlist;
            }

            homeViewModel.ProductCount = ProductListView.ProductList.Last().IdProducto;
            homeViewModel.Productgain = ProductListView.allmoneysgain;
        }

        public void sales()
        {
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

            homeViewModel.salescount = SalesListView.SalesList.Last().IdVenta;

            decimal gain = 0;
            foreach (var sale in SalesListView.SalesList)
            {
                var product = new Producto();
                if (sale.FechaVenta.Month == DateTime.Today.Month)
                {
                    var detalis = _context.DetalleVentas.Where(x => x.IdVenta == sale.IdVenta).ToList();
                    foreach (var saledetail in detalis)
                    {
                        product = ProductListView.ProductList.FirstOrDefault(x => x.IdProducto == saledetail.IdProducto);
                        if (product != null)
                        {
                            if (saledetail.IdProducto == product.IdProducto)
                            {
                                gain = gain + (saledetail.SubTotal - product.PrecioCompra);
                            }
                        }
                    }
                }
            }

            homeViewModel.salesgain = gain;
        }
    }
}
