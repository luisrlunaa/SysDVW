using System.ComponentModel.DataAnnotations;

namespace SysDVW.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remenberme { get; set; }
    }
    public class checkingUserViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        public string UserName { get; set; }
    }
    public class RecoverViewModel
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
    public class ValidateViewModel
    {
        [RegularExpression("^[0-9]{6}?$")]
        [Required(ErrorMessage = "El campo es requerido")]
        public string codigo { get; set; }
    }
}
