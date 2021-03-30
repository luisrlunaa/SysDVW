using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class ncf
    {
        [Key]
        public int id_ncf { get; set; }
        public string descripcion_ncf { get; set; }
        public string prefijo { get; set; }
        public int secuenciaIni { get; set; }
        public int secuenciaF { get; set; }

        public ncf()
        {
        }
    }
}
