using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SysDVW.Pages.Security
{
    public class registerModel : PageModel
    {
        public User user { get; set; }
        private readonly ISecurityCodeGeneratorService _securityCodeGeneratorService;
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public registerModel(ISecurityCodeGeneratorService securityCodeGeneratorService, InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            user = new User();
            _securityCodeGeneratorService = securityCodeGeneratorService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostRegister(User user)
        {
            if (ModelState.IsValid)
            {
                var _Employee = _context.Empleados.FirstOrDefault(x => x.Correo == user.Empleados.Correo && x.Dni == user.Empleados.Dni);
                if (_Employee != null)
                {
                    var userT = _context.UserTable.FirstOrDefault(x => x.IdEmpleado == _Employee.IdEmpleado && x.Usuario == _Employee.Correo);
                    if (userT == null)
                    {
                        var newUser = new User();
                        newUser.Usuario.Usuario = user.Empleados.Correo;
                        newUser.Usuario.Contraseña = user.Usuario.Contraseña;
                        newUser.Usuario.IdEmpleado = user.Empleados.IdEmpleado;
                        newUser.Empleados = _Employee;

                        _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.EmployeeRegistering, newUser);
                        var securityCodeResult = await _securityCodeGeneratorService.GenerateAndSent(_Employee.Correo);
                        if (securityCodeResult.IsFailure)
                        {
                            ModelState.AddModelError("CustomError", securityCodeResult.Error);
                        }
                        return RedirectToPage("/Security/ValidationCode");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "El usuario ya existe");
                    }
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Credenciales incorrectas");
                }
            }
            return Page();
        }
    }
}
