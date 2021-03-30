using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        [Required]
        public string DNI { get; set; }
        public string Apellidos { get; set; }
        [Required]
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int estado { get; set; }

        public Cliente()
        {
        }
    }
}
