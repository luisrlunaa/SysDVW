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
        public Cliente Client { get; set; }
        public UsuarioTable User { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewClientsModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            User = new UsuarioTable();
            Client = new Cliente();
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult OnGet()
        {
            var Clients = _httpContextAccessor.HttpContext.Session.Get<Cliente>(ConstactSession.Client);
            if (Clients != null)
            {
                Client = Clients;
            }
            return Page();
        }

        //Creacion del Cliente
        public IActionResult OnPostNewClient(Cliente Client)
        {
            if (Client.Nombres !=null && Client.Apellidos != null)
            {
                var Clientlist = _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
                if (Clientlist != null)
                {
                    Client.IdCliente = Clientlist.Count();
                }
                else
                {
                    Client.IdCliente = 1;
                }

                var save = _context.Clientes.Add(Client);
                if (save != null)
                {
                    Clientlist.Add(Client);
                    _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ClientList, Clientlist);

                    HttpContext.Session.Remove("Client");
                    _context.SaveChanges();
                }
            }
            return RedirectToPage("/Clients/clients-list");
        }
    }
}
