using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SysDVW.Pages.Employees
{
    public class Employees_listModel : PageModel
    {
        public List<Empleado> listEmployees { get; set; }
        public List<Cargo> listPosition { get; set; }
        public string MessageBetweenPages { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Employees_listModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            listPosition = new List<Cargo>();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            var message = _httpContextAccessor.HttpContext.Session.Get<string>(ConstactSession.MessageBetweenPages);
            if (message != null)
            {
                MessageBetweenPages = message;
                ModelState.AddModelError("CustomError", MessageBetweenPages);
                HttpContext.Session.Remove("MessageBetweenPages");
            }

            var EmployeesList = _httpContextAccessor.HttpContext.Session.Get<List<Empleado>>(ConstactSession.EmployeesList);
            if (EmployeesList == null)
            {
                listPosition = _context.Cargos.Where(x => x.Descripcion.ToLower() != "cliente").ToList();
                EmployeesList = _context.Empleados.Where(p => p.IdEmpleado > 0).ToList();
                foreach (var employees in EmployeesList)
                {
                    foreach (var position in listPosition)
                    {
                        if (employees.IdCargo == position.IdCargo)
                        {
                            employees.Cargo = position.Descripcion;
                        }
                    }
                }
                listEmployees = EmployeesList;

                _httpContextAccessor.HttpContext.Session.Get<List<Empleado>>(ConstactSession.EmployeesList);
                _httpContextAccessor.HttpContext.Session.Set<List<Empleado>>(ConstactSession.EmployeesList, listEmployees);
            }
            else
            {
                listEmployees = EmployeesList;
            }

            return Page();
        }
        public IActionResult OnGetDeleteEmployees(int ID)
        {
            listEmployees = _httpContextAccessor.HttpContext.Session.Get<List<Empleado>>(ConstactSession.EmployeesList);
            if (listEmployees == null)
            {
                foreach (var item in listEmployees)
                {
                    if (item.IdEmpleado == ID)
                    {
                        var save = _context.Empleados.Remove(item);
                        if (save != null)
                        {
                            _context.SaveChanges();
                        }

                        listEmployees.Remove(item);
                       _httpContextAccessor.HttpContext.Session.Set<List<Empleado>>(ConstactSession.EmployeesList,listEmployees);
                        return OnGet();
                    }
                }
                return OnGet();
            }
            else
            {
                ModelState.AddModelError("CustomError", "Debe seleccionar al menos un Empleado antes de eliminarlo");
                return OnGet();
            }
        }
    }
}
