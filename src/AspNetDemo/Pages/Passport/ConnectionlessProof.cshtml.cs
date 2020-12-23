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
    public class ConnectionlessProofModel : PageModel
    {
        private readonly ProofApiService _passportService;

        public ConnectionlessProofModel(
            ProofApiService passportService)
        {
            _passportService = passportService;
        }

        public CreateProofRequestResultModel Proof { get; set; }

        public async Task OnGet()
        {
            Proof = await _passportService.GetEmailProof();
        }

        public async Task<IActionResult> OnGetProofStatusAsync(string proofId)
        {
            ProofState state = await _passportService.GetProofState(proofId);

            if (state == ProofState.Accepted)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }
    }
}
