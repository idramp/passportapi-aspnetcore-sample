using System.Threading.Tasks;
using AspNetDemo.Models;
using AspNetDemo.Services;
using IdRamp.Passport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = AuthConstants.CookieScheme)]
    public class ConnectionlessOfferModel : PageModel
    {
        private readonly CredentialApiService _credService;

        public CredentialOfferModel Credential;


        public ConnectionlessOfferModel(CredentialApiService credService)
        {
            _credService = credService;
        }

        public async Task OnGet()
        {
            Credential = await _credService.IssueEmailCredentialAsync(connectionId: null, "connectionless@demo.org");
        }

        public async Task<IActionResult> OnGetCredentialStatusAsync(string credentialId)
        {
            CredentialState state = await _credService.GetCredentialState(credentialId);

            if (state == CredentialState.Issued || state == CredentialState.Rejected)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
