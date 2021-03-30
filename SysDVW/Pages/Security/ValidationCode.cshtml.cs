using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;

namespace SysDVW.Pages.Security
{
    public class ValidationCodeModel : PageModel
    {
        [BindProperty]
        public ValidateViewModel validar { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISecurityCodeGeneratorService _securityCodeGeneratorService;
        private readonly InvSysContext _context;
        public ValidationCodeModel(ISecurityCodeGeneratorService securityCodeGeneratorService, InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _securityCodeGeneratorService = securityCodeGeneratorService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            validar = new ValidateViewModel();
        }

        public IActionResult OnGet()
        {
            var validacion = _httpContextAccessor.HttpContext.Session.Get<string>(ConstactSession.Validationcode);
            if (validacion != null)
            {
                var EmployeeRegistring = _httpContextAccessor.HttpContext.Session.Get<Empleado>(ConstactSession.EmployeeRegistering);
                if (EmployeeRegistring != null)
                {
                    return Page();
                }
            }
            _httpContextAccessor.HttpContext.Session.Set<string>(ConstactSession.Validationcode, validacion);
            return RedirectToPage("/Security/register");
        }
        public IActionResult OnPostValidar(ValidateViewModel validar)
        {
            if (ModelState.IsValid)
            {
                var EmployeeRegistring = _httpContextAccessor.HttpContext.Session.Get<User>(ConstactSession.EmployeeRegistering);
                if (EmployeeRegistring != null)
                {
                    var validacion = _httpContextAccessor.HttpContext.Session.Get<string>(ConstactSession.Validationcode);
                    if (validar != null && validar.codigo == validacion)
                    {
                        var saveUser = _context.UserTable.Add(EmployeeRegistring.Usuario);
                        if (saveUser != null)
                        {
                            _context.SaveChanges();
                            return RedirectToPage("/Security/RegisterSuccess");
                        }
                    }
                }
                return Page();
            }
            return Page();
        }
    }
}
