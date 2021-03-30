using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System;
using System.Linq;

namespace SysDVW.Pages.Products
{
    public class NewproductModel : PageModel
    {
        public ProductViewModel ProductView { get; set; }
        public Producto Product { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewproductModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            Product = new Producto();
            ProductView = new ProductViewModel();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            var productView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            if (productView != null)
            {
                ProductView = productView;
            }

            var product = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
            if (product != null)
            {
                Product = product;
            }

            return Page();
        }

        //seleccionar categoria
        public IActionResult OnGetSelectCategorytype(int id)
        {
            var categoria = _context.categorias.FirstOrDefault(x => x.IdCategoria == id);
            Product.Categoria = categoria.Descripcion;
            Product.IdCategoria = id;
            _httpContextAccessor.HttpContext.Session.Set<Producto>(ConstactSession.Product, Product);
            return OnGet();
        }

        //seleccionar tipo de producto
        public IActionResult OnGetSelecttype(string id)
        {
            Product = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
            Product.TipoGoma = id;
            _httpContextAccessor.HttpContext.Session.Set<Producto>(ConstactSession.Product, Product);
            return OnGet();
        }

        //Creacion del Producto
        public IActionResult OnPostNewProduct(Producto Product)
        {
            var ProductNew = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
            if (ProductNew.IdCategoria > 0)
            {
                Product.IdCategoria = ProductNew.IdCategoria;
            }

            if (ProductNew.TipoGoma != null)
            {
                Product.TipoGoma = ProductNew.TipoGoma;
            }

            if (Product.PrecioCompra > 0 && Product.Nombre != null)
            {
                ProductView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
                if (ProductView.ProductList != null)
                {
                    Product.FechaCreacion = DateTime.Today;
                    Product.IdProducto = ProductView.ProductList.OrderByDescending(x => x.IdProducto).FirstOrDefault().IdProducto + 1;
                    var save = _context.Productos.Add(Product);
                    if (save != null)
                    {
                        ProductView.ProductList.Add(Product);
                        _httpContextAccessor.HttpContext.Session.Set<ProductViewModel>(ConstactSession.ProductList, ProductView);

                        HttpContext.Session.Remove("Product");
                        _context.SaveChanges();
                    }
                }

            }
            return RedirectToPage("/Products/product-list");
        }
    }
}
