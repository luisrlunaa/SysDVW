using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;

namespace SysDVW.Pages.Employees
{
    public class NewEmployeesModel : PageModel
    {
        public List<Cargo> listposition { get; set; }
        public User NewUser { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewEmployeesModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            listposition = new List<Cargo>();
            NewUser = new User();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            var positionlist = _httpContextAccessor.HttpContext.Session.Get<List<Cargo>>(ConstactSession.PositionList);
            if (positionlist is null)
            {
                listposition = _context.Cargos.Where(x => x.Descripcion.ToLower() != "cliente").ToList();
                _httpContextAccessor.HttpContext.Session.Set<List<Cargo>>(ConstactSession.PositionList, positionlist);
            }
            else
            {
                listposition = positionlist;
            }

            var empleado = _httpContextAccessor.HttpContext.Session.Get<User>(ConstactSession.User);
            if(empleado !=null)
            {
                NewUser.Empleados = empleado.Empleados;
            }

            return Page();
        }

        //seleccionar tipo de empleado
        public IActionResult OnGetSelecttype(int id)
        {
            var cargo = _context.Cargos.FirstOrDefault(x => x.IdCargo == id);
            NewUser.Empleados.IdCargo = cargo.IdCargo;
            NewUser.Empleados.Cargo = cargo.Descripcion;
            _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
            return OnGet();
        }

        //Creacion del Cliente
        public IActionResult OnPostNewEmployees(User NewUser)
        {
            if (NewUser.Empleados.Nombres !=null && NewUser.Empleados.Apellidos != null)
            {
                var EmployeesList = _httpContextAccessor.HttpContext.Session.Get<List<Empleado>>(ConstactSession.EmployeesList);
                if (EmployeesList != null)
                {
                    NewUser.Empleados.IdEmpleado = EmployeesList.Count();
                }
                else
                {
                    NewUser.Empleados.IdEmpleado = 1;
                }

                var save = _context.Empleados.Add(NewUser.Empleados);
                if (save != null)
                {
                    NewUser.Empleados = new Empleado();
                    EmployeesList.Add(NewUser.Empleados);
                    _httpContextAccessor.HttpContext.Session.Set<List<Empleado>>(ConstactSession.EmployeesList, EmployeesList);

                    if (NewUser.changeUser)
                    {
                        if (NewUser.Usuario.ConfirmPassword == NewUser.Usuario.Contraseña)
                        {
                            var Userlist = _httpContextAccessor.HttpContext.Session.Get<List<UsuarioTable>>(ConstactSession.UserList);
                            if (Userlist != null)
                            {
                                NewUser.Usuario.IdUsuario = Userlist.Count();
                            }
                            else
                            {
                                NewUser.Usuario.IdUsuario = 1;
                            }

                            var usercreation = false;
                            foreach (var username in Userlist)
                            {
                                if (NewUser.Usuario.Usuario != username.Usuario)
                                {
                                    usercreation = true;
                                }
                                else
                                {
                                    usercreation = false;
                                }
                            }

                            if (usercreation)
                            {
                                NewUser.Usuario.IdCliente = NewUser.Clientes.IdCliente;
                                Userlist.Add(NewUser.Usuario);
                                var saved = _context.UserTable.Add(NewUser.Usuario);
                                if (saved != null)
                                {
                                    _context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Las contraseñas no coinciden, favor volver a intentar");
                            return RedirectToPage("/Employees/Employees-list");
                        }
                    }

                    _context.SaveChanges();
                    NewUser.Empleados = new Empleado();
                    _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
                    return RedirectToPage("/Employees/Employees-list");
                }
            }
            return RedirectToPage("/Employees/Employees-list");
        }
    }
}
