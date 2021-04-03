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
    public class EditProductModel : PageModel
    {
        public ProductViewModel ProductView { get; set; }
        public Categoria Category { get; set; }
        public Producto Product { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditProductModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            ProductView = new ProductViewModel();
            Category = new Categoria();
            Product = new Producto();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet(int ID)
        {
            ProductView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            if (ProductView != null && ID > 0)
            {
                Product = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
                if (Product is null)
                {
                    foreach (var item in ProductView.ProductList)
                    {
                        if (item.IdProducto == ID)
                        {
                            Product = item;
                            _httpContextAccessor.HttpContext.Session.Set<Producto>(ConstactSession.Product, Product);
                            return Page();
                        }
                    }
                }
                return Page();
            }
            else
            {
                _httpContextAccessor.HttpContext.Session.Set<string>(ConstactSession.MessageBetweenPages, "Debe seleccionar al menos un producto antes de editarlo");
                return RedirectToPage("/Product/product-list");
            }
        }

        //seleccionar categoria
        public IActionResult OnGetSelectCategorytype(int id)
        {
            Product = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
            var categoria = _context.categorias.FirstOrDefault(x => x.IdCategoria == id);
            Product.Categoria = categoria.Descripcion;
            Product.IdCategoria = id;
            _httpContextAccessor.HttpContext.Session.Set<Producto>(ConstactSession.Product, Product);
            return OnGet(Product.IdProducto);
        }

        //agregar categoria
        public IActionResult OnPostAddCategory(Categoria Category)
        {
            ProductView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            Category.IdCategoria = ProductView.categoryList.Count() + 1;

            if (Category.Descripcion != null && Category.IdCategoria >= 1)
            {
                var repeat = false;
                foreach (var category in ProductView.categoryList)
                {
                    if (category.Descripcion.ToUpper() == Category.Descripcion.ToUpper())
                    {
                        repeat = true;
                    }
                }

                if (repeat == false)
                {
                    Category.Descripcion = Category.Descripcion.ToUpper();
                    var save = _context.categorias.Add(Category);
                    if (save != null)
                    {
                        ProductView.categoryList.Add(Category);
                        _httpContextAccessor.HttpContext.Session.Set<ProductViewModel>(ConstactSession.ProductList, ProductView);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("CustomError", "La Categoria ya Exite");
                }
            }

            return OnGet(Product.IdProducto);
        }

        //Eliminar categoria
        public IActionResult OnGetDeleteCategory(int id)
        {
            ProductView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
            if (id >= 1)
            {
                var category = ProductView.categoryList.FirstOrDefault(x => x.IdCategoria == id);
                var save = _context.categorias.Remove(category);
                if (save != null)
                {
                    ProductView.categoryList.Remove(category);
                    _httpContextAccessor.HttpContext.Session.Set<ProductViewModel>(ConstactSession.ProductList, ProductView);
                    _context.SaveChanges();
                }
            }

            return OnGet(Product.IdProducto);
        }
        //seleccionar tipo de producto
        public IActionResult OnGetSelecttype(string id)
        {
            Product = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
            Product.TipoGoma = id;
            _httpContextAccessor.HttpContext.Session.Set<Producto>(ConstactSession.Product, Product);
            return OnGet(Product.IdProducto);
        }

        //Edicion del Producto
        public IActionResult OnPostEditProduct(Producto Product)
        {
            var ProductoEdit = _httpContextAccessor.HttpContext.Session.Get<Producto>(ConstactSession.Product);
            if (ProductoEdit.IdCategoria > 0)
            {
                Product.IdCategoria = ProductoEdit.IdCategoria;
            }

            if (ProductoEdit.TipoGoma != null)
            {
                Product.TipoGoma = ProductoEdit.TipoGoma;
            }

            if (Product.PrecioCompra > 0 && Product.Nombre != null)
            {
                var save = _context.Productos.Update(Product);
                if (save != null)
                {
                    ProductView = _httpContextAccessor.HttpContext.Session.Get<ProductViewModel>(ConstactSession.ProductList);
                    if (ProductView.ProductList != null)
                    {
                        foreach (var item in ProductView.ProductList)
                        {
                            if (item.IdProducto == Product.IdProducto)
                            {
                                Product.FechaModificacion = DateTime.Today;
                                ProductView.ProductList.Remove(item);
                                ProductView.ProductList.Add(Product);
                                _httpContextAccessor.HttpContext.Session.Set<ProductViewModel>(ConstactSession.ProductList, ProductView);
                                HttpContext.Session.Remove("Product");
                                _context.SaveChanges();
                                return RedirectToPage("/Products/product-list");
                            }
                        }
                    }
                }
            }

            return RedirectToPage("/Products/product-list");
        }
    }
}
