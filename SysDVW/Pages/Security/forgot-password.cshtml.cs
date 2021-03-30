using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Services.Email;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SysDVW.Pages.Security
{
    public class forgot_passwordModel : PageModel
    {
        public string Empresa { get; set; }
        [BindProperty]
        public checkingUserViewModel Check { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;

        public forgot_passwordModel(InvSysContext context, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostForgotPassword(checkingUserViewModel check)
        {
            if (ModelState.IsValid)
            {
                if (check.UserName == null)
                {
                    ModelState.AddModelError("CustomError", "Favor Ingresar un Usuario para iniciar la busqueda");
                    return Page();
                }
                else
                {
                    var _currentUser = _context.UserTable.FirstOrDefault(x => x.Usuario == check.UserName);
                    if (_currentUser != null)
                    {
                        string _token = Guid.NewGuid().ToString();
                        var empleado = _context.Empleados.FirstOrDefault(x => x.IdEmpleado == _currentUser.IdEmpleado);
                        if (empleado.Correo == null)
                        {
                            ModelState.AddModelError("CustomError", "Correo no Encontrado");
                            return Page();
                        }
                        empleado.token = _token;
                        _httpContextAccessor.HttpContext.Session.Set<Empleado>(ConstactSession.EmployeeRegistering, empleado);
                        var _callback = Url.PageLink("recover-password", "Recover", new RecoverViewModel { Token = _token, Email = empleado.Correo });
                        var message = new Message(new string[] { empleado.Correo }, "Restablecer la clave de la contraseña", _callback, null);
                        await _emailSender.SendEmailAsync(message);
                        return RedirectToPage("/Security/ForgotPasswordConfirmation");
                    }
                }
            }
            return Page();
        }
    }
}
