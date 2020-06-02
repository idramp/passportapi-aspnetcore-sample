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
    public class ConnectionProofModel : PageModel
    {
        private readonly ProofApiService _passportService;

        public ConnectionProofModel(
            ProofApiService passportService)
        {
            _passportService = passportService;
        }

        public PassportApi.ProofRequestModel Proof { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string connectionId = User.GetConnectionId();
            if (connectionId == null)
                return RedirectToPage("./Connect", new { returnUrl = Url.Page("./ConnectionProof") });

            Proof = await _passportService.GetEmailProof(connectionId);
            return Page();
        }

        public async Task<IActionResult> OnGetProofStatusAsync(string proofId)
        {
            PassportApi.ProofState state = await _passportService.GetProofState(proofId);

            if (state == PassportApi.ProofState.Accepted)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
