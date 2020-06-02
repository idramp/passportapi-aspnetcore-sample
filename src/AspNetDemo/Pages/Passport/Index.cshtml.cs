using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassportApi;

namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
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
            if(connectionId != null)
                Connection = await _passportService.GetConnection(connectionId);
        }
    }
}