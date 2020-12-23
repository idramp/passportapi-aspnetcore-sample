using System.Threading.Tasks;
using AspNetDemo.Models;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PassportApi;


namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = AuthConstants.CookieScheme)]
    public class DisplayModel : PageModel
    {
        private readonly ProofApiService _passportService;

        public DisplayModel(ProofApiService passportService)
        {
            _passportService = passportService;
        }

        public ProofStateModel Proof { get; private set; }

        public async Task OnGetAsync(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Proof = await _passportService.GetProof(id);
            }
        }
    }
}
