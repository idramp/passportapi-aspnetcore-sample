using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetDemo.Pages.Passport
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
    public class ConnectionOfferModel : PageModel
    {
        private readonly CredentialApiService _passportService;

        public ConnectionOfferModel(CredentialApiService passportService)
        {
            _passportService = passportService;
        }

        public PassportApi.CredentialOfferModel Offer { get; set; }

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
            PassportApi.CredentialState state = await _passportService.GetCredentialState(credentialId);

            if (state == PassportApi.CredentialState.Issued ||
                state == PassportApi.CredentialState.Rejected)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
