using System;
using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Comprobantes
    {
        [Key]
        public int id_comprobante { get; set; }
        public int secuenciai { get; set; }
        public int secuenciaf { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_final { get; set; }
        public bool Activo { get; set; }

        public Comprobantes()
        {
        }
    }
}
