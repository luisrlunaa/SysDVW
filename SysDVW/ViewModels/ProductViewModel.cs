using SysDVW.Models.Entities;
using System.Collections.Generic;

namespace SysDVW.ViewModels
{
    public class ProductViewModel
    {
        public Producto Product { get; set; }
        public Categoria Category { get; set; }
        public List<Producto> ProductList { get; set; }
        public List<Categoria> categoryList { get; set; }
        public List<tipoGOma> typeyantaList { get; set; }
        public List<Tipo_trabajo> typejobList { get; set; }
        public decimal allmoneysgain { get; set; }
        public int idcategory { get; set; }
        public int idtypeofproduct { get; set; }

        public ProductViewModel()
        {
            categoryList = new List<Categoria>();
            Product = new Producto();
            Category = new Categoria();
            ProductList = new List<Producto>();
            typejobList = new List<Tipo_trabajo>();
            typeyantaList = new List<tipoGOma>();
        }
    }
}
