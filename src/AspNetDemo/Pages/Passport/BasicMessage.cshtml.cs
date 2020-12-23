using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetDemo.Models;
using AspNetDemo.Services;
using IdRamp.Passport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AspNetDemo.Pages.Passport
{
    [Authorize(AuthenticationSchemes = AuthConstants.CookieScheme)]
    public class BasicMessageModel : PageModel
    {
        private readonly BasicMessageApiService _passportService;

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
                // An HTTP 204 NoContent response can cause an exception if there are no messages waiting.
                Messages = await _passportService.GetMessages(connectionId);
            }
            catch 
            {
            }

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
