using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Tipo_factura
    {
        [Key]
        public int id { get; set; }
        public string Descripcion { get; set; }

        public Tipo_factura()
        {
        }
    }
}
