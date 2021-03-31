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

namespace SysDVW.Pages.Clients
{
    public class EditClientsModel : PageModel
    {
        public User NewUser { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EditClientsModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            NewUser = new User();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet(int ID)
        {
            var Clients = _httpContextAccessor.HttpContext.Session.Get<User>(ConstactSession.User);
            if (Clients is null||Clients.Clientes.IdCliente ==0)
            {
                var Clientlist = _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
                if (Clientlist != null && ID>0)
                {
                    foreach (var item in Clientlist)
                    {
                        if (item.IdCliente == ID)
                        {
                            var user = _context.UserTable.FirstOrDefault(x => x.IdCliente == item.IdCliente);
                            if (user != null)
                            {
                                user.Contraseña = "";
                                NewUser.Usuario = user;
                            }
                            NewUser.Clientes = item;
                            _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
                        }
                    }
                    return Page();
                }
                else
                {
                    _httpContextAccessor.HttpContext.Session.Set<string>(ConstactSession.MessageBetweenPages, "Debe seleccionar al menos un Cliente antes de editarlo");
                    return RedirectToPage("/Clients/clients-list");
                }
            }
            else
            {
                NewUser.Clientes = Clients.Clientes;
                return Page();
            }
        }

        //Edicion del Cliente
        public IActionResult OnPostEditClient(User NewUser)
        {
            if (NewUser.Clientes.Apellidos != null && NewUser.Clientes.Nombres != null)
            {
                var save = _context.Clientes.Update(NewUser.Clientes);
                if (save != null)
                {
                    var Clientlist = _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
                    if (Clientlist != null)
                    {
                        foreach (var item in Clientlist)
                        {
                            if (item.IdCliente == NewUser.Clientes.IdCliente)
                            {
                                item.Nombres = NewUser.Clientes.Nombres;
                                item.Apellidos = NewUser.Clientes.Apellidos;
                                item.Telefono = NewUser.Clientes.Telefono;
                                item.Direccion = NewUser.Clientes.Direccion;

                                Clientlist.Remove(item);
                                Clientlist.Add(NewUser.Clientes);
                                _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ClientList, Clientlist);

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
                                                    _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ClientList, Clientlist);
                                                    return RedirectToPage("/Clients/clients-list");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("CustomError", "Las contraseñas no coinciden, favor volver a intentar");
                                        return RedirectToPage("/Clients/clients-list");
                                    }
                                }

                                _context.SaveChanges();
                                NewUser.Clientes = new Cliente();
                                _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
                                _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ClientList, Clientlist);
                                return RedirectToPage("/Clients/clients-list");
                            }
                        }
                    }
                }
            }
            return RedirectToPage("/Clients/clients-list");
        }
    }
}
