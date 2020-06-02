using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
    public class ConnectionlessProofModel : PageModel
    {
        private readonly ProofApiService _passportService;

        public ConnectionlessProofModel(
            ProofApiService passportService)
        {
            _passportService = passportService;
        }

        [BindProperty(SupportsGet = true)]
        public string TestId { get; set; }

        public PassportApi.ProofRequestModel Proof { get; set; }

        public async Task OnGet()
        {
            Proof = await _passportService.GetEmailProof();
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
