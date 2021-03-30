using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SysDVW.Models;
using SysDVW.Utilities;
using SysDVW.ViewModels;

namespace SysDVW.Pages.Sales
{
    public class SalesDetailsModel : PageModel
    {
        public SalesViewModel SalesView { get; set; }
        private readonly InvSysContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalesDetailsModel(InvSysContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            SalesView = _httpContextAccessor.HttpContext.Session.Get<SalesViewModel>(ConstactSession.SalesDetail); ;
            if (SalesView is null)
            {
                return RedirectToPage("/Sales/sales-list");
            }
            return Page();
        }
    }
}
