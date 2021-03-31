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
    public class EditEmployeesModel : PageModel
    {
        public List<Cargo> listposition { get; set; }
        public User NewUser { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditEmployeesModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            listposition = new List<Cargo>();
            NewUser = new User();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet(int ID)
        {
            var positionlist = _httpContextAccessor.HttpContext.Session.Get<List<Cargo>>(ConstactSession.PositionList);
            if(positionlist is null)
            {
                listposition = _context.Cargos.Where(x => x.Descripcion.ToLower() != "cliente").ToList();
                _httpContextAccessor.HttpContext.Session.Set<List<Cargo>>(ConstactSession.PositionList, positionlist);
            }
            else
            {
                listposition = positionlist;
            }

            var empleado = _httpContextAccessor.HttpContext.Session.Get<User>(ConstactSession.User);
            if (empleado is null)
            {
                var EmployeesList = _httpContextAccessor.HttpContext.Session.Get<List<Empleado>>(ConstactSession.EmployeesList);
                if (EmployeesList != null && ID >0)
                {
                    foreach (var item in EmployeesList)
                    {
                        if (item.IdEmpleado == ID)
                        {
                            foreach(var cargo in listposition)
                            {
                                if(item.IdCargo==cargo.IdCargo)
                                {
                                    item.Cargo = cargo.Descripcion;
                                }
                            }

                            NewUser.Empleados = item;
                            var user = _context.UserTable.FirstOrDefault(x => x.IdEmpleado == item.IdEmpleado);
                            if (user != null)
                            {
                                user.Contraseña = "";
                                NewUser.Usuario = user;
                            }
                            _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
                        }
                    }
                    return Page();
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.Set<string>(ConstactSession.MessageBetweenPages, "Debe seleccionar al menos un Empleado antes de editarlo");
                    return RedirectToPage("/Employees/Employees-list");
                }
            }
            else
            {
                NewUser.Empleados = empleado.Empleados;
                return Page();
            }

        }

        //seleccionar tipo de empleado
        public IActionResult OnGetSelecttype(int id)
        {
            NewUser=_httpContextAccessor.HttpContext.Session.Get<User>(ConstactSession.User);
            var cargo = _context.Cargos.FirstOrDefault(x => x.IdCargo == id);
            NewUser.Empleados.IdCargo = cargo.IdCargo;
            NewUser.Empleados.Cargo = cargo.Descripcion;
            _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
            return OnGet(NewUser.Empleados.IdEmpleado);
        }

        //Edicion del Empleado
        public IActionResult OnPostEditEmployees(User NewUser)
        {
            if (NewUser.Empleados.Apellidos != null && NewUser.Empleados.Nombres != null)
            {
                var save = _context.Empleados.Update(NewUser.Empleados);
                if (save != null)
                {
                    var EmployeesList = _httpContextAccessor.HttpContext.Session.Get<List<Empleado>>(ConstactSession.EmployeesList);
                    if (EmployeesList != null)
                    {
                        foreach (var item in EmployeesList)
                        {
                            if (item.IdEmpleado == NewUser.Empleados.IdEmpleado)
                            {
                                item.Nombres = NewUser.Empleados.Nombres;
                                item.Apellidos = NewUser.Empleados.Apellidos;
                                item.Correo = NewUser.Empleados.Correo;
                                item.Direccion = NewUser.Empleados.Direccion;
                                item.EstadoCivil = NewUser.Empleados.EstadoCivil;

                                EmployeesList.Remove(item);
                                EmployeesList.Add(NewUser.Empleados);
                                _httpContextAccessor.HttpContext.Session.Set<List<Empleado>>(ConstactSession.EmployeesList, EmployeesList);

                                if (NewUser.changeUser)
                                {
                                    var Userlist = _httpContextAccessor.HttpContext.Session.Get<List<UsuarioTable>>(ConstactSession.UserList);
                                    if (NewUser.Usuario.ConfirmPassword == NewUser.Usuario.Contraseña)
                                    {
                                        foreach (var item1 in Userlist)
                                        {
                                            if (item1.Usuario == NewUser.Usuario.Usuario && item1.IdCliente == NewUser.Clientes.IdCliente)
                                            {
                                                Userlist.Remove(item1);
                                                Userlist.Add(NewUser.Usuario);

                                                var saved = _context.UserTable.Update(NewUser.Usuario);
                                                if (saved != null)
                                                {
                                                    _context.SaveChanges();
                                                    NewUser.Usuario = new UsuarioTable();
                                                    _httpContextAccessor.HttpContext.Session.Set<List<Empleado>>(ConstactSession.EmployeesList, EmployeesList);
                                                    return RedirectToPage("/Employees/Employees-list");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("CustomError", "Las contraseñas no coinciden, favor volver a intentar");
                                        return RedirectToPage("/Employees/Employees-list");
                                    }
                                }

                                NewUser.Empleados = new Empleado();
                                _context.SaveChanges();
                                _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
                                _httpContextAccessor.HttpContext.Session.Set<List<Empleado>>(ConstactSession.EmployeesList, EmployeesList);
                                return RedirectToPage("/Employees/Employees-list");
                            }
                        }
                    }
                }
            }
            return RedirectToPage("/Employees/Employees-list");
        }
    }
}
