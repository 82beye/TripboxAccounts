using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Tripbox.Accounts.API.Extensions;
using Tripbox.Accounts.API.Models;
using System.Threading.Tasks;
using Tripbox.Accounts.API.Models.Auth;

namespace Tripbox.Accounts.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        [HttpGet("~/signin")]
        public async Task<IActionResult> SignIn() 
        {
            var schemes = await HttpContext.GetExternalProvidersAsync();
            return View("SignIn", schemes);
        }


        [HttpPost("~/signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SignIn([FromForm] string provider, [FromForm] string company_no)
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(company_no))
            {
                return BadRequest();
            }

            var schemes = await HttpContext.GetExternalProvidersAsync();

            //_logger.LogInformation($"schemes => {JsonConvert.ToString(schemes)}");

            if (schemes == null)
            {
                return BadRequest();
            }

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            //return Challenge(new AuthenticationProperties { RedirectUri = "/" }, provider);
            return Challenge(new AuthenticationProperties { 
                                RedirectUri = string.Format("/callback/{0}", provider.ToLower().ToString()),
                                Items = { { "company_no", company_no } },
                            }, provider);
            //return Challenge(new AuthenticationProperties { }, provider);
        }


        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1")]
        public IActionResult SignOutCurrentUser()
        {
            // Instruct the cookies middleware to delete the local cookie created`
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }


        [HttpGet("~/callback/{provider}")]
        [HttpPost("~/callback/{provider}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1")]
        //public async Task<IActionResult> CallBack(string provider, string code)
        public async Task<IActionResult> CallBack()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            Auth authModel = new Auth();

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                foreach (var claim in HttpContext.User.Claims)
                {
                    authModel.AuAuthenticationType = !string.IsNullOrEmpty(claim.Subject.AuthenticationType) ? claim.Subject.AuthenticationType : "";
                    authModel.NameIdentifier = claim.Type.IndexOf("/nameidentifier") > -1 ? claim.Value : authModel.NameIdentifier;
                    authModel.Name = claim.Type.IndexOf("/name") > -1 ? claim.Value : authModel.Name;
                    authModel.Email = claim.Type.IndexOf("/email") > -1 ? claim.Value : authModel.Email;
                }
            }

            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var company_no = authResult.Properties.Items["company_no"].ToString();

            if (authModel == null)
            {
                return Unauthorized();
            }
            else
            {
                return Json(JsonConvert.SerializeObject(authModel));
            }
        }

    }
}