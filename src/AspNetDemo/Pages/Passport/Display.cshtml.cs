using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
    public class DisplayModel : PageModel
    {
        private readonly ProofApiService _passportService;

        public DisplayModel(ProofApiService passportService)
        {
            _passportService = passportService;
        }

        public PassportApi.ProofStateModel Proof { get; private set; }

        public async Task OnGetAsync(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Proof = await _passportService.GetProof(id);
            }
        }
    }
}
