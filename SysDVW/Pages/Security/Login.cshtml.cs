using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.Utilities.Funtionals;
using SysDVW.ViewModels;
using System.Linq;

namespace SysDVW.Pages.Security
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel Login { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnGet(string ReturnUrl)
        {
           ViewData.Add(nameof(ReturnUrl), string.IsNullOrWhiteSpace(ReturnUrl) ? Url.Page("/Home/Index") : ReturnUrl);
        }
        public IActionResult OnPostInicio(LoginViewModel login, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var userloging = _context.UserTable.FirstOrDefault(x => x.Usuario == login.UserName && x.Contraseña == login.Password);
                if (userloging != null)
                {
                    if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        return RedirectToPage(ReturnUrl);
                    _httpContextAccessor.HttpContext.Session.Set<UsuarioTable>(ConstactSession.userLogger, userloging);

                    var Empleado = _context.Empleados.FirstOrDefault(x => x.IdEmpleado == userloging.IdEmpleado);
                    if (Empleado != null)
                    {
                        Global.UserName = Empleado.Nombres + " " + Empleado.Apellidos;
                        _httpContextAccessor.HttpContext.Session.Set<Empleado>(ConstactSession.Employee, Empleado);
                    }

                    var Cliente = _context.Clientes.FirstOrDefault(x => x.IdCliente == userloging.IdCliente);
                    if (Cliente != null)
                    {
                        Global.UserName = Cliente.Nombres + " " + Cliente.Apellidos;
                        _httpContextAccessor.HttpContext.Session.Set<Cliente>(ConstactSession.Clients, Cliente);
                    }

                    var empresa = _context.NomEmps.FirstOrDefault(x => x.idEmp == Empleado.IdEmpresa);
                    if (empresa != null)
                    {
                        _httpContextAccessor.HttpContext.Session.Set<NomEmp>(ConstactSession.DataBussiness, empresa);
                    }

                    return RedirectToPage("/Home/Index");
                }
            }
            ModelState.AddModelError("CustomError", "Usuario No Existente, Favor revisar credenciales y volver a intentar.");
            return Page();
        }

        public IActionResult OnGetLogout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            // await _signInManager.SignOutAsync();
            return RedirectToPage("/Security/Login");
        }
    }
}
