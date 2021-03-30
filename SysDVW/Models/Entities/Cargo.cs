using System.ComponentModel.DataAnnotations;

namespace SysDVW.Models.Entities
{
    public class Cargo
    {
        [Key]
        public int IdCargo { get; set; }
        public string Descripcion { get; set; }

        public Cargo()
        {
        }
    }
}
