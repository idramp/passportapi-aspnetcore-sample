using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassportApi;

namespace AspNetDemo.Pages.Passport
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
    public class BasicMessageModel : PageModel
    {
        private readonly Services.BasicMessageApiService _passportService;

        public BasicMessageModel(
            BasicMessageApiService passportService)
        {
            _passportService = passportService;
        }

        public ICollection<BasicMessageDetail> Messages { get; private set; }
        [BindProperty]
        public string BasicMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string connectionId = User.GetConnectionId();
            if (connectionId == null)
                return RedirectToPage("./Connect", new { returnUrl = Url.Page("./BasicMessage") });

            try
            {
                // A HTTP 204 response is causing an exception to be thrown in the generated swagger client.
                Messages = await _passportService.GetMessages(connectionId);
            }
            catch { }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            string connectionId = User.GetConnectionId();
            if (connectionId == null)
                return RedirectToPage("./Connect", new { returnUrl = Url.Page("./BasicMessage") });

            await _passportService.SendMessage(connectionId, BasicMessage);
            return RedirectToPage("./BasicMessage");
        }
    }
}
