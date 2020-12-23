using System.Threading.Tasks;
using AspNetDemo.Models;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassportApi;


namespace AspNetDemo.Pages.Passport.Revocable
{
    [Authorize(AuthenticationSchemes = AuthConstants.CookieScheme)]
    public class RevocableOfferModel : PageModel
    {
        private readonly RevocableCredentialApiService _passportService;

        public RevocableOfferModel(RevocableCredentialApiService passportService)
        {
            _passportService = passportService;
        }

        public CredentialOfferModel Offer { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string connectionId = User.GetConnectionId();
            if (connectionId == null)
                return RedirectToPage("./Connect", new { returnUrl = Url.Page("./RevocableOffer") });

            Offer = await _passportService.IssueRoleCredential(connectionId, "guest");
            return Page();
        }

        public async Task<IActionResult> OnGetStatusAsync(string id)
        {
            CredentialState state = await _passportService.GetCredentialState(id);

            if (state == CredentialState.Issued || state == CredentialState.Rejected)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
