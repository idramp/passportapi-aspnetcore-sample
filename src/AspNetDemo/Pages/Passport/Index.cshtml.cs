using System.Threading.Tasks;
using AspNetDemo.Models;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassportApi;


namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = AuthConstants.CookieScheme)]
    public class IndexModel : PageModel
    {
        private readonly ConnectionApiService _passportService;

        public IndexModel(
            ConnectionApiService passportService)
        {
            _passportService = passportService;
        }

        public ConnectionStateModel Connection { get; private set; }

        public async Task OnGet()
        {
            string connectionId = User.GetConnectionId();
            if (connectionId != null)
                Connection = await _passportService.GetConnection(connectionId);
        }
    }
}
