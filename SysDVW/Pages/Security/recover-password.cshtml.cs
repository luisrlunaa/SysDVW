using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System.Linq;

namespace SysDVW.Pages.Security
{
    public class recover_passwordModel : PageModel
    {
        [BindProperty]
        public RecoverViewModel Recovering { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public recover_passwordModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostRecover(RecoverViewModel recover)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(recover.ConfirmPassword))
                {
                    ModelState.AddModelError("CustomError", "Favor llenar el campo contraseña");
                    return Page();
                }

                var _userCache = _httpContextAccessor.HttpContext.Session.Get<Empleado>(ConstactSession.EmployeeRegistering);
                if (_userCache.token != null)
                {
                    var user = _context.UserTable.FirstOrDefault(x => x.IdEmpleado == _userCache.IdEmpleado);
                    if (user != null)
                    {
                        user.Contraseña = recover.Password;
                        var save = _context.Update(user);
                        if (save != null)
                        {
                            _context.SaveChanges();
                            return RedirectToPage("/Security/Recover_success");
                        }
                    }
                    return Page();
                }
                return Page();
            }
            return RedirectToPage("/Security/ResetPasswordConfirmation");
        }
    }
}
