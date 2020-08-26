using System.Threading.Tasks;
using AspNetDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetDemo.Pages.Passport
{
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = Models.AuthConstants.CookieScheme)]
    public class ConnectModel : PageModel
    {
        private readonly ConnectionApiService _passportService;

        public ConnectModel(
            ConnectionApiService passportService)
        {
            _passportService = passportService;
        }

        // TODO jmason : This doesn't appear to ever actually be used. It's only displayed in a partial view that doesn't exist, I think.
        [TempData]
        public string Message { get; set; }

        [BindProperty(Name = "returnUrl", SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public PassportApi.ConnectionOfferModel Connection { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated ||
                User.GetConnectionId() == null)
            {
                return await CreateNewConnection();
            }
            else
            {
                PassportApi.ConnectionStateModel connection = await _passportService.GetConnection(User.GetConnectionId());

                if (connection.State == PassportApi.ConnectionState.Connected)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToPage("./ConnectionProof");
                }
                else
                {
                    return await CreateNewConnection();
                }
            }
        }

        private async Task<IActionResult> CreateNewConnection()
        {
            PassportApi.ConnectionOfferModel result = await _passportService.CreateConnection();
            if (result != null)
            {
                Connection = result;
            }
            else
                Message = "Error: Login failed.";
            return Page();
        }

        public async Task<IActionResult> OnGetStatusAsync(string id)
        {
            PassportApi.ConnectionStateModel connection = await _passportService.GetConnection(id);

            if (connection.State == PassportApi.ConnectionState.Connected)
                return new JsonResult(true);
            else
                return new JsonResult(false);
        }

        public async Task<IActionResult> OnGetContinueAsync(string id)
        {
            string connectionId = id;
            PassportApi.ConnectionStateModel connection = await _passportService.GetConnection(connectionId);

            if (connection.State == PassportApi.ConnectionState.Connected)
            {
                await HttpContext.Login(connection.Id);
                return RedirectToPage("./Index"); // TODO jumason : Use ReturnUrl instead if set, meaning we came from a proof/credential page request?
            }
            else
            {
                Message = "Error: The user declined the connection.";
                return Page();
            }
        }
    }
}
