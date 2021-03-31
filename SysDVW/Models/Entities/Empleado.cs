using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysDVW.Models.Entities
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }
        [Required]
        public int IdCargo { get; set; }
        [Required]
        public string Dni { get; set; }
        [NotMapped]
        public string Correo { get; set; }
        public string Apellidos { get; set; }
        [Required]
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNac { get; set; }
        public char EstadoCivil { get; set; }
        [NotMapped]
        public int IdEmpresa { get; set; }
        [NotMapped]
        public string Cargo { get; set; }
        [NotMapped]
        public string token { get; set; }
        public Empleado()
        {
        }
    }
}
