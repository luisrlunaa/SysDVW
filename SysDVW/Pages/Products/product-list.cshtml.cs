using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System.Linq;

namespace SysDVW.Pages.Products
{
    public class product_listModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public ProductViewModel ProductListView { get; set; }
        public string MessageBetweenPages { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public product_listModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            var message = _httpContextAccessor.HttpContext.Session.Get<string>(ConstactSession.MessageBetweenPages);
            if (message != null)
            {
                MessageBetweenPages = message;
                ModelState.AddModelError("CustomError", MessageBetweenPages);
                HttpContext.Session.Remove("MessageBetweenPages");
            }

            var productlist = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            if (productlist == null)
            {
                var listproduct = _context.Productos.Where(p => p.IdProducto > 0).ToList();
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
                if (productlist.ProductList.Count() > 0)
                {
                    foreach (var item in productlist.ProductList)
                    {
                        foreach (var category in productlist.categoryList)
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
                        productlist.allmoneysgain = ProductListView.allmoneysgain + (item.PrecioVenta - item.PrecioCompra);
                    }
                }

                ProductListView = productlist;
            }
            return Page();
        }

        //Eliminar el Producto
        public IActionResult OnGetDeleteproduct(int ID)
        {
            ProductListView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            if (ProductListView != null && ID > 0)
            {
                foreach (var item in ProductListView.ProductList)
                {
                    if (item.IdProducto == ID)
                    {
                        var save = _context.Productos.Remove(item);
                        if (save != null)
                        {
                            _context.SaveChanges();
                        }

                        ProductListView.ProductList.Remove(item);
                        _httpContextAccessor.HttpContext.Session.Set<ProductViewModel>(ConstactSession.ProductList, ProductListView);
                        return OnGet();
                    }
                }
                return OnGet();
            }
            else
            {
                ModelState.AddModelError("CustomError", "Debe seleccionar al menos un producto antes de eliminarlo");
                return OnGet();
            }
        }
    }
}
