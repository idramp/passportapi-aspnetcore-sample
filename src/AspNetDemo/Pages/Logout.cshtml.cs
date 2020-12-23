using System.Threading.Tasks;
using AspNetDemo.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AspNetDemo.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            await HttpContext.Logout(AuthConstants.CookieScheme);
        }
    }
}
