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
    public class NewClientsModel : PageModel
    {
        public User NewUser { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewClientsModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            NewUser = new User();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        //Creacion del Cliente
        public IActionResult OnPostNewClient(User NewUser)
        {
            if (NewUser.Clientes.Nombres !=null && NewUser.Clientes.Apellidos != null)
            {
                var Clientlist = _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
                if (Clientlist != null)
                {
                    NewUser.Clientes.IdCliente = Clientlist.Count();
                }
                else
                {
                    NewUser.Clientes.IdCliente = 1;
                }

                var save = _context.Clientes.Add(NewUser.Clientes);
                if (save != null)
                {
                    Clientlist.Add(NewUser.Clientes);
                    _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ClientList, Clientlist);

                    if (NewUser.changeUser)
                    {
                        if(NewUser.Usuario.Contraseņa== NewUser.Usuario.Contraseņa)
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
                                    NewUser.Usuario = new UsuarioTable();
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Las contraseņas no coinciden, favor volver a intentar");
                            return RedirectToPage("/Clients/clients-list");
                        }
                    }

                    _context.SaveChanges();
                    NewUser.Clientes = new Cliente();
                    _httpContextAccessor.HttpContext.Session.Set<User>(ConstactSession.User, NewUser);
                    return RedirectToPage("/Clients/clients-list");
                }
            }
            return RedirectToPage("/Clients/clients-list");
        }
    }
}
