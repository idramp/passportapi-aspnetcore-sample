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
    public class ConnectionOfferModel : PageModel
    {
        private readonly CredentialApiService _passportService;

        public ConnectionOfferModel(CredentialApiService passportService)
        {
            _passportService = passportService;
        }

        public CredentialOfferModel Offer { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string connectionId = User.GetConnectionId();
            if (connectionId == null)
                return RedirectToPage("./Connect", new { returnUrl = Url.Page("./ConnectionOffer") });

            Offer = await _passportService.IssueEmailCredentialAsync(connectionId, "demo@example.org");
            return Page();
        }

        public async Task<IActionResult> OnGetStatusAsync(string credentialId)
        {
            CredentialState state = await _passportService.GetCredentialState(credentialId);

            if (state == CredentialState.Issued ||
                state == CredentialState.Rejected)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
