using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Models.Entities;
using SysDVW.Utilities;
using SysDVW.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SysDVW.Pages.Clients
{
    public class clients_listtModel : PageModel
    {
        public List<Cliente> listClients { get; set; }
        public string MessageBetweenPages { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public clients_listtModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
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

            var listClientes = _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
            if (listClientes == null)
            {
                listClientes = _context.Clientes.Where(p => p.IdCliente > 0).ToList();


                listClients = listClientes;
              

                _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
                _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ClientList, listClients);
            }
            else
            {
                listClients = listClientes;
            }
            return Page();
        }
        public IActionResult OnGetDeleteproduct(int ID)
        {
            listClients = _httpContextAccessor.HttpContext.Session.Get<List<Cliente>>(ConstactSession.ClientList);
            if (listClients == null)
            {
                foreach (var item in listClients)
                {
                    if (item.IdCliente == ID)
                    {
                        var save = _context.Clientes.Remove(item);
                        if (save != null)
                        {
                            _context.SaveChanges();
                        }

                        listClients.Remove(item);
                        _httpContextAccessor.HttpContext.Session.Set<List<Cliente>>(ConstactSession.ProductList, listClients);
                        return OnGet();
                    }
                }
                return OnGet();
            }
            else
            {
                ModelState.AddModelError("CustomError", "Debe seleccionar al menos un producto antes de eliminarlo");
                return OnGet();
            }
        }
    }
}
