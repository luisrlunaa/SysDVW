using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }

        public Categoria()
        {
        }
    }
}
