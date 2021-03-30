using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysDVW.Models.Entities
{
    public class NomEmp
    {
        [Key]
        public int idEmp { get; set; }
        [Required]
        public string NombreEmp { get; set; }
        public string DirEmp { get; set; }
        [Required]
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Correo { get; set; }
        public string RNC { get; set; }
        public DateTime FechaVenc { get; set; }
        public string Licencia_Pre { get; set; }
        public string Licencia_Post { get; set; }
        [NotMapped]
        public string PrintName { get; set; }
        public NomEmp()
        {
        }
    }
}
