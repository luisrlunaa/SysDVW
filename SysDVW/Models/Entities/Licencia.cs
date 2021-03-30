using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Licencia
    {
        [Key]
        public int id { get; set; }
        public string empresa { get; set; }
        public int secuenciaini { get; set; }
        public int secuenciacent { get; set; }
        public int secuenciafin { get; set; }
        public string proveedor { get; set; }
        public string palabraclave { get; set; }
        public Licencia()
        {
        }
    }
}
