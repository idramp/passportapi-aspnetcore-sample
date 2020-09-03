using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetDemo.Pages.Passport.Revocable
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
    public class RevokableOfferModel : PageModel
    {
        private readonly RevocableCredentialApiService _passportService;

        public RevokableOfferModel(RevocableCredentialApiService passportService)
        {
            _passportService = passportService;
        }

        public PassportApi.CredentialOfferModel Offer { get; set; }

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
            PassportApi.CredentialState state = await _passportService.GetCredentialState(id);

            if (state == PassportApi.CredentialState.Issued ||
                state == PassportApi.CredentialState.Rejected)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
